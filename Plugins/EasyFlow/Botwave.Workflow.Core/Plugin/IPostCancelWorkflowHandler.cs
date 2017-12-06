using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 取消流程的后续处理器接口.
    /// </summary>
    public interface IPostCancelWorkflowHandler : IActivityExecutionHandler
    {
        /// <summary>
        /// 下一后续处理器对象.
        /// </summary>
        IPostCancelWorkflowHandler Next { get; set; }
    }
}
