using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;

namespace Botwave.XQP.Service.Plugins
{
    public class PostCancelWorkflowHandler : IPostCancelWorkflowHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PostCancelWorkflowHandler));

        #region IPostCancelWorkflowHandler Members

        private IPostCancelWorkflowHandler next;

        public IPostCancelWorkflowHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        #endregion

        #region IActivityExecutionHandler Members

        public void Execute(ActivityExecutionContext context)
        {
            log.Info("execute PostCancelWorkflowHandler ...");
            WorkflowPostHelper.PostCancelWorkflowInstance(context.ActivityInstanceId);
        }

        #endregion
    }
}
