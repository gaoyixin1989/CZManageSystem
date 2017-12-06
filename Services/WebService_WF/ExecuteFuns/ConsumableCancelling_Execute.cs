using CZManageSystem.Core;
using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
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
    /// 耗材退库申请流程
    /// </summary>
    public class ConsumableCancelling_Execute
    {
        public WorkflowDetail workflowDetail;
        IConsumableService _consumableService = new ConsumableService();
        IConsumable_CancellingService _cancellingService = new Consumable_CancellingService();
        IConsumable_CancellingDetailService _cancellingDetailService = new Consumable_CancellingDetailService();

        public ConsumableCancelling_Execute(WorkflowDetail obj)
        {
            workflowDetail = obj;

            string shenheStepName = WfSectionGroup.ConsumableCancelling["ShenHe"].ToString();
            if (workflowDetail.PreActivities.Count > 0 && workflowDetail.PreActivities[0].Name == shenheStepName && workflowDetail.PreActivities[0].Command.ToLower() == "approve")
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
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：获取退库单ID失败", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            Consumable_Cancelling curCancelling = _cancellingService.FindById(dataID);
            if (curCancelling == null || curCancelling.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：查询不到对应退库单信息", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            List<Consumable_CancellingDetail> detailDatas = _cancellingDetailService.List().Where(u => u.ApplyID == curCancelling.ID).ToList();
            foreach (var curCDetail in detailDatas)
            {
                Consumable curConsumable = _consumableService.FindById(curCDetail.ConsumableID ?? 0);
                if (curConsumable != null && curConsumable.ID != 0)
                {
                    curConsumable.Amount -= (curCDetail.CancelNumber ?? 0);
                    _consumableService.Update(curConsumable);
                }
            }
            LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务执行结束", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.success);
            return;

        }


    }

}