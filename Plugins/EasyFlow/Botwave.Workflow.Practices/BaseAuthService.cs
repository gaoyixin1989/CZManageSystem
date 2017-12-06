using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Commons;
using Botwave.Events;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Security.Configuration;

namespace Botwave.Workflow.Practices
{
    /// <summary>
    /// 用户身份验证服务实现类.
    /// </summary>
    public abstract class BaseAuthService : IAuthService
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(BaseAuthService));
        protected static IUserService userService = Botwave.Security.LoginHelper.UserService;
        protected static IResourceService resourceService = Botwave.Security.LoginHelper.ResourceService;

        public IUserService UserService
        {
            get { return userService; }
            set { userService = value; }
        }

        public IResourceService ResourceService
        {
            get { return resourceService; }
            set { resourceService = value; }
        }
        #region IAuthService Members

        public LoginStatus Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("登录用户名(userName)为空.");

            LoginStatus status = LoginStatus.Unknown;
            UserInfo user = GetUser(userName);
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
                    // 先本地身份验证.
                    status = Botwave.Security.LoginStatus.Success;
                    // 本地验证失败，则 portal 身份验证
                    if (user.Type == 0 && SecurityConfig.Default.IsPortalLogin)
                    {
                        log.DebugFormat("protal validate : {0} ....", userName);
                        // 内部用户 Portal 登录
                        if (!PortalLogin(userName, password))
                            status = Botwave.Security.LoginStatus.InvalidValidateToken;
                    }
                    else
                    {
                        string decryptedPassword = TripleDESHelper.Decrypt(user.Password);

                        if (password != decryptedPassword)
                        {
                            status = Botwave.Security.LoginStatus.InvalidPassword;
                        }
                    }
                }
            }

            // 记录登录事件
            AppEvent appEvent = new AppEvent("security", "login fail", userName);

            if (status == LoginStatus.Success)
            {
                LoginUser loginUser = new LoginUser(user);
                loginUser.Resources = resourceService.GetResourcesByUserId(user.UserId);
                this.AddLoginCache(userName, loginUser);
                appEvent.Category = "login success";
            }

            this.HandleAppEvent(appEvent);
            return status;
        }

        public void Logout(string userName)
        {
            // 记录登出事件
            AppEvent appEvent = new AppEvent("security", "logout", userName);
            this.HandleAppEvent(appEvent);

            // 清除缓存.
            this.RemoveLoginCache();
        }

        public LoginStatus TrustedLogin(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("登录用户名(userName)为空.");

            // 记录登录事件.
            AppEvent appEvent = new AppEvent("security", "trusted login", userName);
            this.HandleAppEvent(appEvent);

            try
            {
                UserInfo user = this.GetUser(userName);
                if (user != null)
                {
                    LoginUser loginUser = new LoginUser(user);
                    loginUser.Resources = resourceService.GetResourcesByUserId(user.UserId);
                    // 设置缓存.
                    this.AddLoginCache(userName, loginUser);
                    appEvent.Category = "trusted login success";
                    this.HandleAppEvent(appEvent);
                    return LoginStatus.Success;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            // 记录登录事件.
            appEvent = new AppEvent("security", "trusted login:" + LoginStatus.Unknown, userName);
            this.HandleAppEvent(appEvent);

            return LoginStatus.Unknown;
        }

        #endregion

        #region method

        /// <summary>
        /// 获取用户信息.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual UserInfo GetUser(string user)
        {
            if (userService == null || string.IsNullOrEmpty(user))
                return null;
            UserInfo userdata = userService.GetUserByUserName(user);
            if (userdata == null)
                userdata = userService.GetUserByEmployeeId(user);
            return userdata;
        }

        /// <summary>
        /// 设置登录缓存.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="user"></param>
        protected virtual void AddLoginCache(string userName, LoginUser user)
        {
            Botwave.Security.LoginHelper.User = user;
        }

        /// <summary>
        /// 清除登录缓存.
        /// </summary>
        protected virtual void RemoveLoginCache()
        {
            Botwave.Security.LoginHelper.RemoveUserCache();
        }

        /// <summary>
        /// 处理 AppEvent 对象.
        /// </summary>
        /// <param name="appEvent"></param>
        protected void HandleAppEvent(AppEvent appEvent)
        {
            log.Info(appEvent.ToString());
            Botwave.Security.Web.LogWriterFactory.Writer=new Botwave.XQP.Service.Support.DefaultLogWriter();
            Botwave.Security.Web.LogWriterFactory.Writer.WriteNomalLog(appEvent.Message, "用户登录", appEvent.Category);
        }
        #endregion

        /// <summary>
        /// 内部用户 portal 登录.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected abstract bool PortalLogin(string userName, string password);
    }
}
