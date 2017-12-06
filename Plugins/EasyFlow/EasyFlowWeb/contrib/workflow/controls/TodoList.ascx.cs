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

public partial class contrib_workflow_controls_TodoList : Botwave.Security.Web.UserControlBase
{
    /// <summary>
    /// 流程根目录.
    /// </summary>
    private static readonly string WorkflowRoot = AppPath + "contrib/workflow/";
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
    private bool _enableSearch = true;

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

    public bool EnableSearch
    {
        get
        {
            if (ViewState["EnableSearch"] != null)
                _enableSearch = (bool)ViewState["EnableSearch"];
            return _enableSearch;
        }
        set { ViewState["EnableSearch"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindWorkflowData();
            Search(0, 0);
            this.divSearch.Visible = this.EnableSearch;
        }
    }
    public string Title
    {
        get
        {
            object obj = ViewState["Title"];
            if (null == obj)
            {
                return "待处理任务";
            }
            return (string)obj;
        }
        set
        {
            ViewState["Title"] = value;
        }
    }

    private void BindWorkflowData()
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
        workflows = WorkflowUtility.GetAllowedWorkflows(workflows, CurrentUser.Resources, "0000");
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
        this.ddlWorkflows.SelectedValue = Request.QueryString["wfname"];
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string keywords = HttpUtility.UrlEncode(txtKeywords.Text);
        Response.Redirect(string.Format("{0}pages/default.aspx?wfname={1}&key={2}", WorkflowRoot, HttpUtility.UrlEncode(ddlWorkflows.SelectedValue), keywords));
    }

    protected void listTodoPager_PageIndexChanged(object sender, PageChangedEventArgs e)
    {
        this.Search(listPagerTodoTask.TotalRecordCount, e.NewPageIndex);
    }

    protected void Search(int recordCount, int pageIndex)
    {
        string workflowName = Request.QueryString["wfname"];
        if(!string.IsNullOrEmpty(workflowName))
            workflowName = workflowName.IndexOf("-") > -1 ? workflowName.Substring(3) : workflowName;
        string keywords = Request.QueryString["key"];
        if (!IsPostBack)
        {
            txtKeywords.Text = keywords;
        }
        DataTable source = activityService.GetTaskListByUserName(CurrentUser.UserName, workflowName, keywords, pageIndex, listPagerTodoTask.ItemsPerPage, ref recordCount);
        rptList.DataSource = source;
        rptList.DataBind();

        this.listPagerTodoTask.TotalRecordCount = recordCount;
        this.listPagerTodoTask.DataBind();
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (string.IsNullOrEmpty(currentUserName))
        {
            this.currentUserName = CurrentUser.UserName;
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
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{2}contrib/workflow/res/groups/{1}\" />", workflowAlias, aliasImage, AppPath);
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

    protected void btnExport_Click(object sender, EventArgs e)
    {
        int recordCount = 0;
        string workflowName = Request.QueryString["wfname"];
        if (!string.IsNullOrEmpty(workflowName))
            workflowName = workflowName.IndexOf("-") > -1 ? workflowName.Substring(3) : workflowName;
        string keywords = Request.QueryString["key"];
        string fileName = CurrentUser.RealName +"的待办报表"+DateTime.Now.ToString("yyyyMMdd");
        DataTable source = activityService.GetTaskListByUserName(CurrentUser.UserName, workflowName, keywords, 0, listPagerTodoTask.TotalRecordCount, ref recordCount);
        source.Columns["workflowname"].SetOrdinal(0);
        source.Columns["workflowname"].ColumnName="流程名称";
        source.Columns["WorkflowAlias"].SetOrdinal(1);
        source.Columns["WorkflowAlias"].ColumnName = "类别";
        source.Columns["title"].SetOrdinal(2);
        source.Columns["title"].ColumnName = "标题";
        source.Columns["sheetid"].SetOrdinal(3);
        source.Columns["sheetid"].ColumnName = "受理号";
        source.Columns["ActivityName"].SetOrdinal(4);
        source.Columns["ActivityName"].ColumnName = "当前步骤";
        source.Columns["CreatorName"].SetOrdinal(5);
        source.Columns["CreatorName"].ColumnName = "发起人";
        source.Columns["StartedTime"].SetOrdinal(6);
        source.Columns["StartedTime"].ColumnName = "创建时间";
        DataGrid grid = new DataGrid();
        grid.CssClass = "tblGrayClass";
        grid.ItemDataBound += new DataGridItemEventHandler(dataBusiness_ItemDataBound);
        grid.DataSource = source;
        grid.DataBind();
        if (grid.Items.Count > 0)
        {
            Botwave.XQP.Commons.XQPHelper.ExportExcel(grid, fileName);
        }
    }

    protected void dataBusiness_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        for (int i=7;i< e.Item.Cells.Count;i++)
        {
             e.Item.Cells[i].Visible = false;
        }
    }
}
