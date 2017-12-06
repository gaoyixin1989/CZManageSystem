using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Message
{
    /// <summary>
    /// 默认的消息发送者.
    /// </summary>
    public class DefaultMessageSender : IMessageSender
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DefaultMessageSender));

        private IMessageSenderFactory messageSenderFactory;

        /// <summary>
        /// 消息发送者工厂.
        /// </summary>
        public IMessageSenderFactory MessageSenderFactory
        {
            set { messageSenderFactory = value; }
        }

        #region IMessageSender 成员

        public void SendMessage(string type, string body, string from, params string[] to)
        {
            SendMessage(type, String.Empty, body, from, to);
        }

        public void SendMessage(string type, string title, string body, string from, params string[] to)
        {
            if (String.IsNullOrEmpty(type) || null == to || to.Length == 0)
            {
                log.Warn("type is empty or messageTo is empty");
                return;
            }

            ICollection<IMessageSender> messageSenders = messageSenderFactory.GetMessageSenders(type);
            if (null == messageSenders || messageSenders.Count == 0)
            {
                log.Warn("messageSenders is empty");
                return;
            }

            foreach (IMessageSender sender in messageSenders)
            {
                sender.SendMessage(type, title, body, from, to);
            }
        }

        public void SendMessage(string type, string body, object ext, string from, params string[] to)
        {
            SendMessage(type, String.Empty, body, ext, from, to);
        }

        public void SendMessage(string type, string title, string body, object ext, string from, params string[] to)
        {
            if (String.IsNullOrEmpty(type) || null == to || to.Length == 0)
            {
                return;
            }

            ICollection<IMessageSender> messageSenders = messageSenderFactory.GetMessageSenders(type); 
            if (null == messageSenders || messageSenders.Count == 0)
            {
                log.Warn("messageSenders is empty");
                return;
            }

            foreach (IMessageSender sender in messageSenders)
            {
                sender.SendMessage(type, title, body, ext, from, to);
            }
        }

        #endregion
    }
}
