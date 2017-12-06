using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程设置服务接口.
    /// </summary>
    public interface IWorkflowSettingService
    {
        /// <summary>
        /// 获取指定流程的流程设置信息.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        WorkflowSetting GetWorkflowSetting(Guid workflowId);

        /// <summary>
        /// 获取指定流程名称的流程设置信息.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        WorkflowSetting GetWorkflowSetting(string workflowName);

        /// <summary>
        /// 获取指定流程别名的当前版本(最新版本)的流程定义.
        /// </summary>
        /// <param name="workflowAlias"></param>
        /// <returns></returns>
        WorkflowDefinition GetCurrentWorkflowDefinition(string workflowAlias);

        /// <summary>
        /// 插入流程配置信息.
        /// </summary>
        /// <param name="item"></param>
        void InsertSetting(WorkflowSetting item);

        /// <summary>
        /// 更新流程配置信息.
        /// </summary>
        /// <param name="item"></param>
        int UpdateSetting(WorkflowSetting item);

        /// <summary>
        /// 检查是否存在指定名称的流程设置信息.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        bool ExistsSetting(string workflowName);
    }
}
