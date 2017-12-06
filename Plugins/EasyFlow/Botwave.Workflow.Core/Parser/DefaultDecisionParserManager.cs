using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 默认自定义解析器管理类.
    /// </summary>
    public class DefaultDecisionParserManager : IDecisionParserManager 
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DefaultDecisionParserManager));
        private static IDictionary<string, IDecisionParser> cache = new Dictionary<string, IDecisionParser>();

        #region IDecisionParserManager Members

        /// <summary>
        /// 获取指定类型名称的自定义解析器对象.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public IDecisionParser GetParser(string typeName)
        {
            if (String.IsNullOrEmpty(typeName))
            {
                return null;
            }

            return DoGetParser(typeName);
        }

        #endregion

        /// <summary>
        /// 可以由其它实现来重写此方法.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected virtual IDecisionParser DoGetParser(string typeName)
        {
            return GetParserByTypeName(typeName);
        }

        /// <summary>
        /// 获取指定类型名称的自定义解析器对象.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected static IDecisionParser GetParserByTypeName(string typeName)
        {
            if (cache.ContainsKey(typeName))
            {
                return cache[typeName];
            }

            Type objType = Type.GetType(typeName, true, true);
            IDecisionParser parser = Activator.CreateInstance(objType) as IDecisionParser;
            if (null == parser)
            {
                throw new ArgumentException(String.Format("{0} 不存在", typeName));
            }
            cache.Add(typeName, parser);
            return parser;
        }
    }
}
