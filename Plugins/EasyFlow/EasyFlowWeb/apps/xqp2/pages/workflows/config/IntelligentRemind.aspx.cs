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

public partial class apps_xqp2_pages_workflows_config_IntelligentRemind : Botwave.Security.Web.PageBase
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
        }
    }
    private void LoadData()
    {
        WorkflowDefinition wfDefinition = workflowDefinitionService.GetWorkflowDefinition(WorkflowId);
        WorkflowName = wfDefinition.WorkflowName;
        gvRemind.DataSource = IntelligentRemind.SelectByWorkflowId(WorkflowName);
        gvRemind.DataBind();
    }
    protected void gvRemind_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvRemind.EditIndex = e.NewEditIndex;
        LoadData();
    }
    protected void gvRemind_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox txtStayHours = (TextBox)gvRemind.Rows[e.RowIndex].FindControl("txtStayHours");
        TextBox txtRemindTimes = (TextBox)gvRemind.Rows[e.RowIndex].FindControl("txtRemindTimes");
        DropDownList ddlRemindType = (DropDownList)gvRemind.Rows[e.RowIndex].FindControl("ddlRemindType");
        DropDownList ddlUrgency = (DropDownList)gvRemind.Rows[e.RowIndex].FindControl("ddlUrgency");
        DropDownList ddlImportance = (DropDownList)gvRemind.Rows[e.RowIndex].FindControl("ddlImportance");

        string ActivityName = ((Label)gvRemind.Rows[e.RowIndex].FindControl("lblActivityName")).Text.ToString();

        IntelligentRemind remind = new IntelligentRemind();
        remind.WorkflowName = WorkflowName;
        remind.ActivityName = ActivityName;
        remind.ExtArgs = ddlUrgency.SelectedValue + ddlImportance.SelectedValue;
        remind.StayHours = DbUtils.ToInt32(txtStayHours.Text.Trim());
        remind.RemindTimes = DbUtils.ToInt32(txtRemindTimes.Text.Trim());
        remind.RemindType = ddlRemindType.SelectedValue;
        remind.Creator = CurrentUser.UserName;
        remind.Id = DbUtils.ToInt32(gvRemind.DataKeys[e.RowIndex].Value);

        remind.Create();

        gvRemind.EditIndex = -1;
        LoadData();
    }
    protected void gvRemind_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvRemind.EditIndex = -1;
        LoadData();
    }
    protected void gvRemind_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex <= -1) return;

        if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
        {
            IntelligentRemind dataItem = e.Row.DataItem as IntelligentRemind;
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

            Label lblUrgency = (Label)e.Row.FindControl("lblUrgency");
            Label lblImportance = (Label)e.Row.FindControl("lblImportance");
            string extArgs = dataItem.ExtArgs;
            lblUrgency.Text = extArgs.Substring(0, 1).Equals("0") ? "否" : "是";
            lblImportance.Text = extArgs.Substring(1, 1).Equals("0") ? "否" : "是";

            HyperLink hlinkActivity = (HyperLink)e.Row.FindControl("hlinkActivity");
            hlinkActivity.NavigateUrl = string.Format("ActivityRemind.aspx?w={0}&a={1}", HttpUtility.UrlEncode(dataItem.WorkflowName), HttpUtility.UrlEncode(dataItem.ActivityName));
        }
        if (e.Row.RowState == DataControlRowState.Edit)
        {
            DropDownList ddlRemindType = (DropDownList)e.Row.FindControl("ddlRemindType");
            ddlRemindType.SelectedValue = ((Label)e.Row.FindControl("lblRemindType")).Text.Trim();

            DropDownList ddlUrgency = (DropDownList)e.Row.FindControl("ddlUrgency");
            ddlUrgency.SelectedValue = ((Label)e.Row.FindControl("lblExtArgs")).Text.Substring(0, 1);

            DropDownList ddlImportance = (DropDownList)e.Row.FindControl("ddlImportance");
            ddlImportance.SelectedValue = ((Label)e.Row.FindControl("lblExtArgs")).Text.Substring(1, 1);
        }
    }
}
