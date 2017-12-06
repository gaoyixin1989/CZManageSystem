using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Web;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.XQP.Commons;
using Botwave.XQP.Service;

public partial class apps_xqp2_pages_workflows_report_WorkflowList : Botwave.Security.Web.PageBase
{
    private static readonly string WorkflowPageRoot = AppPath + "contrib/workflow/";

    private ITaskAssignService taskAssignService = (ITaskAssignService)Ctx.GetObject("taskAssignService");
    private IWorkflowReportService workflowReportService = (IWorkflowReportService)Ctx.GetObject("workflowReportService");

    public ITaskAssignService TaskAssignService
    {
        set { taskAssignService = value; }
    }

    public IWorkflowReportService WorkflowReportService
    {
        set { workflowReportService = value; }
    }

    public string WorkflowName
    {
        get { return (string)(ViewState["WorkflowName"]); }
        set { ViewState["WorkflowName"] = value; }
    }

    public string ActivityName
    {
        get { return (string)(ViewState["ActivityName"]); }
        set { ViewState["ActivityName"] = value; }
    }

    public string UserName
    {
        get { return (string)(ViewState["UserName"]); }
        set { ViewState["UserName"] = value; }
    }

    protected override void OnInit(EventArgs e)
    {
        string wname = Request.QueryString["wname"];
        string aname = Request.QueryString["aname"];
        if (String.IsNullOrEmpty(wname) || String.IsNullOrEmpty(aname))
        {
            ShowError(MessageHelper.Message_ArgumentException);
        }
        this.WorkflowName = wname;
        this.ActivityName = aname;
        this.UserName = CurrentUserName;
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Search(0, 0);
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.Search(listPager.TotalRecordCount, e.NewPageIndex);
    }

    private void Search(int recordCount, int pageIndex)
    {
        string owner = null;
        this.rptTrackingWorkflows.DataSource = workflowReportService.GetWorkflowTrackingPager(owner, WorkflowName, ActivityName, pageIndex, listPager.ItemsPerPage, ref recordCount);
        this.rptTrackingWorkflows.DataBind();
        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
    }

    protected void rptTrackingWorkflows_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView item = e.Item.DataItem as DataRowView;
        Guid activityInstanceId = new Guid(item["ActivityInstanceId"].ToString());
        bool isTodo = taskAssignService.IsExistsTodo(activityInstanceId, this.UserName);
        bool isCompleted = (bool)item["IsCompleted"];
        HyperLink link = e.Item.FindControl("linkActivity") as HyperLink;
        if (isTodo && isCompleted == false)
            link.NavigateUrl = string.Format("{0}pages/process.aspx?aiid={1}", WorkflowPageRoot, activityInstanceId.ToString());
        else
            link.NavigateUrl = string.Format("{0}pages/workflowView.aspx?aiid={1}", WorkflowPageRoot, activityInstanceId.ToString());

        string aliasImageUrl = DbUtils.ToString(item["AliasImage"]); // 别名
        if (!string.IsNullOrEmpty(aliasImageUrl))
        {
            string workflowAlias = DbUtils.ToString(item["WorkflowAlias"]);
            Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{1}res/groups/{2}\" />", workflowAlias, WorkflowPageRoot, aliasImageUrl);
        }
    }
}
