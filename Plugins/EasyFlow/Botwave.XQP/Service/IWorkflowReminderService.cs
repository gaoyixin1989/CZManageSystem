using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Domain;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service
{
    /// <summary>
    /// 流程提醒服务接口.
    /// </summary>
    public interface IWorkflowReminderService
    {
        /// <summary>
        /// 获取指定步骤的流程名称.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        string GetWorkflowName(Guid activityId);

        /// <summary>
        /// 获取指定流程步骤实例的下一步骤提醒用户列表.
        /// </summary>
        /// <param name="activityInstanceId">流程步骤实例编号.</param>
        /// <param name="workflowId">流程定义编号.</param>
        /// <returns></returns>
        IList<WorkflowNotifyActor> GetNextNotifyActors(Guid activityInstanceId);

        /// <summary>
        /// 获取指定流程步骤实例的提醒用户列表.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<WorkflowNotifyActor> GetNotifyActors(Guid activityInstanceId);

        /// <summary>
        /// 发送指定流程步骤的邮件提醒和短信提醒.
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="fromMobile"></param>
        /// <param name="notifyActors"></param>
        /// <param name="setting">流程提醒设置.</param>
        /// <param name="operateType"></param>
        void SendMessage(string fromEmail, string fromMobile, string creator, IList<WorkflowNotifyActor> notifyActors, WorkflowSetting setting, int operateType);

        /// <summary>
        /// 直接发送指定待办信息的邮件提醒和短信提醒(不经由自定义发送).
        /// </summary>
        /// <param name="fromEmail">发送人电子邮箱.</param>
        /// <param name="fromMobile">发送人移动电话.</param>
        /// <param name="notifyActors">流程提醒人列表.</param>
        /// <param name="setting">流程提醒设置.</param>
        /// <param name="operateType">操作类型.</param>
        void DirectSendMessage(string fromEmail, string fromMobile, string creator, IList<WorkflowNotifyActor> notifyActors, WorkflowSetting setting, int operateType);

        /// <summary>
        /// 发送指定流程步骤的邮件提醒和短信提醒.:附加审批短信
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="fromMobile"></param>
        /// <param name="notifyActors"></param>
        /// <param name="setting">流程提醒设置.</param>
        /// <param name="operateType"></param>
        void SendMessage2(string fromEmail, string fromMobile, string creator, IList<WorkflowNotifyActor> notifyActors, WorkflowSetting setting, int operateType);

        /// <summary>
        /// 直接发送指定待办信息的邮件提醒和短信提醒(不经由自定义发送).附加审批短信
        /// </summary>
        /// <param name="fromEmail">发送人电子邮箱.</param>
        /// <param name="fromMobile">发送人移动电话.</param>
        /// <param name="notifyActors">流程提醒人列表.</param>
        /// <param name="setting">流程提醒设置.</param>
        /// <param name="operateType">操作类型.</param>
        void DirectSendMessage2(string fromEmail, string fromMobile, string creator, IList<WorkflowNotifyActor> notifyActors, WorkflowSetting setting, int operateType);
    }
}
