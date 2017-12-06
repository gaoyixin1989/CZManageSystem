using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程评论服务接口.
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// 添加评论.
        /// </summary>
        /// <param name="comment"></param>
        void AddComment(Comment comment);

        /// <summary>
        /// 获取指定流程实例的评论数.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        int GetCommentCount(Guid workflowInstanceId);
        
        /// <summary>
        /// 获取指定流程实例ID的流程评论列表.
        /// </summary>
        /// <param name="worklfowInstanceId"></param>
        /// <returns></returns>
        IList<Comment> GetWorkflowComments(Guid worklfowInstanceId);

        /// <summary>
        /// 获取指定活动实例ID的步骤评论列表.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<Comment> GetActivityComments(Guid activityInstanceId);
    }
}
