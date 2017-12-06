using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

namespace Botwave.Web.Themes
{
    /// <summary>
    /// Theme 主题配置节点类.
    /// </summary>
    public class ThemeSectionHandler : IConfigurationSectionHandler
    {
        #region schema fields

        private const string Section_Theme = "botwave.themes";
        private const string Element_Theme = "theme";
        private const string Element_Add = "add";
        private const string Attribute_Type = "type";
        private const string Attribute_Path = "path";
        private const string Attribute_Name = "name";
        private const string Attribute_Title = "title";
        private const string Attribute_Preview = "preview";
        private const string Attribute_DefaultTheme = "defaultTheme";
        private const string Attribute_RepeateColumns = "repeatColumns";

        #endregion

        #region IConfigurationSectionHandler 成员
        
        /// <summary>
        /// 创建节点.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            this.InitThemes(section);
            return section;
        }

        #endregion

        /// <summary>
        /// 初始化主题配置节点.
        /// </summary>
        public static void Init()
        {
            object section = ConfigurationManager.GetSection(Section_Theme);
            if (section == null)
            {
                ConfigurationManager.GetSection("botwave/themes");
            }
        }

        #region private methods

        /// <summary>
        /// 初始化主题信息.
        /// </summary>
        /// <param name="section"></param>
        private void InitThemes(XmlNode section)
        {
            XmlAttribute attr = section.Attributes[Attribute_DefaultTheme];
            ThemeContext.DefaultTheme = (attr == null ? string.Empty : attr.Value);

            attr = section.Attributes[Attribute_RepeateColumns];
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
            {
                ThemeContext.RepeatColumns = Convert.ToInt32(attr.Value.Trim());
            }

            #region themes

            XmlNodeList nodes = section.SelectNodes(Element_Theme);
            if (nodes == null || nodes.Count == 0)
                return;

            int index = 0;
            foreach (XmlNode themeNode in nodes)
            {
                ThemeInfo item = GetThemeInfo(themeNode);
                if (item != null)
                {
                    item.Index = index;
                    string key = item.Name;
                    if (!ThemeContext.Themes.ContainsKey(key))
                    {
                        ThemeContext.Themes.Add(key, item);
                        index++;
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取主题信息.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private ThemeInfo GetThemeInfo(XmlNode node)
        {
            XmlAttributeCollection attributes = node.Attributes;
            if (attributes.Count < 1)
                return null;
            XmlAttribute attr = attributes[Attribute_Name];
            string name = (attr == null ? string.Empty : attr.Value);
            if (string.IsNullOrEmpty(name))
                return null;

            attr = attributes[Attribute_Title];
            string title = (attr == null ? name : attr.Value);

            attr = attributes[Attribute_Preview];
            string preview = (attr == null ? string.Empty : attr.Value);

            name = name.ToLower();
            return new ThemeInfo(name, title, preview);
        }

        /// <summary>
        /// 获取主题头部内容代码.
        /// </summary>
        /// <param name="themeNode"></param>
        /// <returns></returns>
        private static string GetThemeHeader(XmlNode themeNode)
        {
            if (themeNode.ChildNodes == null || themeNode.ChildNodes.Count == 0)
                return string.Empty;

            // 文件属性节点
            StringBuilder builder = new StringBuilder();
            XmlNodeList properties = themeNode.SelectNodes(Element_Add);
            foreach (XmlNode item in properties)
            {
                if (item.Attributes[Attribute_Type] == null || item.Attributes[Attribute_Path] == null)
                    continue;
                string type = item.Attributes[Attribute_Type].Value;
                string path = item.Attributes[Attribute_Path].Value;
                if (string.IsNullOrEmpty(path))
                    continue;
                path = WebUtils.GetAppPath() + path;
                if (type.Equals("css", StringComparison.OrdinalIgnoreCase))
                {
                    builder.AppendFormat("<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}\" />\r\n", path);
                }
                else if (type.Equals("javascript", StringComparison.OrdinalIgnoreCase))
                {
                    builder.AppendFormat("<script type=\"text/javascript\" src=\"{0}\"></script>\r\n", path);
                }
            }
            return builder.ToString();
        }

        #endregion
    }
}
