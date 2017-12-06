using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Web.Controls.ExtendedSiteMap;

namespace Botwave.Web.Controls
{
    /// <summary>
    /// 网站地图路径控件类.
    /// </summary>
    [ToolboxData("<{0}:SiteMapPath runat=server></{0}:SiteMapPath>")]
    public class SiteMapPath : System.Web.UI.WebControls.SiteMapPath
    {
        private Style _mergedCurrentNodeStyle = null;
        private Style _mergedRootNodeStyle = null;
        private IDynamicSitePathHandler dynamicPathHanlder;
        private string currentMappedUrl;            // 当前页面级节点的映射 URL.
        private bool currentPageNodeVisible = true; // 当前页面级节点可见性.

        private int _displaySize = -1;              // 显示路径数
        private string _target;

        #region 属性

        /// <summary>
        /// 显示网站地图路径数目.
        /// </summary>
        [Browsable(true)]
        public int DisplaySize
        {
            get { return _displaySize; }
            set { _displaySize = value; }
        }

        /// <summary>
        /// 网站地图路径链接的链接目标.
        /// </summary>
        [Browsable(true)]
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        #endregion

        #region 重载方法

        /// <summary>
        /// 重写创建子控件.
        /// </summary>
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            this.CreateControlHierarchy();
            base.ClearChildState();
        }

        /// <summary>
        /// 重写数据绑定.
        /// </summary>
        public override void DataBind()
        {
            this.OnDataBinding(EventArgs.Empty);
        }

