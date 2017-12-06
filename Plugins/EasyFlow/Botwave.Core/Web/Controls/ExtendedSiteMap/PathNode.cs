using System;

namespace Botwave.Web.Controls.ExtendedSiteMap
{
    /// <summary>
    /// 路径节点.
    /// </summary>
    public class PathNode
    {
        #region gets / sets

        private string _url;
        private string _title;
        private string _description;

        /// <summary>
        /// 路径链接地址.
        /// </summary>
        public virtual string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 链接显示标题文本.
        /// </summary>
        public virtual string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 链接描述.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 构造方法.
        /// </summary>
        public PathNode()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="title">链接显示标题文本.</param>
        public PathNode(string title)
            : this(null, title, null)
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="url">路径链接地址.</param>
        /// <param name="title">链接显示标题文本.</param>
        public PathNode(string url, string title)
            : this(url, title, null)
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="url">路径链接地址.</param>
        /// <param name="title">链接显示标题文本.</param>
        /// <param name="description">链接描述.</param>
        public PathNode(string url, string title, string description)
        {
            this._url = url;
            this._title = title;
            this._description = description;
        }

        #endregion
    }
}
