using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave;
using Botwave.Web;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Domain;

public partial class xqp2_contrib_workflow_pages_config_ConfigHistoryWorkflow : Botwave.Security.Web.PageBase
{
    static readonly string AliasImageFormat = "<img src=\"" + AppPath + "contrib/workflow/res/groups/{0}\" />";
    static readonly string ImageYes = string.Format("<img title=\"是\" src=\"{0}res/img/ico_yes.gif\" />", AppPath);
    static readonly string ImageNo = string.Format("<img title=\"否\" src=\"{0}res/img/ico_no.gif\" />", AppPath);
    public string AliasImage;


    #region service interfaces

    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        get { return workflowDefinitionService; }
        set { workflowDefinitionService = value; }
    }
    #endregion

    #region properties

    public Guid WorkflowId
    {
        get { return (Guid)ViewState["WorkflowId"]; }
        set { ViewState["WorkflowId"] = value; }
    }

    public string WorkflowName
    {
        get { return (string)ViewState["WorkflowName"]; }
        set { ViewState["WorkflowName"] = value; }
    }
    #endregion 

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wfid = Request.QueryString["wid"];
            if (string.IsNullOrEmpty(wfid))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }
            Guid workflowId = new Guid(wfid);
            WorkflowDefinition definition = workflowDefinitionService.GetWorkflowDefinition(workflowId);

            if (definition == null)
            {
                ShowError("未找到指定流程定义.");
            }
            this.LoadWorkflow(definition);
            this.LoadActivities(workflowId);
            this.WorkflowId = workflowId;
        }
    }

    #region load

    protected void LoadWorkflow(WorkflowDefinition defindtion)
    {
        string workflowName = defindtion.WorkflowName;
        this.ltlTitle.Text = workflowName;
        this.ltlWorkflowName.Text = workflowName;
        this.WorkflowName = workflowName;

        // 别名
        txtRemark.Text = defindtion.Remark;
    }

    protected void LoadActivities(Guid workflowId)
    {
        IList<ActivityDefinition> activities = activityDefinitionService.GetActivitiesByWorkflowId(workflowId);
        // 移除初始步骤与完成步骤

        for (int i = 0; i < activities.Count; i++)
        {
            ActivityDefinition item = activities[i];
            //if (item.PrevActivitySetId == Guid.Empty || item.NextActivitySetId == Guid.Empty)
            //{
            //    activities.RemoveAt(i);
            //    i--;
            //}
            if (item.NextActivitySetId.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                activities.RemoveAt(i);
                i--;
            }
        }
        this.rptActivities.DataSource = activities;
        this.rptActivities.DataBind();
    }

    #endregion

    protected void btnSave_Click(object sender, EventArgs e)
    {

        WorkflowDefinition definition = workflowDefinitionService.GetWorkflowDefinition(WorkflowId);
        definition.Remark = txtRemark.Text;
        if (Botwave.XQP.Domain.CZWorkflowDefinition.UpdateWorklowRemark(definition) >= 1)
        {
            ShowSuccess("更新流程设置成功.");
        }
        else
        {
            ShowError("更新流程设置失败.");
        }
    }

    protected void rptActivities_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        // starter,processor,superior,field
        ActivityDefinition dataItem = e.Item.DataItem as ActivityDefinition;
        string users = dataItem.AllocatorUsers;
        string resouce = dataItem.AllocatorResource;
        string extendAllocators = dataItem.ExtendAllocators;
        extendAllocators = (extendAllocators == null ? string.Empty : extendAllocators);

        Literal ltlItemField = e.Item.FindControl("ltlItemField") as Literal;
        Literal ltlItemUsers = e.Item.FindControl("ltlItemUsers") as Literal;
        Literal ltlItemSuperior = e.Item.FindControl("ltlItemSuperior") as Literal;
        Literal ltlItemResource = e.Item.FindControl("ltlItemResource") as Literal;
        Literal ltlItemRole = e.Item.FindControl("ltlItemRole") as Literal;
        Literal ltlItemControl = e.Item.FindControl("ltlItemControl") as Literal;
        Literal ltlItemProcessor = e.Item.FindControl("ltlItemProcessor") as Literal;
        Literal ltlItemStarter = e.Item.FindControl("ltlItemStarter") as Literal;
        Literal ltlItemProcessctl = e.Item.FindControl("ltlItemProcessctl") as Literal;

        ltlItemField.Text = (extendAllocators.Contains("field") ? ImageYes : ImageNo);
        ltlItemUsers.Text = (string.IsNullOrEmpty(users) ? ImageNo : ImageYes);
        ltlItemSuperior.Text = (extendAllocators.Contains("superior") ? ImageYes : ImageNo);
        ltlItemResource.Text = ((string.IsNullOrEmpty(resouce) || resouce.StartsWith(ResourceHelper.PrefixDisableResource, StringComparison.OrdinalIgnoreCase)) ? ImageNo : ImageYes);
        ltlItemRole.Text = (extendAllocators.Contains("role") ? ImageYes : ImageNo);
        ltlItemControl.Text = (extendAllocators.Contains("activity") ? ImageYes : ImageNo);
        ltlItemProcessor.Text = (extendAllocators.Contains("processor") ? ImageYes : ImageNo);
        ltlItemProcessctl.Text = (extendAllocators.Contains("processctl") ? ImageYes : ImageNo);
        ltlItemStarter.Text = (extendAllocators.Contains("starter") ? ImageYes : ImageNo);
    }

    private static int ToInt32(string inputValue)
    {
        if (string.IsNullOrEmpty(inputValue))
            return -1;
        int result = -1;
        if (int.TryParse(inputValue, out result))
            return result;
        return -1;
    }
}
