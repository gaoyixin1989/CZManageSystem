using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.IBatisNet
{
    public class WorkflowDefinitionService : IWorkflowDefinitionService
    {
        #region IWorkflowDefinitionService Members

        public IList<WorkflowDefinition> GetAllWorkflowDefinition()
        {
            return IBatisMapper.Select<WorkflowDefinition>("bwwf_Workflows_Select_All");
        }

        public IList<WorkflowDefinition> GetWorkflowDefinitionList()
        {
            return IBatisMapper.Select<WorkflowDefinition>("bwwf_Workflows_Select", null);
        }

        public DataTable GetWorkflowDefinitionTable()
        {
            string sql = @"SELECT WorkflowId, WorkflowName, Owner, Enabled,  IsCurrent, Version, Creator, Remark,
                      LastModifier, CreatedTime, LastModTime, IsDeleted, WorkflowAlias, AliasImage
                  FROM vw_bwwf_Workflows_Detail
                  WHERE (IsDeleted = 0) and (Enabled = 1) AND (IsCurrent = 1)
                  ORDER BY  WorkflowAlias asc";
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public IList<WorkflowDefinition> GetWorkflowDefinitionListByName(string workflowName)
        {
            return IBatisMapper.Select<WorkflowDefinition>("bwwf_Workflows_Select_By_WorkflowName", workflowName);
        }

        public WorkflowDefinition GetWorkflowDefinition(Guid workflowDefinitionId)
        {
            IList<WorkflowDefinition> list = IBatisMapper.Select<WorkflowDefinition>("bwwf_Workflows_Select_By_Id", workflowDefinitionId);
            if (null == list || list.Count == 0)
            {
                return null;
            }
            return list[0];
        }

        public WorkflowDefinition GetCurrentWorkflowDefinition(string workflowName)
        {
            IList<WorkflowDefinition> list = IBatisMapper.Select<WorkflowDefinition>("bwwf_Workflows_Select_CurrentVersion_ByName", workflowName);
            if (null == list || list.Count == 0)
                return null;
            return list[0];
        }

        public WorkflowDefinition GetCurrentWorkflowDefinition(Guid workflowDefinitionId)
        {
            IList<WorkflowDefinition> list = IBatisMapper.Select<WorkflowDefinition>("bwwf_Workflows_Select_CurrentVersion_ById", workflowDefinitionId);
            if (null == list || list.Count == 0)
                return null;
            return list[0];
        }

        public int UpdateWorkflowEnabled(Guid workflowDefinitionId, bool enabled)
        {
            Hashtable parameters = new Hashtable(2);
            parameters.Add("WorkflowId", workflowDefinitionId);
            parameters.Add("Enabled", enabled);

            return IBatisMapper.Update("bwwf_Workflows_UpdateEnabled", parameters);
        }

        public bool WorkflowIsExists(Guid workflowDefinitionId)
        {
            return IBatisMapper.Mapper.QueryForObject<int>("bwwf_Workflows_Select_IsExists", workflowDefinitionId) > 0;
        }

        #endregion
    }
}
