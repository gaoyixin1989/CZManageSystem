using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 执行者信息.
    /// </summary>
    public class ActorInfo : Botwave.Entities.BasicUser
    {
        private string _proxyName;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public ActorInfo()
            : base()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        /// <param name="proxyName"></param>
        public ActorInfo(string userName, string realName, string proxyName)
            : base(userName, realName)
        {
            this._proxyName = proxyName;
        }

        /// <summary>
        /// 代理用户.
        /// </summary>
        public string ProxyName
        {
            get { return _proxyName; }
            set { _proxyName = value; }
        }
    }
}
