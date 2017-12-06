using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Botwave.Workflow.Domain;
using Botwave.Entities;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// ����������ɷ���ӿ�.
    /// </summary>
    public interface ITaskAssignService
    {
        /// <summary>
        /// ָ�ɻ/����.
        /// </summary>
        /// <param name="assignment"></param>
        void Assign(Assignment assignment);

        /// <summary>
        /// ��ȡָ������ʵ����ת����Ϣ�б�.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        IList<Assignment> GetAssignments(Guid workflowInstanceId);

        /// <summary>
        /// ��ȡָ�����̻�����ŵ�ת����������ʵ��.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        AllocatorOption GetAssignmentAllocator(Guid activityId);

        /// <summary>
        /// �������̻���������ת������.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        int UpdateAssignmentAllocators(AllocatorOption option);

        /// <summary>
        /// ��ȡ���Ա�ָ�ɵ��û�.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<BasicUser> GetUsers4Assignment(Guid activityInstanceId);

        /// <summary>
        /// ��ȡָ�����̲���ʵ���Ŀɱ�ת�����û��б�.
        /// </summary>
        /// <param name="workflowInstnaceId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        IDictionary<string, string> GetAssignmentActors(Guid workflowInstnaceId, Guid activityInstanceId, string actor);
        
        /// <summary>
        /// ��ȡָ���û���ת�������б�.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="workflowName"></param>
        /// <param name="keywords"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetAssignmentTasks(string actor, string workflowName, string keywords,
            string beginTime, string endTime, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// ��ȡ�������̲���Ĵ�������Ϣ.
        /// </summary>
        /// <param name="activityInstanceId">���̲���ʵ�����.</param>
        /// <returns></returns>
        IList<BasicUser> GetTodoActors(Guid activityInstanceId);

        /// <summary>
        /// ��ȡָ�����̲������һ��������Ϣ�б�(���ڷ��Ͷ��Ż����ʼ�����).
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<TodoInfo> GetNextTodoInfo(Guid activityInstanceId);

        /// <summary>
        /// ����������Ϣ.
        /// </summary>
        /// <param name="item"></param>
        void InsertTodo(TodoInfo item);

        /// <summary>
        /// ɾ��ָ�����̲���ʵ����ŵĴ�����Ϣ.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        int DeleteTodo(Guid activityInstanceId);

        /// <summary>
        /// ɾ��ָ�����̲���ʵ����ŵĴ�����Ϣ.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        int DeleteTodo(Guid activityInstanceId, string userName);
        
        /// <summary>
        /// ���´���δ�����Ѷ�.
        /// </summary>
        /// <param name="activityInstanceId">���̲���ʵ�����.</param>
        /// <param name="userName">�û���.</param>
        /// <param name="isReaded">δ�������Ѷ�.true Ϊ�Ѷ���false Ϊδ��.</param>
        /// <returns></returns>
        int UpdateTodoReaded(Guid activityInstanceId, string userName, bool isReaded);

        /// <summary>
        /// ���ָ���û���ָ������Ĵ�����Ϣ�Ƿ����.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool IsExistsTodo(Guid activityInstanceId, string userName);

        /// <summary>
        /// ��ȡָ�������ָ���û��Ĵ�����Ϣ.
        ///     ����������ʵ������������Ϣ.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        TodoInfo GetTodoInfo(Guid activityInstanceId, string userName);

        /// <summary>
        /// ����Ƿ��ȫ��˾��Ա��ѡ��ת����.
        /// </summary>
        /// <param name="activityInstanceId">���̻ʵ�����.</param>
        /// <returns></returns>
        bool IsAssignFromCompany(Guid activityInstanceId);
    }
}
