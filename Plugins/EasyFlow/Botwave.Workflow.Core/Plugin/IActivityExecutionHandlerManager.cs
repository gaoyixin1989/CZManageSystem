using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// ���̻(����)�Զ���ִ�д���������ӿ�.
    /// </summary>
    public interface IActivityExecutionHandlerManager
    {
        /// <summary>
        /// ��ȡ�Զ��崦����.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        IActivityExecutionHandler GetHandler(string typeName);

        /// <summary>
        /// ִ���Զ��崦����.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="context"></param>
        void Execute(string typeName, ActivityExecutionContext context);
    }
}
