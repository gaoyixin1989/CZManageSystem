using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Botwave.Security.Web.Controls
{
    /// <summary>
    /// 菜单列表项控件类.
    /// </summary>
    [ToolboxData("<{0}:MenuListItem runat=server></{0}:MenuListItem>"),ParseChildren(false)]
    public class MenuListItem : Control
    {
        private NameValueCollection attributes;

        /// <summary>
        /// 属性集合.
        /// </summary>
        public virtual NameValueCollection Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        /// <summary>
        /// 显示文本.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        public string Text
        {
            get { return (string)base.ViewState["Text"]; }
            set { base.ViewState["Text"] = value; }
        }

        /// <summary>
        /// 菜单转向地址.
        /// </summary>
        [Browsable(true), Category("Appearance"), UrlProperty]
        public string NavigateUrl
        {
            get { return (string)base.ViewState["NavigateUrl"]; }
            set { base.ViewState["NavigateUrl"] = value; }
        }

        /// <summary>
        /// 菜单转向目标.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        public string Target
        {
            get { return (string)base.ViewState["Target"]; }
            set { base.ViewState["Target"] = value; }
        }

        /// <summary>
        /// 资源值.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        public string ResourceValue
        {
            get { return (string)base.ViewState["ResourceValue"]; }
            set { base.ViewState["ResourceValue"] = value; }
        }

        /// <summary>
        /// 是否启用主题.
        /// </summary>
        [Browsable(true)]
        public override bool EnableTheming
        {
            get {return base.EnableTheming;}
            set{base.EnableTheming = value; }
        }
    }
}
