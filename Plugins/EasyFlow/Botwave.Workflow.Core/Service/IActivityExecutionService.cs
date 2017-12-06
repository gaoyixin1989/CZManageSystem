using System;
using System.Collections.Generic;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// �ִ�з���ӿ�.
    /// </summary>
    public interface IActivityExecutionService
    {
        /// <summary>
        /// ִ�л.
        /// </summary>
        /// <param name="context"></param>
        void Execute(ActivityExecutionContext context);

        /// <summary>
        /// ��������.
        /// ������һ���id.
        /// </summary>
        /// <param name="instance"></param>
        Guid StartWorkflow(WorkflowInstance instance);
        
        /// <summary>
        /// ���.
        /// </summary>
        /// <param name="context"></param>
        void CompleteWorkflow(ActivityExecutionContext context);

        /// <summary>
        /// ȡ��.
        /// </summary>
        /// <param name="context"></param>
        void CancelWorkflow(ActivityExecutionContext context);

        /// <summary>
        /// ɾ������.
        /// </summary>
        /// <param name="context"></param>
        void RemoveWorkflow(ActivityExecutionContext context);
    }
}
