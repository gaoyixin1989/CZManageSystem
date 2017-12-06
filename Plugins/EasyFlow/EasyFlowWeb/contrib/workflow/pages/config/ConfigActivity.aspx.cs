using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using Botwave.Web;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;

public partial class contrib_workflow_pages_config_ConfigActivity : Botwave.Security.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_workflow_pages_config_ConfigActivity));

    #region service interfaces

    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private ITaskAssignService taskAssignService = (ITaskAssignService)Ctx.GetObject("taskAssignService");
    private IWorkflowFieldService workflowFieldService = (IWorkflowFieldService)Ctx.GetObject("workflowFieldService");

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public ITaskAssignService TaskAssignService
    {
        set { taskAssignService = value; }
    }

    public IWorkflowFieldService WorkflowFieldService
    {
        set { workflowFieldService = value; }
    }
    #endregion

    #region properties

    public Guid WorkflowId
    {
        get { return (Guid)ViewState["WorkflowId"]; }
        set { ViewState["WorkflowId"] = value; }
    }

    public Guid ActivityId
    {
        get { return (Guid)ViewState["ActivityId"]; }
        set { ViewState["ActivityId"] = value; }
    }

    public string WorkflowName
    {
        get { return (string)ViewState["WorkflowName"]; }
        set { ViewState["WorkflowName"] = value; }
    }

    public string ActivityName
    {
        get { return (string)ViewState["ActivityName"]; }
        set { ViewState["ActivityName"] = value; }
    }

    public string ResourceId
    {
        get { return (string)ViewState["ResourceId"]; }
        set { ViewState["ResourceId"] = value; }
    }

    public string AssignResourceId
    {
        get { return (string)ViewState["AssignResourceId"]; }
        set { ViewState["AssignResourceId"] = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string aid = Request.QueryString["aid"];
            if (string.IsNullOrEmpty(aid))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }

            Guid activityId = new Guid(aid);
            ActivityDefinition activity = activityDefinitionService.GetActivityDefinition(activityId);
            AllocatorOption assignmentAllocator = taskAssignService.GetAssignmentAllocator(activityId);

            if (activity == null)
            {
                ShowError("未找到指定流程步骤.");
            }
            this.LoadActivity(activity);
            if (assignmentAllocator != null)
            {
                this.LoadAssingmentAllocator(assignmentAllocator);
            }
            Guid workflowId = activity.WorkflowId;
            string workflowName = WorkflowUtility.GetWorkflowName(workflowId);
            string activityName = activity.ActivityName;
            this.LoadFieldDropList(workflowId);

            this.ActivityId = activityId;
            this.WorkflowId = workflowId;
            this.WorkflowName = workflowName;
            this.ActivityName = activityName;
            this.ResourceId = activity.AllocatorResource;
            this.AssignResourceId = assignmentAllocator.AllocatorResource;
        }
    }

    #region load

    protected void LoadActivity(ActivityDefinition activity)
    {
        this.ltlActivityName.Text = activity.ActivityName;
        this.txtCommandRules.Text = activity.CommandRules;
        string users = activity.AllocatorUsers;
        string resource = activity.AllocatorResource;
        string extendAllocators = activity.ExtendAllocators;
        string extendAllocatorArgs = activity.ExtendAllocatorArgs;
        string defaultAllocator = activity.DefaultAllocator;

        if (!string.IsNullOrEmpty(defaultAllocator) && defaultAllocator.IndexOf(':') > -1)
        {
            defaultAllocator = defaultAllocator.Remove(defaultAllocator.IndexOf(':'));
        }
        this.selDefaultTypes.Value = defaultAllocator;

        if (!string.IsNullOrEmpty(users))
        {
            this.chkUsers.Checked = true;
            this.txtUsers.Text = users;
            this.trUsers.Style.Remove("display");
        }
        this.chkRes.Checked = EnableResouceController(resource);

        IList<string> allocatorList = GetExtendAllocators(extendAllocators);
        if (allocatorList.Count > 0)
        {
            if (allocatorList.Contains("starter"))
            {
                this.chkStarter.Checked = true;
            }
            if (allocatorList.Contains("processor"))
            {
                this.chkPssor.Checked = true;
            }
            if (allocatorList.Contains("field"))
            {
                this.chkField.Checked = true;
                this.trFields.Style.Remove("display");
            }
            if (allocatorList.Contains("superior"))
            {
                this.chkOrg.Checked = true;
                this.trOrg.Style.Remove("display");
            }
        }

        IDictionary<string, IList<string>> dictArgs = GetExtendAllocatorArgs(extendAllocatorArgs);

        IList<string> superiorArgs = dictArgs.ContainsKey("superior") ? dictArgs["superior"] : new List<string>();
        this.ltlOrg.Text = GetSuperiorHtml(superiorArgs);
    }

    protected void LoadAssingmentAllocator(AllocatorOption option)
    {
        this.selAssignDefaultTypes.Value = option.DefaultAllocator;
        string users = option.AllocatorUsers;
        string resource = option.AllocatorResource;
        string extendAllocators = option.ExtendAllocators;
        string extendAllocatorArgs = option.ExtendAllocatorArgs;

        if (!string.IsNullOrEmpty(users))
        {
            this.txtUsersAssign.Text = users;
            this.chkUsersAssign.Checked = true;
            this.trUsersAssign.Style.Remove("display");
        }

        this.chkResAssign.Checked = EnableResouceController(resource);

        IList<string> allocatorList = GetExtendAllocators(extendAllocators);
        if (allocatorList.Count > 0)
        {
            if (allocatorList.Contains("starter"))
            {
                this.chkStarterAssign.Checked = true;
            }
            if (allocatorList.Contains("processor"))
            {
                this.chkPssorAssign.Checked = true;
            }
            if (allocatorList.Contains("superior"))
            {
                this.chkOrgAssign.Checked = true;
                this.trOrgAssign.Style.Remove("display");
            }
        }

        IDictionary<string, IList<string>> dictArgs = GetExtendAllocatorArgs(extendAllocatorArgs);
        IList<string> superiorArgs = dictArgs.ContainsKey("superior") ? dictArgs["superior"] : new List<string>();
        this.ltlOrgAssign.Text = GetAssignSuperiorHtml(superiorArgs);
    }

    private void LoadFieldDropList(Guid workflowId)
    {
        // 绑定流程字段列表.对字段进行控制的数据类型有：单选框,下拉框.
        IList<FieldInfo> items = workflowFieldService.GetControllableFields(workflowId);
        this.ddlFields.DataSource = items;
        this.ddlFields.DataTextField = "HeaderText";
        this.ddlFields.DataValueField = "FieldName";
        this.ddlFields.DataBind();
        this.ddlFields.Items.Insert(0, new ListItem("－ 请选择字段 －", ""));
    }

    private void BindFieldControlInfo(Guid workflowId, string workflowName, string activityName, string fieldName)
    {
        if (!string.IsNullOrEmpty(fieldName))
        {
            IList<FieldControlInfo> sources = new List<FieldControlInfo>();
            FieldInfo fitem = workflowFieldService.GetField(workflowId, fieldName);
            if (fitem != null)
            {
                // 当有可控制的数据时，方才显示编辑界面
                IList<FieldControlInfo> fields = fitem.ToEmptyFieldControls(workflowName, activityName);
                if (fields != null && fields.Count > 0)
                {
                    IList<FieldControlInfo> existsItems = workflowFieldService.GetFieldControls(workflowName, activityName, fieldName);
                    if (existsItems == null || existsItems.Count == 0)
                    {
                        sources = fields;
                        divExistsFields.InnerHtml = string.Format("未设置 - {0}.", fieldName);
                    }
                    else
                    {
                        IDictionary<string, FieldControlInfo> dict = new Dictionary<string, FieldControlInfo>();
                        foreach (FieldControlInfo item in existsItems)
                        {
                            string key = item.FieldValue.ToUpper();
                            if (!dict.ContainsKey(key))
                            {
                                dict.Add(key, item);
                            }
                        }
                        foreach (FieldControlInfo item in fields)
                        {
                            string key = item.FieldValue.ToUpper();
                            if (dict.ContainsKey(key))
                            {
                                sources.Add(dict[key]);
                            }
                            else
                            {
                                sources.Add(item);
                            }
                        }
                        divExistsFields.InnerHtml = string.Format("已经设置 - {0}.", fieldName);
                    }
                }
            }
            this.rptFieldControls.DataSource = sources;
        }
        else
        {
            divExistsFields.InnerHtml = "";
        }
        this.rptFieldControls.DataBind();
    }

    #region generate html

    /// <summary>
    /// 获取下行的组织控制.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    protected string GetSuperiorHtml(IList<string> args)
    {
        StringBuilder builder = new StringBuilder();
        bool isChecked = false;
        builder.AppendLine("<table border=\"0\" cellspacing=\"0\" style=\"margin:0; padding:0\">");
        builder.AppendLine("<tr><td>");

        isChecked = args.Contains("1");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg1\" name=\"chkOrgArgs\" value=\"1\"{0} /><label for=\"chkOrgArg1\">所有上级主管</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("2");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg2\" name=\"chkOrgArgs\" value=\"2\"{0} /><label for=\"chkOrgArg2\">同部门上级主管</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("3");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg3\" name=\"chkOrgArgs\" value=\"3\"{0} /><label for=\"chkOrgArg3\">直接主管</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("4");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg4\" name=\"chkOrgArgs\" value=\"4\"{0} /><label for=\"chkOrgArg4\">同部门其他人员</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td></tr>\r\n<tr><td>");

        isChecked = args.Contains("5");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg5\" name=\"chkOrgArgs\" value=\"5\"{0} /><label for=\"chkOrgArg5\">同科室其他人员</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("6");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg6\" name=\"chkOrgArgs\" value=\"6\"{0} /><label for=\"chkOrgArg6\">室审核</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("7");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg7\" name=\"chkOrgArgs\" value=\"7\"{0} /><label for=\"chkOrgArg7\">部门审核</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("8");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg8\" name=\"chkOrgArgs\" value=\"8\"{0} /><label for=\"chkOrgArg8\">公司领导审核</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td></tr>");
        builder.AppendLine("</table>");

        return builder.ToString();
    }

    /// <summary>
    /// 获取转交的组织控制.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    protected string GetAssignSuperiorHtml(IList<string> args)
    {
        StringBuilder builder = new StringBuilder();
        bool isChecked = false;
        builder.AppendLine("<table border=\"0\" cellspacing=\"0\" style=\"margin:0; padding:0\">");
        builder.AppendLine("<tr><td>");

        isChecked = args.Contains("4");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArgsAssign1\" name=\"chkOrgArgsAssign\" value=\"4\"{0} /><label for=\"chkOrgArgsAssign1\">同部门其他人员</label>", GetCheckedAttribute(isChecked));
        builder.AppendLine("</td><td>");

        isChecked = args.Contains("5");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArgsAssign2\" name=\"chkOrgArgsAssign\" value=\"5\"{0} /><label for=\"chkOrgArgsAssign2\">同科室其他人员</label>", GetCheckedAttribute(isChecked));
        builder.AppendLine("</td><td>");

        isChecked = args.Contains("9");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArgsAssign3\" name=\"chkOrgArgsAssign\" value=\"9\"{0} /><label for=\"chkOrgArgsAssign3\">全公司人员</label>", GetCheckedAttribute(isChecked));
        
        builder.AppendLine("</td></tr></table>");
        return builder.ToString();
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

    #endregion

    #endregion

    #region 分派控制

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

    private static bool EnableResouceController(string resouceId)
    {
        if (string.IsNullOrEmpty(resouceId) || resouceId.StartsWith(ResourceHelper.PrefixDisableResource, StringComparison.OrdinalIgnoreCase))
            return false;
        return true;
    }

    #endregion

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        TextBox txtTargetUsers = e.Item.FindControl("txtTargetUsers") as TextBox;
        HtmlAnchor btnPopupUserSelector = e.Item.FindControl("btnPopupUserSelector") as HtmlAnchor;
        if (txtTargetUsers != null && btnPopupUserSelector != null)
        {
            btnPopupUserSelector.Attributes["onclick"] = string.Format("javascrpt:return openUserSelector('{0}');", txtTargetUsers.ClientID);
        }
    }

    protected void ddlFields_SelectedIndexChanged(object sender, EventArgs e)
    {
        string fieldName = ddlFields.SelectedValue;
        this.BindFieldControlInfo(this.WorkflowId, this.WorkflowName, this.ActivityName, fieldName);
    }

    #region 保存

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Guid activityId = this.ActivityId;
        Guid workflowId = this.WorkflowId;
        string workflowName = this.WorkflowName;
        string activityName = this.ActivityName;

        IList<FieldControlInfo> fieldControls = this.GetFieldControlValues(workflowName, activityName);
        //foreach (FieldControlInfo item in controlItems)
        //{
        //    string msg = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", item.WorkflowName, item.ActivityName, item.FieldName, item.Id.ToString(), item.FieldValue, item.TargetUsers);
        //    log.Warn(msg);
        //}

        workflowFieldService.UpdateFieldControls(fieldControls);

        ActivityDefinition activity = this.GetActivityAllocatorValues(activityId, activityName);
        activityDefinitionService.UpdateActivityAllocators(activity);
        //log.WarnFormat("{0}-{1}-{2}-{3}-{4}-{5}", activity.CommandRules, activity.AllocatorUsers, activity.AllocatorResource, activity.ExtendAllocators, activity.ExtendAllocatorArgs, activity.DefaultAllocator);

        AllocatorOption assignAllocator = this.GetAssignAllocatorValues(activityId);
        taskAssignService.UpdateAssignmentAllocators(assignAllocator);
        //log.WarnFormat("{0}-{1}-{2}-{3}-{4}", assignAllocator.AllocatorUsers, assignAllocator.AllocatorResource, assignAllocator.ExtendAllocators, assignAllocator.ExtendAllocatorArgs, assignAllocator.DefaultAllocator);

        ShowSuccess("更新流程步骤任务分派设置成功.", AppPath + "contrib/workflow/pages/config/configWorkflow.aspx?wid=" + workflowId);
    }

    /// <summary>
    /// 获取字段控制值

    /// </summary>
    /// <returns></returns>
    public IList<FieldControlInfo> GetFieldControlValues(string workflowName, string activityName)
    {
        IList<FieldControlInfo> items = new List<FieldControlInfo>();

        string fieldName = this.ddlFields.SelectedValue;
        int count = this.rptFieldControls.Items.Count;
        log.Warn("Fields Count:" + count);
        foreach (RepeaterItem row in this.rptFieldControls.Items)
        {
            HiddenField hiddenFieldId = row.FindControl("hiddenFieldId") as HiddenField;
            TextBox txtFieldValue = row.FindControl("txtFieldValue") as TextBox;
            TextBox txtTargetUsers = row.FindControl("txtTargetUsers") as TextBox;
            if (hiddenFieldId != null && txtFieldValue != null && txtTargetUsers != null)
            {
                int id = Convert.ToInt32(hiddenFieldId.Value);
                string fieldValue = txtFieldValue.Text.Trim();
                string targetUsers = txtTargetUsers.Text.Trim();

                FieldControlInfo item = new FieldControlInfo(workflowName, activityName, fieldName, fieldValue);
                item.Id = id;
                item.TargetUsers = targetUsers;
                items.Add(item);
            }
        }

        return items;
    }

    /// <summary>
    /// 获取流程步骤下行分派值.
    /// </summary>
    /// <param name="activityId"></param>
    /// <param name="activityName"></param>
    /// <returns></returns>
    public ActivityDefinition GetActivityAllocatorValues(Guid activityId, string activityName)
    {
        // starter,processor,superior,field
        string superiorArgs = Request.Form["chkOrgArgs"];

        ActivityDefinition activity = new ActivityDefinition();
        activity.ActivityId = activityId;
        activity.CommandRules = this.txtCommandRules.Text;

        activity.DefaultAllocator = this.selDefaultTypes.Value;
        activity.AllocatorUsers = this.chkUsers.Checked ? this.txtUsers.Text.Trim() : null;
        activity.AllocatorResource = FormatResourceId(this.ResourceId, this.chkRes.Checked);

        string extendAllocators = string.Empty;
        string extendAllocatorArgs = string.Empty;
        if (this.chkStarter.Checked)
        {
            extendAllocators += ",starter";
        }
        if (this.chkPssor.Checked)
        {
            extendAllocators += ",processor";
        }
        if (this.chkOrg.Checked)
        {
            extendAllocators += ",superior";
            if (!string.IsNullOrEmpty(superiorArgs))
            {
                extendAllocatorArgs += ";superior:" + superiorArgs;
            }
        }
        if (this.chkField.Checked)
        {
            extendAllocators += ",field";
            extendAllocatorArgs += ";field:" + activityName;
        }
        if (extendAllocators.Length > 1)
            extendAllocators = extendAllocators.Remove(0, 1);
        if (extendAllocatorArgs.Length > 1)
            extendAllocatorArgs = extendAllocatorArgs.Remove(0, 1);

        activity.ExtendAllocators = extendAllocators;
        activity.ExtendAllocatorArgs = extendAllocatorArgs;

        return activity;
    }

    /// <summary>
    /// 获取流程步骤平转分派值.
    /// </summary>
    /// <param name="activityId"></param>
    /// <returns></returns>
    public AllocatorOption GetAssignAllocatorValues(Guid activityId)
    {
        // starter,processor,superior
        string assignSuperiorArgs = Request.Form["chkOrgArgsAssign"];

        AllocatorOption option = new AllocatorOption();
        option.ActivityId = activityId;
        option.DefaultAllocator = this.selAssignDefaultTypes.Value;
        option.AllocatorUsers = this.chkUsersAssign.Checked ? this.txtUsersAssign.Text.Trim() : null;
        option.AllocatorResource = FormatResourceId(this.AssignResourceId, this.chkResAssign.Checked);

        string extendAllocators = string.Empty;
        string extendAllocatorArgs = string.Empty;
        if (this.chkStarterAssign.Checked)
        {
            extendAllocators += ",starter";
        }
        if (this.chkPssorAssign.Checked)
        {
            extendAllocators += ",processor";
        }
        if (this.chkOrgAssign.Checked)
        {
            extendAllocators += ",superior";
            if (!string.IsNullOrEmpty(assignSuperiorArgs))
            {
                extendAllocatorArgs = "superior:" + assignSuperiorArgs;
            }
        }
        if (extendAllocators.Length > 1)
            extendAllocators = extendAllocators.Remove(0, 1);

        option.ExtendAllocators = extendAllocators;
        option.ExtendAllocatorArgs = extendAllocatorArgs;

        return option;
    }

    private static string FormatResourceId(string resourceId, bool isChecked)
    {
        if (string.IsNullOrEmpty(resourceId))
            return string.Empty;

        string results = resourceId.ToUpper();
        bool isContain = results.StartsWith(ResourceHelper.PrefixDisableResource, StringComparison.OrdinalIgnoreCase);
        string disablePattern = ResourceHelper.PrefixDisableResource.ToUpper();

        if (isChecked)
        {
            // 允许权限控制
            return (isContain ? results.Replace(disablePattern, "") : results);
        }
        else
        {
            return (isContain ? results : disablePattern + results);
        }
    }

    #endregion
}
