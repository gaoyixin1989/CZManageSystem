using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程报表服务接口.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// 获取任务统计列表.
        /// </summary>
        /// <returns></returns>
        IList<Report> GetWorkflowEfficiency();

        /// <summary>
        /// 获取历史流程时间统计列表.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        IList<Report> GetActivityEfficiency(string workflowName);

        /// <summary>
        /// 获取步骤工单数统计.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <returns></returns>
        IList<Report> GetActivityStat(string workflowName, string startDT, string endDT);

        /// <summary>
        /// 获取超时流程统计
        /// </summary>
        /// <returns></returns>
        IList<Report> GetWorkflowsOvertimeStat();

        /// <summary>
        /// 获取超时流程工单列表
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        System.Data.DataTable GetWorkflowsOvertimeList(string workflowName, int pageIndex, int pageSize, ref int recordCount);
    }
}
