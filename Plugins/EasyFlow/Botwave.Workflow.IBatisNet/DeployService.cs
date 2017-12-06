using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using Botwave.Workflow;
using Botwave.Workflow.Parser;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.IBatisNet
{
    public class DeployService : AbstarctDeployService
    {
        IActivitySetService activitySetService = new ActivitySetService();

        protected override int ExecuteDelete(Guid workflowId)
        {
            return IBatisMapper.Update("bwwf_Workflows_Delete", workflowId);
        }

        protected override IDictionary<Guid, AllocatorOption> GetAssignmentAllocators(Guid workflowId)
        {
            return IBatisMapper.Mapper.QueryForDictionary<Guid, AllocatorOption>("bwwf_AssignmentAllocator_Select_ByWorkflowId", workflowId, "ActivityId");
        }

        protected override IList<Guid> GetAcitivtyIdsBySetId(Guid activitySetId)
        {
            return activitySetService.GetActivityIdSets(activitySetId);
        }

        protected override WorkflowDefinition GetCurrentWorkflow(string workflowName)
        {
            IList<WorkflowDefinition> workflows = IBatisMapper.Select<WorkflowDefinition>("bwwf_Workflows_Select_By_WorkflowName", workflowName);
            if (null == workflows || workflows.Count == 0)
            {
                return null;
            }
            return workflows[0];
        }

        /// <summary>
        /// 更新当前版本为旧版本.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        protected override int UpdateCurrent(Guid workflowId)
        {
            return IBatisMapper.Update("bwwf_Workflows_UpdateCurrent", workflowId);
        }

        protected override DeployActionResult InsertWorkflow(WorkflowDefinition workflow, IList<DeployActivityDefinition> activities)
        {
            DeployActionResult result = new DeployActionResult();
            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                IBatisMapper.Mapper.Insert("bwwf_Workflows_Insert", workflow);

                // 插入各流程步骤.
                foreach (DeployActivityDefinition activity in activities)
                {
                    IBatisMapper.Mapper.Insert("bwwf_Activities_Insert", activity);

                    // 插入活动集合(bwwf_ActivitySet)
                    foreach (DeployActivitySet item in activity.PrevActivitySets)
                    {
                        IBatisMapper.Mapper.Insert("bwwf_ActivitySet_Insert", item);
                    }
                    foreach (DeployActivitySet item in activity.NextActivitySets)
                    {
                        IBatisMapper.Mapper.Insert("bwwf_ActivitySet_Insert", item);
                    }

                    // 插入转交分派定义(bwwf_Assignments)
                    if (activity.AssignmentAllocator != null)
                    {
                        IBatisMapper.Mapper.Insert("bwwf_AssignmentAllocator_Insert", activity.AssignmentAllocator);
                    }
                }
                IBatisMapper.Mapper.CommitTransaction();
                if (string.IsNullOrEmpty(result.Message))
                    result.Message = "流程配置成功!";
                result.Success = true;
            }
            catch (Exception ex)
            {
                IBatisMapper.Mapper.RollBackTransaction();
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }
    }
}
