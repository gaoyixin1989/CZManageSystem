using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Web;

namespace Botwave.Web
{
    /// <summary>
    /// 用户控件基础类.
    /// </summary>
    public class UserControlBase : System.Web.UI.UserControl
    {
        /// <summary>
        /// 应用程序路径.
        /// </summary>
        protected static string AppPath
        {
            get { return WebUtils.GetAppPath(); }
        }

        #region show message page

        /// <summary>
        /// 在错误页面显示指定错误信息.
        /// </summary>
        protected void ShowError()
        {
            ShowError(MessageHelper.Message_Failture);
        }

        /// <summary>
        /// 在错误页面显示指定错误信息.
        /// </summary>
        /// <param name="message">错误信息.</param>
        protected void ShowError(string message)
        {
            ShowError(message, null);
        }

        /// <summary>
        /// 在错误页面显示指定错误信息.
        /// </summary>
        /// <param name="message">错误信息.</param>
        /// <param name="returnUrl">返回的 URL 地址.</param>
        protected void ShowError(string message, string returnUrl)
        {
            MessageHelper.ShowMessagePage(MessageHelper.MessagePage_Error, message, returnUrl);
        }

        /// <summary>
        /// 在成功页面显示默认成功信息.
        /// </summary>
        protected void ShowSuccess()
        {
            ShowSuccess(MessageHelper.Message_Success);
        }

        /// <summary>
        /// 在成功页面显示指定成功消息.
        /// </summary>
        /// <param name="message">消息.</param>
        protected void ShowSuccess(string message)
        {
            ShowSuccess(message, null);
        }

        /// <summary>
        /// 在成功页面显示默认成功信息.
        /// </summary>
        /// <param name="message">消息.</param>
        /// <param name="returnUrl">返回的 URL 地址.</param>
        protected void ShowSuccess(string message, string returnUrl)
        {
            MessageHelper.ShowMessagePage(MessageHelper.MessagePage_Success, message, returnUrl);
        }

        #endregion
    }
}
