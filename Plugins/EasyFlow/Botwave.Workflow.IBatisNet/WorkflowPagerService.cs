using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Botwave.Commons;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.IBatisNet
{
    public class WorkflowPagerService : IWorkflowPagerService
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(IBatisWorkflowPagerService));

        #region IWorkflowPagerService ³ÉÔ±

        public DataTable GetDoneTaskPager(string workflowName, string userName, string keywords, string startDT, string endDT, bool isOnlyStart, int pageIndex, int pageSize, ref int recordCount)
        {
            DateTime startTime = DateTimeUtils.MinValue;
            if (!string.IsNullOrEmpty(startDT))
                startTime = Convert.ToDateTime(startDT);
            DateTime endTime = DateTimeUtils.MaxValue;


            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("WorkflowName", SqlDbType.NVarChar, 200),
                new SqlParameter("Actor", SqlDbType.NVarChar, 50),
                new SqlParameter("Keywords", SqlDbType.NVarChar, 200),
                new SqlParameter("StartTime", SqlDbType.DateTime),
                new SqlParameter("EndTime", SqlDbType.DateTime),
                new SqlParameter("IsStart", SqlDbType.Bit),
                new SqlParameter("PageIndex", SqlDbType.Int),
                new SqlParameter("PageSize", SqlDbType.Int),
                new SqlParameter("RecordCount", SqlDbType.Int)
            };
            parameters[0].Value = workflowName;
            parameters[1].Value = userName;
            parameters[2].Value = keywords;
            parameters[3].Value = startTime;
            parameters[4].Value = endTime;
            parameters[5].Value = isOnlyStart;
            parameters[6].Value = pageIndex;
            parameters[7].Value = pageSize;
            parameters[8].Direction = ParameterDirection.ReturnValue;

            DataTable results = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "bwwf_ext_GetDoneTasks", parameters).Tables[0];
            recordCount = (int)parameters[8].Value;

            return results;
        }

        public DataTable GetDoneTaskPager(string workflowName, string userName, string keywords,string sheetID,string creater, string startDT, string endDT, bool isOnlyStart,string orderBy, int pageIndex, int pageSize, ref int recordCount)
        {
            DateTime startTime = DateTimeUtils.MinValue;
            if (!string.IsNullOrEmpty(startDT))
                startTime = Convert.ToDateTime(startDT);
            DateTime endTime = DateTimeUtils.MaxValue;
            if (string.IsNullOrEmpty(orderBy))
                orderBy = null;
            else
                orderBy = " order by " + orderBy;

            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("WorkflowName", SqlDbType.NVarChar, 200),
                new SqlParameter("Actor", SqlDbType.NVarChar, 50),
                new SqlParameter("Keywords", SqlDbType.NVarChar, 200),
                new SqlParameter("SheetID", SqlDbType.NVarChar, 200),
                new SqlParameter("Creater", SqlDbType.NVarChar, 200),
                new SqlParameter("StartTime", SqlDbType.DateTime),
                new SqlParameter("EndTime", SqlDbType.DateTime),
                new SqlParameter("IsStart", SqlDbType.Bit),
                new SqlParameter("OrderBy", SqlDbType.NVarChar,200),
                new SqlParameter("PageIndex", SqlDbType.Int),
                new SqlParameter("PageSize", SqlDbType.Int),
                new SqlParameter("RecordCount", SqlDbType.Int)
            };
            parameters[0].Value = workflowName;
            parameters[1].Value = userName;
            parameters[2].Value = keywords;
            parameters[3].Value = sheetID;
            parameters[4].Value = creater;
            parameters[5].Value = startTime;
            parameters[6].Value = endTime;
            parameters[7].Value = isOnlyStart;
            parameters[8].Value = orderBy;
            parameters[9].Value = pageIndex;
            parameters[10].Value = pageSize;
            parameters[11].Direction = ParameterDirection.ReturnValue;

            DataTable results = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "bwwf_ext_GetDoneTasks2", parameters).Tables[0];
            recordCount = (int)parameters[11].Value;

            return results;
        }

        public DataTable GetTimeStatPager(Guid workflowId, string keywords, string startDT, string endDT, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwwf_Tracking_Workflows_TimeStat";
            string fieldKey = "StatName";
            string fieldShow = "StatName, WorkflowInstanceId, Creator, StartedTime, FinishedTime";
            string fieldOrder = "StartedTime desc";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("1=1");
            if (!string.IsNullOrEmpty(keywords))
            {
                where.AppendFormat(" and ((StatName like '%{0}%')", keywords);
                where.AppendFormat(" or (Creator like '%{0}%'))", keywords);
            }
            if (!workflowId.Equals(Guid.Empty))
                where.AppendFormat(" and WorkflowId='{0}'", workflowId);
            if (!string.IsNullOrEmpty(startDT))
                where.AppendFormat(" and (convert(varchar(10),StartedTime,21) >= '{0}')", startDT);
            if (!string.IsNullOrEmpty(endDT))
                where.AppendFormat(" and (convert(varchar(10),StartedTime,21) <= '{0}')", endDT);

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }

        public DataTable GetTaskStatPager(string keywords, string startDT, string endDT, int pageIndex, int pageSize, ref int recordCount, int pType)
        {
            string tableName = "vw_bwwf_Tracking_Workflows_TaskStat";
            string fieldKey = "StatName";
            string fieldShow = "StatName, StartedTime, FinishedTime, Time, StatInstance";
            string fieldOrder = "StartedTime desc";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("SType={0}", pType);
            if (!string.IsNullOrEmpty(keywords))
            {
                where.AppendFormat(" and ((StatName like '%{0}%'))", keywords);
            }
            if (!string.IsNullOrEmpty(startDT))
                where.AppendFormat(" and (convert(varchar(10),StartedTime,21) >= '{0}')", startDT);
            if (!string.IsNullOrEmpty(endDT))
                where.AppendFormat(" and (convert(varchar(10),StartedTime,21) <= '{0}')", endDT);

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }
       
        #endregion
    }
}
