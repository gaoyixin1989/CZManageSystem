using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Routing.Domain;
using System.Data.OracleClient;
using Botwave.Workflow.Routing.Implements;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using System.Xml.Linq;
using Botwave.Security.Service;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Spring.Context.Support;
using Botwave.Commons;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Botwave.Workflow.Routing.Contracts
{
    public class ActivityRulesService : IActivityRulesService
    {
        private IFormInstanceService formInstanceService;
        private IUserService userService;
        private IWorkflowUserService workflowUserService;
        private IWorkflowService workflowService;

        
        public IFormInstanceService FormInstanceService
        {
            get { return formInstanceService; }
            set { formInstanceService = value; }
        }

        /// <summary>
        /// 用户服务接口，只写.
        /// </summary>
        public IUserService UserService
        {
            get { return userService; }
            set { userService = value; }
        }

        public IWorkflowService WorkflowService
        {
            set { workflowService = value; }
        }

        public IWorkflowUserService WorkflowUserService
        {
            set { workflowUserService = value; }
        }

        public DataTable GetActivityRules(string workflowId, string activityName, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "bwwf_Activities_Rules";
            string fieldKey = "ruleid";
            //string fieldShow = "Id, FromUserId, ToUserId, IsFullAuthorized, BeginTime, EndTime, Enabled, FromRealName, ToRealName, ToDpFullName";
            string fieldShow = "ruleId, activityName, nextactivityName, stepType, parentRuleid, condition, description, title, status, creator, FieldsAssemble";
            string fieldOrder = "ruleId DESC";

            //string where = string.Format("FromUserId = '{0}'", fromUserId);
            string where = string.Format("activityName = '{0}' and workflowid = '{1}'and status = 1 and StepType = 1", activityName, workflowId);

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where, ref recordCount);
        }

        public RulesDetail GetActivityRule(Guid ruleId)
        {
            RulesDetail info = IBatisMapper.Load<RulesDetail>("bwwf_ActivityRule_Select", ruleId);
            return info;
        }

        public RulesDetail GetStartActivityRules(string WorkflowId, string activityName, string nextActivityName)
        {
            Hashtable parameters = new Hashtable(2);
            parameters.Add("WorkflowId", new Guid(WorkflowId));
            parameters.Add("ActivityName", activityName);
            parameters.Add("NextActivityName", nextActivityName);
            IList<RulesDetail> rulesDetail = IBatisMapper.Select<RulesDetail>("bwwf_StartActivityRules_Select", parameters);
            if (rulesDetail == null || rulesDetail.Count == 0)
                return null;
            return rulesDetail[0];
        }

        public RulesDetail GetNextActivityRules(string WorkflowId, string activityName, string nextActivityName)
        {
            Hashtable parameters = new Hashtable(3);
            parameters.Add("WorkflowId", new Guid(WorkflowId));
            parameters.Add("ActivityName", activityName);
            parameters.Add("NextActivityName", nextActivityName);
            IList<RulesDetail> rulesDetail = IBatisMapper.Select<RulesDetail>("bwwf_NextActivityRules_Select", parameters);
            if (rulesDetail == null || rulesDetail.Count == 0)
                return null;
            return rulesDetail[0];
        }

        public IList<RulesDetail> GetActivityRules(string WorkflowId, string activityName)
        {
            Hashtable parameters = new Hashtable(3);
            parameters.Add("WorkflowId", new Guid(WorkflowId));
            parameters.Add("ActivityName", activityName);
            IList<RulesDetail> rulesDetail = IBatisMapper.Select<RulesDetail>("bwwf_ActivityRules_Select", parameters);
            return rulesDetail;
        }
        public IList<RulesDetail> GetRelationRules(string WorkflowId, string activityName)
        {
            Hashtable parameters = new Hashtable(3);
            parameters.Add("WorkflowId", new Guid(WorkflowId));
            parameters.Add("ActivityName", activityName);
            IList<RulesDetail> rulesDetail = IBatisMapper.Select<RulesDetail>("bwwf_RelationRules_Select", parameters);
            return rulesDetail;
        }

        public void ActivityRulesDetailInsert(RulesDetail rulesDetail)
        {
            object result = IBatisMapper.Insert("bwwf_ActivityRulesDetail_Insert", rulesDetail);
        }

        public int ActivityRulesDetailUpdate(RulesDetail rulesDetail)
        {
            return IBatisMapper.Update("bwwf_ActivityRulesDetail_Update", rulesDetail);
        }

        public int ActivityRulesDetailDelete(string id)
        {
            return IBatisMapper.Delete("bwwf_ActivityRulesDetail_Delete", id);
        }

        public int ActivityRulesInsertForStart(RulesDetail rulesDetail)
        {
            object result = IBatisMapper.Insert("bwwf_ActivityRules_Insert_ForStart", rulesDetail);
            return Convert.ToInt32(result.ToString());
        }

        public int ExistActivityRules(RulesDetail rulesDetail)
        {
            object result = IBatisMapper.Load<object>("bwwf_ActivityRulesDetail_ExistRules", rulesDetail);
            if (result == null)
                return -1;
            return Convert.ToInt32(result);
        }

        public int ActivityRulesDetailUpdateByActName(RulesDetail rulesDetail)
        {
            return IBatisMapper.Update("bwwf_ActivityRulesDetail_Update_ByActName", rulesDetail);
        }

        //public int ActivityRulesAnalysis(RulesDetail rulesDetail)
        //{
        //    OracleParameter[] parameters = new OracleParameter[] { 
        //        new OracleParameter("iv_workflowInstanceId", OracleType.VarChar),
        //        //new OracleParameter("iv_activityId", OracleType.VarChar),
        //        //new OracleParameter("iv_nextActivityId", OracleType.VarChar),
        //        new OracleParameter("iv_contidions", OracleType.VarChar),
        //        new OracleParameter("iv_stepType", OracleType.VarChar),
        //        new OracleParameter("p_cursor", OracleType.Cursor)
        //    };
        //    parameters[0].Direction = ParameterDirection.Input;
        //    parameters[0].Value = rulesDetail.Workflowinstanceid;
        //    parameters[1].Direction = ParameterDirection.Input;
        //    parameters[1].Value = rulesDetail.Conditions;
        //    parameters[2].Direction = ParameterDirection.Input;
        //    parameters[2].Value = rulesDetail.StepType;
        //    parameters[3].Direction = ParameterDirection.Output;

        //    object count = IBatisDbHelper.ExecuteScalar(CommandType.StoredProcedure,"", parameters);
        //    return count == null ? 0 : Convert.ToInt32(count.ToString());
        //}

        public bool GetActivityRulesAnalysisResult(RulesDetail rulesDetail, DataTable dtPreview)
        {
            try
            {
                DataView dvPreview;
                if (dtPreview.Rows.Count > 0 && dtPreview != null)
                {
                    string condition = rulesDetail.Conditions;

                    //if (condition.Trim().Length > 0)
                    //{
                    //    condition = condition.Trim().ToString().Substring(4);
                    //}
                    //dvPreview = new DataView(dtPreview, condition, "", DataViewRowState.CurrentRows);
                    //return dvPreview.Count > 0 ? true : false;
                    return SqlBulkInsert(dtPreview, condition);
                }
                return true;    
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public DataTable GetNextActivitys(string activityid)
        {
            string sql = string.Format(@"select f.ActivityId,f.ActivityName,g.ActivityId as nextactid,g.activityname as nextname from (
select a.ActivityId,a.activityname,a.NextActivitySetId from 
bwwf_Activities a where 
a.activityid ='{0}') f
, (select a.ActivityId, SetID,ActivityName from bwwf_Activities a,bwwf_ActivitySet b where a.ActivityId=b.ActivityId ) g
where f.NextActivitySetId=g.setid", activityid);
            DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            return dt;
        }

        public DataTable GetFormItemDefinitions(string workflowid)
        {
            string sql = string.Format(@"select FName,Name,datasource from bwdf_FormItemDefinitions d where formdefinitionid = 
(select formdefinitionid from bwdf_FormDefinitionInExternals 
where EntityId = '{0}' and entitytype='Form_Workflow') and d.itemType not in (7,9)", workflowid);
            DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            return dt;
        }

        public DataRow GetFormItemDataSource(string workflowid, string fName)
        {
            string sql = string.Format(@"select itemType, datasource from bwdf_FormItemDefinitions d where formdefinitionid = 
(select formdefinitionid from bwdf_FormDefinitionInExternals 
where EntityId = '{0}') and FName = '{1}'", workflowid, fName);
            DataTable result = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            return result.Rows.Count==0 ? null : result.Rows[0];
        }

        public DataTable GetInstanceTable(Guid workflowid, Guid workflowinstanceid, string currentActor, IDictionary<string, object> formVariables, bool isInit)
        {
            if (formInstanceService == null)
            {
                formInstanceService = WebApplicationContext.Current["formInstanceService"] as IFormInstanceService;
            }
            if (workflowService == null)
            {
                workflowService = WebApplicationContext.Current["workflowService"] as IWorkflowService;
            }
            if (workflowUserService == null)
            {
                workflowUserService = WebApplicationContext.Current["workflowUserService"] as IWorkflowUserService;
            }
            if (userService == null)
            {
                userService = WebApplicationContext.Current["userService"] as IUserService;
            }

            WorkflowInstance workflowInstance = null;
            workflowInstance = workflowService.GetWorkflowInstance(workflowinstanceid);
            if (workflowInstance == null)//发单的时候
            {
                DataTable dtItem = GetFormItemDefinitions(workflowid.ToString());
                foreach (DataRow dw in dtItem.Rows)
                {
                    if (!formVariables.ContainsKey(dw["FName"].ToString()))
                        formVariables.Add(dw["FName"].ToString(),string.Empty);
                }
            }

            ActorDetail creatorInfo = workflowUserService.GetActorDetail(workflowInstance == null ? currentActor : workflowInstance.Creator);
            DataTable dtPreview = new DataTable();
            string[] arr_Atc1 = currentActor.Split("$".ToCharArray());
            if (arr_Atc1.Length > 1)//上一步退回处理人
            {
                currentActor = arr_Atc1[0];//防止无法获取到正常流转的下一步处理人               
            }
            Botwave.Security.Domain.UserInfo user = userService.GetUserByUserName(currentActor);

            DataRow row;
            IDictionary<string, string> fArray = new Dictionary<string, string>();
            string textValue = string.Empty;
            string value = string.Empty;
            row = dtPreview.NewRow();
            if (!dtPreview.Columns.Contains("提单人姓名"))
                dtPreview.Columns.Add("提单人姓名");
            if (!dtPreview.Columns.Contains("提单人部门"))
                dtPreview.Columns.Add("提单人部门");
            if (!dtPreview.Columns.Contains("当前部门"))
                dtPreview.Columns.Add("当前部门");
            if (!dtPreview.Columns.Contains("当前用户姓名"))
                dtPreview.Columns.Add("当前用户姓名");
            row["提单人姓名"] = creatorInfo.RealName;
            row["提单人部门"] = creatorInfo.DpFullName;
            row["当前用户姓名"] = user.RealName;
            row["当前部门"] = user.DpFullName;

            foreach (KeyValuePair<string, object> pair in formVariables)
            {
                if (!dtPreview.Columns.Contains(pair.Key))
                {
                    dtPreview.Columns.Add(new DataColumn(pair.Key));
                }
                row[pair.Key] = pair.Value;
            }
            if (isInit&&workflowInstance!=null)
            {
                FormInstance instance = formInstanceService.GetFormInstanceById(workflowinstanceid, true);
                if (null != instance && null != instance.Items)
                {
                    foreach (FormItemInstance itemInstance in instance.Items)
                    {
                        if (null == itemInstance.Definition) continue;

                        if (!dtPreview.Columns.Contains(itemInstance.Definition.FName))
                        {
                            dtPreview.Columns.Add(new DataColumn(itemInstance.Definition.FName));
                            textValue = (itemInstance.TextValue == null ? "" : itemInstance.TextValue);
                            value = (itemInstance.Value == null ? "" : itemInstance.Value);
                            textValue = textValue.Length > 200 ? textValue.Substring(0, 200) : textValue;//只取200长度
                            value = value.Length > 200 ? value.Substring(0, 200) : value;//只取200长度
                            row[itemInstance.Definition.FName] = itemInstance.Definition.ItemDataType == FormItemDefinition.DataType.Text ? textValue : value;
                        }
                       
                    }
                }
            }
            //return null;
            dtPreview.Rows.Add(row);
            return dtPreview;
        }

        public void ActivityOrganizationTypeInsert(string Activityid, int OrganizationType)
        {
            Hashtable parameters = new Hashtable(3);
            parameters.Add("Activityid", Activityid);
            parameters.Add("OrganizationType", OrganizationType);
            parameters.Add("Status", 1);
            IBatisMapper.Insert("bwwf_ActivityOrganizationType_Insert", parameters);
        }

        public int ActivityOrganizationTypeUpdate(string Activityid, int OrganizationType)
        {
            Hashtable parameters = new Hashtable(2);
            parameters.Add("Activityid", Activityid);
            parameters.Add("OrganizationType", OrganizationType);
            return IBatisMapper.Update("bwwf_ActivityOrganizationType_Update", parameters);
        }

        public int ActivityOrganizationTypeDelete(string activityid)
        {
            return IBatisMapper.Delete("bwwf_ActivityOrganizationType_Delete", activityid);
        }

        public int GetActivityOrganizationType(string activityid)
        {
            return  IBatisMapper.Load<int>("bwwf_ActivityOrganizationType_Selete", activityid);
        }

        public int ExistActivityOrganizationType(string activityid)
        {
            return IBatisMapper.Load<int>("bwwf_ActivityOrganizationType_Exist", activityid);
        }

        /// <summary>
        /// 将表单数据插入临时表后直接在数据库执行SQL查询结果
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="conn"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        private bool SqlBulkInsert(DataTable dt, string condition)
        {
            int count = dt.Rows.Count;
            int copyTimeout = 600;
            object cnt = 0;
            StringBuilder sbSql = new StringBuilder();
            StringBuilder sbInsert = new StringBuilder(" insert #TEMP_TBL values (");
            DataView dvPreview;
            SqlConnection conn = new SqlConnection(IBatisDbHelper.ConnectionString);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlTransaction scope = null;
            try
            {
                sbSql.Append("if exists(select * from tempdb..sysobjects where id=object_id('tempdb..#TEMP_TBL')) drop table #TEMP_TBL");
                sbSql.Append(" CREATE TABLE #TEMP_TBL(");
                Regex r = new Regex("^(-?[0-9]*[.]*[0-9]{0,8})$");
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string val = DbUtils.ToString(dt.Rows[0][dt.Columns[i].ColumnName]);
                    //列映射定义数据源中的列和目标表中的列之间的关系   
                    string paramName = "@" + dt.Columns[i].ColumnName.Trim();
                    if (i > 3 && r.IsMatch(val)&&!string.IsNullOrEmpty(val)&&dt.Columns[i].ColumnName.ToLower()!="dpid")
                    {
                        sbSql.Append("\r[" + dt.Columns[i].ColumnName.Trim() + "] decimal NULL,");
                        comm.Parameters.Add(new SqlParameter(paramName, decimal.Parse(val)));
                    }
                    else
                    {
                        sbSql.Append("\r[" + dt.Columns[i].ColumnName.Trim() + "] NVARCHAR(210) NULL,");
                        comm.Parameters.Add(new SqlParameter(paramName, val));
                    }
                   
                    sbInsert.Append(paramName+",");
                    
                }
                sbSql = sbSql.Remove(sbSql.Length-1,1);
                sbSql.Append(")");
                sbInsert = sbInsert.Remove(sbInsert.Length - 1, 1);
                sbInsert.Append(")");
                sbSql.Append(sbInsert);
                sbSql.AppendLine(" select count(0) from #TEMP_TBL where " + condition);
                //string lastval = DbUtils.ToString(dt.Rows[0][dt.Columns[dt.Columns.Count - 1].ColumnName]);
                //if (r.IsMatch(lastval))
                //{
                //    sbSql.Append("\r" + dt.Columns[dt.Columns.Count - 1].ColumnName.Trim() + " decimal NULL) ");
                //}
                //else
                //    sbSql.Append("\r" + dt.Columns[dt.Columns.Count - 1].ColumnName.Trim() + " NVARCHAR(210) NULL) ");
                //IBatisDbHelper.ExecuteNonQuery(conn, CommandType.Text, sbSql.ToString(),param);
                //scope = conn.BeginTransaction();
                //using (SqlBulkCopy sbc = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, scope))
                //{
                //    //服务器上目标表的名称   
                //    sbc.DestinationTableName = "#TEMP_TBL";
                //    sbc.BatchSize = 800;//每次提交的记录数
                //    sbc.BulkCopyTimeout = copyTimeout;
                //    sbc.WriteToServer(dt);
                //} scope.Commit();//有效的事务  
                
                //comm.CommandText = sbSql.ToString();
                //comm.ExecuteNonQuery();
                comm.CommandText = sbSql.ToString();
                cnt = comm.ExecuteScalar();
                comm.CommandText = "if exists(select * from tempdb..sysobjects where id=object_id('tempdb..#TEMP_TBL')) drop table #TEMP_TBL";
                comm.ExecuteNonQuery();
                comm.Dispose();
                //cnt = IBatisDbHelper.ExecuteScalar(conn, CommandType.Text, "select count(0) from #TEMP_TBL where " + condition);
                //IBatisDbHelper.ExecuteNonQuery(conn, CommandType.Text, " drop table #TEMP_TBL");
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            catch (Exception ex)
            {
                if (scope != null)
                {
                    scope.Rollback();
                }
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw ex;
            }
            return DbUtils.ToInt32(cnt) > 0;
        }
    }
}
