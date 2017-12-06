using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.Workflow.Routing.Implements;
using System.Data.OracleClient;
using Botwave.Extension.IBatisNet;
using System.Data;
using Botwave.Workflow.Routing.Domain;

namespace Botwave.Workflow.Routing.Commons
{
    public class ActivityRulesHelper : IActivityRulesHelper
    {
        public bool ImportOldRules(string workflowname)
        {
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter("iv_workflowname", OracleType.VarChar),
            };
            parameters[0].Value = workflowname;

            int returnValue = IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "bwwf_sz_importrules", parameters);
            if (returnValue > 0)
            {
                return true;
            }
            return false;
        }

        public DataTable GetRulesInfo(string workflowid)
        {
            string sql = string.Format(@"select t.title 标题, t.activityname 当前步骤,t.nextactivityname 下行步骤,t.condition 流转条件,t.description 流转条件描述,FieldsAssemble 条件字段集合 from bwwf_activities_rules t
left join bwwf_workflows w on t.workflowid = w.workflowid
where w.workflowid = '{0}'", workflowid);
            DataTable result = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            return result == null ? null : result;
        }

        public void ImportRules(DataTable dtRules, Guid workflowId)
        {
            if (dtRules != null && dtRules.Rows.Count > 0)
            {
                IBatisDbHelper.ExecuteNonQuery(CommandType.Text, string.Format("delete from bwwf_activities_rules where workflowid = '{0}'", workflowId));
                foreach (DataRow dw in dtRules.Rows)
                {
                    RulesDetail rulesDetail = new RulesDetail();
                    rulesDetail.Ruleid = Guid.NewGuid();
                    rulesDetail.ActivityName = dw["当前步骤"].ToString();
                    rulesDetail.Workflowid = workflowId;
                    rulesDetail.NextActivityName = dw["下行步骤"].ToString();
                    rulesDetail.StepType = 1;
                    rulesDetail.ParentRuleId = Guid.Empty;
                    rulesDetail.Conditions = dw["流转条件"].ToString();
                    rulesDetail.Description = dw["流转条件描述"].ToString();
                    rulesDetail.Title = dw["标题"].ToString();
                    rulesDetail.Status = 1;
                    rulesDetail.Creator = "admin";
                    rulesDetail.Createdtime = DateTime.Now;
                    rulesDetail.LastModifier = "admin";
                    rulesDetail.LastModtime = DateTime.Now;
                    rulesDetail.FieldsAssemble = Botwave.Commons.DbUtils.ToString(dw["条件字段集合"]);
                    IBatisMapper.Insert("bwwf_ActivityRulesDetail_Insert", rulesDetail);
                }
            }
        }
    }


}
