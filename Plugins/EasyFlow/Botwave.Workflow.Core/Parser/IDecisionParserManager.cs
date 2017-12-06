using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 分支决策解析器管理接口.
    /// </summary>
    public interface IDecisionParserManager
    {
        /// <summary>
        /// 获取自定义解析器
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        IDecisionParser GetParser(string typeName);
    }
}
