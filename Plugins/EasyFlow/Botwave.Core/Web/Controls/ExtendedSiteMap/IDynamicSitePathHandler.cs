using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Botwave.Web.Controls.ExtendedSiteMap
{
    /// <summary>
    /// 动态网站地图节点处理器接口.
    /// </summary>
    public interface IDynamicSitePathHandler
    {
        /// <summary>
        /// 处理动态路径节点.
        /// </summary>
        /// <param name="url">当前请求的 URL 地址.</param>
        /// <param name="parameters">查询字符串的参数数组.</param>
        /// <param name="pathNodes">已有网站路径节点集合.</param>
        void Handle(string url, NameValueCollection parameters, IList<PathNode> pathNodes);
    }
}
