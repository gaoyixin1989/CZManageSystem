using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow
{
    /// <summary>
    /// 流程引擎接口.
    /// </summary>
    public interface IWorkflowEngine
    {
        /// <summary>
        /// 启动流程.
        /// 返回下一活动的Id.
        /// </summary>
        /// <param name="instance">流程实例信息.</param>
        Guid StartWorkflow(WorkflowInstance instance);

        /// <summary>
        /// 执行活动.
        /// </summary>
        /// <param name="context">流程活动（步骤）执行上下文.</param>
        void ExecuteActivity(ActivityExecutionContext context);

        /// <summary>
        /// 完成流程.
        /// </summary>
        /// <param name="context">流程活动（步骤）执行上下文.</param>
        void CompleteWorkflow(ActivityExecutionContext context);

        /// <summary>
        /// 取消流程.
        /// </summary>
        /// <param name="context">流程活动（步骤）执行上下文.</param>
        void CancelWorkflow(ActivityExecutionContext context);

        /// <summary>
        /// 删除流程.
        /// </summary>
        /// <param name="context">流程活动（步骤）执行上下文.</param>
        void RemoveWorkflow(ActivityExecutionContext context);
    }
}