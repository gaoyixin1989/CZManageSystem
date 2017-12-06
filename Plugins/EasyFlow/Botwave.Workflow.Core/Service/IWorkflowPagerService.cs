using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程分页服务.
    /// </summary>
    public interface IWorkflowPagerService
    {
        /// <summary>
        /// 已处理任务列表.
        /// </summary>
        /// <param name="workflowName">获取某一类流程的任务列表.</param>
        /// <param name="userName"></param>
        /// <param name="keywords"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="isOnlyStart">是否只查询用户发起的已处理任务列表.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetDoneTaskPager(string workflowName, string userName, string keywords, string startDT, string endDT, bool isOnlyStart, int pageIndex, int pageSize, ref int recordCount);

        //支持排序的已处理任务列表.
        DataTable GetDoneTaskPager(string workflowName, string userName, string keywords, string sheetID, string creater, string startDT, string endDT, bool isOnlyStart, string orderBy, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// 工单耗时统计列表.
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
        /// 工作量统计列表.
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
        ///// 高级查询流程.
        ///// </summary>
        ///// <param name="workflowName">流程名称。当为 Empty OR NULL 时，则查询全部.</param>
        ///// <param name="activityName">步骤名称。当为 Empty OR NULL 时，则查询全部.</param>
        ///// <param name="currentUserName">当前登录用户名。</param>
        ///// <param name="createdBeginTime">工单创建日期最小值。</param>
        ///// <param name="createdEndTime">工单创建日期最大值。</param>
        ///// <param name="sheetId">流水工单号。</param>
        ///// <param name="creator">流程发起人。</param>
        ///// <param name="actor">工单处理人。</param>
        ///// <param name="titleKeywords">标题关键字。</param>
        ///// <param name="keywords">关键字。</param>
        ///// <param name="pageIndex">页面索引。</param>
        ///// <param name="pageSize">页面大小。</param>
        ///// <param name="recordCount">数据总数。</param>
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
