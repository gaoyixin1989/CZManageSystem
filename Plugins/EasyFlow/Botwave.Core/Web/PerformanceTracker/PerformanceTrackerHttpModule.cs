using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Botwave.Web.PerformanceTracker
{
    /// <summary>
    /// 性能跟踪 HttpModule 类.
    /// </summary>
    public class PerformanceTrackerHttpModule : IHttpModule
    {
        private DateTime beginTime;

        #region IHttpModule Members

        /// <summary>
        /// 销毁.
        /// </summary>
        public void Dispose()
        {
            //
        }

        /// <summary>
        /// HttpModule 初始化.
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(LogRequest);
        }

        #endregion

        private void context_BeginRequest(object sender, EventArgs e)
        {
            beginTime = DateTime.Now;
        }

        /// <summary>
        /// 记录请求.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogRequest(object sender, EventArgs e)
        {
            PerfLogger.Log(HttpContext.Current.Request, beginTime, DateTime.Now);
        }
    }
}
