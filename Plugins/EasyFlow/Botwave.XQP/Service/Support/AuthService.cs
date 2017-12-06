using System;
using System.Collections.Generic;
using System.Text;
using Spring.Context;
using Botwave.Commons;
using Botwave.Events;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Security.Configuration;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// XQP 用户身份验证服务实现类.
    /// </summary>
    public class AuthService : Botwave.Security.IAuthService, IApplicationContextAware
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AuthService));

        private IApplicationContext applicationContext;
        private IUserService userService;
        private IResourceService resourceService;

        /// <summary>
        /// 用户服务接口.
        /// </summary>
        public IUserService UserService
        {
            set { userService = value; }
        }

        /// <summary>
        /// 权限资源服务接口.
        /// </summary>
        public IResourceService ResourceService
        {
            set { resourceService = value; }
        }

        #region IAuthService Members

        /// <summary>
        /// 登录认证.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LoginStatus Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("登录用户名(userName)为空.");
            if (userService == null)
                userService = LoginHelper.UserService;
            if (resourceService == null)
                resourceService = LoginHelper.ResourceService;
            if (userService == null || resourceService == null)
                throw new NullReferenceException("userService, resourceService 未引用对象.");

            LoginStatus status = LoginStatus.Unknown;
            UserInfo user = userService.GetUserByUserName(userName);
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
                        // 内部用户 Portal 登录
                        try
                        {
                            log.Info("protal validate ....");
                            // 内部用户 Portal 登录
                            Botwave.GmccWsProxies.AuthResult result = Botwave.GmccWsProxies.ServiceFactory.GetUIPService().CommonLogin(userName, password, SecurityConfig.Default.PortalValidateToken, 1);
                            if (!result.authResult)
                                status = Botwave.Security.LoginStatus.InvalidValidateToken;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                    else
                    {
                        //加密方式存在一些问题,明码可能对应多个密码
                        string decryptedPassword = TripleDESHelper.Decrypt(user.Password);

                        if (password != decryptedPassword)
                            status = LoginStatus.InvalidPassword;
                    }
                }
            }

            // 记录登录事件
            AppEvent appEvent = new AppEvent("security", "login fail", userName);
            appEvent.Data = password;

            if (status == LoginStatus.Success)
            {
                LoginUser loginUser = new LoginUser(user);
                loginUser.Resources = resourceService.GetResourcesByUserId(user.UserId);
                LoginHelper.User = loginUser;
                appEvent.Category = "login success";
            }

            this.HandleAppEvent(appEvent);
            return status;
        }

        /// <summary>
        /// 可信的登录认证.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public LoginStatus TrustedLogin(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("登录用户名(userName)为空.");

            if (userService == null)
                userService = LoginHelper.UserService;
            if (resourceService == null)
                resourceService = LoginHelper.ResourceService;

            // 记录登录事件.
            AppEvent appEvent = new AppEvent("security", "trusted login fail", userName);
            this.HandleAppEvent(appEvent);

            UserInfo user = userService.GetUserByUserName(userName);
            if (user != null)
            {
                LoginUser loginUser = new LoginUser(user);
                loginUser.Resources = resourceService.GetResourcesByUserId(user.UserId);
                // 设置缓存.
                LoginHelper.User = loginUser;
                appEvent.Category = "trusted login success";
                return LoginStatus.Success;
            }
            return LoginStatus.Unknown;
        }

        /// <summary>
        /// 注销(登出).
        /// </summary>
        /// <param name="userName"></param>
        public void Logout(string userName)
        {
            // 记录登出事件
            AppEvent appEvent = new AppEvent("security", "logout", userName);
            this.HandleAppEvent(appEvent);

            // 清除缓存.
            LoginHelper.RemoveUserCache();
        }

        #endregion

        #region IApplicationContextAware Members

        /// <summary>
        /// 应用程序上下文.
        /// </summary>
        public IApplicationContext ApplicationContext
        {
            get { return applicationContext; }
            set { applicationContext = value; }
        }

        #endregion

        /// <summary>
        /// 处理 AppEvent 对象.
        /// </summary>
        /// <param name="appEvent"></param>
        private void HandleAppEvent(AppEvent appEvent)
        {
            if (applicationContext != null)
            {
                ApplicationEventArgs args = new ApplicationEventArgs();
                applicationContext.PublishEvent(appEvent, args);
            }
            else
            {
                log.Debug(appEvent.ToString());
            }
        }
    }
}
