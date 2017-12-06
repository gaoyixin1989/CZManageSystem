using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Allocator
{
    /// <summary>
    /// 任务分派接口.
    /// </summary>
    public interface ITaskAllocator
    {
        /// <summary>
        /// 获取要分配任务的目标用户列表.
        /// </summary>
        /// <param name="variable">参数变量.</param>
        /// <returns></returns>
        IList<string> GetTargetUsers(TaskVariable variable);
    }
}
