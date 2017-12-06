using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// �ʵ������ӿ�.
    /// </summary>
    public interface IActivityService
    {
        /// <summary>
        /// ��ȡ�ʵ��.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        ActivityInstance GetActivity(Guid activityInstanceId);

        /// <summary>
        /// ����ָ������ʵ����ʶ�ͻ��ʶ�Ļʵ���б�.
        /// </summary>
        /// <param name="workflowInstanceId">����ʵ����ʶ.</param>
        /// <param name="activityId">���ʶ.</param>
        /// <returns></returns>
        IList<ActivityInstance> GetCompletedActivities(Guid workflowInstanceId, Guid activityId);

        /// <summary>
        /// ͨ���id��õ�ǰ����������Ļʵ����id.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        Guid GetCurrentActivityId(Guid activityInstanceId);

        ///// <summary>
        ///// ͨ���id��õ�ǰ����������Ļʵ����id.
        ///// </summary>
        ///// <param name="activityInstanceId"></param>
        ///// <returns></returns>
        //IList<Guid> GetCurrentActivityIds(Guid activityInstanceId);

        /// <summary>
        /// ���ݻʵ����ȡ��ͬһ����ʵ���е����лʵ��.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<ActivityInstance> GetActivitiesInSameWorkflow(Guid activityInstanceId);

        /// <summary>
        /// ��ȡָ������ʵ����������ɵ����̻�����裩ʵ���б�.
        /// (����ԭ����GetActivitiesInSameWorkflowCompleted.)
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        IList<ActivityInstance> GetWorkflowCompletedActivities(Guid workflowInstanceId);

        /// <summary>
        /// ��ȡָ������ʵ���ĵ�ǰ���̻ʵ��.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        ActivityInstance GetCurrentActivity(Guid workflowInstanceId);

        /// <summary>
        /// ��ȡָ���ʵ����ʶ�ı���ȫ���ʵ���б�.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        IList<ActivityInstance> GetCurrentActivities(Guid workflowInstanceId);

        /// <summary>
        /// ��ȡָ������ʵ�� ID �ͻ���� ID ����һ�ʵ���б�.
        /// </summary>
        /// <param name="workflowInstanceId">����ʵ�� ID.</param>
        /// <param name="activityId">����� ID.</param>
        /// <returns></returns>
        IList<ActivityInstance> GetPrevActivities(Guid workflowInstanceId, Guid activityId);

        /// <summary>
        /// ��ȡ��һ���.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<ActivityInstance> GetNextActivities(Guid activityInstanceId);

        /// <summary>
        /// ����ָ������ʵ����ʶ�����лʵ���б�.
        /// </summary>
        /// <param name="workflowInstanceId">����ʵ����ʶ.</param>
        /// <returns></returns>
        IList<ActivityInstance> GetWorkflowActivities(Guid workflowInstanceId);

        /// <summary>
        /// ��ȡָ���ⲿʵ�����ͺ��ⲿʵ���ʶ�Ļʵ���б�.
        /// </summary>
        /// <param name="entityType">�ⲿʵ������.</param>
        /// <param name="entityId">�ⲿʵ���ʶ.</param>
        /// <returns></returns>
        IList<ActivityInstance> GetActivitiesByExternalEntity(string entityType, string entityId);

        /// <summary>
        /// ��������(�Լ����̼�����ʵ��)��ȡ�����ɵĻʵ��.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        ActivityInstance GetLatestCompletedActivityByActivityName(Guid workflowId, Guid workflowInstanceId, string activityName);

        /// <summary>
        /// ���ݵ�ǰ���ȡǰ�̻�����ʵ��.
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <returns></returns>
        IList<ActivityInstance> GetCompletedActivitiesOfPrevDefinitionByCurrent(ActivityInstance activityInstance);

        /// <summary>
        /// ��ҳ��ȡ���������б�.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="workflowId"></param>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetTaskListByUserName(string userName, string workflowId, string keywords, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// ��ȡָ����Ȩ������Ȩ�����û�����Ĵ��������б�.
        /// </summary>
        /// <param name="proxyName">��Ȩ���û���.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetTaskListByProxy(string proxyName, int pageIndex, int pageSize, ref int recordCount);
    }
}
