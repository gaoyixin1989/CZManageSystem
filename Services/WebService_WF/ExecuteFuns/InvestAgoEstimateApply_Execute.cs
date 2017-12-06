using CZManageSystem.Core;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.CollaborationCenter.Invest;
using CZManageSystem.Service.ITSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_WF.Base;
using WebService_WF.Domain;

namespace WebService_WF.ExecuteFuns
{
    /// <summary>
    /// 耗材调平申请流程
    /// </summary>
    public class InvestAgoEstimateApply_Execute
    {
        public WorkflowDetail workflowDetail;

        IInvestAgoEstimateService _agoEstimateService = new InvestAgoEstimateService();//历史项目暂估
        IInvestAgoEstimateApplyService _applyService = new InvestAgoEstimateApplyService();//历史项目暂估申请
        IInvestAgoEstimateApplyDetailService _applyDetailService = new InvestAgoEstimateApplyDetailService();//历史项目暂估申请明细


        public InvestAgoEstimateApply_Execute(WorkflowDetail obj)
        {
            workflowDetail = obj;

            string confirmStepName = WfSectionGroup.InvestAgoEstimateApply["Confirm"].ToString();
            if (workflowDetail.PreActivities.Count > 0 && workflowDetail.PreActivities[0].Name == confirmStepName && workflowDetail.PreActivities[0].Command.ToLower() == "approve")
            {//财务人员确认通过
                ConfirmStep_Approve();
            }
        }

        /// <summary>
        /// 室经理审批通过后执行方法
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
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：获取历史项目暂估申请单ID失败", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            InvestAgoEstimateApply curApply = _applyService.FindById(dataID);
            if (curApply == null || curApply.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：查询不到对应历史项目暂估申请单信息", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            List<InvestAgoEstimateApplyDetail> detailDatas = _applyDetailService.List().Where(u => u.ApplyID == curApply.ID).ToList();
            foreach (var curDetail in detailDatas)
            {
                var oldData = _agoEstimateService.FindByFeldName(u => u.ProjectID == curDetail.ProjectID && u.ContractID == curDetail.ContractID);
                if (oldData == null || oldData.ID == Guid.Empty)
                {//只更新未重复的信息
                    InvestAgoEstimate newData = new InvestAgoEstimate();
                    //复制数据
                    BaseFun.AutoMapping<InvestAgoEstimateApplyDetail, InvestAgoEstimate>(curDetail, newData);
                    newData.ID = Guid.NewGuid();
                    _agoEstimateService.Insert(newData);
                }

            }
            LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务执行结束", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.success);
            return;

        }


    }

}