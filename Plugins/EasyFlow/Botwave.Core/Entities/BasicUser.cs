using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Entities
{
    /// <summary>
    /// 用户基础类.
    /// </summary>
    public class BasicUser
    {
        private string userName;
        private string realName;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public BasicUser() { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realname"></param>
        public BasicUser(string userName, string realname)
        {
            this.userName = userName;
            this.realName = realname;
        }

        /// <summary>
        /// 用户名.
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 真实名称.
        /// </summary>
        public string RealName
        {
            get { return realName; }
            set { realName = value; }
        }
    }
}
