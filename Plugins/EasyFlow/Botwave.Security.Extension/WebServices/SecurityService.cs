using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;

namespace Botwave.Security.Extension.WebServices
{
    /// <summary>
    /// 用户安全 Web 服务.
    /// </summary>
    [WebService(Namespace = "Botwave.Security.Extension.WebServices.SecurityService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SecurityService : WebService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(SecurityAjaxService));

        private IResourceService resourceService;

        /// <summary>
        /// 权限资源服务.
        /// </summary>
        public IResourceService ResourceService
        {
            get { return resourceService; }
            set { resourceService = value; }
        }

        /// <summary>
        /// 默认构造方法.
        /// </summary>
        public SecurityService()
        {
            Spring.Context.IApplicationContext applicationContext = Spring.Context.Support.ContextRegistry.GetContext();
            this.resourceService = applicationContext["resourceService"] as IResourceService;
        }

        /// <summary>
        /// 获取指定用户名以及资源前缀的用户权限资源.
        /// 返回用户权限资源编号的字符串列表.
        /// </summary>
        /// <param name="userName">指定用户名.</param>
        /// <param name="resourcePrefix">显示的资源前缀.</param>
        /// <returns>返回用户权限资源编号的字符串列表.</returns>
        [WebMethod(Description="获取指定用户名以及资源前缀的用户权限资源.")]
        public List<string> GetResourcesByUser(string userName, string resourcePrefix)
        {
            IDictionary<string, string> dict = resourceService.GetResourcesByUserName(userName, resourcePrefix);
            List<string> results = new List<string>();
            foreach (string key in dict.Keys)
            {
                results.Add(key);
            }
            return results;
        }
    }
}
