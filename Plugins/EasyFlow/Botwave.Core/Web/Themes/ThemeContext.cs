using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Botwave.Web.Themes
{
    /// <summary>
    /// 主题上下文.
    /// </summary>
    public static class ThemeContext
    {
        /// <summary>
        /// 没惹的主题名称.
        /// </summary>
        public static string DefaultTheme;

        /// <summary>
        /// 主题列表显示的重复列数.
        /// </summary>
        public static int RepeatColumns = 2;

        /// <summary>
        /// 主题信息字典(key:主题名称).
        /// </summary>
        public static IDictionary<string, ThemeInfo> Themes;

        /// <summary>
        /// 构造方法.
        /// </summary>
        static ThemeContext()
        {
            Themes = new Dictionary<string, ThemeInfo>();
            ThemeSectionHandler.Init();
        }
    }
}
