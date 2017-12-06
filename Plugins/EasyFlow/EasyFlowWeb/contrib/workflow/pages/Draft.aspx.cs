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

public partial class contrib_workflow_pages_Draft : Botwave.Security.Web.PageBase
{
    private IWorkflowService workflowService = (IWorkflowService)Ctx.GetObject("workflowService");

    public IWorkflowService WorkflowService
    {
        set { workflowService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LoadWorkflowDraft(CurrentUser.UserName);
        }
    }

    private void LoadWorkflowDraft(string userName)
    {
        DataTable result = workflowService.GetWorkflowInstanceByDraft(userName);
        rptList.DataSource = result;
        rptList.DataBind();
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = e.Item.DataItem as DataRowView;
        string workflowAlias = DbUtils.ToString(row["WorkflowAlias"]);
        string aliasImage = DbUtils.ToString(row["AliasImage"]);
        if (!string.IsNullOrEmpty(aliasImage))
        {
            Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"../res/groups/{1}\" />", workflowAlias,  aliasImage);
        }
    }


    protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Guid workflowInstanceId = new Guid(e.CommandArgument.ToString());
        if (e.CommandName == "Delete")
        {
            workflowService.DeleteWorkflowInstance(workflowInstanceId);
            this.LoadWorkflowDraft(CurrentUser.UserName);
        }
    }
}
