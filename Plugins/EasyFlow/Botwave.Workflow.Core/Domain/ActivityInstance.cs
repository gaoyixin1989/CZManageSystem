using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 流程活动(步骤)实例.
    /// </summary>
    public class ActivityInstance
    {
        #region getter/setter
        private Guid activityInstanceId;
        private Guid prevSetId;
        private Guid workflowInstanceId;
        private Guid activityId;
        private bool isCompleted;
        private int _operateType;
        private DateTime createdTime;
        private DateTime finishedTime;
        private string actor;
        private string actorDescription;
        private string command;
        private string reason;
        private string externalEntityType;
        private string externalEntityId;

        /// <summary>
        /// 活动实例Id.
        /// </summary>
        public Guid ActivityInstanceId
        {
            get { return activityInstanceId; }
            set { activityInstanceId = value; }
        }

        /// <summary>
        /// 上一活动集合Id.
        /// </summary>
        public Guid PrevSetId
        {
            get { return prevSetId; }
            set { prevSetId = value; }
        }

        /// <summary>
        /// 流程实例Id.
        /// </summary>
        public Guid WorkflowInstanceId
        {
            get { return workflowInstanceId; }
            set { workflowInstanceId = value; }
        }

        /// <summary>
        /// 活动Id.
        /// </summary>
        public Guid ActivityId
        {
            get { return activityId; }
            set { activityId = value; }
        }

        /// <summary>
        /// 是否完成.
        /// </summary>
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }

        /// <summary>
        /// 步骤操作类型.
        /// 0: 默认操作/通过.
        /// 1: 退还操作.
        /// 2: 指派操作.
        /// </summary>
        public int OperateType
        {
            get { return _operateType; }
            set { _operateType = value; }
        }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        /// <summary>
        /// 完成时间.
        /// </summary>
        public DateTime FinishedTime
        {
            get { return finishedTime; }
            set { finishedTime = value; }
        }

        /// <summary>
        /// 参与者用户名.
        /// </summary>
        public string Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        /// <summary>
        /// 流程活动处理人描述.
        /// 格式：@处理人姓名(代@委托人姓名).
        /// </summary>
        public string ActorDescription
        {
            get { return actorDescription; }
            set { actorDescription = value; }
        }

        /// <summary>
        /// 活动所执行命令.
        /// </summary>
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        /// <summary>
        /// 原因.
        /// </summary>
        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        /// <summary>
        /// 外部实例类型.
        /// </summary>
        public string ExternalEntityType
        {
            get { return externalEntityType; }
            set { externalEntityType = value; }
        }

        /// <summary>
        /// 外部实体Id.
        /// </summary>
        public string ExternalEntityId
        {
            get { return externalEntityId; }
            set { externalEntityId = value; }
        }
        #endregion

        /// <summary>
        /// 构造方法.
        /// </summary>
        public ActivityInstance()
        {
            this.OperateType = 0;
            this.createdTime = DateTime.Now;
        }

        #region 非持久化

        /// <summary>
        /// 流程活动实例的实体类型名称.
        /// </summary>
        public static readonly string EntityType = "bwwf_Tracking_Activities";

        private string activityName;
        private string workItemTitle;
        private string countersignedCondition;

        /// <summary>
        /// 活动名称.
        /// </summary>
        public string ActivityName
        {
            get { return activityName; }
            set { activityName = value; }
        }

        /// <summary>
        /// 工作项标题.
        /// </summary>
        public string WorkItemTitle
        {
            get { return workItemTitle; }
            set { workItemTitle = value; }
        }

        /// <summary>
        /// 会签条件.
        /// </summary>
        public string CountersignedCondition
        {
            get { return countersignedCondition; }
            set { countersignedCondition = value; }
        }

        #endregion
    }
}
