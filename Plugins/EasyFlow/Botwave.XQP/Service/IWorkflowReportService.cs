using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Botwave.XQP.Service
{
    /// <summary>
    /// 流程报表服务接口.
    /// </summary>
    public interface IWorkflowReportService
    {
        /// <summary>
        /// 获取工单流转明细列表.
        /// </summary>
        /// <param name="owner">不为空时，只查询指定用户的数据.</param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="workflowName"></param>
        /// <param name="isByUser"></param>
        /// <returns></returns>
        DataSet GetProcessDetail(string owner, string startDT, string endDT, string workflowName, bool isByUser);

        /// <summary>
        /// 获取流程业务数据分页列表.
        /// </summary>
        /// <param name="owner">不为空时，只查询指定用户的数据.</param>
        /// <param name="workflowName"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="dpnames"></param>
        /// <param name="state"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetWorkflowBusinessPager(string owner, string workflowName, string startDT, string endDT, string dpnames, string state, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// 获取审批意见统计.
        /// </summary>
        /// <param name="owner">不为空时，只查询指定用户的数据.</param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        DataSet GetWorkflowOption(string owner, string startDT, string endDT, string workflowName);

        /// <summary>
        /// 根据流程名称，活动名称获取工单列表.
        /// </summary>
        /// <param name="owner">不为空时，只查询指定用户的数据.</param>
        /// <param name="workflowName"></param>
        /// <param name="activityName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetWorkflowTrackingPager(string owner, string workflowName, string activityName, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// 获取指定流程的步骤处理统计.
        /// </summary>
        /// <param name="owner">不为空时，只查询指定用户的数据.</param>
        /// <param name="workflowName"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <returns></returns>
        DataTable GetActivityStat(string owner, string workflowName, string startDT, string endDT);

        /// <summary>
        /// 根据条件获取所有流程实例表单数据
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="workflowName"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="dpnames"></param>
        /// <param name="state"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable LoadWorkflowFormData(string owner, string workflowName, string startDT, string endDT, string dpnames, string state, int? pageIndex, int? pageSize, ref int recordCount);
    }
}
