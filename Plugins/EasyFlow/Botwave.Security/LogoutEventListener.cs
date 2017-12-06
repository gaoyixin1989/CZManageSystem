using System;
using System.Collections.Generic;
using System.Text;

using Spring.Context;

using Botwave.Events;

namespace Botwave.Security
{
    /// <summary>
    /// 登出事件监听类.
    /// </summary>
    public class LogoutEventListener : IApplicationEventListener
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LogoutEventListener));

        #region IApplicationEventListener Members

        /// <summary>
        /// 处理应用程序事件.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleApplicationEvent(object sender, ApplicationEventArgs e)
        {
            AppEvent appEvent = sender as AppEvent;
            if (null != appEvent
                && appEvent.AppName == "security"
                && appEvent.Category == "logout")
            {
                log.Info(appEvent);
            }
        }

        #endregion
    }
}
