using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 流程实例创建控制器类.
    /// </summary>
    public class WorkflowInstanceCreationController : IWorkflowInstanceCreationController
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowInstanceCreationController));

        #region IWorkflowInstanceCreationController 成员

        /// <summary>
        /// 验证能否发单.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="actor"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool CanCreate(Guid workflowId, string actor, object args)
        {
            return true;
        }

        /// <summary>
        /// 验证能否发单.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="actor"></param>
        /// <param name="urgency"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool CanCreate(Guid workflowId, string actor, int urgency, object args)
        {
            return true;
        }

        #endregion
    }
}
