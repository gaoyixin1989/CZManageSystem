using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Message
{
    /// <summary>
    /// Email消息发送器.
    /// </summary>
    public class EmailMessageSender : IMessageSender
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(EmailMessageSender));

        private string smtpHost;
        private string userName;
        private string password;

        /// <summary>
        /// Smtp主机.
        /// </summary>
        public string SmtpHost
        {
            set { smtpHost = value; }
        }

        /// <summary>
        /// 邮箱用户名.
        /// </summary>
        public string UserName
        {
            set { userName = value; }
        }

        /// <summary>
        /// 邮箱用户密码.
        /// </summary>
        public string Password
        {
            set { password = value; }
        }

        #region IMessageSender 成员

        public void SendMessage(string type, string body, string from, params string[] to)
        {
            SendMessage(type, String.Empty, body, from, to);
        }

        public void SendMessage(string type, string title, string body, string from, params string[] to)
        {
            if (null == to || to.Length == 0)
            {
                log.Warn("messageTo is empty");
                return;
            }

            try
            {
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpHost);
                smtp.Credentials = new System.Net.NetworkCredential(userName, password);
                foreach (string target in to)
                {
                    smtp.Send(from, target, title, body);
                }      
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                throw ex;
            }                  
        }

        public void SendMessage(string type, string body, object ext, string from, params string[] to)
        {
            SendMessage(type, String.Empty, body, from, to);
        }

        public void SendMessage(string type, string title, string body, object ext, string from, params string[] to)
        {
            SendMessage(type, title, body, from, to);
        }

        #endregion
    }
}
