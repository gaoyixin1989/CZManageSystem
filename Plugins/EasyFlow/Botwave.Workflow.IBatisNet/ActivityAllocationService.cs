using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Botwave.Workflow;
using Botwave.Workflow.Allocator;
using Botwave.Workflow.Parser;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Extension.IBatisNet;
using IBatisNet.DataMapper;

namespace Botwave.Workflow.IBatisNet
{
    public class ActivityAllocationService : IActivityAllocationService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ActivityAllocationService));
        
        private ITaskAllocatorManager taskAllocatorManager;
        private IUserProxy userProxy;

        public ITaskAllocatorManager TaskAllocatorManager
        {
            set { taskAllocatorManager = value; }
        }

        public IUserProxy UserProxy
        {
            set { userProxy = value; }
        }

        #region IActivityAllocationService Members

        /// <summary>
        /// 获取目标处理用户.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="options"></param>
        /// <param name="actor"></param>
        /// <param name="withComment"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetTargetUsers(Guid workflowInstanceId, AllocatorOption options, string actor, bool withComment)
        {
            return GetTargetUsers(workflowInstanceId, options, actor, new Dictionary<string, object>(), withComment);
        }

        /// <summary>
        /// 获取目标处理用户.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="options"></param>
        /// <param name="actor"></param>
        /// <param name="variables"></param>
        /// <param name="withComment"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetTargetUsers(Guid workflowInstanceId, AllocatorOption options, string actor, IDictionary<string, object> variables, bool withComment)
        {
            ICollection<string> users = GetAllocationUsers(options, workflowInstanceId, actor, variables);
            IDictionary<string, string> dict = null;
            if ((null == userProxy) || (!WorkflowConfig.Instance.AllowUserProxy))
            {
                dict = new Dictionary<string, string>();
                foreach (string username in users)
                {
                    if (!dict.ContainsKey(username))
                    {
                        dict.Add(username, null);
                    }
                }
                return dict;
            }
            else
            {
                dict = userProxy.GetDistinctProxies(users);
            }
            return dict;
        }

        #endregion

        #region Privates

        /// <summary>
        /// 获取分派指定步骤任务的处理人列表.
        /// </summary>
        /// <param name="options">分派设置实例.</param>
        /// <param name="workflowInstanceId">流程实例编号.</param>
        /// <param name="currentActor">当前处理人.</param>
        /// <param name="variableProperties">变量属性字典.</param>
        /// <returns>返回被分派任务的人数.</returns>
        private ICollection<string> GetAllocationUsers(AllocatorOption options, Guid workflowInstanceId, string currentActor , IDictionary<string, object> variableProperties)
        {
            if (options == null)
                return null;

            IList<string> results = new List<string>();

            string resource = options.AllocatorResource;
            string users = options.AllocatorUsers;
            string extendAllocators = options.ExtendAllocators;
            string extendAllocatorArgs = options.ExtendAllocatorArgs;
            string defaultAllocator = options.DefaultAllocator;

            ITaskAllocator taskAllocator;
            TaskVariable variable;

            const string RESOURCE = "resource";
            const string USERS = "users";

            #region 分派

            // 资源分派
            if (!string.IsNullOrEmpty(resource) && (string.IsNullOrEmpty(defaultAllocator) || defaultAllocator == RESOURCE))
            {
                taskAllocator = taskAllocatorManager.GetTaskAllocator(RESOURCE);
                variable = new TaskVariable();
                variable.Expression = resource;
                variable.Properties = variableProperties;

                AppendTargetUsers(results, taskAllocator, variable);
            }

            // 用户分派
            if (!string.IsNullOrEmpty(users) && (string.IsNullOrEmpty(defaultAllocator) || defaultAllocator == USERS))
            {
                taskAllocator = taskAllocatorManager.GetTaskAllocator(USERS);
                variable = new TaskVariable();
                variable.Expression = users;
                variable.Properties = variableProperties;

                AppendTargetUsers(results, taskAllocator, variable);
            }

            // 扩展分派
            if (!string.IsNullOrEmpty(extendAllocators))
            {
                IDictionary<string, TaskVariable> allocatorArgDict = new Dictionary<string, TaskVariable>();
                if (!string.IsNullOrEmpty(extendAllocatorArgs))
                    allocatorArgDict = TaskAllocatorExpression.GetAllocatorArgument(extendAllocatorArgs, currentActor, workflowInstanceId.ToString(), variableProperties);
                string[] allocatorArray = extendAllocators.ToLower().Replace(" ", "").Split(',', '，');

                foreach (string allocatorName in allocatorArray)
                {
                    if (allocatorName.Length == 0
                        || allocatorName == USERS
                        || allocatorName == RESOURCE
                        || (!string.IsNullOrEmpty(defaultAllocator) && allocatorName != defaultAllocator)) // 不是默认流程
                        continue;
                    taskAllocator = taskAllocatorManager.GetTaskAllocator(allocatorName);
                    if (taskAllocator == null)
                        continue;

                    if (allocatorArgDict.ContainsKey(allocatorName))
                    {
                        variable = allocatorArgDict[allocatorName];
                    }
                    else
                    {
                        variable = new TaskVariable(workflowInstanceId.ToString(), currentActor);
                        variable.Properties = variableProperties;
                    }
                    AppendTargetUsers(results, taskAllocator, variable);
                }
            }

            #endregion

            return results;
        }

        ///// <summary>
        ///// 过滤重复项

        ///// </summary>
        ///// <param name="items"></param>
        ///// <returns></returns>
        //IDictionary<string, string> GetDistinctItems(ICollection<string> items)
        //{
        //    IDictionary<string, string> dict = new Dictionary<string, string>();
        //    foreach (string item in items)
        //    {
        //        if (!dict.ContainsKey(item))
        //        {
        //            dict.Add(item, null);
        //        }
        //    }
        //    return dict;
        //}

        /// <summary>
        /// 追加目标用户
        /// </summary>
        /// <param name="users"></param>
        /// <param name="taskAllocator"></param>
        /// <param name="variable"></param>
        static void AppendTargetUsers(IList<string> users, ITaskAllocator taskAllocator, TaskVariable variable)
        {
            if (taskAllocator != null)
            {
                IList<string> userNames = taskAllocator.GetTargetUsers(variable);
                if (userNames != null && userNames.Count > 0)
                {
                    foreach (string name in userNames)
                    {
                        users.Add(name);
                    }
                }
            }
        }

        #endregion
    }
}
