using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Web.Controls;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;

public partial class contrib_security_pages_DeptRoles : Botwave.Web.PageBase
{
    protected IUserService userService = (IUserService)Ctx.GetObject("userService");
    protected IRoleService roleService = (IRoleService)Ctx.GetObject("roleService");
    protected IDepartmentService departmentService = (IDepartmentService)Ctx.GetObject("departmentService");

    public IUserService UserService
    {
        set { userService = value; }
    }

    public IRoleService RoleService
    {
        set { roleService = value; }
    }

    public IDepartmentService DepartmentService
    {
        set { departmentService = value; }
    }

    #region 部门数据树结构和用户列表

    protected void treeDepts_SelectedNodeChanged(object sender, EventArgs e)
    {
        BindUsers();
        TreeNode current = this.treeDepts.SelectedNode;
        string fullName = string.Empty;
        while (current != null)
        {
            if (!string.IsNullOrEmpty(fullName))
                fullName = " &gt; " + fullName;
            fullName = current.Text + fullName;
            current = current.Parent;
        }
        if (string.IsNullOrEmpty(fullName))
        {
            ltlDpFullName.Text = "<font color=\"red\">未选择部门.</font>";
        }
        else
        {
            ltlDpFullName.Text = string.Format("<font color=\"green\">{0}</font>", fullName);
        }
    }

    protected void BindRoles()
    {
        this.ddlRoles.DataSource = roleService.GetRolesByEnabled();
        this.ddlRoles.DataTextField = "RoleName";
        this.ddlRoles.DataValueField = "RoleId";
        this.ddlRoles.DataBind();
        this.ddlRoles.Items.Insert(0, new ListItem("－－ 请选择角色 －－", ""));
    }

    protected void BindUsers()
    {
        bool isContainsLower = chkboxContainsLower.Checked;
        string dpId = treeDepts.SelectedNode.Value;
        IList<UserInfo> users = null;
        if (isContainsLower)
            users = userService.GetUsersLikeDpId(dpId);
        else
            users = userService.GetUsersByDpId(dpId);
        this.rptUsers.DataSource = users;
        this.rptUsers.DataBind();

        this.divMessage.Visible = false;
        if (isContainsLower && userService.GetUserCountLikeDpId(dpId) > 300)
        {
            this.divMessage.Visible = true;
            this.divMessage.InnerHtml = "数据超过 100 条，出于效率考虑，只取了前 100 条记录.";
        }
    }

    #endregion

    protected void btnBatchAssgin_Click(object sender, EventArgs e)
    {
        if (Request.Form["chkboxUser"] == null)
        {
            //ShowError("请指定被分配的用户.");
            throw new Exception("请指定被分配的用户.");
        }

        string inputUsers = Request.Form["chkboxUser"];
        string[] userArray = inputUsers.Trim().Split(',', '，');
        int count = userArray.Length;

        IList<Guid> users = new List<Guid>();
        for (int i = 0; i < count; i++)
        {
            users.Add(new Guid(userArray[i]));
        }
        this.SaveData(users);
    }

    protected void btnBatchDepartment_Click(object sender, EventArgs e)
    {
        string dpId = this.treeDepts.SelectedValue;
        if (string.IsNullOrEmpty(dpId))
        {
            //ShowError("请先选择部门，然后再进行按部门批量分配.");
            throw new Exception("请先选择部门，然后再进行按部门批量分配.");
        }

        bool isContainsLower = true; // chkboxContainsLower.Checked;
        IList<UserInfo> users = null;
        if (isContainsLower)
            users = userService.GetUsersLikeDpId(dpId);
        else
            users = userService.GetUsersByDpId(dpId);

        IList<Guid> userIds = new List<Guid>();
        foreach (UserInfo item in users)
        {
            userIds.Add(item.UserId);
        }
        this.SaveData(userIds);
    }

    public void SaveData(IList<Guid> users)
    {
        Guid roleId = new Guid(this.ddlRoles.SelectedValue);
        if (rdiolistBatchType.SelectedIndex == 0)
        {
            roleService.InsertUserRoles(users, roleId);
            //ShowSuccess("分配角色成功！");
        }
        else
        {
            roleService.RecycleRoles(roleId, users);
            //ShowSuccess("成功收回角色权限！");
        }
    }
}
