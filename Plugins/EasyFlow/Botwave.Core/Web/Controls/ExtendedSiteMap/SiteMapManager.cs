using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Botwave.Web.Controls.ExtendedSiteMap
{
    /// <summary>
    /// 网站地图管理类.
    /// </summary>
    public sealed class SiteMapManager
    {
        /// <summary>
        /// 网站地图节点名称.
        /// </summary>
        private const string siteMapNodeName = "siteMapNode";
        /// <summary>
        ///  HTTP 上下文.
        /// </summary>
        private static HttpContext context; 
        /// <summary>
        /// 网站地图文件路径.
        /// </summary>
        private static string siteMapFile = "~/web.sitemap"; 

        /// <summary>
        /// 网站地图动态节点处理器缓存.
        /// </summary>
        public static Hashtable dynamicCache; 

        /// <summary>
        /// 静态构造方法.
        /// </summary>
        static SiteMapManager()
        {
            dynamicCache = new Hashtable();
            context = HttpContext.Current;
            siteMapFile = context.Server.MapPath(siteMapFile);
            GetMapPathNodes();
        }

        #region 网站地图路径

        /// <summary>
        /// 获取网站地图路径节点集合列表.
        /// </summary>
        /// <returns></returns>
        public static IList<MapPathNode> GetMapPathNodes()
        {
            IList<MapPathNode> pathItems = new List<MapPathNode>();
            using (XmlReader reader = XmlReader.Create(siteMapFile))
            {
                if (reader == null)
                    return pathItems;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case siteMapNodeName:
                                MapPathNode item = GetMapPathNode(reader);
                                if (item == null || string.IsNullOrEmpty(item.Url))
                                    break;
                                pathItems.Add(item);
                                break;
                            default:
                                break;
                        }
                        reader.MoveToElement();
                    }
                }
            }
            return pathItems;
        }

        /// <summary>
        /// 获取网站地图路径扩展项.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static MapPathNode GetMapPathNode(XmlReader reader)
        {
            MapPathNode item = new MapPathNode();
            int count = reader.AttributeCount;
            for (int i = 0; i < count; i++)
            {
                reader.MoveToAttribute(i);
                switch (reader.Name)
                {
                    case "url":
                        string url = reader.Value;
                        if (string.IsNullOrEmpty(url))
                            return null;
                        item.Url = url.ToLower();
                        break;
                    case "mappedUrl":
                        url = reader.Value;
                        item.MappedUrl = url.ToLower();
                        break;
                    case "target":
                        item.Target = reader.Value;
                        break;
                    case "visible":
                        string visibleValue = reader.Value.ToLower();
                        if (visibleValue == "false" || visibleValue == "0")
                            item.Visible = false;
                        break;
                    case "dynamicType":
                    case"dynamic":
                        if (string.IsNullOrEmpty(reader.Value))
                            return null;
                        item.DynamicType = reader.Value;
                        break;
                    default:
                        item.Properties.Add(reader.Name, reader.Value);
                        break;
                }
            }

            CacheDynamicNode(item);
            return item;
        }

        /// <summary>
        /// 调用获取网站路径项集合列表.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="nodes"></param>
        /// <param name="invoker"></param>
        internal static void DynamicInvoke(string url, NameValueCollection parameters, IList<PathNode> nodes, IDynamicSitePathHandler invoker)
        {
            if (invoker != null)
            {
                invoker.Handle(url, parameters, nodes);
            }
        }

        #endregion

        #region 缓存

        /// <summary>
        /// URL 特殊字符替换正则表达式.
        /// </summary>
        private static Regex UrlReplaceRegex = new Regex("[~:?&#+%.]+", RegexOptions.Compiled);

        /// <summary>
        /// 缓存动态节点对象实例.
        /// </summary>
        /// <param name="item"></param>
        private static void CacheDynamicNode(MapPathNode item)
        {
            string url = item.Url;
            string dynamicTypeName = item.DynamicType;
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(dynamicTypeName))
                return;

            IDynamicSitePathHandler invoker = GetDynaimicHandler(dynamicTypeName);
            if (invoker != null && !dynamicCache.ContainsKey(url))
            {
                item.DynamicHandler = invoker;
                dynamicCache.Add(url, item);
            }
        }

        /// <summary>
        /// 获取指定 URL 的动态节点对象实例.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static MapPathNode GetDynamicNode(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            string applicationPath = context.Request.ApplicationPath;
            if (url.StartsWith(applicationPath))
                url = url.Remove(0, applicationPath.Length);
            if (url.StartsWith("/"))
                url = url.Remove(0, 1);
            url = url.ToLower();
            if (dynamicCache.ContainsKey(url))
                return dynamicCache[url] as MapPathNode;
            return null;
        }

        /// <summary>
        /// 获取指定动态类型名的动态调用接口实例.
        /// </summary>
        /// <param name="dynamicTypeName"></param>
        /// <returns></returns>
        private static IDynamicSitePathHandler GetDynaimicHandler(string dynamicTypeName)
        {
            Type dynamicType = Type.GetType(dynamicTypeName);
            if (dynamicType == null)
                return null;
            if (typeof(IDynamicSitePathHandler).IsAssignableFrom(dynamicType))
            {
                IDynamicSitePathHandler invoker = Activator.CreateInstance(dynamicType) as IDynamicSitePathHandler;
                if (invoker != null)
                    return invoker;
            }
            return null;
        }

        #endregion
    }
}
