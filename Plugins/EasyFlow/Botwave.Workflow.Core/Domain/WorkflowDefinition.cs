using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 流程定义类.
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
        /// 流程定义 Id.
        /// </summary>
        public Guid WorkflowId
        {
            get { return workflowId; }
            set { workflowId = value; }
        }

        /// <summary>
        /// 流程名称.
        /// </summary>
        public string WorkflowName
        {
            get { return workflowName; }
            set { workflowName = value; }
        }

        /// <summary>
        /// 拥有者.
        /// </summary>
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /// <summary>
        /// 流程定义是否有效.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// 流程定义是否当前版本.
        /// </summary>
        public bool IsCurrent
        {
            get { return isCurrent; }
            set { isCurrent = value; }
        }

        /// <summary>
        /// 版本号.
        /// </summary>
        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 创建人.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 备注.
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 最后更新人.
        /// </summary>
        public string LastModifier
        {
            get { return lastModifier; }
            set { lastModifier = value; }
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
        /// 最后更新时间.
        /// </summary>
        public DateTime LastModTime
        {
            get { return lastModTime; }
            set { lastModTime = value; }
        }

        #region 非持久化属性

        private string workflowAlias;

        /// <summary>
        /// 流程别名.
        /// </summary>
        public string WorkflowAlias
        {
            get { return workflowAlias; }
            set { workflowAlias = value; }
        }
        #endregion
    }
}