        /// <summary>
        /// 重写数据绑定时处理.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            this.Controls.Clear();
            base.ClearChildState();
            this.CreateControlHierarchy();
            base.ChildControlsCreated = true;
        }

        /// <summary>
        /// 创建控件层次性架构.
        /// </summary>
        protected override void CreateControlHierarchy()
        {
            this.InitializeExtend();
            if (this.DisplaySize < 2)
            {
                this.CreateControlDefaultHierarchy();
            }
            else
            {
                this.CreateControlAutoSizeHierarchy();
            }
        }

        /// <summary>
        /// 重写，为其添加 Target 属性.
        /// </summary>
        /// <param name="item"></param>
        protected override void InitializeItem(SiteMapNodeItem item)
        {
            ITemplate nodeTemplate = null;
            Style _style = null;
            SiteMapNodeItemType itemType = item.ItemType;
            SiteMapNode siteMapNode = item.SiteMapNode;
            this.InitializeItemTemplates(itemType, out nodeTemplate, out _style);
            if (nodeTemplate == null)
            {
                if (itemType == SiteMapNodeItemType.PathSeparator)
                {
                    item.Controls.Add(this.CreateItemPathSeparator());
                    item.ApplyStyle(_style);
                }
                else if ((itemType == SiteMapNodeItemType.Current) && !this.RenderCurrentNodeAsLink)
                {
                    if (!this.currentPageNodeVisible)
                        return;

                    string url = this.currentMappedUrl;
                    if (string.IsNullOrEmpty(url))
                    {
                        item.Controls.Add(this.CreateItemLiteral(siteMapNode.Title));
                        item.ApplyStyle(_style);
                    }
                    else if (url == "#")
                    {
                        HyperLink link = this.CreateItemLink(siteMapNode.Url, siteMapNode.Title, siteMapNode.Description, this.Target, _style);
                        item.Controls.Add(link);
                        link.ApplyStyle(_style);
                    }
                    else
                    {
                        HyperLink link = this.CreateItemLink(url, siteMapNode.Title, siteMapNode.Description, this.Target, _style);
                        item.Controls.Add(link);
                        link.ApplyStyle(_style);
                    }
                }
                else
                {
                    HyperLink link = this.CreateItemLink(siteMapNode.Url, siteMapNode.Title, siteMapNode.Description, this.Target, _style);
                    item.Controls.Add(link);
                    link.ApplyStyle(_style);
                }
            }
            else
            {
                nodeTemplate.InstantiateIn(item);
                item.ApplyStyle(_style);
            }
        }

        #region 创建控件层次性架构

        /// <summary>
        /// 创建默认的路径分层架构控件.
        /// </summary>
        private void CreateControlDefaultHierarchy()
        {
            base.CreateControlHierarchy();
            IList<PathNode> pathItems = new List<PathNode>();
            this.HandleDynamicPaths(pathItems);
            if (pathItems == null || pathItems.Count == 0)
                return;
            int count = pathItems.Count;
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                if (i > 0 || this.currentPageNodeVisible)
                {
                    this.Controls.Add(CreateItem(index, PathNodeItemType.PathSeparator, null));
                    index++;
                }

                this.Controls.Add(CreateItem(index, PathNodeItemType.Child, pathItems[i]));
                index++;
            }
        }

        /// <summary>
        /// 创建控件自动调整显示路径数分层架构.
        /// </summary>
        private void CreateControlAutoSizeHierarchy()
        {
            IList<PathNode> sourcePathNodes = this.GetPagePathNodes();
            int currentIndex = sourcePathNodes.Count - 1;
            if (!currentPageNodeVisible)
            {
                // 隐藏当前页面级节点.
                sourcePathNodes.RemoveAt(currentIndex); 
                currentIndex = -1;
            }
            this.HandleDynamicPaths(sourcePathNodes);
            int count = sourcePathNodes.Count;
            int index = 0;

            #region
            int minDislayIndex = this.DisplaySize - 1;
            int maxSeparatorIndex = count - 1;

            for (int i = 0; i < count; i++)
            {
                if (i == minDislayIndex && minDislayIndex < maxSeparatorIndex)
                {
                    Label hiddenNodes = new Label();
                    for (; i < maxSeparatorIndex; i++)
                    {
                        PathNodeItemType _itemType = (i == 0 ? PathNodeItemType.Root : PathNodeItemType.Parent);
                        if (i == currentIndex && currentPageNodeVisible)
                            _itemType = PathNodeItemType.Current;
                        else if (i > currentIndex)
                            _itemType = PathNodeItemType.Child;

                        hiddenNodes.Controls.Add(this.CreateItem(index, _itemType, sourcePathNodes[i]));
                        index++;

                        hiddenNodes.Controls.Add(this.CreateItem(index, PathNodeItemType.PathSeparator, null));
                        index++;
                    }

                    hiddenNodes.ID = "hiddenNodes";
                    hiddenNodes.Style["display"] = "none";
                    this.Controls.Add(hiddenNodes);
                    Label displayNodes = new Label();
                    displayNodes.Attributes["id"] = "sitepath_displayNodes";

                    HyperLink link = new HyperLink();
                    link.Text = "...";
                    link.ToolTip = "点击显示隐藏的网站地图路径.";
                    link.NavigateUrl = "javascript:void(0);";
                    link.Attributes["onclick"] = string.Format("document.getElementById('{0}').style.display = 'inline';document.getElementById('sitepath_displayNodes').style.display='none';", hiddenNodes.ClientID);
                    displayNodes.Controls.Add(link);

                    displayNodes.Controls.Add(this.CreateItem(index, PathNodeItemType.PathSeparator, null));
                    this.Controls.Add(displayNodes);

                    index++;
                    i = maxSeparatorIndex;
                }

                PathNodeItemType itemType = (i == 0 ? PathNodeItemType.Root : PathNodeItemType.Parent);
                if (i == currentIndex && currentPageNodeVisible)
                    itemType = PathNodeItemType.Current;
                else if (i > currentIndex)
                    itemType = PathNodeItemType.Child;

                this.Controls.Add(this.CreateItem(index, itemType, sourcePathNodes[i]));
                index++;

                if (i < maxSeparatorIndex)
                {
                    this.Controls.Add(this.CreateItem(index, PathNodeItemType.PathSeparator, null));
                    index++;
                }
            }
            #endregion
        }

        #endregion

        #endregion

        #region 网站地图路径方法

        /// <summary>
        /// 获取页面级的网站地图路径节点.
        /// </summary>
        /// <returns></returns>
        public IList<PathNode> GetPagePathNodes()
        {
            IList<PathNode> pathNodes = new List<PathNode>();
            SiteMapNode _currentNode = this.Provider.CurrentNode;
            while (_currentNode != null)
            {
                pathNodes.Insert(0, new PathNode(_currentNode.Url, _currentNode.Title, _currentNode.Description));
                _currentNode = _currentNode.ParentNode;
            }

            return pathNodes;
        }

        /// <summary>
        /// 创建网站地图路径节点控件.
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="itemType"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual PathNodeItem CreateItem(int itemIndex, PathNodeItemType itemType, PathNode node)
        {
            PathNodeItem item = new PathNodeItem(itemIndex, itemType);
            int index = (this.PathDirection == PathDirection.CurrentToRoot) ? 0 : -1;
            item.PathNode = node;
            this.InitializeItem(item);
            this.Controls.AddAt(index, item);
            item.DataBind();
            item.PathNode = null;
            item.EnableViewState = false;
            return item;
        }

        /// <summary>
        /// 初始化网站地图节点.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void InitializeItem(PathNodeItem item)
        {
            ITemplate nodeTemplate = null;
            Style _style = null;
            PathNodeItemType itemType = item.ItemType;
            PathNode _node = item.PathNode;
            this.InitializeItemTemplates(itemType, out nodeTemplate, out _style);
            if (nodeTemplate == null)
            {
                if (itemType == PathNodeItemType.PathSeparator)
                {
                    item.Controls.Add(this.CreateItemPathSeparator());
                    item.ApplyStyle(_style);
                }
                else if ((itemType == PathNodeItemType.Current) && !this.RenderCurrentNodeAsLink)
                {
                    string url = this.currentMappedUrl;
                    if (string.IsNullOrEmpty(url))
                    {
                        item.Controls.Add(this.CreateItemLiteral(_node.Title));
                        item.ApplyStyle(_style);
                    }
                    else if (url == "#")
                    {
                        HyperLink link = this.CreateItemLink(_node.Url, _node.Title, _node.Description, this.Target, _style);
                        item.Controls.Add(link);
                        link.ApplyStyle(_style);
                    }
                    else
                    {
                        HyperLink link = this.CreateItemLink(url, _node.Title, _node.Description, this.Target, _style);
                        item.Controls.Add(link);
                        link.ApplyStyle(_style);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(_node.Url))
                    {
                        item.Controls.Add(this.CreateItemLiteral(_node.Title));
                        item.ApplyStyle(_style);
                    }
                    else
                    {
                        HyperLink link = this.CreateItemLink(_node.Url, _node.Title, _node.Description, this.Target, _style);
                        item.Controls.Add(link);
                        link.ApplyStyle(_style);
                    }
                }
            }
            else
            {
                nodeTemplate.InstantiateIn(item);
                item.ApplyStyle(_style);
            }
        }

        /// <summary>
        /// 初始化网站地图路径扩展.
        /// </summary>
        protected virtual void InitializeExtend()
        {
            string url = this.Context.Request.Url.AbsolutePath;
            MapPathNode mapNode = SiteMapManager.GetDynamicNode(url);
            if (mapNode == null)
                return;

            if (!string.IsNullOrEmpty(mapNode.Target))
                this._target = mapNode.Target;
            this.currentMappedUrl = mapNode.MappedUrl;
            this.currentPageNodeVisible = mapNode.Visible;
            this.dynamicPathHanlder = mapNode.DynamicHandler;
        }

        private void HandleDynamicPaths(IList<PathNode> rootNodes)
        {
            NameValueCollection parameters = this.Context.Request.QueryString;
            string url = this.Context.Request.Url.AbsolutePath;
            if (this.dynamicPathHanlder != null)
            {
                SiteMapManager.DynamicInvoke(url, parameters, rootNodes, this.dynamicPathHanlder);
            }
        }

        #endregion

        #region 初始化模板

        /// <summary>
        /// 初始化路径项模板以及样式.
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="template"></param>
        /// <param name="itemStyle"></param>
        protected virtual void InitializeItemTemplates(SiteMapNodeItemType itemType, out ITemplate template, out Style itemStyle)
        {
            switch (itemType)
            {
                case SiteMapNodeItemType.Root:
                    template = (this.RootNodeTemplate != null) ? this.RootNodeTemplate : this.NodeTemplate;
                    itemStyle = this._mergedRootNodeStyle;
                    break;

                case SiteMapNodeItemType.Parent:
                    template = this.NodeTemplate;
                    itemStyle = this.NodeStyle;
                    break;

                case SiteMapNodeItemType.Current:
                    template = (this.CurrentNodeTemplate != null) ? this.CurrentNodeTemplate : this.NodeTemplate;
                    itemStyle = this._mergedCurrentNodeStyle;
                    break;

                case SiteMapNodeItemType.PathSeparator:
                    template = this.PathSeparatorTemplate;
                    itemStyle = this.PathSeparatorStyle;
                    break;
                default:
                    template = null;
                    itemStyle = this.NodeStyle;
                    break;
            }
        }

        /// <summary>
        /// 初始化路径项模板以及样式.
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="template"></param>
        /// <param name="itemStyle"></param>
        protected virtual void InitializeItemTemplates(PathNodeItemType itemType, out ITemplate template, out Style itemStyle)
        {
            switch (itemType)
            {
                case PathNodeItemType.Root:
                    template = (this.RootNodeTemplate != null) ? this.RootNodeTemplate : this.NodeTemplate;
                    itemStyle = this._mergedRootNodeStyle;
                    break;

                case PathNodeItemType.Parent:
                    template = this.NodeTemplate;
                    itemStyle = this.NodeStyle;
                    break;

                case PathNodeItemType.Current:
                    template = (this.CurrentNodeTemplate != null) ? this.CurrentNodeTemplate : this.NodeTemplate;
                    itemStyle = this._mergedCurrentNodeStyle;
                    break;

                case PathNodeItemType.PathSeparator:
                    template = this.PathSeparatorTemplate;
                    itemStyle = this.PathSeparatorStyle;
                    break;
                default:
                    template = this.NodeTemplate;
                    itemStyle = this.NodeStyle;
                    break;
            }
        }

        #endregion

        #region 创建子控件

        /// <summary>
        /// 创建路径项超链接控件.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="target"></param>
        /// <param name="linkStyle"></param>
        /// <returns></returns>
        protected virtual HyperLink CreateItemLink(string url, string title, string description, string target, Style linkStyle)
        {
            HyperLink link = new HyperLink();
            if (linkStyle != null)
                link.Font.Underline = linkStyle.Font.Underline;

            link.EnableTheming = false;
            link.Enabled = this.Enabled;

            if (url.StartsWith(@"\\", StringComparison.Ordinal))
                link.NavigateUrl = base.ResolveClientUrl(HttpUtility.UrlPathEncode(url));
            else
                link.NavigateUrl = (this.Context != null) ? this.Context.Response.ApplyAppPathModifier(base.ResolveClientUrl(HttpUtility.UrlPathEncode(url))) : url;

            link.Text = HttpUtility.HtmlEncode(title);
            if (!string.IsNullOrEmpty(this.Target))
                link.Target = this.Target;
            if (this.ShowToolTips && !string.IsNullOrEmpty(description))
            {
                link.ToolTip = description;
            }
            link.ApplyStyle(linkStyle);
            return link;
        }

        /// <summary>
        /// 创建路径项文本控件.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        protected virtual Literal CreateItemLiteral(string title)
        {
            Literal _literal = new Literal();
            _literal.Mode = LiteralMode.Encode;
            _literal.Text = title;
            return _literal;
        }

        /// <summary>
        /// 创建路径分隔符控件.
        /// </summary>
        /// <returns></returns>
        protected virtual Literal CreateItemPathSeparator()
        {
            Literal _separator = new Literal();
            _separator.Mode = LiteralMode.Encode;
            _separator.Text = this.PathSeparator;
            return _separator;
        }

        #endregion

        #region 样式,模板方法

        ///// <summary>
        ///// 创建合并样式.
        ///// </summary>
        //private void CreateMergedStyles()
        //{
        //    this._mergedCurrentNodeStyle = new Style();
        //    this._mergedCurrentNodeStyle.CopyFrom(this.NodeStyle);
        //    this._mergedCurrentNodeStyle.CopyFrom(this.CurrentNodeStyle);
        //    this._mergedRootNodeStyle = new Style();
        //    this._mergedRootNodeStyle.CopyFrom(this.NodeStyle);
        //    this._mergedRootNodeStyle.CopyFrom(this.RootNodeStyle);
        //}

        #endregion
    }
}
