using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;

public partial class contrib_workflow_pages_AssignmentTask : Botwave.Security.Web.PageBase
{
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private ITaskAssignService taskAssignService = (ITaskAssignService)Ctx.GetObject("taskAssignService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        get { return workflowDefinitionService; }
        set { workflowDefinitionService = value; }
    }

    public ITaskAssignService TaskAssignService
    {
        get { return taskAssignService; }
        set { taskAssignService = value; }
    }

    public string Actor
    {
        get { return (string)ViewState["Actor"]; }
        set { ViewState["Actor"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Botwave.Security.LoginUser user = CurrentUser;
            this.Actor = user.UserName;
            this.BindWorkflows(user);
            this.BindControls();
            this.Search(0, 0);
        }
    }

    private void BindControls()
    {
        //string workflowName = Request.QueryString["wf"];
        //string keywords = Request.QueryString["q"];
        //string begintime = Request.QueryString["st"];
        //string endtime = Request.QueryString["ed"];
        string workflowName = Server.HtmlEncode(Request.QueryString["wf"]);
        string keywords = Server.HtmlEncode(Request.QueryString["q"]);
        string begintime = Server.HtmlEncode(Request.QueryString["st"]);
        string endtime = Server.HtmlEncode(Request.QueryString["et"]);

        if (!string.IsNullOrEmpty(workflowName))
            workflowName = HttpUtility.UrlDecode(workflowName);

        if (!string.IsNullOrEmpty(keywords))
            keywords = HttpUtility.UrlDecode(keywords);
        this.textKeywords.Text = keywords;
        this.txtStartDT.Text = begintime;
        this.txtEndDT.Text = endtime;
    }

    private void BindWorkflows(Botwave.Security.LoginUser user)
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
        ddlWorkflows.Items.Insert(0, new ListItem("－ 全部 －", ""));
        //if (workflows != null && workflows.Count > 0)
        //{
        //    foreach (WorkflowDefinition definition in workflows)
        //    {
        //        string name = definition.WorkflowName;
        //        string alias = definition.WorkflowAlias;
        //        ddlWorkflows.Items.Add(new ListItem((string.IsNullOrEmpty(alias) ? string.Empty : (alias + "-")) + name, name));
        //    }
        //}
        if (workflows != null && workflows.Count > 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("alias");
            dt.Columns.Add("name");
            dt.Columns.Add("wname");
            foreach (WorkflowDefinition definition in workflows)
            {
                string name = definition.WorkflowName;
                string alias = definition.WorkflowAlias;
                DataRow dw = dt.NewRow();
                dw[0] = alias;
                dw[1] = name;
                dw[2] = (string.IsNullOrEmpty(alias) ? string.Empty : (alias + "-")) + name;
                dt.Rows.Add(dw);
            }
            DataRow[] dws = dt.Select("", "alias");
            foreach (DataRow dw in dws)
            {
                ddlWorkflows.Items.Add(new ListItem(DbUtils.ToString(dw[2]), DbUtils.ToString(dw[2])));
            }
        }
        this.ddlWorkflows.SelectedValue = Request.QueryString["wf"];
    }

    private void Search(int pageIndex, int recordCount)
    {
        string actor = this.Actor;
        string workflowName = Request.QueryString["wf"];
        string keywords = Request.QueryString["q"];
        string begintime = Request.QueryString["st"];
        string endtime = Request.QueryString["et"];

        if (!string.IsNullOrEmpty(workflowName))
            workflowName = HttpUtility.UrlDecode(workflowName);

        if (!string.IsNullOrEmpty(keywords))
            keywords = HttpUtility.UrlDecode(keywords);
        if (!string.IsNullOrEmpty(workflowName))
            workflowName = workflowName.IndexOf("-") > -1 ? workflowName.Substring(3) : workflowName;
        DataTable results = this.taskAssignService.GetAssignmentTasks(actor, workflowName, keywords,
            begintime, endtime, pageIndex, this.listAssignmentPager.ItemsPerPage, ref recordCount);

        rptAssignmentList.DataSource = results;
        rptAssignmentList.DataBind();

        listAssignmentPager.TotalRecordCount = recordCount;
        listAssignmentPager.DataBind();
    }

    protected void rptAssignmentList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = e.Item.DataItem as DataRowView;
        string aliasImage = DbUtils.ToString(row["AliasImage"]); // 别名
        if (!string.IsNullOrEmpty(aliasImage))
        {
            string workflowAlias = DbUtils.ToString(row["WorkflowAlias"]);
            Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{1}contrib/workflow/res/groups/{2}\" />", workflowAlias, AppPath, aliasImage);
        }
    }

    protected void listAssignmentPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.Search(e.NewPageIndex, listAssignmentPager.TotalRecordCount);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //string url = string.Format("AssignmentTask.aspx?q={0}&wf={1}&st={2}&et={3}", HttpUtility.UrlEncode(textKeywords.Text.Trim()), HttpUtility.UrlEncode(ddlWorkflows.SelectedValue), txtStartDT.Text.Trim(), txtEndDT.Text.Trim());
        string url = string.Format("AssignmentTask.aspx?ass=assign&q={0}&wf={1}&st={2}&et={3}", HttpUtility.UrlEncode(textKeywords.Text.Trim()), HttpUtility.UrlEncode(ddlWorkflows.SelectedValue), txtStartDT.Text.Trim(), txtEndDT.Text.Trim());
        Response.Redirect(url);
    }
}
