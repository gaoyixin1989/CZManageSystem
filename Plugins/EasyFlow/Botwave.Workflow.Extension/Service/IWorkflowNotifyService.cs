using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Extension.Domain;

namespace Botwave.Workflow.Extension.Service
{
    /// <summary>
    /// 流程提醒服务接口.
    /// </summary>
    public interface IWorkflowNotifyService
    {
        /// <summary>
        /// 获取指定流程步骤实例的通知提醒人信息.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<NotifyActor> GetNotifyActors(Guid activityInstanceId);

        /// <summary>
        /// 获取指定流程步骤实例的下一步骤的通知提醒人信息.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<NotifyActor> GetNextNotifyActors(Guid activityInstanceId);

        #region send

        /// <summary>
        /// 发送通知消息（邮件、短信）.
        /// </summary>
        /// <param name="sender">发送人信息.</param>
        /// <param name="operateType">操作类型.</param>
        /// <param name="activityInstanceId">流程活动实例编号.</param>
        /// <param name="setting">流程设置对象.</param>
        /// <param name="workflowInstance">流程实例对象.</param>
        /// <param name="notifyActors">提醒接收人列表.</param>
        void SendMessage(ActorDetail sender, int operateType, Guid activityInstanceId,
            WorkflowSetting setting, WorkflowInstance workflowInstance, IList<NotifyActor> notifyActors);

        /// <summary>
        /// 发送短信通知.
        /// </summary>
        /// <param name="sender">发送人用户名.</param>
        /// <param name="receiver">接收人用户名.</param>
        /// <param name="content">短信内容.</param>
        void SendSMS(string sender, string receiver, string content);

        /// <summary>
        /// 发送邮件通知.
        /// </summary>
        /// <param name="sender">发送人用户名.</param>
        /// <param name="receiver">接收人用户名.</param>
        /// <param name="content">邮件内容.</param>
        void SendEmail(string sender, string receiver, string content);

        #endregion
    }
}
