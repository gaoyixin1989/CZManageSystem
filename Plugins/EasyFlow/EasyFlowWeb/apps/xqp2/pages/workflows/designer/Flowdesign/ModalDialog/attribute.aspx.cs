using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Service;
using Botwave.Workflow.Domain;
using Botwave.XQP.Domain;
using Botwave.Workflow.Extension.Util;
using System.Text;
using Botwave.Security.Domain;
using Newtonsoft.Json;
using System.Data;
using Botwave.Workflow.Routing.Implements;

public partial class apps_xqp2_pages_workflows_designer_Flowdesign_ModalDialog_attribute : Botwave.Security.Web.PageBase
{
    #region service interfaces

    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private ITaskAssignService taskAssignService = (ITaskAssignService)Ctx.GetObject("taskAssignService");
    private IWorkflowFieldService workflowFieldService = (IWorkflowFieldService)Ctx.GetObject("workflowFieldService");
    private IWorkflowRoleService workflowRoleService = (IWorkflowRoleService)Ctx.GetObject("workflowRoleService");
    private IActivityRulesService activityRulesService = (IActivityRulesService)Ctx.GetObject("activityRulesService");

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

    public IActivityRulesService ActivityRulesService
    {
        set { activityRulesService = value; }
    }
    #endregion
    #region properties

