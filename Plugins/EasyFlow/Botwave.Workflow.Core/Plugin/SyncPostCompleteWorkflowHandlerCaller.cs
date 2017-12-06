using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 流程完成流程的后续处理器异步调用类.
    /// </summary>
    public class SyncPostCompleteWorkflowHandlerCaller : Botwave.Commons.Threading.ISyncCaller
    {
        private IPostCompleteWorkflowHandler postCompleteWorkflowHandler;
        private ActivityExecutionContext activityExecutionContext;

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="postCompleteWorkflowHandler"></param>
        /// <param name="activityExecutionContext"></param>
        public SyncPostCompleteWorkflowHandlerCaller(IPostCompleteWorkflowHandler postCompleteWorkflowHandler,
            ActivityExecutionContext activityExecutionContext)
        {
            this.postCompleteWorkflowHandler = postCompleteWorkflowHandler;
            this.activityExecutionContext = activityExecutionContext;
        }

        #region ISyncCaller Members

        /// <summary>
        /// 调用.
        /// </summary>
        public void Call()
        {
            if (null != postCompleteWorkflowHandler)
            {
                postCompleteWorkflowHandler.Execute(activityExecutionContext);
            }
        }

        #endregion
    }
}
