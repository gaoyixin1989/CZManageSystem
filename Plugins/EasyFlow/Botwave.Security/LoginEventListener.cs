using System;
using System.Collections.Generic;
using System.Text;

using Spring.Context;

using Botwave.Events;

namespace Botwave.Security
{
    /// <summary>
    /// 登录事件监听类.
    /// </summary>
    public class LoginEventListener : IApplicationEventListener
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoginEventListener));

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
                && appEvent.Category.IndexOf("login") != -1)
            {
                log.Info(appEvent);
            }
        }

        #endregion
    }
}
