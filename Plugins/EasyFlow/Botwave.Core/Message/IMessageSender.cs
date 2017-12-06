using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Message
{
    /// <summary>
    /// 消息发送者.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// 发送消息.
        /// </summary>
        /// <param name="type">1代表邮件,2代表短信,3表示可回复的短信.可组合,如"12"表示同时发送短信邮件.</param>
        /// <param name="body">消息内容</param>
        /// <param name="from">发送人</param>
        /// <param name="to">接收人</param>
        void SendMessage(string type, string body, string from, params string[] to);

        /// <summary>
        /// 发送消息.
        /// </summary>
        /// <param name="type">1代表邮件,2代表短信,3表示可回复的短信.可组合,如"12"表示同时发送短信邮件.</param>
        /// <param name="title">标题</param>
        /// <param name="body">消息内容</param>
        /// <param name="from">发送人</param>
        /// <param name="to">接收人</param>
        void SendMessage(string type, string title, string body, string from, params string[] to);

        /// <summary>
        /// 发送消息.
        /// </summary>
        /// <param name="type">1代表邮件,2代表短信,3表示可回复的短信.可组合,如"12"表示同时发送短信邮件.</param>
        /// <param name="body">消息内容</param>
        /// <param name="ext">扩展参数</param>
        /// <param name="from">发送人</param>
        /// <param name="to">接收人</param>
        void SendMessage(string type, string body, object ext, string from, params string[] to);

        /// <summary>
        /// 发送消息.
        /// </summary>
        /// <param name="type">1代表邮件,2代表短信,3表示可回复的短信.可组合,如"12"表示同时发送短信邮件.</param>
        /// <param name="title">标题</param>
        /// <param name="body">消息内容</param>
        /// <param name="ext">扩展参数</param>
        /// <param name="from">发送人</param>
        /// <param name="to">接收人</param>
        void SendMessage(string type, string title, string body, object ext, string from, params string[] to);
    }
}
