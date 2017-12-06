using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 取消流程的后续处理器实现类.
    /// </summary>
    public class PostCancelWorkflowHandler : IPostCancelWorkflowHandler
    {
        #region IPostCancelWorkflowHandler 成员

        private IPostCancelWorkflowHandler next = null;

        /// <summary>
        /// 下一后续处理器对象.
        /// </summary>
        public IPostCancelWorkflowHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        #endregion

        #region IActivityExecutionHandler 成员

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(ActivityExecutionContext context)
        {
            // 执行取消工单的处理

        }

        #endregion
    }
}
