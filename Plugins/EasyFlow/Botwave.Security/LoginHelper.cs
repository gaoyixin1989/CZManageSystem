using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Botwave.Commons;
using Botwave.Security.Configuration;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Botwave.Web;

namespace Botwave.Security
{
    /// <summary>
    /// 登录辅助类.
    /// </summary>
    public class LoginHelper
    {
        /// <summary>
        /// 缓存器.
        /// </summary>
        private static CacheManager cache = CacheFactory.GetCacheManager(CacheManagerLoginUser);
        private static string basePage = "~/plugins/easyflow/";
        #region Service

        private static IUserService userService;
        private static IResourceService resourceService;

        /// <summary>
        /// 用户服务接口.
        /// </summary>
        public static IUserService UserService
        {
            get { return userService; }
            set { userService = value; }
        }

        /// <summary>
        /// 权限资源服务接口.
        /// </summary>
        public static IResourceService ResourceService
        {
            get { return resourceService; }
            set { resourceService = value; }
        }

        #endregion

        #region 字段 常量
        /// <summary>
        /// 主程序CookieName
        /// </summary>
        private const string UserCookieName = "CZMS.SysUser";
        /// <summary>
        /// 长时间缓存管理者名称.
        /// </summary>
        public const string CacheManagerLongTime = "Long Time Cache Manager";

        /// <summary>
        /// 用户登陆信息缓存管理者名称.
        /// </summary>
        public const string CacheManagerLoginUser = "Login User Cache Manager";

        /// <summary>
        /// 是否启用 cookie 存储用户名.
        /// </summary>
        public static bool IsCookie = SecurityConfig.Default.IsCookie;

        /// <summary>
        /// 当前用户 Cookie 存储键名.
        /// </summary>
        public static string UserCookieKey = SecurityConfig.Default.UserCookieName;

        /// <summary>
        /// 当前用户 Cookie 过期分钟数.
        /// </summary>
        public static int UserCookieExpireMinutes = SecurityConfig.Default.UserCookieExpireMinutes;

        /// <summary>
        /// 匿名用户名.
        /// </summary>
        public const string AnonymousName = "anonymous";

        #endregion

        #region 属性


        /// <summary>
        /// 判断用户是否登陆
        /// </summary>
        public static bool IsLoginUser
        {
            get { return (null != HttpContext.Current.Request.Cookies[UserCookieKey]); }
        }
        public static bool IsLogin()
        {
            var isture = (!IsLoginUser || UserName == "anonymous");
            return isture;
        }
        /// <summary>
        /// 获取当前用户的用户名称.
        /// 如果未登陆则返回 anonymous.
        /// </summary>
        public static string UserName
        {
            get
            {
                string userName = GetUserNameFromCookie();
                if (string.IsNullOrEmpty(userName))
                    userName = AnonymousName;
                return userName;
            }
            set
            {
                SetUserNameToCookie(value);
            }
        }

        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        public static LoginUser User
        {
            get
            {
                var user = GetUserCache();
                if (user == null)
                {
                    var loginUrl = "/Home/Login";// "contrib/security/pages/login.aspx";
                    //HttpContext.Current.Response.Redirect(MessageHelper.MessagePage_Error + "?returnUrl=" + basePage + loginUrl);
                    HttpContext.Current.Response.Redirect(MessageHelper.MessagePage_Error + "?returnUrl=" + loginUrl);
                }
                return user;
            }
            set { InsertUserCache(value); }
        }

        #endregion

        /// <summary>
        /// 获取用户的登录缓存.
        /// </summary>
        /// <returns></returns>
        private static LoginUser GetUserCache()
        {
            string userName = GetUserNameFromCookie();
            if (String.IsNullOrEmpty(userName))
                return null;
            LoginUser loginUser = GetCache(userName);
            if (loginUser != null)
                return loginUser;
            if (userService != null)
            {
                Guid userId = new Guid();
                //由原来的通过用户名UserName改成用UserId //wbl
                //UserInfo user = Guid.TryParse(userName, out userId) ? userService.GetUserByUserId(userId) : null;
                UserInfo user = userService.GetUserByUserName(userName);//wbl
                if (user == null)
                    return null;
                loginUser = new LoginUser(user);
                if (resourceService != null)
                {
                    loginUser.Resources = resourceService.GetResourcesByUserId(user.UserId);
                }
                InsertCache(userName, loginUser);
                return loginUser;
            }
            return null;
        }

