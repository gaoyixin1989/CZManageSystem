using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Botwave.Web.UrlRewriter
{
    /// <summary>
    /// URL 重写的 HTTP 模块类.
    /// </summary>
    public class UrlRewriterModule : IHttpModule
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(UrlRewriterModule));

        #region IHttpModule 成员

        /// <summary>
        /// 销毁 HTTP 模块.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 初始化 HTTP 模块.
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            // 不进行 URL 重写.
            if (UrlRewriterContext.EnableRewriter == false 
                || UrlRewriterContext.RewriterProperties.Count ==0)
                return;
            context.BeginRequest += new EventHandler(context_BeginRequest_Rewriter);
        }

        #endregion

        #region events

        /// <summary>
        /// 重写 URL 请求的委托方法.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void context_BeginRequest_Rewriter(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            // 重写路径.
            UrlRewriterContext.RewriteUrl(context);
        }

        #endregion
    }
}
