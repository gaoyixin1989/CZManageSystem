using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Entities;
using Botwave.Extension.IBatisNet;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service
{
    /// <summary>
    /// 流程设置辅助类.
    /// </summary>
    public class WorkflowProfileHelper
    {
        /// <summary>
        /// 格式化自动生成流程名称.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string FormatWorkflowInstanceTitle(WorkflowProfile profile, Botwave.Security.LoginUser user)
        {
            if (profile == null)
                profile = WorkflowProfile.Default;
            return FormatWorkflowInstanceTitle(profile.WorkflowInstanceTitle, profile.WorkflowName, user);
        }

        /// <summary>
        /// 格式化自动生成流程名称.
        /// </summary>
        /// <param name="titleFormat"></param>
        /// <param name="workflowName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string FormatWorkflowInstanceTitle(string titleFormat, string workflowName, Botwave.Security.LoginUser user)
        {
            if (string.IsNullOrEmpty(titleFormat))
                return string.Empty;
            
            DateTime now = DateTime.Now;
            titleFormat = titleFormat.ToUpper();
            titleFormat = titleFormat.Replace("$WORKFLOW", workflowName);
            titleFormat = titleFormat.Replace("$DATETIME", now.ToString("yyyMMddHHmmss"));
            titleFormat = titleFormat.Replace("$DATE", now.ToString("yyyMMdd"));
            if (user != null)
            {
                titleFormat = titleFormat.Replace("$USER", user.RealName);
                string deptName = string.Empty;
                if (user.Properties.ContainsKey("DepartmentName"))
                {
                    deptName = user.Properties["DepartmentName"].ToString();
                }
                if (string.IsNullOrEmpty(deptName))
                {
                    deptName = GetDpeartmentName(user.DpId);
                    user.Properties["DepartmentName"] = deptName;
                }
                titleFormat = titleFormat.Replace("$DEPT", deptName);

                string mobile = (user.Mobile == null ? string.Empty : user.Mobile.Trim());
                string tel = (user.Tel == null ? string.Empty : user.Tel.Trim());

                titleFormat = titleFormat.Replace("$MOBILE", mobile);
                titleFormat = titleFormat.Replace("$TEL", (string.IsNullOrEmpty(mobile) ? tel : mobile));
            }
            return titleFormat;
        }

        private static string GetDpeartmentName(string dpId)
        {
            if (string.IsNullOrEmpty(dpId))
                return string.Empty;
            string sql = @"SELECT (CASE WHEN d.DpLevel <=4 THEN d.DpName ELSE pd.DpName END) AS DpName, d.DpFullName 
FROM bw_Depts d
	LEFT JOIN bw_Depts pd ON pd.DpId = d.ParentDpId
WHERE d.DpId='{0}'";
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format(sql, dpId));

            return ((result == null || result == DBNull.Value) ? string.Empty : result.ToString().Trim());
        }

        public static IDictionary<string, string> GetActorNames(ICollection<string> actors, out IList<BasicUser> users)
        {
            users = new List<BasicUser>();
            IDictionary<string, string> details = new Dictionary<string, string>();
            if (actors == null || actors.Count == 0)
                return details;
            StringBuilder where = new StringBuilder();
            foreach (string item in actors)
            {
                if (string.IsNullOrEmpty(item.Trim()))
                    continue;
                where.AppendFormat("'{0}',", item.Trim().Replace("'", "''"));
            }
            if (where.Length == 0)
                return details;

            where.Length = where.Length - 1;
            //string sql = string.Format("SELECT UserName, RealName FROM vw_bw_Users_Detail WHERE UserName IN({0}) ORDER BY SortOrder, [Type], UserName", where.ToString());
            string sql = string.Format("SELECT UserName, RealName FROM vw_bw_Users_Detail WHERE UserName IN({0}) and Status > -1 ORDER BY SortOrder, [Type], UserName", where.ToString());//只获取在职的用户

            DataTable detailTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            foreach (DataRow row in detailTable.Rows)
            {
                string userName = DbUtils.ToString(row["UserName"]);
                string realName = DbUtils.ToString(row["RealName"]);
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(realName))
                    continue;

                if(!details.ContainsKey(userName))
                details.Add(userName, realName);
                users.Add(new BasicUser(userName, realName));
            }
            return details;
        }

        public static DataTable GetWorkflowRejectActivities(Guid workflowInstanceId, Guid activityInstanceId, Guid activityId, string actor)
        {
            IDbDataParameter[] parameters = Botwave.Extension.IBatisNet.IBatisDbHelper.CreateParameterSet(4);
            parameters[0].ParameterName = "WorkflowInstanceId";
            parameters[0].DbType = DbType.Guid;
            parameters[0].Value = workflowInstanceId;
            parameters[1].ParameterName = "ActivityInstanceId";
            parameters[1].DbType = DbType.Guid;
            parameters[1].Value = activityInstanceId;
            parameters[2].ParameterName = "ActivityId";
            parameters[2].DbType = DbType.Guid;
            parameters[2].Value = activityId;
            parameters[3].ParameterName = "Actor";
            parameters[3].DbType = DbType.String;
            parameters[3].Value = actor;

            DataSet result = Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "bwwf_ext_GetRejectActivities", parameters);
            return result == null || result.Tables.Count == 0 ? null : result.Tables[0];
        }
    }
}
