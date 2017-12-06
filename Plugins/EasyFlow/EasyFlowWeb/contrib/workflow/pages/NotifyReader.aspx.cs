using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Botwave.Web;
using Botwave.Workflow;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;
using Botwave.XQP.Service.Plugins;
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;
using Botwave.GMCCServiceHelpers;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Practices.CZMCC.Support;

public partial class contrib_workflow_pages_NotifyReader : Botwave.Security.Web.PageBase
{

    /// <summary>
    /// 显示消息时返回的页面.
    /// </summary>
    private static readonly string WorkflowRootUrl = AppPath + "contrib/workflow/pages/";

    private IActivityService activityService = (IActivityService)Ctx.GetObject("activityService");
    private IWorkflowService workflowService = (IWorkflowService)Ctx.GetObject("workflowService");
    private IWorkflowSettingService workflowSettingService = (IWorkflowSettingService)Ctx.GetObject("workflowSettingService");
    private IWorkflowUserService workflowUserService = (IWorkflowUserService)Ctx.GetObject("workflowUserService");
    private NotifyReaderService notifyReaderService = (NotifyReaderService)Ctx.GetObject("notifyReaderService");

    public IWorkflowService WorkflowService
    {
        set { workflowService = value; }
    }

    public IWorkflowSettingService WorkflowSettingService
    {
        set { workflowSettingService = value; }
    }

    public IWorkflowUserService WorkflowUserService
    {
        set { workflowUserService = value; }
    }

    public IActivityService ActivityService
    {
        set { activityService = value; }
    }

    public NotifyReaderService NotifyReaderService
    {
        set { notifyReaderService = value; }
    }

    public Guid ActivityInstanceId
    {
        get { return (Guid)ViewState["ActivityInstanceId"]; }
        set { ViewState["ActivityInstanceId"] = value; }
    }

    public string WorkflowTitle
    {
        get { return (string)ViewState["WorkflowTitle"]; }
        set { ViewState["WorkflowTitle"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string aiid = Request.QueryString["aiid"];
            if (string.IsNullOrEmpty(aiid))
            {
                //ShowError(MessageHelper.Message_ArgumentException);
            }
            else
            {
                Guid activityInstanceId = new Guid(aiid);
                this.ActivityInstanceId = activityInstanceId;

                this.LoadData(activityInstanceId);
            }
        }
    }

    private void LoadData(Guid activityInstanceId)
    {
        if (activityInstanceId == Guid.Empty)
        {
            ShowError(MessageHelper.Message_ArgumentException);
        }

        // 流程实例
        WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);
        if(workflowInstance == null)
        {
            ShowError(MessageHelper.Message_ArgumentException);
        }
        else if (workflowInstance.State == WorkflowConstants.Complete)
        {
            ShowSuccess("工单已处理完毕，不能再发送抄送消息。", WorkflowRootUrl + string.Format("workflowview.aspx?wiid={0}", workflowInstance.WorkflowInstanceId));
        }
        string workflowTitle = workflowInstance.Title;
        ActorDetail creatorDetail = workflowUserService.GetActorDetail(workflowInstance.Creator);
        string creatorName = (creatorDetail == null ? workflowInstance.Creator : creatorDetail.RealName);
        this.lbWrokflowTitle.Text = workflowTitle;
        this.WorkflowTitle = workflowTitle;

        // 下行步骤列表
        IList<ActivityInstance> nextActivities = activityService.GetNextActivities(activityInstanceId);
        if (nextActivities == null || nextActivities.Count == 0)
            ShowError(MessageHelper.Message_ArgumentException + " 未能找到下行步骤。");
        string activityNames = string.Empty;
        foreach (ActivityInstance item in nextActivities)
        {
            activityNames += string.Format(",{0}", item.ActivityName);
        }
        activityNames = activityNames.Remove(0, 1);

        // 流程配置
        WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowInstance.WorkflowId);
        if (wprofile == null)
            wprofile = WorkflowProfile.Default;
        string templateText = wprofile.EmailNotifyFormat.Replace("处理", "知晓");
        templateText = FormatNotifyMessage(templateText, creatorName, workflowTitle, activityNames, 0);
        this.txtBeReadNotifyFormat.Text = templateText;
    }

    /// <summary>
    /// 格式化指定提醒信息内容.
    /// </summary>
    /// <param name="messageFormat"></param>
    /// <param name="workflowTitle"></param>
    /// <param name="activityName"></param>
    /// <param name="operateType"></param>
    /// <returns></returns>
    protected static string FormatNotifyMessage(string messageFormat, string creator, string workflowTitle, string activityName, int operateType)
    {
        messageFormat = messageFormat.ToLower();
        messageFormat = messageFormat.Replace("处理", "知晓");
        messageFormat = messageFormat.Replace("#creator#", creator);
        messageFormat = messageFormat.Replace("#title#", workflowTitle);
        messageFormat = messageFormat.Replace("#activityname#", activityName);
        messageFormat = messageFormat.Replace("#operatetype#", operateType == TodoInfo.OpBack ? "退回" : "进入");
        return messageFormat;
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        Guid activityInstanceId = this.ActivityInstanceId;
        string title = this.WorkflowTitle;
        string message = this.txtBeReadNotifyFormat.Text;
        if (string.IsNullOrEmpty(message))
            ShowError("填写错误，抄送内容不能为空。");

        string users = txtUsersAssign.Text;
        string[] userName = users.Split(',');
        if (userName == null || userName.Length == 0)
            ShowError("填写错误，请选择抄送人。");

        if (chkSms.Checked)    //短信
        {
            notifyReaderService.SendMessage(NotifyReaderService.SMS, title, message, userName);
        }
        if (chkEmail.Checked)  //邮件
        {
            notifyReaderService.SendMessage(NotifyReaderService.Email, title, message, userName);
        }
        if (chkBeRead.Checked) //待阅
        {
            string entityId = activityInstanceId.ToString();
            string url = WorkflowPostHelper.TransformViewUrlByActivityInstanceId(entityId);

            for (int i = 0; i < userName.Length; i++)
            {
                AsynExtendedPendingJobHelper.AddPendingMsg(userName[i].ToString(), CurrentUserName, title, url, ActivityInstance.EntityType, entityId);
            }
        }

        ShowSuccess("已成功抄送。", AppPath + "contrib/workflow/pages/default.aspx");
    }
}
