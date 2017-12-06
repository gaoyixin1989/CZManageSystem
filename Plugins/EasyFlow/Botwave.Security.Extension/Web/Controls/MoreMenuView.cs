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
    /// 顶部菜单栏"更多"菜单视图控件.
    /// <example>菜单访问控制控件，设控件 TagPrefix 为 bw.
    /// <code>
    /// <![CDATA[
    /// <%@ Register TagPrefix="bw" Namespace="Botwave.XQP.Web.Controls" Assembly="Botwave.XQP" %>
    /// 
    /// <bw:MoreMenuView ID="moreMenu1" VerifyResource="true" runat="server">
    ///     <bw:MenuListItem ID="MenuListItem0" runat="server" Text="用户管理" NavigateUrl="Admin/Membership/ListUser.aspx" ResourceValue="A001" />
    ///     <bw:MenuListItem ID="MenuListItem1" runat="server" Text="流程设计" NavigateUrl="Workflow/WorkflowDeploy.aspx" ResourceValue="A004"/>
    ///     <bw:MenuListItem ID="MenuListItem2" runat="server" Text="权限管理" NavigateUrl="Admin/Membership/ListRole.aspx" ResourceValue="A002" />
    /// </bw:MoreMenuView>
    /// ]]>
    /// </code>
    /// </example>
    /// </summary>
    [ToolboxData("<{0}:MoreMenuView runat=server></{0}:MoreMenuView>"), ParseChildren(typeof(MenuListItem))]
    [Designer(typeof(MoreMenuViewDesigner))]
    [DesignTimeVisible(false)]
    public class MoreMenuView : MenuView
    {
        #region properties

        private string popupBackImage = "res/img/menu_popup_bg.gif";
        private string popupItemImage = "res/img/menu_popup_normal.gif";
        private string popupItemHoverImage = "res/img/menu_popup_hover.gif";

        /// <summary>
        /// 弹出框的背景图片.
        /// </summary>
        [UrlProperty, Category("Appearance")]
        public string PopupBackImage
        {
            get { return popupBackImage; }
            set { popupBackImage = value; }
        }

        /// <summary>
        /// 菜单项的图片.
        /// </summary>
        [UrlProperty, Category("Appearance")]
        public string PopupItemImage
        {
            get { return popupItemImage; }
            set { popupItemImage = value; }
        }

        /// <summary>
        /// 菜单项的悬浮图片.
        /// </summary>
        [UrlProperty, Category("Appearance")]
        public string PopupItemHoverImage
        {
            get { return popupItemHoverImage; }
            set { popupItemHoverImage = value; }
        }

        #endregion

        /// <summary>
        /// 构造方法.
        /// </summary>
        public MoreMenuView()
        { }

        /// <summary>
        /// 重写呈现控件.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            string appPath = WebUtils.GetAppPath();
            string backImage = appPath + popupBackImage;
            string itemImage = appPath + popupItemImage;
            string itemHoverImage = appPath + popupItemHoverImage;

            MenuListItemCollection items = this.Items;
            int count = items.Count;
            if (count > 0)
            {
                writer.AddAttribute("id", "divMoreMenu");
                writer.AddStyleAttribute("display", "none");
                writer.RenderBeginTag("div"); // <div id="divMoreMenu" style="display: none;">

                writer.AddAttribute("class", "main_menu_popup");
                writer.AddStyleAttribute("padding", "6px");
                writer.AddStyleAttribute("border", "1px solid #9B9B9B");
                writer.AddStyleAttribute("background", string.Format("#F9F9F9 url({0}) repeat-x bottom", backImage));
                writer.RenderBeginTag("div"); //<div class="main_menu_popup" style="border: 1px solid #9B9B9B; padding: 6px; background: #F9F9F9 url(images/menu_popup_bg.gif) repeat-x bottom;">

                writer.AddAttribute("width", "124");
                writer.AddStyleAttribute("margin", "0");
                writer.AddStyleAttribute("padding", "0");
                writer.AddStyleAttribute("line-height", "22px");
                writer.AddStyleAttribute("cursor", "pointer");
                writer.AddStyleAttribute("font-size", "12px");
                writer.AddStyleAttribute("border-collapse", "collapse");
                writer.RenderBeginTag("table"); // <table width="124" style="margin: 0; padding: 0; line-height: 22px; cursor: pointer; font-size: 12px; border-collapse: collapse;">

                // 输出菜单列表.
                for (int i = 0; i < count; i++)
                {
                    Render(writer, items[i], itemImage, itemHoverImage);
                }

                writer.RenderEndTag(); // table

                writer.RenderEndTag(); // div()

                writer.RenderEndTag(); // div(divMoreMenu)

                this.RenderScript(writer); // 输出脚本
            }
        }

        /// <summary>
        /// 输出菜单列表中一项.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="item"></param>
        /// <param name="itemBackImage"></param>
        /// <param name="itemHoverImage"></param>
        private static void Render(HtmlTextWriter writer, MenuListItem item, string itemBackImage, string itemHoverImage)
        {

            string url = WebUtils.GetAppPath() + item.NavigateUrl;
            writer.RenderBeginTag("tr");

            writer.AddAttribute("background", itemBackImage);
            writer.AddAttribute("onmouseover", "background='" + itemHoverImage + "';");
            writer.AddAttribute("onmouseout", "background='" + itemBackImage + "';");
            writer.AddAttribute("onclick", "top.rightFrame.location = '" + url + "';");
            writer.RenderBeginTag("td");

            writer.AddStyleAttribute("padding-left", "26px");
            writer.RenderBeginTag("span");
            writer.Write(item.Text);
            writer.RenderEndTag();

            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        /// <summary>
        /// 输出菜单脚本.
        /// </summary>
        /// <param name="writer"></param>
        private void RenderScript(HtmlTextWriter writer)
        {
            writer.Write("\r\n");
            writer.AddAttribute("type", "text/javascript");
            writer.AddAttribute("language", "javascript");
            writer.RenderBeginTag("script");

            writer.Write("<!--//\r\n");
            writer.Write(GetScript(this.GetPopupHeight()));
            writer.Write("//-->");

            writer.RenderEndTag();
        }

        /// <summary>
        /// 获取脚本.
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string GetScript(int height)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine("window.onload = function(){");
            scriptBuilder.AppendLine("  if(top != parent){");
            scriptBuilder.AppendLine("      top.location = parent.location;");
            scriptBuilder.AppendLine("  }");
            scriptBuilder.AppendLine("  document.getElementById(\"getmore\").onclick = function(){");
            scriptBuilder.AppendLine("      showMenu(\"divMoreMenu\", document.getElementById(\"getmore\"));");
            scriptBuilder.AppendLine("  }");
            scriptBuilder.AppendLine("}");

            scriptBuilder.AppendLine("var oPopup = window.createPopup();");
            scriptBuilder.AppendLine("function showMenu(theId, reference){");
            scriptBuilder.AppendLine("  var menu = document.getElementById(theId);");
            scriptBuilder.AppendLine("  var t= reference.offsetTop + 58;");
            scriptBuilder.AppendLine("  var l= reference.offsetLeft;");
            scriptBuilder.AppendLine("  while (reference = reference.offsetParent){");
            scriptBuilder.AppendLine("      t += reference.offsetTop;");
            scriptBuilder.AppendLine("      l += reference.offsetLeft;");
            scriptBuilder.AppendLine("  }");
            scriptBuilder.AppendLine("  // Popup对象和Window对象一样，里面包含了一个完整的HTML文档，在Body中插入HTML");
            scriptBuilder.AppendLine("  oPopup.document.body.innerHTML = menu.innerHTML; ");
            scriptBuilder.AppendLine("  // 设置显示的位置、大小、参照物");
            scriptBuilder.AppendLine("  // left, top, width, height: 38 + 25*(n-1)");
            scriptBuilder.AppendLine("  oPopup.show(l, t, 136, " + height + ", document.body);");
            scriptBuilder.AppendLine("}");

            return scriptBuilder.ToString();
        }

        /* javascript:

    window.onload = function(){
	    if (top != parent){
	        top.location = parent.location;
	    }
	    
		document.getElementById("getmore").onclick = function(){
			showMenu("divMoreMenu", document.getElementById("getmore"));
		}
	}

	var oPopup = window.createPopup();
	function showMenu(theId, reference){
		var menu = document.getElementById(theId);
		
		var t= reference.offsetTop + 58;
		var l= reference.offsetLeft;
		while (reference = reference.offsetParent){
			t += reference.offsetTop;
			l += reference.offsetLeft;
		}
		// Popup对象和Window对象一样，里面包含了一个完整的HTML文档，在Body中插入HTML
		oPopup.document.body.innerHTML = menu.innerHTML; 

		// 设置显示的位置、大小、参照物
		// left, top, width, height: 38 + 25*(n-1)
		oPopup.show(l, t, 136, '<%=this.moreMenu1.GetPopupHeight() %>', document.body);
	}     
         */

        /// <summary>
        /// 获取弹出对话框的高度.
        /// </summary>
        /// <returns></returns>
        public int GetPopupHeight()
        {
            return (38 + (this.Items.Count - 1) * 25);
        }
    }

    #region 设计器显示类

    /// <summary>
    /// MoreMenuView 的设计器类.
    /// </summary>
    public class MoreMenuViewDesigner : System.Web.UI.Design.ControlDesigner
    {
        private MoreMenuView viewControl;

        /// <summary>
        /// 初始化.
        /// </summary>
        /// <param name="component"></param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            viewControl = component as MoreMenuView;
        }

        /// <summary>
        /// 获取设计时的 html 字符串.
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml()
        {
            if (viewControl != null)
                return string.Format("<font color='green'>MoreMenuView : {0}</font>", viewControl.ID);
            else
                return "<font color='red'>MoreMenuView</font>";
        }
    }

    #endregion
}
