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
/// 异常休假申请
/// </summary>
namespace CZManageSystem.Admin.Controllers.HumanResources.Vacation
{
    public class ReVacationApplyController : BaseController
    {
        IAdmin_AttachmentService _attchmentService = new Admin_AttachmentService();//附件
        IHRReVacationApplyService _applyService = new HRReVacationApplyService();//异常休假申请
        IHRVacationMeetingService _meetingService = new HRVacationMeetingService();//假期会议
        IHRVacationCoursesService _coursesService = new HRVacationCoursesService();//假期培训
        IHRVacationTeachingService _teachingService = new HRVacationTeachingService();//内部讲师授课
        IHRVacationOtherService _otherService = new HRVacationOtherService();//其他休假


        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();//流程
        // GET: 
        public ActionResult Index()
        {
            return View();
        }

        //休假申请编辑页
        public ActionResult Edit(Guid? ApplyID, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.ReVacationApply;
            ViewData["type"] = type;
            ViewData["ApplyID"] = ApplyID.HasValue ? ApplyID.ToString() : null;
            ViewData["RealName"] = this.WorkContext.CurrentUser.RealName;
            ViewData["DeptName"] = this.WorkContext.CurrentUser.Dept.DpName;
            ViewData["Mobile"] = this.WorkContext.CurrentUser.Mobile;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + "的异常休假申请" + DateTime.Now.ToString("(yyyy-MM-dd)");
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
        /// <returns></returns>
        public ActionResult GetListData_Apply(int pageIndex = 1, int pageSize = 5, ReVacationApplyQueryBuilder queryBuilder = null, bool isManager = false)
        {
            if (!isManager)
            {
                queryBuilder.Editor = this.WorkContext.CurrentUser.UserId;
            }
            int count = 0;
            var modelList = _applyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (HRReVacationApply)u).ToList();

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
                    item.StartTime,
                    item.EndTime,
                    item.PeriodTime,
                    item.Reason,
                    item.Boral,
                    item.Leader,
                    item.ReApplyReason,
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
        /// 根据ID获取休假申请单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID_Apply(Guid id)
        {
            var item = _applyService.FindById(id);
            object Apply = new
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
                item.StartTime,
                item.EndTime,
                item.PeriodTime,
                item.Reason,
                item.Boral,
                item.Leader,
                item.ReApplyReason,
                item.Attids,
                item.State,
            };
            return Json(new
            {
                Apply = Apply,
                Meeting = _meetingService.FindByFeldName(u => u.ReVacationID == item.ApplyId),//公假-开会
                Courses = _coursesService.FindByFeldName(u => u.ReVacationID == item.ApplyId),//公假-培训
                Teaching = _teachingService.FindByFeldName(u => u.ReVacationID == item.ApplyId),//公假-内部讲师授课
                Other = _otherService.FindByFeldName(u => u.ReVacationID == item.ApplyId),//公假-其他
                Attachments = _attchmentService.GetAllAttachmentList(Guid.Parse(item.Attids))
            });
        }

