using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using Botwave.Commons;
using Botwave.Entities;
using Botwave.Web;
using Botwave.Web.Controls;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Service;
using Botwave.Extension.IBatisNet;

public partial class contrib_workflow_pages_PopupForward : Botwave.Security.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_workflow_pages_PopupForward));
    private static readonly string returnUrl = "contrib/workflow/pages/default.aspx";
    private static readonly string messagePage_success = "contrib/msg/pages/success.aspx";
    private static readonly string messagePage_error = "contrib/msg/pages/error.aspx";

    private ITaskAssignService taskAssignService = (ITaskAssignService)Ctx.GetObject("taskAssignService");
    private IWorkflowUserService workflowUserService = (IWorkflowUserService)Ctx.GetObject("workflowUserService");

    public ITaskAssignService TaskAssignService
    {
        set { taskAssignService = value; }
    }

    public IWorkflowUserService WorkflowUserService
    {
        set { workflowUserService = value; }
    }

    public Guid ActivityInstanceId
    {
        get
        {
            object obj = ViewState["ActivityInstanceId"];
            if (null == obj)
            {
                ShowOpenerError("未指定转交的流程步骤.", "workflow/default.aspx");
            }
            return (Guid)obj;
        }
        set
        {
            ViewState["ActivityInstanceId"] = value;
        }
    }

    public string Actor
    {
        get { return (string)ViewState["Actor"]; }
        set { ViewState["Actor"] = value; }
    }

    public bool IsAssignFromCompany = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["aiid"] == null)
            {
                ShowOpenerError("未指定转交的流程步骤.", returnUrl);
            }
            Botwave.Security.Web.LogWriterFactory.Writer = new Botwave.XQP.Service.Support.DefaultLogWriter();
            Guid activityInstanceId = new Guid(Request.QueryString["aiid"]);
            Guid workflowInstanceId = new Guid(Request.QueryString["wiid"]);
            txtRemark.Text = Request.QueryString["txt"].ToString();
            string actor = CurrentUser.UserName;
            this.Actor = actor;
            this.ActivityInstanceId = activityInstanceId;
            this.IsAssignFromCompany = this.SetIsAssignFromCompany(activityInstanceId);
            bool hasAssignments = this.BindAssignmentActors(activityInstanceId, workflowInstanceId, actor);
            this.BindControl(hasAssignments);
        }
    }

    #region bind

    private void BindControl(bool hasAssignmentActor)
    {
        if (!hasAssignmentActor && !IsAssignFromCompany)
        {
            divMesages.InnerHtml = "<span style=\"color:red;\">对不起，目前没有人可以进行任务转交.</span>";
            btnFw1.Visible = false;
            btnFw2.Visible = false;
        }
    }

    /// <summary>
    /// 重复列数.
    /// </summary>
    const int columnCount = 8;

    private bool BindAssignmentActors(Guid activityInstanceId, Guid workflowInstanceId, string actor)
    {
        #region 增加过程控制，解决个性化定制权限角色(针对组织控制类型和历史处理人类型)
        AllocatorOption options = IBatisMapper.Load<AllocatorOption>("bwwf_AssignmentAllocator_Select_ByActivityInstanceId", activityInstanceId);
        string extendAllocatorArgs = options.ExtendAllocatorArgs;
        string actor1 = Botwave.XQP.Domain.CZActivityInstance.GetPssorActor(workflowInstanceId,extendAllocatorArgs);
        actor = string.IsNullOrEmpty(actor1) ? actor : actor1;
        #endregion
        IDictionary<string, string> dict = taskAssignService.GetAssignmentActors(workflowInstanceId, activityInstanceId, actor);
        if (dict == null || dict.Count == 0)
            return false;

        // 过滤掉已经在待办列表中的用户.
        IList<BasicUser> todoActors = taskAssignService.GetTodoActors(activityInstanceId);
        if (todoActors != null || todoActors.Count > 0)
        {
            foreach (BasicUser user in todoActors)
            {
                string key = user.UserName;
                if (dict.ContainsKey(key))
                    dict.Remove(key);
            }
        }

        IDictionary<string, string> actors = workflowUserService.GetActorRealNames(dict.Keys);
        if (actors == null || actors.Count == 0)
            return false;

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("<div style=\"margin-bottom:5px;margin-top:5px\"><table>");
        int index = 0;
        foreach (string key in actors.Keys)
        {
            if (index % columnCount == 0)
                builder.Append("<tr>");
            if (key.Equals(actor, StringComparison.OrdinalIgnoreCase))
                continue;
            builder.AppendFormat("<td><input type=\"radio\" name=\"chkboxUser\" value=\"{0}\" /><span tooltip=\"{0}\">{1}</span></td>", key, actors[key]);

            if (index % columnCount == columnCount - 1)
                builder.Append("</tr>");
            index++;
        }
        int newIndex = index;
        while (newIndex % columnCount != 0)
        {
            builder.Append("<td></td>");
            newIndex++;
        }
        if (newIndex > index)
            builder.Append("</tr>");
        builder.AppendLine("</table></div>");
        this.ltlAssignmentActors.Text = builder.ToString();
        return true;
    }
    #endregion

    protected void btnFw_Click(object sender, EventArgs e)
    {
        string selectedValue = Request.Form["chkboxUser"];
        if (string.IsNullOrEmpty(selectedValue))
        {
            divMesages.InnerHtml = "<font color=red>请选择转交人.</font>";
        }
        else
        {
            string userName = this.Actor;
            string message = txtRemark.Text.Trim();
            string[] selectedUsers = selectedValue.Split(',');
            if (selectedUsers == null || selectedUsers.Length == 0)
            {
                divMesages.InnerHtml = "<font color=red>请选择转交人.</font>";
                return;
            }
            Guid activityInstanceId = this.ActivityInstanceId;
            string assignedUser = selectedUsers[0];

            if (taskAssignService.GetTodoInfo(activityInstanceId, assignedUser) != null)
            {
                WriteNomalLog(userName, "转交工单", "转交工单失败，被转交人已经存在于待办列表中.");
                ShowOpenerError("转交工单失败，被转交人已经存在于待办列表中.", returnUrl);
            }
            Assignment assignment = new Assignment();
            assignment.ActivityInstanceId = activityInstanceId;
            assignment.AssignedTime = DateTime.Now;
            assignment.AssignedUser = assignedUser;
            assignment.AssigningUser = userName;
            assignment.Message = message;
            try
            {
                taskAssignService.Assign(assignment);  // 转交信息.
                WriteNomalLog(userName, "转交工单", "转交工单成功");
                ShowOpenerSuccess("转交工单成功", returnUrl);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                WriteNomalLog(userName, "转交工单", "转交工单失败.");
                ShowOpenerError("转交工单失败.", returnUrl);
            }
        }
    }

    #region URL 重定向

    protected void RedirectOpener(string url)
    {
        Response.Write(string.Format("<script type=\"text/javascript\">window.close();\r\n window.opener.location = \"{0}\";</script>", url));
    }

    protected void ShowOpenerError(string message, string returnUrl)
    {
        MessageHelper.MessageContent = message;
        string url = string.Format("{0}?returnUrl={1}", (AppPath + messagePage_error), this.Server.UrlEncode(AppPath + returnUrl));
        this.RedirectOpener(url);
    }

    protected void ShowOpenerSuccess(string message, string returnUrl)
    {
        MessageHelper.MessageContent = message;
        string url = string.Format("{0}?returnUrl={1}", (AppPath + messagePage_success), this.Server.UrlEncode(AppPath + returnUrl));
        this.RedirectOpener(url);
    }

    #endregion

    protected bool SetIsAssignFromCompany(Guid activityInstanceId)
    {
        return taskAssignService.IsAssignFromCompany(activityInstanceId);
    }
}
