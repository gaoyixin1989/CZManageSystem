using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;

public partial class apps_xqp2_pages_workflows_config_ActivityRemind : Botwave.Security.Web.PageBase
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_xqp2_pages_workflows_config_ActivityRemind));

    /// <summary>
    /// 流程名称.
    /// </summary>
    public string WorkflowName
    {
        get { return (string)ViewState["WorkflowName"]; }
        set { ViewState["WorkflowName"] = value; }
    }

    /// <summary>
    /// 流程步骤名称.
    /// </summary>
    public string ActivityName
    {
        get { return (string)ViewState["ActivityName"]; }
        set { ViewState["ActivityName"] = value; }
    }

    /// <summary>
    /// 当前用户.
    /// </summary>
    public string UserName
    {
        get { return (string)ViewState["UserName"]; }
        set { ViewState["UserName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string workflowName = Request.QueryString["w"];
            string activityName = Request.QueryString["a"];
            if (string.IsNullOrEmpty(workflowName) || string.IsNullOrEmpty(activityName))
            {
                ShowError("流程智能提醒控制参数错误.");
            }

            IList<IntelligentRemindControl> remindControls = IntelligentRemindControl.GetIntelligentRemindControls(workflowName, activityName);
            if (remindControls.Count > 0)
            {
                LoadRemindControls(remindControls);
                log.Warn("Remind:" + remindControls.Count);
            }

            this.WorkflowName = workflowName;
            this.ActivityName = activityName;
            this.UserName = CurrentUserName;
        }
    }

    private void LoadRemindControls(IList<IntelligentRemindControl> items)
    {
        foreach (IntelligentRemindControl item in items)
        {
            int urgency = item.Urgency;
            if (urgency == 0) // 一般
            {
                log.Warn("Remind-x:" + urgency);
                iri1.BindData(item);
            }
            else if (urgency == 1) // 紧急
            {
                log.Warn("Remind-x:" + urgency);
                iri2.BindData(item);
            }
            else if (urgency == 2) // 最紧急
            {
                log.Warn("Remind-x:" + urgency);
                iri3.BindData(item);
            }
        }
    }

    // 保存智能提醒设置.
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string workflowName = this.WorkflowName;
        string activityName = this.ActivityName;
        string creator = this.UserName;
        // 一般
        IntelligentRemindControl item = iri1.GetData(workflowName, activityName, 0, creator);
        item.Insert();

        // 紧急
        item = iri2.GetData(workflowName, activityName, 1, creator);
        item.Insert();

        // 最紧急
        item = iri3.GetData(workflowName, activityName, 2, creator);
        item.Insert();

        ShowSuccess("成功保存流程步骤智能提醒设置.");
    }
}
