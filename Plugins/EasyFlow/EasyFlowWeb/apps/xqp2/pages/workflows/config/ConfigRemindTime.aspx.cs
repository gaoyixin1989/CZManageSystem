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

public partial class apps_xqp2_pages_workflows_config_ConfigRemindTime : Botwave.Web.PageBase
{
    private IWorkflowDefinitionService workflowDefinitionService;

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public Guid EntityId
    {
        get { return (ViewState["EntityId"] == null ? Guid.Empty : (Guid)ViewState["EntityId"]); }
        set { ViewState["EntityId"] = value; }
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
            string eid = Request.QueryString["eid"];
            if (!string.IsNullOrEmpty(eid))
            {
                this.EntityId = new Guid(eid);
                this.BindTimeSpans(EntityId);
            }
            else
            {
                ShowError("实体不存在。");
            }
        }
    }

    private void BindTimeSpans(Guid entityId)
    {
        this.gvRemindTimespans.DataSource = CZReminderTimeSpan.Select(entityId);
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
    protected static bool VerifyTimeSpan(IList<CZReminderTimeSpan> timespans, int editIndex, CZReminderTimeSpan inputTime, out string message)
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
            CZReminderTimeSpan item = timespans[i];
            TimeSpan beginTime = new TimeSpan(item.BeginHours, item.BeginMinutes, 0);
            TimeSpan endTime = new TimeSpan(item.EndHours, item.EndMinutes, 0);

            // 即相当于：beginTime -> 0:00:00 -> endTime
            if (inputBeginTime >= beginTime && inputBeginTime < endTime)
            {
                message = "起始执行时间已经存在于其它时间段，请重新选择。";
                return false;
            }
            if (inputEndTime > beginTime && inputEndTime <= endTime)
            {
                message = "结束执行时间已经存在于其它时间段，请重新选择。";
                return false;
            }
        }

        return true;
    }

    #region 编辑、删除


    protected void gvRemindTimespans_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex <= -1) return;

        if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
        {
            CZReminderTimeSpan dataItem = e.Row.DataItem as CZReminderTimeSpan;

            Label lblRemindType = (Label)e.Row.FindControl("lblRemindType");
            Label lblRemindTypeName = (Label)e.Row.FindControl("lblRemindTypeName");
            switch (lblRemindType.Text.Trim())
            {
                case "0":
                    lblRemindTypeName.Text = "待办+待阅";
                    break;
                case "1":
                    lblRemindTypeName.Text = "待办";
                    break;
                case "2":
                    lblRemindTypeName.Text = "待阅";
                    break;
            }
        }

        if ((e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            DropDownList ddlRemindType = (DropDownList)e.Row.FindControl("ddlRemindType");
            foreach (ListItem item in ddlRemindType.Items)
            {
                if (item.Selected)
                    item.Selected = false;
            }
            ddlRemindType.SelectedValue = ((Label)e.Row.FindControl("lblRemindType")).Text.Trim();
        }
    }

    protected void gvRemindTimespans_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int timeId = (int)this.gvRemindTimespans.DataKeys[e.RowIndex]["TimeId"];
        CZReminderTimeSpan.Delete(timeId);
        this.BindTimeSpans(EntityId);
    }

    protected void gvRemindTimespans_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.gvRemindTimespans.EditIndex = e.NewEditIndex;
        this.BindTimeSpans(EntityId);
    }

    protected void gvRemindTimespans_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int timeId = (int)this.gvRemindTimespans.DataKeys[e.RowIndex]["TimeId"];
        GridViewRow row = this.gvRemindTimespans.Rows[e.RowIndex];

        apps_xqp2_pages_workflows_controls_TimeSpanSelector selectorBegin = row.FindControl("selectorBeign") as apps_xqp2_pages_workflows_controls_TimeSpanSelector;
        apps_xqp2_pages_workflows_controls_TimeSpanSelector selectorEnd = row.FindControl("selectorEnd") as apps_xqp2_pages_workflows_controls_TimeSpanSelector;
        DropDownList ddlRemindType = (DropDownList)gvRemindTimespans.Rows[e.RowIndex].FindControl("ddlRemindType");
        CZReminderTimeSpan reminderTime = new CZReminderTimeSpan();
        reminderTime.TimeId = timeId;
        reminderTime.BeginHours = selectorBegin.GetHours();
        reminderTime.BeginMinutes = selectorBegin.GetMinutes();
        reminderTime.EndHours = selectorEnd.Hours;
        reminderTime.EndMinutes = selectorEnd.Minutes;
        reminderTime.RemindType = Convert.ToInt32(ddlRemindType.SelectedValue);
        IList<CZReminderTimeSpan> sources = CZReminderTimeSpan.Select(EntityId);
        string message;
        if (!VerifyTimeSpan(sources, e.RowIndex, reminderTime, out message))
        {
            ShowError(message);
        }

        if (reminderTime.Update() == 0)
            ShowError("时间段更新错误！");

        this.gvRemindTimespans.EditIndex = -1;
        this.BindTimeSpans(EntityId);
    }

    protected void gvRemindTimespans_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.gvRemindTimespans.EditIndex = -1;
        this.BindTimeSpans(EntityId);
    }

    #endregion

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        CZReminderTimeSpan time = new CZReminderTimeSpan();
        time.EntityId = EntityId;
        time.BeginHours = inertSelectorBegin.Hours;
        time.BeginMinutes = inertSelectorBegin.Minutes;
        time.EndHours = inserSelectorEnd.Hours;
        time.EndMinutes = inserSelectorEnd.Minutes;
        time.RemindType = Convert.ToInt32(ddlRemindType.SelectedValue);

        IList<CZReminderTimeSpan> sources = CZReminderTimeSpan.Select(EntityId);
        string message;
        if (!VerifyTimeSpan(sources, -1, time, out message))
        {
            ShowError(message);
        }
        time.Insert();
        BindTimeSpans(EntityId);
        //ShowSuccess("新增定时段成功！");
    }
}
