using System;
using System.Collections.Generic;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 活动集合服务接口.
    /// </summary>
    public interface IActivitySetService
    {
        /// <summary>
        /// 获取指定活动集合标识的活动集合.
        /// </summary>
        /// <param name="activitySetId"></param>
        /// <returns></returns>
        IList<ActivitySet> GetActivitySets(Guid activitySetId);

        /// <summary>
        /// 获取指定活动集合标识的活动 ID 列表.
        /// </summary>
        /// <param name="activitySetId"></param>
        /// <returns></returns>
        IList<Guid> GetActivityIdSets(Guid activitySetId);

        /// <summary>
        /// 获取指定流程 ID 的下一活动集合列表.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        IList<ActivitySet> GetNextActivitySets(Guid workflowId);
    }
}
