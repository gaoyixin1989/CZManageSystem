using System;
using System.Collections.Generic;

using System.Text;
using Botwave.Security;
using Botwave.Commons;
using Botwave.Security.Configuration;
using Botwave.Security.Service;
using Botwave.Security.Domain;
using Botwave.Easyflow.API;

namespace Botwave.XQP.API.Service
{
    /// <summary>
    /// 登录接口
    /// </summary>
    public class LoginAPIService : ILoginAPIService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LoginStatus Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new WorkflowAPIException(1);

            string _userName = userName.Trim();

            IUserService userService = Spring.Context.Support.WebApplicationContext.Current["userService"] as IUserService;
          
            if (userService == null)
            {
                return LoginStatus.Unknown;
            }

            LoginStatus status = LoginStatus.Unknown;
            UserInfo user = null;
            user = userService.GetUserByUserName(userName);
            if (user == null)
            {
                status = LoginStatus.AccountNotFound;
            }
            else
            {
                if (user.Status == -1) // 用户被禁用
                {
                    status = LoginStatus.AccountDisabled;
                }
                else
                {
                    status = LoginStatus.Success;
                    //加密方式存在一些问题,明码可能对应多个密码
                    string decryptedPassword = TripleDESHelper.Decrypt(user.Password);
                    if (password != decryptedPassword)
                    {
                        status = LoginStatus.InvalidPassword;
                    }
                }
            }

            if (status == LoginStatus.Success)
            {
                //AuthenticateHelper.SetUserCache(_user);
            }
            return status;
        }
    }
}
