using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 合并条件管理器.
    /// condition格式:
    /// type:typeName;args:activity1+activity2,activity12+activity22
    /// </summary>
    public interface IJoinConditionHandlerManager
    {
        /// <summary>
        /// 是否有效.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool IsValid(string condition);

        /// <summary>
        /// 解析合并条件.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="typeName"></param>
        /// <param name="ifSelectedActivities"></param>
        /// <param name="mustCompletedActivities"></param>
        void ParseCondition(string condition, out string typeName, out IList<string> ifSelectedActivities, out IList<string> mustCompletedActivities); 

        /// <summary>
        /// 获取合并条件处理器.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        IJoinConditionHandler GetHandler(string typeName);
    }
}
