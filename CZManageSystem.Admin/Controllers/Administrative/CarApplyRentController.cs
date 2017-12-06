using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Service.Administrative.VehicleManages;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class CarApplyRentController : BaseController
    {
        ICarApplyRentService carApplyRentService = new CarApplyRentService();
        ISysDeptmentService sysDeptmentService = new SysDeptmentService();
        // GET: CarApplyRent
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(string id = null, string type=null)
        {
            //有值=》编辑状态
            ViewData["id"] = !string.IsNullOrEmpty(id) ? id : "0";
            ViewData["type"] = type;
            ViewData["workflowName"] = Base.FlowInstance.WorkflowType.CarApplyRent;
            return View();
        }
        public ActionResult ForWF(string id = null, string type = null)
        {
            //有值=》编辑状态
            ViewData["id"] = !string.IsNullOrEmpty(id) ? id : "0";
            ViewData["type"] = type;
            ViewData["workflowName"] = Base.FlowInstance.WorkflowType.CarApplyRent;
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
            int count = 0;
            var modelList = carApplyRentService.GetForPaging(out count,new { ApplyTitle }, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            var items = modelList;
            return Json(new { items = items, count = count });
        }
        public ActionResult Delete(Guid[] ids)
        {
            var models = carApplyRentService.List().Where(f => ids.Contains(f.ApplyRentId)).ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "记录不存在！";
                return Json(result);
            }
            if (carApplyRentService.DeleteByList(models))
                result.IsSuccess = true;

            return Json(result);
        }
        public ActionResult GetDataByID(string id, string type = null)
        {
            CarApplyRent model;
            var Dept = sysDeptmentService.FindById(WorkContext.CurrentUser.DpId);
            if (!string.IsNullOrEmpty(id) && id != "0")
                model = carApplyRentService.FindById(new Guid(id));
            else
                model = new CarApplyRent()
                {
                    ApplyTitle = string.Format(WorkContext.CurrentUser.RealName + "的租车申请({0})", DateTime.Now.ToString("yyyy-MM-dd")),
                    Mobile = WorkContext.CurrentUser.Mobile,
                    CreateTime = DateTime.Now,
                    DeptName = Dept?.DpName,
                    ApplyCant = WorkContext.CurrentUser.RealName,
                    Driver = ""

                };
            #region Select

            var CorpList_ = GetDictListByDDName(DataDic.CorpName);
            var CorpList = Dept != null ? CorpList_.FindAll(f => (bool)Dept?.DpFullName.Contains(f.DDText)) : new List<Data.Domain.SysManger.DataDictionary>();
            if (CorpList.Count == 0)
                CorpList = CorpList_.FindAll(f => f.DDText == "市公司本部");
            var RoadList = GetDictListByDDName("路途类别");
            var CarTonnageList = GetDictListByDDName("吨位/人数");
            var HtypeList = GetDictListByDDName("车辆类型");
            var result = new
            {
                model.ApplyRentId,
                model.CorpId,
                CreateTime = Convert.ToDateTime(model.CreateTime).ToString("yyyy-MM-dd"),
                model.DeptName,
                model.ApplyCant,
                model.Driver,
                model.TimeOut,
                StartTime = model.StartTime == null ? "" : Convert.ToDateTime(model.StartTime).ToString("yyyy-MM-dd HH:mm"),
                EndTime = model.EndTime == null ? "" : Convert.ToDateTime(model.EndTime).ToString("yyyy-MM-dd HH:mm"),
                model.Starting,
                model.Destination1,
                model.Destination2,
                model.Destination3,
                model.Destination4,
                model.Destination5,
                model.PersonCount,
                model.Road,
                model.UseType,
                model.Htype,

                model.Allocator,
                model.ApplyTitle,
                model.AllotTime,              
                model.Attach,
                model.Mobile,              
                model.Remark,
                model.Request,
             
                model.CarTonnage,
                model.WorkflowInstanceId
            };
            #endregion
            return Json(new { Items = result, CorpList = CorpList, RoadList = RoadList, CarTonnageList = CarTonnageList, HtypeList= HtypeList });
        }

        public ActionResult Save(CarApplyRent carApplyRent, bool isAction = false, string nextActivity = "", string nextActors = "", string nextCC = null)
        {
            CarApplyRent _carApplyRent;
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (carApplyRent.ApplyRentId.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                _carApplyRent = carApplyRent;
                _carApplyRent.ApplyRentId = Guid.NewGuid();
                if (carApplyRentService.Insert(_carApplyRent))
                    result.IsSuccess = true;
            }
            else
            {
                _carApplyRent = carApplyRentService.FindById(carApplyRent.ApplyRentId);
                if (_carApplyRent == null)
                {
                    result.Message = "更新对象不存在！";
                    return Json(result);
                }
                _carApplyRent.Mobile = carApplyRent.Mobile;
                _carApplyRent.ApplyTitle = carApplyRent.ApplyTitle;
                _carApplyRent.CorpId = carApplyRent.CorpId;
                _carApplyRent.Driver = carApplyRent.Driver;
                _carApplyRent.StartTime = carApplyRent.StartTime;
                _carApplyRent.EndTime = carApplyRent.EndTime;
                _carApplyRent.PersonCount = carApplyRent.PersonCount;
                _carApplyRent.UseType = carApplyRent.UseType;
                _carApplyRent.Road = carApplyRent.Road;
                _carApplyRent.Remark = carApplyRent.Remark;
                _carApplyRent.Destination1 = carApplyRent.Destination1;
                _carApplyRent.Destination2 = carApplyRent.Destination2;
                _carApplyRent.Destination3 = carApplyRent.Destination3;
                _carApplyRent.Destination4 = carApplyRent.Destination4;
                _carApplyRent.Destination5 = carApplyRent.Destination5;
                _carApplyRent.CarTonnage = carApplyRent.CarTonnage;

                if (carApplyRentService.Update(_carApplyRent))
                    result.IsSuccess = true;
            }
            if (isAction)
            {
                result.IsSuccess = false;
                Dictionary<string, string> dictField = new Dictionary<string, string>();
                dictField.Add("F1", _carApplyRent.ApplyRentId.ToString());
                var workFlowResult = WorkFlowHelper.ActionFlow(WorkContext.CurrentUser.UserName, _carApplyRent.ApplyTitle, dictField, nextActivity, nextActors, FlowInstance.WorkflowType.CarApplyRent);
                if (workFlowResult.Success > 0)
                {
                    _carApplyRent.WorkflowInstanceId = workFlowResult.WorkFlow.WorkflowInstanceId;
                    _carApplyRent.ApplySn = workFlowResult.WorkFlow.SheetId;
                    if (carApplyRentService.Update(_carApplyRent))
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
    }
}