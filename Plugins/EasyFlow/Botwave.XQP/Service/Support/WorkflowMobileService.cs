using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.Extension.IBatisNet;
using System.Data.SqlClient;
using System.Data;
using Botwave.Commons;

namespace Botwave.XQP.Service.Support
{
    public class WorkflowMobileService : IWorkflowMobileService
    {
        public bool IsWorkflowMobile(Guid workflowId)
        {
            SqlParameter[] pa = new SqlParameter[1];
            pa[0] = new SqlParameter("@workflowId", SqlDbType.UniqueIdentifier);
            pa[0].Value = workflowId;
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select isnull(ismobile,1) from bwwf_workflows wf  inner join xqp_WorkflowSettings st on wf.workflowname=st.workflowname where workflowid=@workflowId", pa);
            return Botwave.Commons.DbUtils.ToInt32(result)>0;
        }

        public bool IsAvtivityMobile(Guid activityid)
        {
            SqlParameter[] pa = new SqlParameter[1];
            pa[0] = new SqlParameter("@activityid", SqlDbType.UniqueIdentifier);
            pa[0].Value = activityid;
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select isnull(ismobile,1) from bwwf_activities where activityid=@activityid", pa);
            return Botwave.Commons.DbUtils.ToInt32(result) > 0;
        }

        public DataTable GetAssignmentTasks(string actor, string workflowName, string keywords, string beginTime, string endTime, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_mp_bwwf_Tracking_Assignments_Tasks";
            string fieldKey = "Title";
            string fieldShow = "ActivityInstanceId, AssignedUser, AssigningUser, AssignedTime,  AssignedRealName, IsCompleted, ActivityName, CreatorName, SheetId, Title, WorkflowAlias, AliasImage, CurrentActivityNames, CurrentActors";
            string fieldOrder = "AssignedTime desc";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("(AssigningUser = '{0}')", actor);
            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat("AND (WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(keywords))
            {
                keywords = DbUtils.FilterSQL(keywords);
                where.AppendFormat(" AND (Title LIKE '%{0}%')", keywords);
            }
            if (!string.IsNullOrEmpty(beginTime))
            {
                where.AppendFormat(" AND (AssignedTime >= '{0}')", beginTime);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                where.AppendFormat(" AND (AssignedTime <= '{0}')", endTime);
            }

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }

        public DataTable GetDoneTaskPager(string workflowName, string userName, string keywords, string sheetID, string creater, string startDT, string endDT, bool isOnlyStart, string orderBy, int pageIndex, int pageSize, ref int recordCount)
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

            DataTable results = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "bwwf_ext_mp_GetDoneTasks", parameters).Tables[0];
            recordCount = (int)parameters[11].Value;

            return results;
        }

        public DataTable GetTaskListByUserName(string userName, string workflowName, string keywords, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_mp_bwwf_Tracking_Todo";
            string fieldKey = "ActivityInstanceId";
            string fieldShow = @"ActivityInstanceId, UserName, State, ProxyName, OperateType, 
                          IsCompleted, CreatedTime,FinishedTime, Actor, ActivityName, Title, WorkflowAlias,WorkflowName, 
                          WorkflowInstanceId, SheetId,StartedTime, Urgency, Importance, 
                          Creator, CreatorName, AliasImage, TodoActors";
            string fieldOrder = "Urgency DESC, CreatedTime DESC";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("(IsCompleted = 0) AND (UserName = '{0}')", userName);
            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat(" AND (WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(keywords))
            {
                keywords = DbUtils.FilterSQL(keywords);
                where.AppendFormat(" AND ((Actor LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ActivityName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (Title LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (WorkflowAlias LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (SheetId LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (CreatorName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ActivityInstanceId LIKE '%{0}%'))", keywords);
            }

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }
    }
}
