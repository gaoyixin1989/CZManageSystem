using CZManageSystem.Core;
using CZManageSystem.Data.Domain.ITSupport;
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
    /// 办公设备申请
    /// </summary>
    public class EquipApply_Execute
    {
        public WorkflowDetail workflowDetail;
        IEquipApplyService _sysEquipApplyService = new EquipApplyService();
        //IStockService _sysStockService = new StockService();

        public EquipApply_Execute(WorkflowDetail obj)
        {
            workflowDetail = obj;
            string shenheStepName = WfSectionGroup.EquipApply["ShenHe"].ToString();
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
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：获取退库单ID失败", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            var curapp = _sysEquipApplyService.List().Where(u=>u.ApplyId==dataID.ToString()).ToList();
            if (curapp == null) {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：查询不到对应领用单信息", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }
            foreach (var item in curapp)
            {
                item.Status = 3;//完成
                _sysEquipApplyService.Update(item);
            }
            LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务执行结束", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.success);
            return;

        }
    }
}