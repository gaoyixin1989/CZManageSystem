using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 关闭并行流程活动(步骤)实例的后续处理器接口.
    /// </summary>
    public interface IPostCloseParallelActivityInstancesHandler
    {
        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="currentActivityInstance"></param>
        /// <param name="parallelInstances"></param>
        void Execute(ActivityInstance currentActivityInstance, ICollection<ActivityInstance> parallelInstances);
    }
}
