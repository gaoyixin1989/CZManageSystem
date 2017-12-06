using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Web.Themes
{
    /// <summary>
    /// 主题信息.
    /// </summary>
    [Serializable]
    public class ThemeInfo
    {
        private int index;
        private string name;
        private string title;
        private string preview;
        private string header;

        /// <summary>
        /// 索引.
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// 主题名称.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 主题显示文本标题.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 主题的预览图片.
        /// </summary>
        public string Preview
        {
            get { return preview; }
            set { preview = value; }
        }

        /// <summary>
        /// 页面标题类.
        /// </summary>
        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public ThemeInfo()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="preview"></param>
        public ThemeInfo(string name, string title, string preview)
            : this(name, title, preview, string.Empty)
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="preview"></param>
        /// <param name="header"></param>
        public ThemeInfo(string name, string title, string preview, string header)
        {
            this.name = name;
            this.title = title;
            this.preview = preview;
            this.header = header;
        }
    }
}
