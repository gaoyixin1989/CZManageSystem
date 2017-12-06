using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Message;
using Botwave.GMCCServiceHelpers;

namespace Botwave.Workflow.Practices.GZMCC
{
    /// <summary>
    /// 消息发送实现类.
    /// </summary>
    public class MessageSender : IMessageSender
    {
        #region IMessageSender 成员

        public void SendMessage(string type, string body, object ext, string from, params string[] to)
        {
            this.SendMessage(type, null, body, ext, from, to);
        }

        public void SendMessage(string type, string title, string body, object ext, string from, params string[] to)
        {
            if (to == null || to.Length == 0)
                return;

            if (type.IndexOf("1") > -1)
            {
                foreach (string receiver in to)
                    AsynNotifyHelper.SendEmail(receiver, from, body);
            }

            if (type.IndexOf("2") > -1)
            {
                foreach (string receiver in to)
                    AsynNotifyHelper.SendSMS(receiver, from, body);
            }
        }

        public void SendMessage(string type, string body, string from, params string[] to)
        {
            this.SendMessage(type, null, body, from, to);
        }

        public void SendMessage(string type, string title, string body, string from, params string[] to)
        {
            if (to == null || to.Length == 0)
                return;

            if (type.IndexOf("1") > -1)
            {
                foreach (string receiver in to)
                    AsynNotifyHelper.SendEmail(receiver, from, body);
            }

            if (type.IndexOf("2") > -1)
            {
                foreach (string receiver in to)
                    AsynNotifyHelper.SendSMS(receiver, from, body);
            }
        }

        #endregion
    }
}
