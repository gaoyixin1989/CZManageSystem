using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Entities;
using Botwave.Security.Service;
using Botwave.Security.Domain;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Domain;

public partial class contrib_workflow_controls_WorkflowWorkItem : Botwave.Security.Web.UserControlBase
{
    private static readonly string[] arrUrgency = { "一般", "紧急", "特别紧急", "最紧急" };
    private static readonly string[] arrImportance = { "一般", "重要", "特别重要" };

    private Guid _activityInstanceId;
    private string basicInfoHtml;
    public string BasicInfoHtml
    {
        get { return basicInfoHtml; }
        set { basicInfoHtml = value; }
    }

    #region service interfaces

    private IWorkflowService workflowService=(IWorkflowService)Ctx.GetObject("workflowService");
    private IWorkflowSettingService workflowSettingService = (IWorkflowSettingService)Ctx.GetObject("workflowSettingService");
    private ITaskAssignService taskAssignService = (ITaskAssignService)Ctx.GetObject("taskAssignService");
    private IWorkflowUserService workflowUserService = (IWorkflowUserService)Ctx.GetObject("workflowUserService");
    private IFormInstanceService formInstanceService = (IFormInstanceService)Ctx.GetObject("formInstanceService");

    public IWorkflowService WorkflowService
    {
        set { workflowService = value; }
    }

    public IWorkflowSettingService WorkflowSettingService
    {
        set { workflowSettingService = value; }
    }

    public ITaskAssignService TaskAssignService
    {
        set { taskAssignService = value; }
    }

    public IWorkflowUserService WorkflowUserService
    {
        set { workflowUserService = value; }
    }
    public IFormInstanceService FormInstanceService
    {
        get { return formInstanceService; }
        set { formInstanceService = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.txtTitle.Attributes["validatetype"] = "require";
            this.txtTitle.Attributes["require"] = "true";
            this.txtTitle.Attributes["msg"] = "标题不能为空";
        }
    }

