﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Web;
using Botwave.Security;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;
using Botwave.DynamicForm.Extension.Implements;
using System.Data.SqlClient;

public partial class contrib_mobile_pages_Start : Botwave.XQP.Web.Security.PageBase
{
    private static string Draft_ReturnUrl = "contrib/mobile/pages/draft.aspx";
    private string rootDeptId = "34920440002";
    private string expandDeptId = "3492044000201";
    private string rootDeptName = "广州移动通信公司";
    #region interfaces
    private IWorkflowService workflowService = (IWorkflowService)Ctx.GetObject("workflowService");
    private IWorkflowSettingService workflowSettingService = (IWorkflowSettingService)Ctx.GetObject("workflowSettingService");
    private IWorkflowEngine workflowEngine = (IWorkflowEngine)Ctx.GetObject("workflowEngine");
    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IWorkflowInstanceCreationController workflowInstanceCreationController = (IWorkflowInstanceCreationController)Ctx.GetObject("workflowInstanceCreationController");
    private IWorkflowFormService mobileFormService = (IWorkflowFormService)Ctx.GetObject("mobileFormService");
    private IWorkflowFormService workflowFormService = (IWorkflowFormService)Ctx.GetObject("workflowFormService");
    private IWorkflowAttachmentService workflowAttachmentService = (IWorkflowAttachmentService)Ctx.GetObject("workflowAttachmentService");
    private IWorkflowReviewService workflowReviewService = (IWorkflowReviewService)Ctx.GetObject("workflowReviewService");
    public IReviewPending reviewPending = (IReviewPending)Ctx.GetObject("reviewPending");
    private IGetOuterDataHandler getOuterDataHandler = (IGetOuterDataHandler)Ctx.GetObject("getOuterDataHandler");
    private IDataListInstanceService dataListInstanceService = (IDataListInstanceService)Ctx.GetObject("dataListInstanceService");
    private IWorkflowMobileService workflowMobileService = (IWorkflowMobileService)Ctx.GetObject("workflowMobileService");

    public IWorkflowService WorkflowService
    {
        set { workflowService = value; }
    }

    public IWorkflowSettingService WorkflowSettingService
    {
        set { workflowSettingService = value; }
    }

    public IWorkflowEngine WorkflowEngine
    {
        set { workflowEngine = value; }
    }

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public IWorkflowInstanceCreationController WorkflowInstanceCreationController
    {
        set { workflowInstanceCreationController = value; }
    }

    public IWorkflowFormService MobileFormService
    {
        set { mobileFormService = value; }
    }

    public IWorkflowFormService WorkflowFormService
    {
        set { workflowFormService = value; }
    }

    public IWorkflowAttachmentService WorkflowAttachmentService
    {
        get { return workflowAttachmentService; }
        set { workflowAttachmentService = value; }
    }

    public IWorkflowReviewService WorkflowReviewService
    {
        set { workflowReviewService = value; }
    }

    public IReviewPending ReviewPending
    {
        set { reviewPending = value; }
    }

    public IGetOuterDataHandler GetOuterDataHandler
    {
        set { getOuterDataHandler = value; }
    }

    public IDataListInstanceService DataListInstanceService
    {
        set { dataListInstanceService = value; }
    }

    public IWorkflowMobileService WorkflowMobileService
    {
        set { this.workflowMobileService = value; }
    }
    #endregion

    public Guid WorkflowId
    {
        get { return (Guid)(ViewState["WorkflowId"]); }
        set { ViewState["WorkflowId"] = value; }
    }

    public Guid? WorkflowInstanceId
    {
        get
        {
            if (ViewState["WorkflowInstanceId"] == null)
                return null;
            return (Guid)(ViewState["WorkflowInstanceId"]);
        }
        set
        {
            ViewState["WorkflowInstanceId"] = value;
        }
    }

    public Guid WorkflowAttachmentId
    {
        get
        {
            return (Guid)(ViewState["WorkflowAttachmentId"]);
        }
        set
        {
            ViewState["WorkflowAttachmentId"] = value;
        }
    }

