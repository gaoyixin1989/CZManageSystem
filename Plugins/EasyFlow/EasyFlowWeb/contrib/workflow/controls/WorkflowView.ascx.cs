using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

using Botwave.Entities;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Security.Service;
using Botwave.Security.Domain;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;

public partial class contrib_workflow_controls_WorkflowView : Botwave.Security.Web.UserControlBase//System.Web.UI.UserControl
{
    private string basicInfoHtml;
    public string BasicInfoHtml
    {
        get { return basicInfoHtml; }
        set { basicInfoHtml = value; }
    }

    #region service interfaces

    private IWorkflowSettingService workflowSettingService = (IWorkflowSettingService)Ctx.GetObject("workflowSettingService");
    private ITaskAssignService taskAssignService = (ITaskAssignService)Ctx.GetObject("taskAssignService");
    private IWorkflowUserService workflowUserService = (IWorkflowUserService)Ctx.GetObject("workflowUserService");
    private IActivityService activityService = (IActivityService)Ctx.GetObject("activityService");

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

    public IActivityService ActivityService
    {
        set { activityService = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    { }

    public void LoadData(WorkflowInstance workflowInstance, string currentStep, Guid activityInstanceId)
    {
        WorkflowSetting workflowSetting = workflowSettingService.GetWorkflowSetting(workflowInstance.WorkflowId);

        this.ltlTitle.Text = workflowInstance.Title;
        this.ltlSheetId.Text = workflowInstance.SheetId;

        string workflowName = workflowSetting.WorkflowName;
        if (string.IsNullOrEmpty(workflowName))
            workflowName = WorkflowUtility.GetWorkflowName(workflowInstance.WorkflowId);
        this.ltlWorkflowName.Text = workflowName;
        GenBasicInfoHtml(workflowInstance, workflowSetting, currentStep, activityInstanceId);
    }

    private void GenBasicInfoHtml(WorkflowInstance workflowInstance, WorkflowSetting workflowSetting, string currentStep, Guid activityInstanceId)
    {
        //期望完成时间、保密设置、紧急程度、重要级别的字段数目
        int fieldsCount = workflowSetting.GetBasicFieldsCount();

        string expectFinishTime = String.Empty;
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

        StringBuilder sb = new StringBuilder();
        switch (fieldsCount)
        {
            case 1:
                sb.Append("<tr>");
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.ExpectFinishedTime))
                {
                    AppendItemHtml(sb, "期望完成时间", 5, expectFinishTime);
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Secrecy))
                {
                    AppendItemHtml(sb, "保密设置", 5, WorkflowSetting.ToSecrecyDescription(secrecy));
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Urgency))
                {
                    AppendItemHtml(sb, "紧急程度", 5, WorkflowSetting.ToUrgencyDescription(urgency));
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Importance))
                {
                    AppendItemHtml(sb, "重要级别", 5, WorkflowSetting.ToImportanceDescription(importance));
                }
                sb.Append("</tr>");
                break;
            case 2:
                sb.Append("<tr>");
                int colspan = 1;
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.ExpectFinishedTime))
                {
                    AppendItemHtml(sb, "期望完成时间", colspan, expectFinishTime);
                    colspan += 2;
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Secrecy))
                {
                    AppendItemHtml(sb, "保密设置", colspan, WorkflowSetting.ToSecrecyDescription(secrecy));
                    colspan += 2;
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Urgency))
                {
                    AppendItemHtml(sb, "紧急程度", colspan, WorkflowSetting.ToUrgencyDescription(urgency));
                    colspan += 2;
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Importance))
                {
                    AppendItemHtml(sb, "重要级别", colspan, WorkflowSetting.ToImportanceDescription(importance));
                }
                sb.Append("</tr>");
                break;
            case 3:     //一行，无需跨列
                sb.Append("<tr>");
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.ExpectFinishedTime))
                {
                    AppendItemHtml(sb, "期望完成时间", 1, expectFinishTime);
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Secrecy))
                {
                    AppendItemHtml(sb, "保密设置", 1, WorkflowSetting.ToSecrecyDescription(secrecy));
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Urgency))
                {
                    AppendItemHtml(sb, "紧急程度", 1, WorkflowSetting.ToUrgencyDescription(urgency));
                }
                if (workflowSetting.HasBasicField(WorkflowSetting.BasicFieldType.Importance))
                {
                    AppendItemHtml(sb, "重要级别", 1, WorkflowSetting.ToImportanceDescription(importance));
                }
                sb.Append("</tr>");
                break;
            case 4:     //分两行生成，每行两字段

                sb.Append("<tr>");
                AppendItemHtml(sb, "期望完成时间", 1, expectFinishTime);
                AppendItemHtml(sb, "保密设置", 3, WorkflowSetting.ToSecrecyDescription(secrecy));
                sb.Append("</tr>");

                sb.Append("<tr>");
                AppendItemHtml(sb, "紧急程度", 1, WorkflowSetting.ToUrgencyDescription(urgency));
                AppendItemHtml(sb, "重要级别", 3, WorkflowSetting.ToImportanceDescription(importance));
                sb.Append("</tr>");
                break;
            default:
                break;
        }

        AppendCreatorInfo(sb, workflowInstance, currentStep, activityInstanceId);

        this.basicInfoHtml = sb.ToString();
    }

    public void AppendCreatorInfo(StringBuilder sb, WorkflowInstance workflowInstance, string currentStep, Guid activityInstanceId)
    {
        ActorDetail userInfo = workflowUserService.GetActorDetail(workflowInstance.Creator);
        string creatorName = String.Empty;
        string tel = String.Empty;
        if (null != userInfo)
        {
            creatorName = userInfo.RealName;
            if (!string.IsNullOrEmpty(userInfo.Mobile))
                tel += userInfo.Mobile;
            if (!string.IsNullOrEmpty(userInfo.Tel))
                tel += (tel.Length > 0 ? "," : "") + userInfo.Tel;

            string dpFullName = GmccDeptHelper.GetDeptNameByDpFullName(userInfo.DpFullName);
            creatorName += (dpFullName.Length > 0 ? "/" : "") + dpFullName;
        }

        sb.Append("<tr>");
        sb.Append("<th style='width:13%;'>发起人：</th>");
        sb.AppendFormat("<td style='width:20%;'>{0}</td>", creatorName);
        sb.Append("<th style='width:13%;'>联系电话：</th>");
        sb.AppendFormat("<td style='width:20%;'>{0}</td>", tel);
        sb.Append("<th style='width:13%;'>创建时间：</th>");
        sb.AppendFormat("<td style='width:20%;'>{0}</td>", workflowInstance.StartedTime.ToString("yyyy-MM-dd HH:mm:ss"));
        sb.Append("</tr>");

        //获取当前活动列表
        IList<ActivityInstance> currentActivities = activityService.GetCurrentActivities(workflowInstance.WorkflowInstanceId);
        ActivityInstance currentActivityInstance = null;
        StringBuilder sbCurrentStep = new StringBuilder();
        StringBuilder sbCurrentActors = new StringBuilder();
        IDictionary<string, Guid> currentActors = new Dictionary<string, Guid>();
        foreach (ActivityInstance activityInstance in currentActivities)
        {
            Guid currentActivityInstanceId = activityInstance.ActivityInstanceId;
            currentActivityInstance = activityService.GetActivity(currentActivityInstanceId);
            string activityActor = currentActivityInstance.Actor;
            if (sbCurrentStep.ToString().IndexOf(currentActivityInstance.ActivityName) == -1)
                sbCurrentStep.Append("," + currentActivityInstance.ActivityName);

            if (string.IsNullOrEmpty(activityActor))
            {
                IList<BasicUser> users = taskAssignService.GetTodoActors(currentActivityInstanceId);
                if (users == null || users.Count == 0)
                    continue;
                foreach (BasicUser item in users)
                {
                    if (!currentActors.ContainsKey(item.UserName))
                    {
                        currentActors.Add(item.UserName, currentActivityInstanceId);
                        sbCurrentActors.AppendFormat(",<span tooltip=\"{0}\">{1}</span>", item.UserName, item.RealName);
                    }
                }
            }
            else
            {
                if (!currentActors.ContainsKey(activityActor))
                {
                    currentActors.Add(activityActor, currentActivityInstanceId);
                    ActorDetail actorUser = workflowUserService.GetActorDetail(activityActor);
                    sbCurrentActors.AppendFormat(",<span tooltip=\"{0}\">{1}</span>", actorUser.UserName, actorUser.RealName);
                }
            }
        }

        if (sbCurrentStep.Length > 0)
            sbCurrentStep = sbCurrentStep.Remove(0, 1);
        if (sbCurrentActors.Length > 0)
            sbCurrentActors = sbCurrentActors.Remove(0, 1);

        if (workflowInstance.State == 2)
            sbCurrentStep = new StringBuilder("完成");
        else if (workflowInstance.State == 99)
            sbCurrentStep = new StringBuilder("取消");

        sb.Append("<tr>");
        sb.Append("<th style='width:13%;'>当前步骤：</th>");
        if (workflowInstance.State != 2)
        {
            sb.AppendFormat("<td style='width:20%;'>{0}</td>", sbCurrentStep.ToString());
            sb.Append("<th style='width:13%;'>当前处理人：</th>");

            sb.Append("<td colspan='3'>");
            if (sbCurrentActors.ToString().Length > 0)
                sb.Append(sbCurrentActors.ToString());
            else
                sb.Append("&nbsp;");
        }
        else
            sb.AppendFormat("<td colspan='5'>{0}</td>", sbCurrentStep.ToString());
        sb.Append("</td>");

        sb.Append("</tr>");
    }

    private static void AppendItemHtml(StringBuilder builder, string title, int colspan, string text)
    {
        string widthDesc = (colspan == 1) ? "style='width:20%;'" : String.Format("colspan='{0}'", colspan);
        builder.AppendFormat("<th style='width:13%;'>{0}：</th>", title);
        builder.AppendFormat("<td {0}>{1}</td>", widthDesc, text);
    }
}