        /// <summary>
        /// 插入用户的登录缓存.
        /// </summary>
        /// <param name="user"></param>
        private static void InsertUserCache(LoginUser user)
        {
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies[UserCookieKey];
            string cacheKey = null;

            if (user == null)
            {
                // 当前设置用户为 null 且数据缓存又存在，则删除缓存
                if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                {
                    cacheKey = TripleDESHelper.Decrypt(cookie.Value);
                    RemoveCache(cacheKey);
                    context.Response.Cookies[UserCookieKey].Expires = DateTime.Now.AddDays(-1);
                }
            }
            else
            {
                cacheKey = user.UserName;
                if (cookie == null)
                {
                    // Cookie 不存在时.
                    cookie = new HttpCookie(UserCookieKey);
                    cookie.Value = TripleDESHelper.Encrypt(cacheKey);
                    if (UserCookieExpireMinutes > 0)
                        cookie.Expires = DateTime.Now.AddMinutes(UserCookieExpireMinutes);
                    context.Response.Cookies.Add(cookie);
                }
                else
                {
                    // Cookie 存在时.
                    string cookieName = TripleDESHelper.Decrypt(cookie.Value);
                    cookieName = cookieName.Trim();
                    if (!cookieName.Equals(cacheKey, StringComparison.OrdinalIgnoreCase))
                    {
                        cookie.Value = TripleDESHelper.Encrypt(cacheKey);
                        if (UserCookieExpireMinutes > 0)
                            cookie.Expires = DateTime.Now.AddMinutes(UserCookieExpireMinutes);
                        context.Response.Cookies.Add(cookie);
                    }
                }
                InsertCache(cacheKey, user);
            }
        }

        #region Logout

        /// <summary>
        /// 清除用户登录缓存数据.
        /// </summary>
        public static void RemoveUserCache()
        {
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies[UserCookieKey];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                return;

            string cacheKey = TripleDESHelper.Decrypt(cookie.Value);
            RemoveCache(cacheKey);
            context.Response.Cookies[UserCookieKey].Expires = DateTime.Now.AddDays(-1);
            context.Session.Abandon();
        }

        #endregion

        #region Cookie

        /// <summary>
        /// 从 Cookie 中获取当前登录用户名.
        /// </summary>
        /// <returns></returns>
        private static string GetUserNameFromCookie()
        {
            //由原来的通过用户名UserCookieKey改成用UserId（UserCookieName） //wbl
            HttpCookie cookie = GetCookie(UserCookieKey);//拿出用户名
            if (cookie != null)
            { 
                string userName= TripleDESHelper.Decrypt(cookie.Value);//解密
                HttpCookie CzmsCookie = GetCookie("UNameCookie");//取CZMS系统的当前用户名;
                if (userName== CzmsCookie.Value) //取流程易中的当前用户名匹配，不匹配则重新获取
                    return userName; 
            }
            cookie = GetCookie(UserCookieName);//若没有用户名则拿用户ID
            if (cookie == null)
                return string.Empty;
            Guid userId;
            UserInfo user = Guid.TryParse(cookie.Value, out userId) ? userService.GetUserByUserId(userId) : null;
            if (user == null)
                return string.Empty;
            SetUserNameToCookie(user.UserName);
            return user.UserName;

            //return (cookie != null ? cookie.Value : string.Empty);
            #region 通过用户名UserCookieKey
            //HttpCookie cookie = GetCookie(UserCookieKey);
            //string userName = (cookie != null ? cookie.Value : string.Empty);
            //if (string.IsNullOrEmpty(userName))
            //    return null;
            //return TripleDESHelper.Decrypt(cookie.Value);
            #endregion
        }
        /// <summary>
        /// 本插件的Cookie
        /// </summary>
        /// <returns></returns>
        private static HttpCookie GetCookie(string cookieKey)
        {
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies[cookieKey];
            return cookie;
        }
        /// <summary>
        /// 设置用户 Cookie(Cookie 中包含用户名)，用于实现模拟登录.
        /// </summary>
        /// <param name="userName"></param>
        private static void SetUserNameToCookie(string userName)
        {
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies[UserCookieKey];
            if (cookie == null)
            {
                cookie = new HttpCookie(UserCookieKey);
            }
            cookie.Value = TripleDESHelper.Encrypt(userName);
            if (UserCookieExpireMinutes > 0)
                cookie.Expires = DateTime.Now.AddMinutes(UserCookieExpireMinutes);
            context.Response.Cookies.Add(cookie);
        }

        #endregion

        #region Cache

        /// <summary>
        /// 新增登录用户信息到缓存中.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="user"></param>
        private static void InsertCache(string cacheKey, LoginUser user)
        {
            if (!cache.Contains(cacheKey))
                cache.Remove(cacheKey);
            cache.Add(cacheKey, user);
        }

        /// <summary>
        /// 获取指定键名的用户缓存信息.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        private static LoginUser GetCache(string cacheKey)
        {
            if (cache.Contains(cacheKey))
                return cache.GetData(cacheKey) as LoginUser;
            return null;
        }

        /// <summary>
        /// 移除指定键名的用户缓存.
        /// </summary>
        /// <param name="cacheKey"></param>
        private static void RemoveCache(string cacheKey)
        {
            if (cache.Contains(cacheKey))
            {
                cache.Remove(cacheKey);
            }
        }

        #endregion
    }
}
