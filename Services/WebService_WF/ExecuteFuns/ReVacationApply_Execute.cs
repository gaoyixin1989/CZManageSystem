using CZManageSystem.Core;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Service.HumanResources.Vacation;
using System;
using WebService_WF.Base;
using WebService_WF.Domain;

namespace WebService_WF.ExecuteFuns
{
    /// <summary>
    /// 异常休假申请流程
    /// </summary>
    public class ReVacationApply_Execute
    {
        public WorkflowDetail workflowDetail;

        IHRReVacationApplyService _applyService = new HRReVacationApplyService();//异常休假申请
        IHRVacationAnnualLeaveService _xxhrannualleaveservice = new HRVacationAnnualLeaveService();//年假明细

        public ReVacationApply_Execute(WorkflowDetail obj)
        {
            workflowDetail = obj;

            string confirmStepName = WfSectionGroup.ReVacationApply["Confirm"].ToString();
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
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：获取异常休假申请单ID失败", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            HRReVacationApply curApply = _applyService.FindById(dataID);
            if (curApply == null || curApply.ApplyId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：查询不到对应异常休假申请单信息", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            if (curApply.VacationType == "年休假")
            {
                //新增年假信息数据并插入
                HRVacationAnnualLeave annualLeaveObj = new HRVacationAnnualLeave();
                annualLeaveObj.ID = Guid.NewGuid();
                annualLeaveObj.AppID = curApply.ApplyId;
                annualLeaveObj.UserID = curApply.Editor;
                annualLeaveObj.YearDate = curApply.StartTime.Value.Year.ToString();
                annualLeaveObj.SpendDays = curApply.PeriodTime;
                annualLeaveObj.StartTime = curApply.StartTime;
                annualLeaveObj.EndTime = curApply.EndTime;
                annualLeaveObj.Remark = curApply.Reason;

                _xxhrannualleaveservice.Insert(annualLeaveObj);
            }
            
            LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务执行结束", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.success);
            return;

        }


    }

}