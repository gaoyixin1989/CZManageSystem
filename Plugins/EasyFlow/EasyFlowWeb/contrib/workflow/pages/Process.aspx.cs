using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Web;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.UI;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;
using System.Data;
using System.Linq;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Extension.Implements;

public partial class contrib_workflow_pages_Process : Botwave.Security.Web.PageBase
{

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_workflow_pages_Process));
    private static readonly string ReturnUrl = WebUtils.GetAppPath() + "contrib/workflow/pages/default.aspx";

    private Guid activityInstanceId;
    private ActivityDefinition activityDefinition;
    public ActivityInstance activityInstance;

    #region service interfaces

    private IWorkflowEngine workflowEngine = (IWorkflowEngine)Ctx.GetObject("workflowEngine");
    private IWorkflowService workflowService = (IWorkflowService)Ctx.GetObject("workflowService");
    private IActivityService activityService = (IActivityService)Ctx.GetObject("activityService");
    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IActivityAllocationService activityAllocationService = (IActivityAllocationService)Ctx.GetObject("activityAllocationService");
    private ITaskAssignService taskAssignService = (ITaskAssignService)Ctx.GetObject("taskAssignService");
    private IWorkflowFormService workflowFormService = (IWorkflowFormService)Ctx.GetObject("workflowFormService");
    private IActivityExecutionContextHandler activityExecutionContextHandler = (IActivityExecutionContextHandler)Ctx.GetObject("activityExecutionContextHandler");
    private IWorkflowUIProfile workflowUIProfile = (IWorkflowUIProfile)Ctx.GetObject("workflowUIProfile");
    private IWorkflowReviewService workflowReviewService = (IWorkflowReviewService)Ctx.GetObject("workflowReviewService");
    private IReviewPending reviewPending = (IReviewPending)Ctx.GetObject("reviewPending");
    private IFormInstanceService formInstanceService = (IFormInstanceService)Ctx.GetObject("formInstanceService");
    private IGetOuterDataHandler getOuterDataHandler = (IGetOuterDataHandler)Ctx.GetObject("getOuterDataHandler");
    private IDataListInstanceService dataListInstanceService = (IDataListInstanceService)Ctx.GetObject("dataListInstanceService");

    public IWorkflowEngine WorkflowEngine
    {
        set { workflowEngine = value; }
    }

    public IWorkflowService WorkflowService
    {
        set { workflowService = value; }
    }

    public IActivityService ActivityService
    {
        set { activityService = value; }
    }

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public ITaskAssignService TaskAssignService
    {
        set { taskAssignService = value; }
    }

    public IActivityAllocationService ActivityAllocationService
    {
        set { activityAllocationService = value; }
    }

    public IWorkflowFormService WorkflowFormService
    {
        set { workflowFormService = value; }
    }

    public IActivityExecutionContextHandler ActivityExecutionContextHandler
    {
        set { activityExecutionContextHandler = value; }
    }

    public IWorkflowUIProfile WorkflowUIProfile
    {
        set { workflowUIProfile = value; }
    }

    public IWorkflowReviewService WorkflowReviewService
    {
        set { workflowReviewService = value; }
    }

    public IReviewPending ReviewPending
    {
        set { reviewPending = value; }
    }

    public IFormInstanceService FormInstanceService
    {
        get { return formInstanceService; }
        set { formInstanceService = value; }
    }

    public IGetOuterDataHandler GetOuterDataHandler
    {
        set { getOuterDataHandler = value; }
    }

    public IDataListInstanceService DataListInstanceService
    {
        set { dataListInstanceService = value; }
    }
    #endregion

    #region property

    public Guid WorkflowInstanceId
    {
        get { return (Guid)(ViewState["WorkflowInstanceId"]); }
        set { ViewState["WorkflowInstanceId"] = value; }
    }

    public Guid ActivityInstanceId
    {
        get { return (Guid)(ViewState["ActitityInstanceId"]); }
        set { ViewState["ActitityInstanceId"] = value; }
    }

    public string ActivityName
    {
        get { return (string)(ViewState["ActivityName"]); }
        set { ViewState["ActivityName"] = value; }
    }

    public string ExternalEntityId
    {
        get { return ViewState["ExternalEntityId"] as string; }
        set { ViewState["ExternalEntityId"] = value; }
    }

    public Guid WorkflowId
    {
        get { return (Guid)(ViewState["WorkflowId"]); }
        set { ViewState["WorkflowId"] = value; }
    }

    public string MapImageUrl
    {
        get { return (string)(ViewState["MapImageUrl"]); }
        set { ViewState["MapImageUrl"] = value; }
    }

    public string CommentUrl
    {
        get { return (string)ViewState["CommentUrl"]; }
        set { ViewState["CommentUrl"] = value; }
    }

    private string SheetID
    {
        get { return (string)ViewState["SheetID"]; }
        set { ViewState["SheetID"] = value; }
    }

    private string Titles
    {
        get { return (string)ViewState["Title"]; }
        set { ViewState["Title"] = value; }
    }
    /// <summary>
    /// 是否允许抄送功能.
    /// </summary>
    public bool IsReview
    {
        get { return (ViewState["IsReview"] == null ? false : (bool)ViewState["IsReview"]); }
        set { ViewState["IsReview"] = value; }
    }
    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Botwave.Security.Web.LogWriterFactory.Writer = new Botwave.XQP.Service.Support.DefaultLogWriter();
            string aiid = Request.QueryString["aiid"];
            if (String.IsNullOrEmpty(aiid))
            {
                log.Info("processing activityinstanceId is null");
                ShowError(MessageHelper.Message_ArgumentException);
            }
            activityInstanceId = new Guid(aiid);
            activityInstance = activityService.GetActivity(activityInstanceId);
            Botwave.Security.LoginUser user = CurrentUser;
            string actor = user.UserName;
            CheckPermission(activityInstance, actor);

            this.wfInput.ActivityInstanceId = activityInstanceId;
            Guid activityId = activityInstance.ActivityId;
            activityDefinition = activityDefinitionService.GetActivityDefinition(activityId);

            WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);
            string workflowTitle = workflowInstance.Title;
            Titles = workflowTitle;
            Guid workflowInstanceId = workflowInstance.WorkflowInstanceId;
            Guid workflowId = workflowInstance.WorkflowId;

            this.wfInput.LoadData(workflowInstance, activityDefinition.ActivityName, activityInstanceId);
            SheetID = workflowInstance.SheetId;
            this.LoadDynamicForm(workflowId, workflowInstanceId, activityDefinition, user);

            this.SetWorkflowRemarks(user);

            this.LoadRejectActivities(workflowInstanceId, activityInstanceId, activityId, actor);

            // 只有发起人才能执行"取消"(删除)命令.
            if (workflowInstance.Creator.Equals(actor, StringComparison.OrdinalIgnoreCase))
            {
                btnCancel.Visible = true;
            }

            //如果允许手动选择路径
            if (ActivityDefinition.IsManualDecision(activityDefinition.DecisionType))
            {
                this.selectorAllocatee.WorkflowInstanceId = workflowInstanceId;
                this.selectorAllocatee.SplitCondition = activityDefinition.SplitCondition;
                this.selectorAllocatee.LoadData(workflowInstance, activityDefinition, workflowId, activityInstanceId);
            }
            else
            {
                this.selectorAllocatee.Visible = false;
            }
            string workflowName = Botwave.Workflow.Extension.Util.WorkflowUtility.GetWorkflowName(workflowId);
            
            this.Title = workflowTitle;
            this.ActivityInstanceId = activityInstanceId;
            this.ExternalEntityId = activityInstance.ExternalEntityId;
            this.WorkflowInstanceId = workflowInstanceId;
            this.WorkflowId = workflowId;

            this.historyList.OnBind(workflowName, workflowInstanceId);
            IList<CZWorkflowRelationSetting> rSettings = CZWorkflowRelationSetting.Select(workflowId).Where(r => r.Id > 0).ToList();
            if (rSettings.Count == 0)
                WorkflowRelation1.Visible = false;
            else
                //this.WorkflowRelation1.Bind(workflowInstanceId);
                this.WorkflowRelation1.BindProcess(workflowInstanceId, activityId);

            WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
            bool isReview = (wprofile != null && wprofile.IsReview == true);
            this.IsReview = isReview;
            if (isReview)
                this.reviewHistoryList.Initialize(workflowInstanceId);
            else
                this.reviewHistoryList.Visible = false;

            if (wprofile.PrintAndExp == 0)
            {
                btnExport.Visible = true;
                btnPrint.Visible = true;
            }
            else if (wprofile.PrintAndExp == 1)
            {
                btnExport.Visible = true;
                btnPrint.Visible = false;
            }
            else if (wprofile.PrintAndExp == 2)
            {
                btnExport.Visible = false;
                btnPrint.Visible = true;
            }
            else if (wprofile.PrintAndExp == 3)
            {
                btnExport.Visible = false;
                btnPrint.Visible = false;
            }

            CZActivityInstance czActivityInstance = CZActivityInstance.GetWorkflowActivity(activityInstanceId); 
            CZActivityDefinition czactivity = CZActivityDefinition.GetWorkflowActivityByActivityId(activityInstance.ActivityId);
            if (czactivity.ActivityName == activityInstance.ActivityName && czactivity.CanPrint == -1 )
            {
                btnPrint.Visible = false;
            }
            else if (czactivity.ActivityName == activityInstance.ActivityName && czactivity.CanPrint > -1 && czactivity.PrintAmount <= czActivityInstance.PrintCount && czactivity.PrintAmount > -1)
            {
                btnPrint.Visible = false;
            }

            if (czactivity.CanEdit == -1)
            {
                txtReason.Attributes.Add("onfocus", "this.blur()");
            ltlOpinion.Text+="(只读)";
            }

            this.BindScripts(wprofile);
            //this.ltlScripts.Text = "<script>" + getOuterDataHandler.GenerAutoFull(activityDefinition, workflowInstanceId) + "</script>";;

            this.relationHistory1.Bind(workflowInstanceId);
            this.workflowAttention.Bind(workflowInstanceId, 0, actor);
            //this.reviewSelector1.Initialize(wprofile, workflowId, activityDefinition.ActivityId);

            string activityName = activityDefinition.ActivityName;
            this.ActivityName = activityName;
            this.MapImageUrl = string.Format("WorkflowImage.ashx?wid={0}&aname={1}", workflowId.ToString(), HttpUtility.UrlEncode(activityName));
            this.CommentUrl = string.Format("WorkflowComment.aspx?wiid={0}&aiid={1}&t={2}&hid=true", workflowInstanceId.ToString(), activityInstanceId.ToString(), HttpUtility.UrlEncode(workflowTitle));
        }
    }

    protected void Command_Execute(object sender, CommandEventArgs e)
    {
        Botwave.Security.LoginUser user = CurrentUser;
        string actor = user.UserName;
        Guid workflowId = this.WorkflowId;
        Guid activityInstanceId = this.ActivityInstanceId;
        Guid workflowInstanceId = this.WorkflowInstanceId;

        ActivityExecutionContext context = new ActivityExecutionContext();
        context.ActivityInstanceId = activityInstanceId;
        context.Actor = actor;
        context.Command = e.CommandName.ToLower();
        context.Reason = txtReason.Value.Trim();

        IDictionary<string, object> formVariables = workflowFormService.GetFormVariables(this.Request);
        context.Variables = formVariables;

        WorkflowInstance wfInstnce = wfInput.GetInput();
        if (wfInstnce != null)
        {
            context.Variables["Secrecy"] = wfInstnce.Secrecy;
            context.Variables["Urgency"] = wfInstnce.Urgency;
            context.Variables["Importance"] = wfInstnce.Importance;
        }

        string entityId = ExternalEntityId;
        if (!String.IsNullOrEmpty(entityId))
            context.ExternalEntityId = entityId;

        if (!wfInput.IsReadonly)
            wfInput.UpdateWorkflowInstance(workflowInstanceId);

        bool isApprove = ActivityCommands.Approve.Equals(context.Command);
        if (ActivityCommands.Cancel.Equals(context.Command))//对取消特别对待.
        {
            workflowEngine.CancelWorkflow(context);
        }
        //else if (ActivityCommands.Remove.Equals(context.Command))//删除
        //{
        //    //workflowEngine.RemoveWorkflow(context);
        //}
        else
        {
            // "通过"时才检查是否选择分派用户.            
            if (isApprove)
            {
                try
                {
                    context.ActivityAllocatees = this.selectorAllocatee.GetActivityAllocatees();
                }
                catch (WorkflowAllocateException ex)
                {
                    log.Warn(ex);
                    WriteExLog(actor, "提交工单", ex.Message);
                    ShowError(ex.Message);
                }
            }
            else if (ActivityCommands.Reject.Equals(context.Command, StringComparison.OrdinalIgnoreCase))
            {
                string value = this.rblRejectActivities.SelectedValue;
                int index = value.IndexOf("_");
                if (string.IsNullOrEmpty(value) || index < 12)
                {
                    log.WarnFormat("RejectActivities，未选中退还步骤：[ActivityInstanceId:{0}][Actor:{1}][Value:{2}]", activityInstanceId, actor, value);
                }
                else
                {
                    Guid? activityId = Botwave.Commons.DbUtils.ToGuid(value.Substring(0, index));
                    string username = value.Substring(index + 1);
                    if (!activityId.HasValue || string.IsNullOrEmpty(username))
                    {
                        log.WarnFormat("RejectActivities，选中的退还步骤错误：[ActivityInstanceId:{0}][Actor:{1}][Value:{2}]", activityInstanceId, actor, value);
                    }
                    else
                    {
                        IDictionary<string, string> rejectActors = new Dictionary<string, string>();
                        rejectActors.Add(username, null);

                        context.ActivityAllocatees = new Dictionary<Guid, IDictionary<string, string>>();
                        context.ActivityAllocatees.Add(activityId.Value, rejectActors);
                    }
                }
            }


            context.Variables["CurrentUser"] = user; //添加当前用户为流程变量

            context.Variables["CurrentPage"] = this.Page; //添加当前页为流程变量

            if (activityExecutionContextHandler != null)
            {
                activityExecutionContextHandler.Handle(context, Request, workflowInstanceId);
            }

            string strCompletedActivityNames = String.Empty;
            IList<ActivityInstance> instanceList = activityService.GetActivitiesInSameWorkflow(activityInstanceId);
            foreach (ActivityInstance instance in instanceList)
            {
                if (instance.ActivityInstanceId.Equals(activityInstanceId)) continue;
                ActivityDefinition definition = activityDefinitionService.GetActivityDefinition(instance.ActivityId);
                strCompletedActivityNames += definition.ActivityName;
            }
            context.Variables["CompletedActivities"] = strCompletedActivityNames;   //添加已完成步骤为流程变量

            workflowEngine.ExecuteActivity(context);

            //保存表单内容
            if (isApprove || ActivityCommands.Save.Equals(context.Command))
            {
                IDictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("ActivityName", this.ActivityName);
                dict.Add("WorkflowInstanceId", workflowInstanceId);
                dict.Add("ButtonCreate", this.btnApprove.ClientID);
                dict.Add("ApproveButton", this.btnApprove.ClientID);

                dict.Add("CurrentUser", user); //添加当前用户为流程变量.
                dict.Add("CurrentPage", this.Page); //添加当前用户为流程变量.
                // formInstanceService.SaveForm(WorkflowInstanceId, formContext, actor);
                workflowFormService.SaveForm(workflowId, workflowInstanceId, actor, formVariables, dict);
                //保存DataList表单内容
                //dataListInstanceService.SaveDataListInstance(workflowId, workflowInstanceId, formVariables);
                dataListInstanceService.SaveDataListInstance(workflowId, workflowInstanceId, formVariables,this.Request);
            }
        }

        WriteLog(actor, context.Command, SheetID);
        if (isApprove)
        {
            WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
            if (wprofile != null && wprofile.IsReview && reviewPending != null)
            {
                string reviewValue = this.selectorAllocatee.GetReviewActorValue();
                ICollection<Guid> selectedActivities = (context.ActivityAllocatees == null ? new List<Guid>() : context.ActivityAllocatees.Keys);
                reviewPending.Pending(wprofile, new Botwave.Entities.BasicUser(user.UserName, user.RealName), wfInstnce.Title, workflowId, activityInstanceId,
                    this.selectorAllocatee.Visible, selectedActivities, ReviewSelectorHelper.ParserReiewActors(reviewValue, selectedActivities));
            }
        }
        //if (isApprove && this.IsReview)
        //{
        //    WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
        //    wprofile = (wprofile == null ? WorkflowProfile.Default : wprofile);
        //    if (wprofile.IsClassicReviewType)
        //        this.reviewSelector1.PendingReview(wprofile, activityInstanceId, workflowId, wfInstnce.Title, new Botwave.Entities.BasicUser(user.UserName, user.RealName));
        //    else
        //        this.workflowReviewService.PendingReview(wprofile, context, workflowId, wfInstnce.Title, new Botwave.Entities.BasicUser(user.UserName, user.RealName));
        //}

        if (isApprove || ActivityCommands.Reject.Equals(context.Command))
            Response.Redirect(string.Format("Notify.aspx?aiid={0}&c={1}", ActivityInstanceId, context.Command));

        ShowSuccess(MessageHelper.Message_Success, ReturnUrl);
    }

    private void CheckPermission(ActivityInstance activityInstance, string username)
    {
        if (null == activityInstance)
        {
            ShowError(MessageHelper.Message_ArgumentException);
        }

        if (activityInstance.IsCompleted)
        {
            ShowError("对不起，该步骤已处理完毕");
        }

        TodoInfo todo = taskAssignService.GetTodoInfo(activityInstance.ActivityInstanceId, username);
        if (null == todo)
        {
            ShowError("对不起，您没有权限处理该步骤");
        }

        if (TodoInfo.IsUnReaded(todo))
        {
            // 更新为已读.
            taskAssignService.UpdateTodoReaded(activityInstance.ActivityInstanceId, username, true);
        }
    }

    private static void WriteLog(string userName, string command,string sheetid)
    {
        switch (command)
        {
            case "reject":
                WriteNomalLog(userName, "退回工单", "退回工单"+sheetid+"成功");
                break;
            case "approve":
                WriteNomalLog(userName, "提交工单", "提交工单" + sheetid + "成功");
                break;
            case "cancel":
                WriteNomalLog(userName, "取消工单", "作废工单" + sheetid + "成功");
                break;
            case "save":
                WriteNomalLog(userName, "保存工单", "保存工单" + sheetid + "成功");
                break;
        }
    }

    private const string KEY_WORKFLOW_REMARKS = "User_Opinion_Remarks";

    private void SetWorkflowRemarks(Botwave.Security.LoginUser user)
    {
        //用户验收阶段，处理意见为“一般”“满意”“不满意”，“不满意”要写明原因
        IList<ActivityDefinition> nextActivityDefinition = activityDefinitionService.GetNextActivityDefinitionsByInstanceId(activityInstanceId);
        ActivityDefinition activityDefinition = activityDefinitionService.GetActivityDefinitionByInstanceId(activityInstanceId);
        if (nextActivityDefinition.Count == 1 && nextActivityDefinition[0].State == 2)
        {
            /*string acceptRemarks = @"<option value='非常满意'>非常满意</option>
                                <option value='满意'>满意</option>
                                <option value='一般'>一般</option>
                                <option value='不满意，原因：'>不满意</option>";*/
            string acceptRemarks = @"<input type='radio' name='radOption' onclick='setOption(this)' value='非常满意'>非常满意</input>
                                <input type='radio' name='radOption' onclick='setOption(this)' value='满意'>满意</input>
                                <input type='radio' name='radOption' onclick='setOption(this)' value='一般'>一般</input>
                                <input type='radio' name='radOption' onclick='setOption(this)' value='不满意'>不满意</input>";
            string remarksHtml = null;
            //获取步骤自定义处理意见
            if (activityDefinition != null)
            {
                IList<ActivityRemark> aremarks = ActivityRemark.SelectByActivityId(activityDefinition.ActivityId);
                int index = 0;
                StringBuilder sb = new StringBuilder();
                if (aremarks.Count > 0)
                    sb.Append("</br>");
                foreach (ActivityRemark aremard in aremarks)
                {
                    index++;
                    sb.AppendFormat("<input type='radio' name='radOption' onclick='setOption(this)' value='{0}'>{1}</input>", aremard.RemarkValue, aremard.RemarkText);
                    if (index % 6 == 0)
                        sb.Append("</br>");
                }
                remarksHtml = sb.ToString();

            }
            ltlRemarksOption.Text = (string.IsNullOrEmpty(remarksHtml) ? acceptRemarks : (acceptRemarks + remarksHtml));
            ltlRemarksOption.Text = acceptRemarks;
            ltlOpinion.Text = "验收意见";
            //return;
        }
        else
        {
            //公共处理意见
            /*string commonRemarks = @"<option value='同意'>同意</option>
                                    <option value='已核，请审批'>已核，请审批</option>
                                    <option value='已处理，请检验'>已处理，请检验</option>
                                    <option value='不同意，请修改'>不同意，请修改</option>
                                    <option value='不同意'>不同意</option>";*/
            string commonRemarks = @"<input type='radio' name='radOption' onclick='setOption(this)' value='同意'>同意</input>
                                <input type='radio' name='radOption' onclick='setOption(this)' value='已核，请审批'>已核，请审批</input>
                                <input type='radio' name='radOption' onclick='setOption(this)' value='已处理，请检验'>已处理，请检验</input>
                                <input type='radio' name='radOption' onclick='setOption(this)' value='不同意，请修改'>不同意，请修改</input>
                                <input type='radio' name='radOption' onclick='setOption(this)' value='不同意'>不同意</input>";
            string remarksHtml = null;

            /*if (user != null && user.Properties.ContainsKey(KEY_WORKFLOW_REMARKS))
            {
                remarksHtml = user.Properties[KEY_WORKFLOW_REMARKS].ToString();
            }
            else
            {
                if (workflowUIProfile != null)
                {
                    remarksHtml = workflowUIProfile.BuildHandlerOpinionHtml(this.Context, user);
                    if (!string.IsNullOrEmpty(remarksHtml))
                        user.Properties[KEY_WORKFLOW_REMARKS] = remarksHtml;
                }
            }*/

            //获取步骤自定义处理意见
            if (activityDefinition != null)
            {
                IList<ActivityRemark> aremarks = ActivityRemark.SelectByActivityId(activityDefinition.ActivityId);
                int index = 0;
                StringBuilder sb = new StringBuilder();
                if (aremarks.Count > 0)
                    sb.Append("</br>");
                foreach (ActivityRemark aremard in aremarks)
                {
                    index++;
                    sb.AppendFormat("<input type='radio' name='radOption' onclick='setOption(this)' value='{0}'>{1}</input>", aremard.RemarkValue, aremard.RemarkText);
                    if (index % 6 == 0)
                        sb.Append("</br>");
                }
                remarksHtml = sb.ToString();

            }
            ltlRemarksOption.Text = (string.IsNullOrEmpty(remarksHtml) ? commonRemarks : (commonRemarks + remarksHtml));
            ltlOpinion.Text = "处理意见";
        }
    }

    //载入动态表单

    private void LoadDynamicForm(Guid workflowId, Guid workflowInstanceId, ActivityDefinition activityDefinition, Botwave.Security.LoginUser user)
    {
        user = (user == null ? new Botwave.Security.LoginUser() : user);

        IDictionary<string, object> dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        dict.Add("Activity", activityDefinition);
        dict.Add("ActivityName", activityDefinition.ActivityName);
        dict.Add("WorkflowInstanceId", workflowInstanceId);
        dict.Add("ButtonCreate", this.btnApprove.ClientID);
        dict.Add("ApproveButton", this.btnApprove.ClientID);

        dict.Add("CurrentUser", user); //添加当前用户为流程变量.
        dict.Add("CurrentPage", this.Page); //添加当前用户为流程变量.

        divDynamicFormContainer.InnerHtml = workflowFormService.LoadForm(workflowId, workflowInstanceId, dict);
        divDynamicFormContainer.InnerHtml += BindFormItems(activityDefinition.WorkflowId, workflowInstanceId, activityDefinition, user);
        divDynamicFormContainer.InnerHtml += CommonDynamicFormScript(divDynamicFormContainer.ClientID, activityDefinition);
    }

    #region 动态表单公用脚本


    /// <summary>
    /// 动态表单公用脚本.
    /// </summary>
    /// <param name="dynamicContainerClientID"></param>
    /// <param name="activityDefinition"></param>
    /// <returns></returns>
    private static string CommonDynamicFormScript(string dynamicContainerClientID, ActivityDefinition activityDefinition)
    {
        
        /*string commonDynamicFormScript = @"
        <script language=""javascript"" type=""text/javascript"">
        //绑定下拉、单选、多选

        for (var i = 0, icount = __selectionItems__.length; i < icount; i++){
            bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
        } 

        //工单处理时默认表单项为只读,除了已设置为可编辑的表单项

	    $('#{0} input').each(function(){
	        var inputType = $(this).attr('type');
	        if($(this).attr('contentEditable') == 'true' || inputType == 'hidden' || {1} == 0)
	            return;    		        
	        if(inputType == 'checkbox' || inputType == 'radio'){
	            if($(this).attr('checked')){
	                $(this).after($(this).val());	
	                $(this).click(function(){return false;});
                }
                else $(this).hide();		                    
                
                $(this).next('<span>').hide();
            }
	        else{ 
                $(this).hide();
                if(inputType == 'button') return;
                $(this).after($(this).val());
                if($(this).next('.ico_pickdate'))
                    $(this).next('.ico_pickdate').hide();
            }              
	    });
	    $('#{0} textarea').each(function(){
	        if($(this).attr('contentEditable') == 'true' || {1} == 0)
                return; 
            $(this).attr('readonly','readonly');
            $(this).attr('style','width:90%;overflow:auto;border:0;background:#FFF; ');
            // $(this).mouseover(function(){adjustTextArea(this);});
            adjustTextArea(this);
	    });
	    $('#{0} select').each(function(){
            if($(this).attr('contentEditable') == 'true' || {1} == 0)
                return; 
            $(this).hide('fast');
            $(this).after($(this)[0].options[$(this)[0].selectedIndex].text);
	    }); 
        </script>";
        commonDynamicFormScript = commonDynamicFormScript.Replace("{0}", dynamicContainerClientID);
        commonDynamicFormScript = commonDynamicFormScript.Replace("{1}", activityDefinition.State.ToString());
        return commonDynamicFormScript;*/
        return string.Empty;
    }

    protected void BindScripts(WorkflowProfile workflowProfile)
    {
        StringBuilder builder = new StringBuilder();
        //builder.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
        //builder.AppendLine(ReviewSelectorHelper.BuildValidateScript(workflowProfile, this.selectorAllocatee.Visible));
        //builder.AppendLine("</script>");
        //activityDefinition = activityDefinitionService.GetActivityDefinition(this.activityid);
        //builder.Append("<script>" + getOuterDataHandler.GenerAutoFull(this.WorkflowId) + "</script>");
        //this.ltlScripts.Text = builder.ToString();
    }

    /// <summary>
    /// 绑定外部数据源
    /// </summary>
    /// <returns></returns>
    protected string BindFormItems(Guid workflowId, Guid workflowInstanceId, ActivityDefinition activityDefinition, Botwave.Security.LoginUser user)
    {
        string bindFormScript = string.Format(@"
        <script language=""javascript"" type=""text/javascript"">
        var FormDataSet = getFormDataSet('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
        getDataSet('{0}','{1}', FormDataSet, {8}, '{9}');
        ItemIframe.LoadIframe('{0}', '{1}', '{7}', FormDataSet);
        ItemDataList.LoadDataList('{0}', '{1}', FormDataSet, {8});
        //WorkflowExtension.GenerRules('{0}', '{1}', '{7}', '{4}', FormDataSet);
        </script>", workflowId.ToString(), workflowInstanceId.ToString(), this.Titles, this.SheetID, CurrentUserName, CurrentUser.DpId, this.activityInstanceId.ToString(), activityDefinition.ActivityName, activityDefinition.State, divDynamicFormContainer.ClientID);
        bindFormScript+=string.Format(@"
        <script>
          {0}
        </script>", getOuterDataHandler.GenerAutoFull(activityDefinition, workflowInstanceId));
        return bindFormScript;
    }
    #endregion

    #region 退还

    void LoadRejectActivities(Guid workflowInstanceId, Guid activityInstanceId, Guid activityId, string actor)
    {
        DataTable rejectTable = Botwave.XQP.Service.WorkflowProfileHelper.GetWorkflowRejectActivities(workflowInstanceId, activityInstanceId, activityId, actor);
        rblRejectActivities.DataSource = rejectTable;
        rblRejectActivities.DataTextField = "ActivityName";
        rblRejectActivities.DataValueField = "Names";
        rblRejectActivities.DataBind();

        if (rblRejectActivities != null && rblRejectActivities.Items.Count > 0)
            rblRejectActivities.Items[0].Selected = true;
    }

    #endregion
    protected void btnExport_Click(object sender, EventArgs e)
    {
        Botwave.XQP.ImportExport.WorkflowExporter.ExportZip(this.Context, this.WorkflowInstanceId, this.SheetID, SheetID + "_" + Titles);
       // Botwave.XQP.ImportExport.WorkflowExporter.ExportWord(Response, this.WorkflowInstanceId, SheetID + "_" + Titles);
    }
}
