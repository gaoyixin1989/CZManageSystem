using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// ���̻(����)ʵ��.
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
        /// �ʵ��Id.
        /// </summary>
        public Guid ActivityInstanceId
        {
            get { return activityInstanceId; }
            set { activityInstanceId = value; }
        }

        /// <summary>
        /// ��һ�����Id.
        /// </summary>
        public Guid PrevSetId
        {
            get { return prevSetId; }
            set { prevSetId = value; }
        }

        /// <summary>
        /// ����ʵ��Id.
        /// </summary>
        public Guid WorkflowInstanceId
        {
            get { return workflowInstanceId; }
            set { workflowInstanceId = value; }
        }

        /// <summary>
        /// �Id.
        /// </summary>
        public Guid ActivityId
        {
            get { return activityId; }
            set { activityId = value; }
        }

        /// <summary>
        /// �Ƿ����.
        /// </summary>
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }

        /// <summary>
        /// �����������.
        /// 0: Ĭ�ϲ���/ͨ��.
        /// 1: �˻�����.
        /// 2: ָ�ɲ���.
        /// </summary>
        public int OperateType
        {
            get { return _operateType; }
            set { _operateType = value; }
        }

        /// <summary>
        /// ����ʱ��.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        /// <summary>
        /// ���ʱ��.
        /// </summary>
        public DateTime FinishedTime
        {
            get { return finishedTime; }
            set { finishedTime = value; }
        }

        /// <summary>
        /// �������û���.
        /// </summary>
        public string Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        /// <summary>
        /// ���̻����������.
        /// ��ʽ��@����������(��@ί��������).
        /// </summary>
        public string ActorDescription
        {
            get { return actorDescription; }
            set { actorDescription = value; }
        }

        /// <summary>
        /// ���ִ������.
        /// </summary>
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        /// <summary>
        /// ԭ��.
        /// </summary>
        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        /// <summary>
        /// �ⲿʵ������.
        /// </summary>
        public string ExternalEntityType
        {
            get { return externalEntityType; }
            set { externalEntityType = value; }
        }

        /// <summary>
        /// �ⲿʵ��Id.
        /// </summary>
        public string ExternalEntityId
        {
            get { return externalEntityId; }
            set { externalEntityId = value; }
        }
        #endregion

        /// <summary>
        /// ���췽��.
        /// </summary>
        public ActivityInstance()
        {
            this.OperateType = 0;
            this.createdTime = DateTime.Now;
        }

        #region �ǳ־û�

        /// <summary>
        /// ���̻ʵ����ʵ����������.
        /// </summary>
        public static readonly string EntityType = "bwwf_Tracking_Activities";

        private string activityName;
        private string workItemTitle;
        private string countersignedCondition;

        /// <summary>
        /// �����.
        /// </summary>
        public string ActivityName
        {
            get { return activityName; }
            set { activityName = value; }
        }

        /// <summary>
        /// ���������.
        /// </summary>
        public string WorkItemTitle
        {
            get { return workItemTitle; }
            set { workItemTitle = value; }
        }

        /// <summary>
        /// ��ǩ����.
        /// </summary>
        public string CountersignedCondition
        {
            get { return countersignedCondition; }
            set { countersignedCondition = value; }
        }

        #endregion
    }
}
