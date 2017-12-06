using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 流程活动(步骤)执行处理接口.
    /// </summary>
    public interface IActivityExecutionHandler
    {
        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="context"></param>
        void Execute(ActivityExecutionContext context);
    }
}
