using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    public interface IActivitySystemExtentionHandler : IActivityExecutionHandler
    {
        /// <summary>
        /// 下一后续处理器对象.
        /// </summary>
        IActivitySystemExtentionHandler Next { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="actor"></param>
        /// <param name="dataVariables"></param>
        void Execute(ActivityExecutionContext context, IDictionary<string, object> dataVariables);
    }
}
