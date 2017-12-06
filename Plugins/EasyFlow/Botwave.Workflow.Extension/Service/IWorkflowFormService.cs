using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Extension.Service
{
    /// <summary>
    /// 流程动态表单服务接口.
    /// </summary>
    public interface IWorkflowFormService
    {
        /// <summary>
        /// 获取指定页面请求的表单变量字典.
        /// </summary>
        /// <param name="request">页面请求对象.</param>
        /// <returns></returns>
        IDictionary<string, object> GetFormVariables(HttpRequest request);

        /// <summary>
        /// 创建指定流程定义的表单实例.
        /// </summary>
        /// <param name="workflowId">流程定义编号.</param>
        /// <param name="workflowInstanceId">表单实例所属的流程实例编号.</param>
        /// <param name="creator">创建人.</param>
        void CreateFormInstance(Guid workflowId, Guid workflowInstanceId, string creator);

        /// <summary>
        /// 加载指定流程的表单.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="binderVariables">数据绑定的变量字典.</param>
        /// <returns></returns>
        string LoadForm(Guid workflowId, Guid workflowInstanceId, IDictionary<string, object> binderVariables);

        /// <summary>
        /// 保存指定流程的表单数据.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="actor"></param>
        /// <param name="dataVariables"></param>
        void SaveForm(Guid workflowInstanceId, string actor, IDictionary<string, object> dataVariables);

        /// <summary>
        ///  保存指定流程的表单数据, 并在流程完成或取消时将流程表单数据保存为内容字符串.
        /// </summary>
        /// <param name="workflowId">流程定义编号.</param>
        /// <param name="workflowInstanceId">流程实例编号.</param>
        /// <param name="actor">当前操作人.</param>
        /// <param name="dataVariables">字段值的数据字典.</param>
        /// <param name="binderVariables">绑定的变量字典.</param>
        void SaveForm(Guid workflowId, Guid workflowInstanceId, string actor, IDictionary<string, object> dataVariables, IDictionary<string, object> binderVariables);

        /// <summary>
        /// 流程部署时，对流程关联表单也进行重新设置或生成.
        /// </summary>
        /// <param name="oldWorkflow"></param>
        /// <param name="newWorkflow"></param>
        void DeployForm(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow);
    }
}
