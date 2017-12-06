using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Botwave.Web
{
    /// <summary>
    /// Web 消息辅助类.
    /// </summary>
    public static class MessageHelper
    {
        /// <summary>
        /// 网站应用系统名称(签名).
        /// </summary>
        public static readonly string AppName = GlobalSettings.Instance.Signature;

        #region message fields

        private static string messageKey = GlobalSettings.Instance.MessageKey;

        /// <summary>
        /// 操作错误显示的消息内容.
        /// </summary>
        public static readonly string Message_Failture = "操作错误.";

        /// <summary>
        /// 操作成功显示的消息内容.
        /// </summary>
        public static readonly string Message_Success = "操作成功.";

        /// <summary>
        /// 参数错误显示的消息内容.
        /// </summary>
        public static readonly string Message_ArgumentException = "参数错误.";

        /// <summary>
        /// 无权限显示的消息内容.
        /// </summary>
        public static readonly string Message_NoPermission = "无权限访问.";
        public static readonly string MessagePage = "~/plugins/easyflow/";
        /// <summary>
        /// 成功消息显示的页面.
        /// </summary>
        public static readonly string MessagePage_Success = MessagePage+ "contrib/msg/pages/success.aspx";

        /// <summary>
        /// 错误消息显示的页面.
        /// </summary>
        public static readonly string MessagePage_Error = MessagePage + "contrib/msg/pages/error.aspx";

        #endregion

        /// <summary>
        /// 获取或设置当前消息内容.
        /// </summary>
        public static string MessageContent
        {
            get
            {
                return (null == HttpContext.Current.Session[messageKey]) ? string.Empty : HttpContext.Current.Session[messageKey].ToString();
            }
            set
            {
                HttpContext.Current.Session[messageKey] = value;
            }
        }

        /// <summary>
        /// 显示消息页面.
        /// </summary>
        /// <param name="targetUrl"></param>
        /// <param name="message"></param>
        /// <param name="returnUrl"></param>
        public static void ShowMessagePage(string targetUrl, string message, string returnUrl)
        {
            MessageContent = message;
            string url = targetUrl;
            HttpContext context = HttpContext.Current;
            if (null != returnUrl && returnUrl.Length != 0)
            {
                url += "?returnUrl=" + context.Server.UrlEncode(returnUrl);
            }
            context.Response.Redirect(url);
        }
    }
}
