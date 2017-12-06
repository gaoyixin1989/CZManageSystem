using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Web;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.XQP.Domain;
using System.Text;

public partial class apps_xqp2_pages_workflows_config_ConfigIntelligentRemind : Botwave.Security.Web.PageBase
{
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    #region properties

    public Guid WorkflowId
    {
        get { return (Guid)(ViewState["WorkflowId"]); }
        set { ViewState["WorkflowId"] = value; }
    }

    public string WorkflowName
    {
        get { return (string)(ViewState["WorkflowName"]); }
        set { ViewState["WorkflowName"] = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wfid = Request.QueryString["wid"];  // 流程实例编号（草稿箱）

            if (string.IsNullOrEmpty(wfid))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }
            WorkflowId = new Guid(wfid);
            LoadData();
            LoadWorkflowSetting("00");
            LoadActivitySetting("00");
        }
    }

    private void LoadData()
    {
        WorkflowDefinition wfDefinition = workflowDefinitionService.GetWorkflowDefinition(WorkflowId);
        if (wfDefinition == null)
        {
            ShowError("流程实例为空。");
        }
        WorkflowName = wfDefinition.WorkflowName;
        WorkflowProfile profile = WorkflowProfile.LoadByWorkflowId(WorkflowId);
        txtWorkOrderWarningNotifyformat.Text = profile.WorkOrderWarningNotifyformat;
        txtWorkOrderTimeoutNotifyformat.Text = profile.WorkOrderTimeoutNotifyformat;
        txtStepTimeoutNotifyformat.Text = profile.StepTimeoutNotifyformat;
        txtStepWarningNotifyformat.Text = profile.StepWarningNotifyformat;
    }

    private void LoadWorkflowSetting(string extArgs)
    {

        //ltlFlowName.Text = WorkflowName;
        SZIntelligentRemind sZIntelligentRemind = SZIntelligentRemind.SelectInstanceByWorkflowId(WorkflowName, extArgs);
        IList<SZIntelligentRemind> items = SZIntelligentRemind.SelectByWorkflowId(WorkflowName, extArgs);

        /*if (extArgs == "00")
            radWorkflowA.Checked = true;
        else if (extArgs == "01")
            radWorkflowB.Checked = true;
        else if (extArgs == "02")
            radWorkflowC.Checked = true;*/

        //foreach (ListItem item in ddlInstanceReminderType.Items)
        //{
        //    if (item.Selected)
        //        item.Selected = false;
        //}

        if (sZIntelligentRemind != null)
        {
            //hidId.Value = sZIntelligentRemind.Id.ToString().Trim();
            //txtInstanceStayHours.Value = sZIntelligentRemind.StayHours.ToString();
            //txtInstanceToHours.Value = sZIntelligentRemind.ToHours.ToString();
            //txtInstanceToInterval.Value = sZIntelligentRemind.ToInterval.ToString();
            //txtInstanceToTimes.Value = sZIntelligentRemind.ToTimes.ToString();
            //txtInstanceTimeoutInterval.Value = sZIntelligentRemind.TimeoutInterval.ToString();
            //txtInstanceTimeoutTimes.Value = sZIntelligentRemind.TimeoutTimes.ToString();
            //ddlInstanceReminderType.Items.FindByValue(sZIntelligentRemind.RemindType.ToString().Trim()).Selected = true;
            //drdlStartAct.SelectedValue = sZIntelligentRemind.StartActivityName;
            //drdlEndAct.SelectedValue = sZIntelligentRemind.EndActivityName;

            if (sZIntelligentRemind.StatisticsType == 0)
                radNormalday.Checked = true;
            if (sZIntelligentRemind.StatisticsType == 1)
                radUnnormalday.Checked = true;
            if (sZIntelligentRemind.StatisticsType == 2)
                radCusday.Checked = true;
        }
        else
        {
            //hidId.Value = "0";
            //txtInstanceStayHours.Value = "0";
            //txtInstanceToHours.Value = "0";
            //txtInstanceToInterval.Value = "0";
            //txtInstanceToTimes.Value = "0";
            //txtInstanceTimeoutInterval.Value = "0";
            //txtInstanceTimeoutTimes.Value = "0";
            //ddlInstanceReminderType.Items.FindByValue("0").Selected = true;
            foreach (SZIntelligentRemind item in items)
            {
                if (item.StatisticsType != -1)
                {
                    if (item.StatisticsType == 0)
                        radNormalday.Checked = true;
                    if (item.StatisticsType == 1)
                        radUnnormalday.Checked = true;
                    if (item.StatisticsType == 2)
                        radCusday.Checked = true;
                    break;
                }
            }
        }
        BindActivitise(items, sZIntelligentRemind);
    }

    private void LoadActivitySetting(string extArgs)
    {
        if (extArgs == "00")
            radActivityA.Checked = true;
        else if (extArgs == "01")
            radActivityB.Checked = true;
        else if (extArgs == "02")
            radActivityC.Checked = true;
        IList<SZIntelligentRemind> items = SZIntelligentRemind.SelectByWorkflowId(WorkflowName, extArgs);
        gvRemind.DataSource = items;
        gvRemind.DataBind();
    }

    private void saveNotifyFormat()
    {
        string[] profile = new string[5];
        profile[4] = WorkflowName;
        profile[3] = txtWorkOrderTimeoutNotifyformat.Text;
        profile[2] = txtWorkOrderWarningNotifyformat.Text;
        profile[1] = txtStepTimeoutNotifyformat.Text;
        profile[0] = txtStepWarningNotifyformat.Text;
        SZIntelligentRemind.NotifyFormatUpdate(profile);
    }

    private void BindActivitise(IList<SZIntelligentRemind> items, SZIntelligentRemind intelligentRemind)
    {
        int index = 0;
        int _i = 1;
        int _n = 0;
        StringBuilder builder = new StringBuilder();
        bool isChecked = false;
        builder.AppendLine("<table border=\"0\" cellspacing=\"0\" style=\"margin:0; padding:0\">");
        builder.AppendLine("<tr><td><input type=\"checkbox\" id=\"chkAll\" onclick=\"onToggleNotify('chkFlow', this.checked);\"/>全选</td><td></td><td></td><td></td></tr>");
        builder.AppendLine("<tr><td>");
        foreach (SZIntelligentRemind item in items)
        {
            if (intelligentRemind != null)
            {
                if (!string.IsNullOrEmpty(intelligentRemind.ExcludedSteps))
                {
                    IList<string> list = new List<string>();
                    foreach (string str in intelligentRemind.ExcludedSteps.Split(',', '，'))
                    {
                        list.Add(str);
                    }
                    if (list.Contains(item.ActivityName))
                        isChecked = true;
                    else
                        isChecked = false;
                }
            }
            builder.AppendFormat("<input type=\"checkbox\" id=\"chkFlow" + _i.ToString() + "\" name=\"chkFlows\" value=\"" + item.ActivityName + "\" {0} /><label for=\"chkFlow" + _i.ToString() + "\">" + item.ActivityName + "</label>", GetCheckedAttribute(isChecked));
            _n = _i % 5;
            if (_n == 0)
                builder.AppendLine("</td></tr><tr><td>");
            else
                builder.AppendLine("</td><td>");
            _i++;
        }
        builder.AppendLine("</td><td></td><td></td><td></td></tr>");
        //////////////////////
        builder.AppendLine("</table>");
        ltlActivityNames.Text = builder.ToString();
    }

    /// <summary>
    /// 获取 checked 属性 HTML.
    /// </summary>
    /// <param name="isChecked"></param>
    /// <returns></returns>
    protected static string GetCheckedAttribute(bool isChecked)
    {
        return (isChecked ? " checked=\"checked\"" : "");
    }

    protected void gvRemind_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvRemind.EditIndex = e.NewEditIndex;
        //LoadData();
        string extArgs = "00";
        if (radActivityB.Checked)
            extArgs = "01";
        else if (radActivityC.Checked)
            extArgs = "02";
        //LoadData();
        LoadActivitySetting(extArgs);
    }
    protected void gvRemind_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        TextBox txtStayHours = (TextBox)gvRemind.Rows[e.RowIndex].FindControl("txtStayHours");
        TextBox txtToHours = (TextBox)gvRemind.Rows[e.RowIndex].FindControl("txtToHours");
        TextBox txtToInterval = (TextBox)gvRemind.Rows[e.RowIndex].FindControl("txtToInterval");
        TextBox txtToTimes = (TextBox)gvRemind.Rows[e.RowIndex].FindControl("txtToTimes");
        TextBox txtTimeoutInterval = (TextBox)gvRemind.Rows[e.RowIndex].FindControl("txtTimeoutInterval");
        TextBox txtTimeoutTimes = (TextBox)gvRemind.Rows[e.RowIndex].FindControl("txtTimeoutTimes");
        DropDownList ddlRemindType = (DropDownList)gvRemind.Rows[e.RowIndex].FindControl("ddlRemindType");
        DropDownList ddlUrgency = (DropDownList)gvRemind.Rows[e.RowIndex].FindControl("ddlUrgency");
        DropDownList ddlImportance = (DropDownList)gvRemind.Rows[e.RowIndex].FindControl("ddlImportance");
        DropDownList ddlLimitedTime = (DropDownList)gvRemind.Rows[e.RowIndex].FindControl("ddlLimitedTime");

        string id = ((Label)gvRemind.Rows[e.RowIndex].FindControl("lblId")).Text.ToString();
        string ActivityName = ((Label)gvRemind.Rows[e.RowIndex].FindControl("lblActivityName")).Text.ToString();
        string extArgs = "00";
        if (radActivityB.Checked)
            extArgs = "01";
        else if (radActivityC.Checked)
            extArgs = "02";

        if (!radNormalday.Checked && !radUnnormalday.Checked && !radCusday.Checked)
        {
            //Response.Write("<script>alert('请选择时效考核标准！');</script>");
            ScriptManager.RegisterClientScriptBlock(gvRemind, gvRemind.GetType(), gvRemind.UniqueID, "<script language=\"javascript\" type=\"text/javascript\"> alert('请选择时效考核标准！'); </script>", false);
            return;
        }
        if (!string.IsNullOrEmpty(ddlLimitedTime.SelectedValue) && DbUtils.ToSingle(txtStayHours.Text.Trim()) > 0)
        {
            //Response.Write("<script>alert('“期望完成时间”和“允许滞留时间”只能选其一！');</script>");
            ScriptManager.RegisterClientScriptBlock(gvRemind, gvRemind.GetType(), gvRemind.UniqueID, "<script language=\"javascript\" type=\"text/javascript\"> alert('“期望完成时间”和“允许滞留时间”只能选其一！'); </script>", false);
            return;
        }
        SZIntelligentRemind remind = new SZIntelligentRemind();
        remind.WorkflowName = WorkflowName;
        remind.ActivityName = ActivityName;
        remind = SZIntelligentRemind.SelectById(int.Parse(id));
        if (remind == null)
        {
            remind = new SZIntelligentRemind();
            remind.WorkflowName = WorkflowName;
            remind.ActivityName = ActivityName;
        }
        //remind.ExtArgs = ddlUrgency.SelectedValue + ddlImportance.SelectedValue;
        remind.ExtArgs = extArgs;
        remind.SettingType = 1;
        remind.StayHours = decimal.Parse(string.IsNullOrEmpty(txtStayHours.Text.Trim()) ? "-1" : txtStayHours.Text.Trim());
        remind.ToHours = decimal.Parse(string.IsNullOrEmpty(txtToHours.Text.Trim()) ? "-1" : txtToHours.Text.Trim());
        remind.ToInterval = decimal.Parse(string.IsNullOrEmpty(txtToInterval.Text.Trim()) ? "-1" : txtToInterval.Text.Trim());
        remind.ToTimes = DbUtils.ToInt32(string.IsNullOrEmpty(txtToTimes.Text.Trim()) ? "-1" : txtToTimes.Text.Trim());
        remind.TimeoutInterval = decimal.Parse(string.IsNullOrEmpty(txtTimeoutInterval.Text.Trim()) ? "-1" : txtTimeoutInterval.Text.Trim());
        remind.TimeoutTimes = DbUtils.ToInt32(string.IsNullOrEmpty(txtTimeoutTimes.Text.Trim()) ? "-1" : txtTimeoutTimes.Text.Trim());
        remind.RemindType = Botwave.Commons.DbUtils.ToInt32(ddlRemindType.SelectedValue);
        remind.Creator = CurrentUser.UserName;
        remind.Id = DbUtils.ToInt32(gvRemind.DataKeys[e.RowIndex].Value);
        remind.ExpectFinishTime = !string.IsNullOrEmpty(ddlLimitedTime.SelectedValue) ? ddlLimitedTime.SelectedValue + "$" + ddlLimitedTime.SelectedItem.Text : ddlLimitedTime.SelectedValue;
        if (radNormalday.Checked)
            remind.StatisticsType = 0;
        else if (radUnnormalday.Checked)
            remind.StatisticsType = 1;
        else if (radCusday.Checked)
            remind.StatisticsType = 2;
        else
            remind.StatisticsType = -1;

        remind.Create();
        saveNotifyFormat();

        gvRemind.EditIndex = -1;
        LoadData();
        LoadActivitySetting(extArgs);
    }
    protected void gvRemind_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvRemind.EditIndex = -1;
        string extArgs = "00";
        if (radActivityB.Checked)
            extArgs = "01";
        else if (radActivityC.Checked)
            extArgs = "02";
        //LoadData();
        LoadActivitySetting(extArgs);
    }
    protected void gvRemind_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex <= -1) return;

        if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
        {
            SZIntelligentRemind dataItem = e.Row.DataItem as SZIntelligentRemind;

            Label lblRemindType = (Label)e.Row.FindControl("lblRemindType");
            Label lblRemindTypeName = (Label)e.Row.FindControl("lblRemindTypeName");
            switch (lblRemindType.Text.Trim())
            {
                case "0":
                    lblRemindTypeName.Text = "未设置";
                    break;
                case "1":
                    lblRemindTypeName.Text = "电子邮件";
                    break;
                case "2":
                    lblRemindTypeName.Text = "短信";
                    break;
                default:
                    lblRemindTypeName.Text = "短信 + 电子邮件";
                    break;
            }

            string extArgs = dataItem.ExtArgs;
            //HyperLink hlinkActivity = (HyperLink)e.Row.FindControl("hlinkActivity");
            //hlinkActivity.NavigateUrl = string.Format("ConfigIntelligentRemindNotice.aspx?wid={0}&w={1}&a={2}&t=1", WorkflowId, HttpUtility.UrlEncode(dataItem.WorkflowName), HttpUtility.UrlEncode(dataItem.ActivityName));
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

            DropDownList ddlLimitedTime = (DropDownList)e.Row.FindControl("ddlLimitedTime");
            foreach (ListItem item in ddlLimitedTime.Items)
            {
                if (item.Selected)
                    item.Selected = false;
            }
            ddlLimitedTime.SelectedValue = !string.IsNullOrEmpty(((Label)e.Row.FindControl("lblLimitedTime")).Text) ? ((Label)e.Row.FindControl("lblLimitedTime")).Text.Trim().Split('$')[0] : ((Label)e.Row.FindControl("lblLimitedTime")).Text;

            TextBox txtStayHours = (TextBox)e.Row.FindControl("txtStayHours");
            TextBox txtToHours = (TextBox)e.Row.FindControl("txtToHours");
            TextBox txtToInterval = (TextBox)e.Row.FindControl("txtToInterval");
            TextBox txtToTimes = (TextBox)e.Row.FindControl("txtToTimes");
            TextBox txtTimeoutInterval = (TextBox)e.Row.FindControl("txtTimeoutInterval");
            TextBox txtTimeoutTimes = (TextBox)e.Row.FindControl("txtTimeoutTimes");
            //DropDownList ddlRemindType = (DropDownList)e.Row.FindControl("ddlRemindType");
            //DropDownList ddlUrgency = (DropDownList)e.Row.FindControl("ddlUrgency");
            //ddlUrgency.SelectedValue = ((Label)e.Row.FindControl("lblExtArgs")).Text.Substring(0, 1);

            //DropDownList ddlImportance = (DropDownList)e.Row.FindControl("ddlImportance");
            //ddlImportance.SelectedValue = ((Label)e.Row.FindControl("lblExtArgs")).Text.Substring(1, 1);
        }
    }

    protected void radNormalday_CheckedChanged(object sender, EventArgs e)
    {
        if (radNormalday.Checked)
        {
            try
            {
                SZIntelligentRemind.StatisticsTypeUpdateByWorkflowName(WorkflowName, 0);
                ltlInfo.Text = "时效计算标准为自然日";
            }
            catch (Exception ex)
            {

                ltlInfo.Text = "时效计算标准保存错误" + ex.Message;
            }
        }
    }

    protected void radUnnormalday_CheckedChanged(object sender, EventArgs e)
    {
        if (radUnnormalday.Checked)
        {
            try
            {
                SZIntelligentRemind.StatisticsTypeUpdateByWorkflowName(WorkflowName, 1);
                ltlInfo.Text = "时效计算标准为非自然日";
            }
            catch (Exception ex)
            {

                ltlInfo.Text = "时效计算标准保存错误" + ex.Message;
            }
        }
    }

    protected void radCusday_CheckedChanged(object sender, EventArgs e)
    {
        if (radCusday.Checked)
        {
            try
            {
                SZIntelligentRemind.StatisticsTypeUpdateByWorkflowName(WorkflowName, 2);
                ltlInfo.Text = "时效计算标准为客服工作日";
            }
            catch (Exception ex)
            {

                ltlInfo.Text = "时效计算标准保存错误" + ex.Message;
            }
        }
    }

    protected void lbtnSave_Click(object sender, EventArgs e)
    {
        //string extArgs = "00";
        //if (radWorkflowB.Checked)
        //    extArgs = "01";
        //else if (radWorkflowC.Checked)
        //    extArgs = "02";
        //SZIntelligentRemind remind = SZIntelligentRemind.SelectInstanceByWorkflowId(WorkflowName, extArgs);
        //if (remind == null)
        //    remind = new SZIntelligentRemind();
        //remind.WorkflowName = WorkflowName;
        //remind.ActivityName = null;
        //remind.SettingType = 0;
        //remind.StayHours = DbUtils.ToSingle(txtInstanceStayHours.Value.Trim());
        //remind.ToHours = DbUtils.ToSingle(txtInstanceToHours.Value.Trim());
        //remind.ToInterval = DbUtils.ToSingle(txtInstanceToInterval.Value.Trim());
        //remind.ToTimes = DbUtils.ToInt32(txtInstanceToTimes.Value.Trim());
        //remind.TimeoutInterval = DbUtils.ToSingle(txtInstanceTimeoutInterval.Value.Trim());
        //remind.TimeoutTimes = DbUtils.ToInt32(txtInstanceTimeoutTimes.Value.Trim());
        //remind.RemindType = ddlInstanceReminderType.SelectedValue;
        //remind.Creator = CurrentUser.UserName;
        //remind.Id = DbUtils.ToInt32(hidId.Value);
        //remind.ExcludedSteps = Request.Form["chkFlows"];
        //remind.ExtArgs = extArgs;
        ////remind.StartActivityName = drdlStartAct.SelectedValue;
        ////remind.EndActivityName = drdlEndAct.SelectedValue;
        //if (radNormalday.Checked)
        //    remind.StatisticsType = 0;
        //else if (radUnnormalday.Checked)
        //    remind.StatisticsType = 1;
        //else if (radCusday.Checked)
        //    remind.StatisticsType = 2;
        //else
        //    remind.StatisticsType = -1;
        //remind.Create();
        //saveNotifyFormat();
        ////Response.Write("<script>alert('工单时效考核设置已保存！');</script>");
        //ScriptManager.RegisterClientScriptBlock(lbtnSave, lbtnSave.GetType(), lbtnSave.UniqueID, "<script language=\"javascript\" type=\"text/javascript\"> alert('“工单时效考核设置已保存！'); </script>", false);
        //LoadWorkflowSetting(extArgs);
    }

    protected void btnSaveSMSConfig_Click(object sender, EventArgs e)
    {
        saveNotifyFormat();
        Response.Write("<script>alert('效考核消息模版已保存！');</script>");
        LoadData();
    }

    protected void radWorkflowA_CheckedChanged(object sender, EventArgs e)
    {
        //if (radWorkflowA.Checked)
        //    LoadWorkflowSetting("00");
    }

    protected void radWorkflowB_CheckedChanged(object sender, EventArgs e)
    {
        //if (radWorkflowB.Checked)
        //    LoadWorkflowSetting("01");
    }

    protected void radWorkflowC_CheckedChanged(object sender, EventArgs e)
    {
        //if (radWorkflowC.Checked)
        //    LoadWorkflowSetting("02");
    }

    protected void radActivityA_CheckedChanged(object sender, EventArgs e)
    {
        if (radActivityA.Checked)
            LoadActivitySetting("00");
    }

    protected void radActivityB_CheckedChanged(object sender, EventArgs e)
    {
        if (radActivityB.Checked)
            LoadActivitySetting("01");
    }

    protected void radActivityC_CheckedChanged(object sender, EventArgs e)
    {
        if (radActivityC.Checked)
            LoadActivitySetting("02");
    }
}

