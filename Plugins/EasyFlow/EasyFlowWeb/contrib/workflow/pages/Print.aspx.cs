using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Binders;
using Botwave.XQP.Domain;

public partial class contrib_workflow_pages_Print : Botwave.Security.Web.PageBase
{
    private IWorkflowService workflowService = Spring.Context.Support.WebApplicationContext.Current["workflowService"] as IWorkflowService;
    private IActivityDefinitionService activityDefinitionService = Spring.Context.Support.WebApplicationContext.Current["activityDefinitionService"] as IActivityDefinitionService;
    private IActivityService activityService = Spring.Context.Support.WebApplicationContext.Current["activityService"] as IActivityService;
    private IFormDefinitionService formDefinitionService = Spring.Context.Support.WebApplicationContext.Current["formDefinitionService"] as IFormDefinitionService;
    private IFormItemDataBinder formItemDataBinder = Spring.Context.Support.WebApplicationContext.Current["formItemDataBinder"] as IFormItemDataBinder;

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

    public IFormDefinitionService FormDefinitionService
    {
        set { formDefinitionService = value; }
    }

    public IFormItemDataBinder FormItemDataBinder
    {
        set { this.formItemDataBinder = value; }
    }

    public Guid WorkflowInstanceId
    {
        get { return ViewState["WorkflowInstanceId"] == null ? Guid.Empty : (Guid)ViewState["WorkflowInstanceId"]; }
        set { ViewState["WorkflowInstanceId"] = value; }
    }

    public Guid ActivityInstanceId
    {
        get { return ViewState["ActivityInstanceId"] == null ? Guid.Empty : (Guid)ViewState["ActivityInstanceId"]; }
        set { ViewState["ActivityInstanceId"] = value; }
    }

    public string ActivityName
    {
        get { return (ViewState["ActivityName"] == null ? "" : (string)ViewState["ActivityName"]); }
        set { ViewState["ActivityName"] = value; }
    }

    public string WorkflowName
    {
        get { return (ViewState["WorkflowName"] == null ? "" : (string)ViewState["WorkflowName"]); }
        set { ViewState["WorkflowName"] = value; }
    }

