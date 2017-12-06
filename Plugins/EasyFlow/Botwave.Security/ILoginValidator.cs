using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Security
{
    /// <summary>
    /// 登录较验器,可以根据需要进行本地密码验证或者portal验证等
    /// </summary>
    public interface ILoginValidator
    {
        /// <summary>
        /// 登录较验,前提是此用户在系统中已存在,主要执行密码较验或者外部的较验(如portal)等
        /// </summary>
        /// <param name="username">输入用户名</param>
        /// <param name="password">输入用户密码</param>
        /// <param name="targetUser">在系统中的用户信息</param>
        /// <returns></returns>
        LoginStatus Validate(string username, string password, Botwave.Security.Domain.UserInfo targetUser);
    }
}
