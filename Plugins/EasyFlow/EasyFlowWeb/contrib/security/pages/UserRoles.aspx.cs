using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Security.Extension.Web.Controls;

public partial class contrib_security_pages_UserRoles : Botwave.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_security_pages_UserRoles));

    private Guid? userId = null;
    protected string realName;
    protected string departmentName;
    protected DataTable roleSource;

    private IUserService userService = (IUserService)Ctx.GetObject("userService");
    private IRoleService roleService = (IRoleService)Ctx.GetObject("roleService");

    public IUserService UserService
    {
        set { userService = value; }
    }

    public IRoleService RoleService
    {
        set { roleService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["UserId"] == null)
                ShowError("参数错误！");
            userId = new Guid(Request.QueryString["UserId"]);
            this.BindUserInfo();
            this.BindRoles(userId);
        }
    }

    private void BindUserInfo()
    {
        if (userId != null)
        {
            UserInfo user = userService.GetUserByUserId(userId.Value);
            if (user != null)
            {
                this.realName = user.RealName;
                this.departmentName = user.DpFullName;
            }
        }
    }

    #region 绑定角色

    #endregion

    #region 绑定角色

    private void BindRoles(Guid? userId)
    {
        DataTable roles = roleService.GetRolesTableByEnabeld();
        IList<Guid> selectedroles = new List<Guid>();
        if (userId.HasValue)
        {
            IList<RoleInfo> rolelist = roleService.GetRolesByUserId(userId.Value);
            for (int i = 0; i < rolelist.Count; i++)
            {
                Guid roleId = rolelist[i].RoleId;
                if (selectedroles.Contains(roleId))
                {
                    rolelist.RemoveAt(i);
                    i--;
                }
                else
                {
                    selectedroles.Add(roleId);
                }
            }
            this.ltlRoleValues.Text = RenderRoleValues(rolelist);
        }
        this.ltlRoles.Text = this.BuildRoleTable(roles, selectedroles);
    }

    private string BuildRoleTable(DataTable sourceTable, IList<Guid> selectedroles)
    {
        DataRow[] groupRows = sourceTable.Select("ParentId = '00000000-0000-0000-0000-000000000000'", "SortOrder,RoleName");
        if (groupRows == null)
            return string.Empty;

        ComplexHtml.CheckBoxTable resultTable = new ComplexHtml.CheckBoxTable();
        resultTable.RepeatColumns = 4;
        resultTable.Id = "roles";
        resultTable.Name = "roles";
        foreach (DataRow groupRow in groupRows)
        {
            ComplexHtml.CheckBoxGroup group = ToCheckBoxGroup(groupRow);
            DataRow[] rows = sourceTable.Select(string.Format("ParentId = '{0}'", groupRow["RoleId"].ToString()), "RoleName");
            if (rows != null && rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    Guid roleId = new Guid(row["RoleId"].ToString());
                    bool isChecked = selectedroles.Contains(roleId);
                    group.Items.Add(ToCheckBox(row, isChecked));
                }
                resultTable.Groups.Add(group);
            }
        }
        return resultTable.Build();
    }

    private static ComplexHtml.CheckBox ToCheckBox(DataRow row, bool isChecked)
    {
        ComplexHtml.CheckBox item = new ComplexHtml.CheckBox();
        item.Text = row["RoleName"].ToString();
        item.Value = row["RoleId"].ToString();
        item.Checked = isChecked;
        return item;
    }

    private static ComplexHtml.CheckBoxGroup ToCheckBoxGroup(DataRow row)
    {
        ComplexHtml.CheckBoxGroup group = new ComplexHtml.CheckBoxGroup(row["RoleName"].ToString());
        group.Value = row["RoleId"].ToString();
        group.Attributes.Add("style", "cursor:pointer;width:100%");
        group.RowVisible = false;
        return group;
    }

    private static string RenderRoleValues(IList<RoleInfo> roles)
    {
        if (roles == null || roles.Count == 0)
            return string.Empty;

        int repeateColumns = 4;
        int count = roles.Count;

        StringBuilder builder = new StringBuilder();
        builder.Append("<table style=\"width:100%\" class=\"tblGrayClass\" cellpadding=\"4\" cellspacing=\"1\">");

        for (int index = 0; index < count; index++)
        {
            int columnIndex = index % repeateColumns;
            if (columnIndex == 0)
                builder.AppendLine("<tr>");
            RoleInfo item = roles[index];
            string text = item.RoleName;
            builder.AppendFormat("<td>{0}</td>", text);

            if (columnIndex == repeateColumns - 1)
                builder.AppendLine("</tr>");
        }
        int modValue = count % repeateColumns;
        if (modValue > 0)
        {
            for (; modValue < repeateColumns; modValue++)
            {
                builder.Append("<td></td>");
            }
            builder.AppendLine("</tr>");
        }

        builder.Append("</table>");
        return builder.ToString();
    }

    #endregion

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["UserId"] == null)
            ShowError("参数错误！");
        userId = new Guid(Request.QueryString["UserId"]);

        string selectedValue = Request.Form["roles_item"];

        if (string.IsNullOrEmpty(selectedValue))
        {
            ShowError("请选择为用户分配的角色.");
        }

        IList<Guid> roleIds = new List<Guid>();
        try
        {
            string[] valueArray = selectedValue.Split(',');
            if (valueArray == null || valueArray.Length == 0)
            {
                ShowError("请选择为用户分配的角色.");
            }
            foreach (string item in valueArray)
            {
                roleIds.Add(new Guid(item));
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            ShowError("操作错误.");
        }

        if (userId != null)
        {
            roleService.InsertUserRoles(userId.Value, roleIds);
            ShowSuccess("分配角色成功！", AppPath + "contrib/security/pages/users.aspx");
        }
        else
        {
            ShowError("对不起，未找到相应的用户！");
        }
    }
}
