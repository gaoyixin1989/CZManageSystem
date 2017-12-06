using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow
{
    /// <summary>
    /// ��������ӿ�.
    /// </summary>
    public interface IWorkflowEngine
    {
        /// <summary>
        /// ��������.
        /// ������һ���Id.
        /// </summary>
        /// <param name="instance">����ʵ����Ϣ.</param>
        Guid StartWorkflow(WorkflowInstance instance);

        /// <summary>
        /// ִ�л.
        /// </summary>
        /// <param name="context">���̻�����裩ִ��������.</param>
        void ExecuteActivity(ActivityExecutionContext context);

        /// <summary>
        /// �������.
        /// </summary>
        /// <param name="context">���̻�����裩ִ��������.</param>
        void CompleteWorkflow(ActivityExecutionContext context);

        /// <summary>
        /// ȡ������.
        /// </summary>
        /// <param name="context">���̻�����裩ִ��������.</param>
        void CancelWorkflow(ActivityExecutionContext context);

        /// <summary>
        /// ɾ������.
        /// </summary>
        /// <param name="context">���̻�����裩ִ��������.</param>
        void RemoveWorkflow(ActivityExecutionContext context);
    }
}