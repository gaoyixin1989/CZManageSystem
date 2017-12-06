using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave;
using Botwave.Web;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Domain;

public partial class apps_xqp2_pages_workflows_config_ConfigSMSAudit : Botwave.Security.Web.PageBase
{
    private IActivityDefinitionService activityDefinitionService = Spring.Context.Support.WebApplicationContext.Current["activityDefinitionService"] as IActivityDefinitionService;

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public Guid WorkflowId
    {
        get { return (Guid)ViewState["WorkflowId"]; }
        set { ViewState["WorkflowId"] = value; }
    }

    public string WorkflowName
    {
        get { return (string)ViewState["WorkflowName"]; }
        set { ViewState["WorkflowName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wfid = Request.QueryString["wid"];
            if (string.IsNullOrEmpty(wfid))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }
            Guid workflowId = new Guid(wfid);
            WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
            if (wprofile == null)
            {
                wprofile = WorkflowProfile.Default;
                this.WorkflowName = WorkflowUtility.GetWorkflowName(workflowId);
            }
            else
            {
                this.WorkflowName = wprofile.WorkflowName;
            }

            this.WorkflowId = workflowId;
            this.LoadWorkflowProfile(wprofile);
            this.LoadWorkflowActivities(workflowId, wprofile.SMSAuditActivities);
        }
    }

    public void LoadWorkflowProfile(WorkflowProfile wprofile)
    {
        if (wprofile == null)
            return;

        this.chkboxSMSAudit.Checked = wprofile.IsSMSAudit;
        this.txtSMSAuditNotifyFormat.Text = wprofile.SMSAuditNotifyFormat;
    }

    public void LoadWorkflowActivities(Guid workflowId, string selectedActivities)
    {
        // 绑定步骤列表.
        IList<ActivityDefinition> activities = activityDefinitionService.GetActivitiesByWorkflowId(workflowId);
        for (int i = 0; i < activities.Count; i++)
        {
            if (activities[i].State != 1)
            {
                activities.RemoveAt(i);
                i--;
            }
        }
        this.chkboxActivities.DataSource = activities;
        this.chkboxActivities.DataBind();

        // 绑定选中.
        if (string.IsNullOrEmpty(selectedActivities))
            return;

        string[] selectedArray = selectedActivities.Split(',', '，');
        IDictionary<string, string> dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (string item in selectedArray)
        {
            if (!dict.ContainsKey(item))
                dict.Add(item, item);
        }

        foreach (ListItem item in this.chkboxActivities.Items)
        {
            if (dict.ContainsKey(item.Text))
                item.Selected = true;
        }
    }

    /// <summary>
    /// 获取流程短信审批设置.
    /// </summary>
    /// <returns></returns>
    public WorkflowProfile GetWorkflowProfileSMSAudit()
    {
        WorkflowProfile item = new WorkflowProfile();
        item.WorkflowName = this.WorkflowName;
        item.IsSMSAudit = chkboxSMSAudit.Checked;
        item.SMSAuditNotifyFormat = txtSMSAuditNotifyFormat.Text.Trim();
        string smsActivities = string.Empty;
        foreach (ListItem activityItem in this.chkboxActivities.Items)
        {
            if (activityItem.Selected)
            {
                smsActivities += string.Format(",{0}", activityItem.Value);
            }
        }
        if (!string.IsNullOrEmpty(smsActivities))
            smsActivities = smsActivities.Remove(0, 1);
        item.SMSAuditActivities = smsActivities;
        return item;
    }

    protected void btnSaveSMSAudit_Click(object sender, EventArgs e)
    {
        WorkflowProfile smsProfile = GetWorkflowProfileSMSAudit();
        smsProfile.UpdateSMSAudit();
        ShowSuccess("更新流程短信审批设置成功.");
    }

    protected void chkboxActivities_DataBound(object sender, EventArgs e)
    {

    }
}
