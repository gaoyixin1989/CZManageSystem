﻿using System;
using System.Data;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Commons;
using Botwave.Web;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Service;

public partial class contrib_workflow_interface_MyTask : Botwave.Security.Web.PageBase
{
    private static string rrUrl = BasePage + "contrib/workflow/interface/mytask.aspx";

    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IWorkflowSearcher workflowSearcher = (IWorkflowSearcher)Ctx.GetObject("workflowSearcher");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public IWorkflowSearcher WorkflowSearcher
    {
        set { workflowSearcher = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LoadWorkflowData();
        }
    }

    private void LoadWorkflowData()
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
        ddlWorkflows.Items.Insert(0, new ListItem("－ 全部 －", ""));
        if (workflows != null && workflows.Count > 0)
        {
            foreach (WorkflowDefinition definition in workflows)
            {
                string name = definition.WorkflowName;
                string alias = definition.WorkflowAlias;
                ddlWorkflows.Items.Add(new ListItem((string.IsNullOrEmpty(alias) ? string.Empty : (alias + "-")) + name, name));
            }
        }
        this.ddlWorkflows.SelectedValue = Request.QueryString["wfalias"];

        Search(0, 0);
    }

    protected void Search(int recordCount, int pageIndex)
    {
        string keywords = "", workflowName = "", wfalia = "";
        DateTime now = DateTime.Now;
        string startdt = now.AddYears(-100).ToString("yyyy-MM-dd"); ;
        string enddt = now.AddYears(100).ToString("yyyy-MM-dd");

        if (Request.QueryString["q"] != null) // 关键字.
        {
            keywords = Request.QueryString["q"];
            textKeywords.Text = keywords;
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
        if (Request.QueryString["wfalias"] != null) // 流程名称
        {
            wfalia = Request.QueryString["wfalias"].Trim();
            workflowName = GetWfNameByWfalias(wfalia).ToString();
        }

        string creator = CurrentUser.UserName;
        Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition = new Botwave.Workflow.Extension.Service.AdvancedSearchCondition();
        condition.BeginTime = startdt;
        condition.EndTime = enddt;
        condition.CreatorName = creator;
        condition.ProcessorName = creator;
        condition.WorkflowName = workflowName;
        condition.Title = keywords;

        //this.rptList.DataSource = workflowSearcher.Search(condition, pageIndex, listPager.ItemsPerPage, ref recordCount);
        this.rptList.DataSource = CZWorkflowSearcher.GetDoingTask(condition, pageIndex, listPager.ItemsPerPage, ref recordCount);
        this.rptList.DataBind();

        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();

        litlTotalRecordCount.Text = recordCount.ToString();
    }

    #region Methods

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
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

        string aliasImage = DbUtils.ToString(row["AliasImage"]); // 别名
        if (!string.IsNullOrEmpty(aliasImage))
        {
            string workflowAlias = DbUtils.ToString(row["WorkflowAlias"]);
            Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"../res/groups/{1}\" />", workflowAlias, aliasImage);
        }

        Literal ltl = e.Item.FindControl("ltlCurrentActors") as Literal;
        ltl.Text = FormatActors(row["CurrentActors"].ToString());
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        Search(listPager.TotalRecordCount, e.NewPageIndex);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        System.Text.StringBuilder sbUrl = new System.Text.StringBuilder();
        sbUrl.Append(rrUrl);

        sbUrl.AppendFormat("?q={0}&wfalias={1}&st={2}&et={3}", textKeywords.Text.Trim(), HttpUtility.UrlEncode(ddlWorkflows.SelectedValue), txtStartDT.Text.Trim(), txtEndDT.Text.Trim());

        Response.Redirect(sbUrl.ToString());

    }

    /// <summary>
    /// 格式化当前操作人字符串.
    /// </summary>
    /// <param name="currentActors"></param>
    /// <returns></returns>
    public static string FormatActors(string currentActors)
    {
        if (string.IsNullOrEmpty(currentActors))
            return string.Empty;
        StringBuilder builder = new StringBuilder();
        string[] actors = currentActors.Split(',', '，');
        foreach (string item in actors)
        {
            int index = item.LastIndexOf('/');
            builder.AppendFormat(",{0}", (index > -1 && index < item.Length - 2) ? item.Substring(index + 1) : item);
        }
        if (builder.Length > 0)
            builder = builder.Remove(0, 1);
        return builder.ToString();
    }

    protected object GetWfNameByWfalias(string wfalias)
    {
        string sql = string.Format(@"select top 1 WorkflowName from bwwf_workflows where workflowname in
(select WorkflowName from bwwf_WorkflowSettings
where workflowalias='{0}') and iscurrent=1 and IsDeleted=0", wfalias);
        object wfName = SqlHelper.ExecuteScalar(CommandType.Text, sql);
        return wfName;
    }
    #endregion
}
