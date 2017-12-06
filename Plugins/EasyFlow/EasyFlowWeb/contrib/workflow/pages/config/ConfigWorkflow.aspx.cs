using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Web;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;

public partial class contrib_workflow_pages_config_ConfigWorkflow : Botwave.Security.Web.PageBase
{
    static readonly string ImageYes = string.Format("<img title=\"是\" src=\"{0}res/img/ico_yes.gif\" />", AppPath);
    static readonly string ImageNo = string.Format("<img title=\"否\" src=\"{0}res/img/ico_no.gif\" />", AppPath);
    public string AliasImage;

    #region service interfaces

    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IWorkflowSettingService workflowSettingService = (IWorkflowSettingService)Ctx.GetObject("workflowSettingService");

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public IWorkflowSettingService WorkflowSettingService
    {
        set { workflowSettingService = value; }
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
            WorkflowSetting setting = workflowSettingService.GetWorkflowSetting(workflowId);
            if (setting == null)
            {
                setting = WorkflowSetting.Default;
                //ShowError("未找到指定流程设置.");
            }
            this.LoadWorkflow(setting);
            this.LoadActivities(workflowId);
            this.WorkflowId = workflowId;            
        }
    }

    #region load

    protected void LoadWorkflow(WorkflowSetting setting)
    {
        string workflowName = setting.WorkflowName;
        this.ltlTitle.Text = workflowName;
        this.ltlWorkflowName.Text = workflowName;
        this.WorkflowName = workflowName;

        // 别名
        this.txtAlias.Text = setting.WorkflowAlias;
        string aliasImage = setting.AliasImage;
        this.hiddenAliasImage.Value = aliasImage;
        this.AliasImage = string.IsNullOrEmpty(aliasImage) ? "(无图片)" : string.Format("<img src=\"../../res/groups/{0}\" />", aliasImage);

        // 基本字段
        this.chkboxExpectFinishTime.Checked = setting.HasBasicField(WorkflowSetting.BasicFieldType.ExpectFinishedTime);
        this.chkboxSecrecy.Checked = setting.HasBasicField(WorkflowSetting.BasicFieldType.Secrecy);

        bool hasUrgency = setting.HasBasicField(WorkflowSetting.BasicFieldType.Urgency);
        bool hasImportance = setting.HasBasicField(WorkflowSetting.BasicFieldType.Importance);
        this.chkboxUrgency.Checked = hasUrgency;
        this.chkboxImportance.Checked = hasImportance;

        this.txtMaxUndone.Text = setting.UndoneMaxCount.ToString();
        this.txtMinNotifyTaskCount.Text = setting.TaskNotifyMinCount.ToString();
    }

    protected void LoadActivities(Guid workflowId)
    {
        IList<ActivityDefinition> activities = activityDefinitionService.GetActivitiesByWorkflowId(workflowId);
        // 移除初始步骤与完成步骤


        for (int i = 0; i < activities.Count; i++)
        {
            ActivityDefinition item = activities[i];
            if (item.PrevActivitySetId == Guid.Empty || item.NextActivitySetId == Guid.Empty)
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
        string minNotifyTaskCount = this.txtMinNotifyTaskCount.Text.Trim();
        string maxUndone = this.txtMaxUndone.Text.Trim();

        WorkflowSetting setting = new WorkflowSetting();
        setting.WorkflowName = this.WorkflowName;
        setting.BasicFields = GetBasicFields();
        setting.WorkflowAlias = txtAlias.Text.Trim();
        setting.AliasImage = hiddenAliasImage.Value.Trim();
        setting.TaskNotifyMinCount = ToInt32(minNotifyTaskCount);
        setting.UndoneMaxCount = ToInt32(maxUndone);

        if (workflowSettingService.UpdateSetting(setting) >= 1)
        {
            ShowSuccess("更新流程设置成功.");
        }
        else
        {
            ShowError("更新流程设置失败.");
        }
    }

    public string GetBasicFields()
    {
        string basicFields = string.Empty;
        basicFields += (this.chkboxExpectFinishTime.Checked ? "1" : "0");
        basicFields += (this.chkboxSecrecy.Checked ? "1" : "0");
        basicFields += (this.chkboxUrgency.Checked ? "1" : "0");
        basicFields += (this.chkboxImportance.Checked ? "1" : "0");
        return basicFields;
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
        Literal ltlItemProcessor = e.Item.FindControl("ltlItemProcessor") as Literal;
        Literal ltlItemStarter = e.Item.FindControl("ltlItemStarter") as Literal;

        ltlItemField.Text = (extendAllocators.Contains("field") ? ImageYes : ImageNo);
        ltlItemUsers.Text = (string.IsNullOrEmpty(users) ? ImageNo : ImageYes);
        ltlItemSuperior.Text = (extendAllocators.Contains("superior") ? ImageYes : ImageNo);
        ltlItemResource.Text = ((string.IsNullOrEmpty(resouce) || resouce.StartsWith(ResourceHelper.PrefixDisableResource, StringComparison.OrdinalIgnoreCase)) ? ImageNo : ImageYes);
        ltlItemProcessor.Text = (extendAllocators.Contains("processor") ? ImageYes : ImageNo);
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
