using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Botwave.Web;
using Botwave.Security;
using Botwave.Security.Service;

namespace Botwave.Workflow.Practices
{
    public class BaseSSOProxy :  System.Web.UI.Page, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(BaseSSOProxy));
        protected static string workbenchUrl = WebUtils.GetAppPath() + "workbench.aspx";
        protected static IAuthService authService;

        public IAuthService AuthService
        {
            set { authService = value; }
        }

        #region Page

        public override void ProcessRequest(HttpContext context)
        {
            log.InfoFormat("ssoproxy begin ... {0}", context.Request.UrlReferrer);

            string account = GetPortalAccount(context);
            log.InfoFormat("ssoproxy account:{0}", account);
            try
            {
                OnPostGetAccount(ref account);
            }
            catch (Exception ex)
            {
                account = string.Empty;
                log.Error(ex);
            }

            if (String.IsNullOrEmpty(account))
            {
                OnGetAccountFail(context);
            }
            else
            {
                OnGetAccountSuccess(context, account);
            }
        }
        #endregion

        /// <summary>
        /// 登出处理.
        /// </summary>
        protected virtual void OnLogout()
        {
            authService.Logout(string.Empty);
        }

        private string TryGetPortalAccount(HttpContext context)
        {
            try
            {
                return GetPortalAccount(context);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return null;
        }

        protected virtual string GetPortalAccount(HttpContext context)
        {
            return null;
        }

        /// <summary>
        /// 获取 Portal 账号后的后续处理.
        /// </summary>
        /// <param name="portalAccount"></param>
        protected virtual void OnPostGetAccount(ref string portalAccount)
        {
            if (string.IsNullOrEmpty(portalAccount) || LoginHelper.UserName != portalAccount)
            {
                // 先登出已登录的账号.
                if (LoginHelper.IsLoginUser)
                    OnLogout();
            }
        }

        protected virtual void OnGetAccountFail(HttpContext context)
        {
            log.Error("没有取到用户统一登录的相关数据.");
            authService.Logout(string.Empty);
            context.Response.Redirect("login.aspx");
            return;
        }

        protected virtual void OnGetAccountSuccess(HttpContext context, string account)
        {
            log.InfoFormat("OnGetAccountSuccess:{0}", account);
            //portal验证成功，应用本地系统逻辑。根据portal账号获取登录信息，再跳转到本地系统特定页面.
            if (authService.TrustedLogin(account) != LoginStatus.Success)
            {
                log.ErrorFormat("TrustedLogin fail:{0}", account);
                context.Response.Redirect("login.aspx");
                return;
            }
            else
            {
                log.DebugFormat("TrustedLogin success:{0}", account);
                string queryString = context.Request.Url.Query;
                if (queryString != null && queryString.Length > 0)
                {
                    queryString = queryString.Remove(0, 1);
                    queryString = HttpUtility.UrlDecode(queryString);
                    context.Response.Redirect(queryString);
                }
                context.Response.Redirect(workbenchUrl);
            }
        }
    }
}
