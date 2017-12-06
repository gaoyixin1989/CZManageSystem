using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Security
{
    /// <summary>
    /// 用户身份验证服务接口.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 用户登录.
        /// </summary>
        /// <param name="userName">登录用户名.</param>
        /// <param name="password">登录密码.</param>
        /// <returns></returns>
        LoginStatus Login(string userName, string password);

        /// <summary>
        /// 信任的用户登录.
        /// </summary>
        /// <param name="userName">登录用户名.</param>
        /// <returns></returns>
        LoginStatus TrustedLogin(string userName);

        /// <summary>
        /// 用户登出(注销).
        /// </summary>
        /// <param name="userName">登出用户名.</param>
        void Logout(string userName);
    }
}
