using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// ���̷�ҳ����.
    /// </summary>
    public interface IWorkflowPagerService
    {
        /// <summary>
        /// �Ѵ��������б�.
        /// </summary>
        /// <param name="workflowName">��ȡĳһ�����̵������б�.</param>
        /// <param name="userName"></param>
        /// <param name="keywords"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="isOnlyStart">�Ƿ�ֻ��ѯ�û�������Ѵ��������б�.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetDoneTaskPager(string workflowName, string userName, string keywords, string startDT, string endDT, bool isOnlyStart, int pageIndex, int pageSize, ref int recordCount);

        //֧��������Ѵ��������б�.
        DataTable GetDoneTaskPager(string workflowName, string userName, string keywords, string sheetID, string creater, string startDT, string endDT, bool isOnlyStart, string orderBy, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// ������ʱͳ���б�.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="keywords"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetTimeStatPager(Guid workflowId, string keywords, string startDT, string endDT, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// ������ͳ���б�.
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        DataTable GetTaskStatPager(string keywords, string startDT, string endDT, int pageIndex, int pageSize, ref int recordCount, int pType);

        ///// <summary>
        ///// �߼���ѯ����.
        ///// </summary>
        ///// <param name="workflowName">�������ơ���Ϊ Empty OR NULL ʱ�����ѯȫ��.</param>
        ///// <param name="activityName">�������ơ���Ϊ Empty OR NULL ʱ�����ѯȫ��.</param>
        ///// <param name="currentUserName">��ǰ��¼�û�����</param>
        ///// <param name="createdBeginTime">��������������Сֵ��</param>
        ///// <param name="createdEndTime">���������������ֵ��</param>
        ///// <param name="sheetId">��ˮ�����š�</param>
        ///// <param name="creator">���̷����ˡ�</param>
        ///// <param name="actor">���������ˡ�</param>
        ///// <param name="titleKeywords">����ؼ��֡�</param>
        ///// <param name="keywords">�ؼ��֡�</param>
        ///// <param name="pageIndex">ҳ��������</param>
        ///// <param name="pageSize">ҳ���С��</param>
        ///// <param name="recordCount">����������</param>
        ///// <returns></returns>
        //DataTable AdvanceSearch(string workflowName, 
        //    string activityName,
        //    string currentUserName, 
        //    string createdBeginTime, 
        //    string createdEndTime,
        //    string sheetId, 
        //    string creator, 
        //    string actor, 
        //    string titleKeywords, 
        //    string contentKeywods, 
        //    int pageIndex, 
        //    int pageSize,
        //    ref int recordCount);
    }
}
