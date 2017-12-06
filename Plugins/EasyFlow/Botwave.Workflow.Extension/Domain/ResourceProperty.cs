using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Extension.Util;

namespace Botwave.Workflow.Extension.Domain
{
    /// <summary>
    /// 流程权限资源类.
    /// </summary>
    [Serializable]
    public class ResourceProperty
    {
        #region gets / sets

        private string resourceId;
        private string name;
        private string workflowName;
        private string parentId;
        private string type;
        private bool visible;
        private int sortIndex;
        private string alias;

        /// <summary>
        /// 权限资源编号.
        /// </summary>
        public string ResourceId
        {
            get { return resourceId; }
            set { resourceId = value; }
        }

        /// <summary>
        /// 父权限资源编号.
        /// </summary>
        public string ParentId
        {
            get { return parentId; }
            set { parentId = value; }
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
        /// 权限资源名称("流程名称-流程步骤名称").
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    name = (this.Type == ResourceHelper.ResourceType_Workflow) ? this.WorkflowName : string.Format("{0}-{1}", this.WorkflowName, this.Alias);
                return name;
            }
            set { name = value; }
        }

        /// <summary>
        /// 权限资源的类型.
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 权限资源的可见性.
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        /// 排序索引.
        /// </summary>
        public int SortIndex
        {
            get { return sortIndex; }
            set { sortIndex = value; }
        }

        /// <summary>
        /// 获取流程权限资源别名(流程步骤名称).
        /// </summary>
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        #endregion

        /// <summary>
        /// 构造方法.
        /// </summary>
        public ResourceProperty()
        {
            this.visible = true;
            this.sortIndex = 1;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="parentId"></param>
        /// <param name="alias"></param>
        /// <param name="workflowName"></param>
        public ResourceProperty(string resourceId, string parentId, string alias, string workflowName)
            : this()
        {
            this.resourceId = resourceId;
            this.parentId = parentId;
            this.alias = alias;
            this.workflowName = workflowName;
        }
    }
}
