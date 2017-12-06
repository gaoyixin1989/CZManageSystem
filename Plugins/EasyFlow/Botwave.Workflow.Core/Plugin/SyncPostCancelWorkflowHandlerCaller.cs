using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// ����ȡ��(����)���̵ĺ����������첽������.
    /// </summary>
    public class SyncPostCancelWorkflowHandlerCaller : Botwave.Commons.Threading.ISyncCaller
    {
        private IPostCancelWorkflowHandler postCancelWorkflowHandler;
        private ActivityExecutionContext activityExecutionContext;

        /// <summary>
        /// ���췽��.
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
        /// ����.
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
