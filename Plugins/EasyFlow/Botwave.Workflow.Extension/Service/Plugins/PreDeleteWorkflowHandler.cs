using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 删除流程的前续处理器实现类.
    /// </summary>
    public class PreDeleteWorkflowHandler :  IActivityExecutionHandler
    {
        #region IActivityExecutionHandler 成员

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(ActivityExecutionContext context)
        {

        }

        #endregion
    }
}
