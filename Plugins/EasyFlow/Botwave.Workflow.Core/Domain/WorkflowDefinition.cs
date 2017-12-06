using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// ���̶�����.
    /// </summary>
    public class WorkflowDefinition
    {
        private Guid workflowId;
        private string workflowName;
        private string owner;
        private bool enabled;
        private bool isCurrent;
        private int version;
        private string creator;
        private string remark;
        private string lastModifier;
        private DateTime createdTime;
        private DateTime lastModTime;

        /// <summary>
        /// ���̶��� Id.
        /// </summary>
        public Guid WorkflowId
        {
            get { return workflowId; }
            set { workflowId = value; }
        }

        /// <summary>
        /// ��������.
        /// </summary>
        public string WorkflowName
        {
            get { return workflowName; }
            set { workflowName = value; }
        }

        /// <summary>
        /// ӵ����.
        /// </summary>
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /// <summary>
        /// ���̶����Ƿ���Ч.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// ���̶����Ƿ�ǰ�汾.
        /// </summary>
        public bool IsCurrent
        {
            get { return isCurrent; }
            set { isCurrent = value; }
        }

        /// <summary>
        /// �汾��.
        /// </summary>
        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// ������.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// ��ע.
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// ��������.
        /// </summary>
        public string LastModifier
        {
            get { return lastModifier; }
            set { lastModifier = value; }
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
        /// ������ʱ��.
        /// </summary>
        public DateTime LastModTime
        {
            get { return lastModTime; }
            set { lastModTime = value; }
        }

        #region �ǳ־û�����

        private string workflowAlias;

        /// <summary>
        /// ���̱���.
        /// </summary>
        public string WorkflowAlias
        {
            get { return workflowAlias; }
            set { workflowAlias = value; }
        }
        #endregion
    }
}
