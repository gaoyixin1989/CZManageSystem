using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Botwave.Web.Themes
{
    /// <summary>
    /// 主题辅助类.
    /// </summary>
    public static class ThemeHelper
    {
        /// <summary>
        /// 主题存放的相对根目录.
        /// </summary>
        public static string ThemesRootAbsolutPath =  "App_Themes/";//"App_Themes/";//

        /// <summary>
        /// 主题存放的根目录.
        /// </summary>
        private static string ThemesRootPath;

        /// <summary>
        /// 同步锁.
        /// </summary>
        private static object syncLock = new object();

        /// <summary>
        /// 主题头部信息内容(key:主题名称, value: html).
        /// </summary>
        private static IDictionary<string, string> ThemeHeaders;

        /// <summary>
        /// 构造方法.
        /// </summary>
        static ThemeHelper()
        {
            ThemeHeaders = new Dictionary<string, string>();
            ThemesRootPath = HttpContext.Current.Server.MapPath("~/plugins/easyflow/" + ThemesRootAbsolutPath);
            
        }

        /// <summary>
        /// 初始化指定页面的主题.
        /// </summary>
        /// <param name="currentPage"></param>
        public static void InitTheme(Page currentPage)
        { 
            if (GlobalSettings.Instance.AllowTheme && currentPage != null)
            {
                string themeName = WebUtils.GetThemeName();
                if (string.IsNullOrEmpty(themeName))
                    themeName = string.IsNullOrEmpty(currentPage.Theme) ? currentPage.StyleSheetTheme : currentPage.Theme;

                string pageHeader = ThemeHelper.GetTheme(themeName);
                if (!string.IsNullOrEmpty(pageHeader) && currentPage.Header != null)
                {
                    // 清除已有的主题样式表文件引用
                    ControlCollection headerControls = currentPage.Header.Controls;
                    if (headerControls != null && headerControls.Count > 0)
                    {
                        for (int i = 0; i < headerControls.Count; i++)
                        {
                            HtmlLink link = headerControls[i] as HtmlLink;
                            if (link == null)
                                continue;
                            if (link.Href.IndexOf(ThemeHelper.ThemesRootAbsolutPath, StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                headerControls.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    currentPage.Header.Controls.Add(new LiteralControl(pageHeader));
                }
            }
        }

        /// <summary>
        /// 获取主题引用代码.
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns></returns>
        private static string GetTheme(string themeName)
        {
            if (string.IsNullOrEmpty(themeName))
                return string.Empty;

            if (ThemeHeaders.ContainsKey(themeName))
                return ThemeHeaders[themeName];

            string themeValue = GetThemeHeader(themeName);
            lock (syncLock)
            {
                ThemeHeaders[themeName] = themeValue;
            }
            return themeValue;
        }

        /// <summary>
        /// 生成指定主题的头部  HTML 引用内容.
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns></returns>
        private static string GetThemeHeader(string themeName)
        {
            string themePath = Path.Combine(ThemesRootPath, themeName);
            if (!Directory.Exists(themePath))
                return string.Empty;
            DirectoryInfo themeDirectory = new DirectoryInfo(themePath);
            FileInfo[] files = themeDirectory.GetFiles("*.css", SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            foreach (FileInfo item in files)
            {
                string path = item.FullName;
                path = path.Replace(ThemesRootPath, "").Replace('\\', '/');
                path = WebUtils.GetAppPath() + ThemesRootAbsolutPath + path;
                builder.AppendFormat("\r\n<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}\" />", path);
            }
            return builder.ToString();
        }
    }
}
