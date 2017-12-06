using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;

public partial class contrib_security_pages_Roles : Botwave.Security.Web.PageBase
{
    private IRoleService roleService=(IRoleService)Ctx.GetObject("roleService");

    public IRoleService RoleService
    {
        get { return roleService; }
        set { roleService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.btnBindGroups.Style["display"] = "none";
            this.btnBindGroups.Style["height"] = "0";
            this.InitBind();
        }
    }

    private void InitBind()
    {
        Guid empty = Guid.Empty;
        IList<RoleInfo> roles = roleService.GetRolesByParentId(empty); // 角色组数据

        // 绑定角色组

        //this.rptGroups.DataSource = roles;
        //this.rptGroups.DataBind();

        // 角色组下拉列表

        this.ddlParentRoles.DataSource = roles;
        this.ddlParentRoles.DataTextField = "RoleName";
        this.ddlParentRoles.DataValueField = "RoleId";
        this.ddlParentRoles.DataBind();
        this.ddlParentRoles.Items.Insert(0, new ListItem("－ 全部 －", ""));

        // 绑定全部非父角色
        this.BindAllRoles();
    }

    private void BindAllRoles()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["Pid"]))
        {
            ddlParentRoles.Items.FindByValue(Request.QueryString["Pid"]).Selected = true;
            BindChildRoles(new Guid(Request.QueryString["Pid"]));
        }
        else
        {
            this.repeaterRoles.DataSource = roleService.GetRolesByEnabled();
            this.repeaterRoles.DataBind();
        }
    }

    private void BindChildRoles(Guid parentId)
    {
        this.repeaterRoles.DataSource = roleService.GetRolesByParentId(parentId);
        this.repeaterRoles.DataBind();
    }

    protected void ddlParentRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedValue = ddlParentRoles.SelectedValue;
        if (string.IsNullOrEmpty(selectedValue))
        {
            this.BindAllRoles();
        }
        else
        {
            this.BindChildRoles(new Guid(selectedValue));
        }
    }

    // 角色数据行的创建.
    protected void repeaterRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }

    protected void btnBindGroups_Click(object sender, EventArgs e)
    {
        IList<RoleInfo> roles = roleService.GetRolesByParentId(Guid.Empty); // 角色组数据

        // 绑定角色组

        this.rptGroups.DataSource = roles;
        this.rptGroups.DataBind();
    }
}
