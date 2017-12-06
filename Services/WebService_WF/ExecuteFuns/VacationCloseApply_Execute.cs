using CZManageSystem.Core;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Service.HumanResources.Vacation;
using System;
using WebService_WF.Base;
using WebService_WF.Domain;

namespace WebService_WF.ExecuteFuns
{
    /// <summary>
    /// 销假申请流程
    /// </summary>
    public class VacationCloseApply_Execute
    {
        public WorkflowDetail workflowDetail;

        IHRVacationApplyService _OldapplyService = new HRVacationApplyService();//销假申请
        IHRVacationCloseApplyService _applyService = new HRVacationCloseApplyService();//销假申请
        IHRVacationAnnualLeaveService _xxhrannualleaveservice = new HRVacationAnnualLeaveService();//年假明细

        public VacationCloseApply_Execute(WorkflowDetail obj)
        {
            workflowDetail = obj;

            string confirmStepName = WfSectionGroup.VacationCloseApply["Confirm"].ToString();
            if (workflowDetail.PreActivities.Count > 0 && workflowDetail.PreActivities[0].Name == confirmStepName && workflowDetail.PreActivities[0].Command.ToLower() == "approve")
            {//审核通过
                ConfirmStep_Approve();
            }
        }

        /// <summary>
        /// 审核通过后执行方法
        /// </summary>
        public void ConfirmStep_Approve()
        {
            Guid dataID = new Guid();
            string F1_value = "";
            foreach (Field field in workflowDetail.Fields)
            {
                if (field.Key == "F1")
                {
                    F1_value = field.Value;
                    break;
                }
            }

            if (!Guid.TryParse(F1_value, out dataID))
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：获取销假申请单ID失败", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            HRVacationCloseApply curApply = _applyService.FindById(dataID);//销假信息
            if (curApply == null || curApply.ApplyId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：查询不到对应销假申请单信息", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }


            HRVacationApply oldApply = _OldapplyService.FindById(curApply.VacationID);//原来的休假信息
            if (oldApply == null || oldApply.ApplyId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：当前销假申请信息没有对应的休假申请单信息", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            oldApply.CancelVacation = 1;
            oldApply.CancelDays = curApply.ClosedDays;
            _OldapplyService.Update(oldApply);


            if (oldApply.VacationType == "年休假")
            {
                //新增年假信息数据并插入
                HRVacationAnnualLeave annualLeaveObj = _xxhrannualleaveservice.FindByFeldName(u => u.AppID == oldApply.ApplyId);
                if (annualLeaveObj != null && annualLeaveObj.ID != Guid.Empty) {
                    if (oldApply.PeriodTime <= curApply.ClosedDays)
                        _xxhrannualleaveservice.Delete(annualLeaveObj);
                    else
                    {
                        annualLeaveObj.StartTime = curApply.Factst;
                        annualLeaveObj.EndTime = curApply.Factet;
                        annualLeaveObj.SpendDays = oldApply.PeriodTime - curApply.ClosedDays;
                        _xxhrannualleaveservice.Update(annualLeaveObj);
                    }
                }

            }
            
            LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务执行结束", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.success);
            return;

        }


    }

}