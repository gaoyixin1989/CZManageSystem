using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// ���̻(����)ִ�д���ӿ�.
    /// </summary>
    public interface IActivityExecutionHandler
    {
        /// <summary>
        /// ִ��.
        /// </summary>
        /// <param name="context"></param>
        void Execute(ActivityExecutionContext context);
    }
}
