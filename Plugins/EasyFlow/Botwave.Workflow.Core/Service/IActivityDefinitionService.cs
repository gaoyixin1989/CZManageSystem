using System;
using System.Collections.Generic;
using Botwave.Commons;
using Botwave.Entities;
using Botwave.Workflow.Domain;
using System.Data;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// ��������ӿ�.
    /// </summary>
    public interface IActivityDefinitionService
    {
        /// <summary>
        /// ��ȡ��ʼ���Ļ����.
        /// </summary>
        /// <param name="workflowId">���̱�ʶ.</param>
        /// <returns></returns>
        ActivityDefinition GetInitailActivityDefinition(Guid workflowId);

        /// <summary>
        /// ��ȡ���в���
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetInitailActivityDefinitionList(Guid workflowId, string activityName);

        /// <summary>
        ///  ��ȡ��ʼ���Ļ�����б�
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        DataTable GetInitailActivityDefinitionList(Guid workflowInstanceId, Guid activityId);

        /// <summary>
        /// ��ȡָ�����̶�����ʼ��ѡ�������б�.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetStartActivities(Guid workflowId);

        /// <summary>
        /// ��ȡָ������ʵ����ʼ��ѡ�������б�.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetStartActivitiesByWorkflowInstanceId(Guid workflowInstanceId);

        /// <summary>
        /// ��ȡָ�����ʶ�Ļ����.
        /// </summary>
        /// <param name="activityId">���ʶ.</param>
        /// <returns></returns>
        ActivityDefinition GetActivityDefinition(Guid activityId);

        /// <summary>
        /// ���ݻʵ����ȡָ���Ļ����.
        /// </summary>
        /// <param name="activityInstanceId">�ʵ����ʶ.</param>
        /// <returns></returns>
        ActivityDefinition GetActivityDefinitionByInstanceId(Guid activityInstanceId);

        /// <summary>
        /// ��ȡָ��������ʶ����һ������б�.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetPrevActivityDefinitions(Guid activityId);

        /// <summary>
        /// ��ȡ�������һ��������б�.
        /// </summary>
        /// <param name="activityInstaceId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetPrevActivityDefinitionsByInstanceId(Guid activityInstaceId);

        /// <summary>
        /// ��ȡ�������һ��������б�.
        /// </summary>
        /// <param name="activityInstaceId">�ʵ����ʶ.</param>
        /// <returns></returns>
        IList<ActivityDefinition> GetNextActivityDefinitionsByInstanceId(Guid activityInstaceId);

        /// <summary>
        /// �������̼��������ƻ�ȡ��һ���.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="activityNames"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetActivityDefinitionsByActivityNames(Guid workflowId, string[] activityNames);

        /// <summary>
        /// �������̣ɣĻ�ȡ���л��Ϣ.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetActivitiesByWorkflowId(Guid workflowId);

        /// <summary>
        /// �������̣ɣĺ͵�ǰ� ID ��ȡǰ�沽�貿�ֻ��Ϣ.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="currentActivityId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetPartActivities(Guid workflowId, Guid currentActivityId);

        /// <summary>
        /// ��������ID ��ȡƽ�Ѿ��������л��Ϣ.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetSortedActivitiesByWorkflowId(Guid workflowId);

        /// <summary>
        /// ��ȡ�����ȫ������.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<ActivityDefinition> GetAllActivityDefinition(Guid activityInstanceId);

        /// <summary>
        /// ��ָ�����̻�ȡָ����ʼ���������·���б�.
        /// </summary>
        /// <param name="workflowId">���̱��.</param>
        /// <param name="startActivityId">��ʼ������.</param>
        /// <returns></returns>
        IList<WorkflowRoute> GetWorkflowRoute(Guid workflowId, Guid startActivityId);

        /// <summary>
        /// ��ȡָ�����̶�������̻(����)��.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        int GetActivityCountByWorkflowId(Guid workflowId);

        /// <summary>
        /// �������̻������������.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        int UpdateActivityAllocators(ActivityDefinition activity);
    }
}
