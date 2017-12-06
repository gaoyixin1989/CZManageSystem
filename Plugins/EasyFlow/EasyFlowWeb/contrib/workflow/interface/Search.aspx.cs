using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Web;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.Util;

public partial class contrib_workflow_interface_Search : Botwave.Security.Web.PageBase
{
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IWorkflowSearcher workflowSearcher = (IWorkflowSearcher)Ctx.GetObject("workflowSearcher");
    static string wfalias = "";
    private string advanceResource = "A021";

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public IWorkflowSearcher WorkflowSearcher
    {
        set { workflowSearcher = value; }
    }

    /// <summary>
    /// 高级权限资源.
    /// </summary>
    public string AdvanceResource
    {
        get { return advanceResource; }
        set { advanceResource = value; }
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
    public bool EnableAdvance
    {
        get { return (ViewState["EnableAdvance"] == null ? false : (bool)ViewState["EnableAdvance"]); }
        set { ViewState["EnableAdvance"] = value; }
    }

    public string UserName
    {
        get { return (ViewState["UserName"] == null ? string.Empty : (string)ViewState["UserName"]); }
        set { ViewState["UserName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LoadData();
        }
    }

    #region

    protected bool HasAdvanceSearch(Botwave.Security.LoginUser user)
    {
        if (user == null)
            return false;
        if (string.IsNullOrEmpty(this.advanceResource))
            return true;

        bool result = false;
        if (user.Properties.ContainsKey("Search_AdvanceSearch_Enable"))
        {
            result = Convert.ToBoolean(user.Properties["Search_AdvanceSearch_Enable"]);
        }
        else
        {
            result = user.Resources.ContainsKey(this.advanceResource);
            user.Properties["Search_AdvanceSearch_Enable"] = result;
        }
        return result;
    }

    #endregion

    private void LoadData()
    {
        Botwave.Security.LoginUser user = CurrentUser;

        this.UserName = user.UserName;
        bool isAdvance = this.HasAdvanceSearch(user);
        if (isAdvance == false)
        {
            this.ltlSearchName.Text = " - 我的工单";
        }
        this.EnableAdvance = isAdvance;
        this.LoadWorkflowData();

        string bt = Request.QueryString["bt"];

            string workflowName = null;
           
            string activityName = null;
            this.divResults.Visible = true;

            dtpStart1.Text = Request.QueryString["bt"];
            dtpStart2.Text = Request.QueryString["et"];
            if (!string.IsNullOrEmpty(Request.QueryString["wfalias"]))
            {
                wfalias = Request.QueryString["wfalias"];
                workflowName = GetWfNameByWfalias(wfalias).ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["aname"]))
                activityName = Request.QueryString["aname"];
            txtCreator.Text = Request.QueryString["c"];
            txtActor.Text = Request.QueryString["a"];
            txtTitleKeywords.Text = Request.QueryString["tk"];
            txtContentKeywords.Text = Request.QueryString["ck"];
            txtWorkId.Text = Request.QueryString["i"];

            // 绑定流程列表和步骤列表
            if (workflowName != null)
            {
                this.LoadActivityData(workflowName.ToString());
                this.ddlActivityList.SelectedValue = activityName;
            }
            this.Search(0, 0);
        
    }

    private void LoadWorkflowData()
    {
        IList < WorkflowDefinition > workflows = GetAllowedWorkflows("0000", "0001", "0002", "0003", "0004");
        ddlWorkflowList.Items.Clear();
        ddlWorkflowList.Items.Insert(0, new ListItem("－ 全部 －", ""));
        if (workflows != null && workflows.Count > 0)
        {
            foreach (WorkflowDefinition definition in workflows)
            {
                string name = definition.WorkflowName;
                string alias = definition.WorkflowAlias;
                ddlWorkflowList.Items.Add(new ListItem((string.IsNullOrEmpty(alias) ? string.Empty : (alias + "-")) + name, name));
            }
        }
        this.ddlWorkflowList.SelectedValue = Request.QueryString["wfalias"];
    }

    private IList<WorkflowDefinition> GetAllowedWorkflows(params string[] postfixs)
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
        //if (workflows != null && workflows.Count > 0 
        //    && postfixs != null && postfixs.Length > 0)
            //workflows = WorkflowUtility.GetAllowedWorkflows(workflows, CurrentUser.Resources, postfixs);
        return workflows;
    }

