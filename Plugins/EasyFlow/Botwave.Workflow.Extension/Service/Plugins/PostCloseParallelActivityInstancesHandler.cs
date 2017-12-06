using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 关闭并行流程活动实例的后续处理器实现类.
    /// </summary>
    public class PostCloseParallelActivityInstancesHandler : IPostCloseParallelActivityInstancesHandler
    {
        #region IPostCloseParallelActivityInstancesHandler 成员

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="currentActivityInstance"></param>
        /// <param name="parallelInstances"></param>
        public void Execute(ActivityInstance currentActivityInstance, ICollection<ActivityInstance> parallelInstances)
        {
            foreach (ActivityInstance instance in parallelInstances)
            {
                if (!instance.IsCompleted && instance.ActivityInstanceId != currentActivityInstance.ActivityInstanceId)
                {
                    // 执行关闭处理
                }
            }
        }

        #endregion
    }
}
