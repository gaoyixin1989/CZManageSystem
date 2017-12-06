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
using System.Linq;

public partial class apps_xqp2_pages_workflows_config_ConfigWorkflowRelation : Botwave.Security.Web.PageBase
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
            LoadActivitySetting("00");
        }
    }


    private void LoadActivitySetting(string extArgs)
    {
        if (extArgs == "00")
            radActivityA.Checked = true;
        else if (extArgs == "01")
            radActivityB.Checked = true;
        else if (extArgs == "02")
            radActivityC.Checked = true;
        IList<CZWorkflowRelationSetting> items = CZWorkflowRelationSetting.Select(WorkflowId);
        gvRemind.DataSource = items;
        gvRemind.DataBind();
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
        DropDownList ddlRelationWorkflowName = (DropDownList)gvRemind.Rows[e.RowIndex].FindControl("ddlRelationWorkflowName");
        DropDownList ddlSettingType = (DropDownList)gvRemind.Rows[e.RowIndex].FindControl("ddlSettingType");
        DropDownList ddlTriggerType = (DropDownList)gvRemind.Rows[e.RowIndex].FindControl("ddlTriggerType");
        DropDownList ddlOperateType = (DropDownList)gvRemind.Rows[e.RowIndex].FindControl("ddlOperateType");

        string id = ((Label)gvRemind.Rows[e.RowIndex].FindControl("lblId")).Text.ToString();
        string activityid = ((Label)gvRemind.Rows[e.RowIndex].FindControl("lblActivityId")).Text.ToString();
        string workflowId = ((Label)gvRemind.Rows[e.RowIndex].FindControl("lblWorkflowId")).Text.ToString();
        if (string.IsNullOrEmpty(ddlRelationWorkflowName.SelectedValue))
        {
            //Response.Write("<script>alert('“期望完成时间”和“允许滞留时间”只能选其一！');</script>");
            ScriptManager.RegisterClientScriptBlock(gvRemind, gvRemind.GetType(), gvRemind.UniqueID, "<script language=\"javascript\" type=\"text/javascript\"> alert('“请选择子流程！'); </script>", false);
            return;
        }
        CZWorkflowRelationSetting setting = CZWorkflowRelationSetting.SelectById(DbUtils.ToInt32(id));
        if (setting == null)
        {
            setting = new CZWorkflowRelationSetting();
            setting.Id = 0;
        
        }
        setting.WorkflowId = new Guid(workflowId);
        setting.ActivityId = new Guid(activityid);
        setting.RelationWorkflowName = ddlRelationWorkflowName.SelectedValue;
        setting.SettingType = DbUtils.ToInt32(ddlSettingType.SelectedValue);
        setting.TriggerType = DbUtils.ToInt32(ddlTriggerType.SelectedValue);
        setting.OperateType = DbUtils.ToInt32(ddlOperateType.SelectedValue);
        setting.Status = true;
        setting.LastModifier = setting.Creator = CurrentUserName;
        setting.CreatedTime = setting.LastModTime = DateTime.Now;
        setting.Update();

        gvRemind.EditIndex = -1;
        LoadActivitySetting("00");
    }
    protected void gvRemind_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = ((Label)gvRemind.Rows[e.RowIndex].FindControl("lblId")).Text.ToString();
        CZWorkflowRelationSetting.Delete(DbUtils.ToInt32(id));
        gvRemind.EditIndex = -1;
        LoadActivitySetting("00");
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
            CZWorkflowRelationSetting dataItem = e.Row.DataItem as CZWorkflowRelationSetting;
            Label lblSettingType = (Label)e.Row.FindControl("lblSettingType");
            Label lblSettingTypeName = (Label)e.Row.FindControl("lblSettingTypeName");
            LinkButton lbtnDel=(LinkButton)e.Row.FindControl("lbtnDel");
            switch (lblSettingType.Text.Trim())
            {
                case "0":
                    lblSettingTypeName.Text = "未设置";
                    break;
                case "1":
                    lblSettingTypeName.Text = "单发起";
                    break;
                case "2":
                    lblSettingTypeName.Text = "多发起";
                    break;
            }

            Label lblTriggerType = (Label)e.Row.FindControl("lblTriggerType");
            Label lblTriggerTypeName = (Label)e.Row.FindControl("lblTriggerTypeName");
            switch (lblTriggerType.Text.Trim())
            {
                case "0":
                    lblTriggerTypeName.Text = "未设置";
                    break;
                case "1":
                    lblTriggerTypeName.Text = "手动发起";
                    break;
                case "2":
                    lblTriggerTypeName.Text = "超时自动发起";
                    break;
                case "3":
                    lblTriggerTypeName.Text = "手动+超时自动发起";
                    break;
            }

            Label lblOperateType = (Label)e.Row.FindControl("lblOperateType");
            Label lblOperateTypeName = (Label)e.Row.FindControl("lblOperateTypeName");
            switch (lblOperateType.Text.Trim())
            {
                case "0":
                    lblOperateTypeName.Text = "未设置";
                    break;
                case "1":
                    lblOperateTypeName.Text = "等待子流程结束";
                    break;
                case "2":
                    lblOperateTypeName.Text = "根据子流程字段内容";
                    break;
            }
            if (dataItem.Status)
                lbtnDel.Visible = true;
            else
                lbtnDel.Visible = false;
        }

        if ((e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            CZWorkflowRelationSetting dataItem = e.Row.DataItem as CZWorkflowRelationSetting;
            DropDownList ddlRelationWorkflowName = (DropDownList)e.Row.FindControl("ddlRelationWorkflowName");
            LinkButton lbtnDel = (LinkButton)e.Row.FindControl("lbtnDel");
            ddlRelationWorkflowName.DataSource = workflowDefinitionService.GetWorkflowDefinitionList().Where(f=>f.WorkflowId!=dataItem.WorkflowId);
            ddlRelationWorkflowName.DataValueField = "WorkflowName";
            ddlRelationWorkflowName.DataTextField = "WorkflowName";
            ddlRelationWorkflowName.DataBind();
            ddlRelationWorkflowName.Items.Insert(0,new ListItem("- 选择子流程 -",""));
            ddlRelationWorkflowName.SelectedValue = ((Label)e.Row.FindControl("lblRelationWorkflowNameValue")).Text.Trim();

            DropDownList ddlSettingType = (DropDownList)e.Row.FindControl("ddlSettingType");
            foreach (ListItem item in ddlSettingType.Items)
            {
                if (item.Selected)
                    item.Selected = false;
            }
            ddlSettingType.SelectedValue = ((Label)e.Row.FindControl("lblSettingType")).Text.Trim();

            DropDownList ddlTriggerType = (DropDownList)e.Row.FindControl("ddlTriggerType");
            foreach (ListItem item in ddlTriggerType.Items)
            {
                if (item.Selected)
                    item.Selected = false;
            }
            ddlTriggerType.SelectedValue = ((Label)e.Row.FindControl("lblTriggerType")).Text.Trim();

            DropDownList ddlOperateType = (DropDownList)e.Row.FindControl("ddlOperateType");
            foreach (ListItem item in ddlOperateType.Items)
            {
                if (item.Selected)
                    item.Selected = false;
            }
            ddlOperateType.SelectedValue = ((Label)e.Row.FindControl("lblOperateType")).Text.Trim();
            //if (dataItem.Status)
            //    lbtnDel.Visible = true;
            //else
            //    lbtnDel.Visible = false;
            lbtnDel.Visible = false;
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

