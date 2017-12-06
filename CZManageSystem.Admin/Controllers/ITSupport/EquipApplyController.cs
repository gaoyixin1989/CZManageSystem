using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Composite;
using CZManageSystem.Service.ITSupport;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Composite
{
    public class EquipApplyController : BaseController
    {
        IEquipApplyService _sysEquipApplyService = new EquipApplyService();
        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();
        IStockService _sysStockService = new StockService();
        IStockAssetService _sysStockAssetService = new StockAssetService();
        // GET: EquipApply
        public ActionResult Apply(string ApplyId,string startDeptId)
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.EquipApply;
            ViewData["ApplyId"] = ApplyId;
            ViewData["startDeptId"] = this.WorkContext.CurrentUser.DpId;
            if (string.IsNullOrEmpty(ApplyId))
                ViewBag.Title = "设备申请";
            else
                ViewBag.Title = "设备查看";
            return View();
        }
        public ActionResult ApplyList()
        {

            return View();
        }
        public ActionResult MyApplyList()
        {

            return View();
        }

        public ActionResult ApplyDetail(string ApplyId)
        {
            ViewData["ApplyId"] = ApplyId;
            return View();
        }
        public ActionResult ViewApply(string ApplyId)
        {
            ViewData["ApplyId"] = ApplyId;
            return View();
        }
        #region 获取信息
        /// <summary>
        /// 获取申请人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult getNewApplyInfo()
        {
            object AppInfo = new object();
            if (this.WorkContext.CurrentUser != null)
            {
                AppInfo = new
                {
                    ApplyNameId = this.WorkContext.CurrentUser.UserId.ToString(),
                    ApplyName = this.WorkContext.CurrentUser.RealName,
                    ApplyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    DeptnameId = this.WorkContext.CurrentUser.DpId,
                    Deptname = getDeptNamesByIDs(this.WorkContext.CurrentUser.DpId),
                    Job = getJob(this.WorkContext.CurrentUser.DpId),
                    Tel = this.WorkContext.CurrentUser.Mobile,
                    ApplyTitle = this.WorkContext.CurrentUser.RealName + "的办公设备申请(" + DateTime.Now.ToString("yyyy-MM-dd") + ")",//
                    ApplySn = "流程单号待生成",
                    Chief= "陈华元、陈伟彬"
                };
            }
            return Json(AppInfo);
        }
        /// <summary>
        /// 获取岗位职级
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private string getJob(string ids)
        {
            string strResult = "";
            string job = "";
            if (!string.IsNullOrEmpty(ids))
            {
                IVw_cz_uum_LeadersService _vw_cz_uum_LeadersService = new Vw_cz_uum_LeadersService();
                string[] arrID = ids.Split(',');
                var mm = _vw_cz_uum_LeadersService.List().Where(u => arrID.Contains(u.DpId)).Select(u => u.EmployeeLevel).ToList();
                strResult = string.Join(",", mm);
                var joblist= this.GetDictListByDDName("用户职位").Where(s => s.DDValue.Contains(strResult)).Select(s=>s.DDText).ToList();
                job = string.Join(",", joblist);
            }
            return job;
        }
        private string getDeptNamesByIDs(string ids)
        {
            string strResult = "";
            if (!string.IsNullOrEmpty(ids))
            {
                ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
                string[] arrID = ids.Split(',');
                var mm = _sysDeptmentService.List().Where(u => arrID.Contains(u.DpId)).Select(u => u.DpName).ToList();
                strResult = string.Join(",", mm);
              
            }
            return strResult;
        }

       
        #endregion
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public ActionResult EquipAppSave(EquipApp app)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            app.EditTime = DateTime.Now;
            app.Editor = this.WorkContext.CurrentUser.UserName;
            if (string.IsNullOrEmpty(app.ApplyId) || app.ApplyId == "00000000-0000-0000-0000-000000000000")//新增
            {
                app.ApplyId = Guid.NewGuid().ToString();
                app.ApplyTime = DateTime.Now;
                app.ApplyName = this.WorkContext.CurrentUser.RealName;
                app.Status = 0;
                result.IsSuccess = _sysEquipApplyService.Insert(app);
                result.Message = app.ApplyId;
            }
            else
            {//编辑
                result.IsSuccess = _sysEquipApplyService.Update(app);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            else
            {
                result.Message = app.ApplyId.ToString();
            }
            return Json(result);


        }
        public ActionResult ApplyDelete(string[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            string strMsg = "";
            var objs = _sysEquipApplyService.List().Where(u => ids.Contains(u.ApplyId)).ToList();
            if (objs.Where(u => u.Status != 0).Count() > 0)
            {
                strMsg = "其中存在已提交的记录，不能删除";
            }
            else if (objs.Count > 0)
            {
                isSuccess = _sysEquipApplyService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });
           
        }
       

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public ActionResult SubmitApply(string appid, /*int assid,*/ string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (string.IsNullOrEmpty(appid.ToString()) || appid.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                result.IsSuccess = false;
                result.Message = "该数据不存在";
                return Json(result);
            }
            var app = _sysEquipApplyService.List().Where(t => t.ApplyId == appid).ToList()[0];
            ////更改固定资产编码状态
            //StockAsset sa = _sysStockAssetService.List().Where(s => s.AssetSn == app.AssetSn && s.Id == assid).ToList()[0];
            //if (sa != null)
            //{
            //    sa.State = 1;
            //    _sysStockAssetService.Update(sa);
            //}


            result = EquipApply_Sumbit(app, nextActivity, nextActors);

            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                app = _sysEquipApplyService.FindByFeldName(t => t.ApplyId == appid);
                CommonFunction.PendingData(app.WorkflowInstanceId.Value, nextCC);//抄送
            }

            return Json(result);
        }
        public SystemResult EquipApply_Sumbit(EquipApp apply, string nextActivity = null, string nextActors = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.BoardroomApply, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
                if (!string.IsNullOrEmpty(nextActors))
                    nextActors = nextActors.Split(',')[0];
            }
            string objectXML = "";
            if (string.IsNullOrEmpty(apply.WorkflowInstanceId.ToString()) || apply.WorkflowInstanceId.ToString() == "00000000-0000-0000-0000-000000000000")
            {//第一次提交
                objectXML = "<Root action=\"Start\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"workflowId\" value=\"{1}\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"2900-01-01\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.EquipApply, apply.ApplyTitle, apply.ApplyId, nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));
            }
            else
            {//退回提交
                ITracking_TodoService tempService = new Tracking_TodoService();
                Tracking_Todo tempActivity = new Tracking_Todo();
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == apply.WorkflowInstanceId).FirstOrDefault();
                objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                    + "<item name=\"command\" value=\"Approve\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), apply.ApplyTitle, apply.ApplyId, nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));
            }
            string[] args = new string[4];
            args[0] = FlowInstance.Workflow_SystemID;
            args[1] = FlowInstance.Workflow_SystemAcount;
            args[2] = FlowInstance.Workflow_SystemPwd;
            args[3] = objectXML;

            string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.ManageWorkflow, args).ToString();
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(resultXml);
            System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
            string success = resutNode.Attributes["Success"].Value;
            string errmsg = resutNode.Attributes["ErrorMsg"].Value;
            string strWorkflowInstanceId = "";
            string strActivityinstanceId = "";
            int intSuccess = 0;
            int.TryParse(success, out intSuccess);
            if (intSuccess > 0)
            {
                if (string.IsNullOrEmpty(apply.WorkflowInstanceId.ToString()) || apply.WorkflowInstanceId.ToString() == "00000000-0000-0000-0000-000000000000")
                {//第一次提交
                    System.Xml.XmlNodeList xmlList = xdoc.SelectNodes("Result/item/start/item");
                    for (int i = 0; i < xmlList.Count; i++)
                    {
                        switch (xmlList[i].Attributes["name"].Value)
                        {
                            case "WorkflowInstanceId": strWorkflowInstanceId = xmlList[i].Attributes["value"].Value; break;
                            case "ActivityinstanceId": strActivityinstanceId = xmlList[i].Attributes["value"].Value; break;
                            default: break;
                        }
                    }

                    var workFlow = _tracking_WorkflowService.FindById(Guid.Parse(strWorkflowInstanceId));
                    apply.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);
                    apply.ApplySn = workFlow.SheetId;
                    apply.Status = 1;
                    result.IsSuccess = _sysEquipApplyService.Update(apply);
                }
                else
                {//退回提交
                    result.IsSuccess = true;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = errmsg;
            }
            return result;
        }

        public ActionResult GetApplyID(string ApplyId)
        {
            var obj = _sysEquipApplyService.List().Where(a => a.ApplyId == ApplyId).ToList();
            return Json(obj);

        }
        /// <summary>
        /// 获取申请列表
        /// </summary>
        /// <param name="ApplyTitle"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult ApplyGetList(string ApplyTitle, int pageIndex = 1, int pageSize = 5)
        {
            int count = 0;
            var modelList = _sysEquipApplyService.GetApplyList(out count, ApplyTitle, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            return Json(new { items = modelList, count = count });
        }

        public ActionResult GetMyApplyList()
        {
            var obj = _sysEquipApplyService.List().Where(a => a.ApplyName == this.WorkContext.CurrentUser.RealName).ToList();
            return Json(obj);

        }
        /// <summary>
        /// 获取资产编码
        /// </summary>
        /// <param name="EquipClass"></param>
        /// <returns></returns>
        public ActionResult GetAssetSn(string EquipClass)
        {
            var obj = _sysEquipApplyService.GetAssetSn(EquipClass);
            return Json(obj);
        }

        /// <summary>
        /// 取消操作——普通用户
        /// </summary>
        public ActionResult CancelEquipApply_user(string[] ids, string reason)
        {
            var objs = _sysEquipApplyService.List().Where(u => ids.Contains(u.ApplyId)).ToList();
            int successCount = 0;
            int errorCount = 0;

            if (objs.Count > 0)
            {
                SystemResult sysR = new SystemResult();
                foreach (var item in objs)
                {
                    item.Status = -1;
                    item.CancleReason = "申请人取消";
                    if (!string.IsNullOrEmpty(reason)) item.CancleReason += ":" + reason;
                    item.Editor = this.WorkContext.CurrentUser.UserName;
                    sysR = EquipApply_Cancel(item);
                    if (sysR.IsSuccess)
                    {
                        _sysEquipApplyService.Update(item);
                        successCount++;
                    }
                    else
                        errorCount++;
                }
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false
            });
        }
        /// <summary>
        /// 取消办公设备申请工单
        /// </summary>
        /// <returns></returns>
        public SystemResult EquipApply_Cancel(EquipApp curData)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            ITracking_TodoService tempService = new Tracking_TodoService();
            Tracking_Todo tempActivity = new Tracking_Todo();
            tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == curData.WorkflowInstanceId).FirstOrDefault();
            string objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                            + "<parameter>"
                                + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                + "<item name=\"command\" value=\"cancel\"/>"
                                + "<item name=\"workflowProperties\">"
                                    + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{3}\">"
                                    //+ "<nextactivities>"
                                    //    + "<item name=\"{3}\" actors=\"{4}\"/>"
                                    //+ "</nextactivities>"
                                    + "</workflow>"
                                + "</item>"
                                + "<item name = \"Content\" value = \"{2}\" />"
                               + "</parameter>"
                        + "</Root>";
            objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.CancleReason, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));// ,

            string[] args = new string[4];
            args[0] = FlowInstance.Workflow_SystemID;
            args[1] = FlowInstance.Workflow_SystemAcount;
            args[2] = FlowInstance.Workflow_SystemPwd;
            args[3] = objectXML;

            string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.ManageWorkflow, args).ToString();

            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(resultXml);
            System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
            string success = resutNode.Attributes["Success"].Value;
            string errmsg = resutNode.Attributes["ErrorMsg"].Value;
            int intSuccess = 0;
            int.TryParse(success, out intSuccess);
            if (intSuccess > 0)
            {
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = errmsg;
            }
            return result;
        }

        /// <summary>
        /// 取消操作——管理者
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="reason">取消原因</param>
        /// <returns></returns>
        public ActionResult CancelBoardroomApply_manager(string[] ids, string reason)
        {
            int successCount = 0;
            int errorCount = 0;
            var objs = _sysEquipApplyService.List().Where(u => ids.Contains(u.ApplyId)).ToList();
            if (objs.Count > 0)
            {
                SystemResult sysR = new SystemResult();
                foreach (var item in objs)
                {
                    item.Status = -1;
                    item.CancleReason = "管理员取消";
                    if (!string.IsNullOrEmpty(reason)) item.CancleReason += ":" + reason;
                    item.Editor = this.WorkContext.CurrentUser.UserName;
                    sysR = EquipApply_Cancel(item); if (sysR.IsSuccess)
                    {
                        _sysEquipApplyService.Update(item);
                        successCount++;
                    }
                    else
                        errorCount++;
                }
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false
            });
        }

    }
}