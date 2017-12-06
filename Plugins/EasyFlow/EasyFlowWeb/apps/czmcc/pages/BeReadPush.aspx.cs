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
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;
using Botwave.GMCCServiceHelpers;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Practices.CZMCC.Service.Impl;

public partial class apps_czmcc_pages_BeReadPush : Botwave.Security.Web.PageBase
{
    /// <summary>
    /// 显示消息时返回的页面.
    /// </summary>
    private static readonly string WorkflowRootUrl = AppPath + "contrib/workflow/pages/";

    private IActivityService activityService = (IActivityService)Ctx.GetObject("activityService");
    private IWorkflowService workflowService = (IWorkflowService)Ctx.GetObject("workflowService");
    private IWorkflowSettingService workflowSettingService = (IWorkflowSettingService)Ctx.GetObject("workflowSettingService");
    private IWorkflowUserService workflowUserService = (IWorkflowUserService)Ctx.GetObject("workflowUserService");

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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string aiid = Request.QueryString["aiid"];
            if (string.IsNullOrEmpty(aiid))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }
            Guid activityInstanceId = new Guid(aiid);

            Guid workflowId = new Guid(GetWorkFlowId(activityInstanceId));

            this.LoadWorkflowProfile(workflowId);
        }
    }

    private string GetWorkFlowId(Guid activityInstanceId)
    {
        WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);
        this.lbWrokflowTitle.Text = workflowInstance.Title;

        return workflowInstance.WorkflowId.ToString();
    }

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

        this.txtBeReadNotifyFormat.Text = wprofile.EmailNotifyFormat.Replace("处理", "查阅");

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        Guid activityInstanceId = new Guid(Request.QueryString["aiid"]);
        WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);


        if (workflowInstance.State == WorkflowConstants.Complete)
        {
            ShowSuccess("工单已处理完毕.", WorkflowRootUrl + string.Format("workflowview.aspx?wiid={0}", workflowInstance.WorkflowInstanceId));
        }

        string users = txtUsersAssign.Text;
        string[] userName = users.Split(',');

        IList<ActivityInstance> nextActivities = activityService.GetNextActivities(activityInstanceId);

        czWorkflowNotifyService wn = new czWorkflowNotifyService();
        
        if (chkSms.Checked)    //短信
        {
            foreach (ActivityInstance activity in nextActivities)
            {
                wn.SendCreatorMessage(workflowInstance, activity, activityInstanceId, userName, 2);
                break;
            }
            
        }
        if (chkEmail.Checked)  //邮件
        {
            foreach (ActivityInstance activity in nextActivities)
            {
                wn.SendCreatorMessage(workflowInstance, activity, activityInstanceId, userName, 1);
                break;
            }
        }
        if (chkBeRead.Checked) //待阅
        {
            string url = AppPath + "contrib/workflow/pages/WorkflowView.aspx?wiid=" + activityInstanceId;
            string entityId = activityInstanceId.ToString();

            for (int i = 0; i < userName.Length; i++)
            {
                AsynExtendedPendingJobHelper.AddPendingMsg(userName[i].ToString(), CurrentUserName, workflowInstance.Title, url, ActivityInstance.EntityType, entityId);
            }
        }

        ShowSuccess("已成功抄送", AppPath + "contrib/workflow/pages/default.aspx");
    }
}
