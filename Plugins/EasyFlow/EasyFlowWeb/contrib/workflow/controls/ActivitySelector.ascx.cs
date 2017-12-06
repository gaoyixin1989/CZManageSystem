using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.UI;
using Botwave.XQP.Service;
using Botwave.XQP.Domain;

public partial class contrib_workflow_controls_ActivitySelector : Botwave.Security.Web.UserControlBase
{
    #region service interfaces

    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IWorkflowSelectorFactory workflowSelectorFactory = (IWorkflowSelectorFactory)Ctx.GetObject("workflowSelectorFactory");
    private IActivityService activityService = (IActivityService)Ctx.GetObject("activityService");

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public IWorkflowSelectorFactory WorkflowSelectorFactory
    {
        set { workflowSelectorFactory = value; }
    }

    public IActivityService ActivityService
    {
        set { activityService = value; }
    }
    #endregion

    #region properties

    public Guid WorkflowInstanceId
    {
        get
        {
            if (ViewState["WorkflowInstanceId"] == null)
                return Guid.Empty;
            return (Guid)ViewState["WorkflowInstanceId"];
        }
        set
        {
            ViewState["WorkflowInstanceId"] = value;
        }
    }

    /// <summary>
    /// 是否只有唯一个下一步骤.
    /// </summary>
    public bool OnlyNext
    {
        get
        {
            if (ViewState["OnlyNext"] == null)
                return true;
            return (bool)ViewState["OnlyNext"];
        }
        set
        {
            ViewState["OnlyNext"] = value;
        }
    }

    /// <summary>
    /// “完成”步骤编号.
    /// </summary>
    public Guid EndActivityId
    {
        get { return (Guid)ViewState["EndActivityId"]; }
        set { ViewState["EndActivityId"] = value; }
    }