    private void LoadActivityData(string workflowName)
    {
        if (string.IsNullOrEmpty(workflowName))
        {
            ddlActivityList.Items.Clear();
        }
        else
        {
            IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionListByName(workflowName);
            if (workflows != null && workflows.Count > 0)
            {
                Guid workflowId = workflows[0].WorkflowId;
                ddlActivityList.DataSource = activityDefinitionService.GetActivitiesByWorkflowId(workflowId);
                ddlActivityList.DataTextField = "ActivityName";
                ddlActivityList.DataValueField = "ActivityName";
            }
        }
        ddlActivityList.DataBind();
        ddlActivityList.Items.Insert(0, new ListItem("－ 全部 －", ""));
    }

    private void Search(int recordCount, int pageIndex)
    {
        string  wfalia = Request.QueryString["wfalias"];
        string createdBeginTime = dtpStart1.Text.Trim();
        string createdEndTime = dtpStart2.Text.Trim();
        string workflowName = GetWfNameByWfalias(wfalia).ToString();
        string activityName = ddlActivityList.SelectedValue;
        string creator = txtCreator.Text.Trim();
        string processor = txtActor.Text.Trim();
        string titleKeywords = txtTitleKeywords.Text.Trim();
        string contentKeywords = txtContentKeywords.Text.Trim();
        string workId = txtWorkId.Text.Trim();

        AdvancedSearchCondition condition = new AdvancedSearchCondition();
        condition.BeginTime = createdBeginTime;
        condition.EndTime = createdEndTime;
        condition.WorkflowName = workflowName;
        condition.ActivityName = activityName;
        condition.CreatorName = creator;
        condition.ProcessorName = processor;
        condition.SheetId = workId;
        condition.Keywords = contentKeywords;
        condition.Title = titleKeywords;
        condition.OrderField = FieldOrder;
        
        bool enableAdvance = this.EnableAdvance;
        if (!enableAdvance) // 有限制的查询.
        {
            condition.Workflows = new List<string>();
            IList<WorkflowDefinition> items = GetAllowedWorkflows("0004");
            if (items != null && items.Count > 0)
            {
                if (string.IsNullOrEmpty(workflowName))
                {
                    foreach (WorkflowDefinition item in items)
                        condition.Workflows.Add(item.WorkflowName);
                }
                else
                {
                    foreach (WorkflowDefinition item in items)
                    {
                        if (item.WorkflowName.Equals(workflowName, StringComparison.OrdinalIgnoreCase))
                        {
                            condition.Workflows.Add(workflowName);
                            break;
                        }
                    }
                }
            }
        }
        condition.Actor = this.UserName;   // 当前查询用户
        condition.IsProcessed = (enableAdvance == false);

        this.listResults.DataSource = workflowSearcher.Search(condition, pageIndex, listPager.ItemsPerPage, ref recordCount);
        this.listResults.DataBind();
        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
    }

    #region Event Methods

    protected void listResults_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemIndex < 0)
            return;
        DataRowView item = e.Item.DataItem as DataRowView;

        string aliasImage = DbUtils.ToString(item["AliasImage"]); // 别名
        if (!string.IsNullOrEmpty(aliasImage))
        {
            string workflowAlias = DbUtils.ToString(item["WorkflowAlias"]);
            Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"../res/groups/{1}\" />", workflowAlias, aliasImage);
        }
    }

    protected void listResults_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            if (ViewState[e.CommandName.Trim()] == null)
            {
                ViewState[e.CommandName.Trim()] = "asc";
            }
            else
            {
                if (ViewState[e.CommandName.Trim()].ToString().Trim() == "asc")
                {
                    ViewState[e.CommandName.Trim()] = "desc";
                }
                else
                {
                    ViewState[e.CommandName.Trim()] = "asc";
                }
            }

            FieldOrder = e.CommandName.ToString().Trim() + " " + ViewState[e.CommandName.Trim()].ToString().Trim();
            this.Search(RecordCount, PageIndex);
        }
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        RecordCount = listPager.TotalRecordCount;
        PageIndex = e.NewPageIndex;
        this.Search(listPager.TotalRecordCount, e.NewPageIndex);
    }

    protected void ddlWorkflowList_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadActivityData(this.ddlWorkflowList.SelectedValue);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        
        string workflowname = GetWfNameByWfalias(wfalias.ToString()).ToString();
        Response.Redirect(string.Format("Search.aspx?bt={0}&et={1}&wfalias={2}&aname={3}&c={4}&a={5}&tk={6}&ck={7}&i={8}",
            dtpStart1.Text,
            dtpStart2.Text,
            wfalias,
            HttpUtility.UrlEncode(ddlActivityList.SelectedValue),
            HttpUtility.UrlEncode(txtCreator.Text),
            HttpUtility.UrlEncode(txtActor.Text),
            HttpUtility.UrlEncode(txtTitleKeywords.Text),
            HttpUtility.UrlEncode(txtContentKeywords.Text),
            txtWorkId.Text));
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
