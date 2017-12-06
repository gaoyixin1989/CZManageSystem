using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Workflow.Extension.Domain;

namespace Botwave.Workflow.Extension.Service
{
    /// <summary>
    /// 流程权限资源服务接口.
    /// </summary>
    public interface IWorkflowResourceService
    {
        /// <summary>
        /// 更新指定权限资源的页面上的界面可见性.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="isVisible"></param>
        int UpdateResourceVisible(string resourceId, bool isVisible);

        /// <summary>
        /// 更新指定流程的权限资源的界面可见性.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="isVisible"></param>
        int UpdateWorkflowResourceVisible(Guid workflowId, bool isVisible);

        /// <summary>
        /// 新增指定的权限资源列表,返回插入的数据数.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        int InsertResources(IList<ResourceProperty> items);

        /// <summary>
        /// 获取指定父资源下的最大权限资源编号.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        string GetMaxResourceIdByParent(string parentId);

        /// <summary>
        /// 获取指定流程名称的流程资源编号.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        string GetWorkflowResourceId(string workflowName);

        /// <summary>
        /// 获取指定流程名称的流程资源编号.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        string GetWorkflowResourceId(Guid workflowId);

        /// <summary>
        /// 获取指定信息的流程步骤（或公用）资源 ID.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="resourceName"></param>
        /// <param name="isActivityResource"></param>
        /// <returns></returns>
        string GetWorkflowResourceId(string workflowName, string resourceName, bool isActivityResource);

        /// <summary>
        /// 获取指定流程的权限资源信息(Key:资源别名 Alias).
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="isActivityResources"></param>
        /// <returns></returns>
        IDictionary<string, ResourceProperty> GetWorkflowResources(string workflowName, bool isActivityResources);

        /// <summary>
        /// 获取指定的父资源以及资源类型的权限资源字段(Key:资源别名 Alias, Value:资源编号 ResouceId).
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IDictionary<string, string> GetWorkflowResources(string parentId, string type);
    }
}
