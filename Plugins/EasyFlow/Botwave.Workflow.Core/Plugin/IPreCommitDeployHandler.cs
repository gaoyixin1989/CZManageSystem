using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 提交部署数据的前续处理器接口.
    /// </summary>
    public interface IPreCommitDeployHandler
    {
        /// <summary>
        /// 下一处理器.
        /// </summary>
        IPreCommitDeployHandler Next { get; set; }

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="deployWorkflow"></param>
        /// <param name="deployActivities"></param>
        void Execute(WorkflowDefinition deployWorkflow, ICollection<DeployActivityDefinition> deployActivities);
    }
}
