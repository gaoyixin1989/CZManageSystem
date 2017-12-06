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

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程活动实例操作类型枚举.
    /// </summary>
    public enum ActivityInstanceActionType
    {
        /// <summary>
        /// 创建流程活动实例.
        /// </summary>
        Create = 0,
        /// <summary>
        /// 创建并流程活动实例使其完成.
        /// </summary>
        CreateAndComplete = 1,
        /// <summary>
        /// 完成流程活动实例.
        /// </summary>
        Complete = 2,
        /// <summary>
        /// 更新流程活动实例.
        /// </summary>
        Update = 3
    }

    /// <summary>
    /// 流程活动(步骤)执行服务的抽象类.
    /// </summary>
    public abstract class AbstractActivityExecutionService : IActivityExecutionService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AbstractActivityExecutionService));

        #region property getter/setter

        private IActivityExecutionHandlerManager activityExecutionHandlerManager;
        private IActivityDefinitionService activityDefinitionService;
        private IActivityService activityService;
        private IWorkflowDefinitionService workflowDefinitionService;
        private IWorkflowService workflowService;
        private ITaskAssignService taskAssignService;
        private IPostActivityExecutionHandler postActivityExecutionHandler;
        private IPostCancelWorkflowHandler postCancelWorkflowHandler;
        private IPostCompleteWorkflowHandler postCompleteWorkflowHandler;
        private IActivityExecutionHandler preDeleteWorkflowHandler;
        private ICommandRulesParser commandRulesParser;
        private IActivityAllocationService activityAllocationService;
        private IConditionParser joinConditionParser;
        private IConditionParser splitConditionParser;
        private IConditionParser countersignedConditionParser;
        private IDecisionParserManager decisionParserManager;
        private IPostCloseParallelActivityInstancesHandler postCloseParallelActivityInstancesHandler;
        private IUserProxy userProxy;
        private IUserProxyNotifier userProxyNotifier;
        private ICountersignedService countersignedService;
        private IJoinConditionHandlerManager joinConditionHandlerManager;

        /// <summary>
        /// 删除流程的前驱处理器.
        /// </summary>
        public IActivityExecutionHandler PreDeleteWorkflowHandler
        {
            get { return preDeleteWorkflowHandler; }
            set { preDeleteWorkflowHandler = value; }
        }

        /// <summary>
        /// 流程定义服务.
        /// </summary>
        public IWorkflowDefinitionService WorkflowDefinitionService
        {
            get { return workflowDefinitionService; }
            set { workflowDefinitionService = value; }
        }

        /// <summary>
        /// 流程实例服务.
        /// </summary>
        public IWorkflowService WorkflowService
        {
            get { return workflowService; }
            set { workflowService = value; }
        }

        /// <summary>
        /// 任务分派服务.
        /// </summary>
        public ITaskAssignService TaskAssignService
        {
            get { return taskAssignService; }
            set { taskAssignService = value; }
        }

        /// <summary>
        /// 流程活动(步骤)实例服务.
        /// </summary>
        public IActivityService ActivityService
        {
            get { return activityService; }
            set { activityService = value; }
        }

        /// <summary>
        /// 流程活动(步骤)执行处理器管理对象.
        /// </summary>
        public IActivityExecutionHandlerManager ActivityExecutionHandlerManager
        {
            get { return activityExecutionHandlerManager; }
            set { activityExecutionHandlerManager = value; }
        }

        /// <summary>
        /// 流程活动(步骤)定义服务.
        /// </summary>
        public IActivityDefinitionService ActivityDefinitionService
        {
            get { return activityDefinitionService; }
            set { activityDefinitionService = value; }
        }

        /// <summary>
        /// 流程活动(步骤)执行的后续处理器.
        /// </summary>
        public IPostActivityExecutionHandler PostActivityExecutionHandler
        {
            get { return postActivityExecutionHandler; }
            set { postActivityExecutionHandler = value; }
        }

        /// <summary>
        /// 取消流程的后续处理器.
        /// </summary>
        public IPostCancelWorkflowHandler PostCancelWorkflowHandler
        {
            get { return postCancelWorkflowHandler; }
            set { postCancelWorkflowHandler = value; }
        }

        /// <summary>
        /// 完成流程的后续处理器.
        /// </summary>
        public IPostCompleteWorkflowHandler PostCompleteWorkflowHandler
        {
            get { return postCompleteWorkflowHandler; }
            set { postCompleteWorkflowHandler = value; }
        }

        /// <summary>
        /// 命令规则解析.
        /// </summary>
        public ICommandRulesParser CommandRulesParser
        {
            get { return commandRulesParser; }
            set { commandRulesParser = value; }
        }

        /// <summary>
        /// 流程活动(步骤)任务分派服务.
        /// </summary>
        public IActivityAllocationService ActivityAllocationService
        {
            get { return activityAllocationService; }
            set { activityAllocationService = value; }
        }

        /// <summary>
        /// 合并条件解析.
        /// </summary>
        public IConditionParser JoinConditionParser
        {
            get { return joinConditionParser; }
            set { joinConditionParser = value; }
        }

        /// <summary>
        /// 分支条件解析.
        /// </summary>
        public IConditionParser SplitConditionParser
        {
            get { return splitConditionParser; }
            set { splitConditionParser = value; }
        }

        /// <summary>
        /// 会签条件解析.
        /// </summary>
        public IConditionParser CountersignedConditionParser
        {
            get { return countersignedConditionParser; }
            set { countersignedConditionParser = value; }
        }

        /// <summary>
        /// 自定义解析管理对象.
        /// </summary>
        public IDecisionParserManager DecisionParserManager
        {
            get { return decisionParserManager; }
            set { decisionParserManager = value; }
        }

        /// <summary>
        /// 关闭并行流程活动(步骤)实例的后续处理器.
        /// </summary>
        public IPostCloseParallelActivityInstancesHandler PostCloseParallelActivityInstancesHandler
        {
            get { return postCloseParallelActivityInstancesHandler; }
            set { postCloseParallelActivityInstancesHandler = value; }
        }

        /// <summary>
        /// 用户代理.
        /// </summary>
        public IUserProxy UserProxy
        {
            get { return userProxy; }
            set { userProxy = value; }
        }

        /// <summary>
        /// 用户代理提醒.
        /// </summary>
        public IUserProxyNotifier UserProxyNotifier
        {
            get { return userProxyNotifier; }
            set { userProxyNotifier = value; }
        }

        /// <summary>
        /// 会签服务.
        /// </summary>
        public ICountersignedService CountersignedService
        {
            get { return countersignedService; }
            set { countersignedService = value; }
        }

        /// <summary>
        /// 合并条件处理器管理者.
        /// </summary>
        public IJoinConditionHandlerManager JoinConditionHandlerManager
        {
            get { return joinConditionHandlerManager; }
            set { joinConditionHandlerManager = value; }
        }

        #endregion

        #region IActivityExecutionService Members

        /// <summary>
        /// 执行流程活动上下文对象.
        /// </summary>
        /// <param name="context"></param>
        public virtual void Execute(ActivityExecutionContext context)
        {
            DebugFormat("the context is [{0}]", context);

            string command = context.Command;
            if (String.IsNullOrEmpty(command))
            {
                throw new WorkflowException("没有对应的活动执行指令");
            }

            ActivityInstance activityInstance = GetActivityInstanceById(context.ActivityInstanceId);
            if (null == activityInstance)
            {
                throw new ActivityNotFoundException("找不到对应的活动");
            }

            command = command.ToLower(System.Globalization.CultureInfo.InvariantCulture);
            if (activityInstance.IsCompleted)
            {
                if (!ActivityCommands.Withdraw.Equals(command))//撤回时不用判断是否完成/关闭.
                {
                    throw new WorkflowException("此活动已完成/关闭");
                }
            }

            ActivityDefinition activityDefinition = GetActivityDefinitionById(activityInstance.ActivityId);
            //如果有自定义活动执行逻辑.
            if (WorkflowConfig.Instance.AllowExecutionHandler
                && !String.IsNullOrEmpty(activityDefinition.ExecutionHandler))
            {
                Debug("start to execute user defined logic");
                activityExecutionHandlerManager.Execute(activityDefinition.ExecutionHandler, context);
            }
            else
            {
                //如果有会签条件并且通过.
                if (WorkflowConfig.Instance.AllowCountersigned
                    && null != countersignedConditionParser
                    && !String.IsNullOrEmpty(activityDefinition.CountersignedCondition)
                    && ActivityCommands.Approve.Equals(command))
                {
                    bool isCountersignedCompleted = CheckIfCountersignedCompleted(context, activityDefinition);
                    if (!isCountersignedCompleted)
                    {
                        Countersigned countersigned = new Countersigned(context.ActivityInstanceId, context.Actor, context.Reason);
                        countersignedService.Sign(countersigned);
                        return;
                    }
                }

                string actorDescription = GetActorDescription(context);
                context.ActorDescription = actorDescription;

                activityInstance.Command = command;
                activityInstance.ExternalEntityId = context.ExternalEntityId;
                activityInstance.ExternalEntityType = context.ExternalEntityType;
                activityInstance.Reason = context.Reason;
                activityInstance.Actor = context.Actor;
                activityInstance.ActorDescription = actorDescription;

                //if (command.Equals(ActivityCommands.ReturnToDraft) || command.Equals(ActivityCommands.Save))
                //{
                //    // 保存与退还时，直接跳到默认处理中执行
                //    DefaultProcess(activityDefinition, activityInstance, command, context);
                //}
                if (command.Equals(ActivityCommands.Save))
                {
                    SaveActivityInstance(activityInstance, ActivityInstanceActionType.Update);
                }
                else
                {
                    IDictionary<string, ICollection<string>> nextActivityNamesAndUsers = GetNextActivityNamesAndUsersByDicisionParserOrCommandRules(context, command, activityDefinition);

                    if (null == nextActivityNamesAndUsers || nextActivityNamesAndUsers.Count == 0)
                    {
                        //如果没有定义对此命令的解析规则，执行默认的处理逻辑.
                        DefaultProcess(activityDefinition, activityInstance, command, context);
                    }
                    else
                    {
                        SaveActivityInstance(activityInstance, ActivityInstanceActionType.Complete);
                        Debug("execute user defined command rules");

                        int nextActivityCount = nextActivityNamesAndUsers.Count;
                        string[] nextActivityNames = new string[nextActivityCount];
                        nextActivityNamesAndUsers.Keys.CopyTo(nextActivityNames, 0);
                        IList<ICollection<string>> nextActivityUsers = new List<ICollection<string>>();
                        for (int i = 0; i < nextActivityCount; i++)
                        {
                            nextActivityUsers.Add(nextActivityNamesAndUsers[nextActivityNames[i]]);
                        }
                        //取出下一步的活动列表.
                        IList<ActivityDefinition> nextActivities = GetActivityDefinitionsByActivityNames(activityDefinition.WorkflowId, nextActivityNames);
                        CreateNextActivityInstances(nextActivities, nextActivityUsers, activityDefinition, activityInstance, context);
                        if (activityDefinition.State == WorkflowConstants.Initial)
                        {
                            UpdateWorkflowState(activityInstance.ActivityInstanceId, WorkflowConstants.Executing);
                        }
                    }
                }
            }

            ExecutePostActivityExecutionHandler(context, activityDefinition);
        }

        /// <summary>
        /// 启动流程.
        /// </summary>
        /// <param name="workflowInstance"></param>
        /// <returns></returns>
        public virtual Guid StartWorkflow(WorkflowInstance workflowInstance)
        {
            return this.CreateInitActivityInstance(workflowInstance, TodoInfo.OpDefault);
        }

        /// <summary>
        /// 完成指定流程.
        /// </summary>
        /// <param name="context"></param>
        public virtual void CompleteWorkflow(ActivityExecutionContext context)
        {
            UpdateWorkflowState(context.ActivityInstanceId, WorkflowConstants.Complete);
            CloseAllActivitiesOfWorkflowInstance(context);
            ExecuteDefaultPostCompleteWorkflowHandler(context);
        }

        /// <summary>
        /// 取消指定流程.
        /// </summary>
        /// <param name="context"></param>
        public virtual void CancelWorkflow(ActivityExecutionContext context)
        {
            UpdateWorkflowState(context.ActivityInstanceId, WorkflowConstants.Cancel);
            CloseAllActivitiesOfWorkflowInstance(context);
            ExecuteDefaultPostCancelWorkflowHandler(context);
        }

        /// <summary>
        /// 删除指定流程.
        /// </summary>
        /// <param name="context"></param>
        public virtual void RemoveWorkflow(ActivityExecutionContext context)
        {
            ExecutePreDeleteWorkflowHandler(context);
            RemoveWorkflowByActivityInstanceId(context.ActivityInstanceId);
        }

        #endregion

        #region 前驱/后续处理器

        /// <summary>
        /// 删除流程的前处理.
        /// </summary>
        /// <param name="context"></param>
        protected void ExecutePreDeleteWorkflowHandler(ActivityExecutionContext context)
        {
            if (null != preDeleteWorkflowHandler)
            {
                Debug("ExecutePreDeleteWorkflowHandler");
                preDeleteWorkflowHandler.Execute(context);
            }
        }

        /// <summary>
        /// 异步执行默认的取消流程后续处理.
        /// </summary>
        /// <param name="context"></param>
        protected void ExecuteDefaultPostCancelWorkflowHandler(ActivityExecutionContext context)
        {
            if (null != postCancelWorkflowHandler)
            {
                Debug("ExecuteDefaultPostCancelWorkflowHandler");
                Botwave.Commons.Threading.ISyncCaller caller = new SyncPostCancelWorkflowHandlerCaller(postCancelWorkflowHandler, context);
                Botwave.Commons.Threading.SyncCallerHost.Run(caller);
            }
        }

        /// <summary>
        /// 异步执行默认的完成流程后续处理.
        /// </summary>
        /// <param name="context"></param>
        protected void ExecuteDefaultPostCompleteWorkflowHandler(ActivityExecutionContext context)
        {
            if (null != postCompleteWorkflowHandler)
            {
                Debug("ExecuteDefaultPostCompleteWorkflowHandler");
                Botwave.Commons.Threading.ISyncCaller caller = new SyncPostCompleteWorkflowHandlerCaller(postCompleteWorkflowHandler, context);
                Botwave.Commons.Threading.SyncCallerHost.Run(caller);
            }
        }

        /// <summary>
        /// 异步执行默认的用户代理提醒处理.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="relations"></param>
        protected void ExecuteUserProxyNotifier(Guid activityInstanceId, IDictionary<string, string> relations)
        {
            if (null != userProxyNotifier && WorkflowConfig.Instance.AllowUserProxy)
            {
                Debug("ExecuteUserProxyNotifier");
                Botwave.Commons.Threading.ISyncCaller caller = new SyncUserProxyNotifierCaller(userProxyNotifier, activityInstanceId, relations);
                Botwave.Commons.Threading.SyncCallerHost.Run(caller);
            }
        }

        /// <summary>
        /// 异步执行流程活动(步骤)执行的后续处理器.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="activityDefinition"></param>
        protected void ExecutePostActivityExecutionHandler(ActivityExecutionContext context, ActivityDefinition activityDefinition)
        {
            if (context.Command.CompareTo(ActivityCommands.Reject) == 0)
            {
                string rejectOption = activityDefinition.RejectOption;
                if (String.IsNullOrEmpty(rejectOption))
                {
                    rejectOption = WorkflowConfig.Instance.DefaultRejectOption;
                }

                if (RejectOption.IsInitial(rejectOption))
                {
                    context.Command = ActivityCommands.ReturnToDraft;
                }
            }

            IPostActivityExecutionHandler postHandler = activityExecutionHandlerManager.GetHandler(activityDefinition.PostHandler) as IPostActivityExecutionHandler;
            if (null == postHandler)
            {
                postHandler = this.postActivityExecutionHandler;
            }

            Botwave.Commons.Threading.ISyncCaller caller = new SyncPostActivityExecutionHandlerCaller(postHandler, context);
            Botwave.Commons.Threading.SyncCallerHost.Run(caller);
        }

        #endregion

        #region protected virtual methods

        /// <summary>
        /// 默认的活动处理逻辑.
        /// </summary>
        /// <param name="activityDefinition"></param>
        /// <param name="activityInstance"></param>
        /// <param name="command"></param>
        /// <param name="context"></param>
        protected virtual void DefaultProcess(ActivityDefinition activityDefinition, ActivityInstance activityInstance, string command, ActivityExecutionContext context)
        {
            //如果指派了下一步的处理步骤与处理人，按手动指派处理.
            if (null != context.ActivityAllocatees && context.ActivityAllocatees.Count > 0)
            {
                ProcessManually(activityDefinition, activityInstance, context);
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

        /// <summary>
        /// 自动处理
        /// </summary>
        /// <param name="activityDefinition"></param>
        /// <param name="activityInstance"></param>
        /// <param name="command"></param>
        /// <param name="context"></param>
        protected void ProcessAutomatically(ActivityDefinition activityDefinition, ActivityInstance activityInstance, string command, ActivityExecutionContext context)
        {
            if (command.Equals(ActivityCommands.Approve))
            {
                Approve(activityDefinition, activityInstance, context);
            }
            else if (command.Equals(ActivityCommands.Reject))
            {
                Reject(activityDefinition, activityInstance, context);
            }
            else if (command.Equals(ActivityCommands.Cancel))
            {
                CancelWorkflow(context);
            }
            else if (command.Equals(ActivityCommands.Complete))
            {
                CompleteWorkflow(context);
            }
            else if (command.Equals(ActivityCommands.Withdraw))
            {
                //将工单状态置为草稿，删除流程相关活动实例、系统TODO.
                UpdateWorkflowState(context.ActivityInstanceId, WorkflowConstants.Initial);
            }
            else
            {
                throw new WorkflowException(String.Format("未知的活动执行命令：{0}", command));
            }
        }

        /// <summary>
        /// 退还工单的处理
        /// </summary>
        /// <param name="activityDefinition"></param>
        /// <param name="activityInstance"></param>
        /// <param name="context"></param>
        protected void Reject(ActivityDefinition activityDefinition, ActivityInstance activityInstance, ActivityExecutionContext context)
        {
            string rejectOption = activityDefinition.RejectOption;
            if (String.IsNullOrEmpty(rejectOption))
            {
                rejectOption = WorkflowConfig.Instance.DefaultRejectOption;
            }

            if (RejectOption.IsInitial(rejectOption))
            {
                SaveActivityInstance(activityInstance, ActivityInstanceActionType.Complete);
                ReturnToDraft(activityInstance, context);
            }
            else if (RejectOption.IsPrevious(rejectOption))
            {
                IList<ActivityInstance> allPrevInstances = null;
                allPrevInstances = GetActivityInstancesBySetId(activityInstance.PrevSetId);
                //allPrevInstances = activityService.GetCompletedActivitiesOfPrevDefinitionByCurrent(activityInstance);

                IDictionary<Guid, ActivityInstance> prevInstances = new Dictionary<Guid, ActivityInstance>();
                foreach (ActivityInstance ai in allPrevInstances)
                {
                    //过滤可能的重复步骤(如多次退回导致有多个已完成的有相同活动定义的活动实例).
                    if (!prevInstances.ContainsKey(ai.ActivityId))
                    {
                        prevInstances.Add(ai.ActivityId, ai);
                    }
                }

                if (prevInstances.Count == 0)
                {
                    throw new WorkflowException("已经退还到流程初始步骤，无法继续退还。");
                }

                //完成当前活动.
                SaveActivityInstance(activityInstance, ActivityInstanceActionType.Complete);

                //如果上一活动已经产生其它并行分支，则关闭所有其它并行分支.
                CloseParallelActivityInstances(activityDefinition, activityInstance);

                //以prevInstances为模板，更改Id、完成人等，创建新的活动实例.
                Guid prevSetId = Guid.NewGuid();
                CreateActivityInstanceIdSet(prevSetId, activityInstance.ActivityInstanceId);
                foreach (Guid key in prevInstances.Keys)
                {
                    prevInstances[key].PrevSetId = prevSetId; ;
                }
                CopyAndOpenClosedActivityInstances(context, prevInstances.Values);
            }
            else//退回到自定义步骤.
            {
                ActivityInstance targetInstance = activityService.GetLatestCompletedActivityByActivityName(activityDefinition.WorkflowId, activityInstance.WorkflowInstanceId, rejectOption);
                if (null == targetInstance)
                {
                    throw new WorkflowException(String.Format("无法找到退还的目标步骤[{0}]。", rejectOption));
                }

                SaveActivityInstance(activityInstance, ActivityInstanceActionType.Complete);

                CloseParallelActivityInstances(activityDefinition, activityInstance);

                Guid prevSetId = Guid.NewGuid();
                CreateActivityInstanceIdSet(prevSetId, activityInstance.ActivityInstanceId);

                targetInstance.PrevSetId = prevSetId;
                targetInstance.ActorDescription = context.ActorDescription;
                CopyAndOpenClosedActivityInstance(context, targetInstance);
            }

            if (activityDefinition.State == WorkflowConstants.Initial)
            {
                UpdateWorkflowState(activityInstance.ActivityInstanceId, WorkflowConstants.Executing);
            }
        }

        /// <summary>
        /// 退还到草稿箱的处理
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <param name="context"></param>
        protected void ReturnToDraft(ActivityInstance activityInstance, ActivityExecutionContext context)
        {
            //将工单状态置为草稿.
            //UpdateWorkflowState(context.ActivityInstanceId, WorkflowConstants.Initial);
            //关闭此工单的所有活动实例.
            CloseAllActivitiesOfWorkflowInstance(context);

            Guid workflowInstanceId = activityInstance.WorkflowInstanceId;
            int operateType = TodoInfo.OpBack;

            // 创建草稿状态的流程初始步骤.
            WorkflowInstance workflowInstance = workflowService.GetWorkflowInstance(workflowInstanceId);
            string actor = workflowInstance.Creator;
            Guid prevSetId = Guid.NewGuid();
            CreateActivityInstanceIdSet(prevSetId, activityInstance.ActivityInstanceId);
            Guid initActivityInstnaceId = CreateInitActivityInstance(workflowInstance, operateType, prevSetId);

            // 为流程初始步骤分派用户.
            IDictionary<string, string> actors = new Dictionary<string, string>();
            actors.Add(actor, null);
            this.AllocateTask(actors, initActivityInstnaceId, operateType);
        }

        /// <summary>
        /// 通过命令时的流程活动处理方法.
        /// </summary>
        /// <param name="activityDefinition"></param>
        /// <param name="activityInstance"></param>
        /// <param name="context"></param>
        protected void Approve(ActivityDefinition activityDefinition, ActivityInstance activityInstance, ActivityExecutionContext context)
        {
            SaveActivityInstance(activityInstance, ActivityInstanceActionType.Complete);

            //如果没手动选择，默认的分支规则为全部并行.
            IList<ActivityDefinition> nextActivities = GetNextActivityDefinitionsByInstanceId(activityInstance.ActivityInstanceId);

            CreateNextActivityInstances(nextActivities, activityDefinition, activityInstance, context);
            if (activityDefinition.State == WorkflowConstants.Initial)
            {
                UpdateWorkflowState(activityInstance.ActivityInstanceId, WorkflowConstants.Executing);
            }
        }

        /// <summary>
        /// 手动选择后续步骤与处理人的活动处理逻辑
        /// </summary>
        /// <param name="currentDefinition"></param>
        /// <param name="currentInstance"></param>
        /// <param name="context"></param>
        protected virtual void ProcessManually(ActivityDefinition currentDefinition, ActivityInstance currentInstance, ActivityExecutionContext context)
        {
            // 获取指派
            IDictionary<Guid, IDictionary<string, string>> allocatees = context.ActivityAllocatees;

            //关闭本次活动
            SaveActivityInstance(currentInstance, ActivityInstanceActionType.Complete);

            //关联前驱活动实例列表 
            Guid prevSetId = Guid.NewGuid();
            Guid workflowInstanceId = currentInstance.WorkflowInstanceId;
            IList<ActivityInstance> parallelActivityInstances = TryGetParallelActivityInstances(currentDefinition, currentInstance);

            IList<ActivityDefinition> activities = new List<ActivityDefinition>();
            foreach (Guid activityDefinitionId in allocatees.Keys)
            {
                ActivityDefinition activityDefinition = GetActivityDefinitionById(activityDefinitionId);
                if (null == activityDefinition)
                {
                    throw new ActivityNotFoundException("找不到对应的流程活动");
                }

                ////手动选择时，如果不是完成步骤，只要选择处理步骤则必须选择相应的处理人
                //if (activityDefinition.State != WorkflowConstants.Complete
                //    && (null == allocatees[activityDefinitionId] || allocatees[activityDefinitionId].Count == 0))
                //{
                //    throw new WorkflowException(String.Format("步骤[{0}]没有选择相应的处理人", activityDefinition.ActivityName));
                //}
                activities.Add(activityDefinition);
            }

            bool shouldCreatePrevIdSet = false;
            foreach (ActivityDefinition activityDefinition in activities)
            {
                IDictionary<string, string> users = allocatees[activityDefinition.ActivityId];

                // 移出来，是为了跳过对完成步骤的用户检查.
                // 如果下一活动是完成(不对完成结点进行合并条件判断)，认为完成结点只有一个前驱结点.
                if (activityDefinition.State == WorkflowConstants.Complete)
                {
                    CreateEndActivityInstance(activityDefinition, currentInstance, context);
                    return;
                }

                log.Debug("ProcessManually:" + activityDefinition.ActivityName);

                if (users != null && users.Count > 0)
                {
                    //如果满足合并条件
                    if (CanJoin(activityDefinition, parallelActivityInstances))
                    {
                        //创建下一活动实例
                        ActivityInstance instance = GenActivityInstanceByDefinition(activityDefinition);
                        instance.WorkflowInstanceId = workflowInstanceId;
                        instance.PrevSetId = prevSetId;
                        SaveActivityInstance(instance, ActivityInstanceActionType.Create);

                        string actor = context.Actor;
                        // 当非流程初始活动，如果当前处理人也在后续步骤的处理人中，直接完成该步骤.
                        if (WorkflowConfig.Instance.AllowContinuousApprove && users.ContainsKey(actor))
                        {
                            ActivityExecutionContext ctx = new ActivityExecutionContext();
                            ctx.Actor = actor;
                            ctx.ActorDescription = context.ActorDescription;
                            ctx.ActivityInstanceId = instance.ActivityInstanceId;
                            ctx.Command = ActivityCommands.Approve;
                            ctx.Reason = "同意(自动连续审批)";
                            instance.Actor = actor;
                            instance.ActorDescription = context.ActorDescription;
                            instance.Command = ActivityCommands.Approve;
                            instance.Reason = ctx.Reason;
                            instance.ExternalEntityId = context.ExternalEntityId;
                            instance.ExternalEntityType = context.ExternalEntityType;

                            Approve(activityDefinition, instance, ctx);
                        }
                        else
                        {
                            //产生待办任务(处理人可能是受委托的).    
                            IDictionary<string, string> usersDict = new Dictionary<string, string>();
                            foreach (KeyValuePair<string, string> pair in users)
                            {
                                if (!String.IsNullOrEmpty(pair.Value))
                                {
                                    usersDict.Add(pair.Key, pair.Value);
                                }
                                InsertTodoActivity(instance.ActivityInstanceId, pair.Key, null, TodoInfo.OpDefault);
                            }

                            if (usersDict.Count > 0)
                            {
                                ExecuteUserProxyNotifier(instance.ActivityInstanceId, usersDict);
                            }
                        }

                        shouldCreatePrevIdSet = true;
                    }
                }
            }

            if (shouldCreatePrevIdSet)
            {
                CreatePrevIdSet(currentInstance.ActivityInstanceId, prevSetId, parallelActivityInstances);
            }
        }

        /// <summary>
        /// 创建下一步流程活动(步骤)实例列表.
        /// </summary>
        /// <param name="nextActivities"></param>
        /// <param name="currentDefinition"></param>
        /// <param name="currentInstance"></param>
        /// <param name="context"></param>
        protected void CreateNextActivityInstances(IList<ActivityDefinition> nextActivities, ActivityDefinition currentDefinition, ActivityInstance currentInstance, ActivityExecutionContext context)
        {
            CreateNextActivityInstances(nextActivities, null, currentDefinition, currentInstance, context);
        }

        /// <summary>
        /// 创建下一步流程活动(步骤)实例列表.
        /// </summary>
        /// <param name="nextActivities"></param>
        /// <param name="nextProcessUsers"></param>
        /// <param name="currentDefinition"></param>
        /// <param name="currentInstance"></param>
        /// <param name="context"></param>
        protected virtual void CreateNextActivityInstances(IList<ActivityDefinition> nextActivities, IList<ICollection<string>> nextProcessUsers, ActivityDefinition currentDefinition, ActivityInstance currentInstance, ActivityExecutionContext context)
        {
            if (null == nextActivities || nextActivities.Count == 0)
            {
                throw new WorkflowException("已经没有后续活动");
            }

            //如果指定了后续处理用户，则nextActivities与nextProcessUsers的列表长度必须一致


            bool hasAssignedNextProcessUsers = (null != nextProcessUsers);
            if (hasAssignedNextProcessUsers
                && nextProcessUsers.Count != nextActivities.Count)
            {
                throw new WorkflowException("后续活动与后续处理人的列表长度不一致");
            }

            //如果下一活动是完成(不对完成结点进行合并条件判断)，认为完成结点只有一个前驱结点


            if (nextActivities.Count == 1 && nextActivities[0].State == WorkflowConstants.Complete)
            {
                Debug("CreateEndActivityInstance");
                CreateEndActivityInstance(nextActivities[0], currentInstance, context);
                return;
            }

            Guid workflowInstanceId = currentInstance.WorkflowInstanceId;

            //创建下一批活动            
            Guid prevSetId = Guid.NewGuid();
            IList<ActivityInstance> parallelActivityInstances = TryGetParallelActivityInstances(currentDefinition, currentInstance);
            bool shouldCreatePrevIdSet = false;
            bool isBack = context.Command.Equals(ActivityCommands.Reject, StringComparison.OrdinalIgnoreCase);
            for (int i = 0, icount = nextActivities.Count; i < icount; i++)
            {
                ActivityDefinition nextDefinition = nextActivities[i];
                Guid activityId = nextDefinition.ActivityId;
                ActivityInstance nextInstance = GenActivityInstanceByDefinition(nextDefinition);
                nextInstance.WorkflowInstanceId = workflowInstanceId;
                nextInstance.OperateType = isBack ? TodoInfo.OpBack : TodoInfo.OpDefault;

                //如果满足合并条件
                if (CanJoin(nextDefinition, parallelActivityInstances))
                {
                    Debug("join activities");

                    nextInstance.PrevSetId = prevSetId;
                    shouldCreatePrevIdSet = true;

                    //创建下一活动实例
                    SaveActivityInstance(nextInstance, ActivityInstanceActionType.Create);

                    //任务分派
                    if (hasAssignedNextProcessUsers)//已指定后续处理人
                    {
                        if (null != nextProcessUsers[i]
                            && nextProcessUsers[i].Count > 0)
                        {
                            IDictionary<string, string> usersDict = null;
                            if (null != userProxy && WorkflowConfig.Instance.AllowUserProxy)
                            {
                                bool hasProxies = false;
                                usersDict = userProxy.GetDistinctProxies(nextProcessUsers[i], out hasProxies);
                                foreach (string key in usersDict.Keys)
                                {
                                    InsertTodoActivity(nextInstance.ActivityInstanceId, key, null, TodoInfo.OpDefault);
                                }

                                if (hasProxies)
                                {
                                    ExecuteUserProxyNotifier(nextInstance.ActivityInstanceId, usersDict);
                                }
                            }
                            else
                            {
                                foreach (string user in nextProcessUsers[i])
                                {
                                    InsertTodoActivity(nextInstance.ActivityInstanceId, user, null, TodoInfo.OpDefault);
                                }
                            }
                        }
                        else
                        {
                            AssignTasks(workflowInstanceId, nextDefinition, nextInstance, context);
                        }
                    }
                    else
                    {
                        AssignTasks(workflowInstanceId, nextDefinition, nextInstance, context);
                    }
                }
                else
                {
                    Debug("can not join activities");
                }
            }

            if (shouldCreatePrevIdSet)
            {
                CreatePrevIdSet(currentInstance.ActivityInstanceId, prevSetId, parallelActivityInstances);
            }
        }

        /// <summary>
        /// 分派任务.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="activityDefinition"></param>
        /// <param name="activityInstance"></param>
        /// <param name="context"></param>
        protected virtual void AssignTasks(Guid workflowInstanceId, ActivityDefinition activityDefinition, ActivityInstance activityInstance, ActivityExecutionContext context)
        {
            IDictionary<string, string> targetDict = activityAllocationService.GetTargetUsers(workflowInstanceId, activityDefinition, context.Actor, context.Variables, false);
            if (null == targetDict || targetDict.Count == 0)
            {
                return;
            }

            AllocateTask(targetDict, activityInstance.ActivityInstanceId, TodoInfo.OpDefault);
        }

        /// <summary>
        /// 暂时不考虑合并条件，默认前驱活动实例需要全部完成.
        /// </summary>
        /// <param name="ad"></param>
        /// <param name="parallelInstances"></param>
        /// <returns></returns>
        protected virtual bool CanJoin(ActivityDefinition ad, IList<ActivityInstance> parallelInstances)
        {
            bool canJoin = true;

            if (null != parallelInstances && parallelInstances.Count > 0)
            {
                //如果有合并条件


                if (null != joinConditionParser
                    && !String.IsNullOrEmpty(ad.JoinCondition))
                {
                    if (null != joinConditionHandlerManager
                        && joinConditionHandlerManager.IsValid(ad.JoinCondition))
                    {
                        string typeName = null;
                        IList<string> ifSelectedActivities = null;
                        IList<string> mustCompletedActivities = null;
                        joinConditionHandlerManager.ParseCondition(ad.JoinCondition, out typeName, out ifSelectedActivities, out mustCompletedActivities);
                        IJoinConditionHandler joinConditionHandler = joinConditionHandlerManager.GetHandler(typeName);
                        if (null != joinConditionHandler)
                        {
                            canJoin = joinConditionHandler.CanJoin(parallelInstances[0].WorkflowInstanceId, ifSelectedActivities, mustCompletedActivities);
                        }
                    }
                    else
                    {
                        IList<string> activities = new List<string>();
                        IList<string> completedActivites = new List<string>();
                        foreach (ActivityInstance instance in parallelInstances)
                        {
                            DebugFormat("parallelInstance [ActivityInstanceId:{0}] [ActivityName:{1}] [IsCompleted:{2}]", instance.ActivityInstanceId, instance.ActivityName, instance.IsCompleted);
                            ActivityDefinition activityDefinition = GetActivityDefinitionById(instance.ActivityId);
                            activities.Add(activityDefinition.ActivityName);
                            if (instance.IsCompleted)
                            {
                                completedActivites.Add(activityDefinition.ActivityName);
                            }
                        }
                        canJoin = joinConditionParser.Parse(ad.JoinCondition, activities, completedActivites);
                    }
                }
                else
                {
                    foreach (ActivityInstance instance in parallelInstances)
                    {
                        DebugFormat("parallelInstance [ActivityInstanceId:{0}] [ActivityName:{1}] [IsCompleted:{2}]", instance.ActivityInstanceId, instance.ActivityName, instance.IsCompleted);
                        //只要有一分支没完成就不可合并
                        if (!instance.IsCompleted)
                        {
                            canJoin = false;
                            break;
                        }
                    }
                }
            }
            return canJoin;
        }

        /// <summary>
        /// 复制并且打开已关闭的活动实例(用于退回).
        /// </summary>
        /// <param name="context">流程活动执行上下文对象.</param>
        /// <param name="activityInstance">流程活动实例对象.</param>
        protected virtual void CopyAndOpenClosedActivityInstance(ActivityExecutionContext context, ActivityInstance activityInstance)
        {
            string actor = activityInstance.Actor;
            activityInstance.ActivityInstanceId = Guid.NewGuid();
            activityInstance.IsCompleted = false;
            activityInstance.Actor = null;
            activityInstance.OperateType = TodoInfo.OpBack;
            SaveActivityInstance(activityInstance, ActivityInstanceActionType.Create);

            //产生待办任务
            InsertTodoActivity(activityInstance.ActivityInstanceId, actor, null, TodoInfo.OpBack);
        }

        /// <summary>
        /// 复制并且打开已关闭的活动实例(用于退回).
        /// </summary>
        /// <param name="context"></param>
        /// <param name="activityInstances"></param>
        protected virtual void CopyAndOpenClosedActivityInstances(ActivityExecutionContext context, ICollection<ActivityInstance> activityInstances)
        {
            // 使用字典，过滤重复步骤.
            IDictionary<Guid, ActivityInstance> backActivities = new Dictionary<Guid, ActivityInstance>();
            foreach (ActivityInstance ai in activityInstances)
            {
                if (!backActivities.ContainsKey(ai.ActivityId))
                    backActivities.Add(ai.ActivityId, ai);
            }

            foreach (ActivityInstance ai in backActivities.Values)
            {
                string actor = ai.Actor;
                ai.ActivityInstanceId = Guid.NewGuid();
                ai.IsCompleted = false;
                ai.Actor = null;
                ai.OperateType = TodoInfo.OpBack;
                SaveActivityInstance(ai, ActivityInstanceActionType.Create);

                //产生待办任务
                InsertTodoActivity(ai.ActivityInstanceId, actor, null, TodoInfo.OpBack);
            }
        }

        /// <summary>
        /// 创建活动实例Id集合.
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="activityInstances"></param>
        protected virtual void CreateActivityInstanceIdSet(Guid setId, ICollection<ActivityInstance> activityInstances)
        {
            foreach (ActivityInstance instance in activityInstances)
            {
                CreateActivityInstanceIdSet(setId, instance.ActivityInstanceId);
            }
        }

        /// <summary>
        /// 分派指定步骤任务.返回被分派任务的人数.
        /// </summary>
        /// <param name="users"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="operateType"></param>
        /// <returns></returns>
        protected virtual void AllocateTask(IDictionary<string, string> users, Guid activityInstanceId, int operateType)
        {
            foreach (string name in users.Keys)
            {
                InsertTodoActivity(activityInstanceId, name, users[name], operateType);
            }
        }

        /// <summary>
        /// 关闭上一路径并行的所有分支.
        /// </summary>
        /// <param name="activityDefinition"></param>
        /// <param name="activityInstance"></param>
        protected virtual void CloseParallelActivityInstances(ActivityDefinition activityDefinition, ActivityInstance activityInstance)
        {
            string actor = activityInstance.Actor;
            IList<ActivityInstance> parallelInstances = TryGetParallelActivityInstances(activityDefinition, activityInstance);
            foreach (ActivityInstance instance in parallelInstances)
            {
                if (!instance.IsCompleted && instance.ActivityInstanceId != activityInstance.ActivityInstanceId)
                {
                    instance.IsCompleted = true;
                    if (string.IsNullOrEmpty(instance.Actor))
                        instance.Actor = actor;
                    instance.ActorDescription = activityInstance.ActorDescription;
                    instance.Command = "close_activities";
                    instance.Reason = "关闭并行的其它分支(系统自动处理)";
                    SaveActivityInstance(instance, ActivityInstanceActionType.Complete);
                }
            }

            if (null != postCloseParallelActivityInstancesHandler)
            {
                postCloseParallelActivityInstancesHandler.Execute(activityInstance, parallelInstances);
            }
        }

        /// <summary>
        /// 根据活动实例获取并行的活动实例.
        /// 如果是开始活动，则返回空列表.
        /// </summary>
        /// <param name="currentDefinition"></param>
        /// <param name="activityInstance"></param>
        /// <returns></returns>
        protected virtual IList<ActivityInstance> TryGetParallelActivityInstances(ActivityDefinition currentDefinition, ActivityInstance activityInstance)
        {
            if (activityInstance.PrevSetId == Guid.Empty)
            {
                return new List<ActivityInstance>();
            }
            else if (currentDefinition.ParallelActivitySetId == Guid.Empty)
            {
                IList<ActivityInstance> activityInstances = new List<ActivityInstance>();
                activityInstances.Add(activityInstance);
                return activityInstances;
            }

            IList<ActivityInstance> results = GetParellelActivityInstances(activityInstance.WorkflowInstanceId, currentDefinition.ParallelActivitySetId);
            results.Add(activityInstance);
            return results;
        }

        /// <summary>
        /// 根据活动实例Id获取活动实例.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        protected virtual ActivityInstance GetActivityInstanceById(Guid activityInstanceId)
        {
            return activityService.GetActivity(activityInstanceId);
        }

        /// <summary>
        /// 根据活动定义Id获取活动定义.
        /// </summary>
        /// <param name="activityDefinitionId"></param>
        /// <returns></returns>
        protected virtual ActivityDefinition GetActivityDefinitionById(Guid activityDefinitionId)
        {
            return activityDefinitionService.GetActivityDefinition(activityDefinitionId);
        }

        /// <summary>
        /// 根据流程Id及活动名称获取活动定义.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="activityNames"></param>
        /// <returns></returns>
        protected virtual IList<ActivityDefinition> GetActivityDefinitionsByActivityNames(Guid workflowId, string[] activityNames)
        {
            return activityDefinitionService.GetActivityDefinitionsByActivityNames(workflowId, activityNames);
        }

        /// <summary>
        /// 根据流程实例Id获取后续活动定义.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        protected virtual IList<ActivityDefinition> GetNextActivityDefinitionsByInstanceId(Guid activityInstanceId)
        {
            return activityDefinitionService.GetNextActivityDefinitionsByInstanceId(activityInstanceId);
        }

        #endregion

        #region protected abstract methods

        /// <summary>
        /// 删除流程实例.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        protected abstract void RemoveWorkflowByActivityInstanceId(Guid activityInstanceId);

        /// <summary>
        /// 更新流程状态.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="state"></param>
        protected abstract void UpdateWorkflowState(Guid activityInstanceId, int state);

        /// <summary>
        /// 插入到待办列表中.
        /// </summary>
        /// <param name="activityInstanceId">步骤实例编号.</param>
        /// <param name="userName">步骤处理人用户名.</param>
        /// <param name="proxyName">代理人用户名.</param>
        /// <param name="operateType">步骤操作类型.
        /// 0: 默认操作;
        /// 1: 退还操作;
        /// 2: 指派操作.
        /// </param>
        protected abstract void InsertTodoActivity(Guid activityInstanceId, string userName, string proxyName, int operateType);

        /// <summary>
        /// 创建活动实例Id集合.
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="activityInstanceId"></param>
        protected abstract void CreateActivityInstanceIdSet(Guid setId, Guid activityInstanceId);

        /// <summary>
        /// 保存活动实例.
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <param name="actionType"></param>
        protected abstract void SaveActivityInstance(ActivityInstance activityInstance, ActivityInstanceActionType actionType);

        /// <summary>
        /// 根据集合Id获取活动列表.
        /// </summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        protected abstract IList<ActivityInstance> GetActivityInstancesBySetId(Guid setId);

        /// <summary>
        /// 关闭此活动实例所属流程实例的所有活动(未完成活动).
        /// </summary>
        /// <param name="context"></param>
        protected abstract void CloseAllActivitiesOfWorkflowInstance(ActivityExecutionContext context);

        /// <summary>
        /// 根据活动实例集合id获取并行的活动实例.
        /// </summary>
        /// <param name="workflowInstanceId">流程活动实例编号.</param>
        /// <param name="setId">活动实例集合id.</param>
        /// <returns></returns>
        protected abstract IList<ActivityInstance> GetParellelActivityInstances(Guid workflowInstanceId, Guid setId);

        /// <summary>
        /// 获取指定流程实例的指定流程步骤定义的前一步骤任务实例列表.
        /// </summary>
        /// <param name="workflowInstanceId">流程实例编号.</param>
        /// <param name="activityId">流程步骤定义.</param>
        /// <returns></returns>
        protected abstract IList<ActivityInstance> GetPrevActivityInstances(Guid workflowInstanceId, Guid activityId);

        /// <summary>
        /// 获取指定的流程步骤操作人的描述字符串.
        /// </summary>
        /// <param name="context">流程步骤上下文对象.</param>
        /// <returns></returns>
        protected abstract string GetActorDescription(ActivityExecutionContext context);

        #endregion

        #region private methods

        /// <summary>
        /// 根据 自定义分支/任务分配决策解析器 或 自定义规则解析 获取后续步骤及处理人
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <param name="activityDefinition"></param>
        /// <returns></returns>
        private IDictionary<string, ICollection<string>> GetNextActivityNamesAndUsersByDicisionParserOrCommandRules(ActivityExecutionContext context, string command, ActivityDefinition activityDefinition)
        {
            // 回退时，不进行自定义处理.
            if (ActivityCommands.Reject.Equals(command, StringComparison.CurrentCultureIgnoreCase))
                return null;

            IDictionary<string, ICollection<string>> nextActivityNamesAndUsers = null;
            IDecisionParser decisionParser = decisionParserManager.GetParser(activityDefinition.DecisionParser);
            if (WorkflowConfig.Instance.AllowDecisionParser && null != decisionParser) //优先执行自定义分支/任务分配决策解析器.
            {
                nextActivityNamesAndUsers = decisionParser.Parse(context);
            }
            else if (WorkflowConfig.Instance.AllowCommandRules) //然后考虑自定义规则解析.
            {
                if (String.IsNullOrEmpty(WorkflowConfig.Instance.CommandsDisabledRules))
                {
                    nextActivityNamesAndUsers = commandRulesParser.Parse(activityDefinition.CommandRules, context);
                }
                else
                {
                    string[] commandsDisabledRules = WorkflowConfig.Instance.CommandsDisabledRules.Split(',');
                    for (int i = 0, icount = commandsDisabledRules.Length; i < icount; i++)
                    {
                        if (command.Equals(commandsDisabledRules[i]))
                        {
                            nextActivityNamesAndUsers = commandRulesParser.Parse(activityDefinition.CommandRules, context);
                            break;
                        }
                    }
                }
            }
            return nextActivityNamesAndUsers;
        }

        /// <summary>
        /// 检查是否已会签完毕.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="activityDefinition"></param>
        /// <returns></returns>
        private bool CheckIfCountersignedCompleted(ActivityExecutionContext context, ActivityDefinition activityDefinition)
        {
            bool isCountersignedCompleted = true;
            IList<TodoInfo> todolist = countersignedService.GetTodoList(context.ActivityInstanceId);
            int count = todolist.Count;
            if (count > 1)
            {
                IList<string> allNodes = new List<string>(todolist.Count);
                IList<string> finishedNodes = new List<string>();
                for (int i = 0; i < count; i++)
                {
                    TodoInfo todo = todolist[i];
                    allNodes.Add(todo.UserName);
                    if (TodoInfo.IsProcessed(todo))
                    {
                        finishedNodes.Add(todo.UserName);
                    }
                    else
                    {
                        //countersignedConditionParser的验证与顺序有关
                        if (context.Actor.Equals(todo.UserName))
                        {
                            finishedNodes.Add(context.Actor);
                        }
                    }
                }

                //如果满足已定义的会签条件，则完成活动
                isCountersignedCompleted = countersignedConditionParser.Parse(
                        activityDefinition.CountersignedCondition,
                        allNodes,
                        finishedNodes);
            }
            return isCountersignedCompleted;
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

        /// <summary>
        /// 创建流程初始步骤(即流程草稿步骤)的活动实例.
        /// </summary>
        /// <param name="workflowInstance">流程实例信息.</param>
        /// <param name="operateType">操作类型.</param>
        private Guid CreateInitActivityInstance(WorkflowInstance workflowInstance, int operateType)
        {
            return CreateInitActivityInstance(workflowInstance, operateType, Guid.Empty);
        }

        /// <summary>
        /// 创建流程初始步骤(即流程草稿步骤)的活动实例. 
        /// 包括从其它活动退回初始活动的情况
        /// </summary>
        /// <param name="workflowInstance">流程实例信息</param>
        /// <param name="operateType">操作类型</param>
        /// <param name="prevSetId">前驱活动</param>
        /// <returns></returns>
        private Guid CreateInitActivityInstance(WorkflowInstance workflowInstance, int operateType, Guid prevSetId)
        {
            ActivityDefinition activity = activityDefinitionService.GetInitailActivityDefinition(workflowInstance.WorkflowId);
            if (null == activity)
            {
                throw new ArgumentException("找不到对应的流程定义.");
            }

            ActivityInstance instance = GenActivityInstanceByDefinition(activity);
            instance.WorkflowInstanceId = workflowInstance.WorkflowInstanceId;
            instance.ExternalEntityId = workflowInstance.ExternalEntityId;
            instance.ExternalEntityType = workflowInstance.ExternalEntityType;
            instance.OperateType = operateType;
            instance.PrevSetId = prevSetId;

            SaveActivityInstance(instance, ActivityInstanceActionType.Create);
            return instance.ActivityInstanceId;
        }

        /// <summary>
        /// 创建前一活动(步骤)集合.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="prevSetId"></param>
        /// <param name="parallelActivityInstances"></param>
        private void CreatePrevIdSet(Guid activityInstanceId, Guid prevSetId, IList<ActivityInstance> parallelActivityInstances)
        {
            //如果没有并行分支（自己也算其中的一个分支），则说明是初始化活动
            if (null == parallelActivityInstances || parallelActivityInstances.Count == 0)
            {
                CreateActivityInstanceIdSet(prevSetId, activityInstanceId);
            }
            else
            {
                CreateActivityInstanceIdSet(prevSetId, parallelActivityInstances);
            }
        }

        #endregion

        #region protected static methods

        /// <summary>
        /// 根据活动定义产生活动实例.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        protected static ActivityInstance GenActivityInstanceByDefinition(ActivityDefinition activity)
        {
            ActivityInstance instance = new ActivityInstance();
            instance.ActivityId = activity.ActivityId;
            instance.ActivityInstanceId = Guid.NewGuid();
            instance.IsCompleted = false;
            return instance;
        }

        /// <summary>
        /// 写调试日志.
        /// </summary>
        /// <param name="message"></param>
        protected static void Debug(string message)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }

        /// <summary>
        /// 写调试日志.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected static void DebugFormat(string message, params object[] args)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat(message, args);
            }
        }

        #endregion
    }
}