        /// <summary>
        /// 删除休假申请信息
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
        public ActionResult Save_Apply(HRReVacationApply Apply, HRVacationMeeting Meeting, HRVacationCourses Courses, HRVacationTeaching Teaching, HRVacationOther Other)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (Apply.ApplyTitle == null || string.IsNullOrEmpty(Apply.ApplyTitle.Trim()))
                tip = "标题不能为空";
            else if (Apply.VacationType == null || string.IsNullOrEmpty(Apply.VacationType.Trim()))
                tip = "休假类型不能为空";
            else if (!Apply.StartTime.HasValue)
                tip = "休假开始时间不能为空";
            else if (!Apply.EndTime.HasValue)
                tip = "休假结束时间不能为空";
            else if (Apply.StartTime.Value > Apply.EndTime.Value)
                tip = "休假开始时间不能大于结束时间";
            else if ((Apply.PeriodTime ?? 0) <= 0)
                tip = "休假天数不能为空或小于0";
            else if (Apply.Reason == null || string.IsNullOrEmpty(Apply.Reason.Trim()))
                tip = "休假事由不能为空";
            else if (!Apply.Boral.HasValue)
                tip = "是否已口头申请不能为空";
            else if ((Apply.Boral ?? false) == true && (Apply.Leader == null || string.IsNullOrEmpty(Apply.Leader)))
                tip = "口头申请领导人不能为空";
            //else if (Apply.VacationType == "年休假")
            //{
            //}
            else if (Apply.VacationType == "公假")
            {
                #region 公假
                if (Apply.VacationClass == null || string.IsNullOrEmpty(Apply.VacationClass.Trim()))
                    tip = "公假类型不能为空";
                else if (Apply.VacationClass == "开会")
                {
                    if (!Meeting.StartTime.HasValue)
                        tip = "会议开始时间不能为空";
                    else if (!Meeting.EndTime.HasValue)
                        tip = "会议结束时间不能为空";
                    else if (Meeting.StartTime.Value > Meeting.EndTime.Value)
                        tip = "会议开始时间不能大于结束时间";
                    else if ((Meeting.PeriodTime ?? 0) <= 0)
                        tip = "会议天数不能为空或小于0";
                    else if (Meeting.MeetingName == null || string.IsNullOrEmpty(Meeting.MeetingName.Trim()))
                        tip = "会议名称不能为空";
                }
                else if (Apply.VacationClass == "培训")
                {
                    if (!Courses.StartTime.HasValue)
                        tip = "培训开始时间不能为空";
                    else if (!Courses.EndTime.HasValue)
                        tip = "培训结束时间不能为空";
                    else if (Courses.StartTime.Value > Courses.EndTime.Value)
                        tip = "培训开始时间不能大于结束时间";
                    else if ((Courses.PeriodTime ?? 0) <= 0)
                        tip = "培训天数不能为空或小于0";
                    else if (Courses.CoursesType == null || string.IsNullOrEmpty(Courses.CoursesType.Trim()))
                        tip = "课程类别不能为空";
                    else if (Courses.CoursesName == null || string.IsNullOrEmpty(Courses.CoursesName.Trim()))
                        tip = "课程名称不能为空";
                    else if (Courses.ProvinceCity == null || string.IsNullOrEmpty(Courses.ProvinceCity.Trim()))
                        tip = "主办单位不能为空";
                }
                else if (Apply.VacationClass == "内部讲师授课")
                {
                    if (!Teaching.StartTime.HasValue)
                        tip = "授课开始时间不能为空";
                    else if (!Teaching.EndTime.HasValue)
                        tip = "授课结束时间不能为空";
                    else if (Teaching.StartTime.Value > Teaching.EndTime.Value)
                        tip = "授课开始时间不能大于结束时间";
                    else if ((Teaching.PeriodTime ?? 0) <= 0)
                        tip = "授课天数不能为空或小于0";
                    else if (Teaching.TeacherType == null || string.IsNullOrEmpty(Teaching.TeacherType.Trim()))
                        tip = "讲师级别不能为空";
                    else if (Teaching.TeachingPlan == null || string.IsNullOrEmpty(Teaching.TeachingPlan.Trim()))
                        tip = "授课课程名称不能为空";
                }
                else if (Apply.VacationClass == "其他")
                {
                    if (!Other.StartTime.HasValue)
                        tip = "事件开始时间不能为空";
                    else if (!Other.EndTime.HasValue)
                        tip = "事件结束时间不能为空";
                    else if (Other.StartTime.Value > Other.EndTime.Value)
                        tip = "事件开始时间不能大于结束时间";
                    else if ((Other.PeriodTime ?? 0) <= 0)
                        tip = "事件天数不能为空或小于0";
                    else if (Other.OtherName == null || string.IsNullOrEmpty(Other.OtherName.Trim()))
                        tip = "事件名称不能为空";
                }
                #endregion
            }
            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                Apply.ApplyTitle = Apply.ApplyTitle.Trim();
                Apply.Boral = Apply.Boral ?? false;
                if (Apply.Boral == false) Apply.Leader = string.Empty;
                if (Apply.Attids == null || string.IsNullOrEmpty(Apply.Attids))
                    Apply.Attids = Guid.NewGuid().ToString();
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

