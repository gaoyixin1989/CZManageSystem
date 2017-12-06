using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Allocator
{
    /// <summary>
    /// 任务分派表达式.
    /// </summary>
    public class TaskAllocatorExpression
    {
        private IDictionary<string, object> dict = new Dictionary<string, object>();
        private string defaultAllocator;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public TaskAllocatorExpression()
        {
            dict.Add("resource", null);
            dict.Add("users", null);
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="extAllocators"></param>
        public TaskAllocatorExpression(string extAllocators)
            : this()
        {
            if (!string.IsNullOrEmpty(extAllocators))
            {
                string[] allocatorArray = extAllocators.ToLower(System.Globalization.CultureInfo.InvariantCulture).Replace(" ", "").Split(',', '，');
                foreach (string allocatorName in allocatorArray)
                {
                    if (allocatorName.Length > 0 && !dict.ContainsKey(allocatorName))
                    {
                        dict.Add(allocatorName, null);
                    }
                }
            }
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="extAllocators"></param>
        /// <param name="defaultAllocator"></param>
        public TaskAllocatorExpression(string extAllocators, string defaultAllocator)
            : this(extAllocators)
        {
            if (!String.IsNullOrEmpty(defaultAllocator))
            {
                this.defaultAllocator = defaultAllocator.ToLower(System.Globalization.CultureInfo.InvariantCulture);
            }            
        }

        /// <summary>
        /// 默认的任务分派实例名称.
        /// </summary>
        public string DefaultAllocator
        {
            get { return defaultAllocator; }
        }

        /// <summary>
        /// 任务分派实例集合字典.
        /// </summary>
        public IDictionary<string, object> Allocators
        {
            get { return dict; }
        }

        /// <summary>
        /// 获取指定分派实例的参数字符串.
        /// </summary>
        /// <param name="extAllocatorArgs">全部分派实例的参数字符串.</param>
        /// <param name="allocator">指定分派实例.</param>
        /// <returns></returns>
        public static string GetAllocatorArgument(string extAllocatorArgs, string allocator)
        {
            if (string.IsNullOrEmpty(extAllocatorArgs) || string.IsNullOrEmpty(allocator))
                return string.Empty;

            string[] argExpressions = extAllocatorArgs.Replace(" ", "").Split(';', '；');
            foreach (string expression in argExpressions)
            {
                string[] allocatorArray = expression.Split(':', '：' );
                if (allocatorArray.Length != 2)
                    continue;
                if (allocatorArray[0] == allocator)
                    return allocatorArray[1];
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取分派任务实例参数字典.
        /// </summary>
        /// <param name="extAllocatorArgs">全部分派实例的参数字符串.</param>
        /// <param name="actor">处理人.</param>
        /// <param name="workflowInstanceId">流程实例编号.</param>
        /// <returns></returns>
        public static IDictionary<string, TaskVariable> GetAllocatorArgument(string extAllocatorArgs, string actor, string workflowInstanceId)
        {
            return GetAllocatorArgument(extAllocatorArgs, actor, workflowInstanceId, new Dictionary<string, object>());
        }

        /// <summary>
        /// 获取分派任务实例参数字典.
        /// </summary>
        /// <param name="extAllocatorArgs">全部分派实例的参数字符串.</param>
        /// <param name="actor">处理人.</param>
        /// <param name="workflowInstanceId">流程实例编号.</param>
        /// <param name="variableProperties">便利属性字典.</param>
        /// <returns></returns>
        public static IDictionary<string, TaskVariable> GetAllocatorArgument(string extAllocatorArgs, string actor, string workflowInstanceId, IDictionary<string, object> variableProperties)
        {
            IDictionary<string, TaskVariable> dict = new Dictionary<string, TaskVariable>();
            if (string.IsNullOrEmpty(extAllocatorArgs))
                return dict;

            string[] allocatorArgs = extAllocatorArgs.Replace(" ", "").Split(';', '；');
            foreach (string allocatorArg in allocatorArgs)
            {
                string[] allocatorArray = allocatorArg.Split(':', '：');
                int lengthOfAllocatorArray = allocatorArray.Length;
                if (lengthOfAllocatorArray == 0)
                    continue;

                string name = allocatorArray[0].ToLower(System.Globalization.CultureInfo.InvariantCulture);

                TaskVariable variable = new TaskVariable(workflowInstanceId, actor);

                string expression = lengthOfAllocatorArray > 1 ? allocatorArray[1] : string.Empty;
                variable.Expression = expression;
                IList<object> args = new List<object>();
                if (expression.Length > 0)
                {
                    string[] argsArray = expression.Split(',', '，');
                    foreach (string item in argsArray)
                    {
                        if (!string.IsNullOrEmpty(item))
                            args.Add(item);
                    }
                }
                variable.Args = args;
                variable.Properties = variableProperties;

                if (!dict.ContainsKey(name))
                    dict.Add(name, variable);
            }
            return dict;
        }
    }
}
