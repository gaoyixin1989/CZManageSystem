using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Botwave.Web.Controls.ExtendedSiteMap
{
    /// <summary>
    /// 网站地图文件中路径节点类.
    /// </summary>
    public class MapPathNode
    {
        private string _url;
        private string _mappedUrl;
        private string _dynamicType;
        private bool _visible;
        private string _target;
        private NameValueCollection _properties;
        private IDynamicSitePathHandler _dynamicHandler;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public MapPathNode()
        {
            this._visible = true;
            this._properties = new NameValueCollection();
        }

        /// <summary>
        /// 路径链接地址.
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 获取节点的被映射到的 URL 地址.
        /// </summary>
        public string MappedUrl
        {
            get { return _mappedUrl; }
            set { _mappedUrl = value; }
        }

        /// <summary>
        /// 路径动态实现的类型名称, 用于反射实例化.
        /// </summary>
        public string DynamicType
        {
            get { return _dynamicType; }
            set { _dynamicType = value; }
        }

        /// <summary>
        /// 当前节点的可视性.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// 链接转向目标.
        /// </summary>
        public virtual string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        /// <summary>
        /// 属性集合.
        /// </summary>
        public NameValueCollection Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }

        /// <summary>
        /// 动态调用接口对象.
        /// </summary>
        public IDynamicSitePathHandler DynamicHandler
        {
            get { return _dynamicHandler; }
            set { _dynamicHandler = value; }
        }

        /// <summary>
        /// 节点的属性索引器,只读.
        /// </summary>
        /// <param name="propertyName">属性的名称.</param>
        /// <returns>属性的值.</returns>
        public string this[string propertyName]
        {
            get
            {
                if (string.IsNullOrEmpty(propertyName))
                    return null;
                // 为了使属性索引时, 能忽略属性名称的大小写, 不使用默认的索引方式
                foreach (string key in this.Properties.AllKeys)
                {
                    if (propertyName.Equals(key, StringComparison.OrdinalIgnoreCase))
                        return this.Properties[key];
                }
                return null;
            }
        }
    }
}
