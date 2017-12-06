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
using Botwave.XQP.Service;

namespace Botwave.Workflow.Practices.CZMCC.Support
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
        /// <param name="type"></param>
        public delegate void SendMessageHandler(ActorDetail sender, WorkflowNotifyActor receiver, string messageContent, Guid activityInstanceId, int type);

        #region 字段

        /// <summary>
        /// 邮件信息类型值.
        /// </summary>
        private const int EmailMessageType = 1;
        /// <summary>
        /// 短信信息类型值.
        /// </summary>
        private const int SMSMessageType = 2;
        /// <summary>
        /// 可回复短信.
        /// </summary>
        private const int ReceivedSMSMessageType = 4;

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
            #region 发送时间段处理
            IList<Botwave.XQP.Domain.CZReminderTimeSpan> timeSpans = Botwave.XQP.Domain.CZReminderTimeSpan.Select(workflowInstance.WorkflowId);
            bool isWorkflowLimit = false;
            bool isActivityLimit = false;
            int bh, bm, eh, em;
            foreach (Botwave.XQP.Domain.CZReminderTimeSpan timeSpan in timeSpans)
            {
                if (timeSpan.RemindType == 2)
                    continue;
                bh = timeSpan.BeginHours;
                bm = timeSpan.BeginMinutes;
                eh = timeSpan.EndHours;
                em = timeSpan.EndMinutes;
                TimeSpan tsNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                TimeSpan tsBegin = new TimeSpan(bh, bm, 0);
                TimeSpan tsEnd = new TimeSpan(eh, em, 0);
                if (tsNow >= tsBegin && tsNow < tsEnd)
                {
                    isWorkflowLimit = false;
                    break;
                }
            }
            if (!isWorkflowLimit)
            {
                timeSpans = Botwave.XQP.Domain.CZReminderTimeSpan.SelectByActivityInstanceId(activityInstanceId);
                foreach (Botwave.XQP.Domain.CZReminderTimeSpan timeSpan in timeSpans)
                {
                    if (timeSpan.RemindType == 2)
                        continue;
                    bh = timeSpan.BeginHours;
                    bm = timeSpan.BeginMinutes;
                    eh = timeSpan.EndHours;
                    em = timeSpan.EndMinutes;
                    TimeSpan tsNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                    TimeSpan tsBegin = new TimeSpan(bh, bm, 0);
                    TimeSpan tsEnd = new TimeSpan(eh, em, 0);
                    if (tsNow >= tsBegin && tsNow < tsEnd)
                    {
                        isActivityLimit = true;
                        break;
                    }
                }
            }
            else
            {
                log.Info("[WorkflowInstanceId："+workflowInstance.WorkflowInstanceId+"]此时间段不发送提醒信息");
                return;
            }
            if (isActivityLimit)
            {
                log.Info("[ActivityInstanceId：" + activityInstanceId + "]此时间段不发送提醒信息");
                return;
            }
            #endregion
            WorkflowProfile profile = WorkflowProfile.LoadByWorkflowId(workflowInstance.WorkflowId);
            
            string creatorName = workflowInstance.Creator;
            ActorDetail creatorActor = workflowUserService.GetActorDetail(creatorName);
            if (creatorActor != null)
                creatorName = creatorActor.RealName;

            IList<WorkflowNotifyActor> workflowNotifyActors = workflowNotifyExtendService.GetNextNotifyActors(activityInstanceId);

            // 紧急并且重要时，直接插入到 Reminders 表，否则插入 xqp_remindes
            /*if (workflowInstance.Urgency > 0 || workflowInstance.Importance > 0)
            {
                this.SendMessage(sender, creatorName,
                    workflowTitle, workflowNotifyActors, profile, operateType, workflowInstance,
                    new SendMessageHandler(DirectInsertSms), new SendMessageHandler(DirectInsertEmail));
            }
            else
            {
                this.SendMessage(sender, creatorName,
                    workflowTitle, workflowNotifyActors, profile, operateType, workflowInstance,
                    new SendMessageHandler(InsertSMS), new SendMessageHandler(InsertEmail));
            }*/
            string creatorContent = "[潮州综合管理平台]您发起的工单：#title#，已到达步骤：#activityname#。";
            int creatorNotifyType = WorkflowNotify.GetNotifyType(workflowInstance.Creator,activityInstanceId);
            this.SendMessage(sender, creatorName,
                    workflowTitle, workflowNotifyActors, profile, operateType, workflowInstance,
                    new SendMessageHandler(DirectInsertSms), new SendMessageHandler(DirectInsertEmail));
        }

        public virtual void SendSMS(string messageFrom, string messageTo, string messageContent)
        {
            if (messageSender != null)
            {
                messageSender.SendMessage("2", messageContent, messageFrom, new string[] { messageTo });
            }
            else
            {
                //AsynNotifyHelper.SendSMS(messageTo, messageFrom, messageContent);
                Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendSMS(messageTo, messageFrom, messageContent);
            }
        }

        public virtual void SendEmail(string messageFrom, string messageTo, string messageContent)
        {
            if (messageSender != null)
            {
                messageSender.SendMessage("1", messageContent, messageFrom, new string[] { messageTo });
            }
            else
            {
                //AsynNotifyHelper.SendEmail(messageTo, messageFrom, messageContent);
                Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendEmail(messageTo,  messageFrom, messageContent);
            }
        }
        #endregion

        #endregion

        #region 发送短信与电子邮件

        /// <summary>
        /// 发送信息执行方法.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="creator"></param>
        /// <param name="workflowTitle"></param>
        /// <param name="notifyActors"></param>
        /// <param name="profile"></param>
        /// <param name="operateType"></param>
        /// <param name="workflowInstance"></param>
        /// <param name="smsHandler"></param>
        /// <param name="emailHandler"></param>
        private void SendMessage(ActorDetail sender, string creator, string workflowTitle,
            IList<WorkflowNotifyActor> notifyActors,  WorkflowProfile profile, int operateType, WorkflowInstance workflowInstance,
            SendMessageHandler smsHandler, SendMessageHandler emailHandler)
        {
            if (notifyActors == null || notifyActors.Count == 0)
                return;

            Guid workflowInstanceId = workflowInstance.WorkflowInstanceId;

            string workflowName = profile.WorkflowName;
            SMSProfile smsProfile = SMSProfile.GetProfile();
            bool isGratuityWorkflow =(!string.IsNullOrEmpty(smsProfile.GratuityReplyMessage) && workflowName.Equals(smsProfile.GratuityWorkflowName, StringComparison.OrdinalIgnoreCase));

            // 可短信审批的流程活动步骤名称字典.
            IDictionary<string, string> allowSMSAuditActiviies = profile.GetSMSAuditActivities();
            // 消息内容字典.
            IDictionary<Guid, NotifyMessage> messages = new Dictionary<Guid, NotifyMessage>();
            IList<FormField> formFields = FormField.GetFieldsByWorkflowInstanceId(workflowInstanceId);

            foreach (WorkflowNotifyActor actor in notifyActors)
            {
                Guid activityInstanceId = actor.ActivityInstanceId;
                string activityName = actor.ActivityName;
                ActivityInstance instance = activityService.GetActivity(activityInstanceId);

                // 酬金申告流程.
                if (isGratuityWorkflow && activityName.Equals(smsProfile.GratuityNotifyActivity, StringComparison.OrdinalIgnoreCase))
                {
                    this.SendGratuityMessage(sender, activityInstanceId, smsProfile.GratuityReplyMessage, workflowInstance);
                    continue;
                }

                string emailContent = null;
                string smsContent = null;
                string smsAuditContent = null;
                bool isSMSAudit = false;
                string stepWarningNotifyformat = null;
                string stepTimeoutNotifyformat = null;
                string workOrderWarningNotifyformat = null;
                string workOrderTimeoutNotifyformat = null;
                IDictionary<int, string> notifies = new Dictionary<int, string>();
                if (!messages.ContainsKey(activityInstanceId))
                {
                    //ActivityInstance instance = activityService.GetActivity(activityInstanceId);
                    if (instance != null)
                    {
                        // 邮件通知内容.
                        emailContent =  WorkflowProfile.FormatNotifyMessage(profile.EmailNotifyFormat, activityInstanceId, creator, workflowTitle, activityName, operateType);

                        // 短信通知内容.
                        smsContent = WorkflowProfile.FormatNotifyMessage(profile.SmsNotifyFormat, activityInstanceId, creator, workflowTitle, activityName, operateType);

                        stepWarningNotifyformat = WorkflowProfile.FormatNotifyMessage(profile.StepWarningNotifyformat, activityInstanceId, creator, workflowTitle, activityName, operateType);
                        stepTimeoutNotifyformat = WorkflowProfile.FormatNotifyMessage(profile.StepTimeoutNotifyformat, activityInstanceId, creator, workflowTitle, activityName, operateType);
                        workOrderWarningNotifyformat = WorkflowProfile.FormatNotifyMessage(profile.WorkOrderWarningNotifyformat, activityInstanceId, creator, workflowTitle, activityName, operateType);
                        workOrderTimeoutNotifyformat = WorkflowProfile.FormatNotifyMessage(profile.WorkOrderTimeoutNotifyformat, activityInstanceId, creator, workflowTitle, activityName, operateType);
                        notifies.Add(0, workOrderWarningNotifyformat);//工单预警
                        notifies.Add(1, workOrderTimeoutNotifyformat);//工单超时
                        notifies.Add(2, stepWarningNotifyformat);//环节预警
                        notifies.Add(3, stepTimeoutNotifyformat);//环节超时

                        // 短信审批的内容.
                        if (profile.IsSMSAudit && allowSMSAuditActiviies.ContainsKey(activityName))
                        {
                            isSMSAudit = true;
                            smsAuditContent = WorkflowProfile.FormatNotifyMessage(profile.SMSAuditNotifyFormat, activityInstanceId, creator, workflowTitle, activityName, operateType);
                            if (!string.IsNullOrEmpty(smsAuditContent))
                                smsAuditContent = FormField.FormatMessage(smsAuditContent, formFields);
                            DataTable dt= GetNextActivitys(instance.ActivityId);
                            smsAuditContent = GetSendContent(smsAuditContent, dt);
                            
                        }
                        if(string.IsNullOrEmpty(smsAuditContent))
                            smsAuditContent = smsContent;
                        messages[activityInstanceId] = new NotifyMessage(emailContent, smsContent, smsAuditContent, isSMSAudit);
                    }
                }
                else
                {
                    NotifyMessage message = messages[activityInstanceId];
                    if (message != null)
                    {
                        emailContent = message.EmailMessage;
                        smsContent = message.SMSMessage;
                        smsAuditContent = message.SMSAuditMessags;
                        isSMSAudit = message.IsSMSAudit;
                    }
                }

                int notifyType = actor.NotifyType;
                // 发送短信提醒.
                if (notifyType == 1 || notifyType == 3)
                {
                    // 当属于销售精英平台的渠道用户时，不发送短信审批.
                    if (actor.UserName.StartsWith("XSJY_", StringComparison.OrdinalIgnoreCase))
                        isSMSAudit = false;

                    string content = (isSMSAudit ? smsAuditContent : smsContent);
                    if (string.IsNullOrEmpty(content))
                        content = "您有新的工单，请及时处理！";
                    smsHandler.Invoke(sender, actor, content, activityInstanceId, isSMSAudit ? ReceivedSMSMessageType : SMSMessageType);
                }
                // 发送邮件提醒.
                if (notifyType == 1 || notifyType == 2)
                {
                    if (string.IsNullOrEmpty(emailContent))
                        emailContent = "您有新的工单，请及时处理！";
                    emailHandler.Invoke(sender, actor, emailContent, activityInstanceId, 1);
                }
                //插入时效考核信息
                SZIntelligentRemind.SendTimerInfo(sender, actor, notifies, instance);
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
        /// <param name="type"></param>
        private void DirectInsertEmail(ActorDetail sender, WorkflowNotifyActor receiver, string messageBody, Guid activityInstanceId, int type)
        {
            if (messageSender != null)
            {
                messageSender.SendMessage("1", string.Empty, messageBody, activityInstanceId, sender.UserName, new string[] { receiver.UserName });
            }
            else
            {
                //AsynNotifyHelper.SendEmail(sender.Email, receiver.Email, messageBody);
                Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendEmail(receiver.UserName, sender.UserName, "流程易消息通知", messageBody, ActivityInstance.EntityType, activityInstanceId.ToString());
            }
        }

        /// <summary>
        /// 直接发送短信提醒.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="type"></param>
        private void DirectInsertSms(ActorDetail sender, WorkflowNotifyActor receiver, string messageBody, Guid activityInstanceId, int type)
        {
            if (messageSender != null)
            {
                messageSender.SendMessage(type.ToString(), messageBody, activityInstanceId, sender.UserName, new string[] { receiver.UserName });
            }
            else
            {
                //AsynNotifyHelper.SendSMS(sender.Mobile, receiver.Mobile, messageBody);
                Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendSMS(receiver.UserName, sender.UserName, messageBody, ActivityInstance.EntityType, activityInstanceId.ToString());
            }
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
        private static void InsertEmail(ActorDetail sender, WorkflowNotifyActor receiver, string messageBody, Guid activityInstanceId, int type)
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
        private static void InsertSMS(ActorDetail sender, WorkflowNotifyActor receiver, string messageBody, Guid activityInstanceId, int type)
        {
            int msgType = (type == SMSMessageType ? SMSMessageType : ReceivedSMSMessageType);
            IBatisMapper.Insert("xqp_WorkflowReminders_Insert", GetMessageParameters(sender.UserName, receiver.UserName, msgType, messageBody, activityInstanceId));
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

        /// <summary>
        /// 发送酬金申告流程.
        /// </summary>
        protected void SendGratuityMessage(ActorDetail sender, Guid activityInstanceId, string message, WorkflowInstance workflowInstance)
        {
            message = FormatGratuityMessage(message, workflowInstance);
            Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendSMS(workflowInstance.Creator, sender.UserName, message, ActivityInstance.EntityType, activityInstanceId.ToString());
        }

        /// <summary>
        /// 格式化酬金申告流程的消息通知.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="workflowInstance"></param>
        /// <returns></returns>
        protected string FormatGratuityMessage(string message, WorkflowInstance workflowInstance)
        {
            // 酬金申告流程，发送反馈短信.
            string title = workflowInstance.Title;
            string creator = workflowInstance.Creator;
            string creatorName = string.Empty;
            message = message.Trim().ToLower();

            if (message.IndexOf("#creator#", StringComparison.OrdinalIgnoreCase) > -1)
            {
                ActorDetail detail = workflowUserService.GetActorDetail(creator);
                if (detail != null)
                    creatorName = detail.RealName;
                message = message.Replace("#creator#", creatorName);
            }
            message = message.Replace("#title#", title);
            return message;
        }

        DataTable GetNextActivitys(Guid activityId)
        {
            string strSql = string.Format(@"select a.* from dbo.bwwf_Activities a inner join 
(
	select ACS.* from dbo.bwwf_ActivitySet ACS
	inner join dbo.bwwf_Activities AC
	on AC.NextActivitySetId=ACS.setid and AC.ActivityId='{0}'
)
b
on a.ActivityId=b.ActivityId order by sortorder", activityId.ToString());
            DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];
            return dt;
        }

        string GetSendContent(string Content,DataTable dt)
        {
            string ret_val = string.Empty;
            if (dt.Rows.Count > 1)
            {
                string command = "0:不同意,";int i=1;
                foreach (DataRow dr in dt.Rows)
                {
                    command += i.ToString() + ":" + dr["ActivityName"].ToString() + ",";
                    i++;
                }
                ret_val = Content + ";" + command.Remove(command.Length - 1, 1);
            }
            else
            {
                ret_val = Content + ";" + "0:不同意,1:同意";
            }
            return ret_val + ";";
        }

        #endregion

        /// <summary>
        /// 提醒消息类.
        /// </summary>
        [Serializable]
        internal class NotifyMessage
        {
            private string _emailMessage;
            private string _smsMessage;
            private string _smsAuditMessags;
            private bool _isSMSAudit = false;

            public string EmailMessage
            {
                get { return _emailMessage; }
                set { _emailMessage = value; }
            }

            public string SMSMessage
            {
                get { return _smsMessage; }
                set { _smsMessage = value; }
            }

            public string SMSAuditMessags
            {
                get { return _smsAuditMessags; }
                set { _smsAuditMessags = value; }
            }

            public bool IsSMSAudit
            {
                get { return _isSMSAudit; }
                set { _isSMSAudit = value; }
            }

            public NotifyMessage()
            { }

            public NotifyMessage(string emailMessage, string smsMessage,string smsAuditMessage, bool isSMSAudit)
            {
                this._emailMessage = emailMessage;
                this._smsMessage = smsMessage;
                this._smsAuditMessags = smsAuditMessage;
                this._isSMSAudit = isSMSAudit;
            }
        }
    }
}
