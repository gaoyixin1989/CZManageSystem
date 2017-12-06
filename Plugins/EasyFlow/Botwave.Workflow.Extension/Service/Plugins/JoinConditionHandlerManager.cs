using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Parser;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 合并条件处理管理器的默认实现类.
    /// </summary>
    public class JoinConditionHandlerManager : DefaultJoinConditionHandlerManager, Spring.Context.IApplicationContextAware
    {
        private Spring.Context.IApplicationContext applicationContext;

        /// <summary>
        /// 流程活动处理器管理者.
        /// 增加了对由Spring来管理的处理器的支持.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected override IJoinConditionHandler DoGetHandler(string typeName)
        {
            if (applicationContext.ContainsObject(typeName))
            {
                return applicationContext[typeName] as IJoinConditionHandler;
            }
            return GetHandlerByTypeName(typeName);
        }

        #region IApplicationContextAware Members

        /// <summary>
        /// Application 上下文属性.
        /// </summary>
        public Spring.Context.IApplicationContext ApplicationContext
        {
            get { return applicationContext; }
            set { applicationContext = value; }
        }

        #endregion
    }
}
