using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Allocator
{
    /// <summary>
    /// 任务分派器管理服务的默认实现类.
    /// </summary>
    public class DefaultTaskAllocatorManager : ITaskAllocatorManager
    {
        private IDictionary<string, ITaskAllocator> taskAllocators = new Dictionary<string, ITaskAllocator>();

        /// <summary>
        /// 适配spring
        /// srping中的dictionary的类型为HybridDictionary
        /// </summary>
        public System.Collections.Specialized.HybridDictionary AllocatorDict
        {
            set
            {
                if (null != value && value.Count > 0)
                {
                    foreach (string key in value.Keys)
                    {
                        string allocatorName = key.ToLower(System.Globalization.CultureInfo.InvariantCulture);

                        //直接转换，如果有异常则说明配置不正确
                        taskAllocators.Add(allocatorName, (ITaskAllocator)(value[allocatorName]));
                    }
                }
            }
        }

        #region ITaskAllocatorManager Members

        /// <summary>
        /// 获取指定名称的任务分派器对象.
        /// </summary>
        /// <param name="allocator"></param>
        /// <returns></returns>
        public ITaskAllocator GetTaskAllocator(string allocator)
        {
            if (taskAllocators.ContainsKey(allocator))
            {
                return taskAllocators[allocator];
            }
            return null;
        }

        /// <summary>
        /// 获取指定表达式的任务分派器对象列表.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IList<ITaskAllocator> GetTaskAllocators(TaskAllocatorExpression expression)
        {
            IList<ITaskAllocator> list = new List<ITaskAllocator>();
            foreach (string key in expression.Allocators.Keys)
            {
                if (taskAllocators.ContainsKey(key))
                {
                    list.Add(taskAllocators[key]);
                }
            }
            return list;
        }

        #endregion
    } 
}
