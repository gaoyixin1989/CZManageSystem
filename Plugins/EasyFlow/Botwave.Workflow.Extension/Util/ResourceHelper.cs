using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.Extension.Util
{
    /// <summary>
    /// 流程权限资源辅助类.
    /// </summary>
    public static class ResourceHelper
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
        /// 流程(名称)的权限资源的类型名称.
        /// </summary>
        public const string ResourceType_Workflow = "workflow";

        /// <summary>
        /// 流程的通用权限资源的类型名称.
        /// </summary>
        public const string ResourceType_Common = "workflow_common";

        /// <summary>
        /// 流程步骤的权限资源类型名称.
        /// </summary>
        public const string ResourceType_Activity = "workflow_activity";

        /// <summary>
        /// 通用流程资源名称数组.
        /// *0000: 流程协作;
        /// *0001: 提单;
        /// *0002: 报表统计;
        /// *0003: 查看保密单;
        /// *0004: 高级查询;
        /// </summary>
        public static string[] Resources_WorkflowCommons = new string[] { 
            "流程协作", "提单","报表统计","查看保密单","高级查询" };

        #endregion

        #region workflow resources

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
        /// <param name="workflowResourceId"></param>
        /// <returns></returns>
        public static int GetResourceActivityValue(string workflowResourceId)
        {
            if (string.IsNullOrEmpty(workflowResourceId) || workflowResourceId.Length < 12)
                return -1;
            return Convert.ToInt32(workflowResourceId.Substring(8, 4));
        }

        #endregion
    }
}
