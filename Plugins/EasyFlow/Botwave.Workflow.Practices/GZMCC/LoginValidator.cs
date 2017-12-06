using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Commons;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Configuration;

namespace Botwave.Workflow.Practices.GZMCC
{
    public class LoginValidator : ILoginValidator
    {
        #region ILoginValidator 成员

        public LoginStatus Validate(string username, string password, UserInfo targetUser)
        {
            LoginStatus status = Botwave.Security.LoginStatus.Success;
            if (targetUser.Type == 0 && SecurityConfig.Default.IsPortalLogin)
            {
                Botwave.GmccWsProxies.SSO sso = Botwave.GmccWsProxies.GZ.ServiceFactory.GetSSO();
                string systemID = System.Configuration.ConfigurationManager.AppSettings["__SystemID__"];
                string sysAccount = System.Configuration.ConfigurationManager.AppSettings["__SysAccount__"];
                string sysPassword = System.Configuration.ConfigurationManager.AppSettings["__SysPassword__"];

                Botwave.GmccWsProxies.LoginResult result = sso.LoginCheck2(systemID, sysAccount, sysPassword, username, password);
                if (result.returnValue != 0)
                    status = Botwave.Security.LoginStatus.InvalidValidateToken;
            }
            else
            {
                //加密方式存在一些问题,明码可能对应多个密码
                string decryptedPassword = TripleDESHelper.Decrypt(targetUser.Password);
                if (password != decryptedPassword)
                    status = Botwave.Security.LoginStatus.InvalidPassword;
            }
            return status;
        }

        #endregion
    }
}
