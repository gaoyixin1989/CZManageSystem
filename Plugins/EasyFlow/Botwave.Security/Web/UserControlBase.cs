using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Security.Web
{
    /// <summary>
    /// 用户控件基础类.
    /// </summary>
    public class UserControlBase : Botwave.Web.UserControlBase
    {
        readonly static IApplicationContext ctx = ContextRegistry.GetContext();
        public static IApplicationContext Ctx
        {
            get
            {
                return ctx;
            }
        }
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
        #endregion
    }
}
