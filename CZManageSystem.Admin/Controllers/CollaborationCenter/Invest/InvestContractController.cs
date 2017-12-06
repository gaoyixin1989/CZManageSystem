using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.CollaborationCenter.Invest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.CollaborationCenter.Invest
{
    public class InvestContractController : BaseController
    {
        IInvestProjectService _projectService = new InvestProjectService();//投资项目信息
        IInvestContractService _contractService = new InvestContractService();//合同信息
        IInvestMaterialsService _materialsService = new InvestMaterialsService();//物资采购
        IInvestEstimateService _investEstimateService = new InvestEstimateService();//暂估信息
        IInvestTempEstimateService _investTempEstimateService = new InvestTempEstimateService();
        IInvestContractPayService _investContractPayService = new InvestContractPayService();
        // GET: InvestContract
        /// <summary>
        /// 合同信息列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContractSelect(string selected)
        {
            ViewData["selected"] = selected;
            return View();
        }
        /// <summary>
        /// 合同信息编辑页
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(Guid? ID)
        {
            ViewData["ID"] = ID;
            if (ID.HasValue)
                ViewBag.Title = "合同编辑";
            else
                ViewBag.Title = "合同新增";
            return View();
        }

        /// <summary>
        /// 项目合同查询
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectContractQuery()
        {
            return View();
        }

        /// <summary>
        /// 合同信息展示，优先使用ID，如果ID值为空，或根据ID值查询不到对应数据，则使用ProjectID+ContractID查询数据获取ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="ContractID"></param>
        /// <returns></returns>
        public ActionResult ContractInfoShow(Guid? ID, string ProjectID, string ContractID)
        {
            var obj = _contractService.FindById(ID);

            if (obj == null || obj.ID == Guid.Empty)
            {
                if (!string.IsNullOrEmpty(ProjectID) && !string.IsNullOrEmpty(ContractID))
                {
                    obj = _contractService.FindByFeldName(u => u.ProjectID == ProjectID && u.ContractID == ContractID);
                    if (obj != null && obj.ID != Guid.Empty)
                        ID = obj.ID;
                }
            }
            ViewData["ID"] = ID;
            return View();
        }

        /// <summary>
        /// 查询合同信息数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, InvestContractQueryBuilder queryBuilder = null)
        {
            queryBuilder.IsDel = "0";

            int count = 0;
            var modelList = _contractService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (InvestContract)u).Select(u => new
            {
                u.ID,
                u.ImportTime,
                u.ProjectID,
                u.ContractID,
                u.ContractName,
                u.Supply,
                u.SignTime,
                u.DpCode,
                DpCode_Text = CommonFunction.getDeptNamesByIDs(u.DpCode),
                u.UserID,
                User_Text = CommonFunction.getUserRealNamesByIDs(u.UserID?.ToString()),
                u.SignTotal,
                u.AllTotal,
                u.PayTotal,
                u.Content,
                IsMIS = u.IsMIS == "1" ? "是" : "否",
                u.IsDel,
                u.ContractSeries,
                u.Tax,
                u.SignTotalTax,
                u.Currency,
                u.ContractState,
                u.Attribute,
                u.ApproveStartTime,
                u.ApproveEndTime,
                u.ContractFilesNum,
                u.StampTaxrate,
                u.Stamptax,
                u.ContractOpposition,
                u.RequestDp,
                RequestDp_Text = CommonFunction.getDeptNamesByIDs(u.RequestDp),
                u.RelevantDp,
                RelevantDp_Text = CommonFunction.getDeptNamesByIDs(u.RelevantDp),
                u.ProjectCause,
                u.ContractType,
                u.ContractOppositionFrom,
                u.ContractOppositionType,
                u.Purchase,
                u.PayType,
                u.PayRemark,
                u.ContractStartTime,
                u.ContractEndTime,
                u.IsFrameContract,
                u.DraftTime,
                ProjectName = _projectService.FindByFeldName(a => a.ProjectID == u.ProjectID)?.ProjectName,
                u.ProjectTotal,
                u.ProjectAllTotal

            }).ToList();

            var total = new
            {
                SignTotal = _contractService.GetForPaging(out count, queryBuilder).Sum(u => u.SignTotal ?? 0),
                AllTotal = _contractService.GetForPaging(out count, queryBuilder).Sum(u => u.AllTotal ?? 0),
                PayTotal = _contractService.GetForPaging(out count, queryBuilder).Sum(u => u.PayTotal ?? 0),
                Tax = _contractService.GetForPaging(out count, queryBuilder).Sum(u => u.Tax ?? 0),
                SignTotalTax = _contractService.GetForPaging(out count, queryBuilder).Sum(u => u.SignTotalTax ?? 0),
                ProjectTotal = _contractService.GetForPaging(out count, queryBuilder).Sum(u => u.ProjectTotal ?? 0),
                ProjectAllTotal = _contractService.GetForPaging(out count, queryBuilder).Sum(u => u.ProjectAllTotal ?? 0)
            };

            return Json(new { items = modelList, count = count, total = total });
        }

        public ActionResult GetListData_query(int pageIndex = 1, int pageSize = 5, InvestContractQueryBuilder queryBuilder = null)
        {
            queryBuilder.IsDel = "0";

            int count = 0;
            var queryData = _contractService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (InvestContract)u).ToList();
            List<object> modelList = new List<object>();
            foreach (var u in queryData)
            {
                var materialsList = _materialsService.List().Where(a => a.ProjectID == u.ProjectID && a.ContractID == u.ContractID).ToList();
                var lastEstimate = _investEstimateService.List().Where(a => a.ProjectID == u.ProjectID && a.ContractID == u.ContractID).ToList()
                    .OrderByDescending(a => (a.Year ?? 0).ToString().PadLeft(4, '0') + (a.Month ?? 0).ToString().PadLeft(2, '0')).FirstOrDefault();
                modelList.Add(new
                {
                    u.ID,
                    //u.ImportTime,
                    u.ProjectID,
                    u.ContractID,
                    u.ContractName,
                    //u.Supply,
                    //u.SignTime,
                    //u.DpCode,
                    DpCode_Text = CommonFunction.getDeptNamesByIDs(u.DpCode),
                    //u.UserID,
                    User_Text = CommonFunction.getUserRealNamesByIDs(u.UserID?.ToString()),
                    u.SignTotal,
                    //u.AllTotal,
                    u.PayTotal,
                    //u.Content,
                    //IsMIS = u.IsMIS == "1" ? "是" : "否",
                    //u.IsDel,
                    //u.ContractSeries,
                    //u.Tax,
                    //u.SignTotalTax,
                    //u.Currency,
                    //u.ContractState,
                    //u.Attribute,
                    //u.ApproveStartTime,
                    //u.ApproveEndTime,
                    //u.ContractFilesNum,
                    //u.StampTaxrate,
                    //u.Stamptax,
                    //u.ContractOpposition,
                    //u.RequestDp,
                    //RequestDp_Text = CommonFunction.getDeptNamesByIDs(u.RequestDp),
                    //u.RelevantDp,
                    //RelevantDp_Text = CommonFunction.getDeptNamesByIDs(u.RelevantDp),
                    //u.ProjectCause,
                    //u.ContractType,
                    //u.ContractOppositionFrom,
                    //u.ContractOppositionType,
                    //u.Purchase,
                    //u.PayType,
                    //u.PayRemark,
                    //u.ContractStartTime,
                    //u.ContractEndTime,
                    //u.IsFrameContract,
                    //u.DraftTime,
                    ProjectName = _projectService.FindByFeldName(a => a.ProjectID == u.ProjectID)?.ProjectName,
                    //u.ProjectTotal,
                    //u.ProjectAllTotal,
                    MISMoney = materialsList.Sum(a => (decimal)(a.OrderOutSum ?? 0)),
                    Pay = (lastEstimate == null ? 0 : lastEstimate.Pay ?? 0),
                    NotPay =( lastEstimate == null ? 0 : lastEstimate.NotPay ?? 0)
                });

            }

            var query = _contractService.GetForPaging(out count, queryBuilder);
            List<string> ProjectID_ContractID = query.Select(u => u.ProjectID + "_" + u.ContractID).ToList();
            var mList = _materialsService.List().Where(u => ProjectID_ContractID.Contains(u.ProjectID + "_" + u.ContractID)).ToList();

            var lastEList = _investEstimateService.List().Where(u => ProjectID_ContractID.Contains(u.ProjectID + "_" + u.ContractID)).ToList()
                .OrderByDescending(a => (a.Year ?? 0).ToString().PadLeft(4, '0') + (a.Month ?? 0).ToString().PadLeft(2, '0'))
                .GroupBy(u => u.ProjectID + "_" + u.ContractID).Select(u => u.First()).ToList();

            var total = new
            {
                SignTotal = query.Sum(u => u.SignTotal ?? 0),
                //AllTotal = query.Sum(u => u.AllTotal ?? 0),
                PayTotal = query.Sum(u => u.PayTotal ?? 0),
                //Tax = query.Sum(u => u.Tax ?? 0),
                //SignTotalTax = query.Sum(u => u.SignTotalTax ?? 0),
                //ProjectTotal = query.Sum(u => u.ProjectTotal ?? 0),
                //ProjectAllTotal = query.Sum(u => u.ProjectAllTotal ?? 0)
                MISMoney = mList.Sum(u => u.OrderOutSum ?? 0),
                Pay = lastEList.Sum(u => u.Pay ?? 0),
                NotPay = lastEList.Sum(u => u.NotPay ?? 0)
            };

            return Json(new { items = modelList, count = count, total = total, IsMIS = queryBuilder.IsMIS }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载
        /// </summary> 
        public ActionResult Download_InvestProjectContractQuery(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<InvestContractQueryBuilder>(queryBuilder);
                QueryBuilder.IsDel = "0";
                int count = 0;
                
                var queryData = _contractService.GetForPaging(out count, QueryBuilder).Select(u => (InvestContract)u).ToList();
                List<object> modelList = new List<object>();
                foreach (var u in queryData)
                {
                    var materialsList = _materialsService.List().Where(a => a.ProjectID == u.ProjectID && a.ContractID == u.ContractID).ToList();
                    var lastEstimate = _investEstimateService.List().Where(a => a.ProjectID == u.ProjectID && a.ContractID == u.ContractID).ToList()
                        .OrderByDescending(a => (a.Year ?? 0).ToString().PadLeft(4, '0') + (a.Month ?? 0).ToString().PadLeft(2, '0')).FirstOrDefault();
                    modelList.Add(new
                    {
                        u.ID,
                        //u.ImportTime,
                        u.ProjectID,
                        u.ContractID,
                        u.ContractName,
                        //u.Supply,
                        //u.SignTime,
                        //u.DpCode,
                        DpCode_Text = CommonFunction.getDeptNamesByIDs(u.DpCode),
                        //u.UserID,
                        User_Text = CommonFunction.getUserRealNamesByIDs(u.UserID?.ToString()),
                        u.SignTotal,
                        //u.AllTotal,
                        u.PayTotal,
                        //u.Content,
                        IsMIS = u.IsMIS == "1" ? "是" : "否",
                        //u.IsDel,
                        //u.ContractSeries,
                        //u.Tax,
                        //u.SignTotalTax,
                        //u.Currency,
                        //u.ContractState,
                        //u.Attribute,
                        //u.ApproveStartTime,
                        //u.ApproveEndTime,
                        //u.ContractFilesNum,
                        //u.StampTaxrate,
                        //u.Stamptax,
                        //u.ContractOpposition,
                        //u.RequestDp,
                        //RequestDp_Text = CommonFunction.getDeptNamesByIDs(u.RequestDp),
                        //u.RelevantDp,
                        //RelevantDp_Text = CommonFunction.getDeptNamesByIDs(u.RelevantDp),
                        //u.ProjectCause,
                        //u.ContractType,
                        //u.ContractOppositionFrom,
                        //u.ContractOppositionType,
                        //u.Purchase,
                        //u.PayType,
                        //u.PayRemark,
                        //u.ContractStartTime,
                        //u.ContractEndTime,
                        //u.IsFrameContract,
                        //u.DraftTime,
                        ProjectName = _projectService.FindByFeldName(a => a.ProjectID == u.ProjectID)?.ProjectName,
                        //u.ProjectTotal,
                        //u.ProjectAllTotal,
                        MISMoney = u.IsMIS == "1"?(materialsList.Sum(a => a.OrderOutSum ?? 0)).ToString():"",
                        Pay = u.IsMIS == "1"?"":(lastEstimate?.Pay).ToString(),
                        NotPay = u.IsMIS == "1"?"":(lastEstimate?.NotPay).ToString()
                    });

                }


                if (modelList.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.InvestProjectContractQuery + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.InvestProjectContractQuery);
                //设置集合变量
                designer.SetDataSource("emp", modelList);
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
        /// 根据id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(Guid id)
        {
            var obj = _contractService.FindById(id);
            return Json(new
            {
                obj.ID,
                obj.ImportTime,
                obj.ProjectID,
                obj.ContractID,
                obj.ContractName,
                obj.Supply,
                SignTime = obj.SignTime.HasValue ? obj.SignTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                obj.DpCode,
                DpCode_Text = CommonFunction.getDeptNamesByIDs(obj.DpCode),
                obj.UserID,
                User_Text = CommonFunction.getUserRealNamesByIDs(obj.UserID?.ToString()),
                obj.SignTotal,
                obj.AllTotal,
                obj.PayTotal,
                obj.Content,
                obj.IsMIS,
                obj.IsDel,
                obj.ContractSeries,
                obj.Tax,
                obj.SignTotalTax,
                obj.Currency,
                obj.ContractState,
                obj.Attribute,
                obj.ApproveStartTime,
                obj.ApproveEndTime,
                obj.ContractFilesNum,
                obj.StampTaxrate,
                obj.Stamptax,
                obj.ContractOpposition,
                obj.RequestDp,
                obj.RelevantDp,
                obj.ProjectCause,
                obj.ContractType,
                obj.ContractOppositionFrom,
                obj.ContractOppositionType,
                obj.Purchase,
                obj.PayType,
                obj.PayRemark,
                obj.ContractStartTime,
                obj.ContractEndTime,
                obj.IsFrameContract,
                obj.DraftTime,
                ProjectName = _projectService.FindByFeldName(a => a.ProjectID == obj.ProjectID)?.ProjectName,
                obj.ProjectTotal,
                obj.ProjectAllTotal
            });
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;

            var objs = _contractService.List().Where(u => ids.Contains(u.ID)).ToList();
            foreach (var item in objs)
                item.IsDel = "1";
            if (objs.Count > 0)
            {
                isSuccess = _contractService.UpdateByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }

        /// <summary>
        /// 保存合同信息
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Save(InvestContract dataObj,string ProjectID,string ContractID)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过

            if (dataObj.ProjectID == null || string.IsNullOrEmpty(dataObj.ProjectID.Trim()))
                tip = "项目编号不能为空";
            else if (dataObj.ContractID == null || string.IsNullOrEmpty(dataObj.ContractID.Trim()))
                tip = "合同编号不能为空";
            else if (dataObj.ContractName == null || string.IsNullOrEmpty(dataObj.ContractName.Trim()))
                tip = "合同名称不能为空";
            else if (dataObj.DpCode == null || string.IsNullOrEmpty(dataObj.DpCode.Trim()))
                tip = "主办部门不能为空";
            else if (dataObj.UserID == Guid.Empty)
                tip = "主办人不能为空";
            else if (dataObj.Supply == null || string.IsNullOrEmpty(dataObj.Supply.Trim()))
                tip = "供应商不能为空";
            else if (!dataObj.AllTotal.HasValue)
                tip = "合同总金额不能为空";
            else if (!dataObj.SignTotal.HasValue)
                tip = "合同项目金额不能为空";
            else if (!dataObj.Tax.HasValue)
                tip = "合同税金不能为空";
            else if (!dataObj.SignTime.HasValue)
                tip = "签订时间不能为空";
            else if (dataObj.IsMIS == null || string.IsNullOrEmpty(dataObj.IsMIS.Trim()))
                tip = "是否MIS单类不能为空";
           

            else
            {
                isValid = true;
                dataObj.ProjectID = dataObj.ProjectID.Trim();
                dataObj.ContractID = dataObj.ContractID.Trim();
                dataObj.ContractName = dataObj.ContractName.Trim();
                dataObj.DpCode = dataObj.DpCode.Trim();
                dataObj.Supply = dataObj.Supply.Trim();
                dataObj.IsMIS = dataObj.IsMIS == "1" ? "1" : "0";
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion
           

            if (dataObj.ID == Guid.Empty)//新增
            {
                //if (_contractService.List().Where(s => s.ContractID == dataObj.ContractID && s.ProjectID == dataObj.ProjectID).Count() > 0)
                //    tip = "数据重复！[项目编号]与[合同编号]列组合必须唯一";
                dataObj.ID = Guid.NewGuid();
                dataObj.ImportTime = DateTime.Now;
                dataObj.IsDel = "0";
                result.IsSuccess = _contractService.Insert(dataObj);
                if (result.IsSuccess)
                {
                    //插入暂估业务表
                    InvestTempEstimate temp = new InvestTempEstimate();
                    temp.ContractID = dataObj.ContractID;
                    temp.ProjectID = dataObj.ProjectID;
                    temp.SignTotal = dataObj.SignTotal;
                    temp.Supply = dataObj.Supply;
                    temp.PayTotal = dataObj.PayTotal;
                    temp.Status = dataObj.ContractState;
                    temp.ManagerID = dataObj.UserID;
                    var es = _investEstimateService.List().Where(s => s.Year == DateTime.Today.Year && s.Month == DateTime.Today.Month && s.ContractID == dataObj.ContractID && s.ProjectID == dataObj.ProjectID).FirstOrDefault();
                    temp.BackRate = es == null ? 0 : es.BackRate;
                    var cc = _investContractPayService.List().Where(s => s.ContractID == dataObj.ContractID && s.ProjectID == dataObj.ProjectID).FirstOrDefault();
                    temp.Pay = cc == null ? 0 : cc.Pay;
                    temp.NotPay = 0;
                    temp.Rate = 0;
                    temp.ID = Guid.NewGuid();
                   
                    result.IsSuccess= _investTempEstimateService.Insert(temp);
                }
            }
            else
            {//编辑
                result.IsSuccess = _contractService.Update(dataObj);
                if (result.IsSuccess)
                {
                    InvestTempEstimate retemp = _investTempEstimateService.List().Where(s => s.ContractID == ContractID && s.ProjectID == ProjectID).FirstOrDefault();
                    retemp.ContractID = dataObj.ContractID;
                    retemp.ProjectID = dataObj.ProjectID;
                    retemp.SignTotal = dataObj.SignTotal;
                    retemp.Supply = dataObj.Supply;
                    retemp.PayTotal = dataObj.PayTotal;
                    var es = _investEstimateService.List().Where(s => s.Year == DateTime.Today.Year && s.Month == DateTime.Today.Month && s.ContractID == dataObj.ContractID && s.ProjectID == dataObj.ProjectID).FirstOrDefault();
                    retemp.BackRate = es == null ? 0 : es.BackRate;
                    var cc = _investContractPayService.List().Where(s => s.ContractID == dataObj.ContractID && s.ProjectID == dataObj.ProjectID).FirstOrDefault();
                    retemp.Pay = cc == null ? 0 : cc.Pay;
                    retemp.NotPay = 0;
                    retemp.Rate = 0;
                    result.IsSuccess = _investTempEstimateService.Update(retemp);
                }
                
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            else
            {
                result.Message = dataObj.ID.ToString();
            }
            return Json(result);

        }


        /// <summary>
        /// 下载会投资项目信息数据
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download_InvestContract(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<InvestContractQueryBuilder>(queryBuilder);
                QueryBuilder.IsDel = "0";
                int count = 0;

                var listResult = _contractService.GetForPaging(out count, QueryBuilder).Select(u => (InvestContract)u).Select(obj => new
                {
                    obj.ID,
                    ImportTime = obj.ImportTime.HasValue ? obj.ImportTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    obj.ProjectID,
                    obj.ContractID,
                    obj.ContractName,
                    obj.Supply,
                    SignTime = obj.SignTime.HasValue ? obj.SignTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    obj.DpCode,
                    DpCode_Text = CommonFunction.getDeptNamesByIDs(obj.DpCode),
                    obj.UserID,
                    User_Text = CommonFunction.getUserRealNamesByIDs(obj.UserID?.ToString()),
                    obj.SignTotal,
                    obj.AllTotal,
                    obj.PayTotal,
                    obj.Content,
                    obj.IsMIS,
                    obj.IsDel,
                    obj.ContractSeries,
                    obj.Tax,
                    obj.SignTotalTax,
                    obj.Currency,
                    obj.ContractState,
                    obj.Attribute,
                    ApproveStartTime = obj.ApproveStartTime.HasValue ? obj.ApproveStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    ApproveEndTime = obj.ApproveEndTime.HasValue ? obj.ApproveEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    obj.ContractFilesNum,
                    obj.StampTaxrate,
                    obj.Stamptax,
                    obj.ContractOpposition,
                    obj.RequestDp,
                    obj.RelevantDp,
                    obj.ProjectCause,
                    obj.ContractType,
                    obj.ContractOppositionFrom,
                    obj.ContractOppositionType,
                    obj.Purchase,
                    obj.PayType,
                    obj.PayRemark,
                    ContractStartTime = obj.ContractStartTime.HasValue ? obj.ContractStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    ContractEndTime = obj.ContractEndTime.HasValue ? obj.ContractEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    obj.IsFrameContract,
                    DraftTime = obj.DraftTime.HasValue ? obj.DraftTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    ProjectName = _projectService.FindByFeldName(a => a.ProjectID == obj.ProjectID)?.ProjectName,
                    obj.ProjectTotal,
                    obj.ProjectAllTotal
                }).ToList();


                if (listResult.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.InvestContract + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.InvestContract);
                //设置集合变量
                designer.SetDataSource(ImportFileType.InvestContract, listResult);
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

    }
}