using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Botwave.Workflow;
using Botwave.Workflow.Allocator;
using Botwave.Workflow.Parser;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;

namespace Botwave.XQP.Service.Plugins
{
    public class SpecialCycleActivityExecutionHandler : Botwave.Workflow.IBatisNet.ActivityExecutionService, IActivityExecutionHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(SpecialCycleActivityExecutionHandler));

        private string specialActivityName = "提交下一步处理人";

        public string SpecialActivityName
        {
            set { specialActivityName = value; }
        }

        public override void Execute(ActivityExecutionContext context)
        {
            //base.Execute(context);

            ActivityInstance activityInstance = GetActivityInstanceById(context.ActivityInstanceId);
            ActivityDefinition activityDefinition = GetActivityDefinitionById(activityInstance.ActivityId);

            string command = context.Command;
            string actorDescription = GetActorDescription(context);
            context.ActorDescription = actorDescription;

            activityInstance.Command = command;
            activityInstance.ExternalEntityId = context.ExternalEntityId;
            activityInstance.ExternalEntityType = context.ExternalEntityType;
            activityInstance.Reason = context.Reason;
            activityInstance.Actor = context.Actor;
            activityInstance.ActorDescription = actorDescription;

            if (command.Equals(ActivityCommands.Save))
            {
                SaveActivityInstance(activityInstance, ActivityInstanceActionType.Update);
            }
            else
            {
                if (null != context.ActivityAllocatees && context.ActivityAllocatees.Count > 0)
                {
                    DoProcess(activityDefinition, activityInstance, context);
                    if (activityDefinition.State == WorkflowConstants.Initial)
                    {
                        UpdateWorkflowState(activityInstance.ActivityInstanceId, WorkflowConstants.Executing);
                    }
                }
                else//按流程定义自动处理.
                {
                    ProcessAutomatically(activityDefinition, activityInstance, command, context);
                }
            }
        }

        /// <summary>
        /// 创建结束活动的活动实例.
        /// 直接完成活动并且直接完成流程.
        /// </summary>
        /// <param name="activityDefinition"></param>
        /// <param name="currentInstance"></param>
        /// <param name="context"></param>
        private void CreateEndActivityInstance(ActivityDefinition activityDefinition, ActivityInstance currentInstance, ActivityExecutionContext context)
        {
            ActivityInstance nextInstance = GenActivityInstanceByDefinition(activityDefinition);
            nextInstance.WorkflowInstanceId = currentInstance.WorkflowInstanceId;
            nextInstance.Actor = context.Actor;
            nextInstance.Command = ActivityCommands.Approve;
            nextInstance.Reason = "同意";
            nextInstance.IsCompleted = true;
            nextInstance.PrevSetId = Guid.NewGuid();
            nextInstance.ActorDescription = context.ActorDescription;
            CreateActivityInstanceIdSet(nextInstance.PrevSetId, currentInstance.ActivityInstanceId);
            SaveActivityInstance(nextInstance, ActivityInstanceActionType.CreateAndComplete);

            UpdateWorkflowState(context.ActivityInstanceId, WorkflowConstants.Complete);
        }

        private void DoProcess(ActivityDefinition currentDefinition, ActivityInstance currentInstance, ActivityExecutionContext context)
        {
            // 获取指派.
            IDictionary<Guid, IDictionary<string, string>> allocatees = context.ActivityAllocatees;

            //关闭本次活动.
            SaveActivityInstance(currentInstance, ActivityInstanceActionType.Complete);

            //关联前驱活动实例列表 .
            Guid prevSetId = Guid.NewGuid();
            Guid workflowInstanceId = currentInstance.WorkflowInstanceId;
            Guid activityInstanceId = currentInstance.ActivityInstanceId;

            foreach (Guid activityDefinitionId in allocatees.Keys)
            {
                ActivityDefinition activityDefinition = GetActivityDefinitionById(activityDefinitionId);
                if (null == activityDefinition)
                {
                    throw new ActivityNotFoundException("找不到对应的流程活动");
                }

                IDictionary<string, string> users = allocatees[activityDefinition.ActivityId];

                // 移出来，是为了跳过对完成步骤的用户检查.
                // 如果下一活动是完成(不对完成结点进行合并条件判断)，认为完成结点只有一个前驱结点.
                if (activityDefinition.State == WorkflowConstants.Complete)
                {
                    CreateEndActivityInstance(activityDefinition, currentInstance, context);
                    base.CreateActivityInstanceIdSet(prevSetId, activityInstanceId);
                    return;
                }

                if (users == null || users.Count == 0)
                {
                    throw new WorkflowAllocateException("没有选择处理人");
                }

                if (specialActivityName == activityDefinition.ActivityName)//如果是提交到处理步骤.
                {
                    CreateNextActivityInstance(context.Actor, prevSetId, workflowInstanceId, activityDefinition, users);
                    base.CreateActivityInstanceIdSet(prevSetId, activityInstanceId);
                }
                else//提交到需求验收步骤.
                {
                    //如果此工单的所有处理步骤都已完成则提交,否则不用处理.
                    bool isProcessCompleted = CouldSubmit2Creator(workflowInstanceId, currentDefinition.ActivityId);
                    if (isProcessCompleted)
                    {
                        CreateNextActivityInstance(context.Actor, prevSetId, workflowInstanceId, activityDefinition, users);
                        base.CreateActivityInstanceIdSet(prevSetId, activityInstanceId);
                    }
                }

                return;  //此流程只能选一个分支.
            }
        }

        private static bool CouldSubmit2Creator(Guid workflowInstanceId, Guid activityId)
        {
            string sql = "select count(1) from dbo.bwwf_Tracking_Activities where WorkflowInstanceId = @WorkflowInstanceId and ActivityId = ActivityId and IsCompleted = 0";
            System.Data.IDbDataParameter[] parms = Botwave.Extension.IBatisNet.IBatisDbHelper.CreateParameterSet(2);
            parms[0].ParameterName = "@WorkflowInstanceId";
            parms[0].Value = workflowInstanceId;
            parms[1].ParameterName = "@ActivityId";
            parms[1].Value = activityId;

            object obj = Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteScalar(System.Data.CommandType.Text, sql, parms);
            int count = Convert.ToInt32(obj);
            return count == 0;
        }

        private void CreateNextActivityInstance(string actor, Guid prevSetId, Guid workflowInstanceId, ActivityDefinition activityDefinition, IDictionary<string, string> users)
        {
            foreach (KeyValuePair<string, string> pair in users)
            {
                //创建下一活动实例.
                ActivityInstance instance = GenActivityInstanceByDefinition(activityDefinition);
                instance.WorkflowInstanceId = workflowInstanceId;
                instance.PrevSetId = prevSetId;
                SaveActivityInstance(instance, ActivityInstanceActionType.Create);
                InsertTodoActivity(instance.ActivityInstanceId, pair.Key, null, TodoInfo.OpDefault);

                if (!String.IsNullOrEmpty(pair.Value))
                {
                    IDictionary<string, string> usersDict = new Dictionary<string, string>();
                    usersDict.Add(pair.Key, pair.Value);
                    ExecuteUserProxyNotifier(instance.ActivityInstanceId, usersDict);
                }
            }
        }
    }
}
