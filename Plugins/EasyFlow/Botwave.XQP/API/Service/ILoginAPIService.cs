using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Security;

namespace Botwave.XQP.API.Service
{
    public interface ILoginAPIService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        LoginStatus Login(string userName, string password);
    }
}