    public Guid ActivityInstanceId
    {
        get { return _activityInstanceId; }
        set { _activityInstanceId = value; }
    }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(txtTitle.Text.Trim());
    }

    #region 属性


    /// <summary>
    /// 是否只读.
    /// </summary>
    public bool IsReadonly
    {
        get
        {
            if (null == ViewState["IsReadonly"])
                return false;
            return Convert.ToBoolean(ViewState["IsReadonly"]);
        }
        set { ViewState["IsReadonly"] = value; }
    }

    /// <summary>
    /// 保密程度.
    /// </summary>
    public int Secrecy
    {
        get
        {
            if (null == ViewState["Secrecy"])
                return 0;
            return int.Parse(ViewState["Secrecy"].ToString());
        }
        set { ViewState["Secrecy"] = value; }
    }

    /// <summary>
    /// 紧急程度.
    /// </summary>
    public int Urgency
    {
        get
        {
            if (null == ViewState["Urgency"])
                return 0;
            return int.Parse(ViewState["Urgency"].ToString());
        }
        set { ViewState["Urgency"] = value; }
    }

    /// <summary>
    /// 重要级别.
    /// </summary>
    public int Importance
    {
        get
        {
            if (null == ViewState["Importance"])
                return 0;
            return int.Parse(ViewState["Importance"].ToString());
        }
        set { ViewState["Importance"] = value; }
    }

    #endregion

    public void LoadEmptyData(Guid workflowId)
    {
        WorkflowSetting setting = this.workflowSettingService.GetWorkflowSetting(workflowId);
        thSheetId.Visible = false;
        tdSheetId.Visible = false;
        tdTitle.ColSpan = 5;
        string workflowName = setting.WorkflowName;
        if (string.IsNullOrEmpty(workflowName))
            workflowName = WorkflowUtility.GetWorkflowName(workflowId);
        this.BindWorkflowName(workflowName);
        GenBasicInfoHtml(null, setting, false);
    }

    public void LoadData(WorkflowInstance workflowInstance)
    {
        this.LoadData(workflowInstance, String.Empty, Guid.Empty);
    }
    public void LoadData(WorkflowInstance workflowInstance, string currentStep, Guid activityInstanceId)
    {
        this.LoadData(workflowInstance, CurrentUserName, currentStep, activityInstanceId);
    }

    public void LoadData(WorkflowInstance workflowInstance, string currentUserName, string currentStep, Guid activityInstanceId)
    {
        this.txtTitle.Text = workflowInstance.Title;
        this.tdSheetId.InnerText = workflowInstance.SheetId;

        //如果不是发起人则屏蔽基本信息的编辑.
        bool isReadonly = false;
        if (workflowInstance.Creator != currentUserName && !this.IsReadonly)
            isReadonly = true;

        this.txtTitle.Enabled = !isReadonly;
        this.IsReadonly = isReadonly;

        WorkflowSetting setting = workflowSettingService.GetWorkflowSetting(workflowInstance.WorkflowId);
        string workflowName = setting.WorkflowName;
        if (string.IsNullOrEmpty(workflowName))
            workflowName = WorkflowUtility.GetWorkflowName(workflowInstance.WorkflowId);
        this.BindWorkflowName(workflowName);
        GenBasicInfoHtml(workflowInstance, setting, isReadonly, true, currentStep, activityInstanceId);
    }

    public void BindTitle(string title)
    {
        this.txtTitle.Text = title.Trim();
    }

    public void BindWorkflowName(string workflowName)
    {
        this.ltlWorkflowName.Text = workflowName;
    }

    public bool IsEmptyTitle()
    {
        return string.IsNullOrEmpty(this.txtTitle.Text.Trim());
    }

    private void GenBasicInfoHtml(WorkflowInstance workflowInstance, WorkflowSetting workflowSetting, bool isReadonly)
    {
        GenBasicInfoHtml(workflowInstance, workflowSetting, isReadonly, false, null, Guid.Empty);
    }

    private void GenBasicInfoHtml(WorkflowInstance workflowInstance, WorkflowSetting workflowSetting, bool isReadonly, bool withCreatorInfo, string currentStep, Guid activityInstanceId)
    {
        if (null == workflowSetting)
            workflowSetting = WorkflowSetting.Default;

        string basicFields = workflowSetting.BasicFields;
        // 使得不显示期望完成时间
        //if (!basicFields.StartsWith("0"))
        //{
        //    basicFields = basicFields.Remove(0, 1);
        //    basicFields = "0" + basicFields;
        //    workflowSetting.BasicFields = basicFields;
        //}
        //期望完成时间、保密设置、紧急程度、重要级别的字段数目  (不需要期望完成时间)
        int fieldsCount = workflowSetting.GetBasicFieldsCount();

        string expectFinishTime = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss");
        int secrecy = 0;
        int urgency = 0;
        int importance = 0;

        if (null != workflowInstance)
        {
            secrecy = workflowInstance.Secrecy;
            urgency = workflowInstance.Urgency;
            importance = workflowInstance.Importance;
            if (workflowInstance.ExpectFinishedTime.HasValue)
            {
                expectFinishTime = workflowInstance.ExpectFinishedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        this.Secrecy = secrecy;
        this.Urgency = urgency;
        this.Importance = importance;

        StringBuilder sb = new StringBuilder();
        switch (fieldsCount)
        {
            case 1:
                sb.Append("<tr>");
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.ExpectFinishedTime))
                {
                    AppendTimeHtml(sb, "期望完成时间", 5, expectFinishTime, isReadonly);
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Secrecy))
                {
                    AppendSecrecyHtml(sb, 5, secrecy, "radSecrecy", isReadonly);
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Urgency))
                {
                    AppendOptionHtml(sb, "紧急程度", 5, arrUrgency, urgency, "radUrgency", isReadonly);
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Importance))
                {
                    AppendOptionHtml(sb, "重要级别", 5, arrImportance, importance, "radImportance", isReadonly);
                }
                sb.Append("</tr>");
                break;
            case 2:
                sb.Append("<tr>");
                int colspan = 1;
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.ExpectFinishedTime))
                {
                    AppendTimeHtml(sb, "期望完成时间", colspan, expectFinishTime, isReadonly);
                    colspan += 2;
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Secrecy))
                {
                    AppendSecrecyHtml(sb, colspan, secrecy, "radSecrecy", isReadonly);
                    colspan += 2;
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Urgency))
                {
                    AppendOptionHtml(sb, "紧急程度", colspan, arrUrgency, urgency, "radUrgency", isReadonly);
                    colspan += 2;
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Importance))
                {
                    AppendOptionHtml(sb, "重要级别", colspan, arrImportance, importance, "radImportance", isReadonly);
                }
                sb.Append("</tr>");
                break;
            case 3:     //一行，无需跨列
                sb.Append("<tr>");
                AppendSecrecyHtml(sb, 1, secrecy, "radSecrecy", isReadonly);
                AppendOptionHtml(sb, "紧急程度", 1, arrUrgency, urgency, "radUrgency", isReadonly);
                AppendOptionHtml(sb, "重要级别", 1, arrImportance, importance, "radImportance", isReadonly);
                sb.Append("</tr>");
                break;
            case 4:     //分两行生成，每行两字段
                sb.Append("<tr>");

                AppendTimeHtml(sb, "期望完成时间", 1, expectFinishTime, isReadonly);
                AppendSecrecyHtml(sb, 3, secrecy, "radSecrecy", isReadonly);

                sb.Append("</tr>");

                AppendOptionHtml(sb, "紧急程度", 1, arrUrgency, urgency, "radUrgency", isReadonly);
                AppendOptionHtml(sb, "重要级别", 3, arrImportance, importance, "radImportance", isReadonly);

                sb.Append("</tr>");
                break;
            default:
                break;
        }

        if (withCreatorInfo && (null != workflowInstance))
        {
            AppendCreatorInfo(sb, workflowInstance, currentStep, activityInstanceId);
        }

        this.basicInfoHtml = sb.ToString();
        this.ltlBasicHtml.Text = this.basicInfoHtml;
    }

    public void AppendCreatorInfo(StringBuilder sb, WorkflowInstance workflowInstance, string currentStep, Guid activityInstanceId)
    {
        ActorDetail creatorInfo = workflowUserService.GetActorDetail(workflowInstance.Creator);
        string creatorName = String.Empty;
        string tel = String.Empty;
        if (null != creatorInfo)
        {
            creatorName = creatorInfo.RealName;
            if (!String.IsNullOrEmpty(creatorInfo.Mobile))
            {
                tel += creatorInfo.Mobile;
            }
            if (!String.IsNullOrEmpty(creatorInfo.Tel))
            {
                tel += "," + creatorInfo.Tel;
            }

            creatorName += "/" + GmccDeptHelper.GetDeptNameByDpFullName(creatorInfo.DpFullName);
        }

        WorkflowSetting setting = this.workflowSettingService.GetWorkflowSetting(workflowInstance.WorkflowId);
        FormInstance formData = formInstanceService.GetFormInstanceById(workflowInstance.WorkflowInstanceId, true);

        bool withPersonInfo = true;
        string flowNames = ConfigurationManager.AppSettings["flowNames"].ToString();
        string itemNames = ConfigurationManager.AppSettings["itemNames"].ToString();
        string itemValues = ConfigurationManager.AppSettings["itemValues"].ToString();
        if (!string.IsNullOrEmpty(flowNames) && !string.IsNullOrEmpty(itemNames))
        {
            flowNames = "," + flowNames + ",";
            string[] itemNamesArr = itemNames.Split(',');
            string[] itemValueArr = itemValues.Split(',');
            if (flowNames.Contains("," + setting.WorkflowName + ","))
            {
                foreach (var item in formData.Items)
                {
                    for (int i = 0; i < itemNamesArr.Length; i++)
                    {
                        if (item.Definition.FName == itemNamesArr[i] && item.Value == itemValueArr[i])
                        {
                            withPersonInfo = false;
                            break;
                        }
                    }

                }
            }
        }

        sb.Append("<tr>");
        if (withPersonInfo)
        {
            sb.Append("<th style='width:13%;'>发起人：</th>");
            sb.AppendFormat("<td style='width:20%;'>{0}</td>", creatorName);
            sb.Append("<th style='width:13%;'>联系电话：</th>");
            sb.AppendFormat("<td style='width:20%;'>{0}</td>", tel);
        }
        sb.Append("<th style='width:13%;'>创建时间：</th>");
        sb.AppendFormat("<td style='width:20%;'>{0}</td>", workflowInstance.StartedTime.ToString("yyyy-MM-dd HH:mm:ss"));
        sb.Append("</tr>");

        sb.Append("<tr>");
        sb.Append("<th style='width:13%;'>当前步骤：</th>");
        sb.AppendFormat("<td style='width:20%;'>{0}</td>", currentStep);
        sb.Append("<th style='width:13%;'>当前处理人：</th>");

        sb.Append("<td colspan='3'>");
        IList<BasicUser> users = taskAssignService.GetTodoActors(activityInstanceId);
        for (int i = 0, icount = users.Count - 1; i <= icount; i++)
        {
            BasicUser userItem = users[i];
            sb.AppendFormat("<span tooltip=\"{0}\">{1}</span>", userItem.UserName, userItem.RealName);
            if (i < icount)
            {
                sb.Append(',');
            }
        }
        sb.Append("</td>");

        sb.Append("</tr>");
    }

    public WorkflowInstance GetInput()
    {
        string title = this.txtTitle.Text.Trim();
        if (title.Length == 0)
        {
            ShowError("标题不能为空");
        }

        WorkflowInstance instance = new WorkflowInstance();
        string sTime = Request.Form["txtExpectFinishTime"];
        if (String.IsNullOrEmpty(sTime))
        {
            //如果没有期望完成时间，则取一个不可能的时间

            instance.ExpectFinishedTime = new DateTime(2900, 1, 1);
        }
        else
        {
            DateTime expectFinishedTime = Convert.ToDateTime(sTime);
            if (expectFinishedTime <= DateTime.Now && string.IsNullOrEmpty(Request["aiid"]))
                ShowError("期望完成时间不能小于当前时间.");

            instance.ExpectFinishedTime = expectFinishedTime;
        }

        instance.Title = title;
        instance.Secrecy = this.Secrecy;
        instance.Urgency = this.Urgency;
        instance.Importance = this.Importance;

        if (Request.Form["radSecrecy"] != null)
            instance.Secrecy = int.Parse(Request.Form["radSecrecy"]);
        if (Request.Form["radUrgency"] != null)
            instance.Urgency = int.Parse(Request.Form["radUrgency"]);
        if (Request.Form["radImportance"] != null)
            instance.Importance = int.Parse(Request.Form["radImportance"]);
        return instance;
    }

    public void UpdateWorkflowInstance(Guid workflowInstanceId)
    {
        this.UpdateWorkflowInstance(workflowInstanceId, false);
    }

    /// <summary>
    /// 更新流程实例.
    /// </summary>
    /// <param name="workflowInstanceId"></param>
    /// <param name="isDraft">是否保存为草稿.</param>
    public void UpdateWorkflowInstance(Guid workflowInstanceId, bool isDraft)
    {
        WorkflowInstance instance = GetInput();
        instance.WorkflowInstanceId = workflowInstanceId;
        instance.State = (int)(isDraft ? WorkflowConstants.Initial : WorkflowConstants.Executing);
        workflowService.UpdateWorkflowInstance(instance);
    }

    private static void AppendTimeHtml(StringBuilder builder, string title, int colspan, string expectFinishTime, bool isReadonly)
    {
        string widthDesc = (colspan == 1) ? "style='width:20%;'" : String.Format("colspan='{0}'", colspan);
        builder.AppendFormat("<th style='width:13%;'>{0}：</th>", title);
        if (isReadonly)
        {
            builder.AppendFormat("<td {0}><input name='txtExpectFinishTime' type='text' id='txtExpectFinishTime' class='inputbox' style='width:122px;' disabled value='{1}' /></td>", widthDesc, expectFinishTime);
        }
        else
        {
            builder.AppendFormat("<td {0}><input name='txtExpectFinishTime' type='text' id='txtExpectFinishTime' class='inputbox' style='width:122px;' value='{1}' /><span class='ico_pickdate' title='点击选择日期' onclick='return showCalendar(\"txtExpectFinishTime\",\"%Y-%m-%d  %H:%M:%S\", \"24\", true);'>&nbsp;</span><span class='require'>*</span></td>", widthDesc, expectFinishTime);
        }
    }

    private static void AppendOptionHtml(StringBuilder builder, string title, int colspan, string[] options,
        int selectedValue, string controlClientId, bool isReadOnly)
    {
        string widthDesc = (colspan == 1) ? "style='width:20%;'" : String.Format("colspan='{0}'", colspan);
        builder.AppendFormat("<th style='width:13%;'>{0}：</th>", title);
        builder.AppendFormat("<td {0}>", widthDesc);

        if (isReadOnly)
            builder.AppendFormat("<select id=\"{0}\" name=\"{0}\" disabled=\"disabled\">", controlClientId);
        else
            builder.AppendFormat("<select id=\"{0}\" name=\"{0}\">", controlClientId);
        int length = options.Length;
        for (int i = 0; i < length; i++)
        {
            builder.AppendFormat("<option value=\"{0}\"{1}>{2}</option>",
                i,
                selectedValue == i ? " selected=\"selected\"" : string.Empty,
                options[i]);
        }
        builder.Append("</select>");

        builder.Append("</td>");
    }

    private static void AppendSecrecyHtml(StringBuilder builder, int colspan,
        int v, string controlClientId, bool isReadOnly)
    {
        string widthDesc = (colspan == 1) ? "style='width:20%;'" : String.Format("colspan='{0}'", colspan);
        builder.Append("<th style='width:13%;'>保密设置：</th>");
        builder.AppendFormat("<td {0}>", widthDesc);

        string checkedDesc = (v == 1) ? "checked" : String.Empty;

        if (isReadOnly)
            builder.AppendFormat("<input type='checkbox' id=\"{0}\" name=\"{0}\" value='1' disabled=\"disabled\" {1} />保密", controlClientId, checkedDesc);
        else
            builder.AppendFormat("<input type='checkbox' id=\"{0}\" name=\"{0}\" value='1' {1} />保密", controlClientId, checkedDesc);
        builder.Append("</td>");
    }
}