    public string CanPrint
    {
        get { return (ViewState["CanPrint"] == null ? "" : (string)ViewState["CanPrint"]); }
        set { ViewState["CanPrint"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string aiid = Request.QueryString["aiid"];
            string wiid = Request.QueryString["wiid"];
            string c=Request.QueryString["c"];
            if (string.IsNullOrEmpty(aiid) && string.IsNullOrEmpty(wiid))
            {
                ShowError(Botwave.GlobalSettings.Instance.ArgumentExceptionMessage);
            }
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
                activityInstanceId = activityInstance.ActivityInstanceId;
            }

            if (null == activityInstance)
            {
                ShowError(Botwave.GlobalSettings.Instance.ArgumentExceptionMessage);
            }

            CanPrint = c;
            WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);
            WorkflowProfile profile = WorkflowProfile.LoadByWorkflowId(workflowInstance.WorkflowId);
            this.Title = workflowInstance.Title;
            workflowInstanceId = workflowInstance.WorkflowInstanceId;
            Guid workflowId = workflowInstance.WorkflowId;

            //判断是否能打印
            if (workflowInstance.State >= 2)
            {
                CZWorkflowInstance czWorkflowInstance = CZWorkflowInstance.GetWorkflowInstance(workflowInstance.WorkflowInstanceId);
                

                if (czWorkflowInstance.PrintCount >= profile.PrintAmount && profile.PrintAmount > -1)
                {
                    //btnPrint.Visible = false;
                    CanPrint = "false";
                    Response.Write("<script>alert('工单只能打印" + profile.PrintAmount + "次，目前已打印" + czWorkflowInstance.PrintCount + "次！');window.close();</script>");
                }
                else if (profile.PrintAmount > -1)
                {
                    if (string.IsNullOrEmpty(CanPrint))
                        Response.Write("<script>alert('工单只能打印" + profile.PrintAmount + "次，目前已打印" + czWorkflowInstance.PrintCount + "次！');</script>");
                }
            }
            else
            {
                CZActivityInstance czActivityInstance = CZActivityInstance.GetWorkflowActivity(activityInstanceId);
                CZActivityDefinition czactivity = CZActivityDefinition.GetWorkflowActivityByActivityId(czActivityInstance.ActivityId);
                if (czactivity.ActivityName == activityInstance.ActivityName && czactivity.CanPrint == -1 && !czActivityInstance.IsCompleted && czactivity.PrintAmount > -1)
                {
                    if (string.IsNullOrEmpty(CanPrint))
                        Response.Write("<script>alert('工单只能打印" + czactivity.PrintAmount + "次，目前已打印" + czActivityInstance.PrintCount + "次！');</script>");
                }
                else if (czactivity.ActivityName == activityInstance.ActivityName && czactivity.CanPrint > -1 && czactivity.PrintAmount <= czActivityInstance.PrintCount && !czActivityInstance.IsCompleted && czactivity.PrintAmount > -1)
                {
                    //btnPrint.Visible = false;
                    CanPrint = "false";
                    Response.Write("<script>alert('工单只能打印" + czactivity.PrintAmount + "次，目前已打印" + czActivityInstance.PrintCount + "次！');window.close();</script>");
                }
                else if (czactivity.CanPrint > -1 && czactivity.PrintAmount > -1)
                {
                    if (string.IsNullOrEmpty(CanPrint))
                        Response.Write("<script>alert('工单只能打印" + czactivity.PrintAmount + "次，目前已打印" + czActivityInstance.PrintCount + "次！');</script>");
                }
            }

            //控件数据绑定
            ucWorkItemView.LoadData(workflowInstance, null, activityInstanceId);
            //ucCompletWorkFlowList.DataBind(workflowInstanceId);
            ucAttachment.LoadData(workflowInstance);

            this.ucCompletWorkFlowList.OnBind(workflowInstanceId);

            LoadDynamicForm(workflowId, workflowInstanceId, activityInstance, workflowInstance);

            this.WorkflowInstanceId = workflowInstanceId;
            this.ActivityInstanceId = activityInstanceId;
            this.ActivityName = activityInstance.ActivityName;
            this.WorkflowName = profile.WorkflowName;
        }
    }

    //载入动态表单.
    private void LoadDynamicForm(Guid workflowId, Guid workflowInstanceId, ActivityInstance activityInstance, WorkflowInstance workflowInstance)
    {
        FormDefinition definition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", workflowId);
        if (null != definition && !String.IsNullOrEmpty(definition.TemplateContent))
        {
            ActivityDefinition activityDefinition = activityDefinitionService.GetActivityDefinition(activityInstance.ActivityId);
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ActivityName", activityDefinition.ActivityName);
            dict.Add("WorkflowInstanceId", workflowInstanceId);
            dict.Add("CurrentUser", CurrentUser);

            System.IO.StringWriter sw = new System.IO.StringWriter();
            formItemDataBinder.Bind(sw, workflowInstanceId, Botwave.Commons.StringUtils.HtmlDecode(definition.TemplateContent), dict);
            divDynamicFormContainer.InnerHtml = sw.GetStringBuilder().ToString();
            this.divDynamicFormContainer.InnerHtml += BindFormItems(workflowId, workflowInstanceId, activityInstance.ActivityInstanceId, activityDefinition,workflowInstance, CurrentUser);
        }
    }

    /// <summary>
    /// 绑定外部数据源
    /// </summary>
    /// <returns></returns>
    protected string BindFormItems(Guid workflowId, Guid workflowInstanceId, Guid activityInstanceId, ActivityDefinition activityDefinition,WorkflowInstance workflowInstance, Botwave.Security.LoginUser user)
    {
        string bindFormScript = string.Format(@"
        <script language=""javascript"" type=""text/javascript"">
        var FormDataSet = getFormDataSet('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
        getDataSet('{0}','{1}', FormDataSet, 1, '{9}');
        ItemIframe.LoadIframe('{0}', '{1}', '{7}', FormDataSet);
        ItemDataList.LoadDataList('{0}', '{1}', FormDataSet, 1);
        </script>", workflowId.ToString(), workflowInstanceId.ToString(), workflowInstance.Title, workflowInstance.SheetId, CurrentUserName, CurrentUser.DpId, activityInstanceId.ToString(), activityDefinition.ActivityName, activityDefinition.State, divDynamicFormContainer.ClientID);
        return bindFormScript;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string wiid = Request.QueryString["wiid"];
        string aiid = Request.QueryString["aiid"];
        CZWorkflowInstance czWorkflowInstance = CZWorkflowInstance.GetWorkflowInstance(new Guid(wiid));
        if (czWorkflowInstance != null)
        {
            if (czWorkflowInstance.State >= 2)
            {
                WorkflowProfile profile = WorkflowProfile.LoadByWorkflowId(czWorkflowInstance.WorkflowId);

                if (profile.PrintAmount > 0 && (profile.PrintAndExp==0 || profile.PrintAndExp==2))
                {
                    CZWorkflowInstance.UpdateWorkflowInstance(czWorkflowInstance);
                }
            }
            else
            {
                CZActivityInstance activityInstance = CZActivityInstance.GetWorkflowActivity(new Guid(aiid));
                CZActivityDefinition activityDefinition = CZActivityDefinition.GetWorkflowActivityByActivityId(activityInstance.ActivityId);
                if (activityDefinition.CanPrint > -1 && activityDefinition.PrintAmount > 0)
                    CZActivityInstance.WorkflowActivitiesUpdate(activityInstance);
            }
        }
        //Response.Write("<script>window.close();</script>");
        Response.Redirect(string.Format("Print.aspx?wiid={0}&aiid={1}&c={2}",wiid,aiid,"true"));
    }
}
