using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Botwave.Web.Controls.ExtendedSiteMap
{
    /// <summary>
    /// 路径节点项控件.
    /// </summary>
    [ToolboxItem(false)]
    public class PathNodeItem : WebControl, IDataItemContainer, INamingContainer
    {
        private int _itemIndex;
        private PathNodeItemType _itemType;
        private PathNode _pathNode;

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="itemType"></param>
        public PathNodeItem(PathNodeItemType itemType)
        {
            this._itemType = itemType;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="itemType"></param>
        public PathNodeItem(int itemIndex, PathNodeItemType itemType)
        {
            this._itemIndex = itemIndex;
            this._itemType = itemType;
        }

        /// <summary>
        /// 项索引.
        /// </summary>
        public int ItemIndex
        {
            get { return _itemIndex; }
        }

        /// <summary>
        /// 节点项类型.
        /// </summary>
        public PathNodeItemType ItemType
        {
            get { return _itemType; }
        }

        /// <summary>
        /// 节点对象.
        /// </summary>
        public PathNode PathNode
        {
            get { return _pathNode; }
            set { _pathNode = value; }
        }

        #region IDataItemContainer Members

        object IDataItemContainer.DataItem
        {
            get
            {
                return this.PathNode;
            }
        }

        /// <summary>
        /// 数据项索引.
        /// </summary>
        public int DataItemIndex
        {
            get { return this.ItemIndex; }
        }

        /// <summary>
        /// 显示索引.
        /// </summary>
        public int DisplayIndex
        {
            get { return this.ItemIndex; }
        }

        #endregion

        #region override

        /// <summary>
        /// 重写数据绑定时方法.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
        }

        #endregion
    }

    /// <summary>
    /// 网站路径节点类型.
    /// </summary>
    public enum PathNodeItemType
    {
        /// <summary>
        /// 根节点.
        /// </summary>
        Root = 0,
        /// <summary>
        /// 父节点.
        /// </summary>
        Parent = 1,
        /// <summary>
        /// 当前节点.
        /// </summary>
        Current = 2,
        /// <summary>
        /// 路径分割点.
        /// </summary>
        PathSeparator = 3,
        /// <summary>
        /// 子节点.
        /// </summary>
        Child = 4
    }
}
