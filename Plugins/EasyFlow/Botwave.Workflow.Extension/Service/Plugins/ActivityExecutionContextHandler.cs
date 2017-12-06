using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Botwave.Workflow;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// ActivityExecutionContext 上下文对象变量处理的默认实现类.
    /// </summary>
    public class ActivityExecutionContextHandler : IActivityExecutionContextHandler
    {
        #region IActivityExecutionContextHandler 成员

        /// <param name="executionContext">当前 ActivityExecutionContext 对象.</param>
        /// <param name="request">HttpRequest 请求对象.</param>
        /// <param name="workflowInstanceId">流程实例编号.</param>
        public void Handle(ActivityExecutionContext executionContext, System.Web.HttpRequest request, Guid workflowInstanceId)
        {

        }

        #endregion
    }
}
