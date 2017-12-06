using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Allocator
{
    /// <summary>
    /// 任务分派类型管理接口.
    /// </summary>
    public interface ITaskAllocatorManager
    {
        /// <summary>
        /// 获取指定的任务分配算符.
        /// </summary>
        /// <param name="allocator">任务分派类型名称.</param>
        /// <returns></returns>
        ITaskAllocator GetTaskAllocator(string allocator);

        /// <summary>
        /// 根据表达式获取任务分配算符列表.
        /// </summary>
        /// <param name="expression">任务分派表达式.</param>
        /// <returns></returns>
        IList<ITaskAllocator> GetTaskAllocators(TaskAllocatorExpression expression);
    }
}
