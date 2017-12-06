namespace EasyFlowWeb
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Plugin.Core;

    /// <summary>
    /// 内容插件。
    /// </summary>
    public class EasyFlowPlugin : IPlugins
    {
        public string Name
        {
            get { return "EasyFlow"; }
        }

        public void Initialize()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.MapRoute(
                name: "EasyFlow",
                url: "{controller}/{action}/{id}", //url: "content/list",
                defaults: new { controller = "Content", action = "List", id = UrlParameter.Optional, pluginName = this.Name }
            );
            string sqlMapId = IBatisNet.DataMapper.Mapper.Instance().Id;
            IBatisNet.DataMapper.Mapper.Instance().SessionStore = new IBatisNet.DataMapper.SessionStore.HybridWebThreadSessionStore(sqlMapId);
        }

        public void Unload()
        {
            RouteTable.Routes.Remove(RouteTable.Routes["EasyFlow"]);
        }
    }
}