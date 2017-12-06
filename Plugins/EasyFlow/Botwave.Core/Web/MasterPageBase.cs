using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Web;

namespace Botwave.Web
{
    /// <summary>
    /// 模板页基础类.
    /// </summary>
    public class MasterPageBase : System.Web.UI.MasterPage
    {
        /// <summary>
        /// 应用程序路径.
        /// </summary>
        protected static string AppPath
        {
            get { return WebUtils.GetAppPath(); }
        }
    }
}
