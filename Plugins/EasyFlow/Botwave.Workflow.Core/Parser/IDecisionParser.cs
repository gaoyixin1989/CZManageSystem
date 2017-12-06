using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 根据上下文解析出后续步骤及处理人.
    /// </summary>
    public interface IDecisionParser
    {
        /// <summary>
        /// 解析.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IDictionary<string, ICollection<string>> Parse(ActivityExecutionContext context);
    }
}
