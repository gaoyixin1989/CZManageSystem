using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Web;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Commons;
using Botwave.XQP.Domain; 

public partial class apps_gmcc_controls_LeftMenu : Botwave.Security.Web.UserControlBase
{

    private static string GroupImageRoot = AppPath + "contrib/workflow/res/groups/";
    private static string WorkflowUrlFormat = AppPath + "apps/xqp2/pages/workflows/workflowIndex.aspx?wid={0}";

    /// <summary>
    /// 流程菜单 HTML.
    /// </summary>
    public string WorkflowMenuHtml = string.Empty;

    public string CZWorkflowInstanceHtml = string.Empty;

    private IWorkflowResourceService workflowResourceService = (IWorkflowResourceService)Ctx.GetObject("workflowResourceService");

    #region properties

    /// <summary>
    /// 流程资源服务.
    /// </summary>
    public IWorkflowResourceService WorkflowResourceService
    {
        get { return workflowResourceService; }
        set { workflowResourceService = value; }
    }

    /// <summary>
    /// 接入应用系统名称.
    /// </summary>
    public string AppName
    {
        get
        {
            if (null != Session["AppName"])
                return (string)Session["AppName"]; ;
            return null;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            Botwave.Security.LoginUser user = CurrentUser;
            if (user == null)
                ShowError(MessageHelper.Message_NoPermission, AppPath + "contrib/security/pages/login.aspx");

            this.BindMenu(user);

            this.CZWorkflowInstanceHtml = Botwave.XQP.Domain.CZWorkflowInterface.Build("外部流程接入", "subWorkflowInstance");
        }
    }

    private void BindMenu(Botwave.Security.LoginUser user)
    {
        IList<WorkflowMenuGroup> groups = WorkflowMenuGroup.Select();
        IList<WorkflowInMenuGroup> groupWorkflows = null;

        //根据应用系统接入配置获取流程组

        if (string.IsNullOrEmpty(AppName))
        {
            groupWorkflows = WorkflowInMenuGroup.Select();
        }
        else
        {
            Apps appsInfo = Apps.LoadByName(AppName);
            if (null == appsInfo)
                ShowError("对不起，不存在该应用系统接入！", AppPath + "Login.aspx");
            groupWorkflows = WorkflowInMenuGroup.Select(appsInfo.Settings);
        }
        IDictionary<string, string> userResources = user.Resources;
        IDictionary<string, string> workflowResources = workflowResourceService.GetWorkflowResources(ResourceHelper.Workflow_ResourceId, ResourceHelper.ResourceType_Workflow);

        this.WorkflowMenuHtml = BuildMenuHtml(groups, groupWorkflows, workflowResources, userResources);
    }

    private static string BuildMenuHtml(IList<WorkflowMenuGroup> groups,
        IList<WorkflowInMenuGroup> groupWorkflows,
        IDictionary<string, string> workflowResources,
        IDictionary<string, string> userResources)
    {
        StringBuilder builder = new StringBuilder();

        int groupIndex = 1;
        foreach (WorkflowMenuGroup item in groups)
        {
            int workflowCount = 0;
            string groupHtml = BuildGroupHtml(item, groupWorkflows, workflowResources, userResources, out workflowCount);
            if (workflowCount == 0)
                continue;
            string groupName = item.GroupName;
            if (groupName.Length > 10)
                groupName = groupName.Substring(0, 10) + "..";
            bool isExpand = groupName.StartsWith("默认");
            builder.AppendLine(string.Format("<li><h2 onclick=\"changeView(this.id)\" id=\"submenuh2_{0}\"><img src=\"{1}app_themes/gmcc/img/{3}.gif\" id=\"ico_submenuh2_{0}\" />{2}</h2>", groupIndex, AppPath, groupName, isExpand ? "ico_nohave" : "ico_have"));
            builder.AppendLine(string.Format("<div class=\"navigation\" id=\"div_submenuh2_{0}\" style=\"display:{1}\">", groupIndex, isExpand ? "block" : "none"));
            builder.AppendLine(groupHtml);
            builder.AppendLine("</div></li>");
            groupIndex++;
        }

        return builder.ToString();
    }

    private static string BuildGroupHtml(WorkflowMenuGroup group,
        IList<WorkflowInMenuGroup> groupWorkflows,
        IDictionary<string, string> workflowResources,
        IDictionary<string, string> userResources,
        out int workflowCount)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("<div class=\"menuItems\">");
        workflowCount = 0;
        for (int i = 0; i < groupWorkflows.Count; i++)
        {
            WorkflowInMenuGroup item = groupWorkflows[i];
            if (item.MenuGroupId == group.GroupID)
            {
                string workflowName = item.WorkflowName.ToLower();
                if (workflowResources.ContainsKey(workflowName))
                {
                    string requireResouceId = workflowResources[workflowName] + "0000"; // 取得指定流程公用权限.
                    if (!XQPHelper.VerifyAccessResource(userResources, requireResouceId))
                        continue;

                    string name = item.WorkflowName;
                    //if (name.Length > 7)
                    //    name = name.Substring(0, 6) + "..";
                    string alias = null;
                    if (!string.IsNullOrEmpty(item.AliasImage))
                        alias = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{1}\" />", item.WorkflowAlias, GroupImageRoot + item.AliasImage);
                    else
                        alias = string.Format("[{0}]", item.WorkflowAlias);

                    builder.AppendFormat("<a href=\"{0}\" target=\"rightFrame\" title=\"{1}\">{2}{3}</a>", string.Format(WorkflowUrlFormat, item.WorkflowId), item.WorkflowName, alias, name);
                    workflowCount++; //菜单数加 1
                    // 移除当前流程子目录，减少循环次数.
                    groupWorkflows.RemoveAt(i);
                    i--;
                }
            }
        }
        builder.AppendLine("</div>");
        return builder.ToString();
    }
}
