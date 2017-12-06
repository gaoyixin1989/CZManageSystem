using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Security;
using Botwave.Security.Web.Controls;

namespace Botwave.XQP.Web.Controls
{
    /// <summary>
    /// 访问控制器控件.
    /// <example>设置某一块代码区域的访问控制，设控件 TagPrefix 为 bw.
    /// <code>
    /// <![CDATA[
    /// <%@ Register TagPrefix="bw" Namespace="Botwave.Security.Extension.Web.Controls" Assembly="Botwave.Security.Extension" %>
    /// 
    /// <bw:AccessController ID="accessController1" ResourceValue="A0001" runat="server">
    ///     <ContentTemplate>
    ///         <asp:Button ID="button1" runat="server" Text="Button" />
    ///         这时被限制访问的按钮, 需要具有 ResourceValue 属性指定的权限资源时(即"A0001")才可以访问该按钮.
    ///     </ContentTemplate>
    /// </bw:AccessController>
    /// ]]>
    /// </code>
    /// </example>
    /// </summary>
    [ToolboxData("<{0}:AccessController runat=server></{0}:AccessController>"), PersistChildren(false), ParseChildren(true)]
    public class AccessController : Control
    {
        private IDictionary<string, string> _userResources = null;
        private ITemplate _contentTemplate = null;

        /// <summary>
        /// 重写创建子控件.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (!string.IsNullOrEmpty(this.ResourceValue) && this._userResources == null)
                this._userResources = LoginHelper.User.Resources;

            if (_contentTemplate != null && this.HasResource(this.UserResources, this.ResourceValue))
            {
                Control container = new Control();
                _contentTemplate.InstantiateIn(container);
                this.Controls.Add(container);
            }
        }

        /// <summary>
        /// 访问的资源值.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        public string ResourceValue
        {
            get { return (string)base.ViewState["ResourceValue"]; }
            set { base.ViewState["ResourceValue"] = value; }
        }

        /// <summary>
        /// 内容模板.
        /// </summary>
        [Browsable(false), DefaultValue((string)null), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(RepeaterItem))]
        public ITemplate ContentTemplate
        {
            get { return _contentTemplate; }
            set { _contentTemplate = value; }
        }

        /// <summary>
        /// 是否验证权限资源.
        /// </summary>
        [Browsable(false)]
        public virtual IDictionary<string, string> UserResources
        {
            get { return _userResources; }
            set { _userResources = value; }
        }

        /// <summary>
        /// 指定是否具有资源.
        /// </summary>
        /// <param name="userResources"></param>
        /// <returns></returns>
        public bool HasResource(IDictionary<string, string> userResources)
        {
            return this.HasResource(userResources, this.ResourceValue);
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
            string[] resources = menuResourceValue.Split(',', '，');
            foreach (string item in resources)
            {
                if (userResources.ContainsKey(item.Trim()))
                    return true;
            }
            return false;
        }
    }
}
