using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Botwave.Web;
using Botwave.Web.Themes;
using Spring.Context;
using Spring.Context.Support;

namespace Botwave.Web
{
    /// <summary>
    /// 页面基础类.
    /// </summary>
    public class PageBase : System.Web.UI.Page
    {
        readonly static IApplicationContext ctx = ContextRegistry.GetContext();
        public static IApplicationContext Ctx
        {
            get
            {
                return ctx;
            }
        }
        /// <summary>
        /// 应用程序名称.
        /// </summary>
        protected static readonly string AppName = GlobalSettings.Instance.Signature;

        /// <summary>
        /// 应用程序路径.
        /// </summary>
        protected static string AppPath
        {
            get { return WebUtils.GetAppPath(); }
        }

        /// <summary>
        /// 系统提示信息
        /// </summary>
        public static string CurrentMessage
        {
            get
            {
                object message = System.Web.HttpContext.Current.Session[GlobalSettings.Instance.MessageKey];
                return (null == message) ? String.Empty : message.ToString();
            }
            set
            {
                System.Web.HttpContext.Current.Session[GlobalSettings.Instance.MessageKey] = value;
            }
        }

        /// <summary>
        /// 初始化的前续处理.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // [旧]主题 CSS 引用实现方式
            //if (GlobalSettings.Instance.AllowTheme)
            //{
            //    string themeName = WebUtils.GetThemeName();
            //    if (string.IsNullOrEmpty(themeName))
            //        themeName = string.IsNullOrEmpty(this.Theme) ? this.StyleSheetTheme : this.Theme;

            //    if (!string.IsNullOrEmpty(themeName) && this.StyleSheetTheme != themeName)
            //    {
            //        this.StyleSheetTheme = themeName;
            //    }
            //}
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // [新]主题CSS 引用实现方式
            ThemeHelper.InitTheme(this);
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
