using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.XQP.Domain;

public partial class apps_xqp2_pages_workflows_config_RemindTime : Botwave.Web.PageBase
{
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public Guid WorkflowId
    {
        get { return (ViewState["WorkflowId"] == null ? Guid.Empty : (Guid)ViewState["WorkflowId"]); }
        set { ViewState["WorkflowId"] = value; }
    }

    public string WorkflowName
    {
        get { return (ViewState["WorkflowName"] == null ? string.Empty : (string)ViewState["WorkflowName"]); }
        set { ViewState["WorkflowName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wid = Request.QueryString["wid"];
            if (!string.IsNullOrEmpty(wid))
            {
                this.WorkflowId = new Guid(wid);
                WorkflowDefinition item = workflowDefinitionService.GetWorkflowDefinition(WorkflowId);
                if (item != null)
                    this.WorkflowName = item.WorkflowName;
                this.BindTimeSpans(WorkflowId);
            }
            else
            {
                ShowError("流程不存在。");
            }
        }
    }

    private void BindTimeSpans(Guid workflowId)
    {
        this.gvRemindTimespans.DataSource = ReminderTimeSpan.Select(workflowId);
        this.gvRemindTimespans.DataBind();
    }

    /// <summary>
    /// 检查时间段是否已经包含在其他时间里面. 新增时, editIndex = -1.
    /// </summary>
    /// <param name="timespans"></param>
    /// <param name="editIndex"></param>
    /// <param name="inputTime"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    protected static bool VerifyTimeSpan(IList<ReminderTimeSpan> timespans, int editIndex, ReminderTimeSpan inputTime, out string message)
    {
        message = string.Empty;
        if (timespans == null || timespans.Count == 0)
            return true;
        TimeSpan inputBeginTime = new TimeSpan(inputTime.BeginHours, inputTime.BeginMinutes, 0);
        TimeSpan inputEndTime = new TimeSpan(inputTime.EndHours, inputTime.EndMinutes, 0);
        if (inputBeginTime == inputEndTime)
        {
            message = "结束执行时间不能等于起始执行时间.";
            return false;
        }
        int count = timespans.Count;
        for (int i = 0; i < count; i++)
        {
            if (i == editIndex) // 编辑时，将自身时间段对比除外
                continue;
            ReminderTimeSpan item = timespans[i];
            TimeSpan beginTime = new TimeSpan(item.BeginHours, item.BeginMinutes, 0);
            TimeSpan endTime = new TimeSpan(item.EndHours, item.EndMinutes, 0);

            // 即相当于：beginTime -> 0:00:00 -> endTime
            if (inputBeginTime >= beginTime || inputBeginTime < endTime)
            {
                message = "起始执行时间已经存在于其它时间段，请重新选择。";
                return false;
            }
            if (inputEndTime > beginTime || inputEndTime <= endTime)
            {
                message = "结束执行时间已经存在于其它时间段，请重新选择。";
                return false;
            }
        }

        return true;
    }

    #region 编辑、删除




    protected void gvRemindTimespans_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int timeId = (int)this.gvRemindTimespans.DataKeys[e.RowIndex]["TimeId"];
        ReminderTimeSpan.Delete(timeId);
        this.BindTimeSpans(WorkflowId);
    }

    protected void gvRemindTimespans_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.gvRemindTimespans.EditIndex = e.NewEditIndex;
        this.BindTimeSpans(WorkflowId);
    }

    protected void gvRemindTimespans_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int timeId = (int)this.gvRemindTimespans.DataKeys[e.RowIndex]["TimeId"];
        GridViewRow row = this.gvRemindTimespans.Rows[e.RowIndex];

        apps_xqp2_pages_workflows_controls_TimeSpanSelector selectorBegin = row.FindControl("selectorBeign") as apps_xqp2_pages_workflows_controls_TimeSpanSelector;
        apps_xqp2_pages_workflows_controls_TimeSpanSelector selectorEnd = row.FindControl("selectorEnd") as apps_xqp2_pages_workflows_controls_TimeSpanSelector;

        ReminderTimeSpan reminderTime = new ReminderTimeSpan();
        reminderTime.TimeId = timeId;
        reminderTime.BeginHours = selectorBegin.GetHours();
        reminderTime.BeginMinutes = selectorBegin.GetMinutes();
        reminderTime.EndHours = selectorEnd.Hours;
        reminderTime.EndMinutes = selectorEnd.Minutes;

        IList<ReminderTimeSpan> sources = ReminderTimeSpan.Select(WorkflowId);
        string message;
        if (!VerifyTimeSpan(sources, e.RowIndex, reminderTime, out message))
        {
            ShowError(message);
        }

        if (reminderTime.Update() == 0)
            ShowError("时间段更新错误！");

        this.gvRemindTimespans.EditIndex = -1;
        this.BindTimeSpans(WorkflowId);
    }

    protected void gvRemindTimespans_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.gvRemindTimespans.EditIndex = -1;
        this.BindTimeSpans(WorkflowId);
    }

    #endregion

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        ReminderTimeSpan time = new ReminderTimeSpan();
        time.WorkflowName = WorkflowName;
        time.BeginHours = inertSelectorBegin.Hours;
        time.BeginMinutes = inertSelectorBegin.Minutes;
        time.EndHours = inserSelectorEnd.Hours;
        time.EndMinutes = inserSelectorEnd.Minutes;

        Guid wid = this.WorkflowId;
        IList<ReminderTimeSpan> sources = ReminderTimeSpan.Select(wid);
        string message;
        if (!VerifyTimeSpan(sources, -1, time, out message))
        {
            ShowError(message);
        }
        time.Insert();
        BindTimeSpans(wid);
        //ShowSuccess("新增定时段成功！");
    }
}
