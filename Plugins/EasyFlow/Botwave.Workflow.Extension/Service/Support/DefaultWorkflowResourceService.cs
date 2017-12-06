using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;

namespace Botwave.Workflow.Extension.Service.Support
{
    /// <summary>
    /// 流程权限资源服务的默认实现类.
    /// </summary>
    public class DefaultWorkflowResourceService : IWorkflowResourceService
    {
        #region IWorkflowResourceService 成员

        /// <summary>
        /// 更新指定权限资源的可见性.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="isVisible"></param>
        /// <returns></returns>
        public int UpdateResourceVisible(string resourceId, bool isVisible)
        {
            Hashtable paramters = new Hashtable();
            paramters.Add("ResourceId", resourceId);
            paramters.Add("Visible", isVisible);
            return IBatisMapper.Update("bwwf_Resources_Update_Visible", paramters);
        }

        /// <summary>
        /// 更新指定流程的权限资源可见性.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="isVisible"></param>
        /// <returns></returns>
        public int UpdateWorkflowResourceVisible(Guid workflowId, bool isVisible)
        {
            Hashtable paramters = new Hashtable();
            paramters.Add("WorkflowId", workflowId);
            paramters.Add("Visible", isVisible);
            return IBatisMapper.Update("bwwf_Resources_Update_Visible_ByWorkflowId", paramters);

        }

        /// <summary>
        /// 新增权限资源属性列表.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public int InsertResources(IList<ResourceProperty> items)
        {
            if (items == null || items.Count == 0)
                return 0;
            int count = items.Count;
            foreach (ResourceProperty item in items)
            {
                IBatisMapper.Insert("bwwf_Resources_Insert", item);
            }
            return count;
        }

        /// <summary>
        /// 获取指定父资源的最大权限资源编号.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public string GetMaxResourceIdByParent(string parentId)
        {
            object result = IBatisMapper.Mapper.QueryForObject("bwwf_Resources_Select_MaxId_ByParentId", parentId);
            if (result == null)
                return string.Empty;
            return result.ToString();
        }

        /// <summary>
        /// 获取流程权限资源编号.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public string GetWorkflowResourceId(string workflowName)
        {
            return IBatisMapper.Mapper.QueryForObject<string>("bwwf_Resources_Select_ResourceId_ByWorkflowName", workflowName);
        }

        /// <summary>
        /// 获取流程权限资源编号.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public string GetWorkflowResourceId(Guid workflowId)
        {
            return IBatisMapper.Mapper.QueryForObject<string>("bwwf_Resources_Select_ResourceId_ByWorkflowId", workflowId);
        }

        /// <summary>
        /// 获取指定流程名称与资源名称的资源编号.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="resourceName"></param>
        /// <param name="isActivityResource"></param>
        /// <returns></returns>
        public string GetWorkflowResourceId(string workflowName, string resourceName, bool isActivityResource)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowName", workflowName);
            parameters.Add("ResourceName", resourceName);
            parameters.Add("ResourceType", isActivityResource ? "workflow_activity" : "workflow_common");

            object result = IBatisMapper.Mapper.QueryForObject("bwwf_Resources_Select_ResourceId_ByWorkflows", parameters);
            return (result == null) ? string.Empty : result.ToString();
        }

        /// <summary>
        /// 获取指定流程与指定类型的流程权限资源字典.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="isActivityResources"></param>
        /// <returns></returns>
        public IDictionary<string, ResourceProperty> GetWorkflowResources(string workflowName, bool isActivityResources)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowName", workflowName);
            parameters.Add("Type", isActivityResources ? "workflow_activity" : "workflow_common");

            IList<ResourceProperty> resources = IBatisMapper.Select<ResourceProperty>("bwwf_Resources_Select_ByWorkflowNameAndType", parameters);
            IDictionary<string, ResourceProperty> results = new Dictionary<string, ResourceProperty>();
            if (resources != null || resources.Count > 0)
            {
                foreach (ResourceProperty item in resources)
                {
                    if (!results.ContainsKey(item.Alias))
                    {
                        results.Add(item.Alias, item);
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// 获取指定父资源以及类型的流程资源字典集合.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetWorkflowResources(string parentId, string type)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("ParentId", parentId);
            parameters.Add("Type", type);

            IList<ResourceProperty> resources = IBatisMapper.Select<ResourceProperty>("bwwf_Resources_Select_ByParentIdAndType", parameters);
            IDictionary<string, string> results = new Dictionary<string, string>();
            if (resources != null || resources.Count > 0)
            {
                foreach (ResourceProperty item in resources)
                {
                    if (!results.ContainsKey(item.Alias))
                    {
                        results.Add(item.Alias, item.ResourceId);
                    }
                }
            }
            return results;
        }

        #endregion
    }
}
