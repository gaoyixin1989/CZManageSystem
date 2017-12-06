using System;
using System.Collections;
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

public partial class contrib_workflow_pages_WorkflowDoneIndex : Botwave.Web.PageBase
{
    private IWorkflowPagerService workflowPagerService = (IWorkflowPagerService)Ctx.GetObject("workflowPagerService");
    public IWorkflowPagerService WorkflowPagerService
    {
        set { workflowPagerService = value; }
    }

    public string WorkflowName
    {
        get { return (string)(ViewState["WorkflowName"]); }
        set { ViewState["WorkflowName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wname = Request.QueryString["wname"];
            if (string.IsNullOrEmpty(wname))
                ShowError(Botwave.Web.MessageHelper.Message_ArgumentException);
            this.WorkflowName = wname;
            this.InitBindDoneTask();
        }
    }

    #region 已办任务列表

    /// <summary>
    /// 初始化绑定已办任务列表.
    /// </summary>
    public void InitBindDoneTask()
    {
        this.SearchDoneTaskList(0, 0);
    }

    protected void listDoneTask_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = e.Item.DataItem as DataRowView;
        string aliasImage = DbUtils.ToString(row["AliasImage"]); // 别名
        if (!string.IsNullOrEmpty(aliasImage))
        {
            string workflowAlias = DbUtils.ToString(row["WorkflowAlias"]);
            Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"../res/groups/{1}\" />", workflowAlias, aliasImage);
        }

        Literal ltl = e.Item.FindControl("ltlCurrentActors") as Literal;
        ltl.Text =  WorkflowUtility.FormatWorkflowActor(row["CurrentActors"].ToString());
    }

    protected void listPagerDoneTask_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.SearchDoneTaskList(listPagerDoneTask.TotalRecordCount, e.NewPageIndex);
    }

    protected void SearchDoneTaskList(int recordCount, int pageIndex)
    {
        string userName = Botwave.Security.LoginHelper.UserName;
        DataTable results = workflowPagerService.GetDoneTaskPager(this.WorkflowName, userName, "", "", "", false, pageIndex, listPagerDoneTask.ItemsPerPage, ref recordCount);
        this.listDoneTask.DataSource = results;
        this.listDoneTask.DataBind();

        listPagerDoneTask.TotalRecordCount = recordCount;
        listPagerDoneTask.DataBind();
    }

    #endregion
}
