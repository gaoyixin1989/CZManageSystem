using CZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using CZManageSystem.Data.Domain.Administrative;
using CZManageSystem.Service.Administrative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_WF.Base;
using WebService_WF.Domain;

namespace WebService_WF.ExecuteFuns
{
    /// <summary>
    /// 会议室申请流程
    /// </summary>
    public class BoardroomApply_Execute
    {
        public WorkflowDetail workflowDetail;
        IBoardroomInfoService _boardroomInfoService = new BoardroomInfoService();
        IBoardroomApplyService _boardroomApplyService = new BoardroomApplyService();
        IScheduleService _scheduleService = new ScheduleService();

        public BoardroomApply_Execute(WorkflowDetail obj)
        {
            workflowDetail = obj;

            string shenheStepName = WfSectionGroup.BoardroomApply["ShenHeSuccess"].ToString();
            if (workflowDetail.PreActivities.Count > 0 && workflowDetail.PreActivities[0].Name == shenheStepName && workflowDetail.PreActivities[0].Command.ToLower() == "approve")
            {//会议室管理员审批
                FunAfterShenPiSuccess();
            }
        }

        /// <summary>
        /// 会议室申请已经成功，将会议时间添加到对应参与用户的日程安排中
        /// </summary>
        public void FunAfterShenPiSuccess()
        {
            int dataID = 0;
            string F1_value = "";
            foreach (Field field in workflowDetail.Fields)
            {
                if (field.Key == "F1")
                {
                    F1_value = field.Value;
                    break;
                }
            }

            if (!int.TryParse(F1_value, out dataID))
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：获取会议室申请单ID失败", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            BoardroomApply applyData = new BoardroomApply();
            BoardroomInfo roomInfo = new BoardroomInfo();
            applyData = _boardroomApplyService.FindById(dataID);
            if (applyData == null || applyData.ID == 0)
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：查询会议室申请单信息失败", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }
            roomInfo = _boardroomInfoService.FindByFeldName(u => u.BoardroomID == (applyData.Room ?? 0));
            if (roomInfo == null || roomInfo.BoardroomID == 0)
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：查询对应会议室单信息失败", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            DateTime scheduleTime = DateTime.Parse(applyData.MeetingDate.Value.ToString("yyyy-MM-dd") + " " + applyData.StartTime + ":00");
            string strContent = string.Format("参加会议“{0}”，时间：{1}—{2}，地点：{3}——{4}",
                applyData.ApplyTitle,
                applyData.MeetingDate.Value.ToString("yyyy-MM-dd") + " " + applyData.StartTime,
                applyData.EndDate.Value.ToString("yyyy-MM-dd") + " " + applyData.EndTime,
                roomInfo.Address,
                roomInfo.Name);

            if (!string.IsNullOrEmpty(applyData.JoinPeople))
            {
                List<Schedule> listSchedule = new List<Schedule>();
                foreach (var str in applyData.JoinPeople.Split(','))
                {
                    try
                    {
                        Schedule temp = new Schedule();
                        temp.UserId = Guid.Parse(str);
                        temp.Time = scheduleTime;
                        temp.Content = strContent;
                        temp.Createdtime = DateTime.Now;
                        listSchedule.Add(temp);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                _scheduleService.InsertByList(listSchedule);
            }


            LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务执行结束", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.success);
            return;

        }


    }

}