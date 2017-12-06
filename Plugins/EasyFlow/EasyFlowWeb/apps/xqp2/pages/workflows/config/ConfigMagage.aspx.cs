using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;

public partial class apps_xqp2_pages_workflows_config_ConfigMagage : Botwave.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_xqp2_pages_workflows_config_ConfigMagage));

    public string WorkflowName
    {
        get { return (string)ViewState["WorkflowName"]; }
        set { ViewState["WorkflowName"] = value; }
    }

    public int WorkflowReviewActorCount
    {
        get { return (int)ViewState["WorkflowReviewActorCount"]; }
        set { ViewState["WorkflowReviewActorCount"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wid = Request.QueryString["wid"];
            if (string.IsNullOrEmpty(wid))
                ShowError(Botwave.Web.MessageHelper.Message_ArgumentException, AppPath + "apps/xqp2/pages/workflows/workflowDeploy.aspx");
            Guid workflowId = new Guid(wid);

            this.LoadData(workflowId);
        }
    }

    private void LoadData(Guid workflowId)
    {
        WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
        this.WorkflowReviewActorCount = wprofile.ReviewActorCount;

        string workflowName = wprofile.WorkflowName;
        this.WorkflowName = workflowName;
        this.ltlWorkflowName.Text = workflowName;

        this.LoadActivityData(workflowId);
    }

    private void LoadActivityData(Guid workflowId)
    {
        IList<ActivityProfile> profiles = ActivityProfile.GetProfiles(workflowId);
        if (profiles == null || profiles.Count == 0)
        {
            this.divActivityReviewHolder.Visible = false;
            return;
        }
        this.rptActivities.DataSource = profiles;
        this.rptActivities.DataBind();
    }

    public IList<ActivityProfile> GetActivitiyProfiles()
    {
        string workflowName = this.WorkflowName;
        IList<ActivityProfile> profiles = new List<ActivityProfile>();

        foreach (RepeaterItem rptitem in this.rptActivities.Items)
        {
            HiddenField hiddenProfileID = rptitem.FindControl("hiddenProfileID") as HiddenField;
            HiddenField hiddenActivityName = rptitem.FindControl("hiddenActivityName") as HiddenField;
            TextBox txtReviewActorCount = rptitem.FindControl("txtReviewActorCount") as TextBox;
            CheckBox chkboxReviewValidateType = rptitem.FindControl("chkboxReviewValidateType") as CheckBox;

            if (hiddenProfileID == null || hiddenActivityName == null || 
                txtReviewActorCount == null  || chkboxReviewValidateType == null)
                continue;
            int profileID = -1;
            if (!int.TryParse(hiddenProfileID.Value, out profileID))
                profileID = -1;

            int reviewActorCount = 0;
            if (string.IsNullOrEmpty(txtReviewActorCount.Text.Trim()) || !int.TryParse(txtReviewActorCount.Text.Trim(), out reviewActorCount))
                reviewActorCount = 0;
            reviewActorCount = (reviewActorCount < -1 ? -1 : reviewActorCount);

            string activityName = hiddenActivityName.Value;
            bool reviewValidateType = chkboxReviewValidateType.Checked;

            profiles.Add(new ActivityProfile(profileID, workflowName, activityName, reviewActorCount, reviewValidateType));
        }
        return profiles;
    }

    protected void rptActivityList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        ActivityProfile dataItem = e.Item.DataItem as ActivityProfile;
        if (dataItem == null)
            return;

        TextBox txtReviewActorCount = e.Item.FindControl("txtReviewActorCount") as TextBox;
        CheckBox chkboxReviewValidateType = e.Item.FindControl("chkboxReviewValidateType") as CheckBox;

        if ( txtReviewActorCount == null )
            return;

        txtReviewActorCount.Text = dataItem.ManageActorCount.ToString();
        chkboxReviewValidateType.Checked = dataItem.ManageVisible;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            IList<ActivityProfile> aprofiles = this.GetActivitiyProfiles();
            foreach (ActivityProfile item in aprofiles)
            {
                item.ManageInsert();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw ex;
        }
        ShowSuccess("保存处理人设置成功。");
    }
}