    public string ReturnUrl
    {
        get { return (string)(ViewState["ReturnUrl"]); }
        set { ViewState["ReturnUrl"] = value; }
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
    /// 当前用户.
    /// </summary>
    public string CurrentUserName
    {
        get { return (ViewState["CurrentUserName"] == null ? "" : (string)ViewState["CurrentUserName"]); }
        set { ViewState["CurrentUserName"] = value; }
    }

    /// <summary>
    /// 当前用户部门ID.
    /// </summary>
    public string DpId
    {
        get { return (ViewState["DpId"] == null ? "" : (string)ViewState["DpId"]); }
        set { ViewState["DpId"] = value; }
    }

    public string WorkflowName
    {
        get { return (ViewState["WorkflowName"] == null ? "" : (string)ViewState["WorkflowName"]); }
        set { ViewState["WorkflowName"] = value; }
    }

    public string ActivityName
    {
        get { return (ViewState["ActivityName"] == null ? "" : (string)ViewState["ActivityName"]); }
        set { ViewState["ActivityName"] = value; }
    }

    /// <summary>
    /// 显示树的根部门编号.
    /// </summary>
    public string RootDeptId
    {
        set { rootDeptId = value; }
    }

    /// <summary>
    /// 展开显示的部门编号.
    /// </summary>
    public string ExpandDeptId
    {
        set { expandDeptId = value; }
    }

    /// <summary>
    /// 根结点名称.
    /// </summary>
    public string RootDeptName
    {
        set { rootDeptName = value; }
    }

    /// <summary>
    /// 用户部门编号.
    /// </summary>
    public string UserDpId
    {
        get { return ViewState["UserDpId"] == null ? string.Empty : (string)ViewState["UserDpId"]; }
        set { ViewState["UserDpId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wid = Request.QueryString["wid"];    // 流程定义编号
            string wiid = Request.QueryString["wiid"];  // 流程实例编号（草稿箱）.
            string workflowAlias = Request.QueryString["workflow"];

            if (string.IsNullOrEmpty(wiid) && string.IsNullOrEmpty(wid) && string.IsNullOrEmpty(workflowAlias))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }

            Guid workflowId = Guid.Empty;
            if (!string.IsNullOrEmpty(workflowAlias))
            {
                WorkflowDefinition definition = workflowSettingService.GetCurrentWorkflowDefinition(workflowAlias);
                if (definition != null)
                    workflowId = definition.WorkflowId;
            }
            else if (!string.IsNullOrEmpty(wid))
            {
                workflowId = new Guid(wid);
            }

            LoginUser user = CurrentUser;
            if (workflowId != Guid.Empty)
            {
                string returnUrl = AppPath + "Workflow/WorkflowIndex.aspx?wid=" + workflowId.ToString();

                if (!workflowInstanceCreationController.CanCreate(workflowId, user.UserName, user))
                {
                    ShowError("您没有提单权限，或者流程不存在/已停用/每月(周)提单数已经达最大值。如需使用，请联系流程管理员！", returnUrl);
                }

                if (!workflowMobileService.IsWorkflowMobile(workflowId))
                {
                    ShowError("流程不支持手机审批。如需使用，请联系流程管理员！");
                }
                this.WorkflowId = workflowId;
                this.WorkflowAttachmentId = Guid.NewGuid();

                this.myWorkItemInput.LoadEmptyData(workflowId);
                this.LoadDynamicForm(workflowId, Guid.Empty, user);
                this.LoadActivtySelector(null, workflowId, Guid.Empty, false);
            }
            else
            {
                if (string.IsNullOrEmpty(wiid))
                    ShowError("流程实例不存在。如需使用，请联系流程管理员！");

                // 流程实例(用于草稿)
                Guid workflowInstanceId = new Guid(wiid);
                WorkflowInstance instance = workflowService.GetWorkflowInstance(workflowInstanceId);
                if (null == instance)
                    ShowError(MessageHelper.Message_ArgumentException + ", 流程实例不存在。");
                workflowId = instance.WorkflowId;
                this.WorkflowInstanceId = workflowInstanceId;
                this.WorkflowId = workflowId;
                this.WorkflowAttachmentId = workflowInstanceId;

                this.myWorkItemInput.LoadData(instance);
                this.LoadHistoryList(workflowInstanceId);
                this.LoadDynamicForm(workflowId, workflowInstanceId, user);
                this.LoadActivtySelector(instance, workflowId, workflowInstanceId, true);
            }

            WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
            bool isReview = (wprofile != null && wprofile.IsReview == true);
            this.IsReview = isReview;

            this.BindScripts(wprofile);
            if (this.myWorkItemInput.IsEmptyTitle())
            {
                this.myWorkItemInput.BindTitle(WorkflowProfileHelper.FormatWorkflowInstanceTitle(wprofile, user));
            }
            //this.relationHistory1.Bind(this.WorkflowInstanceId, this.WorkflowAttachmentId);

            //this.reviewSelector1.Initialize(wprofile, workflowId, null);

            if (null != Request.UrlReferrer)
                ReturnUrl = Request.UrlReferrer.PathAndQuery;

            this.CurrentUserName = CurrentUser.UserName;
            this.WorkflowName = wprofile.WorkflowName;
            this.DpId = CurrentUser.DpId;
        }
    }

    private void LoadHistoryList(Guid workflowInstanceId)
    {
        divWorkflowHistory.Visible = true;
        historyList.Initialize(workflowInstanceId);
        if (!historyList.Visible)
        {
            divWorkflowHistory.Visible = false;
        }
    }

    private void LoadActivtySelector(WorkflowInstance workflowInstance, Guid workflowId, Guid workflowInstanceId, bool isDraft)
    {
        ActivityDefinition initialActivity = activityDefinitionService.GetInitailActivityDefinition(workflowId);

        //如果允许手动选择
        if (ActivityDefinition.IsManualDecision(initialActivity.DecisionType))
        {
            if (isDraft)
            {
                this.myActivtySelector.LoadStartDataByInstance(workflowInstance, initialActivity, workflowInstanceId, workflowId);
            }
            else
            {
                this.myActivtySelector.LoadStartData(initialActivity, workflowId);
            }
        }
        else
        {
            this.myActivtySelector.Visible = false;
        }
    }

    //载入动态表单

    private void LoadDynamicForm(Guid workflowId, Guid workflowInstanceId, LoginUser user)
    {
        user = (user == null ? new LoginUser() : user);
        ActivityDefinition activityDefinition = activityDefinitionService.GetInitailActivityDefinition(workflowId);

        this.ActivityName = activityDefinition.ActivityName;

        IDictionary<string, object> dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        dict.Add("Activity", activityDefinition);
        dict.Add("ActivityName", activityDefinition.ActivityName);
        dict.Add("WorkflowInstanceId", workflowInstanceId);
        dict.Add("ButtonCreate", btnCreate.ClientID);
        dict.Add("ApproveButton", this.btnCreate.ClientID);

        dict.Add("CurrentUser", user); //添加当前用户为流程变量.
        dict.Add("CurrentPage", this.Page); //添加当前用户为流程变量.

        string formTemplate = mobileFormService.LoadForm(workflowId, workflowInstanceId, dict);
        if (!string.IsNullOrEmpty(formTemplate))
        {
            divDynamicFormContainer.InnerHtml = formTemplate;
        }
    }

    // 发起
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        CreateWorkflow();
    }

