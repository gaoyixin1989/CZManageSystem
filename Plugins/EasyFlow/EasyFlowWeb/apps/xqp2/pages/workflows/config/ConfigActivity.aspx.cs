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
using Botwave.XQP.Domain;
using Botwave.XQP.Service;
using Botwave.Security.Domain;

public partial class xqp2_contrib_workflow_pages_config_ConfigActivity : Botwave.Security.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(xqp2_contrib_workflow_pages_config_ConfigActivity));

    #region service interfaces

    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private ITaskAssignService taskAssignService = (ITaskAssignService)Ctx.GetObject("taskAssignService");
    private IWorkflowFieldService workflowFieldService = (IWorkflowFieldService)Ctx.GetObject("workflowFieldService");
    private IWorkflowRoleService workflowRoleService = (IWorkflowRoleService)Ctx.GetObject("workflowRoleService");
    private string rootDeptId = "34920440002";
    private string expandDeptId = "3492044000201";
    private string rootDeptName = "广州移动通信公司";

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

    public IWorkflowRoleService WorkflowRoleService
    {
        set { workflowRoleService = value; }
    }

    /// <summary>
    /// 显示树的根部门编号.
    /// </summary>
    public string RootDeptId
    {
        set { rootDeptId = value; }
    }

    /// <summary>
    /// 展开显示的部门编号.
    /// </summary>
    public string ExpandDeptId
    {
        set { expandDeptId = value; }
    }

    /// <summary>
    /// 根结点名称.
    /// </summary>
    public string RootNodeName
    {
        set { rootDeptName = value; }
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
            CZActivityDefinition czactivity = CZActivityDefinition.GetWorkflowActivityByActivityId(activityId);
            AllocatorOption assignmentAllocator = taskAssignService.GetAssignmentAllocator(activityId);

            if (activity == null)
            {
                ShowError("未找到指定流程步骤.");
            }
            this.LoadActivity(activity);

            this.txtPrintAmount.Text = czactivity.PrintAmount.ToString();
            this.radOpenPrint.Checked = czactivity.CanPrint > -1;
            this.radClosePrint.Checked = czactivity.CanPrint == -1;
            this.chkOption.Checked = czactivity.CanEdit == 1;
            this.chkReturn.Checked = czactivity.ReturnToPrev == true;
            this.chkIsMobile.Checked = czactivity.IsMobile;
            this.chkboxIsAuto.Checked = czactivity.IsTimeOutContinue;
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

        IList<ActivityDefinition> activityDefinition = activityDefinitionService.GetActivitiesByWorkflowId(activity.WorkflowId);
        drdlActivities.DataSource = activityDefinition;
        drdlActivities.DataTextField = "ActivityName";
        drdlActivities.DataValueField = "ActivityName";
        drdlActivities.DataBind();
        drdlActivities.Items.RemoveAt(drdlActivities.Items.Count - 1);
        drdlActivities.Dispose();
        drdlActivities.Items.Insert(0, new ListItem("- 请选择 -", ""));

        drdlActivitiesAssign.DataSource = activityDefinition;
        drdlActivitiesAssign.DataTextField = "ActivityName";
        drdlActivitiesAssign.DataValueField = "ActivityName";
        drdlActivitiesAssign.DataBind();
        drdlActivitiesAssign.Items.RemoveAt(drdlActivitiesAssign.Items.Count - 1);
        drdlActivitiesAssign.Dispose();
        drdlActivitiesAssign.Items.Insert(0, new ListItem("- 请选择 -", ""));

        drdlPssor.DataSource = activityDefinition;
        drdlPssor.DataTextField = "ActivityName";
        drdlPssor.DataValueField = "ActivityName";
        drdlPssor.DataBind();
        drdlPssor.Items.RemoveAt(drdlPssor.Items.Count - 1);
        drdlPssor.Dispose();
        drdlPssor.Items.Insert(0, new ListItem("- 请选择 -", ""));

        drdlPssorAssign.DataSource = activityDefinition;
        drdlPssorAssign.DataTextField = "ActivityName";
        drdlPssorAssign.DataValueField = "ActivityName";
        drdlPssorAssign.DataBind();
        drdlPssorAssign.Items.RemoveAt(drdlPssorAssign.Items.Count - 1);
        drdlPssorAssign.Dispose();
        drdlPssorAssign.Items.Insert(0, new ListItem("- 请选择 -", ""));

        DataRow[] source = Botwave.XQP.Commons.XQPHelper.GetAllDepartments().Select(string.Format("ParentDpId = '{0}'", this.rootDeptId));
        drdlDepartments.Items.Insert(0, new ListItem("所有部门经理", "所有部门经理"));
        drdlDepartments.Items.Insert(1, new ListItem("所有部门副经理", "所有部门副经理"));
        for (int i = 0; i <= source.Length - 1; i++)
        {
            drdlDepartments.Items.Insert(i + 2, new ListItem(source[i]["dpfullname"].ToString(), source[i]["dpid"].ToString()));
        }
        drdlDepartmentsAssign.Items.Insert(0, new ListItem("所有部门经理", "所有部门经理"));
        drdlDepartmentsAssign.Items.Insert(1, new ListItem("所有部门副经理", "所有部门副经理"));
        for (int i = 0; i <= source.Length - 1; i++)
        {
            drdlDepartmentsAssign.Items.Insert(i + 2, new ListItem(source[i]["dpfullname"].ToString(), source[i]["dpid"].ToString()));
        }

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
            if (allocatorList.Contains("processctl"))
            {
                this.chkPssctl.Checked = true;
                this.orgPssor.Style.Remove("display");
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
            if (allocatorList.Contains("role"))
            {
                this.chkRole.Checked = true;
                this.trRole.Style.Remove("display");
            }
            if (allocatorList.Contains("activity"))
            {
                this.chkControl.Checked = true;
                this.orgType.Style.Remove("display");
            }
            if (allocatorList.Contains("owner"))
            {
                this.chkToOwner.Checked = true;
                this.ownerType.Style.Remove("display");
            }
        }

        IDictionary<string, IList<string>> dictArgs = GetExtendAllocatorArgs(extendAllocatorArgs);

        IList<string> superiorArgs = dictArgs.ContainsKey("superior") ? dictArgs["superior"] : new List<string>();
        drdlActivities.SelectedValue = dictArgs.ContainsKey("activity") ? dictArgs["activity"][0] : "";
        drdlPssor.SelectedValue = dictArgs.ContainsKey("processctl") ? dictArgs["processctl"][0] : "";
        this.ltlOrg.Text = GetSuperiorHtml(superiorArgs);

        IList<string> roleArgs = dictArgs.ContainsKey("role") ? dictArgs["role"] : new List<string>();
        this.ltlRole.Text = GetRoleHtml(roleArgs,activity.AllocatorResource);

        if (dictArgs.ContainsKey("owner"))
            BindOwnerControl(dictArgs["owner"]);
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
            if (allocatorList.Contains("processctl"))
            {
                this.chkPssctlAssign.Checked = true;
                this.orgPssorAssign.Style.Remove("display");
            }
            if (allocatorList.Contains("superior"))
            {
                this.chkOrgAssign.Checked = true;
                this.trOrgAssign.Style.Remove("display");
            }
            if (allocatorList.Contains("role"))
            {
                this.chkRoleAssign.Checked = true;
                this.trOrgAssign.Style.Remove("display");
            }
            if (allocatorList.Contains("activity"))
            {
                this.chkControlAssign.Checked = true;
                this.orgTypeAssign.Style.Remove("display");
            }
            if (allocatorList.Contains("owner"))
            {
                this.chkToOwnerAssign.Checked = true;
                this.ownerAssignType.Style.Remove("display");
            }
        }

        IDictionary<string, IList<string>> dictArgs = GetExtendAllocatorArgs(extendAllocatorArgs);
        IList<string> superiorArgs = dictArgs.ContainsKey("superior") ? dictArgs["superior"] : new List<string>();
        drdlActivitiesAssign.SelectedValue = dictArgs.ContainsKey("activity") ? dictArgs["activity"][0] : "";
        drdlPssorAssign.SelectedValue = dictArgs.ContainsKey("processctl") ? dictArgs["processctl"][0] : "";
        this.ltlOrgAssign.Text = GetAssignSuperiorHtml(superiorArgs);

        IList<string> roleArgs = dictArgs.ContainsKey("role") ? dictArgs["role"] : new List<string>();
        this.ltlRoleAssign.Text = GetAssignRoleHtml(roleArgs,option.AllocatorResource);

        if (dictArgs.ContainsKey("owner"))
            BindOwnerControlAssign(dictArgs["owner"]);
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

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("5");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg5\" name=\"chkOrgArgs\" value=\"5\"{0} /><label for=\"chkOrgArg5\">同科室其他人员</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td></tr>\r\n<tr><td>");


        isChecked = args.Contains("6");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg6\" name=\"chkOrgArgs\" value=\"6\"{0} /><label for=\"chkOrgArg6\">室审核</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("7");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg7\" name=\"chkOrgArgs\" value=\"7\"{0} /><label for=\"chkOrgArg7\">部门正经理</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("11");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg11\" name=\"chkOrgArgs\" value=\"11\"{0} /><label for=\"chkOrgArg11\">部门副经理</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("8");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg8\" name=\"chkOrgArgs\" value=\"8\"{0} /><label for=\"chkOrgArg8\">公司领导审核</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");
        isChecked = args.Contains("9");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg9\" name=\"chkOrgArgs\" value=\"9\"{0} /><label for=\"chkOrgArg9\">店面经理</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");
        isChecked = args.Contains("10");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg10\" name=\"chkOrgArgs\" value=\"10\"{0} /><label for=\"chkOrgArg10\">综合员</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");
        isChecked = args.Contains("12");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg12\" name=\"chkOrgArgs\" value=\"12\"{0} /><label for=\"chkOrgArg12\">区域副总监</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");
        isChecked = args.Contains("all");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArgall\" name=\"chkOrgArgs\" value=\"all\"{0} /><label for=\"chkOrgArg11\">全公司人员</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td>");

        builder.AppendLine("</table>");

        return builder.ToString();
    }

    /// <summary>
    /// 获取角色控制
    /// </summary>
    /// <param name="args"></param>
    /// <param name="AllocatorResource"></param>
    /// <returns></returns>
    protected string GetRoleHtml(IList<string> args, string AllocatorResource)
    {
        if (!string.IsNullOrEmpty(AllocatorResource))
        {
            string resourceId = AllocatorResource.Split('_').Length > 1 ? AllocatorResource.Split('_')[1] : AllocatorResource;
            IList<RoleInfo> infoList = workflowRoleService.GetRoleInfoByResourceId(resourceId);
            int index = 0;
            StringBuilder builder = new StringBuilder();
            bool isChecked = false;
            builder.AppendLine("<table border=\"0\" cellspacing=\"0\" style=\"margin:0; padding:0\">");
            builder.AppendLine("<tr>");
            foreach (RoleInfo info in infoList)
            {
                builder.AppendLine("<td>");
                isChecked = args.Contains(info.RoleId.ToString());
                builder.AppendFormat("<input type=\"checkbox\" id=\"chkRoleArg{0}\" name=\"chkRoleArgs\" value=\"{1}\"{3} /><label for=\"chkRoleArg{0}\">{2}</label>", index, info.RoleId, info.RoleName, GetCheckedAttribute(isChecked));

                builder.AppendLine("</td>");
                index++;
                if (index % 6 == 0 && index >= 6)
                    builder.AppendLine("</tr><tr>");
            }
            builder.AppendLine("</tr>");
            builder.AppendLine("</table>");
            return builder.ToString();
        }
        return string.Empty;
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

        builder.AppendLine("</td><td>");
        isChecked = args.Contains("12");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArgsAssign12\" name=\"chkOrgArgsAssign\" value=\"12\"{0} /><label for=\"chkOrgArgsAssign12\">区域副总监</label>", GetCheckedAttribute(isChecked));

        isChecked = args.Contains("9");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArgsAssign3\" name=\"chkOrgArgsAssign\" value=\"9\"{0} /><label for=\"chkOrgArgsAssign3\">全公司人员</label>", GetCheckedAttribute(isChecked));
        
        builder.AppendLine("</td></tr></table>");
        return builder.ToString();
    }

    /// <summary>
    /// 获取砖家角色控制
    /// </summary>
    /// <param name="args"></param>
    /// <param name="AllocatorResource"></param>
    /// <returns></returns>
    protected string GetAssignRoleHtml(IList<string> args, string AllocatorResource)
    {
        if (!string.IsNullOrEmpty(AllocatorResource))
        {
            string resourceId = AllocatorResource.Split('_').Length > 1 ? AllocatorResource.Split('_')[1] : AllocatorResource;
            IList<RoleInfo> infoList = workflowRoleService.GetRoleInfoByResourceId(resourceId);
            int index = 0;
            StringBuilder builder = new StringBuilder();
            bool isChecked = false;
            builder.AppendLine("<table border=\"0\" cellspacing=\"0\" style=\"margin:0; padding:0\">");
            builder.AppendLine("<tr>");
            foreach (RoleInfo info in infoList)
            {
                builder.AppendLine("<td>");
                isChecked = args.Contains(info.RoleId.ToString());
                builder.AppendFormat("<input type=\"checkbox\" id=\"chkRoleAssignArg{0}\" name=\"chkRoleAssignArgs\" value=\"{1}\"{3} /><label for=\"chkRoleAssignArg{0}\">{2}</label>", index, info.RoleId, info.RoleName, GetCheckedAttribute(isChecked));

                builder.AppendLine("</td>");
                index++;
                if (index % 6 == 0 && index>=6)
                    builder.AppendLine("</tr><tr>");
            }
            builder.AppendLine("</tr>");
            builder.AppendLine("</table>");
            return builder.ToString();
        }
        return string.Empty;
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

    private void BindOwnerControl(IList<string> extArgs)
    {
        foreach (string extArg in extArgs)
        {
            string[] args = extArg.Split('$');
            switch (args.Length)
            {
                case 1:
                    if (drdlDepartments.Items.FindByText(args[0]) != null)
                        drdlDepartments.Items.FindByText(args[0]).Selected = true;
                    //BinddrdlOffice(drdlDepartments.SelectedValue);
                    break;
                case 2:
                    if (drdlDepartments.Items.FindByText(args[0]) != null)
                        drdlDepartments.Items.FindByText(args[0]).Selected = true;
                    //BinddrdlOffice(drdlDepartments.SelectedValue);
                    if (extArgs.Contains(args[0] + "$40"))
                    {
                        radDepartment.Enabled = true; 
                        radDepartment.Checked = true;
                    }
                    if (extArgs.Contains(args[0] + "$50")) {
                        radDepartment2.Enabled = true;
                        radDepartment2.Checked = true;
                    }
                    if (extArgs.Contains(args[0] + "$60"))
                    {
                        radOffice.Enabled = true;
                        radOffice.Checked = true;
                    }
                   if(extArgs.Contains(args[0] + "$user")) 
                       radUser.Checked = true;
                   break;
                case 3:
                    if (drdlDepartments.Items.FindByText(args[0]) != null)
                        drdlDepartments.Items.FindByText(args[0]).Selected = true;
                    BinddrdlOffice(drdlDepartments.SelectedItem.Text);
                    drdlOffice.SelectedValue = args[1];
                    if ("60" == args[2])
                    {
                        radOffice.Enabled = true;
                        radOffice.Checked = true;
                    }
                    if ("user" == args[2]) { radUser.Checked = true; }
                    radDepartment2.Checked = false;
                    radDepartment2.Enabled = false;
                    radDepartment.Checked = false;
                    radDepartment.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        if (drdlOffice.Items.Count < 1)
            BinddrdlOffice(drdlDepartments.SelectedItem.Text);
    }

    private void BindOwnerControlAssign(IList<string> extArgs)
    {
        foreach (string extArg in extArgs)
        {
            string[] args = extArg.Split('$');
            switch (args.Length)
            {
                case 1:
                    if (drdlDepartmentsAssign.Items.FindByText(args[0]) != null)
                        drdlDepartmentsAssign.Items.FindByText(args[0]).Selected = true;
                    //BinddrdlOfficeAssign(drdlDepartmentsAssign.SelectedValue);
                    break;
                case 2:
                    if (drdlDepartmentsAssign.Items.FindByText(args[0]) != null)
                        drdlDepartmentsAssign.Items.FindByText(args[0]).Selected = true;
                    //BinddrdlOfficeAssign(drdlDepartmentsAssign.SelectedValue);
                    if (extArgs.Contains(args[0] + "$40"))
                    {
                        radDepartmentAssign.Enabled = true;
                        radDepartmentAssign.Checked = true;
                    }
                    if (extArgs.Contains(args[0] + "$50"))
                    {
                        radDepartment2Assign.Enabled = true;
                        radDepartment2Assign.Checked = true;
                    }
                    if (extArgs.Contains(args[0] + "$60"))
                    {
                        radOfficeAssign.Enabled = true;
                        radOfficeAssign.Checked = true;
                    }
                    if (extArgs.Contains(args[0] + "$user"))
                        radUserAssign.Checked = true;
                    break;
                case 3:
                    if (drdlDepartmentsAssign.Items.FindByText(args[0]) != null)
                        drdlDepartmentsAssign.Items.FindByText(args[0]).Selected = true;
                    BinddrdlOfficeAssign(drdlDepartmentsAssign.SelectedItem.Text);
                    drdlOfficeAssign.SelectedValue = args[1];
                    if ("60" == args[2])
                    {
                        radOfficeAssign.Enabled = true;
                        radOfficeAssign.Checked = true;
                    }
                    if ("user" == args[2]) radUserAssign.Checked = true;
                    radDepartment2Assign.Checked = false;
                    radDepartment2Assign.Enabled = false;
                    radDepartmentAssign.Checked = false;
                    radDepartmentAssign.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        if (drdlOfficeAssign.Items.Count < 1)
            BinddrdlOfficeAssign(drdlDepartmentsAssign.SelectedItem.Text);
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
        //更新打印控制
        CZActivityDefinition czactivity = CZActivityDefinition.GetWorkflowActivityByActivityId(activityId);
        if (radOpenPrint.Checked)
            czactivity.CanPrint = 1;
        if (radClosePrint.Checked)
            czactivity.CanPrint = -1;
        czactivity.PrintAmount = Botwave.Commons.DbUtils.ToInt32(txtPrintAmount.Text);
        if (chkOption.Checked)
            czactivity.CanEdit = 1;
        else
            czactivity.CanEdit = -1;
        czactivity.ReturnToPrev = this.chkReturn.Checked;
        czactivity.IsMobile = this.chkIsMobile.Checked;
        czactivity.IsTimeOutContinue = this.chkboxIsAuto.Checked;
        CZActivityDefinition.UpdateWorkflowActivityPrint(czactivity);
       
        ShowSuccess("更新流程步骤任务分派设置成功.", AppPath + "apps/xqp2/pages/workflows/config/ConfigWorkflow.aspx?wid=" + workflowId);
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
        string roleArgs = Request.Form["chkRoleArgs"];

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
        if (this.chkPssctl.Checked)
        {
            extendAllocators += ",processctl";
            if (!string.IsNullOrEmpty(drdlPssor.SelectedValue))
            {
                extendAllocatorArgs += ";processctl:" + drdlPssor.SelectedValue;
            }
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
        if (this.chkRole.Checked)
        {
            extendAllocators += ",role";
            extendAllocatorArgs += ";role:" + roleArgs;
        }
        if (this.chkControl.Checked)
        {
            extendAllocators += ",activity";
            if (!string.IsNullOrEmpty(drdlActivities.SelectedValue))
            {
                extendAllocatorArgs += ";activity:" + drdlActivities.SelectedValue;
            }
        } 
        if (this.chkToOwner.Checked)
        {
            string args = "";
            switch (drdlDepartments.SelectedItem.Text)
            {
                case "所有部门经理":
                    args += "," + drdlDepartments.SelectedItem.Text;
                    break;
                case "所有部门副经理":
                    args += "," + drdlDepartments.SelectedItem.Text;
                    break;
                case "公司领导":
                    args += "," + drdlDepartments.SelectedItem.Text;
                    break;
                case "高级经理":
                    args += "," + drdlDepartments.SelectedItem.Text;
                    break;
                default:
                    if (drdlOffice.SelectedIndex == 0)
                    {

                        if (radDepartment.Checked)
                            args += "," + drdlDepartments.SelectedItem.Text + "$40";
                        if (radDepartment2.Checked)
                            args += "," + drdlDepartments.SelectedItem.Text + "$50";
                        if (radOffice.Checked)
                            args += "," + drdlDepartments.SelectedItem.Text + "$60";
                        if (radUser.Checked)
                            args += "," + drdlDepartments.SelectedItem.Text + "$user";
                    }
                    else
                    {
                        if (radOffice.Checked)
                            args += "," + drdlDepartments.SelectedItem.Text + "$" + drdlOffice.SelectedItem.Text + "$60";
                        if (radUser.Checked)
                            args += "," + drdlDepartments.SelectedItem.Text + "$" + drdlOffice.SelectedItem.Text + "$user";
                    }
                    break;
            }
            extendAllocatorArgs += !string.IsNullOrEmpty(args) ? ";owner:" + args.Substring(1) : "";
            extendAllocators += !string.IsNullOrEmpty(args) ? ",owner" : "";
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
        string assighRoleArgs = Request.Form["chkRoleAssignArgs"];
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
        if (this.chkPssctlAssign.Checked)
        {
            extendAllocators += ",processctl";
            if (!string.IsNullOrEmpty(drdlPssorAssign.SelectedValue))
            {
                extendAllocatorArgs += ";processctl:" + drdlPssorAssign.SelectedValue;
            }
        }
        if (this.chkOrgAssign.Checked)
        {
            extendAllocators += ",superior";
            if (!string.IsNullOrEmpty(assignSuperiorArgs))
            {
                extendAllocatorArgs += "superior:" + assignSuperiorArgs;
            }
        }
        if (this.chkRoleAssign.Checked)
        {
            extendAllocators += ",role";
            if (!string.IsNullOrEmpty(assighRoleArgs))
            {
                extendAllocatorArgs += "role:" + assighRoleArgs;
            }
        }
        if (this.chkControlAssign.Checked)
        {
            extendAllocators += ",activity";
            if (!string.IsNullOrEmpty(drdlActivitiesAssign.SelectedValue))
            {
                extendAllocatorArgs += ";activity:" + drdlActivitiesAssign.SelectedValue;
            }
        }
        if (this.chkToOwnerAssign.Checked)
        {
            string args = "";
            switch (drdlDepartmentsAssign.SelectedItem.Text)
            {
                case "所有部门经理":
                    args += "," + drdlDepartmentsAssign.SelectedItem.Text;
                    break;
                case "所有部门副经理":
                    args += "," + drdlDepartmentsAssign.SelectedItem.Text;
                    break;
                case "公司领导":
                    args += "," + drdlDepartmentsAssign.SelectedItem.Text;
                    break;
                case "高级经理":
                    args += "," + drdlDepartmentsAssign.SelectedItem.Text;
                    break;
                default:
                    if (drdlOfficeAssign.SelectedIndex == 0)
                    {

                        if (radDepartmentAssign.Checked)
                            args += "," + drdlDepartmentsAssign.SelectedItem.Text + "$40";
                        if (radDepartment2Assign.Checked)
                            args += "," + drdlDepartmentsAssign.SelectedItem.Text + "$50";
                        if (radOfficeAssign.Checked)
                            args += "," + drdlDepartmentsAssign.SelectedItem.Text + "$60";
                        if (radUserAssign.Checked)
                            args += "," + drdlDepartmentsAssign.SelectedItem.Text + "$user";
                    }
                    else
                    {
                        if (radOfficeAssign.Checked)
                            args += "," + drdlDepartmentsAssign.SelectedItem.Text + "$" + drdlOfficeAssign.SelectedItem.Text + "$60";
                        if (radUserAssign.Checked)
                            args += "," + drdlDepartmentsAssign.SelectedItem.Text + "$" + drdlOfficeAssign.SelectedItem.Text + "$user";
                    }
                    break;
            }
            extendAllocatorArgs += !string.IsNullOrEmpty(args) ? ";owner:" + args.Substring(1) : "";
            extendAllocators += !string.IsNullOrEmpty(args) ? ",owner" : "";
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

    /// <summary>
    /// 绑定下行分派室经理下拉框
    /// </summary>
    /// <param name="dpname"></param>
    private void BinddrdlOffice(string dpname)
    {
        drdlOffice.Items.Clear();
        drdlOffice.Dispose();
        switch (dpname)
        {
            case "所有部门经理":
                radDepartment2.Checked = false;
                radDepartment2.Enabled = false;
                radDepartment.Checked = false;
                radDepartment.Enabled = false;
                radOffice.Checked = false;
                radOffice.Enabled = false;
                drdlOffice.Enabled = false;
                radUser.Checked = false;
                radUser.Enabled = false;
                break;
            case "所有部门副经理":
                radDepartment2.Checked = false;
                radDepartment2.Enabled = false;
                radDepartment.Checked = false;
                radDepartment.Enabled = false;
                radOffice.Checked = false;
                radOffice.Enabled = false;
                drdlOffice.Enabled = false;
                radUser.Checked = false;
                radUser.Enabled = false;
                break;
            case "公司领导":
                radDepartment2.Checked = false;
                radDepartment2.Enabled = false;
                radDepartment.Checked = false;
                radDepartment.Enabled = false;
                radOffice.Checked = false;
                radOffice.Enabled = false;
                drdlOffice.Enabled = false;
                 radUser.Checked = false;
                radUser.Enabled = false;
                break;
            case "高级经理":
                radDepartment2.Checked = false;
                radDepartment2.Enabled = false;
                radDepartment.Checked = false;
                radDepartment.Enabled = false;
                radOffice.Checked = false;
                radOffice.Enabled = false;
                drdlOffice.Enabled = false;
                 radUser.Checked = false;
                radUser.Enabled = false;
                break;
            default:
                drdlOffice.Enabled = true;
                DataRow[] source = Botwave.XQP.Commons.XQPHelper.GetAllDepartments().Select(string.Format(" parentdpid = '{0}'", drdlDepartments.SelectedValue));
                drdlOffice.Items.Insert(0, new ListItem("- 请选择 -", ""));
                for (int i = 0; i <= source.Length - 1; i++)
                {
                    drdlOffice.Items.Insert(i + 1, new ListItem(source[i]["dpname"].ToString(), source[i]["dpname"].ToString()));
                }
                radDepartment2.Enabled = true;
                radDepartment.Enabled = true;
                radOffice.Enabled = true;
                radUser.Enabled = true;
                break;
        }
    }

    /// <summary>
    /// 绑定平转分派室经理下拉框
    /// </summary>
    /// <param name="dpname"></param>
    private void BinddrdlOfficeAssign(string dpname)
    {
        drdlOfficeAssign.Items.Clear();
        drdlOfficeAssign.Dispose();
        switch (dpname)
        {
            case "所有部门经理":
                radDepartment2Assign.Checked = false;
                radDepartment2Assign.Enabled = false;
                radDepartmentAssign.Checked = false;
                radDepartmentAssign.Enabled = false;
                radOfficeAssign.Checked = false;
                radOfficeAssign.Enabled = false;
                drdlOfficeAssign.Enabled = false;
                radUserAssign.Enabled = false;
                radUserAssign.Checked = false;
                break;
            case "所有部门副经理":
                radDepartment2Assign.Checked = false;
                radDepartment2Assign.Enabled = false;
                radDepartmentAssign.Checked = false;
                radDepartmentAssign.Enabled = false;
                radOfficeAssign.Checked = false;
                radOfficeAssign.Enabled = false;
                drdlOfficeAssign.Enabled = false;
                radUserAssign.Enabled = false;
                radUserAssign.Checked = false;
                break;
            case "公司领导":
                radDepartment2Assign.Checked = false;
                radDepartment2Assign.Enabled = false;
                radDepartmentAssign.Checked = false;
                radDepartmentAssign.Enabled = false;
                radOfficeAssign.Checked = false;
                radOfficeAssign.Enabled = false;
                drdlOfficeAssign.Enabled = false;
                radUserAssign.Enabled = false;
                radUserAssign.Checked = false;
                break;
            case "高级经理":
                radDepartment2Assign.Checked = false;
                radDepartment2Assign.Enabled = false;
                radDepartmentAssign.Checked = false;
                radDepartmentAssign.Enabled = false;
                radOfficeAssign.Checked = false;
                radOfficeAssign.Enabled = false;
                drdlOfficeAssign.Enabled = false;
                radUserAssign.Enabled = false;
                radUserAssign.Checked = false;
                break;
            default:
                drdlOfficeAssign.Enabled = true;
                DataRow[] source = Botwave.XQP.Commons.XQPHelper.GetAllDepartments().Select(string.Format(" parentdpid = '{0}'", drdlDepartmentsAssign.SelectedValue));
                drdlOfficeAssign.Items.Insert(0, new ListItem("- 请选择 -", ""));
                for (int i = 0; i <= source.Length - 1; i++)
                {
                    drdlOfficeAssign.Items.Insert(i + 1, new ListItem(source[i]["dpname"].ToString(), source[i]["dpname"].ToString()));
                }
                radDepartment2Assign.Enabled = true;
                radDepartmentAssign.Enabled = true;
                radOfficeAssign.Enabled = true;
                radUserAssign.Enabled = true;
                break;
        }
    }

    #endregion

    protected void drdlDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        BinddrdlOffice(drdlDepartments.SelectedItem.Text);
    }

    protected void drdlOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (drdlOffice.SelectedValue)
        {
            case "":
                radDepartment2.Enabled = true;
                radDepartment.Enabled = true;
                radOffice.Enabled = false;
                radOffice.Checked = false;
                break;
            default:
                radDepartment.Checked = false;
                radDepartment.Enabled = false;
                radDepartment2.Checked = false;
                radDepartment2.Enabled = false;
                radOffice.Checked = true;
                radOffice.Enabled = true;
                break;
        }
    }

    protected void drdlDepartmentsAssign_SelectedIndexChanged(object sender, EventArgs e)
    {
        BinddrdlOfficeAssign(drdlDepartmentsAssign.SelectedItem.Text);
    }

    protected void drdlOfficeAssign_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (drdlOfficeAssign.SelectedValue)
        {
            case "":
                radDepartment2Assign.Enabled = true;
                radDepartmentAssign.Enabled = true;
                radOfficeAssign.Enabled = false;
                radOfficeAssign.Checked = false;
                break;
            default:
                radDepartmentAssign.Checked = false;
                radDepartmentAssign.Enabled = false;
                radDepartment2Assign.Checked = false;
                radDepartment2Assign.Enabled = false;
                radOfficeAssign.Checked = true;
                radOfficeAssign.Enabled = true;
                break;
        }
    }
}
