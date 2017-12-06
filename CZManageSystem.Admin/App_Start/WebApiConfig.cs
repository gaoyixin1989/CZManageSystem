using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CZManageSystem.Admin
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));//允许跨域访问  默认情况下，ASP.NET Web API是不支持跨域访问的。为了支持，需要安装Microsoft.AspNet.WebApi.Cors。安装之后，需要在全局配置生效。

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
