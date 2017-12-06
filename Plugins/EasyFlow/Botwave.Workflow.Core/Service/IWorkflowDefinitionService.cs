using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// ���̶������ӿ�.
    /// </summary>
    public interface IWorkflowDefinitionService
    {
        /// <summary>
        /// ��ȡ���е����̶���(����δ���õ�����).
        /// </summary>
        /// <returns></returns>
        IList<WorkflowDefinition> GetAllWorkflowDefinition();

        /// <summary>
        /// ��ȡ���е����̶���(ֻ�г����õ�����).
        /// </summary>
        /// <returns></returns>
        IList<WorkflowDefinition> GetWorkflowDefinitionList();

        /// <summary>
        /// ��ȡ���е����̶���(ֻ�г����õ�����).
        /// </summary>
        /// <returns></returns>
        DataTable GetWorkflowDefinitionTable();

        /// <summary>
        /// ��ȡָ���������Ƶ����̶����б�.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        IList<WorkflowDefinition> GetWorkflowDefinitionListByName(string workflowName);

        /// <summary>
        /// ��ȡָ��Id�����̶���.
        /// </summary>
        /// <param name="workflowDefinitionId"></param>
        /// <returns></returns>
        WorkflowDefinition GetWorkflowDefinition(Guid workflowDefinitionId);

        /// <summary>
        /// ��ȡָ���������Ƶĵ�ǰ�汾(���°汾)�����̶���.
        /// </summary>
        /// <param name="workflowName">��ȡ��ǰ�汾����������.</param>
        /// <returns></returns>
        WorkflowDefinition GetCurrentWorkflowDefinition(string workflowName);

        /// <summary>
        /// ��ȡָ�����̶������������̵ĵ�ǰ�汾(���°汾)�����̶���.
        /// </summary>
        /// <param name="workflowDefinitionId">��ȡ��ǰ�汾�����̶�����.</param>
        /// <returns></returns>
        WorkflowDefinition GetCurrentWorkflowDefinition(Guid workflowDefinitionId);

        /// <summary>
        /// ����ָ����ŵ����̶����Ƿ�����.
        /// </summary>
        /// <param name="workflowDefinitionId"></param>
        /// <param name="enabled">ָ���Ƿ�����</param>
        /// <returns></returns>
        int UpdateWorkflowEnabled(Guid workflowDefinitionId, bool enabled);

        /// <summary>
        /// ��������Ƿ���ڣ����Ƿ����û����°汾��.
        /// </summary>
        /// <param name="workflowDefinitionId"></param>
        /// <returns></returns>
        bool WorkflowIsExists(Guid workflowDefinitionId);
    }
}
