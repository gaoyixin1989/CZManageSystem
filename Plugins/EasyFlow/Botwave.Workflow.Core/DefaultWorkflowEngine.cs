using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;

namespace Botwave.Workflow
{
    /// <summary>
    /// Ĭ��������������ʵ��.
    /// </summary>
    public class DefaultWorkflowEngine : IWorkflowEngine
    {
        private IActivityExecutionService activityExecutionService;

        /// <summary>
        /// ���̻(����)ִ�з���.
        /// </summary>
        public IActivityExecutionService ActivityExecutionService
        {
            set { activityExecutionService = value; }
        }

        #region IWorkflowEngine Members

        /// <summary>
        /// ��������.
        /// </summary>
        /// <param name="instance">����ʵ������.</param>
        /// <returns></returns>
        public Guid StartWorkflow(WorkflowInstance instance)
        {
            return activityExecutionService.StartWorkflow(instance);
        }

        /// <summary>
        /// ִ�����̻(����).
        /// </summary>
        /// <param name="context">���̻ִ�������Ķ���.</param>
        public void ExecuteActivity(ActivityExecutionContext context)
        {
            activityExecutionService.Execute(context);
        }

        /// <summary>
        /// �������.
        /// </summary>
        /// <param name="context">���̻ִ�������Ķ���.</param>
        public void CompleteWorkflow(ActivityExecutionContext context)
        {
            activityExecutionService.CompleteWorkflow(context);
        }

        /// <summary>
        /// ȡ������.
        /// </summary>
        /// <param name="context">���̻ִ�������Ķ���.</param>
        public void CancelWorkflow(ActivityExecutionContext context)
        {
            activityExecutionService.CancelWorkflow(context);
        }

        /// <summary>
        /// �Ƴ�(ɾ��)����.
        /// </summary>
        /// <param name="context">���̻ִ�������Ķ���.</param>
        public void RemoveWorkflow(ActivityExecutionContext context)
        {
            activityExecutionService.RemoveWorkflow(context);
        }

        #endregion
    }
}
