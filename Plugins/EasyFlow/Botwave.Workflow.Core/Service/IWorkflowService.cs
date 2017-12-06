using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程实例服务接口.
    /// </summary>
    public interface IWorkflowService
    {   
        /// <summary>
        /// 获取流程实例的工作项数据.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        WorkflowInstance GetWorkflowInstance(Guid workflowInstanceId);

        /// <summary>
        /// 获取流程实例的工作项数据.
        /// </summary>
        /// <returns></returns>
        IList<WorkflowInstance> GetWorkflowInstance();

        /// <summary>
        /// 根据流程ID获取流程实例的工作项数据.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        IList<WorkflowInstance> GetWorkflowInstanceByWorkflowId(Guid workflowId);

        /// <summary>
        /// 根据活动实例获取流程实例的工作项数据.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        WorkflowInstance GetWorkflowInstanceByActivityInstanceId(Guid activityInstanceId);

        /// <summary>
        /// 创建流程实例.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        void InsertWorkflowInstance(WorkflowInstance instance);

        /// <summary>
        /// 更新流程实例.
        ///     只更新工作项相关数据.
        /// </summary>
        /// <param name="workflowInstance"></param>
        void UpdateWorkflowInstance(WorkflowInstance workflowInstance);

        /// <summary>
        /// 启动已经存在的流程.
        ///   返回下一活动的Id.
        /// </summary>
        /// <param name="workflowInstance"></param>
        void UpdateWorkflowInstanceForStart(WorkflowInstance workflowInstance);

        /// <summary>
        /// 获取指定流程标题前缀的流程列表.
        /// </summary>
        /// <param name="prefixTitle"></param>
        /// <returns></returns>
        IList<WorkflowInstance> GetWorkflowInstances(string prefixTitle);

        /// <summary>
        /// 获取指定用户的草稿流程列表.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        DataTable GetWorkflowInstanceByDraft(string userName);

        /// <summary>
        /// 获取指定用户的草稿流程列表.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="topCount">返回前几条记录.</param>
        /// <returns></returns>
        DataTable GetWorkflowInstanceByDraft(string userName, int topCount);

        /// <summary>
        /// 删除指定流程实例编号的流程实例（用于流程草稿箱功能）.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        int DeleteWorkflowInstance(Guid workflowInstanceId);
    }
}
