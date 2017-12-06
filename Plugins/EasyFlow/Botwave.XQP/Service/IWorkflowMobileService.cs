using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Botwave.XQP.Service
{
    public interface IWorkflowMobileService
    {
        /// <summary>
        /// 流程是否允许手机审批
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        bool IsWorkflowMobile(Guid workflowId);

        /// <summary>
        /// 步骤是否允许手机审批
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        bool IsAvtivityMobile(Guid activityid);

        /// <summary>
        /// 获取转交记录
        /// </summary>
        /// <param name="actor">转交人</param>
        /// <param name="workflowName">流程名称</param>
        /// <param name="keywords">关键字</param>
        /// <param name="beginTime">转交开始时间</param>
        /// <param name="endTime">转交结束时间</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetAssignmentTasks(string actor, string workflowName, string keywords, string beginTime, string endTime, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// 获取已完成任务
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="userName"></param>
        /// <param name="keywords"></param>
        /// <param name="sheetID"></param>
        /// <param name="creater">工单创建人</param>
        /// <param name="startDT">处理开始时间</param>
        /// <param name="endDT">处理开始结束时间</param>
        /// <param name="isOnlyStart"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetDoneTaskPager(string workflowName, string userName, string keywords, string sheetID, string creater, string startDT, string endDT, bool isOnlyStart, string orderBy, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// 获取我的待办事宜列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="workflowName"></param>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetTaskListByUserName(string userName, string workflowName, string keywords, int pageIndex, int pageSize, ref int recordCount);
    }
}
