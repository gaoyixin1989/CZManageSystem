using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Extension.Service.Support
{
    /// <summary>
    /// 流程表单服务的空实现类.
    /// </summary>
    public class EmptyWorkflowFormService : IWorkflowFormService
    {
        #region IDynamicFormService 成员

        /// <summary>
        /// 获取请求的表单变量字典.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetFormVariables(HttpRequest request)
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// 创建表单实例.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="creator"></param>
        public void CreateFormInstance(Guid workflowId, Guid workflowInstanceId, string creator)
        {

        }

        /// <summary>
        /// 加载表单数据.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="binderVariables"></param>
        /// <returns></returns>
        public string LoadForm(Guid workflowId, Guid workflowInstanceId, IDictionary<string, object> binderVariables)
        {
            return string.Empty;
        }

        /// <summary>
        /// 保存表单数据.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="actor"></param>
        /// <param name="dataVariables"></param>
        public void SaveForm(Guid workflowInstanceId, string actor, IDictionary<string, object> dataVariables)
        {

        }

        /// <summary>
        /// 保存表单数据.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="actor"></param>
        /// <param name="dataVariables"></param>
        /// <param name="binderVariables"></param>
        public void SaveForm(Guid workflowId, Guid workflowInstanceId, string actor, IDictionary<string, object> dataVariables, IDictionary<string, object> binderVariables)
        {

        }

        /// <summary>
        /// 部署表单.
        /// </summary>
        /// <param name="oldWorkflow"></param>
        /// <param name="newWorkflow"></param>
        public void DeployForm(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow)
        {

        }

        #endregion
    }
}
