using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Commons;
using Botwave.XQP.Service;
using Botwave.Commons;

public partial class apps_xqp2_pages_workflows_controls_ActivityStat : Botwave.Security.Web.UserControlBase
{
    #region services

    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IReportService reportService = (IReportService)Ctx.GetObject("reportService");
    private IWorkflowReportService workflowReportService = (IWorkflowReportService)Ctx.GetObject("workflowReportService");
    private IWorkflowResourceService workflowResourceService = (IWorkflowResourceService)Ctx.GetObject("workflowResourceService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        get { return workflowDefinitionService; }
        set { workflowDefinitionService = value; }
    }

    public IReportService ReportService
    {
        get { return reportService; }
        set { reportService = value; }
    }

    public IWorkflowReportService WorkflowReportService
    {
        get { return workflowReportService; }
        set { workflowReportService = value; }
    }

    public IWorkflowResourceService WorkflowResourceService
    {
        get { return workflowResourceService; }
        set { workflowResourceService = value; }
    }

    #endregion

    private string advanceResource = "A019";

    /// <summary>
    /// 流程名称.
    /// </summary>
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
            if (DateTime.Now.Day > 15)
                txtStartDT.Text = DateTime.Now.ToString("yyyy-MM-01");
            else
                txtStartDT.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01");
            txtEndDT.Text = DateTime.Now.ToString("yyyy-MM-dd");

            Botwave.Security.LoginUser user = CurrentUser;
            this.UserName = user.UserName;
            this.EnableAdvanceReport = HasAdvanceReport(user);
            this.BindWorkflowData(user);

            string workflowName = ddlWorkflowList.SelectedValue;
            this.WorkflowName = workflowName;
            this.BindData(workflowName, "", "");
        }
    }

    private void BindWorkflowData(Botwave.Security.LoginUser user)
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
        
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
        workflows = WorkflowUtility.GetAllowedWorkflows(workflows, user.Resources, "0002");
    }

    private void BindData(string workflowName, string startDT, string endDT)
    {
        string owner = (this.EnableAdvanceReport ? string.Empty : this.UserName);
        if (!this.EnableAdvanceReport)
            owner = AllowReport(WorkflowName) ? null : this.UserName;
        if (!string.IsNullOrEmpty(workflowName))
            workflowName = workflowName.IndexOf("-") > -1 ? workflowName.Substring(3) : workflowName;
        DataTable result = workflowReportService.GetActivityStat(owner, workflowName, startDT, endDT);
        //IList<Report> result = reportService.GetActivityStat(workflowName, startDT, endDT);
        rptStateStat.DataSource = result;
        rptStateStat.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.WorkflowName = ddlWorkflowList.SelectedValue;
        BindData(WorkflowName, txtStartDT.Text, txtEndDT.Text);
    }

    protected void ddlWorkflowList_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.WorkflowName = ddlWorkflowList.SelectedValue;
        BindData(WorkflowName, txtStartDT.Text, txtEndDT.Text);
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

    /// <summary>
    /// 是否允许统计该流程的所有工单
    /// </summary>
    /// <param name="workflowName"></param>
    /// <returns></returns>
    private bool AllowReport(string workflowName)
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
            if (workflows != null && workflows.Count > 0)
            {
                IList<WorkflowDefinition> allowWorkflows = WorkflowUtility.GetAllowedWorkflows(workflows, CurrentUser.Resources, "0002");
                foreach (WorkflowDefinition item in allowWorkflows)
                {
                    if (item.WorkflowName == workflowName)
                        return true;
                }
            }
        return false;
    }
}
