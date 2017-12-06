using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Botwave.XQP.Web;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Extension.IBatisNet;

public partial class apps_xqp2_pages_workflows_maintenance_WorkflowHelper : Botwave.Security.Web.PageBase
{
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IRoleService roleService = (IRoleService)Ctx.GetObject("roleService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }
    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }
    public IRoleService RoleService
    {
        set { roleService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadWorkflowData();
    }

    #region 权限转移
    protected void btnTransfer2_Click(object sender, EventArgs e)
    {
        /*string fromUser = txtSourceUserId.Value.Trim();
        string toUser = txtTargetUserId.Value.Trim();
        int isAppend = (cbAppend.Checked) ? 1 : 0;
        int isDelete = (cbDelete.Checked) ? 1 : 0;
        int isTransferFieldControl = (cbFieldControl.Checked) ? 1 : 0;
        int returnValue = 0;
        bool isSelectedRole = false;

        OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter("v_sourceUser", OracleType.NVarChar, 50),
                new OracleParameter("v_targetUser", OracleType.NVarChar, 50),
                new OracleParameter("v_isAppend", OracleType.Int32),
                new OracleParameter("v_isDeleteSource", OracleType.Int32),
                new OracleParameter("v_isTransferFieldControl", OracleType.Int32),
                new OracleParameter("v_transferRoleId", OracleType.VarChar, 50)
            };
        parameters[0].Value = fromUser;
        parameters[1].Value = toUser;
        parameters[2].Value = isAppend;
        parameters[3].Value = isDelete;
        parameters[4].Value = isTransferFieldControl;

        foreach (ListItem item in cblSourceUserRoles.Items)
        {
            if (item.Selected)
            {
                parameters[5].Value = item.Value;
                returnValue = IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "helper_TransferRole", parameters);
                isSelectedRole = true;
            }
        }

        if (!isSelectedRole)
        {
            parameters[5].Value = Guid.Empty.ToString();
            returnValue = IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "helper_TransferRole", parameters);
        }

        if (returnValue > 0)
        {
            ShowSuccess("权限转移成功!");
        }
        else
        {
            ShowError("权限转移失败,请返回重试!");
        }*/
    }

    protected void btnGetSourceUserRoles_Click(object sender, EventArgs e)
    {
        /*string sourceUser = txtSourceUserId.Value.Trim();
        IList<RoleInfo> sourceRoles = roleService.GetRolesByUserId(new Guid(sourceUser));
        if (null == sourceRoles || sourceRoles.Count == 0) return;

        cblSourceUserRoles.DataSource = sourceRoles;
        cblSourceUserRoles.DataTextField = "RoleName";
        cblSourceUserRoles.DataValueField = "RoleId";
        cblSourceUserRoles.DataBind();

        foreach (ListItem item in cblSourceUserRoles.Items)
        {
            item.Selected = true;
        }
        divSourceUserRoles.Visible = true;

        //字段控制信息
        string sql = string.Format(@"SELECT distinct wimg.workflowname||'-'||ActivityName FROM xqp_WorkflowFieldControl wfc
                                        LEFT JOIN xqp_WorkflowInMenuGroup wimg ON wfc.WorkflowName = wimg.WorkflowName
                                        WHERE INSTR((select username from bw_users u where userid = '{0}'),TargetUsers)>0", sourceUser);

        StringBuilder sbFieldControl = new StringBuilder();
        DataTable dtFieldControl = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        foreach (DataRow row in dtFieldControl.Rows)
        {
            sbFieldControl.AppendFormat("| {0} ", row[0].ToString());
        }

        if (sbFieldControl.ToString().Length > 0)
        {
            lblFieldControls.Text = sbFieldControl.ToString().Substring(1);
        }*/
    }

    protected void btnGetTargetUserRoles_Click(object sender, EventArgs e)
    {
        /*string targetUser = txtTargetUserId.Value.Trim();
        IList<RoleInfo> targetRoles = roleService.GetRolesByUserId(new Guid(targetUser));
        if (null == targetRoles || targetRoles.Count == 0) return;

        this.lblTargetUserRoles.Text = RenderTargetUserRole(targetRoles);
        divTargetUserRoles.Visible = true;*/
    }
    
    #endregion

    #region 流程名称修改/流程步骤名称修改
    protected void ddlWorkflowList_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadActivityData(this.ddlWorkflowList.SelectedItem.Text);
    }
        
    private void LoadWorkflowData()
    {
        bool isAdmin = Botwave.XQP.Util.CZWorkflowUtility.HasAdvanceSearch(CurrentUser, "A007");
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
        IList<WorkflowDefinition> allowWorkflows = new List<WorkflowDefinition>();
        if (isAdmin)
        {
            allowWorkflows = workflows;
        }
        else
            allowWorkflows = Botwave.XQP.Util.CZWorkflowUtility.GetAllowedWorkflows(workflows, CurrentUser.Resources, new string[] { "0005" });

        ddlWorkflowList.DataSource = allowWorkflows;
        ddlWorkflowList.DataTextField = "WorkflowName";
        ddlWorkflowList.DataValueField = "WorkflowName";
        ddlWorkflowList.DataBind();
        ddlWorkflowList.Items.Insert(0, new ListItem("－ 请选择 －", ""));
    }
    private void LoadActivityData(string workflowName)
    {
        if (string.IsNullOrEmpty(workflowName))
        {
            ddlActivityList.Items.Clear();
        }
        else
        {
            IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionListByName(workflowName);
            if (workflows != null && workflows.Count > 0)
            {
                Guid workflowId = workflows[0].WorkflowId;
                ddlActivityList.DataSource = activityDefinitionService.GetActivitiesByWorkflowId(workflowId);
                ddlActivityList.DataTextField = "ActivityName";
                ddlActivityList.DataValueField = "ActivityId";
            }
        }
        ddlActivityList.DataBind();
        ddlActivityList.Items.Insert(0, new ListItem("－ 请选择 －", ""));
    }

    protected void btnModify2_Click(object sender, EventArgs e)
    {
        string workflowName = ddlWorkflowList.SelectedItem.Text.Trim();
        string oldName = ddlActivityList.SelectedItem.Text.Trim();
        string newName = txtToActivityName.Text.Trim();

        IList<WorkflowDefinition> wList = workflowDefinitionService.GetWorkflowDefinitionListByName(newName);
        if (null != wList && wList.Count > 0)
            ShowError("新步骤名称已经存在, 请指定其它步骤名称!");

        SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@WorkflowName", SqlDbType.NVarChar, 50),
                new SqlParameter("@OldActivityName", SqlDbType.NVarChar, 50),
                new SqlParameter("@NewActivityName", SqlDbType.NVarChar, 50)
            };
        parameters[0].Value = workflowName;
        parameters[1].Value = oldName;
        parameters[2].Value = newName;

        int returnValue = IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "helper_ChangeActivityName", parameters);
        if (returnValue > 0)
        {
            ShowSuccess("流程步骤名称更改成功!");
        }
        else
        {
            ShowError("流程步骤名称更改失败,请返回重试!");
        }
    }
    #endregion

    #region Methods

    /// <summary>
    /// 呈现被转移用户拥有权限列表.
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    private static string RenderTargetUserRole(IList<RoleInfo> roles)
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

}
