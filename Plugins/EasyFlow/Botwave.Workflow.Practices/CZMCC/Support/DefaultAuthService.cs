using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Practices.CZMCC.Support
{
    public class DefaultAuthService : BaseAuthService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DefaultAuthService));

        protected override bool PortalLogin(string userName, string password)
        {
            string errorMessage;
            bool result = SXT.SSO.Client.SSOEntry.Login(userName, password, out errorMessage);
            log.InfoFormat("portal login:{0}, {1}", result, errorMessage);
            return result;
        }
    }
}
