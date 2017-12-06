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
using Botwave.Entities;
using Botwave.XQP.Domain;

public partial class apps_xqp2_pages_workflows_config_ConfigIntelligentRemindNotice : Botwave.Security.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_xqp2_pages_workflows_config_ConfigIntelligentRemindNotice));

    #region service interfaces

    private ITaskAssignService taskAssignService = (ITaskAssignService)Ctx.GetObject("taskAssignService");

    public ITaskAssignService TaskAssignService
    {
        set { taskAssignService = value; }
    }
    #endregion
     
    #region properties

    public string WorkflowId
    {
        get { return (string)ViewState["WorkflowId"]; }
        set { ViewState["WorkflowId"] = value; }
    }

    public string SettingType
    {
        get { return (string)ViewState["SettingType"]; }
        set { ViewState["SettingType"] = value; }
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

    public string CopyType
    {
        get { return (string)ViewState["CopyType"]; }
        set { ViewState["CopyType"] = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = Request.QueryString["id"];
            string wid = Request.QueryString["wid"];
            string copyType = Request.QueryString["c"];
            SZIntelligentRemind activity = new SZIntelligentRemind();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(wid) || string.IsNullOrEmpty(copyType))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }

            activity = SZIntelligentRemind.SelectById(int.Parse(id));

            if (activity == null)
            {
                ShowError("未保存指定实例.", AppPath + "apps/xqp2/pages/workflows/config/ConfigIntelligentRemind.aspx?wid=" + wid);
            }

            this.WorkflowId = wid;
            this.WorkflowName = activity.WorkflowName;
            this.ActivityName = activity.ActivityName;
            this.CopyType = copyType;
            this.LoadActivity(activity);
        }
    }

    #region load

    protected void LoadActivity(SZIntelligentRemind activity)
    {
        this.ltlActivityName.Text = activity.ActivityName;
        string users = CopyType == "2" ? activity.AllocatorUsers : activity.WarnningAllocatorUsers;
        string extendAllocators = CopyType == "2" ? activity.ExtendAllocators : activity.WarnningExtendAllocators;
        string extendAllocatorArgs = CopyType == "2" ? activity.ExtendAllocatorArgs : activity.WarnningExtendAllocatorArgs;

        if (!string.IsNullOrEmpty(users))
        {
            this.txtUsers.Text = users;
            this.chkUsers.Checked = true;
            this.trUsers.Style.Remove("display");
        }

        IList<string> allocatorList = GetExtendAllocators(extendAllocators);
        if (allocatorList.Count > 0)
        {
            if (allocatorList.Contains("starter"))
            {
                this.chkStarter.Checked = true;
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
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg7\" name=\"chkOrgArgs\" value=\"7\"{0} /><label for=\"chkOrgArg7\">部门审核</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");

        isChecked = args.Contains("8");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg8\" name=\"chkOrgArgs\" value=\"8\"{0} /><label for=\"chkOrgArg8\">公司领导审核</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");
        isChecked = args.Contains("9");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg9\" name=\"chkOrgArgs\" value=\"9\"{0} /><label for=\"chkOrgArg9\">店面经理</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td><td>");
        isChecked = args.Contains("10");
        builder.AppendFormat("<input type=\"checkbox\" id=\"chkOrgArg10\" name=\"chkOrgArgs\" value=\"10\"{0} /><label for=\"chkOrgArg10\">综合员</label>", GetCheckedAttribute(isChecked));

        builder.AppendLine("</td>");

        builder.AppendLine("</table>");

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
            btnPopupUserSelector.Attributes["onclick"] = string.Format("javascrpt:return openUserSelector2('{0}');", txtTargetUsers.ClientID);
        }
    }

    #region 保存

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string workflowId = this.WorkflowId;
        string workflowName = this.WorkflowName;
        string activityName = this.ActivityName;
        SZIntelligentRemind activity = new SZIntelligentRemind();
        activity = SZIntelligentRemind.SelectById(int.Parse(Request.QueryString["id"]));

        //foreach (FieldControlInfo item in controlItems)
        //{
        //    string msg = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", item.WorkflowName, item.ActivityName, item.FieldName, item.Id.ToString(), item.FieldValue, item.TargetUsers);
        //    log.Warn(msg);
        //}

        activity = this.GetActivityAllocatorValues(activity);
        activity.Create();
        //log.WarnFormat("{0}-{1}-{2}-{3}-{4}-{5}", activity.CommandRules, activity.AllocatorUsers, activity.AllocatorResource, activity.ExtendAllocators, activity.ExtendAllocatorArgs, activity.DefaultAllocator);
        //log.WarnFormat("{0}-{1}-{2}-{3}-{4}", assignAllocator.AllocatorUsers, assignAllocator.AllocatorResource, assignAllocator.ExtendAllocators, assignAllocator.ExtendAllocatorArgs, assignAllocator.DefaultAllocator);

        ShowSuccess("更新通知人分派设置成功.", AppPath + "apps/xqp2/pages/workflows/config/ConfigIntelligentRemind.aspx?wid=" + workflowId);
    }



    /// <summary>
    /// 获取流程步骤下行分派值.
    /// </summary>
    /// <param name="activityId"></param>
    /// <param name="activityName"></param>
    /// <returns></returns>
    public SZIntelligentRemind GetActivityAllocatorValues(SZIntelligentRemind activity)
    {
        // starter,processor,superior,field
        string superiorArgs = Request.Form["chkOrgArgs"];

        if (CopyType == "2")
            activity.AllocatorUsers = this.chkUsers.Checked ? this.txtUsers.Text.Trim() : null;
        else if (CopyType == "1")
            activity.WarnningAllocatorUsers = this.chkUsers.Checked ? this.txtUsers.Text.Trim() : null;

        string extendAllocators = string.Empty;
        string extendAllocatorArgs = string.Empty;
        if (this.chkStarter.Checked)
        {
            extendAllocators += ",starter";
        }
        if (this.chkOrg.Checked)
        {
            extendAllocators += ",superior";
            if (!string.IsNullOrEmpty(superiorArgs))
            {
                extendAllocatorArgs += ";superior:" + superiorArgs;

                foreach (string arg in superiorArgs.Split(','))
                {
                    if (arg.ToUpper() == "ALL")
                    {
                        if (activity.AllocatorUsers != null)
                            activity.AllocatorUsers += "," + arg;
                        else
                            activity.AllocatorUsers = arg;
                    }
                }
            }
        }
        if (extendAllocators.Length > 1)
            extendAllocators = extendAllocators.Remove(0, 1);
        if (extendAllocatorArgs.Length > 1)
            extendAllocatorArgs = extendAllocatorArgs.Remove(0, 1);

        if (CopyType == "2")
        {
            activity.ExtendAllocators = extendAllocators;
            activity.ExtendAllocatorArgs = extendAllocatorArgs;
        }
        else if (CopyType == "1")
        {
            activity.WarnningExtendAllocators = extendAllocators;
            activity.WarnningExtendAllocatorArgs = extendAllocatorArgs;
        }

        return activity;
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
