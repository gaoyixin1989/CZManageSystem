using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Security
{
    /// <summary>
    /// 用户身份认证服务的空实现类.
    /// </summary>
    public class EmptyAuthService : IAuthService
    {
        #region IAuthService Members

        /// <summary>
        /// 登录.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LoginStatus Login(string username, string password)
        {
            return LoginStatus.Success;
        }

        /// <summary>
        /// 可信时的登录.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public LoginStatus TrustedLogin(string userName)
        {
            return LoginStatus.Success;
        }

        /// <summary>
        /// 注销.
        /// </summary>
        /// <param name="username"></param>
        public void Logout(string username)
        {
            //;
        }

        #endregion
    }
}
