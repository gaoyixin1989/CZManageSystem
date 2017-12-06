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
using Botwave.XQP.Service;

public partial class apps_xqp2_pages_workflows_maintenance_transfertask : Botwave.Security.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_xqp2_pages_workflows_maintenance_transfertask));
    //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_workflow_pages_Search));
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IWorkflowSearcher workflowSearcher = (IWorkflowSearcher)Ctx.GetObject("workflowSearcher");
    private IWorkflowMaintenanceService workflowMaintenanceService = (IWorkflowMaintenanceService)Ctx.GetObject("workflowMaintenanceService");
    private string advanceResource = "A007";

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

    public IWorkflowMaintenanceService WorkflowMaintenanceService
    {
        set { workflowMaintenanceService = value; }
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

    public string Workflows
    {
        get { return (ViewState["Workflows"] == null ? string.Empty : (string)ViewState["Workflows"]); }
        set { ViewState["Workflows"] = value; }
    }

    public int Type
    {
        get { return (int)ViewState["Type"]; }
        set { ViewState["Type"] = value; }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/common.js");
        //WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery-latest.pack.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery-1.7.2.min.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/HideFieldJs.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery_custom.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Frienddetail.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Base64.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery.showloading.js");
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
        if (bt == null)
        {
            DateTime now = DateTime.Now;
            dtpStart1.Text = now.AddMonths(-1).ToString("yyyy-MM-dd");
            dtpStart2.Text = now.AddDays(1).ToString("yyyy-MM-dd");
        }
        else
        {
            string workflowName = null;
            string activityName = null;
            this.divResults.Visible = true;

            //dtpStart1.Text = Request.QueryString["bt"];
            //dtpStart2.Text = Request.QueryString["et"];
            //if (!string.IsNullOrEmpty(Request.QueryString["wname"]))
            //    workflowName = Request.QueryString["wname"];
            //if (!string.IsNullOrEmpty(Request.QueryString["aname"]))
            //    activityName = Request.QueryString["aname"];
            //txtToUser.Text = Request.QueryString["c"];
            //txtFromUser.Text = Request.QueryString["a"];
            //txtTitleKeywords.Text = Request.QueryString["tk"];
            //txtContentKeywords.Text = Request.QueryString["ck"];
            //txtWorkId.Text = Request.QueryString["i"];

            // 绑定流程列表和步骤列表
            if (workflowName != null)
            {
                this.LoadActivityData(workflowName.ToString());
                this.ddlActivityList.SelectedValue = activityName;
            }
        }
    }

    private void LoadWorkflowData()
    {
        IList < WorkflowDefinition > workflows = GetAllowedWorkflows("0005");
        //IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
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
        this.ddlWorkflowList.SelectedValue = Request.QueryString["wname"];
    }

    private IList<WorkflowDefinition> GetAllowedWorkflows(params string[] postfixs)
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
         IList<WorkflowDefinition> allowWorkflows=new List<WorkflowDefinition>();
        if (!this.EnableAdvance)
        {
            if (workflows != null && workflows.Count > 0
                && postfixs != null && postfixs.Length > 0)
            {
                foreach(string postfix in postfixs){
                    IList<WorkflowDefinition> tmpAllowWorkflows=WorkflowUtility.GetAllowedWorkflows(workflows, CurrentUser.Resources, postfix);
                    foreach (WorkflowDefinition tmpAllowWorkflow in tmpAllowWorkflows)
                    {
                        if (!allowWorkflows.Contains(tmpAllowWorkflow))
                            allowWorkflows.Add(tmpAllowWorkflow);
                    }
                }
                StringBuilder sb = new StringBuilder();
                foreach (WorkflowDefinition item in allowWorkflows)
                {
                    sb.Append(item.WorkflowName + ",");
                }
                if (sb.Length > 0)
                {
                    sb = sb.Remove(sb.Length - 1, 1);
                    Workflows = sb.ToString();
                }
            }
        }
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

    private void Search(int recordCount, int pageIndex, int type)
    {
        Type = type;
        string createdBeginTime = dtpStart1.Text.Trim();
        string createdEndTime = dtpStart2.Text.Trim();
        string workflowName = ddlWorkflowList.SelectedValue;
        string activityName = ddlActivityList.SelectedValue;
        string actor = string.Empty;
        string processor = string.Empty;
        string titleKeywords = txtTitleKeywords.Text.Trim();
        string contentKeywords = txtContentKeywords.Text.Trim();
        string workId = txtWorkId.Text.Trim();

        Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition = new Botwave.Workflow.Extension.Service.AdvancedSearchCondition();
        condition.BeginTime = createdBeginTime;
        condition.EndTime = createdEndTime;
        condition.WorkflowName = workflowName;
        condition.ActivityName = activityName;
        if(type==1)//待办
            actor=hidFromUser.Value.Trim();
        else if(type==2)//已办
            processor=hidFromUser.Value.Trim();
        condition.SheetId = workId;
        condition.Keywords = contentKeywords;
        condition.Title = titleKeywords;
        condition.OrderField = FieldOrder;
        //condition.Workflows

        bool enableAdvance = this.EnableAdvance;
        if (!enableAdvance) // 有限制的查询.
        {
            condition.Workflows = new List<string>();
            IList<WorkflowDefinition> items = GetAllowedWorkflows("0005");
            //log.Info(items.Count);
            if (items != null && items.Count > 0)
            {
                if (string.IsNullOrEmpty(workflowName))
                {
                    foreach (WorkflowDefinition item in items)
                    {
                        if (!condition.Workflows.Contains(item.WorkflowName))
                        {
                            condition.Workflows.Add(item.WorkflowName);
                        }
                    }
                }
                else
                {
                    foreach (WorkflowDefinition item in items)
                    {
                        if (item.WorkflowName.Equals(workflowName, StringComparison.OrdinalIgnoreCase) && !condition.Workflows.Contains(item.WorkflowName))
                        {
                            condition.Workflows.Add(workflowName);
                            break;
                        }
                    }
                }
            }
        }

        this.listResults.DataSource = workflowMaintenanceService.SearchTasks(condition,actor,processor, pageIndex, listPager.ItemsPerPage, ref recordCount);
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
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{2}contrib/workflow/res/groups/{1}\" />", workflowAlias, aliasImage,AppPath);
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
            //this.Search(RecordCount, PageIndex);
        }
        else if (e.CommandName.Equals("delete"))
        {
            List<string> list = new List<string>();
            list.Add(e.CommandArgument.ToString());
            workflowMaintenanceService.DeleteWorkflowInstance(list);
            string  msg = workflowMaintenanceService.DeleteWorkflowInstance(list);
            WriteNomalLog(CurrentUserName, "删除工单", "成功删除工单ID为[" + e.CommandArgument.ToString() + "]的工单.");
        if (msg.StartsWith("成功")) 
            ShowSuccess(msg,this.Request.RawUrl);
        else
            ShowError(msg);
        }
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        RecordCount = listPager.TotalRecordCount;
        PageIndex = e.NewPageIndex;
        this.Search(listPager.TotalRecordCount, e.NewPageIndex, Type);
    }

    protected void ddlWorkflowList_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadActivityData(this.ddlWorkflowList.SelectedValue);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        btnTransferTodo.Visible = true;
        btnTransferDone.Visible = false;
        Search(0,0,1);
    }

    protected void btnSearchDone_Click(object sender, EventArgs e)
    {
        btnTransferDone.Visible = true;
        btnTransferTodo.Visible = false;
        Search(0, 0, 2);
    }

    protected void btnTransferTodo_Click(object sender, EventArgs e)
    {
        string workflowinstanceids=Request.Form["chk"];
        if (!string.IsNullOrEmpty(workflowinstanceids))
        {
            IList<Guid> workflowInstanceiIds = new List<Guid>();
            string[] wiids = workflowinstanceids.Split(',');
            foreach (string wiid in wiids)
            {
                workflowInstanceiIds.Add(new Guid(wiid));

            }
            string toUser = DbUtils.FilterSQL(hidToUser.Value.Trim());
            string result = workflowMaintenanceService.TransferTodoTask(workflowInstanceiIds,hidFromUser.Value, toUser);
            WriteNomalLog(CurrentUserName,"转移待办",result);
            ShowSuccess(result,Request.RawUrl);
        }
    }

    protected void btnTransferDone_Click(object sender, EventArgs e)
    {
        string workflowinstanceids = Request.Form["chk"];
        string fromusers = hidFromUser.Value;
        if (!string.IsNullOrEmpty(workflowinstanceids))
        {
            IList<Guid> workflowInstanceIds = new List<Guid>();
            string[] wiids = workflowinstanceids.Split(',');
            foreach (string wiid in wiids)
            {
                workflowInstanceIds.Add(new Guid(wiid));

            }
            string toUser = DbUtils.FilterSQL(hidToUser.Value.Trim());
            string result = workflowMaintenanceService.TransferDoneTask(workflowInstanceIds, fromusers, toUser);
            WriteNomalLog(CurrentUserName, "转移已办", result);
            ShowSuccess(result, Request.RawUrl);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string ids = Request.Form["chk"];
        int count = 0;string msg = "";
        if (ids.Length > 0)
        {
            List<string> list = new List<string>();
            foreach (string id in ids.Split(','))
            {
                list.Add(id);
            }
            msg = workflowMaintenanceService.DeleteWorkflowInstance(list);
            WriteNomalLog(CurrentUserName, "删除工单", msg);
        }
        if (msg.StartsWith("成功")) 
            ShowSuccess(msg,this.Request.RawUrl);
        else
            ShowError(msg);
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

    #endregion
    
}
