using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Workflow.Service;
using Botwave.XQP.Service;
using Botwave.Workflow.Domain;
using Botwave.XQP.Domain;
using System.Text;
using Botwave.Web;
using System.Data.SqlClient;

public partial class apps_xqp2_pages_workflows_designer_Flowdesign_ModalDialog_WorkflowProfile : Botwave.Security.Web.UserControlBase
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
    protected void Page_Load(object sender, EventArgs e)
    {
        WebUtils.RegisterCSSReference(this.Page, AppPath + "App_Themes/gmcc/div_sh.css", "style");
        WebUtils.RegisterCSSReference(this.Page, AppPath + "App_Themes/gmcc/zTreeStyle.css","style");
        WebUtils.RegisterCSSReference(this.Page, AppPath + "App_Themes/gmcc/style.css", "style");
        WebUtils.RegisterCSSReference(this.Page, AppPath + "App_Themes/gmcc/default/style.css", "style");
        
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/common.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery-1.7.2.min.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/HideFieldJs.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery_custom.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Frienddetail.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Base64.js");
    }

    #region load

    public void LoadWorkflow(WorkflowSetting setting)
    {
        string workflowName = setting.WorkflowName;
        this.ltlWorkflowName.Text = workflowName;

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
        SqlParameter[] param={new SqlParameter("@workflowid",this.WorkflowId)};
        object remark=Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteScalar(System.Data.CommandType.Text,"select remark from bwwf_workflows where workflowid=@workflowid",param);
        txtRemark.Value = Botwave.Commons.DbUtils.ToString(remark);

    }
    #endregion
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
        return item;
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

    private static int ToInt32(string inputValue)
    {
        if (string.IsNullOrEmpty(inputValue))
            return -1;
        int result = -1;
        if (int.TryParse(inputValue, out result))
            return result;
        return -1;
    }

    private void SaveWorkflowProfile()
    {
        WorkflowProfile wprofile = GetWorkflowProfileData();
        wprofile.Insert();
        IList<Guid> userIds = new List<Guid>();
        if (!string.IsNullOrEmpty(hidManager.Value))
        {
            string[] ids = hidManager.Value.Replace("'", "").Split(',');

            foreach (string id in ids)
            {
                userIds.Add(new Guid(id));
            }

        }
        string result = workflowRoleService.InsertWorkflowManager(ResourceId, userIds, CurrentUserName);
        //WriteNomalLog(CurrentUserName, "部署流程", "配置流程管理员：" + result);
    }

    #endregion
}