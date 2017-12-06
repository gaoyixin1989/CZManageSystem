using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Botwave.Security.Extension.Web
{
    /// <summary>
    /// 用户安全辅助类.
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// 登录名保持的 Cookie 键名.
        /// </summary>
        private const string Key_LoginNameCookie = "{5E73B429-3E4A-43EB-9A34-05EFAE8C5DC1}";

        /// <summary>
        /// 获取登录用户名.
        /// </summary>
        /// <returns></returns>
        public static string GetLoginName()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[Key_LoginNameCookie];
            if (null == cookie || string.IsNullOrEmpty(cookie.Value))
                return null;
            return cookie.Value;
        }

        /// <summary>
        /// 设置登录用户名.
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public static void SetLoginName(string loginName)
        {
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = new HttpCookie(Key_LoginNameCookie);
            cookie.Value = loginName;
            cookie.Expires = DateTime.Now.AddMonths(1);
            if (null != context.Request.Cookies[Key_LoginNameCookie])
            {
                context.Request.Cookies.Remove(Key_LoginNameCookie);
            }
            context.Response.Cookies.Add(cookie);   
        }
    }
}
