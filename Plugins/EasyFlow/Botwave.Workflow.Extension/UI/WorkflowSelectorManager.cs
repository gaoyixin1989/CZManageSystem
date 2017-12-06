using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Botwave.Workflow.Extension.UI
{
    /// <summary>
    /// 管理类.
    /// </summary>
    public sealed class WorkflowSelectorManager
    {
        /// <summary>
        /// 流程选择器名值集合.
        /// </summary>
        private static readonly NameValueCollection profileNames = GetProfileNames();

        /// <summary>
        /// 获取流程选择器配置名值集合.
        /// </summary>
        /// <returns></returns>
        public static NameValueCollection GetProfileNames()
        {
            object section = ConfigurationManager.GetSection("botwave.workflowSelector");
            if(section == null)
                section = ConfigurationManager.GetSection("botwave/workflowSelector");
            NameValueCollection results = new NameValueCollection(StringComparer.CurrentCultureIgnoreCase);
            NameValueCollection config = section as NameValueCollection;
            foreach (string key in config.Keys)
            {
                results[key] = config[key];
            }
            return results;
        }
        
        /// <summary>
        /// 获取指定流程名称的流程选择器类型名称.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static string GetProfileName(string workflowName)
        {
            if (string.IsNullOrEmpty(workflowName))
                return string.Empty;
            return profileNames[workflowName];
        }

        /// <summary>
        /// 获取指定流程编号的流程选择器类型名称.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static string GetProfileName(Guid workflowId)
        {
            if (workflowId == Guid.Empty)
                return string.Empty;
            string workflowName = Botwave.Workflow.Extension.Util.WorkflowUtility.GetWorkflowName(workflowId);
            return GetProfileName(workflowName);
        }
    }
}
