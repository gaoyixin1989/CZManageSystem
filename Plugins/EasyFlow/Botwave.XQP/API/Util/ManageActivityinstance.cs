using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using System.Data;

namespace Botwave.XQP.API.Util
{
    public class ManageActivityinstance
    {

        public static string GetActivityinstanceid(Guid workflowinstanceid)
        {
            string sql = string.Format("select ActivityInstanceId from dbo.bwwf_Tracking_Activities where WorkflowInstanceId='{0}'", workflowinstanceid);
            return IBatisDbHelper.ExecuteScalar(CommandType.Text, sql) == null ? string.Empty : IBatisDbHelper.ExecuteScalar(CommandType.Text, sql).ToString();
        }
        #region kamael by 2012-10-23
        public static bool GetEnableApprovalSMSSwitch(Guid WorkflowId, string ActivityInstanceid)
        {
            string sql = string.Format("select [enable] from xqp_ApprovalSMSSwitch where WorkflowId='{0}' and Activityid in (select Activityid from bwwf_Tracking_Activities where ActivityInstanceid='{1}')", WorkflowId, ActivityInstanceid);
            object obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
            if (obj == null || obj == DBNull.Value)
                return false;
            return Convert.ToInt32(obj) == 1 ? true : false;
        }

        public static string GetCreator(Guid workflowinstanceid)
        {
            string sql = string.Format("select creator from bwwf_Tracking_Workflows where WorkflowInstanceId='{0}'", workflowinstanceid);
            object obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
            if (obj == null || obj == DBNull.Value)
                return null;
            return obj.ToString();
        }
        #endregion

    }
}
