using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using Botwave.Web;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;

public partial class contrib_mobile_pages_WorkflowView : Botwave.XQP.Web.Security.PageBase
{
    private static string returnUrl = null;

    #region service interfaces

    private IWorkflowEngine workflowEngine = (IWorkflowEngine)Ctx.GetObject("workflowEngine");
    private IWorkflowService workflowService = (IWorkflowService)Ctx.GetObject("workflowService");
    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IActivityService activityService = (IActivityService)Ctx.GetObject("activityService");
    private ICountersignedService countersignedService = (ICountersignedService)Ctx.GetObject("countersignedService");
    private IWorkflowFormService workflowFormService = (IWorkflowFormService)Ctx.GetObject("workflowFormService");
    private IWorkflowFormService mobileFormService = (IWorkflowFormService)Ctx.GetObject("mobileFormService");
    private IWorkflowResourceService workflowResourceService = (IWorkflowResourceService)Ctx.GetObject("workflowResourceService");
    private IFormInstanceService formInstanceService = (IFormInstanceService)Ctx.GetObject("formInstanceService");
    private IGetOuterDataHandler getOuterDataHandler = (IGetOuterDataHandler)Ctx.GetObject("getOuterDataHandler");
    private IWorkflowMobileService workflowMobileService = (IWorkflowMobileService)Ctx.GetObject("workflowMobileService");
    public IWorkflowEngine WorkflowEngine
    {
        set { workflowEngine = value; }
    }

    public IWorkflowService WorkflowService
    {
        set { workflowService = value; }
    }

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public IActivityService ActivityService
    {
        set { activityService = value; }
    }

    public ICountersignedService CountersignedService
    {
        set { countersignedService = value; }
    }

    public IWorkflowFormService WorkflowFormService
    {
        set { workflowFormService = value; }
    }

    public IWorkflowFormService MobileFormService
    {
        set { mobileFormService = value; }
    }

