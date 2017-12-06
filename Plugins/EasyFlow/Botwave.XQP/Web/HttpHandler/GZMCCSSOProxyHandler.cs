using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Botwave.Web;
using Botwave.Security;
using Botwave.Security.Service;
using Botwave.XQP.Service.Support;

namespace Botwave.XQP.Web.HttpHandler
{
    /// <summary>
    /// 广州移动单点登录(Portal Login)处理类.
    /// </summary>
    public class GZMCCSSOProxyHandler : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(GZMCCSSOProxyHandler));
        private static string workbenchUrl = WebUtils.GetAppPath() + "workbench.aspx";
        private static IAuthService authService;

        static GZMCCSSOProxyHandler()
        {
            authService = Spring.Context.Support.WebApplicationContext.Current["authService"] as IAuthService;
            if(authService == null)
                authService = new Botwave.XQP.Service.Support.AuthService();
        }

        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            log.Info("ssoproxy requesting ...");
            if (authService == null)
                throw new Exception("authService 为 null.");
            string account = (LoginHelper.IsLoginUser ? LoginHelper.UserName : FsllIAMS.SSO.GMCCIAMSModule.GetUserAccount());
            //string account = "admin";
            if (String.IsNullOrEmpty(account))
            {
                log.Debug("没有取到用户统一登录的相关数据.");
                authService.Logout(string.Empty);
                context.Response.Redirect("login.aspx");
                return;
            }
            else
            {
                //portal验证成功，应用本地系统逻辑。根据portal账号获取登录信息，再跳转到本地系统特定页面
                if (authService.TrustedLogin(account) != LoginStatus.Success)
                {
                    context.Response.Redirect("login.aspx");
                    return;
                }
                else
                {
                    string queryString = context.Request.Url.Query;
                    if (queryString != null && queryString.Length > 0)
                    {
                        queryString = queryString.Remove(0, 1);
                        queryString = HttpUtility.UrlDecode(queryString);
                        context.Response.Redirect(workbenchUrl + "?" + queryString);
                    }
                    context.Response.Redirect(workbenchUrl);
                }
            }
        }

        #endregion
    }
}
