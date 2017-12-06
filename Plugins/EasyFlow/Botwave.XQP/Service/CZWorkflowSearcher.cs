using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Botwave.XQP.Commons;
using System.Collections;
using Botwave.Workflow.Extension.Service;

namespace Botwave.XQP.Service
{
    public class CZWorkflowSearcher
    {
        public static DataTable GetDoingTask(Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition, int pageIndex, int pageSize, ref int recordCount)
        {
            int pageEnd = (pageIndex + 1) * pageSize;
            pageIndex = pageIndex * pageSize;
            pageIndex = pageIndex >= 0 ? pageIndex : 0;
            string finishedTime = condition.IsProcessed ? string.Format(" and finishedTime between to_date('{0} 00:00:00', 'yyyy-mm-dd hh24:mi:ss') and to_date( '{0} 23:59:59','yyyy-mm-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd")) : "";

            StringBuilder where = new StringBuilder();
            if (!string.IsNullOrEmpty(condition.WorkflowName))
                where.AppendFormat(" and workflowname like '{0}%'", condition.WorkflowName);
            if (!string.IsNullOrEmpty(condition.Keywords))
                where.AppendFormat(" and (CreatorName LIKE '%{0}%' OR Title like '%{0}%' OR WorkflowAlias LIKE '%{0}%' OR SheetId LIKE '%{0}%')", condition.Keywords);
            if (!string.IsNullOrEmpty(condition.CreatorName))
                where.AppendFormat(" and (CreatorName like '{0}%' or Creator like '%{0}%')", condition.CreatorName);
            if (!string.IsNullOrEmpty(condition.Title))
                where.AppendFormat(" and Title like '{0}%'", condition.Title);
            if (!string.IsNullOrEmpty(condition.SheetId))
                where.AppendFormat(" and SheetId like '{0}%'", condition.SheetId);

            Hashtable ht = new Hashtable();
            ht.Add("Actor", condition.ProcessorName);
            ht.Add("BeginTime", condition.BeginTime);
            ht.Add("EndTime", condition.EndTime);
            DataTable dt = APIServiceSQLHelper.QueryForDataSet("cz_WorkflowExtension_DoingTasks_Select", ht);

            DataRow[] dw = dt.Select("1=1 " + where, "StartedTime DESC");

            recordCount = dw.Length;

            DataTable temp = dt.Clone();

            if (pageEnd > dw.Length)
            { pageEnd = dw.Length; }
            for (int i = pageIndex; i <= pageEnd - 1; i++)
            {
                //DataRow newdr = temp.NewRow();
                DataRow dr = dw[i];
                temp.ImportRow(dr);
            }
            return temp;
        }

        /// <summary>
        /// 获取已办事宜，Wap版。
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static DataTable GetMpDoingTask(Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition, int pageIndex, int pageSize, ref int recordCount)
        {
            int pageEnd = (pageIndex + 1) * pageSize;
            pageIndex = pageIndex * pageSize;
            pageIndex = pageIndex >= 0 ? pageIndex : 0;
            string finishedTime = condition.IsProcessed ? string.Format(" and finishedTime between to_date('{0} 00:00:00', 'yyyy-mm-dd hh24:mi:ss') and to_date( '{0} 23:59:59','yyyy-mm-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd")) : "";

            StringBuilder where = new StringBuilder();
            if (!string.IsNullOrEmpty(condition.WorkflowName))
                where.AppendFormat(" and workflowname like '{0}%'", condition.WorkflowName);
            if (!string.IsNullOrEmpty(condition.Keywords))
                where.AppendFormat(" and (CreatorName LIKE '%{0}%' OR Title like '%{0}%' OR WorkflowAlias LIKE '%{0}%' OR SheetId LIKE '%{0}%')", condition.Keywords);
            if (!string.IsNullOrEmpty(condition.CreatorName))
                where.AppendFormat(" and (CreatorName like '{0}%' or Creator like '%{0}%')", condition.CreatorName);
            if (!string.IsNullOrEmpty(condition.Title))
                where.AppendFormat(" and Title like '{0}%'", condition.Title);
            if (!string.IsNullOrEmpty(condition.SheetId))
                where.AppendFormat(" and SheetId like '{0}%'", condition.SheetId);

            Hashtable ht = new Hashtable();
            ht.Add("Actor", condition.ProcessorName);
            ht.Add("BeginTime", condition.BeginTime);
            ht.Add("EndTime", condition.EndTime);
            DataTable dt = APIServiceSQLHelper.QueryForDataSet("cz_WorkflowExtension_Mp_DoingTasks_Select", ht);

            DataRow[] dw = dt.Select("1=1 " + where);

            recordCount = dw.Length;

            DataTable temp = dt.Clone();

            if (pageEnd > dw.Length)
            { pageEnd = dw.Length; }
            for (int i = pageIndex; i <= pageEnd - 1; i++)
            {
                //DataRow newdr = temp.NewRow();
                DataRow dr = dw[i];
                temp.ImportRow(dr);
            }
            return temp;
        }
    }
}
