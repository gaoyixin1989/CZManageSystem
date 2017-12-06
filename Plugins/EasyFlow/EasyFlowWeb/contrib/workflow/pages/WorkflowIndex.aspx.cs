using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Web;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.Util;

public partial class contrib_workflow_pages_WorkflowIndex : Botwave.Security.Web.PageBase
{
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IWorkflowSettingService workflowSettingService = (IWorkflowSettingService)Ctx.GetObject("workflowSettingService");
    private IWorkflowResourceService workflowResourceService = (IWorkflowResourceService)Ctx.GetObject("workflowResourceService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        get { return workflowDefinitionService; }
        set { workflowDefinitionService = value; }
    }

    public IWorkflowSettingService WorkflowSettingService
    {
        get { return workflowSettingService; }
        set { workflowSettingService = value; }
    }

    public IWorkflowResourceService WorkflowResourceService
    {
        get { return workflowResourceService; }
        set { workflowResourceService = value; }
    }

    #region properties

    public Guid WorkflowId
    {
        get { return (Guid)(ViewState["WorkflowId"]); }
        set { ViewState["WorkflowId"] = value; }
    }

    public string WorkflowName
    {
        get { return (string)(ViewState["WorkflowName"]); }
        set { ViewState["WorkflowName"] = value; }
    }

    public string WorkflowImageUrl
    {
        get { return (string)ViewState["WorkflowImageUrl"]; }
        set { ViewState["WorkflowImageUrl"] = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //string wid = Request.QueryString["wid"];
            //string workflowAlias = Request.QueryString["workflow"];
            string wid = Server.HtmlEncode(Request.QueryString["wid"]);
            string workflowAlias = Server.HtmlEncode(Request.QueryString["workflow"]);
            if (string.IsNullOrEmpty(wid) && string.IsNullOrEmpty(workflowAlias))
                ShowError(MessageHelper.Message_ArgumentException);

            Guid workflowId = Guid.Empty;
            WorkflowDefinition definition = null;
            if (!string.IsNullOrEmpty(wid))
            {
                workflowId = new Guid(wid);
                definition = workflowDefinitionService.GetWorkflowDefinition(workflowId);
                if (definition == null || definition.IsCurrent == false)
                {
                    definition = workflowDefinitionService.GetCurrentWorkflowDefinition(workflowId);
                    if (definition != null)
                        workflowId = definition.WorkflowId;
                }
            }
            else if (!string.IsNullOrEmpty(workflowAlias))
            {
                definition = workflowSettingService.GetCurrentWorkflowDefinition(workflowAlias);
                if (definition != null)
                    workflowId = definition.WorkflowId;
            }

            if(definition == null)
                ShowError(MessageHelper.Message_ArgumentException + "，未找到指定流程定义。");

            this.WorkflowId = workflowId;
            this.WorkflowImageUrl = string.Format("WorkflowImage.ashx?wid={0}", workflowId);

            this.BindWorkflow(definition);
        }
    }

    protected void BindWorkflow(WorkflowDefinition definition)
    {
        ltlRemark.Text = string.IsNullOrEmpty(definition.Remark) ? "略" : WebUtils.HtmlEncode(definition.Remark);
        string workflowName = definition.WorkflowName;

        this.Title = workflowName;
        ltlTitle.Text = workflowName;
        this.WorkflowName = workflowName;

        this.TaskList1.WorkflowId = definition.WorkflowId;
        this.TaskList1.WorkflowName = workflowName;
        this.TaskList1.DataBind();

        // 检查流程权限.
        if (!VerifyWorkflowResource(workflowName))
            ShowError(MessageHelper.Message_NoPermission);
        this.PopupNotice(workflowName);    
    }

    /// <summary>
    /// 检查流程访问权限.
    /// </summary>
    /// <param name="workflowName"></param>
    /// <returns></returns>
    protected bool VerifyWorkflowResource(string workflowName)
    {
        IDictionary<string, string> ownerResources = CurrentUser.Resources;
        string resourceId = workflowResourceService.GetWorkflowResourceId(workflowName, "流程协作", false);
        if (string.IsNullOrEmpty(resourceId) || ownerResources.ContainsKey(resourceId))
            return true;
        return false;
    }

    /// <summary>
    /// 弹出公告窗口.
    /// </summary>
    /// <param name="workflowName"></param>
    protected void PopupNotice(string workflowName)
    {

    }
}
