using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 没按预定步骤走完就强制完成/关闭流程的后续处理.
    /// </summary>
    public interface IPostCompleteWorkflowHandler : IActivityExecutionHandler
    {
        /// <summary>
        /// 下一后续处理器对象.
        /// </summary>
        IPostCompleteWorkflowHandler Next { get; set; }
    }
}
