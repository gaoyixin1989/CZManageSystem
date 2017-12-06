using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 流程部署的后续处理接口.
    /// </summary>
    public interface IPostDeployHandler
    {
        /// <summary>
        /// 下一后续处理器对象.
        /// </summary>
        IPostDeployHandler Next { get; set; }

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="oldWorkflow"></param>
        /// <param name="newWorkflow"></param>
        /// <param name="newActivities"></param>
        void Execute(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow, ICollection<DeployActivityDefinition> newActivities);
    }
}
