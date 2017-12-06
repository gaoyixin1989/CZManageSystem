using System;
using System.Collections.Generic;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 活动执行服务接口.
    /// </summary>
    public interface IActivityExecutionService
    {
        /// <summary>
        /// 执行活动.
        /// </summary>
        /// <param name="context"></param>
        void Execute(ActivityExecutionContext context);

        /// <summary>
        /// 启动流程.
        /// 返回下一活动的id.
        /// </summary>
        /// <param name="instance"></param>
        Guid StartWorkflow(WorkflowInstance instance);
        
        /// <summary>
        /// 完成.
        /// </summary>
        /// <param name="context"></param>
        void CompleteWorkflow(ActivityExecutionContext context);

        /// <summary>
        /// 取消.
        /// </summary>
        /// <param name="context"></param>
        void CancelWorkflow(ActivityExecutionContext context);

        /// <summary>
        /// 删除流程.
        /// </summary>
        /// <param name="context"></param>
        void RemoveWorkflow(ActivityExecutionContext context);
    }
}
