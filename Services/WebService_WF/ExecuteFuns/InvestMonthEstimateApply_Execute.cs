using CZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.CollaborationCenter.Invest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_WF.Base;
using WebService_WF.Domain;

namespace WebService_WF.ExecuteFuns
{
    public class InvestMonthEstimateApply_Execute
    {
        public WorkflowDetail workflowDetail;
        IInvestMonthEstimateApplyService _investMonthEstimateApplyService = new CZManageSystem.Service.CollaborationCenter.Invest.InvestMonthEstimateApplyService();
        IInvestMonthEstimateApplySubListService _investMonthEstimateApplySubListService = new InvestMonthEstimateApplySubListService();
        IInvestEstimateService _investEstimateService = new InvestEstimateService();


        public InvestMonthEstimateApply_Execute(WorkflowDetail obj)
        {
            workflowDetail = obj;
            string shenheStepName = WfSectionGroup.InvestMonthEstimateApply["ShenPi"].ToString();
            if (workflowDetail.PreActivities.Count > 0
                && (workflowDetail.PreActivities[0].Name == shenheStepName)
                && workflowDetail.PreActivities[0].Command.ToLower() == "approve")
            {//审核通过
                ShenHe_Approve();
            }
        }

        /// <summary>
        /// 审核通过后执行方法
        /// </summary>
        public void ShenHe_Approve()
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
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：每月暂估填报流程推送失败", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            List <InvestMonthEstimateApplySubList> monthList = _investMonthEstimateApplySubListService.List().Where(u => u.ApplyID == dataID).ToList();

            List<InvestEstimate> list = new List<InvestEstimate>();
            InvestEstimate eati = new InvestEstimate();
            foreach (var item in monthList)
            {
                eati = new InvestEstimate();
                eati.ID = Guid.NewGuid();
                eati.Year = item.Year;
                eati.Month = item.Month;
                eati.BackRate = item.BackRate;
                eati.ContractID = item.ContractID;
                eati.ProjectName = item.ProjectName;
                eati.ContractName = item.ContractName;
                eati.Course = item.Course;
                eati.ManagerID = item.ManagerID;
                eati.NotPay = item.NotPay;
                eati.Pay = item.Pay;
                eati.PayTotal = item.PayTotal;
                eati.ProjectID = item.ProjectID;
                eati.Rate = item.Rate;
                eati.SignTotal = item.SignTotal;
                eati.Study = item.Study;
                eati.Supply = item.Supply;
                list.Add(eati);
            }
            _investEstimateService.InsertByList(list);
            LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务执行结束", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.success);
            return;

        }


    }
}