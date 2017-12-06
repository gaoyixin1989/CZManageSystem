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

namespace Botwave.XQP.Service.Plugins
{
    /// <summary>
    /// 流程执行后续处理的消息通知处理器.
    /// </summary>
    public class PostActivityExecutionMessageHandler : IPostActivityExecutionHandler
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(PostActivityExecutionMessageHandler));

        #region properties

        private IWorkflowService workflowService;
        private IWorkflowSettingService workflowSettingService;
        private IWorkflowNotifyService workflowNotifyService;
        private IWorkflowUserService workflowUserService;

        public IWorkflowService WorkflowService
        {
            set { workflowService = value; }
        }

        public IWorkflowSettingService WorkflowSettingService
        {
            set { workflowSettingService = value; }
        }

        public IWorkflowNotifyService WorkflowNotifyService
        {
            set { workflowNotifyService = value; }
        }

        public IWorkflowUserService WorkflowUserService
        {
            set { workflowUserService = value; }
        }
        #endregion

        #region IPostActivityExecutionHandler 成员

        private IPostActivityExecutionHandler _next;

        public IPostActivityExecutionHandler Next
        {
            get { return _next; }
            set { _next = value; }
        }

        #endregion

        #region IActivityExecutionHandler 成员

        public virtual void Execute(ActivityExecutionContext context)
        {
            log.Info("Message Handler:" + context.ToString());

            Guid activityInstanceId = context.ActivityInstanceId;
            string actor = context.Actor;
            string command = context.Command.ToLower();
            
            // 获取下行处理人.
            IList<NotifyActor> notifyActors = workflowNotifyService.GetNextNotifyActors(activityInstanceId);
            if (notifyActors == null && notifyActors.Count == 0)
            {
                log.Info("未找到下行处理人." + activityInstanceId.ToString());
                return;
            }

            // 流程信息.
            WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);
            WorkflowSetting setting = workflowSettingService.GetWorkflowSetting(workflowInstance.WorkflowId);
            if (setting == null)
                setting = WorkflowSetting.Default;

            // 发送人信息.
            ActorDetail sender = workflowUserService.GetActorDetail(actor);

            // 操作类型.
            int operateType = TodoInfo.OpDefault;
            if (command.Equals(ActivityCommands.Reject, StringComparison.OrdinalIgnoreCase) || command.Equals(ActivityCommands.ReturnToDraft, StringComparison.OrdinalIgnoreCase))
                operateType = TodoInfo.OpBack;

            workflowNotifyService.SendMessage(sender, operateType, activityInstanceId, setting, workflowInstance, notifyActors);
        }

        #endregion

        protected virtual void SendMessage()
        { 
            
        }
    }
}
