using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程定义服务接口.
    /// </summary>
    public interface IWorkflowDefinitionService
    {
        /// <summary>
        /// 获取所有的流程定义(包括未启用的流程).
        /// </summary>
        /// <returns></returns>
        IList<WorkflowDefinition> GetAllWorkflowDefinition();

        /// <summary>
        /// 获取所有的流程定义(只列出启用的流程).
        /// </summary>
        /// <returns></returns>
        IList<WorkflowDefinition> GetWorkflowDefinitionList();

        /// <summary>
        /// 获取所有的流程定义(只列出启用的流程).
        /// </summary>
        /// <returns></returns>
        DataTable GetWorkflowDefinitionTable();

        /// <summary>
        /// 获取指定流程名称的流程定义列表.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        IList<WorkflowDefinition> GetWorkflowDefinitionListByName(string workflowName);

        /// <summary>
        /// 获取指定Id的流程定义.
        /// </summary>
        /// <param name="workflowDefinitionId"></param>
        /// <returns></returns>
        WorkflowDefinition GetWorkflowDefinition(Guid workflowDefinitionId);

        /// <summary>
        /// 获取指定流程名称的当前版本(最新版本)的流程定义.
        /// </summary>
        /// <param name="workflowName">获取当前版本的流程名称.</param>
        /// <returns></returns>
        WorkflowDefinition GetCurrentWorkflowDefinition(string workflowName);

        /// <summary>
        /// 获取指定流程定义编号所属流程的当前版本(最新版本)的流程定义.
        /// </summary>
        /// <param name="workflowDefinitionId">获取当前版本的流程定义编号.</param>
        /// <returns></returns>
        WorkflowDefinition GetCurrentWorkflowDefinition(Guid workflowDefinitionId);

        /// <summary>
        /// 更新指定编号的流程定义是否启用.
        /// </summary>
        /// <param name="workflowDefinitionId"></param>
        /// <param name="enabled">指定是否启用</param>
        /// <returns></returns>
        int UpdateWorkflowEnabled(Guid workflowDefinitionId, bool enabled);

        /// <summary>
        /// 检查流程是否存在（即是否启用或最新版本）.
        /// </summary>
        /// <param name="workflowDefinitionId"></param>
        /// <returns></returns>
        bool WorkflowIsExists(Guid workflowDefinitionId);
    }
}
