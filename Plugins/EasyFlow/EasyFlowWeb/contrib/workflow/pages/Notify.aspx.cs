using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Commons;
using Botwave.Web;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.XQP.Domain;

public partial class contrib_workflow_pages_Notify : Botwave.Security.Web.PageBase
{
    /// <summary>
    /// 显示消息时返回的页面.
    /// </summary>
    private static readonly string WorkflowRootUrl = AppPath + "contrib/workflow/pages/";
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_workflow_pages_Notify));
    /// <summary>
    /// 转向页面.
    /// </summary>
    public string ReturnUrl = "default.aspx";

    public string aiid; 
    
    private static string feedbackWorkflow = "销售精英竞赛平台酬金申告流程";
    private static string feedbackMessage = "尊敬的会员，您提交的酬金申告单：#title#，已经处理完成，请确认。"; // #creator#, #title#.

    #region service interfaces

    private IWorkflowEngine workflowEngine = (IWorkflowEngine)Ctx.GetObject("workflowEngine");
    private IWorkflowService workflowService = (IWorkflowService)Ctx.GetObject("workflowService");
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IWorkflowSettingService workflowSettingService = (IWorkflowSettingService)Ctx.GetObject("workflowSettingService");
    private IWorkflowNotifyService workflowNotifyService = (IWorkflowNotifyService)Ctx.GetObject("workflowNotifyService");
    private IWorkflowUserService workflowUserService = (IWorkflowUserService)Ctx.GetObject("workflowUserService");
    private IFormInstanceService formInstanceService = (IFormInstanceService)Ctx.GetObject("formInstanceService");
    private bool isDirectComplete = true;

    public IWorkflowEngine WorkflowEngine
    {
        set { workflowEngine = value; }
    }

    public IWorkflowService WorkflowService
    {
        set { workflowService = value; }
    }

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public IWorkflowSettingService WorkflowSettingService
    {
        set { workflowSettingService = value; }
    }

    public IWorkflowNotifyService WorkflowNotifyService
    {
        set { workflowNotifyService = value; }
    }

    public IWorkflowUserService WorkflowUserService
    {
        set { workflowUserService = value; }
    }

    public IFormInstanceService FormInstanceService
    {
        set { formInstanceService = value; }
    }

    public string FeedbackWorkflow
    {
        set { feedbackWorkflow = value; }
    }

    public string FeedbackMessage
    {
        set { feedbackMessage = value; }
    }

    public bool IsDirectComplete
    {
        set { isDirectComplete = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {   }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        aiid = Request.QueryString["aiid"];
        if (String.IsNullOrEmpty(aiid))
        {
            ShowError(MessageHelper.Message_ArgumentException, WorkflowRootUrl + "default.aspx");
        }
        ReturnUrl = string.Format("workflowview.aspx?aiid={0}", aiid);
        string cmdText = Request.QueryString["c"];
        Guid activityInstanceId = new Guid(aiid);
        this.ProcessNotify(activityInstanceId, cmdText);
    }

    #region Methods

    /// <summary>
    /// 处理发送提醒信息.
    /// </summary>
    /// <param name="activityInstanceId"></param>
    /// <param name="cmdText"></param>
    private void ProcessNotify(Guid activityInstanceId, string cmdText)
    {
        WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);

        if (workflowInstance.State == WorkflowConstants.Complete)
        {
            //PostCompleteWorkflow(workflowInstance);
            //ShowSuccess("工单已处理完毕.", WorkflowRootUrl + string.Format("workflowview.aspx?wiid={0}", workflowInstance.WorkflowInstanceId));
            ShowSuccess("工单已处理完毕.", WorkflowRootUrl + "default.aspx");
        }

        IList<NotifyActor> notifyActors = workflowNotifyService.GetNextNotifyActors(activityInstanceId);
        if (notifyActors == null && notifyActors.Count == 0)
        {
            ShowError("流程执行错误，未发现下一步骤执行人.", WorkflowRootUrl + "default.aspx");
        }

        WorkflowSetting wfsetting =workflowSettingService.GetWorkflowSetting(workflowInstance.WorkflowId);
        if (wfsetting == null)
            wfsetting = WorkflowSetting.Default;

        Botwave.Security.LoginUser currentActor = CurrentUser;

        string fromEmail = string.Empty;
        string fromMobile = string.Empty;
        ActorDetail sender = workflowUserService.GetActorDetail(currentActor.UserName);

        int operateType = TodoInfo.OpDefault;
        if (cmdText.Equals(ActivityCommands.Reject, StringComparison.OrdinalIgnoreCase))
            operateType = TodoInfo.OpBack;

        if (notifyActors.Count > 0)
        {
            this.OnAutoProcess(workflowInstance, currentActor, notifyActors);
            log.Info("前：短信已发送");
            workflowNotifyService.SendMessage(sender, operateType, activityInstanceId, wfsetting, workflowInstance, notifyActors);
            log.InfoFormat("后：短信发送成功,activityInstanceId为{0},sender为{1},workflowInstance为{2},operateType{3}", activityInstanceId, sender, workflowInstance, operateType);
            ShowNotifyActors(notifyActors);
            ShowReviewActors(activityInstanceId);
            
            this.OnPreRedirect();
            //ShowSuccess("已成功处理并发送提醒消息！", WorkflowRootUrl + string.Format("workflowview.aspx?aiid={0}", activityInstanceId));
        }
        else
        {
            log.Info("notifyActors.Count数量小于0,成功处理工单！");
            this.OnPreRedirect();
            //ShowSuccess("成功处理工单.", WorkflowRootUrl + string.Format("workflowview.aspx?aiid={0}", activityInstanceId));
            ShowSuccess("成功处理工单.", WorkflowRootUrl + "default.aspx");
        }
    }

    private void ShowNotifyActors(IList<NotifyActor> notifyActors)
    {
        IDictionary<string, StringBuilder> dict = new Dictionary<string, StringBuilder>();
        foreach (NotifyActor notifyActor in notifyActors)
        {
            if (!dict.ContainsKey(notifyActor.ActivityName))
            {
                dict.Add(notifyActor.ActivityName, new StringBuilder());
            }

            if (dict[notifyActor.ActivityName].Length > 0)
            {
                dict[notifyActor.ActivityName].Append(", ");
            }
            dict[notifyActor.ActivityName].Append(notifyActor.RealName);
        }

        this.rptList.DataSource = dict;
        this.rptList.DataBind();
    }

    private void ShowReviewActors(Guid senderActivityInstanceId)
    {
        IDictionary<string, StringBuilder> dict = new Dictionary<string, StringBuilder>(StringComparer.OrdinalIgnoreCase);
        DataTable resultTable = ToReview.GetReviewTableBySender(senderActivityInstanceId);
        if (resultTable != null && resultTable.Rows.Count > 0)
        {
            foreach (DataRow row in resultTable.Rows)
            {
                string activityName = DbUtils.ToString(row["ActivityName"]);
                string actor = DbUtils.ToString("UserName");
                string actorName = DbUtils.ToString(row["RealName"]);
                actorName = (string.IsNullOrEmpty(actorName) ? actor : actorName);

                if (!dict.ContainsKey(activityName))
                    dict.Add(activityName, new StringBuilder());
                if (dict[activityName].Length > 0)
                    dict[activityName].AppendFormat(",{0} ", actorName);
                else
                    dict[activityName].Append(actorName);
            }
        }
        if (dict.Count == 0)
        {
            this.divReviewActors.Visible = false;
        }
        else
        {
            this.rptReviewActors.DataSource = dict;
            this.rptReviewActors.DataBind();
        }
    }

    private void PostCompleteWorkflow(WorkflowInstance workflowInstance)
    {
        WorkflowDefinition workflowDefinition = workflowDefinitionService.GetWorkflowDefinition(workflowInstance.WorkflowId);
        if (workflowDefinition == null)
            return;

        SMSProfile profile = SMSProfile.GetProfile();
        string message = profile.GratuityReplyMessage;
        if (string.IsNullOrEmpty(message))
            message = feedbackMessage;
        if (!string.IsNullOrEmpty(feedbackMessage) && workflowDefinition.WorkflowName.Equals(feedbackWorkflow, StringComparison.OrdinalIgnoreCase))
        {
            message = FormatGratuityMessage(message, workflowInstance);
            Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendSMS(workflowInstance.Creator, string.Empty, message, "bwwf_Tracking_Workflows", workflowInstance.WorkflowInstanceId.ToString());
        }        
    }

    /// <summary>
    /// 格式化酬金申告流程的消息通知.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="workflowInstance"></param>
    /// <returns></returns>
    private string FormatGratuityMessage(string message, WorkflowInstance workflowInstance)
    {
        // 酬金申告流程，发送反馈短信.
        string title = workflowInstance.Title;
        string creator = workflowInstance.Creator;
        string creatorName = string.Empty;
        message = message.Trim().ToLower();

        if (message.IndexOf("#creator#", StringComparison.OrdinalIgnoreCase) > -1)
        {
            ActorDetail detail = workflowUserService.GetActorDetail(creator);
            if (detail != null)
                creatorName = detail.RealName;
            message = message.Replace("#creator#", creatorName);
        }
        message = message.Replace("#title#", title);
        return message;
    }
    #endregion

    protected void OnPreRedirect()
    {
        if (this.isDirectComplete)
        {
            Response.Redirect(WorkflowRootUrl + "default.aspx");
        }
    }

    /// <summary>
    /// 自动处理.
    /// </summary>
    /// <param name="workflowInstance"></param>
    /// <param name="actor"></param>
    /// <param name="nextActors"></param>
    protected void OnAutoProcess(WorkflowInstance workflowInstance, Botwave.Security.LoginUser currentActor, IList<NotifyActor> nextActors)
    {
        if (currentActor == null || nextActors == null || nextActors.Count != 1 ||
            !nextActors[0].UserName.Equals(currentActor.UserName, StringComparison.OrdinalIgnoreCase))
            return;
        Guid workflowid = workflowInstance.WorkflowId;
        Guid workflowInstanceId = workflowInstance.WorkflowInstanceId;
        WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowid);
        if (wprofile == null || !wprofile.IsAutoContinue || string.IsNullOrEmpty(wprofile.AutoContinueActivities))
            return;

        IDictionary<string, string> activities = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        string[] autoActivities = wprofile.AutoContinueActivities.Split(',', '，');
        foreach (string activityName in autoActivities)
        {
            if (!activities.ContainsKey(activityName.Trim()))
                activities.Add(activityName.Trim(), string.Empty);
        }
        NotifyActor next = nextActors[0];
        if (!activities.ContainsKey(next.ActivityName))
            return;

        Guid activityInstanceId = next.ActivityInstanceId;
        // 自动处理.
        ActivityExecutionContext executionContext = new ActivityExecutionContext();
        executionContext.ActivityInstanceId = activityInstanceId;
        executionContext.Actor = currentActor.UserName;
        executionContext.Command = ActivityCommands.Approve;
        executionContext.Reason = "系统自动处理";

        executionContext.Variables = GetFormVariables(workflowInstanceId);
        if (workflowInstance != null)
        {
            executionContext.Variables["Secrecy"] = workflowInstance.Secrecy;
            executionContext.Variables["Urgency"] = workflowInstance.Urgency;
            executionContext.Variables["Importance"] = workflowInstance.Importance;
        }
        executionContext.Variables["CurrentUser"] = currentActor; //添加当前用户为流程变量
        executionContext.Variables["CurrentPage"] = this.Page; //添加当前页为流程变量

        workflowEngine.ExecuteActivity(executionContext);
        Response.Redirect(string.Format("Notify.aspx?aiid={0}&c={1}", activityInstanceId, executionContext.Command));
    }

    private IDictionary<string, object> GetFormVariables(Guid workflowInstanceId)
    {
        IDictionary<string, object> results = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        IList<FormItemInstance> items =formInstanceService.GetFormItemInstancesByFormInstanceId(workflowInstanceId, true);
        if (items == null || items.Count == 0)
            return results;
        foreach (FormItemInstance item in items)
        {
            if (item.Definition == null)
                continue;
            object value = null;
            if (item.Definition.ItemDataType == FormItemDefinition.DataType.Decimal)
                value = item.DecimalValue;
            else
                value = (string.IsNullOrEmpty(item.TextValue) ? item.Value : item.TextValue);
            results[item.Definition.FName] = value;
        }
        return results;
    }
}
