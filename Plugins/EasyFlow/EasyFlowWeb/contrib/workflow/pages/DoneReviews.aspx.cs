using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Commons;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.XQP.Domain;

public partial class contrib_workflow_pages_DoneReviews : Botwave.Security.Web.PageBase
{
    private static string returnUrl = BasePage + "contrib/workflow/pages/doneReviews.aspx";
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public string UserName
    {
        get { return (string)ViewState["UserName"]; }
        set { ViewState["UserName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Botwave.Security.LoginUser user = CurrentUser;
            string userName = user.UserName;
            this.UserName = userName;

            this.BindWorkflowsList(user);
            this.Search(0, 0);
        }
    }

    protected void BindWorkflowsList(Botwave.Security.LoginUser user)
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string keywords = HttpUtility.UrlEncode(textKeywords.Text.Trim());
        string workflow = HttpUtility.UrlEncode(ddlWorkflows.SelectedValue);

        System.Text.StringBuilder sbUrl = new System.Text.StringBuilder();
        sbUrl.Append(returnUrl);

        sbUrl.AppendFormat("?q={0}&wf={1}&st={2}&et={3}", keywords, workflow, txtStartDT.Text.Trim(), txtEndDT.Text.Trim(), 0);

        Response.Redirect(sbUrl.ToString());
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = e.Item.DataItem as DataRowView;
        string toReviewActors = DbUtils.ToString(row["ToReviewActors"]);

        string aliasImage = DbUtils.ToString(row["AliasImage"]); // 别名图片
        if (!string.IsNullOrEmpty(aliasImage))
        {
            string workflowAlias = DbUtils.ToString(row["WorkflowAlias"]);
            Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{2}contrib/workflow/res/groups/{1}\" />", workflowAlias, aliasImage, AppPath);
        }
        Literal ltlToReviewActors = e.Item.FindControl("ltlToReviewActors") as Literal;
        if (!string.IsNullOrEmpty(toReviewActors))
        {
            ltlToReviewActors.Text = WorkflowUtility.FormatWorkflowActor(toReviewActors, this.UserName);
        }
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        Search(listPager.TotalRecordCount, e.NewPageIndex);
    }

    private void Search(int recordCount, int pageIndex)
    {
        string workflowName = Request.QueryString["wf"];
        string keywords = Request.QueryString["q"];
        string startTime = Request.QueryString["st"];
        string endTime = Request.QueryString["et"];
        if (!string.IsNullOrEmpty(workflowName))
        {
            workflowName = HttpUtility.UrlDecode(workflowName);
        }
        if (!string.IsNullOrEmpty(keywords))
        {
            keywords = HttpUtility.UrlDecode(keywords);
            this.textKeywords.Text = keywords;
        }
        DateTime outTime;
        if (!string.IsNullOrEmpty(startTime))
        {
            if (!DateTime.TryParse(startTime, out outTime))
                startTime = string.Empty;
            this.txtStartDT.Text = startTime;
        }
        if (!string.IsNullOrEmpty(endTime))
        {
            if (!DateTime.TryParse(endTime, out outTime))
                endTime = string.Empty;
            this.txtEndDT.Text = endTime;
        }
        if (!string.IsNullOrEmpty(workflowName))
            workflowName = workflowName.IndexOf("-") > -1 ? workflowName.Substring(3) : workflowName;
        DataTable resultTable = ToReview.GetReviewDoneTable(this.UserName, workflowName, keywords, startTime, endTime, pageIndex, listPager.ItemsPerPage, ref recordCount);
        rptList.DataSource = resultTable;
        rptList.DataBind();

        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
    }
}
