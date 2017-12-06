using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Commons;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Configuration;

namespace Botwave.Workflow.Practices.CZMCC
{
    public class LoginValidator : ILoginValidator
    {
        #region ILoginValidator 成员

        public LoginStatus Validate(string username, string password, UserInfo targetUser)
        {
            LoginStatus status = LoginStatus.Unknown;
            if (targetUser.Type == 0 && SecurityConfig.Default.IsPortalLogin)
            {
                Botwave.GmccWsProxies.CZ.SSOService sso = Botwave.GmccWsProxies.CZ.ServiceFactory.GetSSO();
                string systemID = System.Configuration.ConfigurationManager.AppSettings["__SystemID__"];
                string sysAccount = System.Configuration.ConfigurationManager.AppSettings["__SysAccount__"];
                string sysPassword = System.Configuration.ConfigurationManager.AppSettings["__SysPassword__"];

                Botwave.GmccWsProxies.CZ.SSOLoginIn loginIn = new Botwave.GmccWsProxies.CZ.SSOLoginIn();
                loginIn.loginID = username;
                loginIn.password = password;
                Botwave.GmccWsProxies.CZ.SSOLoginOut loginOut = new Botwave.GmccWsProxies.CZ.SSOLoginOut();

                Botwave.GmccWsProxies.CZ.UNMPCallResult result = sso.SSOLogin(systemID, sysAccount, sysPassword, loginIn, out loginOut);

                if (((result.returnCode == 0) || (result.returnCode == 318)) || (result.returnCode == 317))
                    status = Botwave.Security.LoginStatus.InvalidValidateToken;
                else
                    status = LoginStatus.Success;
            }
            else
            {
                //加密方式存在一些问题,明码可能对应多个密码
                string decryptedPassword = TripleDESHelper.Decrypt(targetUser.Password);
                if (password != decryptedPassword)
                    status = Botwave.Security.LoginStatus.InvalidPassword;
                else
                    status = LoginStatus.Success;
            }
            return status;
        }

        #endregion
    }
}