    // 保存
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (myWorkItemInput.IsValid())
        {
            SaveWorkflow();
        }
    }

    private void CreateWorkflow()
    {
        Botwave.Security.LoginUser user = CurrentUser;
        string actor = user.UserName;
        Guid activityInstanceId = Guid.Empty;
        Guid workflowId = this.WorkflowId;
        WorkflowInstance instance;

        IDictionary<string, object> formVariables = workflowFormService.GetFormVariables(this.Request);

        bool isUpdate = false;
        if (!WorkflowInstanceId.HasValue)
        {
            Guid _workflowInstanceId = Guid.NewGuid();
            instance = GetWorkflowInstance(_workflowInstanceId, actor);

            int urgency = instance.Urgency;
            if (urgency > 0)
            {
                if (!workflowInstanceCreationController.CanCreate(workflowId, actor, urgency, user))
                {
                    ShowError("发起工单失败，您未完成工单已达到最大数或者每月(周)紧急重要工单的提单数已经达最大，如需使用，请联系管理员。");
                }
            }
        }
        else
        {
            // 发起工单数据.
            isUpdate = true;
            instance = workflowService.GetWorkflowInstance(WorkflowInstanceId.Value);
            WorkflowInstance inputInstance = myWorkItemInput.GetInput();
            int urgency = inputInstance.Urgency;
            instance.Title = Server.HtmlEncode(inputInstance.Title).Replace("\"","”").Replace("'","‘");
            instance.ExpectFinishedTime = inputInstance.ExpectFinishedTime;
            instance.Secrecy = inputInstance.Secrecy;
            instance.Urgency = urgency;
            instance.Importance = inputInstance.Importance;
            instance.State = (int)WorkflowConstants.Executing;

            if (urgency > 0)
            {
                if (!workflowInstanceCreationController.CanCreate(workflowId, actor, urgency, user))
                {
                    ShowError("发起工单失败，您未完成工单已达到最大数或者每月(周)紧急重要工单的提单数已经达最大，如需使用，请联系管理员。");
                }
            }
        }

        // 流程初始步骤执行上下文

        ActivityExecutionContext executionContext = this.GetInitActivityExecutionContext(instance, user, formVariables);

        //activityInstanceId = this.NormalExecute(instance, formVariables, executionContext, actor, isUpdate);
        activityInstanceId = this.TransactionExecute(instance, formVariables, executionContext, actor, isUpdate);
        if (activityInstanceId == Guid.Empty)
            ShowError();


        //保存DataList表单内容
        dataListInstanceService.SaveDataListInstance(instance.WorkflowId, instance.WorkflowInstanceId, formVariables);

        if (!isUpdate)
        {
            Guid userId = (null != user ? user.UserId : Guid.Empty);
            Guid workflowInstanceId = instance.WorkflowInstanceId;
            workflowAttachmentService.UpdateWorkflowAttachmentEntities(this.WorkflowAttachmentId, workflowInstanceId);

           // this.relationHistory1.UpdateWorkflowInstanceId(workflowInstanceId, actor);
        }

        WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
        if (wprofile != null && wprofile.IsReview && reviewPending != null)
        {
            string reviewValue = this.myActivtySelector.GetReviewActorValue();
            ICollection<Guid> selectedActivities = (executionContext.ActivityAllocatees == null ? new List<Guid>() : executionContext.ActivityAllocatees.Keys);
            reviewPending.Pending(wprofile, new Botwave.Entities.BasicUser(user.UserName, user.RealName), instance.Title, workflowId, activityInstanceId, 
                this.myActivtySelector.Visible, selectedActivities, ReviewSelectorHelper.ParserReiewActors(reviewValue, selectedActivities));
        }
        //if (this.IsReview)
        //{
        //    WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
        //    wprofile = (wprofile == null ? WorkflowProfile.Default : wprofile);

        //    if (wprofile.IsClassicReviewType)
        //        this.reviewSelector1.PendingReview(wprofile, activityInstanceId, workflowId, instance.Title, new Botwave.Entities.BasicUser(user.UserName, user.RealName));
        //    else
        //        this.workflowReviewService.PendingReview(wprofile, activityInstanceId, workflowId, instance.Title, new Botwave.Entities.BasicUser(user.UserName, user.RealName));
        //}

        WriteNomalLog(actor, "发起工单", "发起工单成功");
        Response.Redirect(string.Format("Notify.aspx?aiid={0}&c={1}", activityInstanceId.ToString(), ActivityCommands.Approve));
    }

    private void SaveWorkflow()
    {
        IDictionary<string, object> formVariables = workflowFormService.GetFormVariables(this.Request);

        Botwave.Security.LoginUser user = CurrentUser;

        string actor = user.UserName;
        Guid? workflowInstanceId = this.WorkflowInstanceId;

        if (!workflowInstanceId.HasValue)
        {
            Guid newWorkflowInstanceId = Guid.NewGuid();
            WorkflowInstance instance = GetWorkflowInstance(newWorkflowInstanceId, actor);
            instance.State = (int)WorkflowConstants.Initial;

            workflowService.InsertWorkflowInstance(instance);
            CreateFormInstance(WorkflowId, newWorkflowInstanceId, actor, formVariables);

            Guid userId = (null != user ? user.UserId : Guid.Empty);
            workflowAttachmentService.UpdateWorkflowAttachmentEntities(this.WorkflowAttachmentId, newWorkflowInstanceId);

            //this.relationHistory1.UpdateWorkflowInstanceId(newWorkflowInstanceId, actor);
        }
        else
        {
            // 草稿箱状态

            myWorkItemInput.UpdateWorkflowInstance(workflowInstanceId.Value, true);
            //保存表单内容
            workflowFormService.SaveForm(workflowInstanceId.Value, actor, formVariables);

            //保存DataList表单内容
            dataListInstanceService.SaveDataListInstance(WorkflowId ,workflowInstanceId.Value, formVariables);

            //this.relationHistory1.UpdateWorkflowInstanceId(workflowInstanceId.Value, actor);
        }

        WriteNomalLog(actor, "保存工单", "保存工单成功");
        ShowSuccess("操作成功.", AppPath + Draft_ReturnUrl);
    }

    #region insert to database

    /// <summary>
    /// 事务执行.
    /// </summary>
    private Guid TransactionExecute(WorkflowInstance instance, IDictionary<string, object> formVariables, ActivityExecutionContext executionContext, string actor, bool isUpdate)
    {
        return WorkflowDataHelper.StartWorkflowByLock(instance, formVariables, executionContext, actor, isUpdate);
    }

    /// <summary>
    /// 普通执行.
    /// </summary>
    private Guid NormalExecute(WorkflowInstance instance, IDictionary<string, object> formVariables, ActivityExecutionContext executionContext, string actor, bool isUpdate)
    {
        return WorkflowDataHelper.StartWorkflow(instance, formVariables, executionContext, actor, isUpdate);
    }

    #endregion

    /// <summary>
    /// 获取初始化流程步骤执行上下文.
    /// </summary>
    /// <param name="_workflowInstance"></param>
    /// <param name="user"></param>
    /// <param name="formVariables"></param>
    /// <returns></returns>
    private ActivityExecutionContext GetInitActivityExecutionContext(WorkflowInstance _workflowInstance, Botwave.Security.LoginUser user, IDictionary<string, object> formVariables)
    {
        ActivityExecutionContext context = new ActivityExecutionContext();
        context.Actor = user.UserName;
        context.Command = ActivityCommands.Approve;
        context.Reason = "同意";

        try
        {
            context.ActivityAllocatees = this.myActivtySelector.GetActivityAllocatees();
        }
        catch (WorkflowAllocateException ex)
        {
            ShowError(ex.Message);
        }

        if (formVariables != null && formVariables.Count > 0)
            context.Variables = formVariables;
        context.Variables["Secrecy"] = _workflowInstance.Secrecy;
        context.Variables["Urgency"] = _workflowInstance.Urgency;
        context.Variables["Importance"] = _workflowInstance.Importance;

        context.Variables["CurrentUser"] = user;//添加当前用户为流程变量
        context.Variables["CurrentPage"] = this.Page; //添加当前用户为流程变量.

        return context;
    }

    private WorkflowInstance GetWorkflowInstance(Guid _workflowInstanceId, string actor)
    {
        WorkflowInstance instance = myWorkItemInput.GetInput();
        instance.WorkflowId = WorkflowId;
        instance.WorkflowInstanceId = _workflowInstanceId;
        instance.Creator = actor;

        return instance;
    }

    private void CreateFormInstance(Guid workflowId, Guid workflowInstanceId, string actor, IDictionary<string, object> formVariables)
    {
        // 创建表单实例
        workflowFormService.CreateFormInstance(workflowId, workflowInstanceId, actor);
        // 保存表单
        workflowFormService.SaveForm(workflowInstanceId, actor, formVariables);

        //保存DataList表单内容
        dataListInstanceService.SaveDataListInstance(WorkflowId, workflowInstanceId, formVariables);
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(ReturnUrl))
            Response.Redirect(ReturnUrl);
        Response.Redirect("default.aspx");        
    }

    protected void BindScripts(WorkflowProfile workflowProfile)
    {
        StringBuilder builder = new StringBuilder();
        //builder.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
        //builder.AppendLine(ReviewSelectorHelper.BuildValidateScript(workflowProfile, this.myActivtySelector.Visible));
        //builder.AppendLine("</script>");
        //ActivityDefinition activityDefinition = new ActivityDefinition();
        //activityDefinition.WorkflowId = this.WorkflowId;
        //activityDefinition.State = 0;
        //builder.Append("<script>" + getOuterDataHandler.GenerAutoFull(activityDefinition, this.WorkflowAttachmentId) + "</script>");
        //this.ltlScripts.Text = builder.ToString();
    }

    #region 从全公司选择处理人
    /// <summary>
    /// 绑定部门树结构.
    /// </summary>
    private void BindDepartmentTree()
    {
        DataView source = Botwave.XQP.Commons.XQPHelper.GetAllDepartments().DefaultView; //departmentService.GetAllDepartments().DefaultView;
        TreeNode node = new TreeNode(this.rootDeptName);
        node.Value = this.rootDeptId;
        this.treeDepts.Nodes.Add(node);
        this.RecursiveDepartmets(source, this.rootDeptId, node);  // 直接获取"广州移动通信公司"以下部门
        if (!string.IsNullOrEmpty(CurrentUser.DpId))
        {
            StringBuilder sb = new StringBuilder();
            SqlParameter[] pa = new SqlParameter[1];
            pa[0] = new SqlParameter("@dpid", SqlDbType.NVarChar, 50);
            pa[0].Value = CurrentUser.DpId;
            DataTable results = Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteDataset(CommandType.Text, "select userid,username,realname from bw_users where dpid like @dpid+'%'", pa).Tables[0];
            foreach (DataRow dw in results.Rows)
            {
                sb.Append("<div class=\"checkbox\"><label style='margin-bottom: 1em;'><input type=\"checkbox\" value=\"" + dw["username"] + "\" title=\"" + dw["realname"] + "\" userid=\"" + dw["userid"] + "\" name=\"chkCompanyUser\" /><span for='chkCompanyUser" + dw["userid"] + "' tooltip='" + dw["username"] + "'>" + dw["realname"] + "</span></label></div>");
            }
            ltlMen.Text = sb.ToString();
        }
    }

    private void RecursiveDepartmets(DataView sourceData, string parentId, TreeNode parentNode)
    {
        DataView data = new DataView(sourceData.Table, string.Format("ParentDpId = '{0}'", parentId), "", DataViewRowState.CurrentRows);
        TreeNode node;
        int count = data.Count;
        for (int i = 0; i < count; i++)
        {
            int level = Botwave.Commons.DbUtils.ToInt32(data[i]["DpLevel"]);
            if (level > 6)
                break;
            node = new TreeNode();
            if (level > 2)
                node.Expanded = false;

            string dpId = Botwave.Commons.DbUtils.ToString(data[i]["DpId"]).Trim();
            if (dpId == this.expandDeptId)
            {
                node.Expanded = true;
            }
            if (dpId == this.UserDpId)
            {
                parentNode.Expanded = true;
                node.Expanded = true;
                node.Checked = true;
            }
            node.Text = Botwave.Commons.DbUtils.ToString(data[i]["DpName"]);
            node.Value = dpId;
            parentNode.ChildNodes.Add(node);
            this.RecursiveDepartmets(sourceData, dpId, node);
        }
    }


    #region 部门数据树结构和用户列表

    protected void treeDepts_SelectedNodeChanged(object sender, EventArgs e)
    {
        string dpId = string.Empty;
        if (this.treeDepts.SelectedNode != null)
        {
            dpId = this.treeDepts.SelectedNode.Value;
            dpId = (this.treeDepts.SelectedNode.Parent == null ? string.Empty : dpId);
        }
        SearchUsers(dpId, 0, 0);
    }

    private void SearchUsers(string dpId, int recordCount, int pageIndex)
    {
        string keywords = this.txtPeople.Text;
        StringBuilder sb = new StringBuilder();
        //DataTable results = userService.GetUsersByPager(keywords, dpId, pageIndex, listPager.ItemsPerPage, ref recordCount);
        DataTable results = Botwave.Workflow.Practices.CZMCC.Support.UserService.Current.GetUsersByPager(keywords, dpId, 0, 1000, ref recordCount);
        foreach (DataRow dw in results.Rows)
        {
            sb.Append("<div class=\"checkbox\"><label style='margin-bottom: 1em;'><input type=\"checkbox\" value=\"" + dw["username"] + "\" title=\"" + dw["realname"] + "\" userid=\"" + dw["userid"] + "\" name=\"chkCompanyUser\" /><span for='chkCompanyUser" + dw["userid"] + "' tooltip='" + dw["username"] + "'>" + dw["realname"] + "</span></label></div>");
        }
        ltlMen.Text = sb.ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string dpId = string.Empty;
        if (this.treeDepts.SelectedNode != null)
        {
            dpId = this.treeDepts.SelectedNode.Value;
        }
        SearchUsers(dpId, 0, 0);
    }
    #endregion

    protected void btnChoose_Click(object sender, EventArgs e)
    {
        ltlMen.Text = string.Empty;
        BindDepartmentTree();
    }
    #endregion
}
