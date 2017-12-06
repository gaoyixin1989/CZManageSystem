using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 流程创建/发起控制器.
    /// </summary>
    public interface IWorkflowInstanceCreationController
    {
        /// <summary>
        /// 是否可以创建工单.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="actor"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        bool CanCreate(Guid workflowId, string actor, object args);

        /// <summary>
        /// 是否可以创建工单.
        /// 当 urgency 值为 0 时，表示为普通单。大于等于 0 时，表示紧急单.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="actor"></param>
        /// <param name="urgency">紧急程度值。当 urgency 值为 0 时，表示为普通单。大于等于 0 时，表示紧急单.</param>
        /// <param name="args"></param>
        /// <returns></returns>
        bool CanCreate(Guid workflowId, string actor, int urgency, object args);
    }
}