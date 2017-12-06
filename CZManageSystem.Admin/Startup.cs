using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
//using Plugin.Core;

[assembly: OwinStartup(typeof(CZManageSystem.Admin.Startup))]

namespace CZManageSystem.Admin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.Use<PluginsMiddleware>();
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
