using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.DynamicForm.Extension.Domain;
using Botwave.XQP.Commons;
using Botwave.DynamicForm.Services;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// 流程报表服务的默认实现类.
    /// </summary>
    public class WorkflowReportService : IWorkflowReportService
    {
        #region properties

        private IDataListDefinitionService dataListDefinitionService;

        private IDataListInstanceService dataListInstanceService;

        private IFormInstanceService formInstanceService;

        public IDataListDefinitionService DataListDefinitionService
        {
            set { dataListDefinitionService = value; }
        }

        public IDataListInstanceService DataListInstanceService
        {
            set { this.dataListInstanceService = value; }
        }

        public IFormInstanceService FormInstanceService
        {
            get { return formInstanceService; }
            set { formInstanceService = value; }
        }

        #endregion

        #region IWorkflowReportService Members

        public DataSet GetProcessDetail(string owner, string startDT, string endDT, string workflowName, bool isByUser)
        {
            StringBuilder sbSQL = new StringBuilder();
            string statName = string.Empty;
            string joinSql = string.Empty;
            if (isByUser)
            {
                statName = "ISNULL(u.RealName,'无')";
            }
            else
            {
                statName = "ISNULL(d.DpName,'无')";
                joinSql = " LEFT JOIN bw_Depts as d on u.DpId=d.DpId";
            }
            sbSQL.AppendFormat(@"
                      SELECT convert(varchar,datepart(year,ta.CreatedTime))+'年'+convert(varchar,datepart(month,ta.CreatedTime))+'月'+'_'+{3} as StatName,count(*) as TotalCount
                      FROM bwwf_Tracking_Activities_Completed AS ta
                          LEFT JOIN bwwf_Activities as a on ta.ActivityId=a.ActivityId
		                  LEFT JOIN bw_Users as u on ta.actor=u.username{4}
                          LEFT JOIN bwwf_Workflows AS w on a.WorkflowId=w.WorkflowId
                      WHERE ta.CreatedTime between '{0}' and '{1}' AND w.WorkflowName='{2}' {5}
                      group by convert(varchar,datepart(year,ta.CreatedTime))+'年'+convert(varchar,datepart(month,ta.CreatedTime))+'月'+'_'+{3};
                        
                      SELECT convert(varchar,datepart(year,ta.CreatedTime))+'年'+convert(varchar,datepart(month,ta.CreatedTime))+'月'+'_'+{3}+'_'+cast(ta.activityid as varchar(50)) as StatName,count(*) as ActivityTrackingCount
                      FROM bwwf_Tracking_Activities_Completed AS ta
                          LEFT JOIN bwwf_Activities as a on ta.ActivityId=a.ActivityId
                          LEFT JOIN bw_Users as u on ta.actor=u.username{4}
                          LEFT JOIN bwwf_Workflows AS w on a.WorkflowId=w.WorkflowId
                      WHERE ta.CreatedTime between '{0}' and '{1}' AND w.WorkflowName='{2}'  {5}
                      group by convert(varchar,datepart(year,ta.CreatedTime))+'年'+convert(varchar,datepart(month,ta.CreatedTime))+'月'+'_'+{3}+'_'+cast(ta.activityid as varchar(50));

                      SELECT * FROM bwwf_Activities AS a 
                            LEFT JOIN bwwf_Workflows AS w on a.WorkflowId=w.WorkflowId  
                      WHERE w.WorkflowName='{2}' AND w.IsCurrent=1 order by state", startDT, endDT, workflowName, statName, joinSql, string.IsNullOrEmpty(owner) ? string.Empty : string.Format("and (ta.workflowInstanceId in (select workflowInstanceid from bwwf_Tracking_Activities_Completed where Actor = '{0}'))", owner));

            DataSet dsReport = IBatisDbHelper.ExecuteDataset(CommandType.Text, sbSQL.ToString());

            DataTable childDt;
            DataRow[] drList;
            DataRow blankRow;
            string[] fields;
            bool hasData;
            int[] activityCount;
            int totalCount = 0;
            string guidStr;

            dsReport.Tables[1].Columns.Add("StatTime", typeof(string));

            foreach (DataRow dr in dsReport.Tables[1].Rows)
            {
                fields = dr["StatName"].ToString().Split('_');
                dr["StatTime"] = fields[0] + "_" + fields[1];
            }

            dsReport.Relations.Add("relation", dsReport.Tables[0].Columns["StatName"], dsReport.Tables[1].Columns["StatTime"], false);

            dsReport.Tables[0].Columns.Add("Actor", typeof(string));
            dsReport.Tables[0].Columns.Add("StatTime", typeof(string));

            childDt = dsReport.Tables[1].Clone();

            activityCount = new int[dsReport.Tables[2].Rows.Count];

            foreach (DataRow dr in dsReport.Tables[0].Rows)
            {
                fields = dr["StatName"].ToString().Split('_');

                dr["Actor"] = fields[1];
                dr["StatTime"] = fields[0];

                drList = dr.GetChildRows("relation");

                for (int i = 0; i < dsReport.Tables[2].Rows.Count; i++)
                {
                    hasData = false;

                    foreach (DataRow _dr in drList)
                    {
                        if (_dr["StatName"].ToString().Split('_')[2].ToLower() == dsReport.Tables[2].Rows[i]["ActivityId"].ToString().ToLower())
                        {
                            childDt.ImportRow(_dr);
                            activityCount[i] += (int)_dr["ActivityTrackingCount"];
                            hasData = true;

                            break;
                        }
                    }

                    if (!hasData)
                    {
                        blankRow = childDt.NewRow();
                        blankRow["StatTime"] = dr["StatName"];
                        blankRow["ActivityTrackingCount"] = 0;

                        childDt.Rows.Add(blankRow);
                    }
                }

                totalCount += (int)dr["TotalCount"];
            }

            dsReport.Tables[1].Rows.Clear();

            foreach (DataRow dr in childDt.Rows)
            {
                dsReport.Tables[1].ImportRow(dr);
            }

            guidStr = Guid.NewGuid().ToString();

            dsReport.Tables[0].Rows.Add(new object[] { guidStr, totalCount, "总计", "" });

            foreach (int count in activityCount)
            {
                dsReport.Tables[1].Rows.Add(new object[] { guidStr, count, guidStr });
            }

            return dsReport;
        }

        public DataTable GetWorkflowBusinessPager(string owner, string workflowName, string startDT, string endDT,string dpnames,string state, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_xqp_Tracking_Workflows_Detail";
            string fieldKey = "WorkflowInstanceId";
            string fieldShow = "WorkflowInstanceId,工单标题,工单号,发起人,发起人部门,发起时间,期望完成时间,保密级别,紧急程度,重要级别,case 状态 when 1 then '流转中' when 2 then '已完成' when 99 then '已取消' end as 工单状态";
            string fieldOrder = "发起时间 DESC";
            StringBuilder where = new StringBuilder();

            where.AppendFormat("(WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(owner))
                where.AppendFormat(" AND ((发起人 = '{0}') OR (WorkflowInstanceId IN (SELECT WorkflowInstanceId FROM vw_bwwf_Tracking_Activities_All ta LEFT JOIN bw_Users u ON ta.Actor = u.UserName WHERE u.RealName = '{0}')))", owner);
            if (!string.IsNullOrEmpty(startDT))
                where.AppendFormat(" AND (发起时间 >= CAST('{0}' AS datetime))", startDT);
            if (!string.IsNullOrEmpty(endDT))
                where.AppendFormat(" AND (发起时间 <= CAST('{0}' AS datetime))", endDT);
            if (!string.IsNullOrEmpty(dpnames))
            {
                where.AppendFormat(" and 发起人 in (select realname from bw_users u inner join bw_depts d on u.dpid=d.dpid where dpfullname in ('{0}'))", dpnames.Replace(",", "','"));
            }
            if (!string.IsNullOrEmpty(state))
            {
                where.AppendFormat(" and 状态 ={0}",state);
            }
            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }

        public DataSet GetWorkflowOption(string owner, string startDT, string endDT, string workflowName)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendFormat(@"
                    SELECT ta.ActivityInstanceId, tw.WorkflowInstanceId, tw.Title,tw.Sheetid, w.WorkflowName FROM (
	                    SELECT MAX(cast(ActivityInstanceId AS varchar(36))) ActivityInstanceId, WorkflowInstanceId
	                    FROM bwwf_Tracking_Activities_Completed ta {3}
	                    GROUP BY WorkflowInstanceId
                    )ta
	                    LEFT JOIN bwwf_Tracking_Workflows tw ON tw.WorkflowInstanceId = ta.WorkflowInstanceId
	                    LEFT JOIN bwwf_Workflows w ON w.WorkflowId = tw.WorkflowId
                    WHERE w.WorkflowName = '{0}' AND 
	                    tw.StartedTime >= CAST('{1}' AS datetime) AND tw.StartedTime <= CAST('{2}' AS datetime)
                    
                    SELECT a.ActivityName, a.ActorName, a.Reason, a.FinishedTime, a.WorkflowInstanceId from
                    (SELECT a.ActivityName, u.RealName AS ActorName, ta.Reason, ta.FinishedTime, ta.WorkflowInstanceId,1 as sort
                    FROM bwwf_Tracking_Activities_Completed ta 
                        LEFT JOIN bwwf_Tracking_Workflows tw ON tw.WorkflowInstanceId = ta.WorkflowInstanceId
	                    LEFT JOIN bwwf_Workflows w ON w.WorkflowId = tw.WorkflowId
	                    LEFT JOIN bwwf_Activities a ON a.ActivityId = ta.ActivityId 
	                    LEFT JOIN bw_Users u ON u.UserName = ta.Actor {4}
                    WHERE w.WorkflowName = '{0}' AND 
	                    tw.StartedTime >= CAST('{1}' AS datetime) AND tw.StartedTime <= CAST('{2}' AS datetime)
                    union 
                    SELECT a.ActivityName, dbo.fn_bwwf_GetCurrentActors(ta.WorkflowInstanceId) AS ActorName,
                    ta.Reason, ta.FinishedTime, ta.WorkflowInstanceId,2 as sort
                    
                    FROM bwwf_Tracking_Activities ta 
	                    LEFT JOIN bwwf_Activities a ON a.ActivityId = ta.ActivityId 
                        LEFT JOIN bwwf_Tracking_Workflows tw ON tw.WorkflowInstanceId = ta.WorkflowInstanceId
	                    LEFT JOIN bwwf_Workflows w ON w.WorkflowId = tw.WorkflowId
                    WHERE w.WorkflowName = '{0}' AND 
	                    tw.StartedTime >= CAST('{1}' AS datetime) AND tw.StartedTime <= CAST('{2}' AS datetime)) a
                    ORDER BY sort,FinishedTime", workflowName, startDT, endDT, string.IsNullOrEmpty(owner) ? string.Empty : string.Format(" where ta.Actor = '{0}'", owner), string.IsNullOrEmpty(owner) ? string.Empty : string.Format(" and ta.Actor = '{0}'", owner));

            DataSet dsOption = IBatisDbHelper.ExecuteDataset(CommandType.Text, sbSQL.ToString());

            dsOption.Relations.Add("option", dsOption.Tables[0].Columns["WorkflowInstanceId"], dsOption.Tables[1].Columns["WorkflowInstanceId"], false);

            return dsOption;
        }

        public DataTable GetWorkflowTrackingPager(string owner, string workflowName, string activityName, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_xqp_Tracking_Activities_Todo";
            string fieldKey = "ActivityInstanceId";
            string fieldShow = "ActivityInstanceId, ActivityName, WorkflowInstanceId, SheetId, CreatedTime, Title, WorkflowAlias, CreatorName, AliasImage, IsCompleted ";
            string fieldOrder = "StartedTime DESC";

            StringBuilder where = new StringBuilder();

            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat(" (WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(activityName))
                where.AppendFormat(" AND (ActivityName = '{0}')", activityName);

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }

        public DataTable GetActivityStat(string owner, string workflowName, string startDT, string endDT)
        {
            IDbDataParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Owner", SqlDbType.NVarChar, 50),
                new SqlParameter("@WorkflowName", SqlDbType.NVarChar, 50),
                new SqlParameter("@StartDT", SqlDbType.DateTime),
                new SqlParameter("@EndDT", SqlDbType.DateTime)
            };

            DateTime start = (string.IsNullOrEmpty(startDT) ? Botwave.Commons.DateTimeUtils.MinValue : Convert.ToDateTime(startDT));
            DateTime end = (string.IsNullOrEmpty(endDT) ? Botwave.Commons.DateTimeUtils.MaxValue : Convert.ToDateTime(endDT));

            parameters[0].Value = owner;
            parameters[1].Value = workflowName;
            parameters[2].Value = start;
            parameters[3].Value = end;

            DataSet result = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "bwwf_ext_GetActivitiesStatByName_Owner", parameters);
            return ((result.Tables != null && result.Tables.Count > 0) ? result.Tables[0] : null);
        }

        #endregion

        /// <summary>
        /// 获取所有业务数据，主要用于导出.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="workflowName"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <returns></returns>
        public static DataTable GetWorkflowBusinessAll(string owner, string workflowName, string startDT, string endDT, string dpnames, string state)
        {
            string sql = @"select WorkflowInstanceId,工单标题,工单号,发起人,发起人部门,发起时间,期望完成时间,保密级别,紧急程度,重要级别,case 状态 when 1 then '流转中' when 2 then '已完成' when 99 then '已取消' end as 工单状态
                                        from vw_xqp_Tracking_Workflows_Detail
                                        {0} order by 发起时间 DESC";

            StringBuilder where = new StringBuilder();

            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat("(WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(owner))
                where.AppendFormat(" AND ((发起人 = '{0}') OR (WorkflowInstanceId IN (SELECT WorkflowInstanceId FROM vw_bwwf_Tracking_Activities_All ta LEFT JOIN bw_Users u ON ta.Actor = u.UserName WHERE u.RealName = '{0}')))", owner);
            if (!string.IsNullOrEmpty(startDT))
                where.AppendFormat(" AND (发起时间 >= CAST('{0}' AS datetime))", startDT);
            if (!string.IsNullOrEmpty(endDT))
                where.AppendFormat(" AND (发起时间 <= CAST('{0}' AS datetime))", endDT);
            if (!string.IsNullOrEmpty(dpnames))
            {
                where.AppendFormat(" and 发起人 in (select realname from bw_users u inner join bw_depts d on u.dpid=d.dpid where dpfullname in ('{0}'))", dpnames.Replace(",", "','"));
            }
            if (!string.IsNullOrEmpty(state))
            {
                where.AppendFormat(" and 状态 ={0}", state);
            }

            sql = string.Format(sql, (string.IsNullOrEmpty(where.ToString()) ? "" : (" where " + where.ToString())));
            DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            return dt;
        }

        public  DataTable LoadWorkflowFormData(string owner, string workflowName, string startDT, string endDT,string dpnames,string state, int? pageIndex, int? pageSize, ref int recordCount)
        {
            DataTable workflowDt = null;
           
            if (!pageIndex.HasValue && !pageSize.HasValue )
            {
                workflowDt = WorkflowReportService.GetWorkflowBusinessAll(owner, workflowName, startDT, endDT, dpnames, state);
            }
            else
            {
                workflowDt = GetWorkflowBusinessPager(owner, workflowName, startDT, endDT, dpnames, state, pageIndex.Value, pageSize.Value, ref recordCount);

            }

            DataTable formItemInstanceDt = new DataTable();
            formItemInstanceDt.Columns.Add(new DataColumn("FormInstanceId", typeof(System.Guid)));

            DataTable dlItemInstanceDt = new DataTable();
            dlItemInstanceDt.Columns.Add(new DataColumn("fid", typeof(System.Guid)));

            //附加表单项实例数据
            foreach (DataRow row in workflowDt.Rows)
            {

                if (string.IsNullOrEmpty(Botwave.Commons.DbUtils.ToString(row["WorkflowInstanceId"])))
                    continue;

                Guid instanceId = new Guid(Botwave.Commons.DbUtils.ToString(row["WorkflowInstanceId"]));
                FormInstance formInstance = formInstanceService.GetFormInstanceById(instanceId, true);
                IList<FormItemInstance> itemInstanceList = formInstance.Items;
                fillFormItemInstance2Row(ref formItemInstanceDt, itemInstanceList);
                fillDlItem2Row(ref dlItemInstanceDt, itemInstanceList);

            }


            DataTable mergeTable = DataTableJoinUtil.JoinTwoDataTables(workflowDt, formItemInstanceDt, "WorkflowInstanceId", "FormInstanceId", DataTableJoinUtil.JoinType.Left);
            mergeTable = DataTableJoinUtil.JoinTwoDataTables(mergeTable, dlItemInstanceDt, "FormInstanceId", "fid", DataTableJoinUtil.JoinType.Left);


            return mergeTable;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="formItemInstances"></param>
        private void fillFormItemInstance2Row(ref DataTable formItemInstanceDt, IList<FormItemInstance> formItemInstances)
        {
            if (formItemInstances.Count < 1) return;
            DataRow dr = formItemInstanceDt.NewRow();
            formItemInstanceDt.Rows.Add(dr);

            foreach (FormItemInstance itemInstance in formItemInstances)
            {

                if (itemInstance.Definition.ItemType.Equals(FormItemDefinition.FormItemType.Html)) continue;

                FormItemDefinition.DataType itemDataType = itemInstance.Definition.ItemDataType;
                Type dataType = (itemDataType == FormItemDefinition.DataType.Decimal) ? typeof(Decimal) : typeof(String);
                if (!formItemInstanceDt.Columns.Contains(itemInstance.Definition.Name))
                {
                    DataColumn newDataColumn = new DataColumn(itemInstance.Definition.Name, dataType);
                    formItemInstanceDt.Columns.Add(newDataColumn);
                }



                switch (itemDataType)
                {
                    case FormItemDefinition.DataType.Text:
                        dr[itemInstance.Definition.Name] = itemInstance.TextValue;
                        break;

                    case FormItemDefinition.DataType.String:
                        dr[itemInstance.Definition.Name] = itemInstance.Value;
                        break;

                    case FormItemDefinition.DataType.Decimal:
                        dr[itemInstance.Definition.Name] = itemInstance.DecimalValue;
                        break;

                }

                //dr["FormItemDefinitionId"] = itemInstance.FormItemDefinitionId;

            }

            dr["FormInstanceId"] = formItemInstances[0].FormInstanceId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="formItemInstances"></param>
        private void fillDlItem2Row(ref DataTable dlItemDatable, IList<FormItemInstance> formItemInstances)
        {

            int currentCount = dlItemDatable.Rows.Count;
            foreach (FormItemInstance itemInstance in formItemInstances)
            {

                if (!itemInstance.Definition.ItemType.Equals(FormItemDefinition.FormItemType.Html)) continue;

                IList<DataListItemDefinition> dlItemDefs = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(itemInstance.Definition.Id);

                foreach (DataListItemDefinition dlItemDef in dlItemDefs)
                {
                    String title = String.Concat(dlItemDef.Name, "[", itemInstance.Definition.Name, "]");

                    if (!dlItemDatable.Columns.Contains(title))
                    {

                        DataColumn newDc = new DataColumn(title);
                        //newDc.Caption = dlItemDef.Name;
                        dlItemDatable.Columns.Add(newDc);
                    }

                    Hashtable cond = new Hashtable();

                    IList<DataListItemInstance> dlItemInstances = dataListInstanceService.GetDlItemInstancesByDlItemDefId(itemInstance.FormInstanceId, dlItemDef.Id);

                    for (int i = 0; i < dlItemInstances.Count; i++)
                    {
                        int currentIndex = currentCount + i;
                        DataListItemInstance dataListItemInstance = dlItemInstances[i];
                        DataRow newDataRow = null;
                        if (dlItemDatable.Rows.Count <= currentIndex)
                        {
                            newDataRow = dlItemDatable.NewRow();
                            dlItemDatable.Rows.Add(newDataRow);
                        }
                        else
                        {
                            newDataRow = dlItemDatable.Rows[currentIndex];
                        }
                        // DataRow newDataRow = dlItemDatable.Rows.Count <= currentIndex ? dlItemDatable.NewRow() : dlItemDatable.Rows[currentIndex];
                        newDataRow[title] = dlItemInstances[i].Value;
                        newDataRow["fid"] = itemInstance.FormInstanceId;

                    }


                }
            }


        }
    }
}