    public string WorkflowId
    {
        get { return (string)ViewState["WorkflowId"]; }
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

    protected override void OnInit(EventArgs e) { }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string aid = Request.QueryString["aid"];
            string processDataJson = Context.Request["ProcessData"];
            if (string.IsNullOrEmpty(processDataJson))
            {
                //ShowError(MessageHelper.Message_ArgumentException);
                processDataJson ="{}";
            }
            ProcessData processData = JsonConvert.DeserializeObject<ProcessData>(processDataJson);
            Guid activityId = new Guid(aid);
            WorkflowId =processData.Flow_id;
            //ActivityDefinition activity = processData.List.Where(l=>l.Process_id=activityId).First();
            CZActivityDefinition activity = processData.List.Where(l => l.Process_id == activityId).First().Activity;
            AllocatorOption assignmentAllocator =  processData.List.Where(l => l.Process_id == activityId).First().Assignment;

            if(activity!=null){
                IList<CZActivityDefinition> activityDefinition=new List<CZActivityDefinition>();
                foreach(list l in processData.List){
                    activityDefinition.Add(l.Activity);
                }

                this.LoadActivity(activity, activityDefinition.OrderBy(l => l.State).OrderBy (l=>l.SortOrder ).ToList());
            }
            if (activity == null)
            {
                //ShowError("未找到指定流程步骤.");
                activity = new CZActivityDefinition();
            }
            if (assignmentAllocator == null)
            {
                assignmentAllocator = new AllocatorOption();
            }
            
            this.txtPrintAmount.Text = activity.PrintAmount.ToString();
            this.radOpenPrint.Checked = activity.CanPrint > -1;
            this.radClosePrint.Checked = activity.CanPrint == -1;
            this.chkOption.Checked = activity.CanEdit == 1;
            this.chkReturn.Checked = activity.ReturnToPrev == true;
            this.chkIsMobile.Checked = activity.IsMobile;
            if (assignmentAllocator != null)
            {
                this.LoadAssingmentAllocator(assignmentAllocator);
            }
            this.ddlrejectOption.Value = activity.RejectOption;
            this.txtjoincondition.Value = activity.JoinCondition;
            this.txtsplitcondition.Value = activity.SplitCondition;
            this.txtcountersignedcondition.Value = activity.CountersignedCondition;
            Guid workflowId = activity.WorkflowId;
            string workflowName = WorkflowUtility.GetWorkflowName(workflowId);
            string activityName = activity.ActivityName;

            this.ActivityId = activityId;
            //this.WorkflowId = workflowId;
            this.WorkflowName = workflowName;
            this.ActivityName = activityName;
            txtActivityName.Text = activityName;
            this.ResourceId = activity.AllocatorResource;
            this.AssignResourceId = assignmentAllocator.AllocatorResource;
            string rulesJson= processData.List.Where(l => l.Process_id == activityId).First().Rules;
            this.LoadRules(rulesJson);
        }
    }

    #region load

    protected void LoadActivity(ActivityDefinition activity, IList<CZActivityDefinition> activityDefinition)
    {
        string users = activity.AllocatorUsers;
        string resource = activity.AllocatorResource;
        string extendAllocators = activity.ExtendAllocators;
        string extendAllocatorArgs = activity.ExtendAllocatorArgs;
        string defaultAllocator = activity.DefaultAllocator;

        drdlActivities.DataSource = activityDefinition;
        drdlActivities.DataTextField = "ActivityName";
        drdlActivities.DataValueField = "ActivityName";
        drdlActivities.DataBind();
        if(activityDefinition.Count>0)
            drdlActivities.Items.RemoveAt(drdlActivities.Items.Count - 1);
        drdlActivities.Dispose();
        drdlActivities.Items.Insert(0, new ListItem("- 请选择 -", ""));

        drdlActivitiesAssign.DataSource = activityDefinition;
        drdlActivitiesAssign.DataTextField = "ActivityName";
        drdlActivitiesAssign.DataValueField = "ActivityName";
        drdlActivitiesAssign.DataBind();
        if (activityDefinition.Count > 0)
            drdlActivitiesAssign.Items.RemoveAt(drdlActivitiesAssign.Items.Count - 1);
        drdlActivitiesAssign.Dispose();
        drdlActivitiesAssign.Items.Insert(0, new ListItem("- 请选择 -", ""));

        drdlPssor.DataSource = activityDefinition;
        drdlPssor.DataTextField = "ActivityName";
        drdlPssor.DataValueField = "ActivityName";
        drdlPssor.DataBind();
        if (activityDefinition.Count > 0)
            drdlPssor.Items.RemoveAt(drdlPssor.Items.Count - 1);
        drdlPssor.Dispose();
        drdlPssor.Items.Insert(0, new ListItem("- 请选择 -", ""));

        drdlPssorAssign.DataSource = activityDefinition;
        drdlPssorAssign.DataTextField = "ActivityName";
        drdlPssorAssign.DataValueField = "ActivityName";
        drdlPssorAssign.DataBind();
        if (activityDefinition.Count > 0)
            drdlPssorAssign.Items.RemoveAt(drdlPssorAssign.Items.Count - 1);
        drdlPssorAssign.Dispose();
        drdlPssorAssign.Items.Insert(0, new ListItem("- 请选择 -", ""));

        ddlcustomize.DataSource = activityDefinition.Where(l=>l.ActivityName!=activity.ActivityName);
        ddlcustomize.DataTextField = "ActivityName";
        ddlcustomize.DataValueField = "ActivityName";
        ddlcustomize.DataBind();
        if (activityDefinition.Count > 0)
            drdlPssorAssign.Items.RemoveAt(drdlPssorAssign.Items.Count - 1);
        ddlcustomize.Dispose();
        ddlcustomize.Items.Insert(0, new ListItem("- 请选择 -", ""));
        ddlcustomize.SelectedValue = activity.RejectOption;

        drdlNextActivity.Items.Clear();
        drdlNextActivity.Items.Insert(0, new ListItem("- 请选择 -", ""));
        for (int i=0;i<activity.NextActivityNames.Count;i++)
        {
            drdlNextActivity.Items.Insert(i+1,activity.NextActivityNames[i]);
        }

        if (!string.IsNullOrEmpty(defaultAllocator) && defaultAllocator.IndexOf(':') > -1)
        {
            defaultAllocator = defaultAllocator.Remove(defaultAllocator.IndexOf(':'));
        }
        this.selDefaultTypes.Value = defaultAllocator;

        if (!string.IsNullOrEmpty(users))
        {
            this.chkUsers.Checked = true;
            this.txtUsers.Value = users;
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
        }

        IDictionary<string, IList<string>> dictArgs = GetExtendAllocatorArgs(extendAllocatorArgs);

        IList<string> superiorArgs = dictArgs.ContainsKey("superior") ? dictArgs["superior"] : new List<string>();
        drdlActivities.SelectedValue = dictArgs.ContainsKey("activity") ? dictArgs["activity"][0] : "";
        drdlPssor.SelectedValue = dictArgs.ContainsKey("processctl") ? dictArgs["processctl"][0] : "";
        this.ltlOrg.Text = GetSuperiorHtml(superiorArgs);

        IList<string> roleArgs = dictArgs.ContainsKey("role") ? dictArgs["role"] : new List<string>();
        this.ltlRole.Text = GetRoleHtml(roleArgs, activity.AllocatorResource);
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
            this.txtUsersAssign.Value = users;
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
        }

        IDictionary<string, IList<string>> dictArgs = GetExtendAllocatorArgs(extendAllocatorArgs);
        IList<string> superiorArgs = dictArgs.ContainsKey("superior") ? dictArgs["superior"] : new List<string>();
        drdlActivitiesAssign.SelectedValue = dictArgs.ContainsKey("activity") ? dictArgs["activity"][0] : "";
        drdlPssorAssign.SelectedValue = dictArgs.ContainsKey("processctl") ? dictArgs["processctl"][0] : "";
        this.ltlOrgAssign.Text = GetAssignSuperiorHtml(superiorArgs);

        IList<string> roleArgs = dictArgs.ContainsKey("role") ? dictArgs["role"] : new List<string>();
        this.ltlRoleAssign.Text = GetAssignRoleHtml(roleArgs, option.AllocatorResource);
    }

    protected void LoadRules(string rulesJson)
    {
        if (!string.IsNullOrEmpty(rulesJson))
        {
            rulesJson = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", rulesJson);
            DataTable source = JsonConvert.DeserializeObject<DataTable>(rulesJson);
            listResults.DataSource = source;
            listResults.DataBind();
        }
        drdlFName.DataSource = activityRulesService.GetFormItemDefinitions(this.WorkflowId);
        drdlFName.DataTextField = "Name";
        drdlFName.DataValueField = "FName";
        drdlFName.DataBind();
        drdlFName.Dispose();
        drdlFName.Items.Insert(0, new ListItem("- 请选择 -", ""));
        drdlFName.Items.Insert(1, new ListItem("提单人部门", "提单人部门"));
        drdlFName.Items.Insert(2, new ListItem("提单人姓名", "提单人姓名"));
        drdlFName.Items.Insert(3, new ListItem("当前部门", "当前部门"));
        drdlFName.Items.Insert(4, new ListItem("当前用户姓名", "当前用户姓名"));
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

        isChecked = args.Contains("1");
        builder.AppendFormat("<label for=\"chkOrgArg1\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArg1\" name=\"chkOrgArgs\" value=\"1\"{0} />所有上级主管</label>", GetCheckedAttribute(isChecked));


        isChecked = args.Contains("2");
        builder.AppendFormat("<label for=\"chkOrgArg2\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArg2\" name=\"chkOrgArgs\" value=\"2\"{0} />同部门上级主管</label>", GetCheckedAttribute(isChecked));


        isChecked = args.Contains("3");
        builder.AppendFormat("<label for=\"chkOrgArg3\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArg3\" name=\"chkOrgArgs\" value=\"3\"{0} />直接主管</label>", GetCheckedAttribute(isChecked));

        isChecked = args.Contains("4");
        builder.AppendFormat("<label for=\"chkOrgArg4\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArg4\" name=\"chkOrgArgs\" value=\"4\"{0} />同部门其他人员</label>", GetCheckedAttribute(isChecked));

        isChecked = args.Contains("5");
        builder.AppendFormat("<label for=\"chkOrgArg5\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArg5\" name=\"chkOrgArgs\" value=\"5\"{0} />同科室其他人员</label>", GetCheckedAttribute(isChecked));



        isChecked = args.Contains("6");
        builder.AppendFormat("<label for=\"chkOrgArg6\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArg6\" name=\"chkOrgArgs\" value=\"6\"{0} />室审核</label>", GetCheckedAttribute(isChecked));


        isChecked = args.Contains("7");
        builder.AppendFormat("<label for=\"chkOrgArg7\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArg7\" name=\"chkOrgArgs\" value=\"7\"{0} />部门正经理</label>", GetCheckedAttribute(isChecked));


        isChecked = args.Contains("11");
        builder.AppendFormat("<label for=\"chkOrgArg11\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArg11\" name=\"chkOrgArgs\" value=\"11\"{0} />部门副经理</label>", GetCheckedAttribute(isChecked));


        isChecked = args.Contains("8");
        builder.AppendFormat("<label for=\"chkOrgArg8\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArg8\" name=\"chkOrgArgs\" value=\"8\"{0} />公司领导审核</label>", GetCheckedAttribute(isChecked));

        isChecked = args.Contains("9");
        builder.AppendFormat("<label for=\"chkOrgArg9\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArg9\" name=\"chkOrgArgs\" value=\"9\"{0} />店面经理</label>", GetCheckedAttribute(isChecked));

        isChecked = args.Contains("10");
        builder.AppendFormat("<label for=\"chkOrgArg10\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArg10\" name=\"chkOrgArgs\" value=\"10\"{0} />综合员</label>", GetCheckedAttribute(isChecked));

        isChecked = args.Contains("all");
        builder.AppendFormat("<label for=\"chkOrgArg10\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArgall\" name=\"chkOrgArgs\" value=\"all\"{0} />全公司人员</label>", GetCheckedAttribute(isChecked));

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

        isChecked = args.Contains("4");
        builder.AppendFormat("<label for=\"chkOrgArgsAssign1\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArgsAssign1\" name=\"chkOrgArgsAssign\" value=\"4\"{0} />同部门其他人员</label>", GetCheckedAttribute(isChecked));

        isChecked = args.Contains("5");
        builder.AppendFormat("<label for=\"chkOrgArgsAssign2\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArgsAssign2\" name=\"chkOrgArgsAssign\" value=\"5\"{0} />同科室其他人员</label>", GetCheckedAttribute(isChecked));

        isChecked = args.Contains("9");
        builder.AppendFormat("<label for=\"chkOrgArgsAssign3\" class=\"checkbox inline\"><input type=\"checkbox\" id=\"chkOrgArgsAssign3\" name=\"chkOrgArgsAssign\" value=\"9\"{0} />全公司人员</label>", GetCheckedAttribute(isChecked));

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
}