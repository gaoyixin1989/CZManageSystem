using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 条件解析接口.
    /// </summary>
    public interface IConditionParser
    {
        /// <summary>
        /// 验证.
        /// </summary>
        /// <param name="condition">条件表达式.</param>
        /// <returns></returns>
        bool Validate(string condition);

        /// <summary>
        /// 根据选中节点解析条件规则.
        /// </summary>
        /// <param name="condition">条件表达式.</param>
        /// <param name="allNodes">所有活动节点列表.</param>
        /// <param name="selectedNodes">被选中的活动节点列表.</param>
        /// <returns></returns>
        bool Parse(string condition, IList<string> allNodes, IList<string> selectedNodes);
    }
}
