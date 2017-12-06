using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Message
{
    /// <summary>
    /// 消息发送者工厂.
    /// </summary>
    public interface IMessageSenderFactory
    {
        /// <summary>
        /// 获取消息发送者.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ICollection<IMessageSender> GetMessageSenders(string type);
    }
}
