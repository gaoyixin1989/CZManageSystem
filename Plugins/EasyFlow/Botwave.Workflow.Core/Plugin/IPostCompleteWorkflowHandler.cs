using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// û��Ԥ�����������ǿ�����/�ر����̵ĺ�������.
    /// </summary>
    public interface IPostCompleteWorkflowHandler : IActivityExecutionHandler
    {
        /// <summary>
        /// ��һ��������������.
        /// </summary>
        IPostCompleteWorkflowHandler Next { get; set; }
    }
}
