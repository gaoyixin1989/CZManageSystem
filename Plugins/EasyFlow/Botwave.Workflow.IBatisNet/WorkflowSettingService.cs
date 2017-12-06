using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;

namespace Botwave.Workflow.IBatisNet
{
    public class WorkflowSettingService : IWorkflowSettingService
    {
        #region IWorkflowSettingService 成员

        public WorkflowSetting GetWorkflowSetting(Guid workflowId)
        {
            return IBatisMapper.Load<WorkflowSetting>("bwwf_WorkflowSettings_Select_ByWorkflowId", workflowId);
        }

        public WorkflowSetting GetWorkflowSetting(string workflowName)
        {
            return IBatisMapper.Load<WorkflowSetting>("bwwf_WorkflowSettings_Select_ByWorkflowName", workflowName);
        }

        public WorkflowDefinition GetCurrentWorkflowDefinition(string workflowAlias)
        {
            return IBatisMapper.Load<WorkflowDefinition>("bwwf_Workflows_Select_CurrentVersion_ByWorkflowAlias", workflowAlias);
        }

        public void InsertSetting(WorkflowSetting item)
        {
            IBatisMapper.Insert("bwwf_WorkflowSettings_Insert", item);
        }

        public int UpdateSetting(WorkflowSetting item)
        {
            return IBatisMapper.Update("bwwf_WorkflowSettings_Update", item);
        }

        public bool ExistsSetting(string workflowName)
        {
            int result = IBatisMapper.Mapper.QueryForObject<int>("bwwf_WorkflowSettings_Select_IsExists", workflowName);
            return (result > 0);
        }

        #endregion
    }
}
