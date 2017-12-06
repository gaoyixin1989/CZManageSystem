using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;

namespace Botwave.XQP.Web.Controls
{
    /// <summary>
    /// 流程用户控件.
    /// </summary>
    public class WorkflowControlBase : Botwave.Security.Web.UserControlBase
    {
        /// <summary>
        /// 构造方法.
        /// </summary>
        public WorkflowControlBase()
            : base()
        { }

        /// <summary>
        /// 控件所有者流程名称集合.
        /// </summary>
        protected NameValueCollection ownerWorkflowNames = new NameValueCollection(StringComparer.CurrentCultureIgnoreCase);

        /// <summary>
        /// 控件所有者流程名称列表.
        /// </summary>
        public virtual IList<string> OwnerWorkflows
        {
            get
            {
                IList<string> results = new List<string>();
                foreach (string key in ownerWorkflowNames)
                    results.Add(key);
                return results;
            }
            set
            {
                foreach (string key in value)
                {
                    ownerWorkflowNames[key] = key;
                }
            }
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        /// <param name="workflowName"></param>
        public virtual void Initialize(string workflowName)
        {
            if (!string.IsNullOrEmpty(workflowName) && !this.HasOwner(workflowName))
                this.Visible = false;
        }

        /// <summary>
        /// 判断是否存在指定所有者流程.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        protected virtual bool HasOwner(string workflowName)
        {
            return (!string.IsNullOrEmpty(ownerWorkflowNames[workflowName]));
        }
    }
}
