using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Service
{
    public interface IManageAPIService
    {
        /// <summary>
        /// 评论工单
        /// </summary>
        /// <param name="workflowInstanceId">工单唯一标识</param>
        /// <param name="activityInstanceId">活动ID</param>
        /// <param name="username">用户名</param>
        /// <param name="content">评论内容</param>
        /// <param name="CommentProperties">附件信息XML格式</param>
        /// <returns></returns>
        void CommentWorkflow(string workflowInstanceId, string activityInstanceId, string username, string content, string CommentProperties);

        /// <summary>
        /// 转交工单
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="assignedUser">被指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(活动实例ID或工单号sheetId)</param>
        /// <param name="Message">转交意见</param>
        /// <returns></returns>
        void AssignWorkflow(string username, string assignedUser, string activityInstanceId, string Message);

        /// <summary>
        /// 发起工单
        /// </summary>
        /// <param name="userName">指定用户的Portal 登录账号</param>
        /// <param name="workflowId">流程定义ID（标识）</param>
        /// <param name="workflowTitle">需求单工单标题</param>
        /// <param name="startTime">工单发起时间</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <returns></returns>
        string StartWorkflow(string userName, string workflowId, string workflowTitle, string workflowProperties);


        /// <summary>
        /// 传入表单数据以处理需求单
        /// </summary>
        /// <param name="userName">指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(流程实例ID)</param>
        /// <param name="command">工单处理命令。approve：通过审核。reject：退回工单。cancel：取消工单。save：保存工单。</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <param name="manageOpinion">处理意见</param>
        /// <returns></returns>
        string ExecuteWorkflow(string username, string activityInstanceId, string command, string workflowProperties, string manageOpinion);
    }
}
