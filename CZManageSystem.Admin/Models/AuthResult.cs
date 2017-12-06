using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CZManageSystem.Admin.Models
{
    /// <summary>
    /// portal单点登录返回结果
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// 认证结果
        /// </summary>
        public bool authResult { get; set; }
        /// <summary>
        /// 如果令牌验证成功，统一信息平台提供该令牌拥有者的帐号名称
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// 认证操作消息（一般为错误消息）
        /// </summary>
        public string authMsg { get; set; }
        public string idsTokenName { get; set; }
        public string idsTokenValue { get; set; }
    }
}