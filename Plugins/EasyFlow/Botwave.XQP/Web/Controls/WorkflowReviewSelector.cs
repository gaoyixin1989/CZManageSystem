using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;
using Botwave.XQP.Service.Plugins;

namespace Botwave.XQP.Web.Controls
{
    /// <summary>
    /// 流程抄送选择器控件.
    /// </summary>
    public abstract class WorkflowReviewSelector : Botwave.Security.Web.UserControlBase, IWorkflowReviewService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowReviewSelector));

        #region properties

        /// <summary>
        /// 流程实例服务.
        /// </summary>
        protected IWorkflowService workflowService;
        /// <summary>
        /// 流程活动实例服务.
        /// </summary>
        protected IActivityService activityService;
        /// <summary>
        /// 用户服务.
        /// </summary>
        protected IWorkflowUserService workflowUserService;

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

        /// <summary>
        /// 构造方法.
        /// </summary>
        public WorkflowReviewSelector()
        { }

        #region IWorkflowReviewService 成员

        public virtual bool PendingReview(WorkflowProfile workflowProfile, Guid activityInstanceId, Guid workflowId, string workflowTitle, Botwave.Entities.BasicUser sender)
        {
            return false;
        }

        public virtual bool PendingReview(WorkflowProfile workflowProfile, Botwave.Workflow.ActivityExecutionContext context, Guid workflowId, string workflowTitle, Botwave.Entities.BasicUser sender)
        {
            return PendingReview(workflowProfile, context.ActivityInstanceId, workflowId, workflowTitle, sender);
        }

        #endregion

        #region methods


        #endregion

    }
}
