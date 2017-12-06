using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// ���̻(����)ִ�д���Ĺ�������Ĭ��ʵ����.
    /// </summary>
    public class DefaultActivityExecutionHandlerManager : IActivityExecutionHandlerManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DefaultActivityExecutionHandlerManager));
        private static IDictionary<string, IActivityExecutionHandler> cache = new Dictionary<string, IActivityExecutionHandler>();

        #region IActivityExecutionHandlerManager Members

        /// <summary>
        /// ��ȡָ�����͵����̻������.
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
        /// ִ��.
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
        /// ��ȡָ���������Ƶ����̻(����)ִ�д���������.
        /// ����������ʵ������д�˷���.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected virtual IActivityExecutionHandler DoGetHandler(string typeName)
        {
            return GetHandlerByTypeName(typeName);
        }

        /// <summary>
        /// ��ȡָ���������Ƶ����̻(����)ִ�д���������.
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
                throw new ArgumentException(String.Format("{0} ������", typeName));
            }
            cache.Add(typeName, handler);
            return handler;
        }
    }
}
