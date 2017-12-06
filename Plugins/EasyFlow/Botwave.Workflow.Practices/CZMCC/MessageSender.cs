using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Message;
using Botwave.GMCCServiceHelpers.CZ;

namespace Botwave.Workflow.Practices.CZMCC
{
    /// <summary>
    /// 消息发送实现类.
    /// </summary>
    public class MessageSender : IMessageSender
    {
        private readonly string EntityType = "bwwf_Tracking_Activities";

        #region IMessageSender 成员

        public void SendMessage(string type, string body, object ext, string from, params string[] to)
        {
            this.SendMessage(type, null, body, ext, from, to);
        }

        public void SendMessage(string type, string title, string body, object ext, string from, params string[] to)
        {
            if (to == null || to.Length == 0)
                return;

            if (string.IsNullOrEmpty(title))
                title = "潮州综合管理平台消息通知";
            if (type.IndexOf("1") > -1)
            {
                foreach (string receiver in to)
                    AsynNotifyHelper.SendEmail(receiver, from, title, body, EntityType, ext.ToString());
            }

            if (type.IndexOf("2") > -1)
            {
                foreach (string receiver in to)
                    AsynNotifyHelper.SendSMS(receiver, from, body, EntityType, ext.ToString());
            }
            else if (type.IndexOf("4") > -1)
            {
                foreach (string receiver in to)
                    AsynNotifyHelper.SendMessage(4, receiver, from, body, EntityType, ext.ToString());
            }
        }

        public void SendMessage(string type, string title, string body, string from, params string[] to)
        {
            if (to == null || to.Length == 0)
                return;

            if (type.IndexOf("1") > -1)
            {
                if (string.IsNullOrEmpty(title))
                {
                    foreach (string receiver in to)
                        AsynNotifyHelper.SendEmail(receiver, from, body);
                }
                else
                {
                    foreach (string receiver in to)
                        AsynNotifyHelper.SendEmail(receiver, from, title, body);
                }
            }

            if (type.IndexOf("2") > -1)
            {
                foreach (string receiver in to)
                    AsynNotifyHelper.SendSMS(receiver, from, body);
            }
            else if (type.IndexOf("4") > -1)
            {
                foreach (string receiver in to)
                    AsynNotifyHelper.SendMessage(4, receiver, from, body, string.Empty, string.Empty);
            }
        }

        public void SendMessage(string type, string body, string from, params string[] to)
        {
            this.SendMessage(type, null, body, from, to);
        }

        #endregion
    }
}
