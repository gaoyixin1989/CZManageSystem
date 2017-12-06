using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Xml;

namespace Botwave.FileManager
{
    /// <summary>
    /// 文件管理配置节点处理类.
    /// </summary>
    public class FileManagerSectionHandler : ConfigurationElement, IConfigurationSectionHandler
    {
        #region fields

        /// <summary>
        /// 文档上传服务对象类型.
        /// </summary>
        internal static Type FileServiceType = null;

        /// <summary>
        /// 配置属性集合.
        /// </summary>
        internal static NameValueCollection CurrentProperties = new NameValueCollection(StringComparer.CurrentCultureIgnoreCase);
        
        #endregion

        #region IConfigurationSectionHandler 成员

        /// <summary>
        /// 创建配置节点.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            BindConfig(section);
            return section;
        }

        #endregion

        /// <summary>
        /// 初始化配置节点对象.
        /// </summary>
        public static void Initialize()
        {
            object section = ConfigurationManager.GetSection("fileManager");
            if (section == null)
                section = ConfigurationManager.GetSection("botwave/fileManager");
            if (section == null)
                section = ConfigurationManager.GetSection("botwave.fileManager");
        }

        /// <summary>
        /// 绑定配置.
        /// </summary>
        /// <param name="section"></param>
        protected static void BindConfig(XmlNode section)
        {
            XmlAttribute attribute = null;
            // 先设置属性
            XmlNodeList childNodes = section.ChildNodes;
            foreach (XmlNode child in childNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    attribute = child.Attributes["name"];
                    if (attribute == null)
                        attribute = child.Attributes["key"];
                    string name = (attribute == null ? child.Name : attribute.InnerText);
                    attribute = child.Attributes["value"];
                    string value = (attribute == null ? child.InnerText : attribute.Value);
                    CurrentProperties[name] = value;
                }
            }
            // 设置文件上传对象实例
            attribute = section.Attributes["type"];
            if (attribute != null)
            {
                Type t = Type.GetType(attribute.InnerText.Trim());
               FileServiceType = t;
            }
        }
    }
}
