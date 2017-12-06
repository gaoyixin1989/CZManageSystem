using System;
using System.Collections.Generic;
using System.Text;

using Spring.Context;

using Botwave.Commons;
using Botwave.Events;
using Botwave.Security.Domain;
using Botwave.Security.Service;

namespace Botwave.Security
{
    /// <summary>
    /// 用户身份认证服务的默认实现类.
    /// </summary>
    [Serializable]
    public class DefaultAuthService : IAuthService, IApplicationContextAware
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DefaultAuthService));

        private IApplicationContext applicationContext;
        private IUserService userService;
        private IResourceService resourceService;

        /// <summary>
        /// 减少已引用此组件的相关修改,先初始化
        /// </summary>
        private ILoginValidator loginValidator = DefaultLoginValidator.GetInstance();

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

        /// <summary>
        /// 登录较验器

        /// </summary>
        public ILoginValidator LoginValidator
        {
            set { loginValidator = value; }
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

            LoginStatus status = LoginStatus.Unknown;
            UserInfo user = userService.GetUserByUserName(userName);
            if (user == null)
            {
                status = LoginStatus.AccountNotFound;
            }
            else
            {
                if (user.Status == -1)
                {
                    // 用户被禁用.
                    status = LoginStatus.AccountDisabled;
                }
                else
                {
                    status = loginValidator.Validate(userName, password, user);
                }
            }

            // 记录登录事件.
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

            LoginStatus status = LoginStatus.Unknown;
            if (userService == null)
                userService = LoginHelper.UserService;
            if (resourceService == null)
                resourceService = LoginHelper.ResourceService;

            // 记录登录事件.
            AppEvent appEvent = new AppEvent("security", "trusted login fail", userName);            
            UserInfo user = userService.GetUserByUserName(userName);
            if (user != null)
            {
                LoginUser loginUser = new LoginUser(user);
                loginUser.Resources = resourceService.GetResourcesByUserId(user.UserId);
                // 设置缓存.
                LoginHelper.User = loginUser;
                appEvent.Category = "trusted login success";
                status = LoginStatus.Success;
            }
            this.HandleAppEvent(appEvent);

            return status;
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
            ApplicationEventArgs args = new ApplicationEventArgs();
            applicationContext.PublishEvent(appEvent, args);
        }
    }
}
