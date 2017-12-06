using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Extension.Util;

public partial class contrib_mobile_pages_Todo : Botwave.XQP.Web.Security.PageBase

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
        if (workflows != null && workflows.Count > 0)
        {
            foreach (WorkflowDefinition definition in workflows)
            {
                string name = definition.WorkflowName;
                string alias = definition.WorkflowAlias;
                ddlWorkflows.Items.Add(new ListItem((string.IsNullOrEmpty(alias) ? string.Empty : (alias + "-")) + name, name));
            }
        }
        this.ddlWorkflows.SelectedValue = Request.QueryString["wfname"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindWorkflowData();
        }
    }
}