    /// <summary>
    /// 分支条件
    /// </summary>
    public string SplitCondition
    {
        get { return ViewState["SplitCondition"] as string; }
        set
        {
            string v = (null == value) ? String.Empty : value;
            ViewState["SplitCondition"] = v;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    { }

    #region Load Data

    /// <summary>
    /// 获取指定流程定义的起始步骤的分派数据.
    /// </summary>
    /// <param name="activityDefinition"></param>
    /// <param name="workflowId"></param>
    public void LoadStartData(ActivityDefinition activityDefinition, Guid workflowId)
    {
        IList<ActivityDefinition> activities = activityDefinitionService.GetStartActivities(workflowId);
        string selectorType = WorkflowSelectorManager.GetProfileName(workflowId);
        BuildNextActivities(null, activityDefinition, Guid.Empty, selectorType, activities);
    }

    /// <summary>
    /// 获取指定流程实例的起始步骤的分派数据.
    /// </summary>
    /// <param name="workflowInstance"></param>
    /// <param name="activityDefinition"></param>
    /// <param name="workflowInstanceId"></param>
    /// <param name="workflowId"></param>
    public void LoadStartDataByInstance(WorkflowInstance workflowInstance, ActivityDefinition activityDefinition, Guid workflowInstanceId, Guid workflowId)
    {
        this.WorkflowInstanceId = workflowInstanceId;
        IList<ActivityDefinition> activities = activityDefinitionService.GetStartActivitiesByWorkflowInstanceId(workflowInstanceId);
        string selectorType = WorkflowSelectorManager.GetProfileName(workflowId);
        BuildNextActivities(workflowInstance, activityDefinition, Guid.Empty, selectorType, activities);
    }

    /// <summary>
    /// 获取指定流程步骤实例的分派数据.
    /// </summary>
    /// <param name="workflowInstance"></param>
    /// <param name="activityDefinition"></param>
    /// <param name="workflowId"></param>
    /// <param name="activityInstanceId"></param>
    public void LoadData(WorkflowInstance workflowInstance, ActivityDefinition activityDefinition, Guid workflowId, Guid activityInstanceId)
    {
        IList<ActivityDefinition> activities = activityDefinitionService.GetNextActivityDefinitionsByInstanceId(activityInstanceId);
        string selectorType = WorkflowSelectorManager.GetProfileName(workflowId);
        BuildNextActivities(workflowInstance, activityDefinition, activityInstanceId, selectorType, activities);
    }

    #endregion

    #region Get Data

    /// <summary>
    /// 获取被分派的任务以及用户列表
    /// </summary>
    /// <returns>IDictionary<活动编号, IList<KeyValuePair<用户名, 授权人用户名>>></returns>
    public IDictionary<Guid, IDictionary<string, string>> GetActivityAllocatees()
    {
        //如果配置为完全由系统决定分支路径，将不会显示
        if (!this.Visible)
            return null;

        Guid endActivityId = this.EndActivityId;
        IDictionary<Guid, IDictionary<string, string>> dict = new Dictionary<Guid, IDictionary<string, string>>();

        string activityOptions = Request.Form["activityOption"];
        string activityAllocatees = Request.Form["activityAllocatee"];
        if (!string.IsNullOrEmpty(activityOptions))
        {
            string[] activityArray = activityOptions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (activityArray != null && activityArray.Length > 0)
            {
                foreach (string itemValue in activityArray)
                {
                    dict.Add(new Guid(itemValue), new Dictionary<string, string>());
                }
            }
        }

        if (string.IsNullOrEmpty(activityAllocatees))
        {
            // 当选中为 结束步骤时.
            if (endActivityId != Guid.Empty && endActivityId.ToString().Equals(activityOptions, StringComparison.OrdinalIgnoreCase))
            {
                dict[new Guid(activityOptions)] = null;
                return dict;
            }

            //可以不选择分支，此时按配置走.
            if (!this.OnlyNext && !string.IsNullOrEmpty(activityOptions))
                throw new WorkflowAllocateException("必须为流程分派任务处理人（即必须选择选定步骤的任务处理人）.");
        }
        else
        {
            string[] allocateeArray = activityAllocatees.Trim().Split(',');
            foreach (string item in allocateeArray)
            {
                string[] arr = item.Split('$');
                if (arr.Length == 3)
                {
                    Guid activityId = new Guid(arr[0]);
                    string actorName = arr[1];
                    string proxyName = (string.IsNullOrEmpty(arr[2]) ? null : arr[2]); // 委托人.

                    if (dict.ContainsKey(activityId))
                    {
                        dict[activityId][actorName] = proxyName;
                    }
                    else
                    {
                        IDictionary<string, string> names = new Dictionary<string, string>();
                        names.Add(actorName, proxyName);
                        dict.Add(activityId, names);
                    }
                }
            }
        }

        int count = dict.Count;
        if (count== 0)
        {
            //ShowError("请选择处理步骤及处理人");
            OnShowError("请为处理步骤选择相应的处理人", 1);
        }
        else
        {
            if (count == 1 && dict.ContainsKey(endActivityId))
                return dict;

            foreach (Guid key in dict.Keys)
            {
                if (dict[key].Count == 0)
                {
                    // ShowError("请为处理步骤选择相应的处理人");
                    OnShowError("请为处理步骤选择相应的处理人", 1);
                }
            }
        }

        return dict;
    }

    /// <summary>
    /// 获取抄送人数据值.
    /// </summary>
    /// <returns></returns>
    public string GetReviewActorValue()
    {
        if (this.Visible == false)
            return string.Empty;
        string values = Request.Form[ReviewSelectorHelper.ReviewAcotrs_ControlID];
        return (values == null ? string.Empty : values);
    }

    /// <summary>
    /// 显示错误消息.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="historyIndex"></param>
    public void OnShowError(string message, int historyIndex)
    {
        Botwave.Web.MessageHelper.MessageContent = message;
        string url = Botwave.Web.MessageHelper.MessagePage_Error;
        HttpContext context = HttpContext.Current;
        string js = string.Format("window.history.go(-{0});", historyIndex);
        if (!string.IsNullOrEmpty(js))
        {
            url += "?js=" + context.Server.UrlEncode(js);
        }
        context.Response.Redirect(url);
    }
    #endregion

    #region Render activities selector

    /// <summary>
    /// 生成下行步骤.
    /// </summary>
    /// <param name="currentWorkflow">当前流程实例对象.</param>
    /// <param name="currentActivity">当前流程活动定义.</param>
    /// <param name="activityInstanceId">流程还多实例编号.</param>
    /// <param name="selectorType">流程选择器对象名称.</param>
    /// <param name="activities">下行流程步骤定义.</param>
    private void BuildNextActivities(WorkflowInstance currentWorkflow, ActivityDefinition currentActivity, Guid activityInstanceId, string selectorType, IList<ActivityDefinition> activities)
    {
        Guid workflowId = currentActivity.WorkflowId;
        int activityCount = activities.Count;
        this.OnlyNext = (activityCount == 1);
        this.SplitCondition = currentActivity.SplitCondition;
        this.EndActivityId = Guid.Empty;

        if (activityCount == 1 && activities[0].State == WorkflowConstants.Complete)//下一步为完成. 
        {
            Guid activityId = activities[0].ActivityId;
            EndActivityId = activityId;

            string attributes;
            string reviewHtml = OnRenderEndActivityReviewSelector(workflowId, activityId, out attributes);

            StringBuilder textBuilder = new StringBuilder();
            textBuilder.AppendLine("<div style=\"margin-left:102px\">");
            textBuilder.AppendFormat("<input type=\"radio\" id=\"activityOption_0\" name=\"activityOption\" value=\"{0}\" checked=\"checked\" {2}/><span>{1}</span>", activityId, activities[0].ActivityName, attributes);
            textBuilder.AppendLine("</div>");

            textBuilder.AppendLine(reviewHtml);

            this.ltlNextActivities.Text = textBuilder.ToString();
            return;
        }
        else
        {
            foreach (ActivityDefinition next in activities)
            {
                if (next.State == WorkflowConstants.Complete)
                {
                    EndActivityId = next.ActivityId;
                    break;
                }
            }
        }

        string htmlTemplate = null;

        if (workflowSelectorFactory != null)
        {
            IWorkflowSelectorProfile selectorProfile = workflowSelectorFactory.GetProfile(selectorType);
            if (selectorProfile == null)
                selectorProfile = Spring.Context.Support.WebApplicationContext.Current["defaultWorkflowSelectorProfile"] as IWorkflowSelectorProfile;
            if (selectorProfile != null)
            {
                WorkflowSelectorContext selectorContext = new WorkflowSelectorContext(currentActivity);
                selectorContext.WorkflowInstanceId = WorkflowInstanceId;
                selectorContext.WorkflowInstance = currentWorkflow;
                selectorContext.ActivityInstanceId = activityInstanceId;
                selectorContext.NextActivities = activities;
                selectorContext.Actor = CurrentUser.UserName;

               
                ActivityInstance currentActivityInstance = activityService.GetActivity(activityInstanceId);
                //获取当前步骤的上一步
                if (currentActivityInstance != null)
                {
                    IList<CZActivityInstance> prevActivityInstances = CZActivityInstance.GetPrevActivitiesByPrevSetId(currentActivityInstance.PrevSetId);

                    List<Guid> activityids = new List<Guid>();
                    foreach (ActivityDefinition ad in selectorContext.NextActivities)
                    {
                        activityids.Add(ad.ActivityId);
                    }
                    foreach (CZActivityInstance item in prevActivityInstances)
                    {
                        CZActivityDefinition cZActivityDefinition = CZActivityDefinition.GetWorkflowActivityByActivityId(item.ActivityId);
                        if ((ActivityCommands.Reject.Equals(item.Command) || ActivityCommands.ReturnToDraft.Equals(item.Command)) && cZActivityDefinition.ReturnToPrev && !activityids.Contains(item.ActivityId))
                        {
                            ActivityDefinition prevActivity = activityDefinitionService.GetActivityDefinition(item.ActivityId);
                            prevActivity.ExtendAllocatorArgs = string.Empty;
                            prevActivity.ExtendAllocators = string.Empty;
                            prevActivity.AllocatorUsers = item.Actor;//设为上行步骤处理人
                            selectorContext.NextActivities.Add(prevActivity);
                        }
                    }
                }
                htmlTemplate = selectorProfile.BuildActivitySelectorHtml(this.Context, selectorContext);
            }
        }
        if (string.IsNullOrEmpty(htmlTemplate))
            htmlTemplate = "<div style=\"margin:10px 10px 10px 102px;color:red;  font-size:15px\">未找到流程下行步骤，请联系管理员。</div>";
        this.ltlNextActivities.Text = htmlTemplate;
    }

    private string OnRenderEndActivityReviewSelector(Guid workflowId, Guid activityId, out string attributes)
    {
        attributes = string.Empty;
        ReviewType reviewType = ReviewSelectorHelper.GeteviewType(workflowId);
        if (reviewType == ReviewType.Classic)
        {
            return ReviewSelectorHelper.BuildClassicHtml();
        }
        else if (reviewType == ReviewType.CheckBox)
        {
            ActivityProfile activityProfile = ActivityProfile.GetProfile(workflowId, activityId);
            if (activityProfile != null)
            {
                attributes = string.Format(" isReview=\"{0}\" reviewActorCount=\"{1}\" reviewAllChecked=\"{2}\"", activityProfile.IsReview, activityProfile.ReviewActorCount, activityProfile.ReviewValidateType); ;
            }
            string text = ReviewSelectorHelper.BuildProfileItemHtml(activityProfile,this.WorkflowInstanceId,CurrentUserName);
            return (string.IsNullOrEmpty(text) ? string.Empty : string.Format("<div style=\"padding-left:120px\">{0}</div>", text));
        }
        return string.Empty;
    }

    #endregion
}
