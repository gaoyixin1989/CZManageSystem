using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.Service.WS
{ 
    /// <summary>
    /// 应用系统接入认证结果
    /// </summary>
    internal sealed class AppAuthConstants
    {
        /// <summary>
        /// 接入认证成功
        /// </summary>
        public const int Success = 0;

        /// <summary>
        /// 指定接入系统或帐号不正确
        /// </summary>
        public const int AccountError = -98;

        /// <summary>
        /// 接入信息间不匹配
        /// </summary>
        public const int UnMatch = -99;

        /// <summary>
        /// 其他错误
        /// </summary>
        public const int Other = -100;

        /// <summary>
        /// 是否验证成功
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public static bool IsValid(int auth)
        {
            return auth == Success;
        }
    }
}
