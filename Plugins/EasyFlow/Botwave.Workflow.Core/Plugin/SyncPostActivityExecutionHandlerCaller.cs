using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// ���̻(����)ִ�еĺ����������첽������.
    /// </summary>
    public class SyncPostActivityExecutionHandlerCaller : Botwave.Commons.Threading.ISyncCaller
    {
        private IPostActivityExecutionHandler postActivityExecutionHandler;
        private ActivityExecutionContext activityExecutionContext;

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="postActivityExecutionHandler"></param>
        /// <param name="activityExecutionContext"></param>
        public SyncPostActivityExecutionHandlerCaller(IPostActivityExecutionHandler postActivityExecutionHandler,
            ActivityExecutionContext activityExecutionContext)
        {
            this.postActivityExecutionHandler = postActivityExecutionHandler;
            this.activityExecutionContext = activityExecutionContext;
        }

        #region ISyncCaller Members

        /// <summary>
        /// ����.
        /// </summary>
        public void Call()
        {
            if (null != postActivityExecutionHandler)
            {
                postActivityExecutionHandler.Execute(activityExecutionContext);
                IPostActivityExecutionHandler next = postActivityExecutionHandler.Next;
                while (null != next)
                {
                    next.Execute(activityExecutionContext);
                    next = next.Next;
                }
            }
        }

        #endregion
    }
}
