using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 流程活动(步骤)执行处理器异步调用类.
    /// </summary>
    public class SyncActivityExecutionHandlerCaller : Botwave.Commons.Threading.ISyncCaller
    {
        private IActivityExecutionHandler activityExecutionHandler;
        private ActivityExecutionContext activityExecutionContext;

        /// <summary>
        /// 构造方法.
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
        /// 调用.
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
