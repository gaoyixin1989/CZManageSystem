using System;
using System.Collections.Generic;
using System.Text;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.WS
{
    public class Authenticator : IAuthenticator
    {
        #region IAuthenticator Members
        /// <summary>
        /// 认证接入应用系统的信息。
        /// </summary>
        /// <param name="sysId">应用系统的标识。</param>
        /// <param name="sysPassword">应用系统的登录密码。</param>
        /// <returns></returns>
        public int Authenticate(string sysAccount, string sysPassword)
        {
            if (string.IsNullOrEmpty(sysAccount) || string.IsNullOrEmpty(sysPassword))
                return AppAuthConstants.AccountError;

            Apps currentApp = Apps.LoadByName(sysAccount);

            //应用系统不存在
            if (null == currentApp)
                return AppAuthConstants.AccountError;

            //被禁用
            if (!currentApp.Enabled)
                return AppAuthConstants.Other;

            //密码不匹配
            if (currentApp.Password != sysPassword)
                return AppAuthConstants.UnMatch;

            return AppAuthConstants.Success;
        }

        #endregion
    }
}
