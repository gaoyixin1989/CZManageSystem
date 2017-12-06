using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 流程取消(废弃)流程的后续处理器异步调用类.
    /// </summary>
    public class SyncPostCancelWorkflowHandlerCaller : Botwave.Commons.Threading.ISyncCaller
    {
        private IPostCancelWorkflowHandler postCancelWorkflowHandler;
        private ActivityExecutionContext activityExecutionContext;

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="postCancelWorkflowHandler"></param>
        /// <param name="activityExecutionContext"></param>
        public SyncPostCancelWorkflowHandlerCaller(IPostCancelWorkflowHandler postCancelWorkflowHandler,
            ActivityExecutionContext activityExecutionContext)
        {
            this.postCancelWorkflowHandler = postCancelWorkflowHandler;
            this.activityExecutionContext = activityExecutionContext;
        }

        #region ISyncCaller Members

        /// <summary>
        /// 调用.
        /// </summary>
        public void Call()
        {
            if (null != postCancelWorkflowHandler)
            {
                postCancelWorkflowHandler.Execute(activityExecutionContext);
            }
        }

        #endregion
    }
}
