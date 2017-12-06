using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.XQP.Commons;
using Botwave.XQP.Service;

public partial class apps_xqp2_pages_workflows_controls_WorkflowDetailStat : Botwave.Security.Web.UserControlBase
{
    #region service

    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IWorkflowResourceService workflowResourceService = (IWorkflowResourceService)Ctx.GetObject("workflowResourceService");
    private IWorkflowReportService workflowReportService = (IWorkflowReportService)Ctx.GetObject("workflowReportService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        get { return workflowDefinitionService; }
        set { workflowDefinitionService = value; }
    }

    public IWorkflowResourceService WorkflowResourceService
    {
        get { return workflowResourceService; }
        set { workflowResourceService = value; }
    }

    public IWorkflowReportService WorkflowReportService
    {
        get { return workflowReportService; }
        set { workflowReportService = value; }
    }
    #endregion
    private string advanceResource = "A019";

    public string WorkflowName
    {
        get { return (string)(ViewState["WorkflowName"]); }
        set { ViewState["WorkflowName"] = value; }
    }

    /// <summary>
    /// 流程名称.
    /// </summary>
    public string UserName
    {
        get { return (string)(ViewState["UserName"]); }
        set { ViewState["UserName"] = value; }
    }

    /// <summary>
    /// 高级权限资源.
    /// </summary>
    public string AdvanceResource
    {
        get { return advanceResource; }
        set { advanceResource = value; }
    }

    public bool EnableAdvanceReport
    {
        get { return ViewState["EnableAdvanceReport"] == null ? false : (bool)ViewState["EnableAdvanceReport"]; }
        set { ViewState["EnableAdvanceReport"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtStartDT.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01");
            txtEndDT.Text = DateTime.Now.ToString("yyyy-MM-dd");

            Botwave.Security.LoginUser user = CurrentUser;
            this.UserName = user.UserName;
            this.EnableAdvanceReport = HasAdvanceReport(user);
            LoadWorkflowData(user);
            LoadData();
        }
    }

    private void LoadWorkflowData(Botwave.Security.LoginUser user)
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
        workflows = WorkflowUtility.GetAllowedWorkflows(workflows, user.Resources, "0002");
        ddlWorkflowList.Items.Insert(0, new ListItem("---请选择---", ""));
        if (workflows != null && workflows.Count > 0)
        {
            //foreach (WorkflowDefinition definition in workflows)
            //{
            //    string name = definition.WorkflowName;
            //    string alias = definition.WorkflowAlias;
            //    ddlWorkflowList.Items.Add(new ListItem((string.IsNullOrEmpty(alias) ? string.Empty : (alias + "-")) + name, name));
            //}
            DataTable dt = new DataTable();
            dt.Columns.Add("alias");
            dt.Columns.Add("name");
            dt.Columns.Add("wname");
            foreach (WorkflowDefinition definition in workflows)
            {
                string name = definition.WorkflowName;
                string alias = definition.WorkflowAlias;
                //ddlWorkflowList.Items.Add(new ListItem((string.IsNullOrEmpty(alias) ? string.Empty : (alias + "-")) + name, name));
                DataRow dw = dt.NewRow();
                dw[0] = alias;
                dw[1] = name;
                dw[2] = (string.IsNullOrEmpty(alias) ? string.Empty : (alias + "-")) + name;
                dt.Rows.Add(dw);
            }
            DataRow[] dws = dt.Select("", "alias");
            foreach (DataRow dw in dws)
            {
                ddlWorkflowList.Items.Add(new ListItem(DbUtils.ToString(dw[2]), DbUtils.ToString(dw[2])));
            }
        }
        this.WorkflowName = ddlWorkflowList.SelectedValue;
    }

    private void LoadData()
    {
        string startDT = txtStartDT.Text.Trim();
        string endDT = txtEndDT.Text.Trim();
        bool isByUser = ddlStatType.SelectedIndex == 0 ? true : false;

        string owner = (this.EnableAdvanceReport ? null : this.UserName);
        if (!string.IsNullOrEmpty(WorkflowName))
            WorkflowName = WorkflowName.IndexOf("-") > -1 ? WorkflowName.Substring(3) : WorkflowName;
        DataSet ds = workflowReportService.GetProcessDetail(owner, startDT, endDT, WorkflowName, isByUser);

        rptActivityList.DataSource = ds.Tables[2];
        rptActivityList.DataBind();

        rptReportDetail.DataSource = ds.Tables[0];
        rptReportDetail.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        WorkflowName = ddlWorkflowList.SelectedValue;
        LoadData();
    }

    /// <summary>
    /// 导出为 Excel 文件.
    /// </summary>
    public void ExportExcel()
    {
        string fileName = (this.ddlWorkflowList.SelectedItem == null ? this.ddlWorkflowList.SelectedValue : this.ddlWorkflowList.SelectedItem.Text);
        fileName = fileName + "流转明细报表";
        Botwave.XQP.Commons.XQPHelper.ExportExcel(this.dataTable1, fileName);
    }

    protected bool HasAdvanceReport(Botwave.Security.LoginUser user)
    {
        if (user == null)
            return false;
        if (string.IsNullOrEmpty(this.advanceResource))
            return true;

        bool result = false;
        if (user.Properties.ContainsKey("Report_Advance_Enable"))
        {
            result = Convert.ToBoolean(user.Properties["Report_Advance_Enable"]);
        }
        else
        {
            result = user.Resources.ContainsKey(this.advanceResource);
            user.Properties["Report_Advance_Enable"] = result;
        }
        return result;
    }
}
