using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// ���̻(����)ִ�д������첽������.
    /// </summary>
    public class SyncActivityExecutionHandlerCaller : Botwave.Commons.Threading.ISyncCaller
    {
        private IActivityExecutionHandler activityExecutionHandler;
        private ActivityExecutionContext activityExecutionContext;

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="activityExecutionHandler"></param>
        /// <param name="activityExecutionContext"></param>
        public SyncActivityExecutionHandlerCaller(IActivityExecutionHandler activityExecutionHandler,
            ActivityExecutionContext activityExecutionContext)
        {
            this.activityExecutionHandler = activityExecutionHandler;
            this.activityExecutionContext = activityExecutionContext;
        }

        #region ISyncCaller Members

        /// <summary>
        /// ����.
        /// </summary>
        public void Call()
        {
            if (null != activityExecutionHandler)
            {
                activityExecutionHandler.Execute(activityExecutionContext);
            }
        }

        #endregion
    }
}
