using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// ȡ�����̵ĺ����������ӿ�.
    /// </summary>
    public interface IPostCancelWorkflowHandler : IActivityExecutionHandler
    {
        /// <summary>
        /// ��һ��������������.
        /// </summary>
        IPostCancelWorkflowHandler Next { get; set; }
    }
}
