using System;
using System.Collections.Generic;
using Botwave.Commons;
using Botwave.Entities;
using Botwave.Workflow.Domain;
using System.Data;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 活动定义服务接口.
    /// </summary>
    public interface IActivityDefinitionService
    {
        /// <summary>
        /// 获取初始化的活动定义.
        /// </summary>
        /// <param name="workflowId">流程标识.</param>
        /// <returns></returns>
        ActivityDefinition GetInitailActivityDefinition(Guid workflowId);

        /// <summary>
        /// 获取所有步骤
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetInitailActivityDefinitionList(Guid workflowId, string activityName);

        /// <summary>
        ///  获取初始化的活动定义列表
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        DataTable GetInitailActivityDefinitionList(Guid workflowInstanceId, Guid activityId);

        /// <summary>
        /// 获取指定流程定义起始可选择活动定义列表.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetStartActivities(Guid workflowId);

        /// <summary>
        /// 获取指定流程实例起始可选择活动定义列表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetStartActivitiesByWorkflowInstanceId(Guid workflowInstanceId);

        /// <summary>
        /// 获取指定活动标识的活动定义.
        /// </summary>
        /// <param name="activityId">活动标识.</param>
        /// <returns></returns>
        ActivityDefinition GetActivityDefinition(Guid activityId);

        /// <summary>
        /// 根据活动实例获取指定的活动定义.
        /// </summary>
        /// <param name="activityInstanceId">活动实例标识.</param>
        /// <returns></returns>
        ActivityDefinition GetActivityDefinitionByInstanceId(Guid activityInstanceId);

        /// <summary>
        /// 获取指定活动定义标识的上一活动定义列表.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetPrevActivityDefinitions(Guid activityId);

        /// <summary>
        /// 获取本活动的上一步活动定义列表.
        /// </summary>
        /// <param name="activityInstaceId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetPrevActivityDefinitionsByInstanceId(Guid activityInstaceId);

        /// <summary>
        /// 获取本活动的下一步活动定义列表.
        /// </summary>
        /// <param name="activityInstaceId">活动实例标识.</param>
        /// <returns></returns>
        IList<ActivityDefinition> GetNextActivityDefinitionsByInstanceId(Guid activityInstaceId);

        /// <summary>
        /// 根据流程及步骤名称获取下一步活动.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="activityNames"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetActivityDefinitionsByActivityNames(Guid workflowId, string[] activityNames);

        /// <summary>
        /// 根据流程ＩＤ获取所有活动信息.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetActivitiesByWorkflowId(Guid workflowId);

        /// <summary>
        /// 根据流程ＩＤ和当前活动 ID 获取前面步骤部分活动信息.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="currentActivityId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetPartActivities(Guid workflowId, Guid currentActivityId);

        /// <summary>
        /// 根据流程ID 获取平已经排序所有活动信息.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetSortedActivitiesByWorkflowId(Guid workflowId);

        /// <summary>
        /// 获取本活动的全部流程.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetAllActivityDefinition(Guid activityInstanceId);

        /// <summary>
        /// 从指定流程获取指定起始步骤的流程路由列表.
        /// </summary>
        /// <param name="workflowId">流程编号.</param>
        /// <param name="startActivityId">起始步骤编号.</param>
        /// <returns></returns>
        IList<WorkflowRoute> GetWorkflowRoute(Guid workflowId, Guid startActivityId);

        /// <summary>
        /// 获取指定流程定义的流程活动(步骤)数.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        int GetActivityCountByWorkflowId(Guid workflowId);

        /// <summary>
        /// 更新流程活动定义的任务分派.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        int UpdateActivityAllocators(ActivityDefinition activity);
    }
}
