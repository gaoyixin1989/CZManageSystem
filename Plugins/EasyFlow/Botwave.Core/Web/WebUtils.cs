using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Botwave.Web
{
    /// <summary>
    /// WebUtils 的摘要说明。
    /// </summary>
    public static class WebUtils
    {
        private static string m_AppPath = null;

        /// <summary>
        /// 获取相对于站点根目录的应用程序路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppPath()
        {

            //return  HttpContext.Current.Request.ApplicationPath + "plugins/easyflow/";
            if (m_AppPath == null)
            {
                m_AppPath = HttpContext.Current.Request.ApplicationPath + "plugins/easyflow";
                if (m_AppPath != "/")
                {
                    m_AppPath += "/";
                }
            }

            return m_AppPath;
        }

        /// <summary>
        /// 获取相对应用根目录的应用程序路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppStrPath()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath;
        }

        /// <summary>
        /// 简单 HTML 编码.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string HtmlEncode(string text)
        {
            text = text.Replace(" ", "&nbsp;");
            text = text.Replace("\r\n", "<br />");
            return text;
        }

        /// <summary>
        /// 输出XML串
        /// </summary>
        /// <param name="xml"></param>
        public static void ResponseXML(string xml)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Cache-Control", "no-cache");
            response.ContentType = "text/xml";
            response.Write(xml);
            response.End();
        }

        /// <summary>
        /// 选择列表项
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="val"></param>
        public static void SelectDropDownList(DropDownList ddl, string val)
        {
            if (null == val)
            {
                return;
            }

            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (ddl.Items[i].Value == val || ddl.Items[i].Text == val)
                {
                    ddl.Items[i].Selected = true;
                    return;
                }
            }
        }


        /// <summary>
        /// 根据传入的url及appPath(/appPath/)获取相对于本应用的页面路径
        /// GetPagePath("http://localhost/app/Report/Default.aspx?", "/APP/") = Report/Default.aspx
        /// GetPagePath("http://localhost/Report/Default.aspx?", "/") = Report/Default.aspx
        /// </summary>
        /// <param name="url"></param>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetPagePath(string url, string appPath)
        {
            if (String.IsNullOrEmpty(url) || String.IsNullOrEmpty(appPath))
            {
                return String.Empty;
            }

            url = url.ToLower();
            int idx = url.IndexOf(".aspx");
            if (idx <= 0)
            {
                return String.Empty;
            }

            const int LENGTH_OF_ASPX_POSTFIX = 5;	//.aspx
            const int LENGTH_OF_HTTP_PREFIX = 7;	//http://            
            int idxOfAppPath = 0;
            if (appPath == "/")
            {
                idxOfAppPath = url.IndexOf('/', LENGTH_OF_HTTP_PREFIX);
            }
            else
            {
                appPath = appPath.ToLower();
                idxOfAppPath = url.IndexOf(appPath);
            }

            if (idxOfAppPath <= 0)
            {
                return String.Empty;
            }

            int start = idxOfAppPath + appPath.Length;
            int len = idx + LENGTH_OF_ASPX_POSTFIX - start;
            return url.Substring(start, len);
        }

        /// <summary>
        /// 从 Cookie 获取用户主题.
        /// </summary>
        /// <returns></returns>
        public static string GetThemeName()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[GlobalSettings.Instance.ThemeKey];
            if (cookie == null)
                return string.Empty;
            return cookie.Value;
        }

        /// <summary>
        /// 设置用户主题 Cookie.
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns></returns>
        public static void SetThemeName(string themeName)
        {
            string themeKey = GlobalSettings.Instance.ThemeKey;
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies[themeKey];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                context.Request.Cookies.Remove(themeKey);
            }
            cookie = new HttpCookie(themeKey);
            cookie.Expires = DateTime.Now.AddMonths(1);
            cookie.Value = themeName;
            context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 清除主题 Cookie 数据.
        /// </summary>
        public static void ClearThemeCookie()
        {
            string themeKey = GlobalSettings.Instance.ThemeKey;
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies[themeKey];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                context.Request.Cookies.Remove(themeKey);
            }
        }

        /// <summary>
        /// 页面注册脚本引用.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="filePath"></param>
        public static void RegisterScriptReference(Page currentPage, string filePath)
        {
            HtmlGenericControl script = new HtmlGenericControl("script");
            script.Attributes.Add("type", "text/javascript");
            script.Attributes.Add("src", filePath);
            currentPage.Header.Controls.Add(script);
        }

        /// <summary>
        /// 页面注册CSS文件引用.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="filePath"></param>
        /// <param name="title"></param>
        public static void RegisterCSSReference(Page currentPage, string filePath, string title)
        {
            HtmlLink link = new HtmlLink();
            link.Attributes["rel"] = "stylesheet";
            link.Attributes["type"] = "text/css";
            link.Attributes["media"] = "all";
            if (!string.IsNullOrEmpty(title))
                link.Attributes["title"] = title;
            link.Attributes["href"] = filePath;
            currentPage.Header.Controls.Add(link);
        }

        /// <summary>
        /// 将指定的相对页面定向到当前 HttpResponse 对象的父页面.
        /// </summary>
        /// <param name="response">当前响应的 Response 对象.</param>
        /// <param name="path">重定向的页面相对路径（如："default.aspx", "workbench.aspx", "workflow/default.aspx"）.</param>
        public static void RedirectParent(HttpResponse response, string path)
        {
            string url = WebUtils.GetAppPath() + path;
            response.Write(string.Format("<script>parent.location='{0}';</script>", url));
        }
    }
}
