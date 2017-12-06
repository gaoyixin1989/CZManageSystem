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

public partial class apps_xqp2_pages_workflows_config_ConfigWorkflowAuto : Botwave.Security.Web.PageBase
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
            string wid = Request.QueryString["wid"];
            if (string.IsNullOrEmpty(wid))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }
            Guid workflowId = new Guid(wid);
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
            this.chkboxIsAuto.Checked = wprofile.IsAutoContinue;
            this.LoadWorkflowActivities(workflowId, wprofile.AutoContinueActivities);
        }
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
    public WorkflowProfile GetWorkflowProfileAutoContinue()
    {
        WorkflowProfile item = new WorkflowProfile();
        item.WorkflowName = this.WorkflowName;
        item.IsAutoContinue = chkboxIsAuto.Checked;
        string activities = string.Empty;
        foreach (ListItem activityItem in this.chkboxActivities.Items)
        {
            if (activityItem.Selected)
            {
                activities += string.Format(",{0}", activityItem.Value);
            }
        }
        if (!string.IsNullOrEmpty(activities))
            activities = activities.Remove(0, 1);
        item.AutoContinueActivities = activities;
        return item;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        WorkflowProfile item = GetWorkflowProfileAutoContinue();
        item.UpdateAutoContinue();
        ShowSuccess("更新流程自动处理设置成功.");
    }
}
