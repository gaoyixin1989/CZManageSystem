using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.GMCCServiceHelpers.CZ;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Plugins
{
    /// <summary>
    /// 完成工单的后续处理类.
    /// </summary>
    public class PostCompleteWorkflowHandler : IPostCompleteWorkflowHandler
    {
        #region properties

        private static string workflowName = "销售精英竞赛平台酬金申告流程";
        private static string feedbackMessage = "尊敬的会员，您提交的酬金申告单：#title#，已经处理完成，请确认。"; // #creator#, #title#.
        private IWorkflowService workflowService;
        private IWorkflowDefinitionService workflowDefinitionService;
        private IWorkflowUserService workflowUserService;

        public IWorkflowService WorkflowService
        {
            set { workflowService = value; }
        }

        public IWorkflowDefinitionService WorkflowDefinitionService
        {
            set { workflowDefinitionService = value; }
        }

        public IWorkflowUserService WorkflowUserService
        {
            set { workflowUserService = value; }
        }

        public string WorkflowName
        {
            set { workflowName = value; }
        }

        public string FeedbackMessage
        {
            set { feedbackMessage = value; }
        }

        #endregion

        /// <summary>
        /// 构造方法.
        /// </summary>
        public PostCompleteWorkflowHandler()
        { 
        
        }

        #region IPostCompleteWorkflowHandler 成员

        private IPostCompleteWorkflowHandler _next = null;

        public IPostCompleteWorkflowHandler Next
        {
            get { return _next; }
            set { _next = value; }
        }

        #endregion

        #region IActivityExecutionHandler 成员

        public void Execute(ActivityExecutionContext context)
        {
            Guid activityInstanceId = context.ActivityInstanceId;
            WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);

            if (workflowInstance == null || workflowInstance.State != WorkflowConstants.Complete)
                return;

            WorkflowDefinition workflowDefinition = workflowDefinitionService.GetWorkflowDefinition(workflowInstance.WorkflowId);
            if (workflowDefinition == null)
                return;

            if (!string.IsNullOrEmpty(feedbackMessage) && workflowDefinition.WorkflowName.Equals(workflowName, StringComparison.OrdinalIgnoreCase))
            {
                // 酬金申告流程，发送反馈短信.
                string title = workflowInstance.Title;
                string creator = string.Empty;
                string message = feedbackMessage.Trim().ToLower();
                if (message.IndexOf("#creator#", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    ActorDetail detail = workflowUserService.GetActorDetail(workflowInstance.Creator);
                    if (detail != null)
                        creator = detail.RealName;
                    message = message.Replace("#creator#", creator);
                }
                message = message.Replace("#title#", title);
                Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendSMS(creator, string.Empty, message, "bwwf_Tracking_Workflows", workflowInstance.WorkflowInstanceId.ToString());
            }
        }

        #endregion
    }
}
