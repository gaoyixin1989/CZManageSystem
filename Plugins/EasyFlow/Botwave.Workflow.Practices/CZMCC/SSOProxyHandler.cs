using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Botwave.Web;
using Botwave.Security;
using Botwave.Security.Service;
using Botwave.Workflow.Practices.CZMCC.Support;

namespace Botwave.Workflow.Practices.CZMCC
{
    /// <summary>
    /// 潮州移动单点登录(Portal Login)处理类.
    /// </summary>
    public class SSOProxyHandler : BaseSSOProxy
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(SSOProxyHandler));

        static SSOProxyHandler()
        {
            authService = new Botwave.Workflow.Practices.CZMCC.Support.DefaultAuthService();
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public SSOProxyHandler()
        {  }

        protected override string GetPortalAccount(HttpContext context)
        {
            try
            {
               log.InfoFormat("UserName:{0}; Token:{1}; TokenKey:{2}; UserID:{3}; EmployeeId:{4}",
                    SXT.SSO.Client.SSOEntry.CurrentUserName,
                    SXT.SSO.Client.SSOEntry.Token,
                    SXT.SSO.Client.SSOEntry.TokenKey,
                    SXT.SSO.Client.SSOEntry.CurrentUserID,
                    SXT.SSO.Client.SSOEntry.CurrentEmployeeID);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("display: {0}", ex.ToString());
            }

            try
            {
                return SXT.SSO.Client.SSOEntry.CurrentEmployeeID;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        protected override void OnPostGetAccount(ref string portalAccount)
        {
            if (string.IsNullOrEmpty(portalAccount) && LoginHelper.IsLoginUser)
            {
                portalAccount = LoginHelper.UserName;
                log.DebugFormat("sso.current: {0}", portalAccount);
            }
        }
    }
}
