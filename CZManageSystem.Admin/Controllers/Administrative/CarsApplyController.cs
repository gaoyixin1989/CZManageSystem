using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Service.Administrative;
using CZManageSystem.Service.Administrative.VehicleManages;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class CarsApplyController : BaseController
    {
        // GET: CarsApply
        #region Field
        ICarApplyService carApplyService = new CarApplyService();
        ICarInfoService carInfoService = new CarInfoService();
        ICarApplyFeeService carApplyFeeService = new CarApplyFeeService();
        ICarStatusService carStatusService = new CarStatusService();
        ISysDeptmentService sysDeptmentService = new SysDeptmentService();
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        #endregion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id = null)
        {
            //有值=》编辑状态
            ViewData["id"] = !string.IsNullOrEmpty(id) ? id : "0";
            ViewData["workflowName"] = Base.FlowInstance.WorkflowType.UsedCarsApply;
            return View();
        }
        public ActionResult CarsReport()
        {
            return View();
        }
        public ActionResult ForWF(string id = null, string type = null, string change = "")
        {
            //有值=》编辑状态
            ViewData["id"] = !string.IsNullOrEmpty(id) ? id : "0";
            ViewData["change"] = change;
            ViewData["workflowName"] = Base.FlowInstance.WorkflowType.UsedCarsApply;
            ViewData["type"] = type;
            return View();
        }

        public ActionResult UrgentIndex()
        {
            return View();
        }
        public ActionResult UrgentEdit(string id = null)
        {
            //有值=》编辑状态
            ViewData["id"] = !string.IsNullOrEmpty(id) ? id : "0";
            ViewData["workflowName"] = Base.FlowInstance.WorkflowType.UrgentUsedCarsApply;
            return View();
        }
        public ActionResult CarServiceIndex()
        {
            return View();
        }
        public ActionResult CarServiceEdit(string id = null, string type = null)
        {
            //有值=》编辑状态
            ViewData["id"] = !string.IsNullOrEmpty(id) ? id : "0";
            ViewData["type"] = type;
            ViewData["workflowName"] = Base.FlowInstance.WorkflowType.PaidCarServiceApply;
            return View();
        }
        public ActionResult CarServiceWF(string id = null, string type = null)
        {
            //有值=》编辑状态
            ViewData["id"] = !string.IsNullOrEmpty(id) ? id : "0";
            ViewData["workflowName"] = Base.FlowInstance.WorkflowType.UsedCarsApply;
            ViewData["type"] = type;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type"></param>
        /// <param name="ApplyTitle"></param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, int type = 0, string ApplyTitle = null)
        {
            // List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);

            int count = 0;
            object obj = WorkContext.CurrentUser.UserName == Base.Admin.UserName ?
            (obj = new { ApplyType = type, ApplyTitle = ApplyTitle }) :
            (obj = new { ApplyType = type, ApplyTitle = ApplyTitle, ApplyCantId = WorkContext.CurrentUser.UserId });
            var modelList = carApplyService.GetForPaging(out count, obj, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            var items = modelList;
            return Json(new { items = items, count = count });
        }
        public ActionResult GetLeaders()
        {
            var list = GetDictListByDDName(DataDic.Leader).Select(s => new { s.DDValue, s.DDText });
            return Json(list);
        }

        public ActionResult SaveAllotIntro(Guid ApplyId, string CarIds, string AllotIntro, string BalRemark = "")
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                var model = carApplyService.FindById(ApplyId);
                model.AllotIntro = AllotIntro;
                model.BalRemark = BalRemark;
                model.CarIds = CarIds;
                model.Allocator = WorkContext.CurrentUser.RealName;
                model.AllotTime = DateTime.Now;
                var listStatus = carStatusService.List().Where(w => w.CarApplyId == ApplyId).ToList();
                if (listStatus.Count > 0)
                    carStatusService.DeleteByList(listStatus);
                if (carApplyService.Update(model))
                {
                    var list = CarIds.Split(new char[] { ',' });
                    List<CarApplyFee> carApplyFeeList = new List<CarApplyFee>();
                    List<CarStatus> carStatusList = new List<CarStatus>();

                    foreach (var item in list)
                    {
                        var carApplyFee = new CarApplyFee()
                        {
                            ApplyFeeId = Guid.NewGuid(),
                            ApplyTitle = model.ApplyTitle,
                            CarId = new Guid(item),
                            ApplySn = model.ApplySn,
                            CarApplyId = model.ApplyId

                        };
                        carApplyFeeList.Add(carApplyFee);
                        var carStatus = new CarStatus()
                        {
                            Id = Guid.NewGuid(),
                            CarId = new Guid(item),
                            TimeOut = model.TimeOut,
                            FinishTime = model.FinishTime,
                            CarApplyId = model.ApplyId

                        };
                        carStatusList.Add(carStatus);
                    }

                    if (carApplyFeeService.InsertByList(carApplyFeeList) && carStatusService.InsertByList(carStatusList))
                    {

                        result.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(result);
        }
        public ActionResult GetCarsInfo(int corpId, string corpName)
        {
            var list = carInfoService.List().Where(w => w.CorpId == corpId).ToList();
            //所属单位:市公司本部　车牌:粤U3C139　类型:商务车　吨位/人数:7座　品牌:其它　型号:比亚迪M6　司机:林俊涛　手机:13502631215

            var result = list.Select(
                s => new
                {
                    s.CarId,
                    corpName = corpName,//所属单位:市公司本部
                    s.LicensePlateNum,//车牌:粤U3C139
                    s.CarType,//类型:商务车
                    s.CarTonnage,//吨位/人数:7座
                    s.CarBrand,//品牌:其它
                    s.CarModel,//型号:比亚迪M6
                    s.Status,//状态
                    StatusText = GetSta_Text(s.Status),
                    s.CarDriverInfo?.Name,//司机:林俊涛
                    s.CarDriverInfo?.Mobile // 手机:13502631215
                });
            return Json(result);
        }
        public string GetSta_Text(int? sta)
        {
            string text = "";
            switch (sta)
            {
                case 0:
                    text = "空闲";
                    break;
                case 1:
                    text = "出车中";
                    break;
                case 2:
                    text = "送修";
                    break;
                case 3:
                    text = "保养";
                    break;
                case 4:
                    text = "停用";
                    break;
                default:
                    break;
            }
            return text;

        }
        public ActionResult GetDataByID(string id, int type = 0)
        {
            try
            {
                CarApply model;
                var Dept = sysDeptmentService.FindById(WorkContext.CurrentUser.DpId);
                if (!string.IsNullOrEmpty(id) && id != "0")
                    model = carApplyService.FindById(new Guid(id));
                else
                    model = new CarApply()
                    {
                        ApplyTitle = WorkContext.CurrentUser.RealName + GetType(type),
                        Mobile = WorkContext.CurrentUser.Mobile,
                        CreateTime = DateTime.Now,
                        DeptName = Dept?.DpName,
                        ApplyCant = WorkContext.CurrentUser.RealName,
                        Driver = "",
                        Boral = false

                    };
                #region Select

                var CorpList_ = GetDictListByDDName(DataDic.CorpName);
                var CorpList = Dept != null ? CorpList_.FindAll(f => (bool)Dept?.DpFullName.Contains(f.DDText)) : new List<Data.Domain.SysManger.DataDictionary>();
                if (CorpList.Count == 0)
                    CorpList = CorpList_.FindAll(f => f.DDText == "市公司本部");
                var RoadList = GetDictListByDDName("路途类别");
                var CarTonnageList = GetDictListByDDName("吨位/人数");
                var result = new
                {
                    model.Allocator,
                    model.ApplyTitle,
                    model.AllotIntro,
                    model.AllotTime,
                    model.ApplyCant,
                    model.ApplyId,
                    model.Attach,
                    model.Boral,
                    model.CarIds,
                    model.CorpId,
                    CreateTime = Convert.ToDateTime(model.CreateTime).ToString("yyyy-MM-dd"),
                    StartTime = model.StartTime == null ? "" : Convert.ToDateTime(model.StartTime).ToString("yyyy-MM-dd HH:mm"),
                    EndTime = model.EndTime == null ? "" : Convert.ToDateTime(model.EndTime).ToString("yyyy-MM-dd HH:mm"),
                    model.DeptName,
                    model.Starting,
                    model.Destination1,
                    model.Destination2,
                    model.Destination3,
                    model.Destination4,
                    model.Destination5,
                    model.Driver,

                    model.FinishTime,
                    model.KmCount,
                    model.KmNum1,
                    model.KmNum2,
                    model.Leader,
                    model.Mobile,

                    model.PersonCount,
                    model.Remark,
                    model.Request,
                    model.Road,
                    model.SpecialReason,
                    model.TimeOut,
                    model.UptTime,
                    model.UptUser,
                    model.UseType,
                    model.CarTonnage,
                    //CorpName = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == model.CorpId.ToString()).DDText,
                    model.WorkflowInstanceId
                };
                #endregion
                return Json(new { Items = result, CorpList = CorpList, RoadList = RoadList, CarTonnageList = CarTonnageList });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult Delete(Guid[] ids)
        {
            var models = carApplyService.List().Where(f => ids.Contains(f.ApplyId)).ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "记录不存在！";
                return Json(result);
            }
            if (carApplyService.DeleteByList(models))
                result.IsSuccess = true;

            return Json(result);
        }

        public ActionResult Save(CarApply carApply, bool isAction = false, string nextActivity = "", string nextActors = "", string nextCC = null)
        {
            CarApply _carApply;
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (carApply.ApplyId.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                _carApply = carApply;
                _carApply.ApplyId = Guid.NewGuid();
                if (carApplyService.Insert(_carApply))
                    result.IsSuccess = true;
            }
            else
            {
                _carApply = carApplyService.FindById(carApply.ApplyId);
                if (_carApply == null)
                {
                    result.Message = "更新对象不存在！";
                    return Json(result);
                }
                _carApply.Mobile = carApply.Mobile;
                _carApply.ApplyTitle = carApply.ApplyTitle;
                _carApply.CorpId = carApply.CorpId;
                _carApply.Driver = carApply.Driver;
                _carApply.DriverIds = carApply.DriverIds;
                _carApply.StartTime = carApply.StartTime;
                _carApply.EndTime = carApply.EndTime;
                _carApply.PersonCount = carApply.PersonCount;
                _carApply.UseType = carApply.UseType;
                _carApply.Road = carApply.Road;
                _carApply.Remark = carApply.Remark;
                _carApply.Destination1 = carApply.Destination1;
                _carApply.Destination2 = carApply.Destination2;
                _carApply.Destination3 = carApply.Destination3;
                _carApply.Destination4 = carApply.Destination4;
                _carApply.Destination5 = carApply.Destination5;
                _carApply.CarTonnage = carApply.CarTonnage;
                if (carApplyService.Update(_carApply))
                    result.IsSuccess = true;
            }
            if (isAction && result.IsSuccess)
            {
                string workflowType = FlowInstance.WorkflowType.UsedCarsApply;
                switch (_carApply.ApplyType)
                {
                    case 0:
                        workflowType = FlowInstance.WorkflowType.UsedCarsApply;
                        break;
                    case 1:
                        workflowType = FlowInstance.WorkflowType.PaidCarServiceApply;
                        break;
                    case 2:
                        workflowType = FlowInstance.WorkflowType.UrgentUsedCarsApply;
                        break;
                    default:
                        break;
                }
                result.IsSuccess = false;
                Dictionary<string, string> dictField = new Dictionary<string, string>();
                dictField.Add("F1", _carApply.ApplyId.ToString());
                var workFlowResult = WorkFlowHelper.ActionFlow(WorkContext.CurrentUser.UserName, _carApply.ApplyTitle, dictField, nextActivity, nextActors, workflowType);
                if (workFlowResult.Success > 0)
                {
                    _carApply.WorkflowInstanceId = workFlowResult.WorkFlow.WorkflowInstanceId;
                    _carApply.ApplySn = workFlowResult.WorkFlow.SheetId;
                    if (carApplyService.Update(_carApply))
                    {
                        result.IsSuccess = true;
                    }

                    //提交成功后抄送
                    if (!string.IsNullOrEmpty(nextCC))
                        CommonFunction.PendingData(workFlowResult.WorkFlow.WorkflowInstanceId, nextCC);//抄送
                }
                else
                    result.Message += workFlowResult?.Errmsg;

            }
            return Json(result);
        }

        #region 方法
        string GetType(int type = 0)
        {
            //
            string result = "的用车申请({0})";
            switch (type)
            {
                case 0:
                    result = "的用车申请({0})";
                    break;
                case 1:
                    result = "员工有偿服务用车({0})";
                    break;
                case 2:
                    result = "紧急用车申请({0})";
                    break;
                default:
                    break;
            }

            return string.Format(result, DateTime.Now.ToString("yyyy-MM-dd"));
        }
        #endregion

        #region 用车报表
        public ActionResult GeServiceReport(int pageIndex = 1, int pageSize = 5, CarsReportBuilder queryBuilder = null)
        {
            int count = 0;
            var source = carApplyService.List().Where(w => 1 == 1);
            if (queryBuilder.ApplyType != -1)
            {
                source = source.Where(w => w.ApplyType == queryBuilder.ApplyType);
            }
            if (queryBuilder.StartTime != null)
            {
                source = source.Where(w => w.StartTime >= queryBuilder.StartTime);
            }
            if (queryBuilder.TimeOut != null)
            {
                source = source.Where(w => w.TimeOut >= queryBuilder.TimeOut);
            }

            var pageList = new PagedList<CarApply>().QueryPagedList(source.OrderByDescending(o => o.ApplyId), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            return Json(new { items = pageList, count = count });
        }
        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<CarsReportBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                //int count = 0;
                //var modelList = carApplyService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();
                int count = 0;
                var source = carApplyService.List().Where(w => 1 == 1);
                if (QueryBuilder.ApplyType != -1)
                {
                    source = source.Where(w => w.ApplyType == QueryBuilder.ApplyType);
                }
                if (QueryBuilder.StartTime != null)
                {
                    source = source.Where(w => w.StartTime >= QueryBuilder.StartTime);
                }
                if (QueryBuilder.TimeOut != null)
                {
                    source = source.Where(w => w.TimeOut >= QueryBuilder.TimeOut);
                }

                var pageList = new PagedList<CarApply>().QueryPagedList(source.OrderByDescending(o => o.ApplyId), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count).ToList().Select(
                s => new
                {
                    s.Driver,
                    StartTime= Convert.ToDateTime(s.StartTime).ToString("yyyy-MM-dd"),
                    TimeOut= Convert.ToDateTime(s.TimeOut).ToString("yyyy-MM-dd"),
                    s.CarTonnage,
                    s.Road,
                    s.UseType,
                    s.Starting,
                    s.Destination1,
                    s.Destination2,
                    s.AllotIntro,
                    s.Remark
                }).ToList<object>();
                if (pageList.Count <= 0)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.CarsReport + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.CarsReport);
                //设置集合变量
                designer.SetDataSource(ImportFileType.CarsReport, pageList);
                //根据数据源处理生成报表内容
                designer.Process();
                //designer.Save(path, FileFormatType.Excel2003);
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
    }
}