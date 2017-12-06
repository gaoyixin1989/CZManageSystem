using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Autofac;
using System.Configuration;
using CZManageSystem.Core;
using Autofac.Integration.Mvc;
using CZManageSystem.Service.SysManger;
using CZManageSystem.Admin.Models;
using System.IO;
using System.Data.Entity.Infrastructure.Interception;

namespace CZManageSystem.Admin
{
    public class Global : HttpApplication
    {
        //private ISysUserService _userService = null;
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            //DbInterception.Add(new EFIntercepterLogging());//捕获的sql语句
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Add(new RazorViewEngine
            {

                ViewLocationFormats = new string[]{
                    "~/views/SysManager/{1}/{0}.cshtml",
                    "~/views/Composite/{1}/{0}.cshtml",
                    "~/views/FilesManager/{1}/{0}.cshtml",
                    "~/views/Administrative/{1}/{0}.cshtml",
                    "~/views/ITSupport/{1}/{0}.cshtml",
                    "~/views/Administrative/VehicleManages/{1}/{0}.cshtml",
                    "~/views/Administrative/BirthControl/{1}/{0}.cshtml",
                    "~/views/Query/{1}/{0}.cshtml",
                    "~/views/HumanResources/{1}/{0}.cshtml",
                    "~/views/CollaborationCenter/Invest/{1}/{0}.cshtml", 
                    "~/views/HumanResources/CheckingIn/{1}/{0}.cshtml",
                    "~/views/HumanResources/OverTime/{1}/{0}.cshtml",
                    "~/views/HumanResources/Integral/{1}/{0}.cshtml",
                    "~/views/HumanResources/AnnualLeave/{1}/{0}.cshtml",
                     "~/views/HumanResources/Vacation/{1}/{0}.cshtml" ,
                     "~/views/Administrative/Dinning/{1}/{0}.cshtml",
                    "~/views/CollaborationCenter/SmsManager/{1}/{0}.cshtml",
                    "~/views/OperatingFloor/ComeBack/{1}/{0}.cshtml",
                    "~/views/CollaborationCenter/MarketOrder/{1}/{0}.cshtml",
                    "~/views/HumanResources/Knowledge/{1}/{0}.cshtml",
                    "~/views/CollaborationCenter/Payment/{1}/{0}.cshtml",
                    "~/views/CollaborationCenter/ReturnPremium/{1}/{0}.cshtml"
                }


            });
            ///依赖注册
            //DependencyRegistrar.Register();
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("~/Plugins/EasyFlow/Log4Net.config")));
            string sqlMapId = IBatisNet.DataMapper.Mapper.Instance().Id;
            IBatisNet.DataMapper.Mapper.Instance().SessionStore = new IBatisNet.DataMapper.SessionStore.HybridWebThreadSessionStore(sqlMapId);
        }

        #region Session_Start
        protected void Session_Start(Object sender, EventArgs e)
        {



        }
        #endregion

        void Application_BeginRequest(object sender, EventArgs e)
        {

            if (Request.Cookies != null)
            {
                if (SafeCheck.CookieData())
                {
                    Response.ContentType = "text/html; charset=UTF-8";
                    Response.Write("您提交的Cookie数据有恶意字符！");
                    Response.End();

                }


            }

            if (Request.UrlReferrer != null)
            {
                if (SafeCheck.referer())
                {
                    Response.ContentType = "text/html; charset=UTF-8";
                    Response.Write("您提交的UrlReferrer 数据有恶意字符！");
                    Response.End();
                }
            }

            if (Request.RequestType.ToUpper() == "POST")
            {
                if (SafeCheck.PostData())
                {
                    Response.ContentType = "text/html; charset=UTF-8";
                    Response.Write("您提交的Post数据有恶意字符！");
                    Response.End();
                }
            }
            if (Request.RequestType.ToUpper() == "GET")
            {
                if (SafeCheck.GetData())
                {

                    Response.ContentType = "text/html; charset=UTF-8";
                    Response.Write("您提交的Get数据有恶意字符！");
                    Response.End();
                }
            }


        }
    }
}