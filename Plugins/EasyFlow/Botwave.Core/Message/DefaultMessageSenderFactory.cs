using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Message
{
    public class DefaultMessageSenderFactory : IMessageSenderFactory
    {
        private IDictionary<string, IMessageSender> cache = new Dictionary<string, IMessageSender>();

        /// <summary>
        /// 适配spring
        /// srping中的dictionary的类型为HybridDictionary
        /// </summary>
        public System.Collections.Specialized.HybridDictionary Senders
        {
            set
            {
                if (null != value && value.Count > 0)
                {
                    foreach (string key in value.Keys)
                    {
                        string name = key.ToLower();

                        //直接转换，如果有异常则说明配置不正确
                        cache.Add(name, (IMessageSender)(value[name]));
                    }
                }
            }
        }

        #region IMessageSenderFactory 成员

        public ICollection<IMessageSender> GetMessageSenders(string type)
        {
            if (String.IsNullOrEmpty(type))
            {
                return null;
            }

            IList<IMessageSender> list = new List<IMessageSender>();
            foreach (char c in type)
            {
                string k = c.ToString();
                if (cache.ContainsKey(k))
                {
                    list.Add(cache[k]);
                }
            }
            return list;
        }

        #endregion
    }
}
