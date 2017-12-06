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
using Botwave.XQP.Service;
using System.Text;
using Botwave.Security.Service;
using Botwave.Security.Domain;

public partial class xqp2_contrib_workflow_pages_config_ConfigWorkflow : Botwave.Security.Web.PageBase
{
    static readonly string AliasImageFormat = "<img src=\"" + AppPath + "contrib/workflow/res/groups/{0}\" />";
    static readonly string ImageYes = string.Format("<img title=\"是\" src=\"{0}res/img/ico_yes.gif\" />", AppPath);
    static readonly string ImageNo = string.Format("<img title=\"否\" src=\"{0}res/img/ico_no.gif\" />", AppPath);
    public string AliasImage;


    #region service interfaces

    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IWorkflowSettingService workflowSettingService = (IWorkflowSettingService)Ctx.GetObject("workflowSettingService");
    private IWorkflowRoleService workflowRoleService = (IWorkflowRoleService)Ctx.GetObject("workflowRoleService");

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public IWorkflowSettingService WorkflowSettingService
    {
        set { workflowSettingService = value; }
    }

    public IWorkflowRoleService WorkflowRoleService
    {
        set { workflowRoleService = value; }
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

    public string ResourceId
    {
        get { return (string)ViewState["ResourceId"]; }
        set { ViewState["ResourceId"] = value; }
    }
    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/common.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery-latest.pack.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery-1.7.2.min.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/HideFieldJs.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery_custom.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Frienddetail.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Base64.js");
    }

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
                ShowError("未找到指定流程设置.流程ID:"+ workflowId.ToString());
            }
            this.LoadWorkflow(setting);
            this.LoadWorkflowProfile(workflowId);
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
        this.AliasImage = string.IsNullOrEmpty(aliasImage) ? "(无图片)" : string.Format(AliasImageFormat, aliasImage);

        // 基本字段
        this.chkboxExpectFinishTime.Checked = setting.HasBasicField(WorkflowSetting.BasicFieldType.ExpectFinishedTime);
        this.chkboxSecrecy.Checked = setting.HasBasicField(WorkflowSetting.BasicFieldType.Secrecy);

        bool hasUrgency = setting.HasBasicField(WorkflowSetting.BasicFieldType.Urgency);
        bool hasImportance = setting.HasBasicField(WorkflowSetting.BasicFieldType.Importance);
        this.chkboxUrgency.Checked = hasUrgency;
        this.chkboxImportance.Checked = hasImportance;

        this.txtMaxUndone.Text = setting.UndoneMaxCount.ToString();
        this.txtMinNotifyTaskCount.Text = setting.TaskNotifyMinCount.ToString();

        this.LoadWorkflowSite(workflowName, setting.WorkflowAlias);
    }

    protected void LoadActivities(Guid workflowId)
    {
        IList<ActivityDefinition> activities = activityDefinitionService.GetActivitiesByWorkflowId(workflowId);
        // 移除初始步骤与完成步骤

        //for (int i = 0; i < activities.Count; i++)
        //{
        //    ActivityDefinition item = activities[i];
        //    if (item.PrevActivitySetId == Guid.Empty || item.NextActivitySetId == Guid.Empty)
        //    {
        //        activities.RemoveAt(i);
        //        i--;
        //    }
        //}
        for (int i = 0; i < activities.Count; i++)
        {
            ActivityDefinition item = activities[i];
            //if (item.PrevActivitySetId == "00000000-0000-0000-0000-000000000000" || item.NextActivitySetId == "00000000-0000-0000-0000-000000000000")
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
        this.SaveWorkflowProfile();

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
            Botwave.XQP.Commons.XQPHelper.PushWorkflowList();
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
        Literal ltlItemRole = e.Item.FindControl("ltlItemRole") as Literal;
        Literal ltlItemControl = e.Item.FindControl("ltlItemControl") as Literal;
        Literal ltlItemProcessor = e.Item.FindControl("ltlItemProcessor") as Literal;
        Literal ltlItemStarter = e.Item.FindControl("ltlItemStarter") as Literal;
        Literal ltlItemProcessctl = e.Item.FindControl("ltlItemProcessctl") as Literal;

        ltlItemField.Text = (extendAllocators.Contains("field") ? ImageYes : ImageNo);
        ltlItemUsers.Text = (string.IsNullOrEmpty(users) ? ImageNo : ImageYes);
        ltlItemSuperior.Text = (extendAllocators.Contains("superior") ? ImageYes : ImageNo);
        //ltlItemResource.Text = ((string.IsNullOrEmpty(resouce) || resouce.StartsWith(ResourceHelper.PrefixDisableResource, StringComparison.OrdinalIgnoreCase)) ? ImageNo : ImageYes);
        ltlItemResource.Text = (!extendAllocators.Contains("owner") ? ImageNo : ImageYes);
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

    #region workflow profile

    public void LoadWorkflowProfile(Guid workflowId)
    {
        WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
        if (wprofile == null)
            wprofile = WorkflowProfile.Default;
        this.LoadWorkflowProfile(wprofile);
    }

    public void LoadWorkflowProfile(WorkflowProfile wprofile)
    {
        if (wprofile == null)
            return;

        this.txtSmsNotifyFormat.Text = wprofile.SmsNotifyFormat;
        this.txtEmailNotifyFormat.Text = wprofile.EmailNotifyFormat;
        this.txtStatSmsNotifyFormat.Text = wprofile.StatSmsNodifyFormat;
        this.txtStatEmailNotifyFormat.Text = wprofile.StatEmailNodifyFormat;

        // 普通单发起控制
        this.ddlCreationTypes.SelectedValue = wprofile.CreationControlType;
        this.txtMaxInMonth.Text = wprofile.MaxCreationInMonth.ToString();
        this.txtMaxInWeek.Text = wprofile.MaxCreationInWeek.ToString();

        this.txtWorkflowInstanceTitle.Text = wprofile.WorkflowInstanceTitle;

        this.chkExp.Checked = wprofile.PrintAndExp == 1 || wprofile.PrintAndExp == 0;
        this.chkPrint.Checked = wprofile.PrintAndExp == 2 || wprofile.PrintAndExp == 0;
        this.txtPrintCount.Text = wprofile.PrintAmount.ToString();
        this.txtDepts.Text = wprofile.Depts;
        this.txtManager.Text = wprofile.Manager;
        this.chkIsMobile.Checked = wprofile.IsMobile;
        this.chkboxIsAuto.Checked = wprofile.IsTimeOutContinue;
        this.chkIsToShortDateString.Checked = wprofile.IsToShortDateString;
        //从角色中加载流程管理员
        Botwave.Security.Domain.ResourceInfo manager = workflowRoleService.GetResourceInfoByName(wprofile.WorkflowName + "-流程管理");
       
        IList<Botwave.Security.Domain.UserInfo> managerList = new List<Botwave.Security.Domain.UserInfo>();
        if (manager != null)
        {
            ResourceId = manager.ResourceId;
            managerList = workflowRoleService.GetUsersByResourceId(manager.ResourceId);
            StringBuilder sbManager = new StringBuilder();
            StringBuilder sbManager1 = new StringBuilder();
            foreach (Botwave.Security.Domain.UserInfo userinfo in managerList)
            {
                sbManager.Append(",'" + userinfo.UserId + "'");
                sbManager1.Append("," + userinfo.RealName + "");
            }
            if (sbManager.Length > 1)
                sbManager = sbManager.Remove(0, 1);
            if (sbManager1.Length > 1)
                sbManager1 = sbManager1.Remove(0, 1);
            hidManager.Value = sbManager.ToString();
            txtManager.Text = sbManager1.ToString();
        }
    }

    public void LoadWorkflowSite(string workflowName, string workflowAlias)
    {
        string hostAddress = GlobalSettings.Instance.Address;
        this.lblStartSite.Text = String.Format("{0}ssoproxy/start/{1}.ashx", hostAddress, workflowAlias).ToLower();
        this.lblWorkflowSite.Text = String.Format("{0}ssoproxy/index/{1}.ashx", hostAddress, workflowAlias).ToLower();
        this.lblTodoSite.Text = String.Format("{0}ssoproxy/todo.ashx?name={1}", hostAddress, HttpUtility.UrlEncode(workflowName));
    }

    public WorkflowProfile GetWorkflowProfileData()
    {
        string minNotifyTaskCount = this.txtMinNotifyTaskCount.Text.Trim();
        string smsNotifyFormat = this.txtSmsNotifyFormat.Text;
        string emailNotifyFormat = this.txtEmailNotifyFormat.Text;
        string statSmsNotifyFormat = this.txtStatSmsNotifyFormat.Text;
        string statEmailNotifyFormat = this.txtStatEmailNotifyFormat.Text;
        string creationControlType = this.ddlCreationTypes.SelectedValue;
        string maxInMonth = this.txtMaxInMonth.Text.Trim();
        string maxInWeek = this.txtMaxInWeek.Text.Trim();

        WorkflowProfile item = new WorkflowProfile();

        item.WorkflowName = this.WorkflowName;
        item.BasicFields = GetBasicFields();
        item.MinNotifyTaskCount = Convert.ToInt32(minNotifyTaskCount);
        item.SmsNotifyFormat = smsNotifyFormat;
        item.EmailNotifyFormat = emailNotifyFormat;
        item.StatSmsNodifyFormat = statSmsNotifyFormat;
        item.StatEmailNodifyFormat = statEmailNotifyFormat;
        item.MaxCreationUndone = ToInt32(this.txtMaxUndone.Text.Trim());
        item.CreationControlType = creationControlType;
        item.MaxCreationInMonth = Convert.ToInt32(maxInMonth);
        item.MaxCreationInWeek = Convert.ToInt32(maxInWeek);
        item.WorkflowInstanceTitle = this.txtWorkflowInstanceTitle.Text.Trim();
        if (this.chkPrint.Checked && !this.chkExp.Checked)
            item.PrintAndExp = 2;
        else if (this.chkExp.Checked && !this.chkPrint.Checked)
            item.PrintAndExp = 1;
        else if (this.chkExp.Checked && this.chkPrint.Checked)
            item.PrintAndExp = 0;
        else
            item.PrintAndExp = 3;
        item.PrintAmount = Botwave.Commons.DbUtils.ToInt32(txtPrintCount.Text);

        item.Depts = txtDepts.Text;
        item.Manager = txtManager.Text;
        item.IsMobile = this.chkIsMobile.Checked;
        item.IsTimeOutContinue = this.chkboxIsAuto.Checked;
        item.IsToShortDateString = this.chkIsToShortDateString.Checked;
        return item;
    }

    private void SaveWorkflowProfile()
    {
        WorkflowProfile wprofile = GetWorkflowProfileData();
        wprofile.Insert();
        IList<Guid> userIds = new List<Guid>();
        if (!string.IsNullOrEmpty(hidManager.Value))
        {
            string[] ids=hidManager.Value.Replace("'","").Split(',');
            
            foreach (string id in ids)
            {
                userIds.Add(new Guid(id));
            }
            
        }
        string result = workflowRoleService.InsertWorkflowManager(ResourceId, userIds, CurrentUserName);
        WriteNomalLog(CurrentUserName, "部署流程", "配置流程管理员：" + result);
    }

    #endregion
}
