using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.Service.WS
{
    public interface IAuthenticator
    {
        /// <summary>
        /// 访问权限鉴别
        /// </summary>
        /// <param name="sysAccount"></param>
        /// <param name="sysPassword"></param>
        /// <returns></returns>
        int Authenticate(string sysAccount, string sysPassword);
    }
}
