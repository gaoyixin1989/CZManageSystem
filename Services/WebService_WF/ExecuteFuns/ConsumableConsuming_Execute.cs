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
    /// 耗材领用申请流程
    /// </summary>
    public class ConsumableConsuming_Execute
    {
        public WorkflowDetail workflowDetail;
        IConsumableService _consumableService = new ConsumableService();
        IConsumable_ConsumingService _consumableConsumingService = new Consumable_ConsumingService();
        IConsumable_ConsumingDetailService _consumable_ConsumingDetailService = new Consumable_ConsumingDetailService();

        public ConsumableConsuming_Execute(WorkflowDetail obj)
        {
            workflowDetail = obj;
            string shenheStepName = WfSectionGroup.ConsumableApply["ShenHe"].ToString();
            string yezhiStepName = WfSectionGroup.ConsumableApply["YeZhi"].ToString();

            if (workflowDetail.PreActivities.Count > 0
                && workflowDetail.PreActivities[0].Name == "提交" && workflowDetail.Activities[0].Name == yezhiStepName)
            {//低值产品，下一步为“业务支持中心”，应该在提交时及修改库存量
                ShenHe_Approve();
            }
            if (workflowDetail.PreActivities.Count > 0
                && workflowDetail.PreActivities[0].Name == shenheStepName
                && workflowDetail.PreActivities[0].Command.ToLower() == "approve")
            {//非低值产品，业支中心领导审批ShenHe通过时，修改耗材库存量
                ShenHe_Approve();
            }
        }

        /// <summary>
        /// 修改库存
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

            Consumable_Consuming curConsuming = _consumableConsumingService.FindById(dataID);
            if (curConsuming == null || curConsuming.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：查询不到对应领用单信息", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            List<Consumable_ConsumingDetail> detailDatas = _consumable_ConsumingDetailService.List().Where(u => u.ApplyID == curConsuming.ID).ToList();
            foreach (var curCDetail in detailDatas)
            {
                Consumable curConsumable = _consumableService.FindById(curCDetail.ConsumableID ?? 0);
                if (curConsumable != null && curConsumable.ID != 0)
                {
                    curConsumable.Amount -= (curCDetail.ClaimsNumber ?? 0);
                    _consumableService.Update(curConsumable);
                }
            }
            LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务执行结束", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.success);
            return;

        }


    }
}