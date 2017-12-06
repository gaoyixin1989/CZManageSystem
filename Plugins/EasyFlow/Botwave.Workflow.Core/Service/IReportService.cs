using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// ���̱������ӿ�.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// ��ȡ����ͳ���б�.
        /// </summary>
        /// <returns></returns>
        IList<Report> GetWorkflowEfficiency();

        /// <summary>
        /// ��ȡ��ʷ����ʱ��ͳ���б�.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        IList<Report> GetActivityEfficiency(string workflowName);

        /// <summary>
        /// ��ȡ���蹤����ͳ��.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <returns></returns>
        IList<Report> GetActivityStat(string workflowName, string startDT, string endDT);

        /// <summary>
        /// ��ȡ��ʱ����ͳ��
        /// </summary>
        /// <returns></returns>
        IList<Report> GetWorkflowsOvertimeStat();

        /// <summary>
        /// ��ȡ��ʱ���̹����б�
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        System.Data.DataTable GetWorkflowsOvertimeList(string workflowName, int pageIndex, int pageSize, ref int recordCount);
    }
}