    public IWorkflowResourceService WorkflowResourceService
    {
        set { workflowResourceService = value; }
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

    public IWorkflowMobileService WorkflowMobileService
    {
        set { this.workflowMobileService = value; }
    }
    #endregion

    #region gets / sets

    /// <summary>
    /// 流程图链接.
    /// </summary>
    public string ImageUrl
    {
        get { return ViewState["ImageUrl"].ToString(); }
        set { ViewState["ImageUrl"] = value; }
    }

    public Guid WorkflowInstanceId
    {
        get { return (Guid)ViewState["WorkflowInstanceId"]; }
        set { ViewState["WorkflowInstanceId"] = value; }
    }

    public Guid ActivityInstanceId
    {
        get { return (Guid)ViewState["ActivityInstanceId"]; }
        set { ViewState["ActivityInstanceId"] = value; }
    }

    public string CommentUrl
    {
        get { return (string)ViewState["CommentUrl"]; }
        set { ViewState["CommentUrl"] = value; }
    }

    public string UserName
    {
        get { return (string)ViewState["UserName"]; }
        set { ViewState["UserName"] = value; }
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

    public string ActivityName
    {
        get { return (ViewState["ActivityName"] == null ? "" : (string)ViewState["ActivityName"]); }
        set { ViewState["ActivityName"] = value; }
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
            string aiid = Request.QueryString["aiid"];
            string wiid = Request.QueryString["wiid"];
            string type = Request.QueryString["type"];
            if (string.IsNullOrEmpty(aiid) && string.IsNullOrEmpty(wiid))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }
            // 是否属于待阅类型.
            bool isReview = (!string.IsNullOrEmpty(type) && type.Equals("review", StringComparison.OrdinalIgnoreCase));

            Guid activityInstanceId = Guid.Empty;
            Guid workflowInstanceId = Guid.Empty;
            ActivityInstance activityInstance;
            if (!string.IsNullOrEmpty(aiid))
            {
                activityInstanceId = new Guid(aiid);
                activityInstance = activityService.GetActivity(activityInstanceId);
            }
            else
            {
                workflowInstanceId = new Guid(wiid);
                activityInstance = activityService.GetCurrentActivity(workflowInstanceId);
            }

            if (null == activityInstance)
                ShowError(MessageHelper.Message_ArgumentException);
            else
                activityInstanceId = activityInstance.ActivityInstanceId;

            if (!workflowMobileService.IsAvtivityMobile(activityInstance.ActivityId))
            {
                ShowError("流程不支持手机查看。如需使用，请联系流程管理员！");
            }
            Botwave.Security.LoginUser user = CurrentUser;
            string currentUserName = user.UserName.ToLower();
            this.UserName = currentUserName;

            bool isComplete = activityInstance.IsCompleted;
            // 如果工单未完成并且当前登录用户在待办列表中, 转向到处理页面.
            if (!isComplete && !isReview)
                this.RedirectProcessPage(currentUserName, activityInstanceId);

            WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);

            string workflowTitle = workflowInstance.Title;
            this.Title = workflowTitle;
            Titles = workflowTitle;
            SheetID = workflowInstance.SheetId;
            workflowInstanceId = workflowInstance.WorkflowInstanceId;
            Guid workflowId = workflowInstance.WorkflowId;

            string actor = activityInstance.Actor; // 流程步骤操作人.
            string activityName = ((workflowInstance.State == 99) ? "取消" : activityInstance.ActivityName);

            // 当前用户非操作人时，检查用户是否具有查看保密单的权限.
            if (!StringCompare(currentUserName, actor))
            {
                IDictionary<string, string> userResources = user.Resources;
                this.LoadAccessController(currentUserName, userResources, workflowId, workflowInstanceId, workflowInstance.Secrecy);
            }

            Botwave.XQP.Domain.WorkflowProfile profile = Botwave.XQP.Domain.WorkflowProfile.LoadByWorkflowId(workflowInstance.WorkflowId);
            //if (profile.PrintAndExp == 0)
            //{
            //    btnExport.Visible = true;
            //    btnPrint.Visible = true;
            //}
            //else if (profile.PrintAndExp == 1)
            //{
            //    btnExport.Visible = true;
            //    btnPrint.Visible = false;
            //}
            //else if (profile.PrintAndExp == 2)
            //{
            //    btnExport.Visible = false;
            //    btnPrint.Visible = true;
            //}
            //else if (profile.PrintAndExp == 3)
            //{
            //    btnExport.Visible = false;
            //    btnPrint.Visible = false;
            //}
            IList<CZWorkflowRelationSetting> rSettings = CZWorkflowRelationSetting.Select(workflowId).Where(r => r.Id > 0).ToList();
            if (rSettings.Count == 0)
                WorkflowRelation1.Visible = false;
            else
                this.WorkflowRelation1.Bind(workflowInstanceId);
            //判断是否能打印
            if (workflowInstance.State >= 2)
            {
                //if (CurrentUserName != workflowInstance.Creator)//工单完成后只能由提单人进行打印
                //    btnPrint.Visible = false;
                //CZWorkflowInstance czWorkflowInstance = CZWorkflowInstance.GetWorkflowInstance(workflowInstance.WorkflowInstanceId);
                //WorkflowProfile profile = WorkflowProfile.LoadByWorkflowId(workflowInstance.WorkflowId);

                //if (profile.PrintAmount > -1 && czWorkflowInstance.PrintCount >= profile.PrintAmount)
                //{
                //    btnPrint.Visible = false;
                //    //Response.Write("<script>alert('工单只能打印" + profile.PrintAmount + "次，目前已打印" + czWorkflowInstance.PrintCount + "次！');</script>");
                //}
            }
            else
            {
                //CZActivityInstance czActivityInstance = CZActivityInstance.GetWorkflowActivity(activityInstance.ActivityInstanceId);
                //CZActivityDefinition czactivity = CZActivityDefinition.GetWorkflowActivityByActivityId(czActivityInstance.ActivityId);
                //if (czactivity.ActivityName == activityInstance.ActivityName && czactivity.CanPrint == -1)
                //{
                //    btnPrint.Visible = false;
                //}
                //else if (czactivity.ActivityName == activityInstance.ActivityName && czactivity.CanPrint > -1 && czactivity.PrintAmount <= czActivityInstance.PrintCount && czactivity.PrintAmount > -1)
                //{
                //    btnPrint.Visible = false;
                //}
            }

            //控件数据绑定
            string workflowName = Botwave.Workflow.Extension.Util.WorkflowUtility.GetWorkflowName(workflowId);
            ucWorkItemView.LoadData(workflowInstance, activityName, activityInstanceId);
            ucCompletWorkFlowList.OnBind(workflowName, workflowInstanceId);
            ucAttachment.LoadData(workflowInstance);
            this.reviewHistoryList.Initialize(workflowId, workflowInstanceId);

            //this.relationHistory1.Bind(workflowInstanceId);
            //this.workflowAttention.Bind(workflowInstanceId, 1, currentUserName);

            LoadDynamicForm(workflowId, workflowInstanceId, activityInstance, user);

            // 流程步骤图片
            this.ImageUrl = string.Format(AppPath+"contrib/workflow/pages/WorkflowImage.ashx?wid={0}&aname={1}", workflowId, HttpUtility.UrlEncode(activityName));
            //this.workflowImage.ImageUrl = this.ImageUrl + "&width=700";

            //if (activityInstance.PrevSetId != Guid.Empty || workflowInstance.State != WorkflowConstants.Executing || !activityService.IsRetractable(workflowInstanceId, activityInstanceId))
            if (workflowInstance.State >= 2 || activityInstance.PrevSetId != Guid.Empty
                || workflowInstance.State != WorkflowConstants.Executing
                || workflowInstance.Creator != currentUserName)
            {
                this.btnRetract.Visible = false; // 是否显示撤回按钮
            }

            this.WorkflowInstanceId = workflowInstanceId;
            this.ActivityInstanceId = activityInstanceId;
            this.CommentUrl = string.Format(AppPath + "contrib/workflow/pages/WorkflowComment.aspx?wiid={0}&aiid={1}&actor={2}&t={3}", workflowInstanceId.ToString(), activityInstanceId.ToString(), HttpUtility.UrlEncode(actor), HttpUtility.UrlEncode(workflowTitle));

            if (null != Request.UrlReferrer)
                returnUrl = Request.UrlReferrer.PathAndQuery;
            if (Botwave.XQP.Domain.ToReview.EnableReview(activityInstanceId, currentUserName))
                this.btnReview.Visible = true;


        }
    }

    #region 初始化


    /// <summary>
    /// 如果工单未完成并且当前登录用户在待办列表中, 转向到处理页面.
    /// </summary>
    /// <param name="currentUserName"></param>
    /// <param name="activityInstanceId"></param>
    protected void RedirectProcessPage(string currentUserName, Guid activityInstanceId)
    {
        bool isActor = false;
        IList<TodoInfo> todolist = countersignedService.GetTodoList(activityInstanceId);
        foreach (TodoInfo todoInfo in todolist)
        {
            if (StringCompare(currentUserName, todoInfo.UserName))
            {
                isActor = true;
                break;
            }
        }
        if (isActor)
            Response.Redirect(String.Format("Process.aspx?aiid={0}&returnurl={1}", activityInstanceId, Request.QueryString["returnurl"]));
    }

    /// <summary>
    /// 加载当前登录用户的访问控制(检查用户是否具有查看保密单的权限.).
    /// </summary>
    /// <param name="currentUserName"></param>
    /// <param name="userResources"></param>
    /// <param name="workflowId"></param>
    /// <param name="workflowInstanceId"></param>
    /// <param name="secrecy"></param>
    protected void LoadAccessController(string currentUserName, IDictionary<string, string> userResources, Guid workflowId, Guid workflowInstanceId, int secrecy)
    {
        IList<string> actors = WorkflowUtility.GetWorkflowProcessors(workflowInstanceId);    // 处理人列表.
        IList<string> reviewActors = Botwave.XQP.Domain.ToReview.GetReviewActors(workflowInstanceId); // 抄送人列表.

        // 检查用户是否具有查看附件与保密单的权限.
        if (!(actors.Contains(currentUserName) || reviewActors.Contains(currentUserName)))
        {
            //divAttachments.Visible = false;//取消附件查看限制
            if (secrecy == 1)
            {
                string workflowResourceId = workflowResourceService.GetWorkflowResourceId(workflowId);
                if (!string.IsNullOrEmpty(workflowResourceId))
                {
                    if (!userResources.ContainsKey(workflowResourceId + "0003"))
                        ShowError("对不起，您无权限查看该保密单.");
                }
            }
        }
    }

    /// <summary>
    /// 载入动态表单.
    /// </summary>
    /// <param name="workflowId"></param>
    /// <param name="workflowInstanceId"></param>
    /// <param name="activityInstance"></param>
    private void LoadDynamicForm(Guid workflowId, Guid workflowInstanceId, ActivityInstance activityInstance, Botwave.Security.LoginUser user)
    {
        user = (user == null ? new Botwave.Security.LoginUser() : user);
        ActivityDefinition activityDefinition = activityDefinitionService.GetActivityDefinition(activityInstance.ActivityId);

        IDictionary<string, object> dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        dict.Add("Activity", activityDefinition);
        dict.Add("ActivityName", activityDefinition.ActivityName);
        dict.Add("WorkflowInstanceId", workflowInstanceId);
        dict.Add("ButtonCreate", string.Empty);
        dict.Add("ApproveButton", string.Empty);

        dict.Add("CurrentUser", user); //添加当前用户为流程变量.
        dict.Add("CurrentPage", this.Page); //添加当前用户为流程变量.

        this.divDynamicFormContainer.InnerHtml = mobileFormService.LoadForm(workflowId, workflowInstanceId, dict);
        //this.divDynamicFormContainer.InnerHtml += CommonDynamicFormScript();
        this.divDynamicFormContainer.InnerHtml += BindFormItems(workflowId, workflowInstanceId, activityInstance.ActivityInstanceId, activityDefinition, user);
    }

    #endregion

    protected void btnRetract_Click(object sender, EventArgs e)
    {
        string actor = this.UserName;
        ActivityExecutionContext context = new ActivityExecutionContext();
        context.Actor = actor;
        context.ActivityInstanceId = this.ActivityInstanceId;
        context.Command = ActivityCommands.Withdraw;
        workflowEngine.ExecuteActivity(context);
        ShowSuccess("撤回工单成功.", AppPath + "contrib/mobile/pages/default.aspx");
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        if (null != Request.UrlReferrer)
            returnUrl = Request.UrlReferrer.AbsoluteUri;
        if (!string.IsNullOrEmpty(returnUrl))
            Response.Redirect(returnUrl);

        Response.Redirect(AppPath + "contrib/mobile/pages/default.aspx");
    }

    protected void btnReview_Click(object sender, EventArgs e)
    {
        string actor = this.UserName;
        Guid activityInstanceId = this.ActivityInstanceId;
        Botwave.XQP.Domain.ToReview.UpdateReview(activityInstanceId, actor);
        Botwave.XQP.Domain.ToReview.DeletePendingMsg(activityInstanceId, actor);

        ShowSuccess("操作成功.", AppPath + "contrib/mobile/pages/toreview.aspx");
    }


    #region 动态表单公用脚本

    private string CommonDynamicFormScript()
    {
        string commonDynamicFormScript = @"
        <script language='javascript' type='text/javascript'>
        <!--//
        //绑定下拉、单选、多选
        for (var i = 0, icount = __selectionItems__.length; i < icount; i++){
            bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
        }

        //工单查看时默认表单项为只读
	    $('#{0} input').each(function(){
            var inputType = $(this).attr('type');
            if(inputType == 'hidden') return;
            if(inputType == 'checkbox' || inputType == 'radio'){
                if($(this).attr('checked')){
                    $(this).after($(this).val());
                    $(this).click(function(){return false;});
                }
                else $(this).hide();	

                $(this).next('span').hide();
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
            $(this).attr('readonly','readonly');
            $(this).attr('style','width:90%;overflow:auto;border:0;background:#FFF; ');
            adjustTextArea(this);
        });
        $('#{0} select').each(function(){
            $(this).hide();
            $(this).after($(this)[0].options[$(this)[0].selectedIndex].text);
        }); 
        //-->
        </script>";
        commonDynamicFormScript = commonDynamicFormScript.Replace("{0}", divDynamicFormContainer.ClientID);
        return commonDynamicFormScript;
    }


    /// <summary>
    /// 绑定外部数据源
    /// </summary>
    /// <returns></returns>
    protected string BindFormItems(Guid workflowId, Guid workflowInstanceId, Guid activityInstanceId, ActivityDefinition activityDefinition, Botwave.Security.LoginUser user)
    {
        string bindFormScript = string.Format(@"
        <script language=""javascript"" type=""text/javascript"">
        var FormDataSet = getFormDataSet('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
        getDataSet('{0}','{1}', FormDataSet, 1, '{9}');
        ItemIframe.LoadIframe('{0}', '{1}', '{7}', FormDataSet);
        ItemDataList.LoadDataList('{0}', '{1}', FormDataSet, 1);
        </script>", workflowId.ToString(), workflowInstanceId.ToString(), this.Title, this.SheetID, CurrentUserName, CurrentUser.DpId, activityInstanceId.ToString(), activityDefinition.ActivityName, activityDefinition.State, divDynamicFormContainer.ClientID);
        bindFormScript += string.Format(@"
        <script>
          {0}
        </script>", getOuterDataHandler.GenerAutoFull(activityDefinition, workflowInstanceId));
        return bindFormScript;
    }
    #endregion

    /// <summary>
    /// 指定两字符串忽略大小写的比较是否一致.
    /// </summary>
    /// <param name="leftValue"></param>
    /// <param name="rightValue"></param>
    /// <returns></returns>
    public static bool StringCompare(string leftValue, string rightValue)
    {
        return leftValue.Equals(rightValue, StringComparison.OrdinalIgnoreCase);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        Botwave.XQP.ImportExport.WorkflowExporter.ExportZip(this.Context, this.WorkflowInstanceId, this.SheetID, SheetID + "_" + Titles);
        // Botwave.XQP.ImportExport.WorkflowExporter.ExportWord(Response, this.WorkflowInstanceId, SheetID + "_" + Titles);
    }
}
