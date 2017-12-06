using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 合并条件处理类.
    /// </summary>
    public interface IJoinConditionHandler
    {
        /// <summary>
        /// 是否可以合并.
        /// </summary>
        /// <param name="workflowInstanceId">流程实例Id.</param>
        /// <param name="ifSelectedActivities">如果选择了的活动.</param>
        /// <param name="mustCompletedActivities">需要完成的活动.</param>
        /// <returns></returns>
        bool CanJoin(Guid workflowInstanceId, IList<string> ifSelectedActivities, IList<string> mustCompletedActivities);
    }
}
