using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Botwave.Workflow;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 流程步骤执行处理器管理类.
    /// </summary>
    public class ActivityExecutionHandlerManager : DefaultActivityExecutionHandlerManager, Spring.Context.IApplicationContextAware
    {
        private Spring.Context.IApplicationContext applicationContext;

        /// <summary>
        /// 流程活动处理器管理者.
        /// 增加了对由Spring来管理的处理器的支持.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected override IActivityExecutionHandler DoGetHandler(string typeName)
        {
            if (applicationContext.ContainsObject(typeName))
            {
                return applicationContext[typeName] as IActivityExecutionHandler;
            }
            return GetHandlerByTypeName(typeName);
        }

        #region IApplicationContextAware Members

        /// <summary>
        /// 应用程序上下文.
        /// </summary>
        public Spring.Context.IApplicationContext ApplicationContext
        {
            get { return applicationContext; }
            set { applicationContext = value; }
        }

        #endregion
    }
}
