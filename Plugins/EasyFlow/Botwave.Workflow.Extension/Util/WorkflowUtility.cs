using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;

namespace Botwave.Workflow.Extension.Util
{
    /// <summary>
    /// Workflow 辅助类.
    /// </summary>
    public static class WorkflowUtility
    {
        #region fields

        /// <summary>
        /// 流程附件的实体关系类型名称.
        /// </summary>
        public const string EntityType_WorkflowAttachment = "W_A";

        /// <summary>
        /// 流程模板附件的实体关系类型名称.
        /// </summary>
        public const string EntityType_WorkflowTemplate = "W_T";

        /// <summary>
        /// 流程表单的实体管理类型名称.
        /// </summary>
        public const string EntityType_WorkflowForm = "Form_Workflow";

        /// <summary>
        /// 流程表单的实体类型名称§IIS§
        /// </summary>
        public const string EntityType_IISForm = "Form_IIS";

        /// <summary>
        /// WorkflowService 的对象名称.
        /// </summary>
        public static string Object_WorkflowService = "workflowService";

        /// <summary>
        /// WorkflowUserService 的对象名称.
        /// </summary>
        public static string Object_WorkflowUserService = "workflowUserService";

        #endregion

        #region service proepties

        private static IWorkflowResourceService workflowResourceService;

        /// <summary>
        /// 流程权限资源服务.
        /// </summary>
        public static IWorkflowResourceService WorkflowResourceService
        {
            get { return workflowResourceService; }
            set { workflowResourceService = value; }
        }

        #endregion

        /// <summary>
        /// 静态构造方法.
        /// </summary>
        static WorkflowUtility()
        {
            workflowResourceService = Spring.Context.Support.WebApplicationContext.Current["workflowResourceService"] as IWorkflowResourceService;
        }

        #region Format

        private const char ActorSeparator = '/';

        /// <summary>
        /// 格式化流程执行人名称(可为:用户名/用户真实姓名,如：admin/系统管理员).
        /// 将其转换为有 Tooltip 的字符串.
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static string FormatWorkflowActor(string actor)
        {
            if (actor.IndexOf(ActorSeparator) == -1)
                return actor;
            string[] actorArray = actor.Split(',', '，');
            StringBuilder builder = new StringBuilder();
            foreach (string item in actorArray)
            {
                int index = item.LastIndexOf('/');
                if (index < 1 || index >= item.Length - 1)
                    builder.Append(item + ",");
                else
                    builder.AppendFormat("<span tooltip=\"{0}\">{1}</span>,", item.Substring(0, index), item.Substring(index + 1));
            }
            if (builder.Length > 0)
                builder.Length = builder.Length - 1;
            return builder.ToString();
        }

        /// <summary>
        /// 格式化流程执行人名称(可为:用户名/用户真实姓名,如：admin/系统管理员).
        /// 将其转换为有 Tooltip 的字符串.
        /// </summary>
        /// <param name="todoActors"></param>
        /// <param name="excludeActor">排除用户名.</param>
        /// <returns></returns>
        public static string FormatWorkflowActor(string todoActors, string excludeActor)
        {
            if (todoActors.IndexOf(ActorSeparator) == -1)
                return todoActors;
            string[] actorArray = todoActors.Split(',', '，');
            StringBuilder builder = new StringBuilder();
            bool hasExclude = string.IsNullOrEmpty(excludeActor);
            foreach (string item in actorArray)
            {
                int index = item.LastIndexOf('/');
                if (index < 1 || index >= item.Length - 1)
                {
                    builder.Append(item + ",");
                }
                else
                {
                    string username = item.Substring(0, index);
                    if (!hasExclude)
                    {
                        // 排除(不显示)指定用户
                        hasExclude = username.Equals(excludeActor, StringComparison.OrdinalIgnoreCase);
                        if (hasExclude)
                            continue;
                    }
                    builder.AppendFormat("<span tooltip=\"{0}\">{1}</span>,", username, item.Substring(index + 1));
                }
            }
            if (builder.Length > 0)
                builder.Length = builder.Length - 1;
            return builder.ToString();
        }
        #endregion

        #region resource filters

        /// <summary>
        /// 获取允许的流程列表.
        /// </summary>
        /// <param name="workflows"></param>
        /// <param name="userResources"></param>
        /// <param name="resourcePostfix"></param>
        /// <returns></returns>
        public static IList<WorkflowDefinition> GetAllowedWorkflows(IList<WorkflowDefinition> workflows, IDictionary<string, string> userResources, string resourcePostfix)
        {
            return GetAllowedWorkflows(
                workflows, 
                workflowResourceService.GetWorkflowResources(ResourceHelper.Workflow_ResourceId, ResourceHelper.ResourceType_Workflow), 
                userResources, 
                resourcePostfix
                );
        }

        /// <summary>
        /// 获取允许的流程列表.
        /// </summary>
        /// <param name="workflows"></param>
        /// <param name="workflowResourceDict"></param>
        /// <param name="userResources"></param>
        /// <param name="resourcePostfix"></param>
        /// <returns></returns>
        public static IList<WorkflowDefinition> GetAllowedWorkflows(IList<WorkflowDefinition> workflows, IDictionary<string, string> workflowResourceDict, IDictionary<string, string> userResources, string resourcePostfix)
        {
            if (workflows == null)
                return workflows;
            for (int i = 0; i < workflows.Count; i++)
            {
                string workflowName = workflows[i].WorkflowName.ToLower();
                if (workflowResourceDict.ContainsKey(workflowName))
                {
                    string resourceId = workflowResourceDict[workflowName] + resourcePostfix;
                    if (!userResources.ContainsKey(resourceId))
                    {
                        workflows.RemoveAt(i);
                        i--;
                    }
                }
            }
            return workflows;
        }

        #endregion

        #region 数据操作.
        
        /// <summary>
        /// 获取指定流程实例的全部处理人用户名列表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static IList<string> GetWorkflowProcessors(Guid workflowInstanceId)
        {
            IList<string> results = new List<string>();
            string sql = string.Format("SELECT DISTINCT Actor FROM bwwf_Tracking_Activities_Completed WHERE WorkflowInstanceId = '{0}'", workflowInstanceId);
            using (IDataReader reader = IBatisDbHelper.ExecuteReader(CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    //对于强制完成、退回等操作，不一定有操作人

                    if (!reader.IsDBNull(0))
                    {
                        string actor = reader.GetString(0);
                        if (!results.Contains(actor))
                            results.Add(actor);
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// 获取指定流程编号的流程名称.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static string GetWorkflowName(Guid workflowId)
        {
            return IBatisMapper.Mapper.QueryForObject<string>("bwwf_Workflows_Select_WfName_ByWorkflowId", workflowId);
        }

        #endregion

        #region 其他方法

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

        #endregion
    }
}
