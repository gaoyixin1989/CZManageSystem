using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Botwave.Commons;

using Botwave.Web;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;

namespace Botwave.Security.Extension.Web.HttpModules
{
    /// <summary>
    /// 页面访问控制 HTTP 处理模块.
    /// </summary>
    public class PageAclModule : IHttpModule, IRequiresSessionState
    {
        /// <summary>
        /// 页面控制列表字典集合
        /// </summary>
        private IDictionary<string, string> pageAcl = null;

        private string loginUrl = "contrib/workflow/pages/default.aspx";
        private IResourceService resourceService;

        /// <summary>
        /// 登录 URL.
        /// </summary>
        public string LoginUrl
        {
            set { loginUrl = value; }
        }

        /// <summary>
        /// 系统资源服务.
        /// </summary>
        public IResourceService ResourceService
        {
            set { resourceService = value; }
        }

        /// <summary>
        /// 获取页面控制列表字典集合.
        /// </summary>
        /// <returns></returns>
        private void InitPageAcl()
        {
            pageAcl = new Dictionary<string, string>();
            if (resourceService != null)
            {
                IList<ResourceInfo> resources = resourceService.GetResourcesByType("page");
                int count = resources.Count;
                for (int i = 0; i < count; i++)
                {
                    //适应一资源对应多页面的情况, 以半角或者全角逗号隔开
                    string[] nameArray = resources[i].Name.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in nameArray)
                    {
                        string key = item.Trim();
                        if (!pageAcl.ContainsKey(key))
                        {
                            pageAcl.Add(key, resources[i].ResourceId);
                        }
                    }
                }
            }
        }

        #region IHttpModule 成员

        /// <summary>
        /// 销毁.
        /// </summary>
        public void Dispose()
        { }

        /// <summary>
        ///  HttpModule 初始化.
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            InitPageAcl();
            context.AcquireRequestState += new EventHandler(this.DoAclFilter);
        }

        #endregion

        /// <summary>
        /// 访问控制过滤.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoAclFilter(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            string portalAccount = context.Request["user"];
            if (!string.IsNullOrEmpty(portalAccount))
                LoginHelper.UserName = portalAccount;

            //根据传入的url及appPath(/appPath/)获取相对于本应用的页面路径       
            string appPath = WebUtils.GetAppPath();
            string pagePath = WebUtils.GetPagePath(context.Request.Url.ToString(), appPath).ToLower();
            if (pageAcl.ContainsKey(pagePath))
            {
                string requiredResources = pageAcl[pagePath];
                LoginUser currentUser = LoginHelper.User;
                if (null != currentUser)
                {
                    if (requiredResources == null
                        || requiredResources.Length == 0
                        || currentUser.HasAnyResources(requiredResources))
                        return;
                } 
                MessageHelper.MessageContent = MessageHelper.Message_NoPermission;
                loginUrl = (LoginHelper.User == null ? "contrib/security/pages/login.aspx" : "contrib/workflow/pages/default.aspx");
                context.Response.Redirect(MessageHelper.MessagePage_Error + "?returnUrl=" + appPath + loginUrl);
            }
        }
    }
}
