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

public partial class contrib_workflow_pages_DoneTask : Botwave.Security.Web.PageBase
{
    private const string returnUrl = "~/plugins/easyflow/contrib/workflow/pages/DoneTask.aspx";//wbl

    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IWorkflowPagerService workflowPagerService = (IWorkflowPagerService)Ctx.GetObject("workflowPagerService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public IWorkflowPagerService WorkflowPagerService
    {
        set { this.workflowPagerService = value; }
    }

    public string UserName
    {
        get { return (string)ViewState["UserName"]; }
        set { ViewState["UserName"] = value; }
    }
    public string FieldOrder
    {
        get { return (string)ViewState["FieldOrder"]; }
        set { ViewState["FieldOrder"] = value; }
    }
    public int RecordCount
    {
        get
        {
            if (ViewState["RecordCount"] == null) return 0;
            return (int)ViewState["RecordCount"];
        }
        set { ViewState["RecordCount"] = value; }
    }
    public int PageIndex
    {
        get
        {
            if (ViewState["PageIndex"] == null) return 0;
            return (int)ViewState["PageIndex"];
        }
        set { ViewState["PageIndex"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Botwave.Security.LoginUser user = CurrentUser;
            string userName = user.UserName;
            this.UserName = userName;

            this.BindWorkflowsList(user);
            SearchDealedTask(0, 0);
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

    protected void SearchDealedTask(int recordCount, int pageIndex)
    {
        string keywords = "", startdt = "", enddt = "", workflowName = "", creater = string.Empty, sheetID = string.Empty;
        if (Request.QueryString["q"] != null) // 关键字.
        {
            keywords = Request.QueryString["q"];
            textKeywords.Text = keywords;
        }

        if (Request.QueryString["c"] != null) // 发起人.
        {
            creater = Request.QueryString["c"];
            txtCreater.Text = creater;
        }

        if (Request.QueryString["s"] != null) // 受理号.
        {
            sheetID = Request.QueryString["s"];
            txtSheetID.Text = sheetID;
        }

        if (Request.QueryString["st"] != null) // 最小流程处理时间.
        {
            startdt = Request.QueryString["st"];
            txtStartDT.Text = startdt;
        }
        if (Request.QueryString["et"] != null) // 最大流程处理时间

        {
            enddt = Request.QueryString["et"];
            txtEndDT.Text = enddt;
        }
        if (Request.QueryString["wf"] != null) // 流程名称
        {
            workflowName = Request.QueryString["wf"].Trim();
        }

        bool isStarter = false;
        if (!string.IsNullOrEmpty(workflowName))
            workflowName = workflowName.IndexOf("-") > -1 ? workflowName.Substring(3) : workflowName;
        //DataTable dt = workflowPagerService.GetDoneTaskPager(workflowName, this.UserName, keywords, startdt, enddt, isStarter, pageIndex, listPager.ItemsPerPage, ref recordCount);
        DataTable dt = workflowPagerService.GetDoneTaskPager(workflowName, this.UserName, keywords, sheetID, creater, startdt, enddt, isStarter, FieldOrder, pageIndex, listPager.ItemsPerPage, ref recordCount);
        rptList.DataSource = dt;
        rptList.DataBind();

        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
    }

    #region Methods

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemIndex < 0)
            return;
        DataRowView row = e.Item.DataItem as DataRowView;
        string activityName = DbUtils.ToString(row["ActivityName"]);
        if (string.IsNullOrEmpty(activityName))
        {
            string back = DbUtils.ToString(row["OperateType"]);

            Literal ltlCurrentActivityName = e.Item.FindControl("ltlActivityName") as Literal;
            if (back == "1")
                ltlCurrentActivityName.Text = string.Format("<font color=\"red\">{0}</font>", activityName);
            else
                ltlCurrentActivityName.Text = string.Format("<font color=\"green\">{0}</font>", activityName);
        }

        string aliasImageUrl = DbUtils.ToString(row["AliasImage"]); // 别名
        if (!string.IsNullOrEmpty(aliasImageUrl))
        {
            string workflowAlias = DbUtils.ToString(row["WorkflowAlias"]);
            Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{2}contrib/workflow/res/groups/{1}\" />", workflowAlias, aliasImageUrl, AppPath);
        }

        Literal ltl = e.Item.FindControl("ltlCurrentActors") as Literal;
        ltl.Text = WorkflowUtility.FormatWorkflowActor(row["CurrentActors"].ToString());
    }

    protected void rptList_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            if (ViewState[e.CommandName.Trim()] == null)
            {
                ViewState[e.CommandName.Trim()] = "DESC";
            }
            else
            {
                if (ViewState[e.CommandName.Trim()].ToString().Trim() == "DESC")
                {
                    ViewState[e.CommandName.Trim()] = "ASC";
                }
                else
                {
                    ViewState[e.CommandName.Trim()] = "DESC";
                }
            }

            FieldOrder = e.CommandName.ToString().Trim() + " " + ViewState[e.CommandName.Trim()].ToString().Trim();
            SearchDealedTask(RecordCount, PageIndex);
        }
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        RecordCount = listPager.TotalRecordCount;
        PageIndex = e.NewPageIndex;
        SearchDealedTask(listPager.TotalRecordCount, e.NewPageIndex);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string keywords = HttpUtility.UrlEncode(textKeywords.Text.Trim());
        System.Text.StringBuilder sbUrl = new System.Text.StringBuilder();
        sbUrl.Append(returnUrl);

        //sbUrl.AppendFormat("?q={0}&wf={1}&st={2}&et={3}", keywords, HttpUtility.UrlEncode(ddlWorkflows.SelectedValue), txtStartDT.Text.Trim(), txtEndDT.Text.Trim(), 0);
        sbUrl.AppendFormat("?q={0}&wf={1}&st={2}&et={3}&c={4}&s={5}", textKeywords.Text.Trim(), HttpUtility.UrlEncode(ddlWorkflows.SelectedValue), txtStartDT.Text.Trim(), txtEndDT.Text.Trim(), txtCreater.Text.Trim(), txtSheetID.Text.Trim());
        Response.Redirect(sbUrl.ToString());
    }

    protected void chk_CheckedChanged(object sender, EventArgs e)
    {
        this.listPager.CurrentPageIndex = 0;
        this.SearchDealedTask(0, 0);
    }



    #endregion
}
