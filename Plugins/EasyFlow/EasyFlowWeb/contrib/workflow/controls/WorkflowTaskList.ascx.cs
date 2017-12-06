using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Commons;
using Botwave.Web.Controls;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;

public partial class contrib_workflow_controls_WorkflowTaskList : Botwave.Security.Web.UserControlBase
{
    public static readonly string WorkflowRoot = AppPath + "contrib/workflow/";
    protected static string[] urgencyArray = { "一般", "紧急", "很紧急", "非常紧急" };
    protected static string[] importanceArray = { "一般", "重要", "很重要", "非常重要" };
    private string currentUserName;

    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IActivityService activityService = (IActivityService)Ctx.GetObject("activityService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public IActivityService ActivityService
    {
        set { activityService = value; }
    }

    private Guid? _workflowId = null;
    private string _workflowName = null;

    public Guid? WorkflowId
    {
        get
        {
            if (ViewState["WorkflowId"] != null)
                _workflowId = (Guid)ViewState["WorkflowId"];
            return _workflowId;
        }
        set { ViewState["WorkflowId"] = value; }
    }

    public string WorkflowName
    {
        get
        {
            if (ViewState["WorkflowName"] != null)
                _workflowName = (string)ViewState["WorkflowName"];
            return _workflowName;
        }
        set { ViewState["WorkflowName"] = value; }
    }

    public string UserName
    {
        get
        {
            if (ViewState["UserName"] != null)
                currentUserName = (string)ViewState["UserName"];
            return currentUserName;
        }
        set { ViewState["UserName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public override void DataBind()
    {
        base.DataBind();
        if (!IsPostBack)
        {
            this.UserName = CurrentUser.UserName;
            Search(0, 0);
        }
    }

    protected void listTodoPager_PageIndexChanged(object sender, PageChangedEventArgs e)
    {
        this.Search(listPagerTodoTask.TotalRecordCount, e.NewPageIndex);
    }

    protected void Search(int recordCount, int pageIndex)
    {
        string workflowName = WorkflowName;
        string keywords = "";
        DataTable source = activityService.GetTaskListByUserName(this.UserName, workflowName, keywords, pageIndex, listPagerTodoTask.ItemsPerPage, ref recordCount);
        rptList.DataSource = source;
        rptList.DataBind();

        this.listPagerTodoTask.TotalRecordCount = recordCount;
        this.listPagerTodoTask.DataBind();
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (string.IsNullOrEmpty(currentUserName))
        {
            this.currentUserName = this.UserName;
        }
        //lbTitle
        Label lbTitle = (Label)e.Item.FindControl("lbTitle");
        Literal ltlActivityName = e.Item.FindControl("ltlActivityName") as Literal;
        Literal ltlActivityIcons = e.Item.FindControl("ltlActivityIcons") as Literal;
        HtmlTableRow listRow = e.Item.FindControl("listRow") as HtmlTableRow;

        DataRowView row = e.Item.DataItem as DataRowView;

        string title = lbTitle.Text;
        int importance = DbUtils.ToInt32(row["Importance"]);
        int urgency = DbUtils.ToInt32(row["Urgency"]);
        int operateType = DbUtils.ToInt32(row["OperateType"]);  // 等于 1 即为退还


        bool isReaded = !TodoInfo.IsUnReaded(DbUtils.ToInt32(row["State"]));     // 是否已读.
        //string todoActors = DbUtils.ToString(row["TodoActors"]);

        string aliasImage = DbUtils.ToString(row["AliasImage"]); // 别名图片

        if (!string.IsNullOrEmpty(aliasImage))
        {
            string workflowAlias = DbUtils.ToString(row["WorkflowAlias"]);
            Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{1}res/groups/{2}\" />", workflowAlias, WorkflowRoot, aliasImage);
        }

        if (operateType == 1) // 退回

        {
            ltlActivityIcons.Text = ltlActivityIcons.Text + "<font color=\"red\">[退回]</font>";
        }
        if (importance > 0)
        {
            ltlActivityIcons.Text = ltlActivityIcons.Text + "<font color=\"red\">[重要]</font>";
        }
        if (urgency > 0)
        {
            ltlActivityIcons.Text = ltlActivityIcons.Text + "[紧急]";
        }

        //Literal ltlTodoActors = e.Item.FindControl("ltlTodoActors") as Literal;
        //if (string.IsNullOrEmpty(todoActors))
        //{
        //    ltlTodoActors.Text = "无";
        //}
        //else
        //{
        //    ltlTodoActors.Text = WorkflowUtility.FormatWorkflowActor(todoActors, this.currentUserName);
        //}

        string backgoundClass = (e.Item.ItemIndex % 2 == 1) ? "trClass " : "";
        listRow.Attributes["class"] = backgoundClass + (isReaded ? "readed" : "unread");
    }
}
