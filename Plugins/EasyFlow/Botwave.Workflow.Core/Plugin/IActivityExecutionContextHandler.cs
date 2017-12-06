using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// ActivityExecutionContext 上下文对象变量处理接口.
    /// </summary>
    public interface IActivityExecutionContextHandler
    {
        /// <summary>
        /// 处理设置 ActivityExecutionContext 对象.
        /// </summary>
        /// <param name="executionContext">当前 ActivityExecutionContext 对象.</param>
        /// <param name="request">HttpRequest 请求对象.</param>
        /// <param name="workflowInstanceId">流程实例编号.</param>
        void Handle(ActivityExecutionContext executionContext, HttpRequest request, Guid workflowInstanceId);
    }
}
