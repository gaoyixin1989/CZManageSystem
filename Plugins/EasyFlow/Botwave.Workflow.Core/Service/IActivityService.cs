using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 活动实例服务接口.
    /// </summary>
    public interface IActivityService
    {
        /// <summary>
        /// 获取活动实例.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        ActivityInstance GetActivity(Guid activityInstanceId);

        /// <summary>
        /// 根据指定流程实例标识和活动标识的活动实例列表.
        /// </summary>
        /// <param name="workflowInstanceId">流程实例标识.</param>
        /// <param name="activityId">活动标识.</param>
        /// <returns></returns>
        IList<ActivityInstance> GetCompletedActivities(Guid workflowInstanceId, Guid activityId);

        /// <summary>
        /// 通过活动id获得单前流程正处理的活动实例的id.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        Guid GetCurrentActivityId(Guid activityInstanceId);

        ///// <summary>
        ///// 通过活动id获得单前流程正处理的活动实例的id.
        ///// </summary>
        ///// <param name="activityInstanceId"></param>
        ///// <returns></returns>
        //IList<Guid> GetCurrentActivityIds(Guid activityInstanceId);

        /// <summary>
        /// 根据活动实例获取在同一流程实例中的所有活动实例.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<ActivityInstance> GetActivitiesInSameWorkflow(Guid activityInstanceId);

        /// <summary>
        /// 获取指定流程实例的所有完成的流程活动（步骤）实例列表.
        /// (方法原名：GetActivitiesInSameWorkflowCompleted.)
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        IList<ActivityInstance> GetWorkflowCompletedActivities(Guid workflowInstanceId);

        /// <summary>
        /// 获取指定流程实例的当前流程活动实例.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        ActivityInstance GetCurrentActivity(Guid workflowInstanceId);

        /// <summary>
        /// 获取指定活动实例标识的本次全部活动实例列表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        IList<ActivityInstance> GetCurrentActivities(Guid workflowInstanceId);

        /// <summary>
        /// 获取指定流程实例 ID 和活动定义 ID 的上一活动实例列表.
        /// </summary>
        /// <param name="workflowInstanceId">流程实例 ID.</param>
        /// <param name="activityId">活动定义 ID.</param>
        /// <returns></returns>
        IList<ActivityInstance> GetPrevActivities(Guid workflowInstanceId, Guid activityId);

        /// <summary>
        /// 获取下一步活动.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<ActivityInstance> GetNextActivities(Guid activityInstanceId);

        /// <summary>
        /// 根据指定流程实例标识的所有活动实例列表.
        /// </summary>
        /// <param name="workflowInstanceId">流程实例标识.</param>
        /// <returns></returns>
        IList<ActivityInstance> GetWorkflowActivities(Guid workflowInstanceId);

        /// <summary>
        /// 获取指定外部实体类型和外部实体标识的活动实例列表.
        /// </summary>
        /// <param name="entityType">外部实体类型.</param>
        /// <param name="entityId">外部实体标识.</param>
        /// <returns></returns>
        IList<ActivityInstance> GetActivitiesByExternalEntity(string entityType, string entityId);

        /// <summary>
        /// 根据名称(以及流程及流程实例)获取最近完成的活动实例.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        ActivityInstance GetLatestCompletedActivityByActivityName(Guid workflowId, Guid workflowInstanceId, string activityName);

        /// <summary>
        /// 根据当前活动获取前继活动定义的实例.
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <returns></returns>
        IList<ActivityInstance> GetCompletedActivitiesOfPrevDefinitionByCurrent(ActivityInstance activityInstance);

        /// <summary>
        /// 分页获取待办任务列表.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="workflowId"></param>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetTaskListByUserName(string userName, string workflowId, string keywords, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// 获取指定授权人所授权其他用户处理的待办任务列表.
        /// </summary>
        /// <param name="proxyName">授权人用户名.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetTaskListByProxy(string proxyName, int pageIndex, int pageSize, ref int recordCount);
    }
}
