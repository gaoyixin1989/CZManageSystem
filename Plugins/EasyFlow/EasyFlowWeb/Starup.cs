
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

[assembly: System.Web.PreApplicationStartMethod(typeof(EasyFlowWeb.Starup), "Initialize")]
namespace EasyFlowWeb
{
    public static class Starup
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Global.asax");
        static void Start()
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            ////在应用程序启动时运行的代码

            //WebApiConfig.Register(GlobalConfiguration.Configuration);  
            log4net.Config.XmlConfigurator.Configure();
            string sqlMapId = IBatisNet.DataMapper.Mapper.Instance().Id;
            IBatisNet.DataMapper.Mapper.Instance().SessionStore = new IBatisNet.DataMapper.SessionStore.HybridWebThreadSessionStore(sqlMapId);

        }

        static void Application_Error()
        {
            //在出现未处理的错误时运行的代码
            if (Botwave.Configuration.ExceptionConfig.Default.CatchException)
            {
                string message = "";
                Botwave.Security.LoginUser user = Botwave.Security.LoginHelper.User;
                if (user != null)
                {
                    string describe = user.RealName;
                    message = Botwave.Commons.ExceptionLogger.Log(describe);
                }
                else
                {
                    Exception ex = HttpContext.Current.Server.GetLastError();
                    HttpContext.Current.Server.ClearError();
                    log.Error(ex);
                    try
                    {
                        Botwave.XQP.Commons.LogWriter.Write("未知用户", ex);
                    }
                    catch (Exception ex2)
                    {
                        log.Error(ex2);
                    }
                    message = "发生错误，请联系管理员.";
                    //Response.Redirect("~/default.aspx");
                }
                Botwave.Web.PageBase.CurrentMessage = message;
                //Response.Redirect("~/contrib/msg/pages/Error.aspx");
            }
        }

    }
}
