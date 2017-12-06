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

public partial class apps_xqp2_pages_workflows_controls_WorkflowCommentStat : Botwave.Security.Web.UserControlBase
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
        this.WorkflowName = ddlWorkflowList.SelectedValue;
    }

    private void LoadData()
    {
        string startDT = txtStartDT.Text.Trim();
        string endDT = txtEndDT.Text.Trim();

        string owner = (this.EnableAdvanceReport ? null : this.UserName);
        if (!string.IsNullOrEmpty(WorkflowName))
            WorkflowName = WorkflowName.IndexOf("-") > -1 ? WorkflowName.Substring(3) : WorkflowName;
        if (!this.EnableAdvanceReport)
            owner = AllowReport(WorkflowName) ? null : this.UserName;
        DataSet ds = workflowReportService.GetWorkflowOption(owner, startDT, endDT, WorkflowName);
        rptOption.DataSource = ds.Tables[0];
        rptOption.DataBind();
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        WorkflowName = ddlWorkflowList.SelectedValue;
        LoadData();
    }

    public void ExportExcel()
    {
        string fileName = (this.ddlWorkflowList.SelectedItem == null ? this.ddlWorkflowList.SelectedValue : this.ddlWorkflowList.SelectedItem.Text);
        fileName = fileName + "审批意见报表";
        string startDT = txtStartDT.Text.Trim();
        string endDT = txtEndDT.Text.Trim();
        if (!string.IsNullOrEmpty(WorkflowName))
            WorkflowName = WorkflowName.IndexOf("-") > -1 ? WorkflowName.Substring(3) : WorkflowName;
        string owner = (this.EnableAdvanceReport ? null : this.UserName);
        if (!this.EnableAdvanceReport)
            owner = AllowReport(WorkflowName) ? null : this.UserName;
        string sql = string.Format(@"select 标题,受理号,审批步骤,审批人,审批意见,审批时间 from 
        (select title as 标题,sheetid as 受理号,activityname as 审批步骤,realname as 审批人,reason as 审批意见,CONVERT(varchar(100),ta.finishedtime,20) as 审批时间,1 as sort,tw.StartedTime
        from bwwf_tracking_activities_completed ta left join bwwf_tracking_workflows tw on ta.workflowinstanceid=tw.workflowinstanceid
        LEFT JOIN bwwf_Activities a ON a.ActivityId = ta.ActivityId 
	    LEFT JOIN bw_Users u ON u.UserName = ta.Actor
        LEFT JOIN bwwf_Workflows w ON w.WorkflowId = tw.WorkflowId WHERE w.WorkflowName = '{0}' AND 
	                    tw.StartedTime >= CAST('{1}' AS datetime) AND tw.StartedTime <= CAST('{2}' AS datetime) {3} 
        union
        select title as 标题,sheetid as 受理号,activityname as 审批步骤,(CASE tw.State WHEN 2 THEN '' WHEN 99 THEN '' ELSE dbo.fn_bwwf_GetCurrentActors(tw.WorkflowInstanceId) END) as 审批人,reason as 审批意见,CONVERT(varchar(100),ta.finishedtime,20) as 审批时间,2 as sort,tw.StartedTime
        from bwwf_tracking_activities ta left join bwwf_tracking_workflows tw on ta.workflowinstanceid=tw.workflowinstanceid
        LEFT JOIN bwwf_Activities a ON a.ActivityId = ta.ActivityId 
        LEFT JOIN bwwf_Workflows w ON w.WorkflowId = tw.WorkflowId WHERE w.WorkflowName = '{0}' AND 
	                    tw.StartedTime >= CAST('{1}' AS datetime) AND tw.StartedTime <= CAST('{2}' AS datetime)) t
        ORDER BY startedtime desc,受理号,sort, 审批时间",
        
        WorkflowName, startDT, endDT, string.IsNullOrEmpty(owner) ? string.Empty : string.Format(" and ta.Actor = '{0}'", owner));
        DataGrid grid = new DataGrid();
        grid.CssClass = "tblGrayClass";
        grid.DataSource = Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteDataset(CommandType.Text,sql).Tables[0];
        grid.DataBind();
        Botwave.XQP.Commons.XQPHelper.ExportExcel(grid, fileName);
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
