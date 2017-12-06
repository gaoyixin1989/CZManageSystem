using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Botwave.Web;
using System.Web.UI.HtmlControls;
using System.Web;

namespace Botwave.XQP.Web.Security
{
    public class PageBase : Botwave.Security.Web.PageBase
    {
        protected override void OnInit(EventArgs e)
        {
            

            // 清除已有的主题样式表文件引用
            if (Page.Header != null)
            {
                base.OnInit(e);
                ControlCollection headerControls = Page.Header.Controls;

                headerControls.Clear();
                HtmlMeta meta = new HtmlMeta();
                meta.Attributes.Add("chartset","utf-8");
                meta.Attributes.Add("http-equiv", "X-UA-Compatible");
                meta.Content = "IE=edge";
                Page.Header.Controls.Add(meta);
                meta = new HtmlMeta();
                meta.Name = "viewport";
                meta.Content = "width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no";
                Page.Header.Controls.Add(meta);
                WebUtils.RegisterCSSReference(this.Page, AppPath + "res/bootstrap-3.2.0-dist/css/bootstrap.min.css", "stylesheet");
                WebUtils.RegisterCSSReference(this.Page, AppPath + "res/bootstrap-3.2.0-dist/css/bootstrap-theme.min.css", "stylesheet");
                WebUtils.RegisterCSSReference(this.Page, AppPath + "res/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css", "stylesheet");
                WebUtils.RegisterCSSReference(this.Page, AppPath + "app_themes/mobile/customer.css", "stylesheet");
                WebUtils.RegisterCSSReference(this.Page, AppPath + "app_themes/gmcc/showLoading.css", "stylesheet");
                WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery-1.7.2.min.js");
                WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery.cookie.js");
                WebUtils.RegisterScriptReference(this.Page, AppPath + "res/bootstrap-3.2.0-dist/js/bootstrap.min.js");
                WebUtils.RegisterScriptReference(this.Page, AppPath + "res/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js");
                WebUtils.RegisterScriptReference(this.Page, AppPath + "res/bootstrap-datetimepicker/locales/bootstrap-datetimepicker.zh-CN.js");
                WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/iscroll.js");
                WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery.showLoading.js");
            }
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
            //MessageHelper.ShowMessagePage(AppPath + "contrib/msg/pages/waperror.aspx", message, returnUrl);
            MessageHelper.MessageContent = message;
            string url = AppPath + "contrib/msg/pages/waperror.aspx";
            HttpContext context = HttpContext.Current;
            if (null != returnUrl && returnUrl.Length != 0)
            {
                url += "?returnUrl=" + context.Server.UrlEncode(returnUrl);
            }
            context.Response.Redirect(url, false);
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
            //MessageHelper.ShowMessagePage(AppPath+"contrib/msg/pages/wapsuccess.aspx", message, returnUrl);
            MessageHelper.MessageContent = message;
            string url = AppPath + "contrib/msg/pages/wapsuccess.aspx";
            HttpContext context = HttpContext.Current;
            if (null != returnUrl && returnUrl.Length != 0)
            {
                url += "?returnUrl=" + context.Server.UrlEncode(returnUrl);
            }
            context.Response.Redirect(url,false);
        }

        #endregion
    }
}
