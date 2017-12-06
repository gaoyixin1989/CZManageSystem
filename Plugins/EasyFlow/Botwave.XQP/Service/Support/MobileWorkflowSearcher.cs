using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Extension.Service;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// 高级搜索服务的手机版实现类.
    /// </summary>
    public class MobileWorkflowSearcher : IWorkflowSearcher
    {
        #region IAdvancedSearcher Members

        /// <summary>
        /// 搜索.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable Search(Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition, int pageIndex, int pageSize, ref int recordCount)
        {
            if (condition.IsProcessed == true)
                return DoComplexSearchByProcessed(condition, pageIndex, pageSize, ref recordCount);

            if (condition.IsComplex())
                return DoComplexSearch(condition, pageIndex, pageSize, ref recordCount);
            else
                return DoSimpleSearch(condition, pageIndex, pageSize, ref recordCount);
        }

        /// <summary>
        /// 复杂条件搜索.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        private static DataTable DoComplexSearch(Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition, int pageIndex, int pageSize, ref int recordCount)
        {
            //将从0开始的页码转换为从1开始的页码
            pageIndex++;
            string fieldOrder = "StartedTime desc";
            if (!String.IsNullOrEmpty(condition.OrderField))
            {
                fieldOrder = condition.OrderField;
            }
            IDbDataParameter[] paramSet = IBatisDbHelper.CreateParameterSet(13);
            paramSet[0].ParameterName = "@BeginTime";
            paramSet[0].DbType = DbType.String;
            paramSet[0].Value = condition.BeginTime;
            paramSet[1].ParameterName = "@EndTime";
            paramSet[1].DbType = DbType.String;
            paramSet[1].Value = condition.EndTime;
            paramSet[2].ParameterName = "@Title";
            paramSet[2].DbType = DbType.String;
            paramSet[2].Value = condition.Title;
            paramSet[3].ParameterName = "@SheetId";
            paramSet[3].DbType = DbType.String;
            paramSet[3].Value = condition.SheetId;
            paramSet[4].ParameterName = "@WorkflowName";
            paramSet[4].DbType = DbType.String;
            paramSet[4].Value = condition.WorkflowName;
            paramSet[5].ParameterName = "@ActivityName";
            paramSet[5].DbType = DbType.String;
            paramSet[5].Value = condition.ActivityName;
            paramSet[6].ParameterName = "@CreatorName";
            paramSet[6].DbType = DbType.String;
            paramSet[6].Value = condition.CreatorName;
            paramSet[7].ParameterName = "@ProcessorName";
            paramSet[7].DbType = DbType.String;
            paramSet[7].Value = condition.ProcessorName;
            paramSet[8].ParameterName = "@Keywords";
            paramSet[8].DbType = DbType.String;
            paramSet[8].Value = condition.Keywords;
            paramSet[9].ParameterName = "@PageIndex";
            paramSet[9].DbType = DbType.Int32;
            paramSet[9].Value = pageIndex;
            paramSet[10].ParameterName = "@PageSize";
            paramSet[10].DbType = DbType.Int32;
            paramSet[10].Value = pageSize;
            paramSet[11].ParameterName = "@FieldOrder";
            paramSet[11].DbType = DbType.String;
            paramSet[11].Value = fieldOrder;
            paramSet[12].ParameterName = "@RecordCount";
            paramSet[12].DbType = DbType.Int32;
            paramSet[12].Value = recordCount;
            paramSet[12].Direction = ParameterDirection.InputOutput;
            DataSet ds = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "bwwf_ext_mp_AdvancedSearch", paramSet);
            recordCount = Convert.ToInt32(paramSet[12].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 简单条件搜索.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        private static DataTable DoSimpleSearch(Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwwf_mp_SimpleSearch";
            string fieldKey = "WorkflowInstanceId";
            string fieldShow = "WorkflowInstanceId,WorkflowAlias,AliasImage,Title,SheetId,ActivityName,CreatorName,StartedTime,FinishedTime, CurrentActors";
            string fieldOrder = "StartedTime desc";
            StringBuilder where = new StringBuilder();
            where.AppendFormat(" StartedTime >= '{0}'", condition.BeginTime);
            where.AppendFormat(" and StartedTime <= '{0}'", condition.EndTime);
            if (!String.IsNullOrEmpty(condition.Title))
            {
                where.AppendFormat(" and Title like '%{0}%'", condition.Title);
            }
            if (!String.IsNullOrEmpty(condition.SheetId))
            {
                where.AppendFormat(" and SheetId like '%{0}%'", condition.SheetId);
            }
            if (!String.IsNullOrEmpty(condition.WorkflowName))
            {
                where.AppendFormat(" and WorkflowName in ('{0}')", condition.WorkflowName);
            }
            if (!String.IsNullOrEmpty(condition.CreatorName))
            {
                where.AppendFormat(" and CreatorName like '%{0}%'", condition.CreatorName);
            }
            if (!String.IsNullOrEmpty(condition.OrderField))
            {
                fieldOrder = condition.OrderField;
            }

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }

        /// <summary>
        /// 复杂条件搜索.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        private static DataTable DoComplexSearchByProcessed(Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition, int pageIndex, int pageSize, ref int recordCount)
        {
            //将从0开始的页码转换为从1开始的页码
            pageIndex++;

            string str = string.Empty;
            StringBuilder builder = new StringBuilder();
            if ((condition.Workflows != null) && (condition.Workflows.Count > 0))
            {
                foreach (string str2 in condition.Workflows)
                {
                    builder.AppendFormat("'{0}',", str2);
                }
                builder.Length--;
                str = builder.ToString();
            }

            string fieldOrder = "StartedTime desc";
            if (!String.IsNullOrEmpty(condition.OrderField))
            {
                fieldOrder = condition.OrderField;
            }

            IDbDataParameter[] parameterArray = IBatisDbHelper.CreateParameterSet(15);
            parameterArray[0].ParameterName = "@BeginTime";
            parameterArray[0].DbType = DbType.String;
            parameterArray[0].Value = condition.BeginTime;
            parameterArray[1].ParameterName = "@EndTime";
            parameterArray[1].DbType = DbType.String;
            parameterArray[1].Value = condition.EndTime;
            parameterArray[2].ParameterName = "@Title";
            parameterArray[2].DbType = DbType.String;
            parameterArray[2].Value = condition.Title;
            parameterArray[3].ParameterName = "@SheetId";
            parameterArray[3].DbType = DbType.String;
            parameterArray[3].Value = condition.SheetId;
            parameterArray[4].ParameterName = "@WorkflowName";
            parameterArray[4].DbType = DbType.String;
            parameterArray[4].Value = condition.WorkflowName;
            parameterArray[5].ParameterName = "@AllowedWorkflowName";
            parameterArray[5].DbType = DbType.String;
            parameterArray[5].Value = str;
            parameterArray[6].ParameterName = "@ActivityName";
            parameterArray[6].DbType = DbType.String;
            parameterArray[6].Value = condition.ActivityName;
            parameterArray[7].ParameterName = "@CreatorName";
            parameterArray[7].DbType = DbType.String;
            parameterArray[7].Value = condition.CreatorName;
            parameterArray[8].ParameterName = "@ProcessorName";
            parameterArray[8].DbType = DbType.String;
            parameterArray[8].Value = condition.ProcessorName;
            parameterArray[9].ParameterName = "@Actor";
            parameterArray[9].DbType = DbType.String;
            parameterArray[9].Value = condition.Actor;
            parameterArray[10].ParameterName = "@Keywords";
            parameterArray[10].DbType = DbType.String;
            parameterArray[10].Value = condition.Keywords;
            parameterArray[11].ParameterName = "@PageIndex";
            parameterArray[11].DbType = DbType.Int32;
            parameterArray[11].Value = pageIndex;
            parameterArray[12].ParameterName = "@PageSize";
            parameterArray[12].DbType = DbType.Int32;
            parameterArray[12].Value = pageSize;
            parameterArray[13].ParameterName = "@FieldOrder";
            parameterArray[13].DbType = DbType.String;
            parameterArray[13].Value = fieldOrder;
            parameterArray[14].ParameterName = "@RecordCount";
            parameterArray[14].DbType = DbType.Int32;
            parameterArray[14].Value = (int)recordCount;
            parameterArray[14].Direction = ParameterDirection.InputOutput;
            DataSet set = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "bwwf_ext_mp_AdvancedSearch_ByProcessed", parameterArray);
            recordCount = Convert.ToInt32(parameterArray[14].Value);
            return set.Tables[0];

        }

        /// <summary>
        /// 部门/科室搜索.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static DataTable DoOrgSearch(Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition,string dpid, int pageIndex, int pageSize, ref int recordCount)
        {
            //将从0开始的页码转换为从1开始的页码
            pageIndex++;
            string fieldOrder = "StartedTime desc";
            if (!String.IsNullOrEmpty(condition.OrderField))
            {
                fieldOrder = condition.OrderField;
            }
            IDbDataParameter[] paramSet = IBatisDbHelper.CreateParameterSet(14);
            paramSet[0].ParameterName = "@BeginTime";
            paramSet[0].DbType = DbType.String;
            paramSet[0].Value = condition.BeginTime;
            paramSet[1].ParameterName = "@EndTime";
            paramSet[1].DbType = DbType.String;
            paramSet[1].Value = condition.EndTime;
            paramSet[2].ParameterName = "@Title";
            paramSet[2].DbType = DbType.String;
            paramSet[2].Value = condition.Title;
            paramSet[3].ParameterName = "@SheetId";
            paramSet[3].DbType = DbType.String;
            paramSet[3].Value = condition.SheetId;
            paramSet[4].ParameterName = "@WorkflowName";
            paramSet[4].DbType = DbType.String;
            paramSet[4].Value = condition.WorkflowName;
            paramSet[5].ParameterName = "@ActivityName";
            paramSet[5].DbType = DbType.String;
            paramSet[5].Value = condition.ActivityName;
            paramSet[6].ParameterName = "@CreatorName";
            paramSet[6].DbType = DbType.String;
            paramSet[6].Value = condition.CreatorName;
            paramSet[7].ParameterName = "@ProcessorName";
            paramSet[7].DbType = DbType.String;
            paramSet[7].Value = condition.ProcessorName;
            paramSet[8].ParameterName = "@Keywords";
            paramSet[8].DbType = DbType.String;
            paramSet[8].Value = condition.Keywords;
            paramSet[9].ParameterName = "@DpId";
            paramSet[9].DbType = DbType.String;
            paramSet[9].Value = dpid;
            paramSet[10].ParameterName = "@PageIndex";
            paramSet[10].DbType = DbType.Int32;
            paramSet[10].Value = pageIndex;
            paramSet[11].ParameterName = "@PageSize";
            paramSet[11].DbType = DbType.Int32;
            paramSet[11].Value = pageSize;
            paramSet[12].ParameterName = "@FieldOrder";
            paramSet[12].DbType = DbType.String;
            paramSet[12].Value = fieldOrder;
            paramSet[13].ParameterName = "@RecordCount";
            paramSet[13].DbType = DbType.Int32;
            paramSet[13].Value = recordCount;
            paramSet[13].Direction = ParameterDirection.InputOutput;
            DataSet ds = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "bwwf_ext_AdvancedSearch_ByOrg", paramSet);
            recordCount = Convert.ToInt32(paramSet[13].Value);
            return ds.Tables[0];
        }
        #endregion
    }
}
