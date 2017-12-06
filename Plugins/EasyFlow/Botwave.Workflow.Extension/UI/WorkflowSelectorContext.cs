using System;
using System.Collections.Generic;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Extension.UI
{
    /// <summary>
    /// 流程选择器上下对象类.
    /// </summary>
    [Serializable]
    public class WorkflowSelectorContext
    {
        private Guid _workflowInstanceId;
        private Guid _workflowId;
        private Guid _activityInstanceId;
        private Guid _activityId;
        private string _activityName;
        private string _actor;
        private string _splitCondition;
        private WorkflowInstance _workflowInstance;
        private IList<ActivityDefinition> _nextActivities;

        /// <summary>
        /// 当前流程实例编号.
        /// </summary>
        public Guid WorkflowInstanceId
        {
            get { return _workflowInstanceId; }
            set { _workflowInstanceId = value; }
        }

        /// <summary>
        /// 当前流程定义编号.
        /// </summary>
        public Guid WorkflowId
        {
            get { return _workflowId; }
            set { _workflowId = value; }
        }

        /// <summary>
        /// 当前流程活动(步骤)实例编号.
        /// </summary>
        public Guid ActivityInstanceId
        {
            get { return _activityInstanceId; }
            set { _activityInstanceId = value; }
        }

        /// <summary>
        /// 当前流程定义编号.
        /// </summary>
        public Guid ActivityId
        {
            get { return _activityId; }
            set { _activityId = value; }
        }

        /// <summary>
        /// 当前步骤名称.
        /// </summary>
        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }

        /// <summary>
        /// 当前步骤处理人.
        /// </summary>
        public string Actor
        {
            get { return _actor; }
            set { _actor = value; }
        }

        /// <summary>
        /// 当前步骤分支条件.
        /// </summary>
        public string SplitCondition
        {
            get { return _splitCondition; }
            set { _splitCondition = value; }
        }

        /// <summary>
        /// 流程实例对象.
        /// </summary>
        public WorkflowInstance WorkflowInstance
        {
            get { return _workflowInstance; }
            set { _workflowInstance = value; }
        }

        /// <summary>
        /// 下行步骤列表.
        /// </summary>
        public IList<ActivityDefinition> NextActivities
        {
            get { return _nextActivities; }
            set { _nextActivities = value; }
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public WorkflowSelectorContext()
        {
            this._workflowInstanceId = Guid.Empty;
            this._activityInstanceId = Guid.Empty;
            this._workflowInstance = null;
            this._nextActivities = new List<ActivityDefinition>();
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="activity">流程活动定义对象.</param>
        public WorkflowSelectorContext(ActivityDefinition activity)
            : this()
        {
            if (activity != null)
            {
                this._workflowId = activity.WorkflowId;
                this._activityId = activity.ActivityId;
                this._activityName = activity.ActivityName;
                this._splitCondition = activity.SplitCondition;
            }
        }

        #region ActivityActor

        /// <summary>
        /// 流程活动处理信息类.
        /// </summary>
        public class ActivityActor
        {
            private Guid activityId;
            private string activityName;
            private IDictionary<string, string> actors;
            private bool hasSuperior = false;

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="activityId"></param>
            /// <param name="activityName"></param>
            public ActivityActor(Guid activityId, string activityName)
                : this(activityId, activityName, new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase))
            { }

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="activityId"></param>
            /// <param name="activityName"></param>
            /// <param name="actors"></param>
            public ActivityActor(Guid activityId, string activityName, IDictionary<string, string> actors)
            {
                this.activityId = activityId;
                this.activityName = activityName;
                this.actors = actors;
            }

            /// <summary>
            /// 流程活动定义编号.
            /// </summary>
            public Guid ActivityId
            {
                get { return activityId; }
                set { activityId = value; }
            }

            /// <summary>
            /// 流程活动名称.
            /// </summary>
            public string ActivityName
            {
                get { return activityName; }
                set { activityName = value; }
            }

            /// <summary>
            /// 流程活动处理人.
            /// </summary>
            public IDictionary<string, string> Actors
            {
                get { return actors; }
                set { actors = value; }
            }

            /// <summary>
            /// 是否有部门审核的任务分派.
            /// </summary>
            public bool HasSuperior
            {
                get { return hasSuperior; }
                set { hasSuperior = value; }
            }
        }
        #endregion
    }
}
