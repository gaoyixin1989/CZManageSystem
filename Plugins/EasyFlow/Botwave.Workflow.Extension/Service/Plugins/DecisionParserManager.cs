using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow;
using Botwave.Workflow.Parser;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 决策解析管理类.
    /// </summary>
    public class DecisionParserManager : DefaultDecisionParserManager, Spring.Context.IApplicationContextAware
    {
        private Spring.Context.IApplicationContext applicationContext;

        /// <summary>
        /// 获取指定类型的决策解析对象.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected override IDecisionParser DoGetParser(string typeName)
        {
            if (applicationContext.ContainsObject(typeName))
            {
                return applicationContext[typeName] as IDecisionParser;
            }

            return GetParserByTypeName(typeName);
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
