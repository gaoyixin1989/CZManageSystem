using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Service;

namespace Botwave.XQP.Util
{
    public class CZWorkflowUtility
    {
        private static IWorkflowResourceService workflowResourceService;
        #region resource filters


        /// <summary>
        /// 获取允许的流程列表.
        /// </summary>
        /// <param name="workflows"></param>
        /// <param name="userResources"></param>
        /// <param name="resourcePostfixs">权限资源后缀数组.</param>
        /// <returns></returns>
        public static IList<WorkflowDefinition> GetAllowedWorkflows(IList<WorkflowDefinition> workflows, IDictionary<string, string> userResources, params string[] resourcePostfixs)
        {
            workflowResourceService = Spring.Context.Support.WebApplicationContext.Current["workflowResourceService"] as IWorkflowResourceService;
            return GetAllowedWorkflows(
                workflows,
                //workflowResourceService.GetWorkflowResources(ResourceHelper.Workflow_ResourceId, ResourceHelper.ResourceType_Workflow),
                workflowResourceService.GetWorkflowResources("11", "workflow"),
                userResources,
                resourcePostfixs
                );
        }

        /// <summary>
        /// 获取允许的流程列表.
        /// </summary>
        /// <param name="workflows"></param>
        /// <param name="workflowResourceDict"></param>
        /// <param name="userResources"></param>
        /// <param name="resourcePostfixs">权限资源后缀数组.</param>
        /// <returns></returns>
        public static IList<WorkflowDefinition> GetAllowedWorkflows(IList<WorkflowDefinition> workflows, IDictionary<string, string> workflowResourceDict, IDictionary<string, string> userResources, params string[] resourcePostfixs)
        {
            if (workflows == null || resourcePostfixs == null || resourcePostfixs.Length == 0)
                return workflows;
            IList<WorkflowDefinition> items = new List<WorkflowDefinition>();
            for (int i = 0; i < workflows.Count; i++)
            {
                string workflowName = workflows[i].WorkflowName.ToLower();
                if (workflowResourceDict.ContainsKey(workflowName))
                {
                    bool isAllowed = false;
                    foreach (string postfix in resourcePostfixs)
                    {
                        string resourceId = workflowResourceDict[workflowName] + postfix;
                        if (userResources.ContainsKey(resourceId))
                        {
                            isAllowed = true;
                            items.Add(workflows[i]);
                            break;
                        }
                    }

                    //if (!isAllowed)
                    //{
                    //    workflows.RemoveAt(i);
                    //    i--;
                    //}
                }
            }
            return items;
        }

        /// <summary>
        /// 是否具备高级查询（管理员）权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="advanceResource"></param>
        /// <returns></returns>
        public static bool HasAdvanceSearch(Botwave.Security.LoginUser user, string advanceResource)
        {
            if (user == null)
                return false;
            if (string.IsNullOrEmpty(advanceResource))
                return true;

            bool result = false;
            if (user.Properties.ContainsKey("Search_AdvanceSearch_Enable"))
            {
                result = Convert.ToBoolean(user.Properties["Search_AdvanceSearch_Enable"]);
            }
            else
            {
                result = user.Resources.ContainsKey(advanceResource);
                user.Properties["Search_AdvanceSearch_Enable"] = result;
            }
            return result;
        }
        #endregion
    }
}