                if (Apply.VacationType == "公假")
                {
                    switch (Apply.VacationClass)
                    {
                        case "开会": { Apply.Meetings = new List<HRVacationMeeting>(); Meeting.ID = Guid.NewGuid(); Apply.Meetings.Add(Meeting); } break;
                        case "培训": { Apply.Courses = new List<HRVacationCourses>(); Courses.CoursesId = Guid.NewGuid(); Apply.Courses.Add(Courses); } break;
                        case "内部讲师授课": { Apply.Teachings = new List<HRVacationTeaching>(); Teaching.ID = Guid.NewGuid(); Apply.Teachings.Add(Teaching); } break;
                        case "其他": { Apply.Others = new List<HRVacationOther>(); Other.ID = Guid.NewGuid(); Apply.Others.Add(Other); } break;
                        default: break;
                    }
                }

                result.IsSuccess = _applyService.Insert(Apply);
            }
            else
            {//编辑
             //Apply = _applyService.FindById(Apply.ApplyId);
             //假期会议_meetingService
                var temp1 = _meetingService.FindByFeldName(u => u.ReVacationID == Apply.ApplyId);
                if (temp1 != null && temp1.ID != Guid.Empty)
                    _meetingService.Delete(temp1);
                //假期培训_coursesService
                var temp2 = _coursesService.FindByFeldName(u => u.ReVacationID == Apply.ApplyId);
                if (temp2 != null && temp2.CoursesId != Guid.Empty)
                    _coursesService.Delete(temp2);
                //内部讲师授课_teachingService
                var temp3 = _teachingService.FindByFeldName(u => u.ReVacationID == Apply.ApplyId);
                if (temp3 != null && temp3.ID != Guid.Empty)
                    _teachingService.Delete(temp3);
                //其他休假_otherService
                var temp4 = _otherService.FindByFeldName(u => u.ReVacationID == Apply.ApplyId);
                if (temp4 != null && temp4.ID != Guid.Empty)
                    _otherService.Delete(temp4);

                if (Apply.VacationType == "公假")
                {
                    switch (Apply.VacationClass)
                    {
                        case "开会":
                            {
                                if (Meeting.ID == Guid.Empty) Meeting.ID = Guid.NewGuid();
                                Meeting.ReVacationID = Apply.ApplyId;
                                _meetingService.Insert(Meeting);
                            }
                            break;
                        case "培训":
                            {
                                if (Courses.CoursesId == Guid.Empty) Courses.CoursesId = Guid.NewGuid();
                                Courses.ReVacationID = Apply.ApplyId;
                                _coursesService.Insert(Courses);
                            }
                            break;
                        case "内部讲师授课":
                            {
                                if (Teaching.ID == Guid.Empty) Teaching.ID = Guid.NewGuid();
                                Teaching.ReVacationID = Apply.ApplyId;
                                _teachingService.Insert(Teaching);
                            }
                            break;
                        case "其他":
                            {
                                if (Other.ID == Guid.Empty) Other.ID = Guid.NewGuid();
                                Other.ReVacationID = Apply.ApplyId;
                                _otherService.Insert(Other);
                            }
                            break;
                        default: break;
                    }
                }

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
        public SystemResult Sumbit_AgoEstimateApply_WF(HRReVacationApply curData, string nextActivity, string nextActors)
        {
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.ReVacationApply, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
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
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.ReVacationApply, curData.ApplyTitle, curData.ApplyId.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

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