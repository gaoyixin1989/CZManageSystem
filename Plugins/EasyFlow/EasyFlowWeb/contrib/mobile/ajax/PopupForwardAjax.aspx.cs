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

public partial class contrib_mobile_pages_PopupForwardAjax : Botwave.XQP.Web.Security.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_mobile_pages_PopupForwardAjax));
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

    //public Guid ActivityInstanceId
    //{
    //    get
    //    {
    //        object obj = ViewState["ActivityInstanceId"];
    //        if (null == obj)
    //        {
    //            ShowOpenerError("未指定转交的流程步骤.", "workflow/default.aspx");
    //        }
    //        return (Guid)obj;
    //    }
    //    set
    //    {
    //        ViewState["ActivityInstanceId"] = value;
    //    }
    //}

    //public string Actor
    //{
    //    get { return (string)ViewState["Actor"]; }
    //    set { ViewState["Actor"] = value; }
    //}

    public bool IsAssignFromCompany = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        string aiid = this.Context.Request["aiid"];
        string wiid = this.Context.Request["wiid"];
        //this.Context.Response.ContentType = "text/plain";
        if (string.IsNullOrEmpty(aiid))
            this.Context.Response.Write("{\"result\":\"error\",\"info\":\"未指定转交的流程步骤.\"}");
        else
        {
            Botwave.Security.Web.LogWriterFactory.Writer = new Botwave.XQP.Service.Support.DefaultLogWriter();
            Guid activityInstanceId = new Guid(aiid);
            Guid workflowInstanceId = new Guid(wiid);
            string actor = CurrentUser.UserName;
            //this.Actor = actor;
            //this.ActivityInstanceId = activityInstanceId;
            //this.IsAssignFromCompany = this.SetIsAssignFromCompany(activityInstanceId);
            this.BindAssignmentActors(activityInstanceId, workflowInstanceId, actor);
            //this.BindControl(hasAssignments);
        }
        Response.End();
    }

    #region bind

    private void BindControl(bool hasAssignmentActor)
    {
        if (!hasAssignmentActor)
        {
            Response.Write("{\"result\":\"error\",\"info\":\"<span style='color:red;'>对不起，目前没有人可以进行任务转交.</span>\"}");
        }
    }

    /// <summary>
    /// 重复列数.
    /// </summary>
    const int columnCount = 8;

    private void BindAssignmentActors(Guid activityInstanceId, Guid workflowInstanceId, string actor)
    {
        string json = string.Empty;
        IDictionary<string, string> dict = taskAssignService.GetAssignmentActors(workflowInstanceId, activityInstanceId, actor);
        if (dict == null || dict.Count == 0)
        {
            //return false;
            json = "{\"result\":\"error\",\"info\":\"<span style='color:red;'>对不起，目前没有人可以进行任务转交.</span>\"}";
        }
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
        {
            json = "{\"result\":\"error\",\"info\":\"<span style='color:red;'>对不起，目前没有人可以进行任务转交.</span>\"}";
        }
        StringBuilder builder = new StringBuilder();
        int index = 0;
        if (actors.Keys.Count > 0)
        {
            builder.Append(",{\"title\":\"从同部门/科室中选择转交人\",\"html\":\"<div style='line-height: 1.8em;'>");
            foreach (string key in actors.Keys)
            {
                if (key.Equals(actor, StringComparison.OrdinalIgnoreCase))
                    continue;
                builder.Append("<label style='margin-bottom: 1em;'><input type='radio' name='chkboxUser' value='" + key + "' id='chkboxUser" + key + "' /><span for='chkboxUser" + key + "' tooltip='" + key + "' style='margin-right: 1.6em;'>" + actors[key] + "</span></label>&nbsp;");
            }
            builder.Append("</div>\"}");
        }
        if (this.SetIsAssignFromCompany(activityInstanceId))
        {
            builder.Append(",{\"title\":\"从全公司人员中选择转交人\",\"html\":\"<div id='divAssignActorFromCompany' style='line-height: 1.8em;'></div><button type='button' onclick='javascrpt:return popupforward.OpenPopupUserPick();' title='点击从全公司人员中选择转交人' class='btn btn-info' style='margin-left:6px;'>点击选择转交人</button>\"}");
        }
        if (builder.Length > 0)
        {
            json = "{\"result\":\"success\",\"info\":[" + builder.Remove(0, 1).ToString() + "]}";
            //return true;
        }
        //return false;
        else
            json = "{\"result\":\"error\",\"info\":\"<span style='color:red;'>对不起，目前没有人可以进行任务转交.</span>\"}";
        Context.Response.Write(json);
    }
    #endregion

    protected bool SetIsAssignFromCompany(Guid activityInstanceId)
    {
        return taskAssignService.IsAssignFromCompany(activityInstanceId);
    }
}
