using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Security.Domain;
using Botwave.Workflow.Extension.Util;

namespace Botwave.XQP.Util
{
    /// <summary>
    /// 访问资源辅助类.
    /// </summary>
    public sealed class ResourceHelper
    {
        #region fields

        /// <summary>
        /// 不进行权限控制的分派资源前缀.使得系统找不到该资源.
        /// </summary>
        public const string PrefixDisableResource = "#NONE#_";

        /// <summary>
        /// 系统资源父资源.
        /// </summary>
        public const string System_ResourceId = "00";

        /// <summary>
        /// 流程资源父资源.
        /// </summary>
        public const string Workflow_ResourceId = "11";

        /// <summary>
        /// 通用流程资源名称数组.
        /// *0000: 流程协作;
        /// *0001: 提单;
        /// *0002: 报表统计;
        /// *0003: 查看保密单;
        /// *0004: 高级查询;
        /// *0005: 删除附件
        /// </summary>
        public static string[] Workflow_ResourceNames = new string[] { 
            "流程协作", "提单","报表统计","查看保密单","高级查询","删除附件" };

        #endregion

        #region verify resource

        /// <summary>
        /// 验证用户是否有访问指定流程资源的权限.
        /// </summary>
        /// <param name="userResources"></param>
        /// <param name="workflowAlias"></param>
        /// <returns></returns>
        public static bool VerifyWorkflowResource(IDictionary<string, string> userResources, string workflowAlias)
        {
            if (string.IsNullOrEmpty(workflowAlias))
                return true;
            if (userResources == null || userResources.Count == 0)
                return false;
            // 比较资源是否以流程别名结束.是，则表示有访问权限
            foreach (string resource in userResources.Keys)
            {
                if (resource.EndsWith(workflowAlias, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 验证用户是否有访问指定资源的权限.
        /// </summary>
        /// <param name="userResources"></param>
        /// <param name="requireResouceId"></param>
        /// <returns></returns>
        public static bool VerifyResource(IDictionary<string, string> userResources, string requireResouceId)
        {
            if (string.IsNullOrEmpty(requireResouceId))
                return true;
            if (userResources == null || userResources.Count == 0)
                return false;
            // 比较资源是否以流程别名结束.是，则表示有访问权限
            if (userResources.ContainsKey(requireResouceId))
                return true;
            return false;
        }

        /// <summary>
        /// 检查是否为一个可用资源(即不以"#NONE#_"开头).
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public static bool IsDisableResource(string resourceId)
        {
            if (string.IsNullOrEmpty(resourceId) || resourceId.StartsWith(PrefixDisableResource, StringComparison.OrdinalIgnoreCase))
                return false;
            return true;
        }

        #endregion

        #region workflow resource

        /// <summary>
        /// 格式化指定流程资源整型值.格式："WF"+"######"(流程资源长度为 6,空余位置以 0 代替).
        /// </summary>
        /// <param name="workflowValue">生成长度为 6 的数字字符串("######",空余位置以 0 代替).</param>
        /// <returns></returns>
        public static string FormatWorkflowResourceId(int workflowValue)
        {
            return string.Format("WF{0:D6}", workflowValue);
        }

        /// <summary>
        /// 格式化指定流程步骤资源整型值（包括流程整型值与流程步骤整型值）.格式："WF"+"######"(流程资源长度为 6,空余位置以 0 代替)+"####"(步骤资源长度为 4,空余位置以 0 代替).
        /// </summary>
        /// <param name="workflowValue">生成长度为 6 的数字字符串("######",空余位置以 0 代替)</param>
        /// <param name="activityValue">生成长度为 4 的数字字符串("####",空余位置以 0 代替).</param>
        /// <returns></returns>
        public static string FormatWorkflowResourceId(int workflowValue, int activityValue)
        {
            return string.Format("WF{0:D6}{1:D4}", workflowValue, activityValue);
        }

        /// <summary>
        /// 格式化指定流程步骤资源整型值（包括流程整型值与流程步骤整型值）.格式："WF"+"######"(流程资源长度为 6,空余位置以 0 代替)+"####"(步骤资源长度为 4,空余位置以 0 代替).
        /// </summary>
        /// <param name="workflowResourceId">格式为 6 的数字字符串("######",空余位置以 0 代替)的字符串.</param>
        /// <param name="activityValue">生成长度为 4 的数字字符串("####",空余位置以 0 代替).</param>
        /// <returns></returns>
        public static string FormatWorkflowResourceId(string workflowResourceId, int activityValue)
        {
            return string.Format("{0}{1:D4}", workflowResourceId, activityValue);
        }

        /// <summary>
        /// 获取指定流程资源编号的流程资源整型值.
        /// </summary>
        /// <param name="workflowResourceId"></param>
        /// <returns></returns>
        public static int GetResourceWorkflowValue(string workflowResourceId)
        {
            if (string.IsNullOrEmpty(workflowResourceId) || workflowResourceId.Length < 8)
                return -1;
            return Convert.ToInt32(workflowResourceId.Substring(2, 6));
        }

        /// <summary>
        /// 获取指定流程资源编号的流程活动（步骤）资源整型值.
        /// </summary>
        /// <param name="activityResourceId"></param>
        /// <returns></returns>
        public static int GetResourceActivityValue(string activityResourceId)
        {
            if (string.IsNullOrEmpty(activityResourceId) || activityResourceId.Length < 12)
                return -1;
            return Convert.ToInt32(activityResourceId.Substring(8, 4));
        }

        #endregion

        #region resource datatable

        /// <summary>
        /// 创建资源数据表(bw_Resources)框架.
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateResourceDataTable()
        {
            DataTable resourceTable = new DataTable("bw_Resources");
            resourceTable.Columns.Add("ResourceId", typeof(string));
            resourceTable.Columns.Add("ParentId", typeof(string));
            resourceTable.Columns.Add("Type", typeof(string));
            resourceTable.Columns.Add("Name", typeof(string));
            resourceTable.Columns.Add("Alias", typeof(string));
            resourceTable.Columns.Add("Enabled", typeof(bool));
            resourceTable.Columns.Add("Visible", typeof(bool));
            DataColumn[] primaryKey = new DataColumn[1];
            primaryKey[0] = resourceTable.Columns["ResourceId"];
            resourceTable.PrimaryKey = primaryKey;
            return resourceTable;
        }

        /// <summary>
        /// 生成资源数据表的数据行(DataRow).
        /// </summary>
        /// <param name="resourceTable"></param>
        /// <param name="resourceId"></param>
        /// <param name="parentId"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static DataRow ToResourceDataRow(DataTable resourceTable, string resourceId, string parentId, string type, string name, string alias)
        {
            DataRow row = resourceTable.NewRow();
            row["ResourceId"] = resourceId;
            row["ParentId"] = parentId;
            row["Type"] = type;
            row["Name"] = name;
            row["Alias"] = alias;
            row["Enabled"] = true;
            row["Visible"] = true;
            return row;
        }

        /// <summary>
        /// 批量复制指定资源数据表到数据库服务器上.
        /// </summary>
        /// <param name="resourceTable"></param>
        /// <returns></returns>
        public static bool BulkCopyResource(DataTable resourceTable)
        {
            if (resourceTable.Rows.Count == 0)
                return true;
            string connectionString = IBatisDbHelper.ConnectionString;
            NameValueCollection columnMappings = new NameValueCollection();
            columnMappings.Add("ResourceId", "ResourceId");
            columnMappings.Add("ParentId", "ParentId");
            columnMappings.Add("Type", "Type");
            columnMappings.Add("Name", "Name");
            columnMappings.Add("Alias", "Alias");
            columnMappings.Add("Enabled", "Enabled");
            columnMappings.Add("Visible", "Visible");
            return SqlBulkCopyHelper.ExecuteBulkCopy(connectionString, "bw_Resources", columnMappings, resourceTable);
        }

        #endregion

        #region resource dictionary

        /// <summary>
        /// 获取指定流程的流程资源编号.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static string GetWorkflowResourceId(Guid workflowId)
        {
            return IBatisMapper.Mapper.QueryForObject<string>("bwwf_Resources_Select_ResourceId_ByWorkflowId", workflowId);
        }

        /// <summary>
        /// 获取指定流程名称的流程资源编号.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static string GetWorkflowResourceId(string workflowName)
        {
            return IBatisMapper.Mapper.QueryForObject<string>("bwwf_Resources_Select_ResourceId_ByWorkflowName", workflowName);
        }

        /// <summary>
        /// 获取指定父资源的资源字典（Key:资源名称 Name, Value:资源编号 ResouceId）.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetResouceByParentId(string parentId)
        {
            return IBatisMapper.Mapper.QueryForDictionary<string, string>("bw_Resources_Select_ByParentId", parentId, "Name", "ResourceId");
        }

        /// <summary>
        /// 验证用户是否有访问指定流程以及子资源名称的权限.
        /// </summary>
        /// <param name="userResources"></param>
        /// <param name="workflowName">流程名称</param>
        /// <param name="resouceName">资源名称.</param>
        /// <param name="isActivity">是否步骤.</param>
        /// <returns></returns>
        public static bool VerifyWorkflowResource(IDictionary<string, string> userResources, string workflowName, string resouceName, bool isActivity)
        {
            if (userResources == null || userResources.Count == 0)
                return false;

            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowName", workflowName);
            parameters.Add("ResourceName", resouceName);
            parameters.Add("ResourceType", isActivity ? "workflow_activity" : "workflow_common");
            ResourceInfo resouce = IBatisMapper.Load<ResourceInfo>("bwwf_Resources_Select_ByWorkflowResouceNameAndType", parameters);
            
            if (resouce == null || userResources.ContainsKey(resouce.ResourceId))
                return true;
            return false;
        }
        #endregion

        #region othor methods

        /// <summary>
        /// 清除字符串首尾的特殊空白字符.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string TrimWhitespace(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;
            return inputString.Trim('\r', '\n', '\t', ' ');  // 去除空白
        }

        /// <summary>
        /// 获取流程处理人列表，并取得转交过任务的用户列表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static IList<string> GetWorkflowProcessors(Guid workflowInstanceId)
        {
            IList<string> results = new List<string>();

            //  获取流程处理人列表，并取得转交过任务的用户列表.
            string sql = string.Format(@"SELECT DISTINCT Actor FROM bwwf_Tracking_Activities_Completed WHERE WorkflowInstanceId = '{0}'
                        UNION
                        SELECT AssigningUser AS Actor
                        FROM bwwf_Tracking_Assignments
                        WHERE ActivityInstanceId IN(
	                        SELECT ActivityInstanceId FROM vw_bwwf_Tracking_Activities_All WHERE WorkflowInstanceId = '{0}'
                        )", workflowInstanceId);
            using (IDataReader reader = IBatisDbHelper.ExecuteReader(System.Data.CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    //对于强制完成、退回等操作，不一定有操作人
                    if (!reader.IsDBNull(0))
                        results.Add(reader.GetString(0));
                }
            }
            return results;
        }

        #endregion
    }
}
