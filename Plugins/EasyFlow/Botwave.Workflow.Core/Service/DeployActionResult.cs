using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 部署的动作结果类
    /// </summary>
    [Serializable]
    public class DeployActionResult : ActionResult
    {
        private Guid _workflowId;

        /// <summary>
        /// 部署后新流程定义编号.
        /// </summary>
        public Guid WorkflowId
        {
            get { return _workflowId; }
            set { _workflowId = value; }
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public DeployActionResult()
        {
            this._workflowId = Guid.Empty;
        }
    }
}
