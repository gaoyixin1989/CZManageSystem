using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// ����������̵ĺ����������첽������.
    /// </summary>
    public class SyncPostCompleteWorkflowHandlerCaller : Botwave.Commons.Threading.ISyncCaller
    {
        private IPostCompleteWorkflowHandler postCompleteWorkflowHandler;
        private ActivityExecutionContext activityExecutionContext;

        /// <summary>
        /// ���췽��.
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
        /// ����.
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
