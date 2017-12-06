namespace Botwave.Easyflow.Practices
{
    using System;
    using System.Web.SessionState;
    using System.Web.UI;

    public class SSOProxyHandler : Page, IRequiresSessionState
    {
        protected virtual string GetPortalAccount()
        {
            return string.Empty;
        }

        protected virtual void OnLoginFailure()
        {
        }

        protected virtual void OnLoginSuccess()
        {
        }
    }
}

