using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 命令规则解析接口.
    /// </summary>
    public interface ICommandRulesParser
    {
        /// <summary>
        /// 根据传入命令解析规则，返回后续步骤及处理人.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        IDictionary<string, ICollection<string>> Parse(string rules, string command);

        /// <summary>
        /// 根据上下文解析规则，返回后续步骤及处理人.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        IDictionary<string, ICollection<string>> Parse(string rules, ActivityExecutionContext context);
    }
}
