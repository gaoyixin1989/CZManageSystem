using CZManageSystem.Core;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Service.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Admin;
using CZManageSystem.Admin.Models;
using System.Threading.Tasks;
using CZManageSystem.Core.Helpers;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Common.Excel;
using System.Data;
using CZManageSystem.Data;
using Aspose.Cells;
using CZManageSystem.Admin.Base;
using Newtonsoft.Json;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Admin.Controllers.Composite
{
    #region TempModel
    public class tempOGSMData
    {
        public int Id { get; set; }
        public string USR_NBR { get; set; }
        public string PAY_MON { get; set; }
        public decimal PreKwh { get; set; }
        public decimal NowKwh { get; set; }
        public int MF { get; set; }
        public decimal CHG { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Adjustment { get; set; }
        public Nullable<decimal> Money { get; set; }
        public int New_Meter { get; set; }
        public Nullable<System.DateTime> RTime { get; set; }
        public string Payee { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string BankAcount { get; set; }
        public string Bank { get; set; }
        public string Address { get; set; }
        public Nullable<int> PSubPayMonth { get; set; }
        public string Remark { get; set; }
        public string BaseStation { get; set; }
        public string PowerType { get; set; }
    }
    #endregion
    public class OGSMController : BaseController
    {

        IOGSMService _ogsmService = new OGSMService();
        IOGSMMonthService _ogsmmonthService = new OGSMMonthService();
        IOGSMInfoService _ogsminfoService = new OGSMInfoService();
        IOGSMElectricityService _ogsmelectricityService = new OGSMElectricityService();
        IAdmin_AttachmentService _attchmentService = new Admin_AttachmentService();
        IOGSMStaticService _ogsmstaticservice = new OGSMStaticService();

        //**********************  基站基础数据  Start  ****************
        #region 基站基础数据
        string AthId = Guid.NewGuid().ToString();
        // GET: OGSM
        [SysOperation(OperationType.Browse, "访问基站基础数据页面")]
        public ActionResult OGSMIndex()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult GetOGSMListData(int pageIndex = 1, int pageSize = int.MaxValue, OGSMQueryBuilder queryBuilder = null)
        {
            if (queryBuilder.ContractEndTime_Start != null)
                queryBuilder.ContractEndTime_Start = queryBuilder.ContractEndTime_Start.Value.Date;
            if (queryBuilder.ContractEndTime_End != null)
                queryBuilder.ContractEndTime_End = queryBuilder.ContractEndTime_End.Value.AddDays(1).Date.AddSeconds(-1);
            int count = 0;
            var modelList = _ogsmService.GetForData(out count, queryBuilder, pageIndex <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });
        }
        [SysOperation(OperationType.Export, "导出基站基础数据")]
        public ActionResult OGSMExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OGSMQueryBuilder>(queryBuilder);
                if (QueryBuilder.ContractEndTime_Start != null)
                    QueryBuilder.ContractEndTime_Start = QueryBuilder.ContractEndTime_Start.Value.Date;
                if (QueryBuilder.ContractEndTime_End != null)
                    QueryBuilder.ContractEndTime_End = QueryBuilder.ContractEndTime_End.Value.AddDays(1).Date.AddSeconds(-1);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _ogsmService.GetForData(out count, QueryBuilder, pageIndex <= 0 ? 0 : pageIndex, pageSize);
                var listResult = modelList.Select(item => new
                {
                    item.Group_Name,
                    item.Town,
                    item.USR_NBR,
                    item.PowerStation,
                    item.BaseStation,
                    item.PowerType,
                    item.PropertyRight,
                    item.IsRemove,
                    RemoveTime = item.RemoveTime.HasValue ? item.RemoveTime.Value.ToString("yyyy-MM-dd") : "",
                    item.Price,
                    item.Property,
                    item.LinkMan,
                    item.Mobile,
                    item.IsShare,
                    item.Address,
                    item.PAY_CYC,
                    item.IsWarn,
                    item.WarnCount,
                    item.Remark,
                    ContractStartTime = item.ContractStartTime.HasValue ? item.ContractStartTime.Value.ToString("yyyy-MM-dd") : "",
                    ContractEndTime = item.ContractEndTime.HasValue ? item.ContractEndTime.Value.ToString("yyyy-MM-dd") : ""
                }).ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.OGSMBase + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.OGSMBase);
                //设置集合变量
                designer.SetDataSource(ImportFileType.OGSMBase, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        [SysOperation(OperationType.Delete, "删除基站基础数据")]
        public ActionResult OGSMDelete(string[] ids)
        {
            var objs = _ogsmService.List().Where(u => ids.Contains(u.USR_NBR)).ToList();
            if (objs.Count > 0)
            {
                if (_ogsmmonthService.List().Where(u => ids.Contains(u.USR_NBR)).ToList().Count > 0 || _ogsminfoService.List().Where(u => ids.Contains(u.USR_NBR)).ToList().Count > 0)
                {
                    //return Json(new { status = 0, message = "选择的记录中存在缴费明细或者月度明细，只能删除没有缴费明细以及没有月度明细的记录！" });
                    return Json(new { status = 2, message = "存在明细" });
                }
                else
                {
                    if (_ogsmService.DeleteByList(objs))
                    {
                        return Json(new { status = 0, message = "删除成功" });
                    }
                    else
                    {
                        return Json(new { status = 0, message = "删除失败" });
                    }
                }


            }
            else
            {
                return Json(new { status = 0, message = "没有可删除的数据" });
            }
        }

        public ActionResult OGSMEdit(string id, string type)
        {
            ViewData["type"] = type;
            ViewData["id"] = id;
            ViewData["AthId"] = Guid.NewGuid().ToString();
            return View();
        }
        public ActionResult GetOGSMDataByID(string id)
        {
            OGSM ogsm = new OGSM();
            ogsm = _ogsmService.FindByFeldName(a => a.USR_NBR == id);
            AthId = ogsm.AttachmentId.ToString();
            var ogsmList = new
            {
                ogsm.Id,
                ogsm.Group_Name,
                ogsm.Town,
                ogsm.USR_NBR,
                ogsm.PowerStation,
                ogsm.BaseStation,
                ogsm.PowerType,
                ogsm.PropertyRight,
                ogsm.IsRemove,
                RemoveTime = ogsm.RemoveTime == null ? "" : ogsm.RemoveTime.Value.ToString("yyyy-MM-dd"),
                ogsm.Price,
                ogsm.Address,
                ogsm.PAY_CYC,
                ogsm.Property,
                ogsm.Mobile,
                ogsm.LinkMan,
                ogsm.IsWarn,
                ogsm.WarnCount,
                ogsm.Remark,
                ogsm.IsShare,
                ContractStartTime = ogsm.ContractStartTime == null ? "" : ogsm.ContractStartTime.Value.ToString("yyyy-MM-dd"),
                ContractEndTime = ogsm.ContractEndTime == null ? "" : ogsm.ContractEndTime.Value.ToString("yyyy-MM-dd"),
                ogsm.AttachmentId
            };
            var modelList = _attchmentService.GetAllAttachmentList(ogsm.AttachmentId);
            return Json(new { itemogsm = ogsmList, data = modelList });
            //return Json(new
            //{
            //    objs.Id,
            //    objs.Group_Name,
            //    objs.Town,
            //    objs.USR_NBR,
            //    objs.PowerStation,
            //    objs.BaseStation,
            //    objs.PowerType,
            //    objs.PropertyRight,
            //    objs.IsRemove,
            //    RemoveTime = objs.RemoveTime == null ? "" : objs.RemoveTime.Value.ToString("yyyy-MM-dd"),
            //    objs.Price,
            //    objs.Address,
            //    objs.PAY_CYC,
            //    objs.Property,
            //    objs.Mobile,
            //    objs.LinkMan,
            //    objs.IsWarn,
            //    objs.WarnCount,
            //    objs.Remark,
            //    objs.IsShare,
            //    ContractStartTime = objs.ContractStartTime == null ? "" : objs.ContractStartTime.Value.ToString("yyyy-MM-dd"),
            //    ContractEndTime = objs.ContractEndTime == null ? "" : objs.ContractEndTime.Value.ToString("yyyy-MM-dd"),
            //    objs.AttachmentId
            //});
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        [SysOperation(OperationType.Save, "保存基站基础数据")]
        public ActionResult OGSMSave(OGSM dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法            
            #endregion
            var objs = _ogsmService.List().Where(u => u.USR_NBR == dataObj.USR_NBR && u.Id != dataObj.Id);
            if (objs.Count() > 0)
            {
                result.Message = "该户号已经被使用，请更换！";
                return Json(result);
            }
            if (dataObj.Id != 0)
            {
                result.IsSuccess = _ogsmService.Update(dataObj);
            }
            else
            {
                //dataObj.AttachmentId = new Guid(AthId);
                dataObj.IsNew = "0";
                result.IsSuccess = _ogsmService.Insert(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }
        //public ActionResult OGSMUpload(string Upguid)
        //{
        //    SystemResult result = new SystemResult() { IsSuccess = false };
        //    HttpPostedFileBase file = Request.Files[0];
        //    Admin_Attachment attachment = new Admin_Attachment();
        //    HttpContext context= System.Web.HttpContext.Current;
        //    string aid = Upguid;// context.Request.Params["Upguid"];
        //    string fileName = null;
        //    try
        //    {
        //        fileName = FileHelper.GetUniqueFileName(file.FileName);
        //        fileName = UploadHelper.Upload(file, fileName);

        //        if(!string.IsNullOrEmpty(fileName))
        //        { 
        //            attachment.FileName = System.IO.Path.GetFileName(file.FileName);
        //            attachment.MimeType = System.IO.Path.GetExtension(file.FileName);
        //            attachment.FileSize = FileHelper.FileSize(file.ContentLength.ToString());
        //            attachment.Upguid = new Guid(aid);
        //            attachment.Fileupload = fileName;
        //            attachment.Creator = this.WorkContext.CurrentUser.UserName;
        //            attachment.CreatedTime = DateTime.Now;
        //            attachment.Id = Guid.NewGuid();
        //            result.IsSuccess = _attchmentService.Insert(attachment);
        //            result.Message = "上传文件成功!";
        //            var modelList = _attchmentService.GetAllAttachmentList(new Guid(Upguid));
        //            result.data = modelList;
        //        }
        //        else
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "上传文件错误!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string exstr = ex.ToString();
        //        result.Message = "上传文件错误!";
        //    }
        //    return Json(result);
        //}

        //public ActionResult OGSMUploadDelete(Guid id)
        //{
        //    SystemResult result = new SystemResult() { IsSuccess = false };
        //    var objs = _attchmentService.FindById(id);
        //    var obj = _attchmentService.List().Where(u => u.Id == id).ToList();
        //    var Upguid = objs.Upguid.ToString();
        //    var modelList = _attchmentService.GetAllAttachmentList(new Guid(Upguid));
        //    if (objs !=null)
        //    {
        //        if (_attchmentService.DeleteByList(obj))
        //        {
        //            modelList = _attchmentService.GetAllAttachmentList(new Guid(Upguid));
        //            result.IsSuccess = true;
        //            result.Message = "删除成功";
        //            result.data = modelList;
        //        }
        //    }
        //    if (!result.IsSuccess)
        //    {
        //        result.Message = "删除失败";
        //        result.data = modelList;
        //    }
        //    return Json(result);
        //}           

        #endregion
        //********************  基站基础数据 End      ******************
        //********************  基站月度数据 Start    ******************
        #region 基站月度数据
        // GET: OGSMMonth
        [SysOperation(OperationType.Browse, "访问基站月度数据页面")]
        public ActionResult OGSMMonthIndex()
        {
            return View();
        }

        /// <summary>
        /// 获取所有基站月度数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="USR_NBR"></param>
        /// <param name="IsPayment"></param>
        /// <param name="PaymentTime_Start"></param>
        /// <param name="PaymentTime_End"></param>
        /// <param name="AccountTime_Start"></param>
        /// <param name="AccountTime_End"></param>
        /// <param name="PAY_MON"></param>
        /// <returns></returns>
        public ActionResult GetOGSMMonthListDataByCondition(int pageIndex = 1, int pageSize = 5, OGSMMonthQueryBuilder queryBuilder = null)
        {
            int count = 0;

            var modelList = _ogsmmonthService.GetForPagingByCondition(out count, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, queryBuilder);
            List<object> resultList = new List<object>();
            foreach (var obj in modelList)
            {
                resultList.Add(new
                {
                    obj.Id,
                    obj.PAY_MON,
                    obj.USR_NBR,
                    obj.IsPayment,
                    PaymentTime = obj.PaymentTime.HasValue ? Convert.ToDateTime(obj.PaymentTime).ToString("yyyy-MM-dd") : "",
                    AccountTime = obj.AccountTime.HasValue ? Convert.ToDateTime(obj.AccountTime).ToString("yyyy-MM-dd") : "",
                    obj.AccountMoney,
                    obj.AccountNo,
                    obj.CMPower2G,
                    obj.CMPower3G,
                    obj.CMPower4G,
                    obj.CUPower2G,
                    obj.CUPower3G,
                    obj.CUPower4G,
                    obj.CTPower2G,
                    obj.CTPower3G,
                    obj.CTPower4G
                });
            }
            return Json(new { items = resultList, count = count });
        }
        [SysOperation(OperationType.Export, "导出基站月度数据")]
        public ActionResult OGSMMonthExport(string queryBuilder = null)
        {
            int pay_mon_st = 0, pay_mon_ed = 0;
            var QueryBuilder = JsonConvert.DeserializeObject<OGSMMonthQueryBuilder>(queryBuilder);
            //if (string.IsNullOrEmpty(QueryBuilder.PAY_MON_End) || string.IsNullOrEmpty(QueryBuilder.PAY_MON_Start))
            //{
            //    pay_mon_st = 0;
            //    pay_mon_ed = 0;
            //}
            //else
            //{
            //    int.TryParse(QueryBuilder.PAY_MON_Start, out pay_mon_st);
            //    int.TryParse(QueryBuilder.PAY_MON_End, out pay_mon_ed);
            //}
            try
            {
                var modelList = _ogsmmonthService.GetForExportData(QueryBuilder);                
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.OGSMMonth + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.OGSMMonth);
                //设置集合变量
                designer.SetDataSource(ImportFileType.OGSMMonth, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [SysOperation(OperationType.Delete, "删除基站月度数据")]
        public ActionResult OGSMMonthDelete(int[] ids)
        {
            //Delete
            var objs = _ogsmmonthService.List().Where(u => ids.Contains(u.Id)).ToList();
            if (objs.Count > 0)
            {
                if (_ogsmmonthService.DeleteByList(objs))
                {
                    return Json(new { status = 0, message = "删除成功" });
                }
                else
                {
                    return Json(new { status = 0, message = "删除失败" });
                }
            }
            else
            {
                return Json(new { status = 0, message = "没有可删除的数据" });
            }
        }
        /// <summary>
        /// 新增编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OGSMMonthEdit(int? id)
        {
            ViewData["id"] = id;
            if (id == null)
                ViewBag.Title = "新增基站月度数据";
            else
                ViewBag.Title = "编辑基站月度数据";
            return View();
        }
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetOGSMMonthDataByID(int id)
        {
            var obj = _ogsmmonthService.FindById(id);
            return Json(new
            {
                obj.Id,
                obj.USR_NBR,
                obj.PAY_MON,
                obj.IsPayment,
                PaymentTime = obj.PaymentTime == null ? "" : obj.PaymentTime.Value.ToString("yyyy-MM-dd"),
                AccountTime = obj.AccountTime == null ? "" : obj.AccountTime.Value.ToString("yyyy-MM-dd"),
                obj.AccountMoney,
                obj.AccountNo,
                obj.CMPower2G,
                obj.CMPower3G,
                obj.CMPower4G,
                obj.CUPower2G,
                obj.CUPower3G,
                obj.CUPower4G,
                obj.CTPower2G,
                obj.CTPower3G,
                obj.CTPower4G,
                obj.Remark,
                obj.Creator,
                obj.CreatedTime,
                obj.LastModifier,
                obj.LastModTime
            });
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        [SysOperation(OperationType.Save, "保存基站月度数据")]
        public ActionResult OGSMMonthSave(OGSMMonth dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过            
            #endregion

            if (dataObj.Id == 0)//新增
            {
                dataObj.Creator = this.WorkContext.CurrentUser.UserName;
                dataObj.CreatedTime = DateTime.Now;
                result.IsSuccess = _ogsmmonthService.Insert(dataObj);
            }
            else
            {//编辑
                dataObj.LastModifier = this.WorkContext.CurrentUser.UserName;
                dataObj.LastModTime = DateTime.Now;
                if (dataObj.Remark == null) dataObj.Remark = "";
                result.IsSuccess = _ogsmmonthService.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }
        #endregion
        //*******************  基站月度数据 End  *******************
        //*******************  基站电量缴费明细数据 Start  *******************
        #region 基站电量缴费明细数据
        [SysOperation(OperationType.Browse, "访问基站电量缴费明细数据页面")]
        // GET: OGSMInfo
        public ActionResult OGSMInfoIndex(string USR_NBR, string PAY_MON_Start, string PAY_MON_End, string BaseStation, string Group_Name, string type = "NJUMP")
        {
            ViewData["Type"] = type;
            ViewData["USR_NBR"] = USR_NBR;
            ViewData["PAY_MON_Start"] = PAY_MON_Start;
            ViewData["PAY_MON_End"] = PAY_MON_End;
            ViewData["BaseStation"] = BaseStation;
            ViewData["Group_Name"] = Group_Name;
            return View();
        }
        public ActionResult GetOGSMInfoListDataByCondition(int pageIndex = 1, int pageSize = 5, OGSMInfoQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ogsminfoService.GetForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, queryBuilder);
            return Json(new { items = modelList, count = count });
        }
        [SysOperation(OperationType.Export, "导出基站电量缴费明细数据")]
        public ActionResult OGSMInfoExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OGSMInfoQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;                
                var modelList = _ogsminfoService.GetForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, QueryBuilder);
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.OGSMInfo + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.OGSMInfo);
                //设置集合变量
                designer.SetDataSource(ImportFileType.OGSMInfo, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();                
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        [SysOperation(OperationType.Delete, "删除基站缴费数据")]
        public ActionResult OGSMInfoDelete(int[] ids)
        {
            //Delete
            var objs = _ogsminfoService.List().Where(u => ids.Contains(u.Id)).ToList();
            if (objs.Count > 0)
            {
                if (_ogsminfoService.DeleteByList(objs))
                {
                    return Json(new { status = 0, message = "删除成功" });
                }
                else
                {
                    return Json(new { status = 0, message = "删除失败" });
                }
            }
            else
            {
                return Json(new { status = 0, message = "没有可删除的数据" });
            }
        }
        /// <summary>
        /// 新增编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OGSMInfoEdit(int? id)
        {
            ViewData["id"] = id;
            if (id == null)
                ViewBag.Title = "新增基站电量缴费明细数据";
            else
                ViewBag.Title = "编辑基站电量缴费明细数据";
            return View();
        }
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetOGSMInfoDataByID(int id)
        {
            var mm = id;
            var obj = _ogsminfoService.FindById(id);
            var ogsmobj = _ogsmService.FindByFeldName(u => u.USR_NBR == obj.USR_NBR);
            return Json(new
            {
                obj.Id,
                obj.USR_NBR,
                obj.PAY_MON,
                obj.PreKwh,
                obj.NowKwh,
                obj.MF,
                obj.CHG,
                obj.Price,
                obj.Adjustment,
                obj.Money,
                obj.CHG_COMPARE,
                obj.Money_COMPARE,
                obj.New_Meter,
                RTime = obj.RTime == null ? "" : obj.RTime.Value.ToString("yyyy-MM-dd"),
                obj.Payee,
                obj.Mobile1,
                obj.Mobile2,
                obj.BankAcount,
                obj.Bank,
                obj.Address,
                obj.PSubPayMonth,
                obj.Remark,
                obj.Creator,
                obj.CreatedTime,
                obj.LastModifier,
                obj.LastModTime,
                ogsmobj.BaseStation,
                ogsmobj.PowerType
            });
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        [SysOperation(OperationType.Save, "保存基站缴费数据")]
        public ActionResult OGSMInfoSave(OGSM_Info dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            OGSM ogsmobj = new OGSM();
            OGSMInfo ogsminfoobj = new OGSMInfo();
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过            
            if (dataObj.PowerType == "私电")
            {
                if (dataObj.Payee == null)
                {
                    tip = "收款人姓名不能为空";
                }
                else if (dataObj.Mobile1 == null)
                {
                    tip = "联系电话1不能为空";
                }
                else if (dataObj.Mobile2 == null)
                {
                    tip = "联系电话2不能为空";
                }
                else if (dataObj.BankAcount == null)
                {
                    tip = "银行账号不能为空";
                }
                else if (dataObj.Bank == null)
                {
                    tip = "开户行不能为空";
                }
                else if (dataObj.Address == null)
                {
                    tip = "详细地址不能为空";
                }
                else if (dataObj.PSubPayMonth == null)
                {
                    tip = "私电分缴月数不能为空";
                }
                if (tip == "")
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }
            else
            {
                isValid = true;

            }
            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            int pay_mon = 0;
            //int.TryParse(dataObj.PAY_MON, out pay_mon);
            var objs = _ogsmService.List().Where(u => u.USR_NBR == dataObj.USR_NBR);
            //如果在基础表中不存在户名，则标识为新增
            if (objs.Count() == 0)
            {
                ogsmobj.IsNew = "1";
                ogsmobj.USR_NBR = dataObj.USR_NBR;
                ogsmobj.BaseStation = dataObj.BaseStation;
                _ogsmService.Insert(ogsmobj);
            }

            if (dataObj.Id == 0)//新增
            {
                ogsminfoobj.USR_NBR = dataObj.USR_NBR;
                ogsminfoobj.PAY_MON = dataObj.PAY_MON;
                ogsminfoobj.PreKwh = dataObj.PreKwh;
                ogsminfoobj.NowKwh = dataObj.NowKwh;
                ogsminfoobj.MF = dataObj.MF;
                ogsminfoobj.CHG = dataObj.CHG;
                ogsminfoobj.Price = dataObj.Price;
                ogsminfoobj.Adjustment = dataObj.Adjustment;
                ogsminfoobj.Money = dataObj.Money;
                ogsminfoobj.New_Meter = dataObj.New_Meter;
                ogsminfoobj.RTime = dataObj.RTime;
                ogsminfoobj.Payee = dataObj.Payee;
                ogsminfoobj.Mobile1 = dataObj.Mobile1;
                ogsminfoobj.Mobile2 = dataObj.Mobile2;
                ogsminfoobj.BankAcount = dataObj.BankAcount;
                ogsminfoobj.Bank = dataObj.Bank;
                ogsminfoobj.Address = dataObj.Address;
                ogsminfoobj.PSubPayMonth = dataObj.PSubPayMonth;
                ogsminfoobj.Remark = dataObj.Remark;
                ogsminfoobj.CHG_COMPARE = "准确";
                ogsminfoobj.Money_COMPARE = "准确";
                ogsminfoobj.Creator = this.WorkContext.CurrentUser.UserName;
                ogsminfoobj.CreatedTime = DateTime.Now;
                result.IsSuccess = _ogsminfoService.Insert(ogsminfoobj);
            }
            else
            {//编辑
                ogsminfoobj.Id = dataObj.Id;
                ogsminfoobj.USR_NBR = dataObj.USR_NBR;
                ogsminfoobj.PAY_MON = dataObj.PAY_MON;
                ogsminfoobj.PreKwh = dataObj.PreKwh;
                ogsminfoobj.NowKwh = dataObj.NowKwh;
                ogsminfoobj.MF = dataObj.MF;
                ogsminfoobj.CHG = dataObj.CHG;
                ogsminfoobj.Price = dataObj.Price;
                ogsminfoobj.Adjustment = dataObj.Adjustment;
                ogsminfoobj.Money = dataObj.Money;
                ogsminfoobj.CHG_COMPARE = "准确";
                ogsminfoobj.Money_COMPARE = "准确";
                ogsminfoobj.New_Meter = dataObj.New_Meter;
                ogsminfoobj.RTime = dataObj.RTime;
                ogsminfoobj.Payee = dataObj.Payee;
                ogsminfoobj.Mobile1 = dataObj.Mobile1;
                ogsminfoobj.Mobile2 = dataObj.Mobile2;
                ogsminfoobj.BankAcount = dataObj.BankAcount;
                ogsminfoobj.Bank = dataObj.Bank;
                ogsminfoobj.Address = dataObj.Address;
                ogsminfoobj.PSubPayMonth = dataObj.PSubPayMonth;
                ogsminfoobj.LastModifier = this.WorkContext.CurrentUser.UserName;
                ogsminfoobj.LastModTime = DateTime.Now;
                if (dataObj.Remark == null)
                    ogsminfoobj.Remark = "";
                else
                    ogsminfoobj.Remark = dataObj.Remark;
                result.IsSuccess = _ogsminfoService.Update(ogsminfoobj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }




        #endregion
        //*******************  基站电量缴费明细数据 End    *******************

        //*******************   基站电量分表 Start  *******************
        #region 基站电量分表

        [SysOperation(OperationType.Browse, "访问基站电量分表页面")]
        // GET: OGSMMonth
        public ActionResult OGSMElectricityIndex(int? id, string USR_NBR, string PAY_MON)
        {
            if (id != null)
            {
                var obj = _ogsminfoService.FindById(id);
                ViewData["USR_NBR"] = obj.USR_NBR;
                ViewData["PAY_MON"] = obj.PAY_MON;
            }
            else
            {
                ViewData["USR_NBR"] = USR_NBR;
                ViewData["PAY_MON"] = PAY_MON;
            }

            return View();
        }

        /// <summary>
        /// 获取基站电量分表数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="USR_NBR"></param>
        /// <param name="ElectricityMeter"></param>
        /// <param name="PAY_MON"></param>
        /// <returns></returns>
        public ActionResult GetOGSMElectricityListDataByCondition(int pageIndex = 1, int pageSize = 5, OGSMElectricityQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ogsmelectricityService.GetForPagingByCondition(out count, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, queryBuilder);
            List<object> resultList = new List<object>();
            foreach (var obj in modelList)
            {
                resultList.Add(new
                {
                    obj.Id,
                    obj.PAY_MON,
                    obj.USR_NBR,
                    obj.ElectricityMeter,
                    obj.Electricity,
                    obj.Remark
                });
            }
            return Json(new { items = resultList, count = count });
        }
        [SysOperation(OperationType.Export, "导出基站电量分表数据")]
        public ActionResult OGSMElectricityIExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OGSMInfoQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                int pay_mon_st = 0, pay_mon_ed = 0;
                if (string.IsNullOrEmpty(QueryBuilder.PAY_MON_End) || string.IsNullOrEmpty(QueryBuilder.PAY_MON_Start))
                {
                    pay_mon_st = 0;
                    pay_mon_ed = 0;
                }
                else
                {
                    int.TryParse(QueryBuilder.PAY_MON_Start, out pay_mon_st);
                    int.TryParse(QueryBuilder.PAY_MON_End, out pay_mon_ed);
                }

                var modelList = _ogsmelectricityService.GetForPagingIByCondition(QueryBuilder);
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.Electricity + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Electricity);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Electricity, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        [SysOperation(OperationType.Export, "导出基站电量分表数据")]
        public ActionResult OGSMElectricityExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OGSMElectricityQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _ogsmelectricityService.GetForPagingByCondition(out count, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, QueryBuilder);
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.Electricity + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Electricity);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Electricity, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();                
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [SysOperation(OperationType.Delete, "删除基站缴费分表数据")]
        public ActionResult OGSMElectricityDelete(int[] ids)
        {
            //Delete
            var objs = _ogsmelectricityService.List().Where(u => ids.Contains(u.Id)).ToList();
            if (objs.Count > 0)
            {
                if (_ogsmelectricityService.DeleteByList(objs))
                {
                    return Json(new { status = 0, message = "删除成功" });
                }
                else
                {
                    return Json(new { status = 0, message = "删除失败" });
                }
            }
            else
            {
                return Json(new { status = 0, message = "没有可删除的数据" });
            }
        }
        /// <summary>
        /// 新增编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OGSMElectricityEdit(int? id, string USR_NBR, string PAY_MON)
        {
            ViewData["id"] = id;

            if (id == null)
            {
                ViewBag.Title = "新增基站电量分表数据";
                ViewData["USR_NBR"] = USR_NBR;
                ViewData["PAY_MON"] = PAY_MON;
            }
            else
            {
                var obj = _ogsmelectricityService.FindById(id);
                ViewBag.Title = "编辑基站电量分表数据";
                ViewData["USR_NBR"] = obj.USR_NBR;
                ViewData["PAY_MON"] = obj.PAY_MON;
            }


            return View();
        }
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetOGSMElectricityDataByID(int id)
        {
            var obj = _ogsmelectricityService.FindById(id);
            return Json(new
            {
                obj.Id,
                obj.USR_NBR,
                obj.PAY_MON,
                obj.ElectricityMeter,
                obj.Electricity,
                obj.Remark,
                obj.Creator,
                obj.CreatedTime,
                obj.LastModifier,
                obj.LastModTime
            });
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        [SysOperation(OperationType.Save, "保存基站缴费分表数据")]
        public ActionResult OGSMElectricitySave(OGSMElectricity dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过            
            #endregion

            if (dataObj.Id == 0)//新增
            {
                dataObj.Creator = this.WorkContext.CurrentUser.UserName;
                dataObj.CreatedTime = DateTime.Now;
                result.IsSuccess = _ogsmelectricityService.Insert(dataObj);
            }
            else
            {//编辑
                dataObj.LastModifier = this.WorkContext.CurrentUser.UserName;
                dataObj.LastModTime = DateTime.Now;
                if (dataObj.Remark == null) dataObj.Remark = "";
                result.IsSuccess = _ogsmelectricityService.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }
        #endregion
        //*******************   基站电量分表 End    *******************


        //******************* 基站月度电费情况统计  Start  *******************
        #region 基站月度电费情况统计
        [SysOperation(OperationType.Browse, "访问基站月度电费情况统计页面")]
        public ActionResult OGSMBasestationMonthStatic()
        {
            ViewData["PAY_MON_Start"] = DateTime.Now.AddMonths(-12).ToString("yyyyMM");
            ViewData["PAY_MON_End"] = DateTime.Now.ToString("yyyyMM");
            return View();
        }


        public ActionResult GetOGSMBasestationMonthStatic(int pageIndex = 1, int pageSize = 5, OGSMBasestationMonthStaticQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ogsmstaticservice.GetForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, queryBuilder);
            return Json(new { items = modelList, count = count });

        }

        [SysOperation(OperationType.Export, "导出基站月度电费情况统计数据")]
        public ActionResult OGSMBasestationMonthStaticExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OGSMBasestationMonthStaticQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _ogsmstaticservice.GetForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, QueryBuilder);
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.BasestationMonth + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.BasestationMonth);
                //设置集合变量
                designer.SetDataSource(ImportFileType.BasestationMonth, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();                
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        #endregion
        //******************* 基站月度电费情况统计  End    *******************


        //******************* 基站年度电费情况统计  Start  *******************
        #region 基站年度电费情况统计
        [SysOperation(OperationType.Browse, "访问基站年度电费情况统计页面")]
        public ActionResult OGSMBasestationYearStatic()
        {
            ViewData["PAY_MON_Start"] = DateTime.Now.ToString("yyyy");
            ViewData["PAY_MON_End"] = DateTime.Now.ToString("yyyy");
            return View();
        }


        public ActionResult GetOGSMBasestationYearStatic(int pageIndex = 1, int pageSize = 5, OGSMBasestationMonthStaticQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ogsmstaticservice.GetBasestationYearForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, queryBuilder);
            return Json(new { items = modelList, count = count });

        }
        [SysOperation(OperationType.Export, "导出基站年度电费情况统计数据")]
        public ActionResult OGSMBasestationYearStaticExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OGSMBasestationMonthStaticQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _ogsmstaticservice.GetBasestationYearForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, QueryBuilder);
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.BasestationYear + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.BasestationYear);
                //设置集合变量
                designer.SetDataSource(ImportFileType.BasestationYear, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        #endregion
        //******************* 基站年度电费情况统计  End    *******************



        //******************* 分公司月度电费情况统计  Start  *******************
        #region 分公司月度电费情况统计
        [SysOperation(OperationType.Browse, "访问分公司月度电费情况统计页面")]
        public ActionResult OGSMGroupMonthStatic()
        {
            ViewData["PAY_MON_Start"] = DateTime.Now.AddMonths(-12).ToString("yyyyMM");
            ViewData["PAY_MON_End"] = DateTime.Now.ToString("yyyyMM");
            return View();
        }


        public ActionResult GetOGSMGroupMonthStatic(int pageIndex = 1, int pageSize = 5, OGSMGroupMonthStaticQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ogsmstaticservice.GetGroupMonthForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, queryBuilder);
            return Json(new { items = modelList, count = count });

        }

        [SysOperation(OperationType.Export, "导出分公司月度电费情况统计数据")]
        public ActionResult OGSMGroupMonthStaticExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OGSMGroupMonthStaticQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _ogsmstaticservice.GetGroupMonthForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, QueryBuilder);
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.GroupMonth + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.GroupMonth);
                //设置集合变量
                designer.SetDataSource(ImportFileType.GroupMonth, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();                
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;

        }
        #endregion
        //******************* 分公司月度电费情况统计  End    *******************

        //******************* 分公司年度电费情况统计  Start  *******************
        #region 分公司年度电费情况统计
        [SysOperation(OperationType.Browse, "访问分公司年度电费情况统计页面")]
        public ActionResult OGSMGroupYearStatic()
        {
            ViewData["PAY_MON_Start"] = DateTime.Now.ToString("yyyy");
            ViewData["PAY_MON_End"] = DateTime.Now.ToString("yyyy");
            return View();
        }


        public ActionResult GetOGSMGroupYearStatic(int pageIndex = 1, int pageSize = 5, OGSMGroupMonthStaticQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ogsmstaticservice.GetGroupYearForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, queryBuilder);
            return Json(new { items = modelList, count = count });

        }
        [SysOperation(OperationType.Export, "导出分公司年度电费情况统计数据")]
        public ActionResult OGSMGroupYearStaticExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OGSMGroupMonthStaticQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _ogsmstaticservice.GetGroupYearForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, QueryBuilder);
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.GroupYear + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.GroupYear);
                //设置集合变量
                designer.SetDataSource(ImportFileType.GroupYear, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        #endregion
        //******************* 分公司年度电费情况统计  End    *******************



        //******************* 分公司基站用电统计  Start  *******************
        #region 分公司基站用电统计
        [SysOperation(OperationType.Browse, "访问分公司基站用电统计页面")]
        public ActionResult OGSMGroupBasestationStatic()
        {
            return View();
        }


        public ActionResult GetGroupBasestationStatic(int pageIndex = 1, int pageSize = 5)
        {
            int count = 0;
            var modelList = _ogsmstaticservice.GetGroupBasestationForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }
        #endregion
        //******************* 分公司基站用电统计  End    *******************



        //******************* 基站变化趋势分析统计  Start  *******************
        #region 基站变化趋势分析统计
        [SysOperation(OperationType.Browse, "访问基站变化趋势分析统计页面")]
        public ActionResult OGSMBasestationChangeStatic()
        {
            ViewData["PAY_MON_Start"] = DateTime.Now.AddMonths(-12).ToString("yyyyMM");
            ViewData["PAY_MON_End"] = DateTime.Now.ToString("yyyyMM");
            return View();
        }


        public ActionResult GetBasestationChangeStatic(int pageIndex = 1, int pageSize = 5, OGSMBasestationChangeQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ogsmstaticservice.GetBasestationChangeForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, queryBuilder);
            return Json(new { items = modelList, count = count });

        }
        [SysOperation(OperationType.Export, "导出基站变化趋势分析统计数据")]
        public ActionResult OGSMBasestationChangeExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OGSMBasestationChangeQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _ogsmstaticservice.GetBasestationChangeForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, QueryBuilder);
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.BasestationChange + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.BasestationChange);
                //设置集合变量
                designer.SetDataSource(ImportFileType.BasestationChange, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        #endregion
        //******************* 基站变化趋势分析统计  End    *******************


        //******************* 合同到期预警统计  Start  *******************
        #region 合同到期预警统计
        [SysOperation(OperationType.Browse, "访问合同到期预警统计页面")]
        public ActionResult OGSMContractWarningStatic()
        {
            return View();
        }


        public ActionResult GetContractWarningStatic(int pageIndex = 1, int pageSize = 5, OGSMContractWarningQueryBuilder queryBuilder = null)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(queryBuilder.WarningSituation))
                queryBuilder.WarningSituation = queryBuilder.WarningSituation.Remove(0, 1);
            var modelList = _ogsmstaticservice.GetContractWarningForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, queryBuilder);
            return Json(new { items = modelList, count = count });

        }
        [SysOperation(OperationType.Export, "导出合同到期预警统计数据")]
        public ActionResult OGSMContractWarningExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OGSMContractWarningQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                if (!string.IsNullOrEmpty(QueryBuilder.WarningSituation))
                    QueryBuilder.WarningSituation = QueryBuilder.WarningSituation.Remove(0, 1);
                var modelList = _ogsmstaticservice.GetContractWarningForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, QueryBuilder);
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.ContractWarning + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.ContractWarning);
                //设置集合变量
                designer.SetDataSource(ImportFileType.ContractWarning, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        #endregion
        //******************* 合同到期预警统计  End    *******************

        //******************* 基站无缴费预警统计  Start  *******************
        #region 基站无缴费预警统计
        [SysOperation(OperationType.Browse, "访问基站无缴费预警统计页面")]
        public ActionResult OGSMNoPaymentWarningStatic()
        {
            return View();
        }


        public ActionResult GetBasestationNoPaymentWarningStatic(int pageIndex = 1, int pageSize = 5, OGSMNoPaymentWarningQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ogsmstaticservice.GetBasestationNoPaymentWarningForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, queryBuilder);
            return Json(new { items = modelList, count = count });

        }
        [SysOperation(OperationType.Export, "导出基站无缴费预警统计数据")]
        public ActionResult OGSMNoPaymentWarningExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OGSMNoPaymentWarningQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _ogsmstaticservice.GetBasestationNoPaymentWarningForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, QueryBuilder);
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.NoPaymentWarning + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.NoPaymentWarning);
                //设置集合变量
                designer.SetDataSource(ImportFileType.NoPaymentWarning, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();                
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        #endregion
        //******************* 基站无缴费预警统计  End    *******************


        //*******************  其他方法  Start  *******************        
        #region 其他方法
        public ActionResult getUsrNbrAsDict()
        {
            var objList = _ogsmService.List().Select(u => new { u.USR_NBR, u.BaseStation, u.PowerType }).ToList();
            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getBaseStationAsDict()
        {
            var objList = _ogsmService.List().Select(u => new { u.BaseStation }).Distinct().ToList();
            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectBaseStationIndex(string selectedId)
        {
            ViewData["selectedId"] = selectedId;
            return View();
        }
        public ActionResult GetBaseStationListData(int pageIndex = 1, int pageSize = 5, string queryBuilder = null)
        {
            int count = 0;
            var modelList = _ogsmService.GetBaseStationListForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : pageIndex, pageSize).ToList();
            return Json(new { items = modelList, count = count });
        }
        public ActionResult getUsrNbrPowerType(tempOGSMData dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var tempnbr = dataObj.USR_NBR;
            var objPowerType = _ogsmService.FindByFeldName(d => d.USR_NBR == tempnbr.ToString())?.PowerType;
            result.Message = objPowerType;
            return Json(result);
        }

        public static bool ExportExcel(string designerSpreadsheet, DataTable dt, string fileName, System.Web.HttpResponse response)
        {
            try
            {
                WorkbookDesigner designer = new WorkbookDesigner();
                designer.Open(designerSpreadsheet);//打开模板
                designer.SetDataSource(dt);//设置数据源
                designer.Process();
                designer.Save(fileName, SaveType.OpenInExcel, FileFormatType.Excel2007Xlsx, response);//保存格式
                response.Flush();
                response.Close();
                response.End();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        //*******************  其他方法  End    *******************

        //*******************   Start  *******************
        //*******************   End    *******************
    }
}