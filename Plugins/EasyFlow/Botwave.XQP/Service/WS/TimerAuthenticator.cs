using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.Service.WS
{
    /// <summary>
    /// 定时服务的身份认证..
    /// </summary>
    public class TimerAuthenticator : IAuthenticator
    {
        #region IAuthenticator Members
        /// <summary>
        /// 认证接入应用系统的信息.
        /// </summary>
        /// <param name="sysId">应用系统的标识。</param>
        /// <param name="sysPassword">应用系统的登录密码。</param>
        /// <returns></returns>
        public int Authenticate(string sysAccount, string sysPassword)
        {
            if (string.IsNullOrEmpty(sysAccount) || string.IsNullOrEmpty(sysPassword))
                return AppAuthConstants.AccountError;

            return AppAuthConstants.Success;
        }

        #endregion
    }
}
