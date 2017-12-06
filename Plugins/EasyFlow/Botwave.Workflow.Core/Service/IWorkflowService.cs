using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// ����ʵ������ӿ�.
    /// </summary>
    public interface IWorkflowService
    {   
        /// <summary>
        /// ��ȡ����ʵ���Ĺ���������.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        WorkflowInstance GetWorkflowInstance(Guid workflowInstanceId);

        /// <summary>
        /// ��ȡ����ʵ���Ĺ���������.
        /// </summary>
        /// <returns></returns>
        IList<WorkflowInstance> GetWorkflowInstance();

        /// <summary>
        /// ��������ID��ȡ����ʵ���Ĺ���������.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        IList<WorkflowInstance> GetWorkflowInstanceByWorkflowId(Guid workflowId);

        /// <summary>
        /// ���ݻʵ����ȡ����ʵ���Ĺ���������.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        WorkflowInstance GetWorkflowInstanceByActivityInstanceId(Guid activityInstanceId);

        /// <summary>
        /// ��������ʵ��.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        void InsertWorkflowInstance(WorkflowInstance instance);

        /// <summary>
        /// ��������ʵ��.
        ///     ֻ���¹������������.
        /// </summary>
        /// <param name="workflowInstance"></param>
        void UpdateWorkflowInstance(WorkflowInstance workflowInstance);

        /// <summary>
        /// �����Ѿ����ڵ�����.
        ///   ������һ���Id.
        /// </summary>
        /// <param name="workflowInstance"></param>
        void UpdateWorkflowInstanceForStart(WorkflowInstance workflowInstance);

        /// <summary>
        /// ��ȡָ�����̱���ǰ׺�������б�.
        /// </summary>
        /// <param name="prefixTitle"></param>
        /// <returns></returns>
        IList<WorkflowInstance> GetWorkflowInstances(string prefixTitle);

        /// <summary>
        /// ��ȡָ���û��Ĳݸ������б�.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        DataTable GetWorkflowInstanceByDraft(string userName);

        /// <summary>
        /// ��ȡָ���û��Ĳݸ������б�.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="topCount">����ǰ������¼.</param>
        /// <returns></returns>
        DataTable GetWorkflowInstanceByDraft(string userName, int topCount);

        /// <summary>
        /// ɾ��ָ������ʵ����ŵ�����ʵ�����������̲ݸ��书�ܣ�.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        int DeleteWorkflowInstance(Guid workflowInstanceId);
    }
}
