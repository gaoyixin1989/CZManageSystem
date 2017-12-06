using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Entities;
using Botwave.Workflow;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service
{
    /// <summary>
    /// 待阅服务接口.
    /// </summary>
    public interface IWorkflowReviewService
    {
        /// <summary>
        /// 发送待阅(抄送)，主要用于发单页.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="workflowId"></param>
        /// <param name="workflowTitle"></param>
        /// <param name="sender">发送人用户信息.</param>
        /// <returns></returns>
        bool PendingReview(WorkflowProfile profile, Guid activityInstanceId, Guid workflowId, string workflowTitle, BasicUser sender);

        /// <summary>
        /// 发送待阅(抄送).
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="context"></param>
        /// <param name="workflowId"></param>
        /// <param name="workflowTitle"></param>
        /// <param name="sender">发送人用户信息.</param>
        /// <returns></returns>
        bool PendingReview(WorkflowProfile profile, ActivityExecutionContext context, Guid workflowId, string workflowTitle, BasicUser sender);
    }
}
