using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Practices.CZMCC.Service.Impl
{
    public class StateDealWithHandler : IPostActivityExecutionHandler
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(StateDealWithHandler));

        private string activity_Enrollment = "ITC管理员登记调拨";// 步骤：登记调拨.
        private string activity_Check = "ITC管理员检查登记"; // 步骤：检查登记.
        private string activity_Confirm = "需求人确认"; // 步骤：需求人确认.

        private IActivityService activityService;
        private IWorkflowService workflowService;
        private IWorkflowDefinitionService workflowDefinitionService;
        private IActivityDefinitionService activityDefinitionService;

        public IActivityService ActivityService
        {
            set { activityService = value; }
        }
        public IWorkflowService WorkflowService
        {
            set { workflowService = value; }
        }
        public IWorkflowDefinitionService WorkflowDefinitionService
        {
            set { workflowDefinitionService = value; }
        }

        public IActivityDefinitionService ActivityDefinitionService
        {
            set { activityDefinitionService = value; }
        }

        private ActivityDefinition activityDefinition;

        private IPostActivityExecutionHandler next = null;
        public IPostActivityExecutionHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        public void Execute(ActivityExecutionContext context)
        {
            log.Info("######### StateDealWithHandler IS Begin ###########");

            ResourcesExecutionService re = new ResourcesExecutionService();
            Guid activityInstanceId = context.ActivityInstanceId;

            ActivityInstance activityInstance = activityService.GetActivity(activityInstanceId);
            WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);
            Guid workflowInstanceId = workflowInstance.WorkflowInstanceId;

            WorkflowDefinition wd = workflowDefinitionService.GetWorkflowDefinition(workflowInstance.WorkflowId);
            activityDefinition = activityDefinitionService.GetActivityDefinition(activityInstance.ActivityId);

            string cId = context.Variables["F7"].ToString();
            string eId = context.Variables["F8"].ToString();

            string activityName = activityDefinition.ActivityName;

            if (!string.IsNullOrEmpty(cId))
            {
                Guid id = new Guid(cId);
                this.HandleResource(re, id, workflowInstanceId, activityInstanceId, activityName);
            }

            if (!string.IsNullOrEmpty(eId))
            {
                Guid id = new Guid(eId);
                this.HandleResource(re, id, workflowInstanceId, activityInstanceId, activityName);
            }
        }

        /// <summary>
        /// 处理资源.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityName"></param>
        protected virtual void HandleResource(ResourcesExecutionService service, Guid id, Guid workflowInstanceId, Guid activityInstanceId, string activityName)
        {
            // 登记调拨.
            if (activityName.Equals(this.activity_Enrollment, StringComparison.OrdinalIgnoreCase))
            {
                service.UpdateResourcesInfo(id, 1);
                service.SaveBorrowInfo(id, workflowInstanceId, activityInstanceId);
                log.Info("#########借用资源成功###########");
            }
            // 检查登记.
            if (activityName.Equals(this.activity_Check, StringComparison.OrdinalIgnoreCase))
            {
                service.UpdateResourcesInfo(id, 0);
                service.UpdateBorrowInfo(id, workflowInstanceId, 1);
                log.Info("#########退还资源成功###########");
            }
            // 需求人确认.
            if (activityName.Equals(this.activity_Confirm, StringComparison.OrdinalIgnoreCase))
            {
                //service.SaveBorrowInfo(id, activityInstanceId);
                service.UpdateBorrowInfo(id, workflowInstanceId, 1);// 更新借用记录. 2009-006-23 以后更新包，请将此行代码注释.
                log.Info("#########保存借用记录成功###########");
            }
        }
    }
}
