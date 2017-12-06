using System;
using System.Collections.Generic;
using System.Configuration;

namespace Botwave.Security.Configuration
{
    /// <summary>
    /// 安全配置节点.
    /// </summary>
    public class SecurityConfig : ConfigurationSection
    {
        private static SecurityConfig _default = null;

        /// <summary>
        /// 默认实例对象.
        /// </summary>
        public static SecurityConfig Default
        {
            get
            {
                if (_default == null)
                {
                    lock (typeof(SecurityConfig))
                    {
                        if(_default == null)
                            _default = ConfigurationManager.GetSection("botwave/security") as SecurityConfig;
                    }
                }
                return _default;
            }
        }

        /// <summary>
        /// Portal验证令牌.
        /// </summary>
        [ConfigurationProperty("portalValidateToken")]
        public string PortalValidateToken
        {
            get { return (string)this["portalValidateToken"]; }
            set { this["portalValidateToken"] = value; }
        }

        /// <summary>
        /// 是否Portal登录，仅针对内部用户.
        /// </summary>
        [ConfigurationProperty("isPortalLogin", DefaultValue = false)]
        public bool IsPortalLogin
        {
            get { return (bool)this["isPortalLogin"]; }
            set { this["isPortalLogin"] = value; }
        }

        /// <summary>
        /// 用户名存储的 Cookie 名称.
        /// </summary>
        [ConfigurationProperty("userCookieName", DefaultValue = "{83CC3CC5-10B3-419e-97DA-B0172B10AAC7}")]
        public string UserCookieName
        {
            get { return (string)this["userCookieName"]; }
            set { this["userCookieName"] = value; }
        }

        /// <summary>
        /// 用户名存储 Cookie 的过期分钟数（默认为 -1, 表示不设置过期时间）.
        /// </summary>
        [ConfigurationProperty("userCookieExpireMinutes", DefaultValue = "-1")]
        public int UserCookieExpireMinutes
        {
            get { return (int)this["userCookieExpireMinutes"]; }
            set { this["userCookieExpireMinutes"] = value; }
        }

        /// <summary>
        ///  是否使用 Cookie 存储用户名.false 则表示启用 session 存储.
        /// </summary>
        [ConfigurationProperty("IsCookie", DefaultValue = true)]
        public bool IsCookie
        {
            get { return (bool)this["IsCookie"]; }
            set { this["IsCookie"] = value; }
        }

        /// <summary>
        /// 是否大小写敏感,用于用户登录
        /// </summary>
        [ConfigurationProperty("isCaseSensitive", DefaultValue = true)]
        public bool IsCaseSensitive
        {
            get { return (bool)this["isCaseSensitive"]; }
            set { this["isCaseSensitive"] = value; }
        }
    }
}
