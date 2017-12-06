using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.GMCCServiceHelpers;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Plugins
{
    public class WorkflowNotifyService : IWorkflowNotifyService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowNotifyService));

        /// <summary>
        /// 发送消息的委托处理类.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="messageContent"></param>
        /// <param name="activityInstanceId"></param>
        public delegate void SendMessageHandler(ActorDetail sender, WorkflowNotifyActor receiver, string messageContent, Guid activityInstanceId);

        #region 字段

        /// <summary>
        /// 邮件信息类型值.
        /// </summary>
        private const int EmailMessageType = 1;
        /// <summary>
        /// 短信信息类型值.
        /// </summary>
        private const int SMSMessageType = 2;

        #endregion

        #region 引用服务

        private IActivityService activityService;
        private IWorkflowUserService workflowUserService;
        private IWorkflowNotifyExtendService workflowNotifyExtendService;
        private Botwave.Message.IMessageSender messageSender;

        public IActivityService ActivityService
        {
            get { return activityService; }
            set { activityService = value; }
        }

        public IWorkflowUserService WorkflowUserService
        {
            get { return workflowUserService; }
            set { workflowUserService = value; }
        }

        public IWorkflowNotifyExtendService WorkflowNotifyExtendService
        {
            get { return workflowNotifyExtendService; }
            set { workflowNotifyExtendService = value; }
        }

        /// <summary>
        /// 消息发送对象.
        /// </summary>
        public Botwave.Message.IMessageSender MessageSender
        {
            set { messageSender = value; }
        }
        #endregion

        #region IWorkflowNotifyService 成员

        public IList<NotifyActor> GetNotifyActors(Guid activityInstanceId)
        {
            return IBatisMapper.Select<NotifyActor>("bwwf_WorkflowNotifyActors_Select_Current", activityInstanceId);
        }

        public IList<NotifyActor> GetNextNotifyActors(Guid activityInstanceId)
        {
            return IBatisMapper.Select<NotifyActor>("bwwf_WorkflowNotifyActors_Select_Next", activityInstanceId);
        }

        #region send

        public void SendMessage(ActorDetail sender, int operateType, Guid activityInstanceId, WorkflowSetting setting, WorkflowInstance workflowInstance, IList<NotifyActor> notifyActors)
        {
            string workflowTitle = workflowInstance.Title;
            WorkflowProfile profile = WorkflowProfile.LoadByWorkflowId(workflowInstance.WorkflowId);

            string creatorName = workflowInstance.Creator;
            ActorDetail creatorActor = workflowUserService.GetActorDetail(creatorName);
            if (creatorActor != null)
                creatorName = creatorActor.RealName;

            IList<WorkflowNotifyActor> workflowNotifyActors = workflowNotifyExtendService.GetNextNotifyActors(activityInstanceId);

            // 紧急并且重要时，直接插入到 Reminders 表，否则插入 xqp_remindes
            if (workflowInstance.Urgency > 0 || workflowInstance.Importance > 0)
            {
                this.SendMessage(sender, creatorName,
                    workflowTitle, workflowNotifyActors, profile, operateType,
                    new SendMessageHandler(DirectInsertSms), new SendMessageHandler(DirectInsertEmail));
            }
            else
            {
                this.SendMessage(sender, creatorName,
                    workflowTitle, workflowNotifyActors, profile, operateType,
                    new SendMessageHandler(InsertSMS), new SendMessageHandler(InsertEmail));
            }
        }

        public virtual void SendSMS(string messageFrom, string messageTo, string messageContent)
        {
            if (messageSender != null)
                messageSender.SendMessage("2", messageContent, messageFrom, new string[] { messageTo });
            else
                AsynNotifyHelper.SendSMS(messageTo, messageFrom, messageContent);
        }

        public virtual void SendEmail(string messageFrom, string messageTo, string messageContent)
        {
            if (messageSender != null)
                messageSender.SendMessage("1", messageContent, messageFrom, new string[] { messageTo });
            else
                AsynNotifyHelper.SendEmail(messageTo, messageFrom, messageContent);
        }
        #endregion

        #endregion

        #region 发送短信与电子邮件

        /// <summary>
        /// 发送信息执行方法.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="notifyActors"></param>
        /// <param name="setting"></param>
        /// <param name="operateType"></param>
        /// <param name="smsHandler"></param>
        /// <param name="emailHandler"></param>
        private void SendMessage(ActorDetail sender, string creator, string workflowTitle,
            IList<WorkflowNotifyActor> notifyActors,  WorkflowProfile profile, int operateType, 
            SendMessageHandler smsHandler, SendMessageHandler emailHandler)
        {
            if (notifyActors == null || notifyActors.Count == 0)
                return;

            // 消息内容字典.
            IDictionary<Guid, string> notifySmsContents = new Dictionary<Guid, string>();
            IDictionary<Guid, string> notifyEmailContents = new Dictionary<Guid, string>();

            foreach (WorkflowNotifyActor actor in notifyActors)
            {
                Guid activityInstanceId = actor.ActivityInstanceId;
                string emailContent = null;
                string smsContent = null;
                if (!notifyEmailContents.ContainsKey(activityInstanceId))
                {
                    ActivityInstance instance = activityService.GetActivity(activityInstanceId);
                    if (instance != null)
                    {
                        string activityName = instance.ActivityName;

                        emailContent =  WorkflowProfile.FormatNotifyMessage(profile.EmailNotifyFormat, creator, workflowTitle, activityName, operateType);
                        notifyEmailContents[activityInstanceId] = emailContent;

                        smsContent = WorkflowProfile.FormatNotifyMessage(profile.SmsNotifyFormat, creator, workflowTitle, activityName, operateType);
                        notifySmsContents[activityInstanceId] = smsContent;
                    }
                }
                else
                {
                    emailContent = notifyEmailContents[activityInstanceId];
                    smsContent = notifySmsContents[activityInstanceId];
                }
                int notifyType = actor.NotifyType;

                // 发送短信提醒.
                if (notifyType == 1 || notifyType == 3)
                {
                    if (string.IsNullOrEmpty(smsContent))
                        smsContent = "您有新的工单，请及时处理！";
                    smsHandler.Invoke(sender, actor, smsContent, activityInstanceId);
                }
                // 发送邮件提醒.
                if (notifyType == 1 || notifyType == 2)
                {
                    if (string.IsNullOrEmpty(emailContent))
                        emailContent = "您有新的工单，请及时处理！";
                    emailHandler.Invoke(sender, actor, emailContent, activityInstanceId);
                }
            }
        }

        #region remindes

        /// <summary>
        /// 直接发送电子邮件提醒.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        private void DirectInsertEmail(ActorDetail sender, WorkflowNotifyActor receiver, string messageBody, Guid activityInstanceId)
        {
            if (messageSender != null)
                messageSender.SendMessage("1", messageBody, sender.Email, new string[] { receiver.Email });
            else
                AsynNotifyHelper.SendEmail(sender.Email, receiver.Email, messageBody);
        }

        /// <summary>
        /// 直接发送短信提醒.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        private void DirectInsertSms(ActorDetail sender, WorkflowNotifyActor receiver, string messageBody, Guid activityInstanceId)
        {
            if (messageSender != null)
                messageSender.SendMessage("2", messageBody, sender.Mobile, new string[] { receiver.Mobile });
            else
                AsynNotifyHelper.SendSMS(sender.Mobile, receiver.Mobile, messageBody);
        }
        #endregion

        #region xqp_reminders

        /// <summary>
        /// 执行插入电子邮件提醒信息.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        private static void InsertEmail(ActorDetail sender, WorkflowNotifyActor receiver, string messageBody, Guid activityInstanceId)
        {
            IBatisMapper.Insert("xqp_WorkflowReminders_Insert", GetMessageParameters(sender.UserName, receiver.UserName, EmailMessageType, messageBody, activityInstanceId));
        }

        /// <summary>
        /// 执行插入短信提醒信息.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        private static void InsertSMS(ActorDetail sender, WorkflowNotifyActor receiver, string messageBody, Guid activityInstanceId)
        {
            IBatisMapper.Insert("xqp_WorkflowReminders_Insert", GetMessageParameters(sender.UserName, receiver.UserName, SMSMessageType, messageBody, activityInstanceId));
        }

        /// <summary>
        /// 获取提醒信息插入数据参数集合.
        /// </summary>
        /// <param name="messageFrom"></param>
        /// <param name="messageTo"></param>
        /// <param name="MessageType"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        private static Hashtable GetMessageParameters(string messageFrom, string messageTo, int MessageType, string messageBody, Guid activityInstanceId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("MessageFrom", messageFrom);
            parameters.Add("MessageTo", messageTo);
            parameters.Add("MessageType", MessageType);
            parameters.Add("MessageBody", messageBody);
            parameters.Add("ActivityInstanceId", activityInstanceId);
            return parameters;
        }
        #endregion

        #endregion
    }
}
