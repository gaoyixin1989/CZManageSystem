using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;
using System.Text;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;

public partial class apps_xqp2_pages_workflows_config_ConfigReview : Botwave.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_xqp2_pages_workflows_config_ConfigReview));

    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

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

    protected IList<ActivityDefinition> activityDefinition;
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
        this.chkboxIsRevew.Checked = wprofile.IsReview;
        this.chkboxIsClassicType.Checked = wprofile.IsClassicReviewType;
        this.txtReviewMessage.Text = wprofile.ReviewNotifyMessage;
        //this.txtReviewActorCount.Text = wprofile.ReviewActorCount.ToString();
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
        activityDefinition = activityDefinitionService.GetActivitiesByWorkflowId(workflowId);
        this.rptActivities.DataSource = profiles;
        this.rptActivities.DataBind();
    }

    public WorkflowProfile GetWorkflowProfile()
    {
        WorkflowProfile profile = new WorkflowProfile();
        profile.WorkflowName = this.WorkflowName;
        profile.IsReview = this.chkboxIsRevew.Checked;
        profile.IsClassicReviewType = this.chkboxIsClassicType.Checked;
        profile.ReviewNotifyMessage = this.txtReviewMessage.Text;

        //string reviewActorCount = this.txtReviewActorCount.Text.Trim();
        //int actorCount = -1;
        //if (string.IsNullOrEmpty(reviewActorCount) || !int.TryParse(reviewActorCount, out actorCount))
        //    actorCount = -1;
        //profile.ReviewActorCount = (actorCount < -1 ? -1 : actorCount);
        profile.ReviewActorCount = WorkflowReviewActorCount;

        return profile;
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


    /// <summary>
    /// 获取下行的组织控制.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    protected string GetSuperiorHtml(IList<string> args,int index)
    {
        StringBuilder builder = new StringBuilder();
        bool isChecked = false;
        builder.AppendLine("<table border=\"0\" cellspacing=\"0\" style=\"margin:0; padding:0\">");
        builder.AppendLine("<tr><td>");

        isChecked = args.Contains("1");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg1\" name=\"chkOrgArgs{1}\" value=\"1\"{0} /><label for=\"chkOrgArg1\">所有上级主管</label>", GetCheckedAttribute(isChecked), index);

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("2");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg2\" name=\"chkOrgArgs{1}\" value=\"2\"{0} /><label for=\"chkOrgArg2\">同部门上级主管</label>", GetCheckedAttribute(isChecked), index);

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("3");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg3\" name=\"chkOrgArgs{1}\" value=\"3\"{0} /><label for=\"chkOrgArg3\">直接主管</label>", GetCheckedAttribute(isChecked), index);

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("4");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg4\" name=\"chkOrgArgs{1}\" value=\"4\"{0} /><label for=\"chkOrgArg4\">同部门其他人员</label>", GetCheckedAttribute(isChecked), index);

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("5");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg5\" name=\"chkOrgArgs{1}\" value=\"5\"{0} /><label for=\"chkOrgArg5\">同科室其他人员</label>", GetCheckedAttribute(isChecked), index);

        builder.AppendLine("</td></tr>\r\n<tr><td>");


        isChecked = args.Contains("6");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg6\" name=\"chkOrgArgs{1}\" value=\"6\"{0} /><label for=\"chkOrgArg6\">室审核</label>", GetCheckedAttribute(isChecked), index);

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("7");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg7\" name=\"chkOrgArgs{1}\" value=\"7\"{0} /><label for=\"chkOrgArg7\">部门正经理</label>", GetCheckedAttribute(isChecked), index);

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("11");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg11\" name=\"chkOrgArgs{1}\" value=\"11\"{0} /><label for=\"chkOrgArg11\">部门副经理</label>", GetCheckedAttribute(isChecked),index);

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("8");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg8\" name=\"chkOrgArgs{1}\" value=\"8\"{0} /><label for=\"chkOrgArg8\">公司领导审核</label>", GetCheckedAttribute(isChecked), index);

        builder.AppendLine("</td><td>");
        isChecked = args.Contains("9");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg9\" name=\"chkOrgArgs{1}\" value=\"9\"{0} /><label for=\"chkOrgArg9\">店面经理</label>", GetCheckedAttribute(isChecked), index);

        builder.AppendLine("</td><td>");
        isChecked = args.Contains("10");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg10\" name=\"chkOrgArgs{1}\" value=\"10\"{0} /><label for=\"chkOrgArg10\">综合员</label>", GetCheckedAttribute(isChecked),index);

        builder.AppendLine("</td>");

        builder.AppendLine("</table>");

        return builder.ToString();
    }
    /// <summary>
    /// 获取指定扩展分派类型名称列表.
    /// </summary>
    /// <param name="allocators"></param>
    /// <returns></returns>
    public static IList<string> GetExtendAllocators(string allocators)
    {
        if (string.IsNullOrEmpty(allocators))
            return new List<string>();

        string[] allocatorArray = allocators.Split(',', '，');
        if (allocatorArray == null || allocatorArray.Length == 0)
            return new List<string>();

        IList<string> results = new List<string>();
        foreach (string item in allocatorArray)
        {
            string allocator = item.Trim().ToLower();
            if (!results.Contains(allocator))
                results.Add(allocator);
        }
        return results;
    }
    /// <summary>
    /// 获取指定扩展分派类型的参数字典.
    /// </summary>
    /// <param name="allocatorArgs"></param>
    /// <returns></returns>
    public static IDictionary<string, IList<string>> GetExtendAllocatorArgs(string allocatorArgs)
    {
        if (string.IsNullOrEmpty(allocatorArgs))
            return new Dictionary<string, IList<string>>();

        string[] argExpressions = allocatorArgs.Replace(" ", "").Split(';', '；');
        if (argExpressions == null || argExpressions.Length == 0)
            return new Dictionary<string, IList<string>>();

        IDictionary<string, IList<string>> arguments = new Dictionary<string, IList<string>>();
        foreach (string expression in argExpressions)
        {
            string[] allocatorArray = expression.Split(':', '：');
            int arrayLength = allocatorArray.Length;
            if (arrayLength == 1)
            {
                string allocatorName = allocatorArray[0].ToLower().Trim();
                if (!arguments.ContainsKey(allocatorName))
                {
                    arguments.Add(allocatorName, new List<string>());
                }
            }
            else if (arrayLength == 2)
            {
                string allocatorName = allocatorArray[0].ToLower().Trim();
                string args = allocatorArray[1].Trim();
                string[] argArray = args.Split(',', '，');
                IList<string> argList = new List<string>();
                if (argArray != null)
                {
                    foreach (string item in argArray)
                    {
                        string arg = item.Trim();
                        argList.Add(arg);
                    }
                }
                arguments[allocatorName] = argList;
            }
        }
        return arguments;
    }
    public IList<ActivityProfile> GetActivitiyProfiles()
    {
        string workflowName = this.WorkflowName;
        IList<ActivityProfile> profiles = new List<ActivityProfile>();

        foreach (RepeaterItem rptitem in this.rptActivities.Items)
        {
            HiddenField hiddenProfileID = rptitem.FindControl("hiddenProfileID") as HiddenField;
            HiddenField hiddenActivityName = rptitem.FindControl("hiddenActivityName") as HiddenField;
            TextBox txtReviewActors = rptitem.FindControl("txtReviewActors") as TextBox;
            TextBox txtReviewActorCount = rptitem.FindControl("txtReviewActorCount") as TextBox;
            CheckBox chkboxActivityIsReview = rptitem.FindControl("chkboxActivityIsReview") as CheckBox;
            CheckBox chkboxReviewValidateType = rptitem.FindControl("chkboxReviewValidateType") as CheckBox;
            CheckBox chkActivity = rptitem.FindControl("chkActivity") as CheckBox;
            DropDownList ddlActivity = rptitem.FindControl("ddlActivity") as DropDownList;

            if (hiddenProfileID == null || hiddenActivityName == null || txtReviewActors == null || 
                txtReviewActorCount ==null || chkboxActivityIsReview == null || chkboxReviewValidateType == null)
                continue;
            int profileID = -1;
            if (!int.TryParse(hiddenProfileID.Value, out profileID))
                profileID = -1;

            int reviewActorCount = 0;
            if (string.IsNullOrEmpty(txtReviewActorCount.Text.Trim()) || !int.TryParse(txtReviewActorCount.Text.Trim(), out reviewActorCount))
                reviewActorCount = 0;
            reviewActorCount = (reviewActorCount < -1 ? -1 : reviewActorCount);

            string activityName = hiddenActivityName.Value;
            string reviewActors = txtReviewActors.Text.Trim();
            bool isReview = chkboxActivityIsReview.Checked;
            bool reviewValidateType = chkboxReviewValidateType.Checked;

            ActivityProfile profile =  new ActivityProfile();
            if (chkActivity.Checked)
            {
                
                profile = GetActivityAllocatorValues(profile, ddlActivity.SelectedValue, rptitem.ItemIndex);
            }

            profiles.Add(new ActivityProfile(profileID, workflowName, activityName, isReview, reviewActors, reviewActorCount, reviewValidateType, true,profile.ExtendAllocators,profile.ExtendAllocatorArgs));
        }
        return profiles;
    }

    /// <summary>
    /// 获取流程步骤下行分派值.
    /// </summary>
    /// <param name="activityId"></param>
    /// <param name="activityName"></param>
    /// <returns></returns>
    public ActivityProfile GetActivityAllocatorValues(ActivityProfile activity, string activityName, int index)
    {
        // starter,processor,superior,field
        string superiorArgs = Request.Form["chkOrgArgs"+index];

        string extendAllocators = string.Empty;
        string extendAllocatorArgs = string.Empty;
        if (!string.IsNullOrEmpty(superiorArgs))
        {
            extendAllocators += ",superior";
            extendAllocatorArgs += ";superior:" + superiorArgs;
        }
        
        if (!string.IsNullOrEmpty(activityName))
        {
            extendAllocators += ",activity";
                extendAllocatorArgs += ";activity:" + activityName;
        }
        if (extendAllocators.Length > 1)
            extendAllocators = extendAllocators.Remove(0, 1);
        if (extendAllocatorArgs.Length > 1)
            extendAllocatorArgs = extendAllocatorArgs.Remove(0, 1);

        activity.ExtendAllocators = extendAllocators;
        activity.ExtendAllocatorArgs = extendAllocatorArgs;

        return activity;
    }

    protected void rptActivityList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        ActivityProfile dataItem = e.Item.DataItem as ActivityProfile;
        if (dataItem == null)
            return;

        TextBox txtReviewActors = e.Item.FindControl("txtReviewActors") as TextBox;
        TextBox txtReviewActorCount = e.Item.FindControl("txtReviewActorCount") as TextBox;
        CheckBox chkboxActivityIsReview = e.Item.FindControl("chkboxActivityIsReview") as CheckBox;
        CheckBox chkboxReviewValidateType = e.Item.FindControl("chkboxReviewValidateType") as CheckBox;
        HtmlAnchor linkPickActors = e.Item.FindControl("linkPickActors") as HtmlAnchor;
        DropDownList ddlActivity = e.Item.FindControl("ddlActivity") as DropDownList;
        Literal ltlOrg = e.Item.FindControl("ltlOrg") as Literal;
        if (txtReviewActors == null || txtReviewActorCount == null || linkPickActors == null)
            return;
        ddlActivity.DataSource = activityDefinition;
        ddlActivity.DataTextField = "ActivityName";
        ddlActivity.DataValueField = "ActivityName";
        ddlActivity.DataBind();
        ddlActivity.Items.RemoveAt(ddlActivity.Items.Count - 1);
        ddlActivity.Dispose();
        ddlActivity.Items.Insert(0, new ListItem("- 请选择 -", ""));

        txtReviewActors.Text = dataItem.ReviewActors;
        txtReviewActorCount.Text = dataItem.ReviewActorCount.ToString();
        chkboxActivityIsReview.Checked = dataItem.IsReview;
        chkboxReviewValidateType.Checked = dataItem.ReviewValidateType;
        CheckBox chkActivity = e.Item.FindControl("chkActivity") as CheckBox;
        HtmlTableRow trOrg = e.Item.FindControl("trOrg") as HtmlTableRow;
        HtmlTableRow trOrgAct = e.Item.FindControl("trOrgAct") as HtmlTableRow;
        linkPickActors.Attributes["onclick"] = string.Format("javascrpt:return openUserSelector('{0}');", txtReviewActors.ClientID);
        IDictionary<string, IList<string>> dictArgs = GetExtendAllocatorArgs(dataItem.ExtendAllocatorArgs);
        IList<string> allocatorList = GetExtendAllocators(dataItem.ExtendAllocators);
        if (allocatorList.Count > 0)
        {
            if (allocatorList.Contains("starter"))
            {
                //this.chkStarterAssign.Checked = true;
            }
            if (allocatorList.Contains("processor"))
            {
                //this.chkPssorAssign.Checked = true;
            }
            if (allocatorList.Contains("superior"))
            {
                //this.chkOrgAssign.Checked = true;
                //this.trOrgAssign.Style.Remove("display");
                chkActivity.Checked = true;
                trOrg.Style.Remove("display");
                trOrgAct.Style.Remove("display");
            }
            if (allocatorList.Contains("activity"))
            {
                //this.chkControlAssign.Checked = true;
                //this.orgAssignType.Style.Remove("display");
            }
            if (allocatorList.Contains("owner"))
            {
                //this.chkToOwnerAssign.Checked = true;
                //this.ownerAssignType.Style.Remove("display");
            }
        }
        IList<string> superiorArgs = dictArgs.ContainsKey("superior") ? dictArgs["superior"] : new List<string>();
        ltlOrg.Text = GetSuperiorHtml(superiorArgs,e.Item.ItemIndex);
        ddlActivity.SelectedValue = dictArgs.ContainsKey("activity") ? dictArgs["activity"][0] : "";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            WorkflowProfile wprofile = this.GetWorkflowProfile();
            wprofile.UpdateReview();

            IList<ActivityProfile> aprofiles = this.GetActivitiyProfiles();
            foreach (ActivityProfile item in aprofiles)
            {
                item.Insert();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw ex;
        }
        ShowSuccess("保存抄送设置成功。");
    }
}