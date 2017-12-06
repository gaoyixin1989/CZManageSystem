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
using Botwave.XQP.Web.Controls;
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;

public partial class contrib_workflow_controls_ReviewSelector : Botwave.Security.Web.UserControlBase// System.Web.UI.UserControl
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_workflow_controls_ReviewSelector));
    private IReviewSelectorProfile reviewSelectorProfile = null;

    #region properties

    /// <summary>
    /// 流程实例服务.
    /// </summary>
    protected IWorkflowService workflowService = (IWorkflowService)Ctx.GetObject("workflowService");
    /// <summary>
    /// 流程活动实例服务.
    /// </summary>
    protected IActivityService activityService = (IActivityService)Ctx.GetObject("activityService");
    /// <summary>
    /// 用户服务.
    /// </summary>
    protected IWorkflowUserService workflowUserService = (IWorkflowUserService)Ctx.GetObject("workflowUserService");

    /// <summary>
    /// 流程实例服务.
    /// </summary>
    public IWorkflowService WorkflowService
    {
        set { workflowService = value; }
    }

    /// <summary>
    /// 流程活动实例服务.
    /// </summary>
    public IActivityService ActivityService
    {
        set { activityService = value; }
    }

    /// <summary>
    /// 用户服务.
    /// </summary>
    public IWorkflowUserService WorkflowUserService
    {
        set { workflowUserService = value; }
    }

    /// <summary>
    /// 是否可以使用抄送.
    /// </summary>
    public bool EnableReview
    {
        get { return (ViewState["EnableReview"] == null ? false : (bool)ViewState["EnableReview"]); }
        set { ViewState["EnableReview"] = value; }
    }
    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    /// <summary>
    /// 初始化.
    /// </summary>
    /// <param name="workflowProfile"></param>
    /// <param name="isStart">是否属于发单.</param>
    public void Initialize(WorkflowProfile workflowProfile, Guid workflowId, Guid? activityId)
    {
        if (workflowProfile == null || workflowProfile.IsReview == false)
        {
            this.Visible = false;
            return;
        }

        bool isClassicType = (workflowProfile == null || workflowProfile.IsClassicReviewType);
        string serviceName = (isClassicType ? "defaultReviewSelectorProfile" : "checkReviewSelectorProfile");
        reviewSelectorProfile = Spring.Context.Support.WebApplicationContext.Current[serviceName] as IReviewSelectorProfile;
        if (reviewSelectorProfile == null)
            reviewSelectorProfile = new Botwave.XQP.Service.Support.DefaultReviewSelectorProfile();
        if (reviewSelectorProfile != null)
        {
            ReviewSelectorContext selectorContext = new ReviewSelectorContext();

            if (isClassicType == false)
            {
                selectorContext.NextProfiles = ((activityId == null || activityId.Value == Guid.Empty) ? ActivityProfile.GetStartProfiles(workflowId) : ActivityProfile.GetNextProfiles(workflowId, activityId.Value));
            }
            string text = reviewSelectorProfile.BuildSelectorHtml(this.Context, selectorContext);
            if (!string.IsNullOrEmpty(text))
            {
                this.ltlHtml.Text = text;
                this.EnableReview = true;
            }
            else
            {
                this.Visible = false;
            }
        }
    }

    public IList<string> GetValues()
    {
        return new List<string>();
    }

    public bool PendingReview(WorkflowProfile workflowProfile, Guid activityInstanceId, Guid workflowId, string workflowTitle, Botwave.Entities.BasicUser sender)
    {
        if (this.Visible == false || this.EnableReview == false)
            return false;
        return false;
    }

    public bool PendingReview(WorkflowProfile workflowProfile, ActivityExecutionContext context, Guid workflowId, string workflowTitle, Botwave.Entities.BasicUser sender)
    {
        if (this.Visible == false || this.EnableReview == false)
            return false;
        return false;
    }
}
