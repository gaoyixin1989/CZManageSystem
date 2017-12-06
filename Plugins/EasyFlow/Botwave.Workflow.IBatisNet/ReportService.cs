using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.IBatisNet
{
    public class ReportService : IReportService
    {
        #region IReportService Members

        public IList<Report> GetWorkflowEfficiency()
        {
            return IBatisMapper.Select<Report>("bwwf_WorkFlows_Efficiency", null);
        }

        public IList<Report> GetActivityEfficiency(string workflowName)
        {
            return IBatisMapper.Select<Report>("bwwf_Activities_Efficiency", workflowName);
        }

        public IList<Report> GetActivityStat(string workflowName, string startDT, string endDT)
        {
            Hashtable ht = new Hashtable();
            ht.Add("WorkflowName", workflowName);
            ht.Add("StartDT", startDT);
            ht.Add("EndDT", endDT);
            return IBatisMapper.Select<Report>("bwwf_Activities_Stat", ht);
        }

        public IList<Report> GetWorkflowsOvertimeStat()
        {
            return IBatisMapper.Select<Report>("bwwf_Workflows_Overtime");
        }

        public System.Data.DataTable GetWorkflowsOvertimeList(string workflowName, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwwf_Tracking_Workflows_Detail";
            string fieldKey = "WorkflowInstanceId";
            string fieldShow = "WorkflowInstanceId,工单标题,工单号,发起人,发起时间,期望完成时间";
            string fieldOrder = "发起时间 DESC";
            StringBuilder where = new StringBuilder();

            where.AppendFormat("期望完成时间 < GETDATE() AND 状态 = 1 AND WorkflowName = '{0}'", workflowName);

            System.Data.DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }

        #endregion
    }
}