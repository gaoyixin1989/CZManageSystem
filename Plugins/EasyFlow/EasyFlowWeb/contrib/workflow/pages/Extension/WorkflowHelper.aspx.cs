using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Botwave.Extension.IBatisNet;
using System.Data.SqlClient;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;

public partial class contrib_workflow_pages_Extension_WorkflowHelper : Botwave.Security.Web.PageBase
{
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadWorkflowData();
    }

    #region 流程名称修改

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

        ddlWorkflows.DataSource = allowWorkflows;
        ddlWorkflows.DataTextField = "WorkflowName";
        ddlWorkflows.DataValueField = "WorkflowName";
        ddlWorkflows.DataBind();
        ddlWorkflows.Items.Insert(0, new ListItem("－ 请选择 －", ""));
    }

    protected void btnModify_Click(object sender, EventArgs e)
    {
        string oldName = ddlWorkflows.SelectedItem.Text.Trim();
        string newName = txtToWorkflowName.Text.Trim();

        SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@oldName", SqlDbType.NVarChar, 50),
                new SqlParameter("@newName", SqlDbType.NVarChar, 50)
            };
        parameters[0].Value = oldName;
        parameters[1].Value = newName;

        int returnValue = IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[helper_ChangeWorkflowName]", parameters);
        if (returnValue > 0)
        {
            ShowSuccess("流程名称更改成功!");
        }
        else
        {
            ShowError("流程名称更改失败,请返回重试!");
        }
    }
  
    #endregion

}
