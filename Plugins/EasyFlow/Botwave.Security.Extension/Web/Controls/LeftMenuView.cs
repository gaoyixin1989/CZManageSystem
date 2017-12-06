using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Web;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Web.Controls;

namespace Botwave.Security.Extension.Web.Controls
{
    /// <summary>
    /// 左侧菜单列表项.
    /// </summary>   
    [Designer(typeof(MoreMenuViewDesigner)), DesignTimeVisible(false)]
    public class LeftMenuView : MenuView
    {
        private string title = "菜单标题";
        private string target = "rightFrame";

        /// <summary>
        /// 左侧菜单的标题.
        /// </summary>
        [Category("Appearance")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 菜单项的链接目标.
        /// </summary>
        [Category("Appearance")]
        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <summary>
        /// 重写呈现控件的方法.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderControl(HtmlTextWriter writer)
        {            
            MenuListItemCollection items = this.Items;            
            int count = items.Count;
            if (count > 0)
            {
                writer.RenderBeginTag("li");

                writer.AddAttribute("title", this.Title);
                writer.RenderBeginTag("h4");
                writer.Write(this.Title);
                writer.RenderEndTag();

                writer.AddAttribute("class", "menuContent");
                writer.RenderBeginTag("div");

                // 输出菜单列表.
                for (int i = 0; i < count; i++)
                {
                    RenderItem(writer, items[i], this.Target);
                }

                writer.RenderEndTag();

                writer.RenderEndTag();
            }
        }

        private static void RenderItem(HtmlTextWriter writer, MenuListItem item, string target)
        {
            writer.AddAttribute("href", item.NavigateUrl);
            if (!string.IsNullOrEmpty(item.Target))
                target = item.Target;
            if (!string.IsNullOrEmpty(target))
                writer.AddAttribute("target", target);
            writer.AddAttribute("title", item.Text);

            writer.RenderBeginTag("a");
            writer.Write(item.Text);
            writer.RenderEndTag();
        }
    }

    #region 设计器显示类

    /// <summary>
    /// LeftMenuView 的设计器类.
    /// </summary>
    public class LeftMenuViewDesigner : System.Web.UI.Design.ControlDesigner
    {
        private LeftMenuView viewControl;

        /// <summary>
        /// 初始化.
        /// </summary>
        /// <param name="component"></param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            viewControl = component as LeftMenuView;
        }

        /// <summary>
        /// 获取设计时的 html 字符串.
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml()
        {
            if (viewControl != null)
                return string.Format("<font color='green'>LeftMenuView : {0}</font>", viewControl.ID);
            else
                return "<font color='red'>LeftMenuView</font>";
        }
    }

    #endregion
}
