using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Botwave.Web.RequestTracker
{
    /// <summary>
    /// 请求跟踪 HttpModule 类.
    /// </summary>
    public class RequestTrackerHttpModule : IHttpModule
    {
        #region IHttpModule Members

        /// <summary>
        /// 销毁.
        /// </summary>
        public void Dispose()
        {
            //
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(LogRequest);
        }

        #endregion

        /// <summary>
        /// 记录请求.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogRequest(object sender, EventArgs e)
        {
            RequestLogger.Log(HttpContext.Current.Request);
        }
    }
}
