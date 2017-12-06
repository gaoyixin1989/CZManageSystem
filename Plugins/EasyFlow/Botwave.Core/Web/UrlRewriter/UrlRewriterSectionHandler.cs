using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Xml;

namespace Botwave.Web.UrlRewriter
{
    /// <summary>
    /// URL 重写配置处理类.
    /// </summary>
    public class UrlRewriterSectionHandler : ConfigurationElement, IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler 成员

        /// <summary>
        /// 创建配置节点对象.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            this.InitSection(section);
            return section;
        }

        #endregion

        /// <summary>
        /// 初始化 UrlRewriterSectionHandler 对象.
        /// </summary>
        public static void Init()
        {
            object section = ConfigurationManager.GetSection(RewriterShema.Section_UrlRewriter);
            if (section == null)
            {
                ConfigurationManager.GetSection("botwave.urlRewriter");
            }
        }

        #region 获取重写 URL 列表

        /// <summary>
        /// 初始化节点数据.
        /// </summary>
        /// <param name="section"></param>
        private void InitSection(XmlNode section)
        {
            // 是否启用 URL 重写.
            XmlAttribute attr = section.Attributes[RewriterShema.Attribute_Enable];
            if (attr != null)
            {
                UrlRewriterContext.EnableRewriter = (attr.Value.Equals("false", StringComparison.OrdinalIgnoreCase)) ? false : true;
            }
            // 禁用 URL 重写.
            if (UrlRewriterContext.EnableRewriter == false)
                return;

            #region rewriters

            XmlNodeList nodes = section.SelectNodes(RewriterShema.Element_Rewriter);
            if (nodes == null || nodes.Count == 0)
                return;

            foreach (XmlNode rewriterNode in nodes)
            {
                // 获取重写规则.
                RewriterProperty item = GetRewriterProperty(rewriterNode);
                if (item != null)
                    UrlRewriterContext.RewriterProperties.Add(item);

                // 重写路径的过滤名称(如：".aspx").
                XmlAttribute pathAttribute = rewriterNode.Attributes[RewriterShema.Attribute_Path];
                if (pathAttribute != null && !string.IsNullOrEmpty(pathAttribute.Value))
                {
                    string path = pathAttribute.Value.Trim().ToLower();
                    if (!UrlRewriterContext.PathFilters.Contains(path))
                        UrlRewriterContext.PathFilters.Add(path);
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取 RewriterProperty 对象.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static RewriterProperty GetRewriterProperty(XmlNode node)
        {
            XmlAttributeCollection attributes = node.Attributes;
            int count = attributes.Count;

            if (count == 0 || attributes[RewriterShema.Attribute_Pattern] == null
                || attributes[RewriterShema.Attribute_Url] == null)
                return null;

            string pattern = attributes[RewriterShema.Attribute_Pattern].Value;
            string url = attributes[RewriterShema.Attribute_Url].Value;
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(url))
                return null;

            // 移除开头的"/"
            if (url.StartsWith("~/"))
                url = url.Remove(0, 2);
            else if (url.StartsWith("/"))
                url = url.Remove(0, 1);

            // 加上应用程序根路径.
            if (pattern.StartsWith("~/"))
                pattern = pattern.Remove(0, 1);
            else
                pattern = (pattern.StartsWith("/") ? string.Empty : "/") + pattern;
            pattern = (UrlRewriterContext.WebApplicationPath == "/" ? string.Empty : UrlRewriterContext.WebApplicationPath) + pattern;
            
            return new RewriterProperty(pattern, url);
        }
        
        #endregion

        /// <summary>
        /// URL 重写架构类.
        /// </summary>
        internal static class RewriterShema
        {
            /// <summary>
            /// URL 重写配置节点名称.
            /// </summary>
            internal const string Section_UrlRewriter = "botwave/urlRewriter";

            /// <summary>
            /// Rewriter 节点名称.
            /// </summary>
            internal const string Element_Rewriter = "rewriter";

            /// <summary>
            /// Enable 属性名称.
            /// </summary>
            internal const string Attribute_Enable = "enable";

            /// <summary>
            /// Path 属性名称.
            /// </summary>
            internal const string Attribute_Path = "path";

            /// <summary>
            /// URL 属性名称.
            /// </summary>
            internal const string Attribute_Url = "url";

            /// <summary>
            /// Pattern 属性名称.
            /// </summary>
            internal const string Attribute_Pattern = "pattern";
        }
    }
}
