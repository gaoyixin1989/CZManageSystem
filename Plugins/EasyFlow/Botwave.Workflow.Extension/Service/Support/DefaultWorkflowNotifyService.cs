using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Extension.Domain;

namespace Botwave.Workflow.Extension.Service.Support
{
    /// <summary>
    /// 流程提醒服务的默认实现类.
    /// </summary>
    public class DefaultWorkflowNotifyService : IWorkflowNotifyService
    {
        #region IWorkflowNotifyService 成员

        /// <summary>
        /// 获取指定流程活动实例的提醒用户列表.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public IList<NotifyActor> GetNotifyActors(Guid activityInstanceId)
        {
            return IBatisMapper.Select<NotifyActor>("bwwf_WorkflowNotifyActors_Select_Current", activityInstanceId);
        }

        /// <summary>
        /// 获取指定流程活动实例的下一活动的提醒用户列表.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public IList<NotifyActor> GetNextNotifyActors(Guid activityInstanceId)
        {
            return IBatisMapper.Select<NotifyActor>("bwwf_WorkflowNotifyActors_Select_Next", activityInstanceId);
        }

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
        public void SendMessage(ActorDetail sender, int operateType, Guid activityInstanceId, WorkflowSetting setting, WorkflowInstance workflowInstance, IList<NotifyActor> notifyActors)
        {   }

        /// <summary>
        /// 发送短信通知.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="content"></param>
        public virtual void SendSMS(string sender, string receiver, string content)
        {    }

        /// <summary>
        ///  发送电子邮件通知.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="content"></param>
        public virtual void SendEmail(string sender, string receiver, string content)
        {    }

        #endregion

        #endregion
    }
}
