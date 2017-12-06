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

namespace Botwave.Security.Web.Controls
{
    /// <summary>
    /// 菜单视图控件.
    /// <example>作为其他列表项菜单控件的基础类.使用方式如:Botwave.XQP.Web.Controls.MoreMenuView
    /// <code>
    /// public class MoreMenuView:MenuView
    /// {
    ///     /* 主要通过重写这个方法来呈现不同样式的菜单控件. */
    ///     public override void RenderControl(HtmlTextWriter writer)
    ///     { }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    [ToolboxData("<{0}:MenuView runat=server></{0}:MenuView>"), ParseChildren(typeof(MenuListItem))]
    [Designer(typeof(MenuViewDesigner))]
    [DesignTimeVisible(false)]
    public class MenuView : Control
    {
        private IDictionary<string, string> userResources = null;

        /// <summary>
        /// 当前用户的权限资源数组.
        /// </summary>
        [Browsable(false)]
        public IDictionary<string, string> UserResources
        {
            get { return userResources; }
            set { userResources = value; }
        }

        /// <summary>
        /// 是否启用主题.
        /// </summary>
        [Browsable(true)]
        public override bool EnableTheming
        {
            get { return base.EnableTheming; }
            set { base.EnableTheming = value; }
        }

        /// <summary>
        /// 是否验证权限资源.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public virtual bool VerifyResource
        {
            get { return (base.ViewState["VerifyResource"] == null ? false : (bool)base.ViewState["VerifyResource"]); }
            set { base.ViewState["VerifyResource"] = value; }
        }

        /// <summary>
        /// 菜单项列表集合.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerDefaultProperty), Browsable(false)]
        public virtual MenuListItemCollection Items
        {
            get
            {
                return (MenuListItemCollection)this.Controls;
            }
        }

        /// <summary>
        /// 创建子控件.
        /// </summary>
        /// <returns></returns>
        protected override ControlCollection CreateControlCollection()
        {
            return new MenuListItemCollection(this);
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.InitilizeRender();
        }

        /// <summary>
        /// 初始化呈现菜单项.
        /// </summary>
        protected virtual void InitilizeRender()
        {
            if (this.VerifyResource)
            {
                if (this.UserResources == null || this.UserResources.Count == 0)
                    this.UserResources = this.GetUserResources(); // 用户资源.

                // 移除无需要资源的菜单项.
                MenuListItemCollection items = this.Items;
                for (int i = 0; i < items.Count; i++)
                {
                    if (!this.HasResource(items[i].ResourceValue))
                    {
                        items.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前用户的访问权限数组.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, string> GetUserResources()
        {
            LoginUser user = LoginHelper.User;
            if (user == null)
                return new Dictionary<string, string>();
            return user.Resources;
        }

        /// <summary>
        /// 检查当前用户是否有指定的菜单访问权限.
        /// </summary>
        /// <param name="menuResourceValue">指定的菜单访问权限.</param>
        /// <returns></returns>
        protected virtual bool HasResource(string menuResourceValue)
        {
            return this.HasResource(this.UserResources, menuResourceValue);
        }

        /// <summary>
        /// 检查当前用户是否有指定的菜单访问权限.
        /// </summary>
        /// <param name="userResources"></param>
        /// <param name="menuResourceValue">指定的菜单访问权限.</param>
        /// <returns></returns>
        protected virtual bool HasResource(IDictionary<string, string> userResources, string menuResourceValue)
        {
            if (string.IsNullOrEmpty(menuResourceValue))
                return true;
            if (userResources == null || userResources.Count == 0)
                return false;
            if (userResources.ContainsKey(menuResourceValue))
                return true;
            return false;
        }
    }

    #region 设计器显示类

    /// <summary>
    /// MenuView 的设计器类.
    /// </summary>
    public class MenuViewDesigner : System.Web.UI.Design.ControlDesigner
    {
        private MenuView viewControl;

        /// <summary>
        /// 初始化设计器.
        /// </summary>
        /// <param name="component"></param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            viewControl = component as MenuView;
        }

        /// <summary>
        /// 获取设计时的 html 字符串.
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml()
        {
            if (viewControl != null)
                return string.Format("<font color='green'>MenuView : {0}</font>", viewControl.ID);
            else
                return "<font color='red'>MenuView</font>";
        }
    }

    #endregion
}
