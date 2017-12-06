using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Botwave.Web
{
    /// <summary>
    /// WebUtils ��ժҪ˵����
    /// </summary>
    public static class WebUtils
    {
        private static string m_AppPath = null;

        /// <summary>
        /// ��ȡ�����վ���Ŀ¼��Ӧ�ó���·��
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
        /// ��ȡ���Ӧ�ø�Ŀ¼��Ӧ�ó���·��
        /// </summary>
        /// <returns></returns>
        public static string GetAppStrPath()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath;
        }

        /// <summary>
        /// �� HTML ����.
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
        /// ���XML��
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
        /// ѡ���б���
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
        /// ���ݴ����url��appPath(/appPath/)��ȡ����ڱ�Ӧ�õ�ҳ��·��
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
        /// �� Cookie ��ȡ�û�����.
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
        /// �����û����� Cookie.
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
        /// ������� Cookie ����.
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
        /// ҳ��ע��ű�����.
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
        /// ҳ��ע��CSS�ļ�����.
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
        /// ��ָ�������ҳ�涨�򵽵�ǰ HttpResponse ����ĸ�ҳ��.
        /// </summary>
        /// <param name="response">��ǰ��Ӧ�� Response ����.</param>
        /// <param name="path">�ض����ҳ�����·�����磺"default.aspx", "workbench.aspx", "workflow/default.aspx"��.</param>
        public static void RedirectParent(HttpResponse response, string path)
        {
            string url = WebUtils.GetAppPath() + path;
            response.Write(string.Format("<script>parent.location='{0}';</script>", url));
        }
    }
}
