using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 完成流程的后续处理器实现类.
    /// </summary>
    public class PostCompleteWorkflowHandler : IPostCompleteWorkflowHandler
    {
        #region IPostCompleteWorkflowHandler 成员

        private IPostCompleteWorkflowHandler next = null;

        /// <summary>
        /// 下一后续处理器.
        /// </summary>
        public IPostCompleteWorkflowHandler Next
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
            // 完成工单时处理

        }

        #endregion
    }
}
