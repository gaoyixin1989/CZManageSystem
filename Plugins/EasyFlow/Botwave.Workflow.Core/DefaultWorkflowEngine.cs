using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;

namespace Botwave.Workflow
{
    /// <summary>
    /// 默认流程引擎对象的实现.
    /// </summary>
    public class DefaultWorkflowEngine : IWorkflowEngine
    {
        private IActivityExecutionService activityExecutionService;

        /// <summary>
        /// 流程活动(步骤)执行服务.
        /// </summary>
        public IActivityExecutionService ActivityExecutionService
        {
            set { activityExecutionService = value; }
        }

        #region IWorkflowEngine Members

        /// <summary>
        /// 启动流程.
        /// </summary>
        /// <param name="instance">流程实例对象.</param>
        /// <returns></returns>
        public Guid StartWorkflow(WorkflowInstance instance)
        {
            return activityExecutionService.StartWorkflow(instance);
        }

        /// <summary>
        /// 执行流程活动(步骤).
        /// </summary>
        /// <param name="context">流程活动执行上下文对象.</param>
        public void ExecuteActivity(ActivityExecutionContext context)
        {
            activityExecutionService.Execute(context);
        }

        /// <summary>
        /// 完成流程.
        /// </summary>
        /// <param name="context">流程活动执行上下文对象.</param>
        public void CompleteWorkflow(ActivityExecutionContext context)
        {
            activityExecutionService.CompleteWorkflow(context);
        }

        /// <summary>
        /// 取消流程.
        /// </summary>
        /// <param name="context">流程活动执行上下文对象.</param>
        public void CancelWorkflow(ActivityExecutionContext context)
        {
            activityExecutionService.CancelWorkflow(context);
        }

        /// <summary>
        /// 移除(删除)流程.
        /// </summary>
        /// <param name="context">流程活动执行上下文对象.</param>
        public void RemoveWorkflow(ActivityExecutionContext context)
        {
            activityExecutionService.RemoveWorkflow(context);
        }

        #endregion
    }
}
