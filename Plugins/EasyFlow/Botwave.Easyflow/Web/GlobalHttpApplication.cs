namespace Botwave.Easyflow.Web
{
    using IBatisNet.DataMapper;
    using IBatisNet.DataMapper.SessionStore;
    using log4net;
    using log4net.Config;
    using System;
    using System.IO;
    using System.Web;

    public class GlobalHttpApplication : HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GlobalHttpApplication));

        public void Application_End(object sender, EventArgs e)
        {
        }

        public void Application_Error(object sender, EventArgs e)
        {
            Exception lastError = base.Server.GetLastError();
            if (lastError != null)
            {
                log.Error(lastError);
                base.Server.ClearError();
            }
        }

        public void Application_Start(object sender, EventArgs e)
        {
            XmlConfigurator.Configure(new FileInfo(base.Server.MapPath("~/log4net.config")));
            string str = Mapper.Instance().Id;
            Mapper.Instance().SessionStore = new IBatisNet.DataMapper.SessionStore.HybridWebThreadSessionStore(str);
        }

        public void Session_End(object sender, EventArgs e)
        {
        }

        public void Session_Start(object sender, EventArgs e)
        {
        }
    }
}

