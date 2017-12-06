using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 活动执行的后续处理器接口.
    /// </summary>
    public interface IPostActivityExecutionHandler : IActivityExecutionHandler
    {
        /// <summary>
        /// 下一处理器对象.
        /// </summary>
        IPostActivityExecutionHandler Next { get; set; }
    }
}
