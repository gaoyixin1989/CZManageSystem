using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Composite;
using CZManageSystem.Service.HumanResources.Vacation;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 销假申请
/// </summary>
namespace CZManageSystem.Admin.Controllers.HumanResources.Vacation
{
    public class VacationCloseApplyController : BaseController
    {
        IAdmin_AttachmentService _attchmentService = new Admin_AttachmentService();//附件
        IHRVacationCloseApplyService _applyService = new HRVacationCloseApplyService();//休假申请
        IHRVacationApplyService _VapplyService = new HRVacationApplyService();//休假申请
        IHRVacationMeetingService _meetingService = new HRVacationMeetingService();//假期会议
        IHRVacationCoursesService _coursesService = new HRVacationCoursesService();//假期培训
        IHRVacationTeachingService _teachingService = new HRVacationTeachingService();//内部讲师授课
        IHRVacationOtherService _otherService = new HRVacationOtherService();//其他休假

        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();//流程

        // GET: VacationCloseApply
        /// <summary>
        /// 休假申请完成列表
        /// </summary>
        /// <returns></returns>
        public ActionResult VacationApplyList()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        //休假申请编辑页
        public ActionResult Edit(Guid? ApplyID, Guid? VacationID, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.VacationCloseApply;
            ViewData["type"] = type;
            ViewData["ApplyID"] = ApplyID.HasValue ? ApplyID.ToString() : null;
            ViewData["VacationID"] = VacationID.HasValue ? VacationID.ToString() : null;
            ViewData["RealName"] = this.WorkContext.CurrentUser.RealName;
            ViewData["DeptName"] = this.WorkContext.CurrentUser.Dept.DpName;
            ViewData["Mobile"] = this.WorkContext.CurrentUser.Mobile;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + "的销假申请" + DateTime.Now.ToString("(yyyy-MM-dd)");
            ViewData["CurDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["AthId"] = Guid.NewGuid().ToString();
            return View();
        }

        //休假申请信息——流程中使用
        public ActionResult ApplyInfo_WF(Guid? ApplyID)
        {
            ViewData["ApplyID"] = ApplyID.HasValue ? ApplyID.ToString() : null;
            return View();
        }


        /// <summary>
        /// 获取休假申请信息列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <param name="isManager">是否管理员，默认否</param>
        /// <param name="hasPass">是否已经流程通过，默认无该条件</param>
        /// <returns></returns>
        public ActionResult GetListData_VacationApply(int pageIndex = 1, int pageSize = 5, VacationApplyQueryBuilder queryBuilder = null, bool isManager = false, bool? hasPass = null)
        {
            if (!isManager)
            {
                queryBuilder.Editor = this.WorkContext.CurrentUser.UserId;
            }

            if (hasPass.HasValue)
            {
                queryBuilder.WF_State = new List<int>();
                if (hasPass == true)
                    queryBuilder.WF_State.Add(2);
                else
                {
                    queryBuilder.WF_State.Add(0);
                    queryBuilder.WF_State.Add(1);
                    queryBuilder.WF_State.Add(99);

                }
            }

            int count = 0;
            var modelList = _VapplyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (HRVacationApply)u).ToList();

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    CloseID = _applyService.FindByFeldName(u => u.VacationID == item.ApplyId)?.ApplyId,
                    item.ApplyId,
                    item.WorkflowInstanceId,
                    item.ApplySn,
                    item.ApplyTitle,
                    item.Editor,
                    Editor_Text = item.EditorObj.RealName,
                    item.EditTime,
                    item.VacationType,
                    item.VacationClass,
                    item.StartTime,
                    item.EndTime,
                    item.PeriodTime,
                    item.Reason,
                    item.OutAddress,
                    item.OverTime,
                    item.Attids,
                    item.State,
                    WF_StateText = CommonFunction.GetFlowStateText(item.Tracking_Workflow),
                    WF_CurActivityName = _tracking_WorkflowService.GetCurrentActivityNames(item.WorkflowInstanceId ?? Guid.Parse("00000000-0000-0000-0000-000000000000")).FirstOrDefault()
                    //WF_LastActivityInfo = CommonFunction.GetActivitiesCompletedByID(item.WorkflowInstanceId).Select(u => new
                    //{
                    //    u.Activities.ActivityName,
                    //    u.ActorDescription,
                    //    u.FinishedTime,
                    //    u.Reason,
                    //    u.Command,
                    //    CommandText = CommonFunction.GetActivitieResultText(u.Command)
                    //}).LastOrDefault()
                });
            }

            return Json(new { items = listResult, count = count }, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 获取销假申请信息列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <param name="isManager">是否管理员，默认否</param>
        /// <param name="hasPass">是否已经流程通过，默认无该条件</param>
        /// <returns></returns>
        public ActionResult GetListData_Apply(int pageIndex = 1, int pageSize = 5, ReVacationApplyQueryBuilder queryBuilder = null, bool isManager = false, bool? hasPass = null)
        {
            if (!isManager)
            {
                queryBuilder.Editor = this.WorkContext.CurrentUser.UserId;
            }

            if (hasPass.HasValue)
            {
                queryBuilder.WF_State = new List<int>();
                if (hasPass == true)
                    queryBuilder.WF_State.Add(2);
                else
                {
                    queryBuilder.WF_State.Add(0);
                    queryBuilder.WF_State.Add(1);
                    queryBuilder.WF_State.Add(99);
                }
            }

            int count = 0;
            var modelList = _applyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (HRVacationCloseApply)u).ToList();

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.ApplyId,
                    item.WorkflowInstanceId,
                    item.ApplySn,
                    item.ApplyTitle,
                    item.Editor,
                    Editor_Text = item.EditorObj.RealName,
                    item.EditTime,
                    item.VacationType,
                    item.VacationClass,
                    item.ClosedDays,
                    item.Reason,
                    item.VacationID,
                    item.Note,
                    item.Factst,
                    item.Factet,
                    item.Factdays,
                    item.State,
                    WF_StateText = CommonFunction.GetFlowStateText(item.Tracking_Workflow),
                    WF_CurActivityName = _tracking_WorkflowService.GetCurrentActivityNames(item.WorkflowInstanceId ?? Guid.Parse("00000000-0000-0000-0000-000000000000")).FirstOrDefault()
                    //WF_LastActivityInfo = CommonFunction.GetActivitiesCompletedByID(item.WorkflowInstanceId).Select(u => new
                    //{
                    //    u.Activities.ActivityName,
                    //    u.ActorDescription,
                    //    u.FinishedTime,
                    //    u.Reason,
                    //    u.Command,
                    //    CommandText = CommonFunction.GetActivitieResultText(u.Command)
                    //}).LastOrDefault()
                });
            }

            return Json(new { items = listResult, count = count }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 根据ID获取销假申请单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID_Apply(Guid? id, Guid? vacationID)
        {
            object Apply = new { };
            object Vacation = new { };
            HRVacationCloseApply item = new HRVacationCloseApply();
            if (id.HasValue && id != Guid.Empty)
            {
                item = _applyService.FindById(id);
                Apply = new
                {
                    item.ApplyId,
                    item.WorkflowInstanceId,
                    item.ApplySn,
                    item.ApplyTitle,
                    item.Editor,
                    Editor_Text = item.EditorObj.RealName,
                    DeptName = item.EditorObj.Dept.DpName,
                    Mobile = item.EditorObj.Mobile,
                    item.EditTime,
                    item.VacationType,
                    item.VacationClass,
                    item.ClosedDays,
                    item.Reason,
                    item.VacationID,
                    item.Note,
                    item.Factst,
                    item.Factet,
                    item.Factdays,
                    item.State
                };
            }

            if (item != null && item.ApplyId != Guid.Empty)
                vacationID = item.VacationID;
            var vacation = _VapplyService.FindById(vacationID);
            Vacation = new
            {
                vacation.ApplyId,
                vacation.WorkflowInstanceId,
                vacation.ApplySn,
                vacation.ApplyTitle,
                vacation.Editor,
                Editor_Text = vacation.EditorObj.RealName,
                DeptName = vacation.EditorObj.Dept.DpName,
                Mobile = vacation.EditorObj.Mobile,
                vacation.EditTime,
                vacation.VacationType,
                vacation.VacationClass,
                vacation.StartTime,
                vacation.EndTime,
                vacation.PeriodTime,
                vacation.Reason,
                vacation.CancelVacation,
                vacation.CancelDays,
                vacation.Newpt,
                vacation.Newst,
                vacation.Newet,
                vacation.OutAddress,
                vacation.OverTime,
                vacation.Attids,
                vacation.State
            };

            return Json(new
            {
                Apply = Apply,
                Vacation = Vacation,
                Meeting = _meetingService.FindByFeldName(u => u.VacationID == vacation.ApplyId),//公假-开会
                Courses = _coursesService.FindByFeldName(u => u.VacationID == vacation.ApplyId),//公假-培训
                Teaching = _teachingService.FindByFeldName(u => u.VacationID == vacation.ApplyId),//公假-内部讲师授课
                Other = _otherService.FindByFeldName(u => u.VacationID == vacation.ApplyId),//公假-其他
                Attachments = _attchmentService.GetAllAttachmentList(Guid.Parse(vacation.Attids))
            });
        }

        /// <summary>
        /// 删除销假申请信息
        /// </summary>
        /// <param name="IDs">退库单ID</param>
        /// <returns></returns>
        public ActionResult Delete_Applys(Guid[] IDs)
        {
            bool isSuccess = false;
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var applyDatas = _applyService.List().Where(u => IDs.Contains(u.ApplyId)).ToList();//申请信息
            allCount = applyDatas.Count;

            if (applyDatas.Where(u => u.State == 1).Count() > 0)
            {
                strMsg = "其中存在已提交的记录，不能删除";
            }
            else if (applyDatas.Count > 0)
            {
                if (_applyService.DeleteByList(applyDatas))
                {
                    successCount = applyDatas.Count;
                }
            }
            else
            {
                strMsg = "删除失败";
            }

            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });

        }


        /// <summary>
        /// 保存申请单
        /// </summary>
        /// <param name="Apply">申请单</param>
        /// <param name="Meeting">开会</param>
        /// <param name="Courses">培训</param>
        /// <param name="Teaching">授课</param>
        /// <param name="Other">其他</param>
        /// <returns></returns>
        public ActionResult Save_Apply(HRVacationCloseApply Apply)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (Apply.ApplyTitle == null || string.IsNullOrEmpty(Apply.ApplyTitle.Trim()))
                tip = "标题不能为空";
            else if (!Apply.Factst.HasValue)
                tip = "休假开始时间不能为空";
            else if (!Apply.Factet.HasValue)
                tip = "休假结束时间不能为空";
            else if (Apply.Factst.Value > Apply.Factet.Value)
                tip = "休假开始时间不能大于结束时间";
            else if ((Apply.ClosedDays ?? 0) <= 0)
                tip = "销假天数不能为空或小于0";
            else if (Apply.Note == null || string.IsNullOrEmpty(Apply.Note.Trim()))
                tip = "休假事由不能为空";
            else if (Apply.VacationID == null || Apply.VacationID == Guid.Empty)
                tip = "没有对应的休假申请ID";

            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                Apply.ApplyTitle = Apply.ApplyTitle.Trim();
            }
            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (Apply.ApplyId == Guid.Empty)
            {//新增
                Apply.ApplyId = Guid.NewGuid();
                Apply.State = 0;
                Apply.Editor = this.WorkContext.CurrentUser.UserId;

                result.IsSuccess = _applyService.Insert(Apply);
            }
            else
            {//编辑
                result.IsSuccess = _applyService.Update(Apply);
            }

            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            else
            {
                result.Message = Apply.ApplyId.ToString();
            }
            return Json(result);
        }

        //提交休假申请信息
        public ActionResult Sumbit_Apply(Guid applyID, string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };

            var curData = _applyService.FindById(applyID);
            if (curData == null || curData.ApplyId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = false;
                result.Message = "该申请单信息不存在";
                return Json(result);
            }

            result = Sumbit_AgoEstimateApply_WF(curData, nextActivity, nextActors);
            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                curData = _applyService.FindById(curData.ApplyId);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, nextCC);//抄送
            }

            return Json(result);
        }

        /// <summary>
        /// 提交申请
        /// </summary>
        public SystemResult Sumbit_AgoEstimateApply_WF(HRVacationCloseApply curData, string nextActivity, string nextActors)
        {
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.VacationCloseApply, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
            }

            SystemResult result = new SystemResult() { IsSuccess = false };
            string objectXML = "";

            if (!curData.WorkflowInstanceId.HasValue)
            {//第一次提交
                objectXML = "<Root action=\"Start\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"workflowId\" value=\"{1}\"/>"
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
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.VacationCloseApply, curData.ApplyTitle, curData.ApplyId.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }
            else
            {//退回提交
                ITracking_TodoService tempService = new Tracking_TodoService();
                Tracking_Todo tempActivity = new Tracking_Todo();
                Guid guid = curData.WorkflowInstanceId.Value;
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == guid).FirstOrDefault();//当前节点实例
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
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.ApplyTitle, curData.ApplyId.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

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
                if (!curData.WorkflowInstanceId.HasValue)
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
                    curData.State = 1;
                    curData.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);
                    curData.ApplySn = workFlow.SheetId;
                    curData.EditTime = DateTime.Now;

                    result.IsSuccess = _applyService.Update(curData);
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



    }
}