using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Botwave.Web.Controls.ExtendedSiteMap
{
    /// <summary>
    /// 动态网站地图路径节点处理的测试实现类.
    /// </summary>
    public class TestDynamicSitePathHandler : IDynamicSitePathHandler
    {
        #region IDynamicSitePathHandler 成员

        /// <summary>
        /// 处理网站地图路径节点.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="pathNodes"></param>
        public void Handle(string url, NameValueCollection parameters, IList<PathNode> pathNodes)
        {
            pathNodes.Add(new PathNode("#asp", "asp"));
            pathNodes.Add(new PathNode("", ".net"));
        }

        #endregion
    }
}