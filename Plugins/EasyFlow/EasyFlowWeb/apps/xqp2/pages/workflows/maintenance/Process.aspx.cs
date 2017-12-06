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
using Botwave.Security;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;
using Botwave.XQP.API.Entity;
using Botwave.Security.Domain;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Binders;
using Botwave.Report.DataAccess;
using Botwave.DynamicForm.Extension.Implements;

public partial class apps_xqp2_pages_workflows_maintenance_Process : Botwave.Security.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_xqp2_pages_workflows_maintenance_Process));
    private static readonly string ReturnUrl = WebUtils.GetAppPath() + "contrib/workflow/pages/default.aspx";

    private Guid activityInstanceId;
    private ActivityDefinition activityDefinition;
    public ActivityInstance activityInstance;
    private static bool isVirtualGoup;
    private Guid guid = Guid.NewGuid();

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
    private IWorkflowMaintenanceService workflowMaintenanceService = (IWorkflowMaintenanceService)Ctx.GetObject("workflowMaintenanceService");

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

    public IWorkflowMaintenanceService WorkflowMaintenanceService
    {
        set { workflowMaintenanceService = value; }
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

    /// <summary>
    /// 是否允许抄送功能.
    /// </summary>
    public bool IsReview
    {
        get { return (ViewState["IsReview"] == null ? false : (bool)ViewState["IsReview"]); }
        set { ViewState["IsReview"] = value; }
    }

    /// <summary>
    /// 流程
    /// </summary>
    public string Workflow_Name
    {
        get { return (string)(ViewState["Workflow_Name"]); }
        set { ViewState["Workflow_Name"] = value; }
    }
    public string _uuid
    {
        get { return (string)ViewState["_uuid"]; }
        set { ViewState["_uuid"] = value; }
    }
    public string jsondata
    {
        get { return (string)ViewState["jsondata"]; }
        set { ViewState["jsondata"] = value; }
    }
    public string HR_Userid
    {
        get { return (string)ViewState["HR_Userid"]; }
        set { ViewState["HR_Userid"] = value; }
    }
    public string DpId
    {
        get { return (string)ViewState["DpId"]; }
        set { ViewState["DpId"] = value; }
    }
    public int OperateType
    {
        get { return (int)ViewState["OperateType"]; }
        set { ViewState["OperateType"] = value; }
    }
    public string SheetID
    {
        get { return (string)ViewState["SheetID"]; }
        set { ViewState["SheetID"] = value; }
    }
    private string Titles
    {
        get { return (string)ViewState["Title"]; }
        set { ViewState["Title"] = value; }
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
            string wiid = Request.QueryString["wiid"];
            LblUserName.Text = CurrentUser.RealName;
            LblPhone.Text = CurrentUser.Mobile;
            LblDepId.Text = CurrentUser.DpId;
            if (String.IsNullOrEmpty(wiid))
            {
                log.Info("processing workflowinstanceId is null");
                ShowError(MessageHelper.Message_ArgumentException);
            }
            Guid activityInstanceId = Guid.Empty;
            Guid workflowInstanceId = Guid.Empty;
            workflowInstanceId = new Guid(wiid);
            activityInstance = activityService.GetCurrentActivity(workflowInstanceId);
            Botwave.Security.LoginUser user = CurrentUser;
            DpId = user.DpId;
            string actor = user.UserName;
            //CheckPermission(activityInstance, actor);

            this.wfInput.ActivityInstanceId = activityInstance.ActivityInstanceId;

            Guid _activityId = activityInstance.ActivityId;
            activityDefinition = activityDefinitionService.GetActivityDefinition(_activityId);

            WorkflowInstance workflowInstance = workflowService.GetWorkflowInstance(workflowInstanceId);
            string workflowTitle = workflowInstance.Title;
            //Guid workflowInstanceId = new Guid(workflowInstance.WorkflowInstanceId.ToUpper());
            Guid workflowId = workflowInstance.WorkflowId;

            this.wfInput.LoadData(workflowInstance, activityDefinition.ActivityName, activityInstanceId);
            SheetID = workflowInstance.SheetId;
            this.LoadDynamicForm(workflowId, workflowInstanceId, activityDefinition, user);

            string workflowName = Workflow_Name = Botwave.Workflow.Extension.Util.WorkflowUtility.GetWorkflowName(workflowId);//获取流程名称
            Titles = workflowTitle;
            //CheckPermission(activityInstance, actor);
     
            this.Title = workflowTitle;
            this.ActivityInstanceId = activityInstanceId;
            this.ExternalEntityId = activityInstance.ExternalEntityId;
            this.WorkflowInstanceId = workflowInstanceId;
            this.WorkflowId = workflowId;

            this.historyList.OnBind(workflowName, workflowInstanceId);

            WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
            bool isReview = (wprofile != null && wprofile.IsReview == true);
            this.IsReview = isReview;
            if (isReview)
                this.reviewHistoryList.Initialize(workflowInstanceId);
            //else
            //    this.reviewHistoryList.Visible = false;
            else
            {
                this.reviewHistoryList.Visible = false;
            }
            this.BindScripts(wprofile);
            //this.reviewSelector1.Initialize(wprofile, workflowId, activityDefinition.ActivityId);

            string activityName = activityDefinition.ActivityName;
            this.ActivityName = activityName;
            //this.MapImageUrl = string.Format("WorkflowImage.ashx?wid={0}&aname={1}", workflowId.ToString(), HttpUtility.UrlEncode(activityName));
            this.MapImageUrl = string.Format(AppPath + "contrib/workflow/pages/WorkflowImage.ashx?wid={0}&aname={1}&wname={2}", workflowId.ToString(), HttpUtility.UrlEncode(activityName), HttpUtility.UrlEncode(workflowName));
            this.CommentUrl = string.Format(AppPath + "contrib/workflow/pages/WorkflowComment.aspx?wiid={0}&aiid={1}&t={2}&hid=true", workflowInstanceId.ToString(), activityInstanceId.ToString(), HttpUtility.UrlEncode(workflowTitle));
            //this.CommentUrl = string.Format("WorkflowCommentEdit.aspx?wiid={0}&aiid={1}&t={2}&hid=true", workflowInstanceId.ToString(), activityInstanceId.ToString(), HttpUtility.UrlEncode(workflowInstance.SheetId));
            this.txtReason.Value = activityInstance.Reason;//处理意见
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
        context.Reason = Server.HtmlEncode(txtReason.Value.Trim().Replace("'","‘"));

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
                    //context.ActivityAllocatees = this.selectorAllocatee.GetActivityAllocatees();
                }
                catch (WorkflowAllocateException ex)
                {
                    log.Warn(ex);
                    //WriteExLog(actor, "提交工单", ex.Message);
                    ShowError(ex.Message);
                }
            }

            context.Variables["CurrentUser"] = user; //添加当前用户为流程变量

            context.Variables["CurrentPage"] = this.Page; //添加当前页为流程变量
            context.Variables["Now"] = DateTime.Now;

            if (activityExecutionContextHandler != null)
            {
                activityExecutionContextHandler.Handle(context, Request, workflowInstanceId);
            }

            string strCompletedActivityNames = String.Empty;
            IList<ActivityInstance> instanceList = activityService.GetActivitiesInSameWorkflow(activityInstanceId);
            //IList<ActivityInstance> instanceList = activityService.GetActivitiesInSameWorkflowByWiid(workflowInstanceId);//20120222kangliu

            Guid _activityId;
            foreach (ActivityInstance instance in instanceList)
            {
                if (instance.ActivityInstanceId.Equals(activityInstanceId)) continue;
                _activityId = instance.ActivityId;
                ActivityDefinition definition = activityDefinitionService.GetActivityDefinition(_activityId);
                strCompletedActivityNames += definition.ActivityName;
            }
            context.Variables["CompletedActivities"] = strCompletedActivityNames;   //添加已完成步骤为流程变量
            //context.Variables["SelectReturnWay"] = ((RadioButton)selectReturnWay.FindControl("RadioBtn_App")).Checked ? "initial" : "previous";//加入退回方式
            string isReject = context.Command;
            //workflowEngine.ExecuteActivity(context);

            #region 退回调用接口
            //if (ActivityCommands.Reject.Equals(isReject))//退回
            //{
            //    string aiid = Request.QueryString["aiid"];
            //    activityInstanceId = new Guid(aiid);
            //    activityInstance = activityService.GetActivity(activityInstanceId);
            //    if (((RadioButton)selectReturnWay.FindControl("RadioBtn_App")).Checked)//直接退回给申请人
            //    {
            //        IWuYe.ApplicantState(Workflow_Name, workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId).WorkflowInstanceId.ToUpper(), "0");
            //        IHRSys.ReturnState(Workflow_Name, workflowInstanceId, "-2");
            //    }
            //    else
            //    {
            //        IList<ActivityInstance> instanceList_Reject = activityService.GetCompletedActivitiesOfPrevDefinitionByCurrent(activityInstance);
            //        string Prev_str = "";
            //        foreach (ActivityInstance ai in instanceList_Reject)
            //        {
            //            Prev_str = ai.PrevSetId;
            //        }
            //        if (!string.IsNullOrEmpty(Prev_str) && Prev_str == "00000000-0000-0000-0000-000000000000")//退回到提单步骤
            //        {
            //            IWuYe.ApplicantState(Workflow_Name, workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId).WorkflowInstanceId.ToUpper(), "0");
            //            IHRSys.ReturnState(Workflow_Name, workflowInstanceId, "-2");
            //        }
            //    }
            //}
            #endregion

            //保存表单内容
            if (isApprove || ActivityCommands.Save.Equals(context.Command))
            {
                IDictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("ActivityName", this.ActivityName);
                dict.Add("WorkflowInstanceId", workflowInstanceId);
                //dict.Add("ButtonCreate", this.btnApprove.ClientID);
                //dict.Add("ApproveButton", this.btnApprove.ClientID);

                dict.Add("CurrentUser", user); //添加当前用户为流程变量.
                dict.Add("CurrentPage", this.Page); //添加当前用户为流程变量.
                // formInstanceService.SaveForm(WorkflowInstanceId, formContext, actor);
                workflowFormService.SaveForm(workflowId, workflowInstanceId, actor, formVariables, dict);
                //保存DataList表单内容
                //dataListInstanceService.SaveDataListInstance(workflowId, workflowInstanceId, formVariables);
                dataListInstanceService.SaveDataListInstance(workflowId, workflowInstanceId, formVariables, this.Request);
                //保存处理意见
                workflowMaintenanceService.WorkflowProcessHistoryUpdate(workflowInstanceId,this.Request);
            }
            //获取当前选择的下一步骤
            string _activityAllocatees = Request.Form["activityAllocatee"];
            
        }

        WriteLog(actor, context.Command,SheetID);
        //if (isApprove && this.IsReview)
        //{
        //    WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
        //    wprofile = (wprofile == null ? WorkflowProfile.Default : wprofile);
        //    if (wprofile.IsClassicReviewType)
        //        this.reviewSelector1.PendingReview(wprofile, activityInstanceId, workflowId, wfInstnce.Title, new Botwave.Entities.BasicUser(user.UserName, user.RealName));
        //    else
        //        this.workflowReviewService.PendingReview(wprofile, context, workflowId, wfInstnce.Title, new Botwave.Entities.BasicUser(user.UserName, user.RealName));
        //}

        //if (isApprove || ActivityCommands.Reject.Equals(context.Command))
        /*if (isApprove || ActivityCommands.ReturnToDraft.Equals(context.Command))
            Response.Redirect(string.Format("Notify.aspx?aiid={0}&c={1}", ActivityInstanceId, context.Command));
        //if (AlertSuccess("成功处理工单.")) { return; }//深圳联通物业系统特殊处理
        if (isVirtualGoup && ActivityCommands.Save.Equals(context.Command))//工单池工单特殊处理
            Response.Redirect(WebUtils.GetAppPath() + "apps/xqp2/pages/security/VirtualUser.aspx");
        ShowSuccess(MessageHelper.Message_Success, ReturnUrl);*/
        //Response.Redirect(string.Format("Notify.aspx?aiid={0}&c={1}&son=son", ActivityInstanceId, context.Command));
        ShowSuccess(MessageHelper.Message_Success, string.Format(AppPath+"apps/xqp2/pages/workflows/maintenance/process.aspx?wiid={0}",workflowInstanceId));
    }

    

    private static void WriteLog(string userName, string command,string sheetId)
    {
        switch (command)
        {
            case "reject":
                WriteNomalLog(userName, "退回工单", "退回工单成功");
                break;
            case "approve":
                WriteNomalLog(userName, "提交工单", "提交工单成功");
                break;
            case "cancel":
                WriteNomalLog(userName, "取消工单", "作废工单成功");
                break;
            case "save":
                WriteNomalLog(userName, "保存工单", "修改工单" + sheetId + "内容成功");
                break;
        }
    }

    private const string KEY_WORKFLOW_REMARKS = "User_Opinion_Remarks";


    //载入动态表单

    private void LoadDynamicForm(Guid workflowId, Guid workflowInstanceId, ActivityDefinition activityDefinition, Botwave.Security.LoginUser user)
    {

        user = (user == null ? new Botwave.Security.LoginUser() : user);

        IDictionary<string, object> dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        dict.Add("Activity", activityDefinition);
        dict.Add("ActivityName", activityDefinition.ActivityName);
        dict.Add("WorkflowInstanceId", workflowInstanceId);
        //dict.Add("ButtonCreate", this.btnApprove.ClientID);
        //dict.Add("ApproveButton", this.btnApprove.ClientID);

        dict.Add("CurrentUser", user); //添加当前用户为流程变量.
        dict.Add("CurrentPage", this.Page); //添加当前用户为流程变量.
        dict.Add("Now", DateTime.Now);

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

        return string.Empty ;
    }

    protected void BindScripts(WorkflowProfile workflowProfile)
    {
        StringBuilder builder = new StringBuilder();
        //builder.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
        //builder.AppendLine(ReviewSelectorHelper.BuildValidateScript(workflowProfile, this.selectorAllocatee.Visible));
        //builder.AppendLine("</script>");
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
        </script>", workflowId.ToString(), workflowInstanceId.ToString(), this.Title, this.SheetID, CurrentUserName, CurrentUser.DpId, this.activityInstanceId.ToString(), activityDefinition.ActivityName, activityDefinition.State, divDynamicFormContainer.ClientID);
        bindFormScript += string.Format(@"
        <script>
          {0}
        </script>", getOuterDataHandler.GenerAutoFull(activityDefinition, workflowInstanceId));
        return bindFormScript;
    }
    #endregion

    protected void btnExport_Click(object sender, EventArgs e)
    {
        Botwave.XQP.ImportExport.WorkflowExporter.ExportWord(Response, this.WorkflowInstanceId, SheetID + "_" + Titles);
    }
}
