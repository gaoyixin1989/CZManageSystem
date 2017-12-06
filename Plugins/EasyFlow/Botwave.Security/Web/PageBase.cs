using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Botwave.Security.Web
{
    /// <summary>
    /// 页面基础类.
    /// </summary>
    public class PageBase : Botwave.Web.PageBase
    {
        readonly static IApplicationContext ctx = ContextRegistry.GetContext();
        public static IApplicationContext Ctx
        {
            get
            {
                return ctx;
            }
        }
        private static string basePage = "~/plugins/easyflow/";
        #region user info

        /// <summary>
        /// 当前登录用户.
        /// </summary>
        protected static LoginUser CurrentUser
        {
            get { return LoginHelper.User; }
            set { LoginHelper.User = value; }
        }

        /// <summary>
        /// 获取当前登录用户名.
        /// </summary>
        protected static string CurrentUserName
        {
            get { return LoginHelper.UserName; }
        }


        public static string BasePage
        {
            get
            {
                return basePage;
            }
        }
        #endregion


        #region GMCC日志记录相关

        /// <summary>
        ///  记录异常日志
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="description"></param>
        protected static void WriteExceptionLog(string operation, string description)
        {
            WriteExLog(CurrentUserName, operation, description);
        }

        /// <summary>
        ///  记录异常日志
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="exception"></param>
        /// <param name="description"></param>
        protected static void WriteExceptionLog(string operation, string exception, string description)
        {
            WriteExLog(CurrentUserName, operation, exception, description);
        }

        /// <summary>
        /// 记录正常日志
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="description"></param>
        protected static void WriteNomalLog(string operation, string description)
        {
            WriteNomalLog(CurrentUserName, operation, description);
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="userName">操作用户</param>
        /// <param name="operation">异常名称</param>
        /// <param name="description">描述</param>
        protected static void WriteExLog(string userName, string operation, string description)
        {
            LogWriterFactory.Writer.WriteExLog(userName, operation, "异常操作", description);
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="userName">操作用户</param>
        /// <param name="operation">操作名称</param>
        /// <param name="exception">异常名称</param>
        /// <param name="description">描述</param>
        protected static void WriteExLog(string userName, string operation, string exception, string description)
        {
            LogWriterFactory.Writer.WriteExLog(userName, operation, exception, description);
        }

        /// <summary>
        /// 记录正常日志
        /// </summary>
        /// <param name="userName">操作用户</param>
        /// <param name="operation">操作名称</param>
        /// <param name="description">描述</param>
        protected static void WriteNomalLog(string userName, string operation, string description)
        {
            LogWriterFactory.Writer.WriteNomalLog(userName, operation, description);
        }
        #endregion
    }
}
