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

public partial class contrib_security_pages_EditRole : Botwave.Security.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_security_pages_EditRole));
    protected string CommandText = "新增";
    protected Guid? roleId = null;
    protected DataTable source;

    private IRoleService roleService = (IRoleService)Ctx.GetObject("roleService");
    private IResourceService resourceService = (IResourceService)Ctx.GetObject("resourceService");

    public IRoleService RoleService
    {
        set { roleService = value; }
    }

    public IResourceService ResourceService
    {
        set { resourceService = value; }
    }

    /// <summary>
    /// 是否为父角色.
    /// </summary>
    public bool IsParent
    {
        get { return (ViewState["IsParent"] == null ? false : (bool)ViewState["IsParent"]); }
        set { ViewState["IsParent"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            source = resourceService.GetResourcesByVisible();

            this.BindParentRoles();
            if (Request.QueryString["RoleId"] != null)
            {
                roleId = new Guid(Request.QueryString["RoleId"]);
                this.SetEditRole(roleId.Value);
            }
            else
            {
                if (this.ddlParentRoles.Items.Count > 2)
                    this.ddlParentRoles.SelectedIndex = 2;
                DateTime _date = DateTime.Now;
                this.dtpBeginTime.Text = _date.ToString("yyyy-MM-dd HH:mm:ss");
                this.dtpEndTime.Text = _date.AddYears(10).ToString("yyyy-MM-dd HH:mm:ss");
            }
            this.BindResources(source, roleId);
        }
    }

    private void BindParentRoles()
    {
        this.ddlParentRoles.DataSource = roleService.GetRolesByParentId(Guid.Empty);
        this.ddlParentRoles.DataTextField = "RoleName";
        this.ddlParentRoles.DataValueField = "RoleId";
        this.ddlParentRoles.DataBind();
        this.ddlParentRoles.Items.Insert(0, new ListItem("", ""));
    }

    #region 绑定权限资源列表

    private void BindResources(DataTable source, Guid? roleId)
    {
        IList<string> selectedresources = new List<string>();
        if (roleId.HasValue)
        {
            // 父资源名称.
            IDictionary<string, string> parentNames = new Dictionary<string, string>();
            DataRow[] parentRows = source.Select("(ParentId = '00') OR (parentId = '11')");
            foreach (DataRow row in parentRows)
            {
                string resourceId = row["ResourceId"].ToString();
                string alias = row["Alias"].ToString();
                if (!parentNames.ContainsKey(resourceId))
                {
                    parentNames.Add(resourceId, alias);
                }
            }
            // 已设置的资源列表.
            IList<ResourceInfo> resources = resourceService.GetResourcesByRoleId(roleId.Value);
            log.Warn(resources.Count);
            for (int i = 0; i < resources.Count; i++)
            {
                string resourceId = resources[i].ResourceId;
                if (selectedresources.Contains(resourceId))
                {
                    resources.RemoveAt(i);
                    i--;
                }
                else
                {
                    selectedresources.Add(resourceId);
                }
            }
            this.ltlResourceValues.Text = RenderSelectedResources(parentNames, resources);
        }

        this.ltlSystemResources.Text = this.BuildSystemResources(source, selectedresources);
        this.ltlWorkflowResources.Text = this.BuildWorkflowResources(source, selectedresources);
    }

    private string BuildSystemResources(DataTable resourceTable, IList<string> selectedresources)
    {
        string parentId = "00"; // 系统资源
        DataRow[] groupRows = resourceTable.Select(string.Format("parentId = '{0}'", parentId), "ResourceId");
        if (groupRows == null)
            return string.Empty;

        ComplexHtml.CheckBoxTable resultTable = new ComplexHtml.CheckBoxTable();
        resultTable.Attributes.Add("style", "width:100%");
        resultTable.RepeatColumns = 4;
        resultTable.Id = "sysres";
        resultTable.Name = "res";
        foreach (DataRow groupRow in groupRows)
        {
            ComplexHtml.CheckBoxGroup group = ToCheckBoxGroup(groupRow);
            DataRow[] rows = resourceTable.Select(string.Format("ParentId = '{0}'", groupRow["ResourceId"].ToString()), "ResourceId");
            if (rows != null && rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    string roleId = row["ResourceId"].ToString();
                    bool isChecked = selectedresources.Contains(roleId);
                    group.Items.Add(ToCheckBox(row, isChecked));
                }
                resultTable.Groups.Add(group);
            }
        }
        return resultTable.Build();
    }

    private string BuildWorkflowResources(DataTable resourceTable, IList<string> selectedresources)
    {
        string parentId = "11"; // 流程资源
        DataRow[] groupRows = resourceTable.Select(string.Format("parentId = '{0}'", parentId), "ResourceId");
        if (groupRows == null)
            return string.Empty;

        ComplexHtml.CheckBoxTable resultTable = new ComplexHtml.CheckBoxTable();
        resultTable.RepeatColumns = 4;
        resultTable.Id = "wfres";
        resultTable.Name = "res";
        foreach (DataRow groupRow in groupRows)
        {
            ComplexHtml.CheckBoxGroup group = ToCheckBoxGroup(groupRow);
            group.RowVisible = false;
            DataRow[] rows = resourceTable.Select(string.Format("ParentId = '{0}'", groupRow["ResourceId"].ToString()), "ResourceId");
            if (rows != null && rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    string roleId = row["ResourceId"].ToString();
                    bool isChecked = selectedresources.Contains(roleId);
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
        item.Text = row["Alias"].ToString();
        item.Value = row["ResourceId"].ToString();
        item.Checked = isChecked;
        return item;
    }

    private static ComplexHtml.CheckBoxGroup ToCheckBoxGroup(DataRow row)
    {
        ComplexHtml.CheckBoxGroup group = new ComplexHtml.CheckBoxGroup(row["Alias"].ToString());
        group.Value = row["ResourceId"].ToString();
        group.Attributes.Add("style", "cursor:pointer;width:100%");
        return group;
    }

    #endregion

    private static string RenderSelectedResources(IDictionary<string, string> parentResources, IList<ResourceInfo> resouces)
    {
        if (resouces == null || resouces.Count == 0)
            return string.Empty;

        int repeateColumns = 3;
        int count = resouces.Count;

        StringBuilder builder = new StringBuilder();
        builder.Append("<table style=\"width:100%\" class=\"tblGrayClass\" cellpadding=\"4\" cellspacing=\"1\">");

        for (int index = 0; index < count; index++)
        {
            int columnIndex = index % repeateColumns;
            if (columnIndex == 0)
                builder.AppendLine("<tr>");
            ResourceInfo item = resouces[index];
            string parentId = item.ParentId;
            string text = item.Alias;
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

    private void SetEditRole(Guid roleId)
    {
        CommandText = "编辑";
        btnEdit.CssClass = "btn_sav";
        btnEdit.Text = "保存";

        RoleInfo role = roleService.GetRoleById(roleId);
        if (role == null)
            ShowError("找不到角色信息!");

        textRoleName.Text = role.RoleName;
        dtpBeginTime.Text = role.BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
        dtpEndTime.Text = role.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
        textComment.Text = role.Comment;
        //chkboxInheritable.Checked = role.IsInheritable;
        if (role.ParentId == Guid.Empty)
        {
            this.IsParent = true;

            // 有子角色时不能修改其父角色属性

            int count = roleService.GetRoleCountByParentId(roleId);
            if (count > 0)
            {
                this.ddlParentRoles.Enabled = false;
            }
            else
            {
                // 当修改的是父角色时，从父角色下拉列表中删除当前父角色,防止角色自身引用
                string currentId = roleId.ToString();
                ListItemCollection listItems = this.ddlParentRoles.Items;
                for (int i = 0; i < listItems.Count; i++)
                {
                    if (currentId.Equals(listItems[i].Value, StringComparison.OrdinalIgnoreCase))
                    {
                        listItems.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        else
        {
            // 非 父角色

            this.ddlParentRoles.SelectedValue = role.ParentId.ToString();
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string creator = CurrentUserName;

        RoleInfo item = new RoleInfo();
        item.RoleName = textRoleName.Text;
        //item.IsInheritable = chkboxInheritable.Checked;
        item.Comment = textComment.Text;
        item.BeginTime = Convert.ToDateTime(dtpBeginTime.Text);
        item.EndTime = Convert.ToDateTime(dtpEndTime.Text);
        item.LastModTime = DateTime.Now;
        item.LastModifier = creator;

        // 编辑
        if (Request.QueryString["RoleId"] != null)
            roleId = new Guid(Request.QueryString["RoleId"]);

        // 父角色编号

        if (!string.IsNullOrEmpty(ddlParentRoles.SelectedValue))
            item.ParentId = new Guid(ddlParentRoles.SelectedValue);
        else
            item.ParentId = Guid.Empty;

        string message = "";
        if (roleId == null)
        {
            // 新增
            roleId = Guid.NewGuid();
            item.RoleId = roleId.Value;
            item.CreatedTime = DateTime.Now;

            roleService.InsertRole(item);
            message = "创建角色成功！";
        }
        else
        {
            item.RoleId = roleId.Value;
            if (roleService.UpdateRole(item) == 1)
            {
                message = "修改角色成功！";
            }
            else
            {
                ShowError("修改角色失败！");
            }
        }

        string selectedValues = Request.Form["res_item"];
        if (!string.IsNullOrEmpty(selectedValues))
        {
            IList<string> selectedresources = new List<string>();
            string[] valueArray = selectedValues.Split(',');
            foreach (string arrayItem in valueArray)
            {
                selectedresources.Add(arrayItem);
            }

            if (roleId != null)
            {
                roleService.InsertRoleResources(roleId.Value, selectedresources);
            }
        }
        ShowSuccess(message, AppPath + "contrib/security/pages/roles.aspx?Pid="+Request.QueryString["Pid"]);
    }
}
