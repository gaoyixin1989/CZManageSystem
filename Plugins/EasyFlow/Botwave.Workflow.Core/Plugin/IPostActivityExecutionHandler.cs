using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// �ִ�еĺ����������ӿ�.
    /// </summary>
    public interface IPostActivityExecutionHandler : IActivityExecutionHandler
    {
        /// <summary>
        /// ��һ����������.
        /// </summary>
        IPostActivityExecutionHandler Next { get; set; }
    }
}
