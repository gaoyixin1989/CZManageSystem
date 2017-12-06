using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;

public partial class apps_xqp2_pages_workflows_config_SMSConfig : Botwave.Web.PageBase
{
    public int ConfigID
    {
        get { return (int)ViewState["ConfigID"]; }
        set { ViewState["ConfigID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LoadConfig();
        }
    }

    private void LoadConfig()
    {
        string sms = Request.QueryString["sms"];
        if (string.IsNullOrEmpty(sms))
            ShowError("参数错误！");
        SMSProfile profile = SMSProfile.GetProfile();
        this.ConfigID = profile.ID;
        this.txtRejectMessage.Text = profile.ActivityRejectMessage;
        this.txtAssignmentMessage.Text = profile.AssignmentMessage;
        this.txtFeedbackSuccessMessage.Text = profile.FeedbackSuccessMessage;
        this.txtFeedbackErrorMessge.Text = profile.FeedbackErrorMessage;
        this.txtReceiveInvalidMessage.Text = profile.ReceiveInvalidMessage;
        this.txtLastReceiveInvalidMessage.Text = profile.LastReceiveInvalidMessage;
        this.txtGratuityFeedbackMessage.Text = profile.GratuityReplyMessage;
    }

    public SMSProfile GetProfileData()
    {
        SMSProfile proflie = new SMSProfile();
        proflie.ID = this.ConfigID;
        proflie.ActivityRejectMessage = this.txtRejectMessage.Text;
        proflie.AssignmentMessage = this.txtAssignmentMessage.Text;
        proflie.FeedbackSuccessMessage = this.txtFeedbackSuccessMessage.Text;
        proflie.FeedbackErrorMessage = this.txtFeedbackErrorMessge.Text;
        proflie.ReceiveInvalidMessage = this.txtReceiveInvalidMessage.Text;
        proflie.LastReceiveInvalidMessage = this.txtLastReceiveInvalidMessage.Text;
        proflie.GratuityReplyMessage = this.txtGratuityFeedbackMessage.Text;

        return proflie;
    }

    protected void btnSaveSMSConfig_Click(object sender, EventArgs e)
    {
        SMSProfile profile = GetProfileData();
        profile.Update();

        ShowSuccess("保存设置成功.");
    }
}
