using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Botwave.Web;
using Botwave.Workflow;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;
using Botwave.XQP.Service.Plugins;
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;
using Botwave.GMCCServiceHelpers;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Practices.CZMCC.Support;

public partial class contrib_workflow_controls_ReviewInputSelector : Botwave.XQP.Web.Controls.WorkflowReviewSelector
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_workflow_controls_ReviewInputSelector));
    private IWorkflowService workflowService;
    private IActivityService activityService;
    private IWorkflowUserService workflowUserService;

    public IWorkflowService WorkflowService
    {
        set { workflowService = value; }
    }

    public IActivityService ActivityService
    {
        set { activityService = value; }
    }

    public IWorkflowUserService WorkflowUserService
    {
        set { workflowUserService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            
        }
    }

    public IList<string> GetSelectUsers()
    {
        string values = this.txtReviewActors.Value.Trim();
        if (string.IsNullOrEmpty(values))
            return new string[0];
        string[] results = values.Split(',', '，');
        return GetDistinctActors(results);
    }


    #region IWorkflowReviewService 成员

    /// <summary>
    /// 发送待阅.
    /// </summary>
    /// <param name="workflowProfile"></param>
    /// <param name="activityInstanceId"></param>
    /// <param name="workflowId"></param>
    /// <param name="workflowTitle"></param>
    /// <param name="sender"></param>
    /// <returns></returns>
    public override bool PendingReview(WorkflowProfile workflowProfile, Guid activityInstanceId, Guid workflowId, string workflowTitle, Botwave.Entities.BasicUser sender)
    {
        IList<string> users = GetSelectUsers();
        if (workflowProfile == null || !workflowProfile.IsReview || this.Visible == false || users.Count == 0)
            return false;

        string senderName = sender.UserName;
        string senderRealName = sender.RealName;
        // 消息内容.
        string message = workflowProfile.ReviewNotifyMessage;
        if (!string.IsNullOrEmpty(message))
        {
            message = message.ToLower().Replace("#title#", workflowTitle).Replace("#from#", senderRealName);
        }

        // 为每个下行步骤设置抄送人(待阅人)
        IList<ActivityInstance> nextActivities = activityService.GetNextActivities(activityInstanceId);
        if (nextActivities == null || nextActivities.Count == 0)
        {
            log.Error("未能找到下行步骤列表. 直接发送到 activityInstanceId:" + activityInstanceId.ToString());
            ToReview.OnPendingReview(activityInstanceId, message, senderName, users);
            return true;
        }
        string activityNames = string.Empty;
        IDictionary<Guid, string> sendedDict = new Dictionary<Guid, string>();
        foreach (ActivityInstance item in nextActivities)
        {
            if (sendedDict.ContainsKey(item.ActivityId))
                continue;
            ToReview.OnPendingReview(item.ActivityInstanceId, message, senderName, users);
            sendedDict.Add(item.ActivityId, string.Empty);
        }
        return true;
    }

    #endregion

    /// <summary>
    /// 格式化指定提醒信息内容.
    /// </summary>
    /// <param name="messageFormat"></param>
    /// <param name="workflowTitle"></param>
    /// <param name="activityName"></param>
    /// <param name="operateType"></param>
    /// <returns></returns>
    protected static string FormatNotifyMessage(string messageFormat, string creator, string workflowTitle, string activityName, int operateType)
    {
        messageFormat = messageFormat.ToLower();
        messageFormat = messageFormat.Replace("处理", "知晓");
        messageFormat = messageFormat.Replace("#creator#", creator);
        messageFormat = messageFormat.Replace("#title#", workflowTitle);
        messageFormat = messageFormat.Replace("#activityname#", activityName);
        messageFormat = messageFormat.Replace("#operatetype#", operateType == TodoInfo.OpBack ? "退回" : "进入");
        return messageFormat;
    }

    protected static IList<string> GetDistinctActors(string[] actors)
    {
        if (actors == null || actors.Length == 0)
            return new List<string>();
        IDictionary<string, string> dict = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

        for (int i = 0; i < actors.Length; i++)
        {
            string value = actors[i];
            if (!dict.ContainsKey(value))
                dict.Add(value, value);
        }

        return new List<string>(dict.Keys);
    }

    protected void InsertReviews(Guid activityInstanceId, string sender, IList<string> users)
    {
        if (activityInstanceId == Guid.Empty || users == null || users.Count == 0)
            return;
        try
        {
            foreach (string user in users)
            {
                ToReview item = new ToReview(activityInstanceId, user, sender);
                item.Insert();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw ex;
        }
    }
}
