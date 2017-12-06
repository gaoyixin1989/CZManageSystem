using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 流程活动(步骤)执行处理的管理器的默认实现类.
    /// </summary>
    public class DefaultActivityExecutionHandlerManager : IActivityExecutionHandlerManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DefaultActivityExecutionHandlerManager));
        private static IDictionary<string, IActivityExecutionHandler> cache = new Dictionary<string, IActivityExecutionHandler>();

        #region IActivityExecutionHandlerManager Members

        /// <summary>
        /// 获取指定类型的流程活动处理器.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public IActivityExecutionHandler GetHandler(string typeName)
        {
            if (String.IsNullOrEmpty(typeName))
            {
                return null;
            }

            return DoGetHandler(typeName);
        }

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="context"></param>
        public void Execute(string typeName, ActivityExecutionContext context)
        {
            IActivityExecutionHandler handler = GetHandler(typeName);
            if (null != handler)
            {
                log.Debug("executing handler " + typeName);
                handler.Execute(context);
            }
        }

        #endregion

        /// <summary>
        /// 获取指定类型名称的流程活动(步骤)执行处理器对象.
        /// 可以由其它实现来重写此方法.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected virtual IActivityExecutionHandler DoGetHandler(string typeName)
        {
            return GetHandlerByTypeName(typeName);
        }

        /// <summary>
        /// 获取指定类型名称的流程活动(步骤)执行处理器对象.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected static IActivityExecutionHandler GetHandlerByTypeName(string typeName)
        {
            if (cache.ContainsKey(typeName))
            {
                return cache[typeName];
            }

            Type objType = Type.GetType(typeName, true, true);
            IActivityExecutionHandler handler = Activator.CreateInstance(objType) as IActivityExecutionHandler;
            if (null == handler)
            {
                throw new ArgumentException(String.Format("{0} 不存在", typeName));
            }
            cache.Add(typeName, handler);
            return handler;
        }
    }
}
