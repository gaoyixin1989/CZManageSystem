using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 提交部署数据的前续处理类.
    /// </summary>
    public class PreCommitDeployHandler : IPreCommitDeployHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PreCommitDeployHandler));

        #region service properties

        private IWorkflowResourceService workflowResourceService;

        /// <summary>
        /// 流程权限资源服务.
        /// </summary>
        public IWorkflowResourceService WorkflowResourceService
        {
            get { return workflowResourceService; }
            set { workflowResourceService = value; }
        }

        #endregion

        #region IPreCommitDeployHandler 成员

        private IPreCommitDeployHandler next = null;

        /// <summary>
        /// 下一前续处理器对象.
        /// </summary>
        public IPreCommitDeployHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="deployWorkflow"></param>
        /// <param name="deployActivities"></param>
        public void Execute(WorkflowDefinition deployWorkflow, ICollection<DeployActivityDefinition> deployActivities)
        {
            log.Debug("deploy: PreCommitDeployHandler .");
            this.UpdateResources(deployWorkflow, deployActivities);
        }

        #endregion

        #region private methods

        /// <summary>
        /// 更新权限资源.
        /// </summary>
        /// <param name="deployWorkflow"></param>
        /// <param name="deployActivities"></param>
        private void UpdateResources(WorkflowDefinition deployWorkflow, ICollection<DeployActivityDefinition> deployActivities)
        {
            // 流程名称，流程步骤名称 都以小写形式保存到数据库.
            string workflowName = WorkflowUtility.TrimWhitespace(deployWorkflow.WorkflowName);
            // 新增的权限资源列表.
            IList<ResourceProperty> newResources = new List<ResourceProperty>();
            // 已有的流程步骤权限资源列表.
            IDictionary<string, ResourceProperty> dict = workflowResourceService.GetWorkflowResources(workflowName, true);
            if (dict == null)
                dict = new Dictionary<string, ResourceProperty>();

            #region 流程资源 ID

            string workflowResourceId = null;
            int count = dict.Count; ;
            if (count > 0)
            {
                foreach (ResourceProperty item in dict.Values)
                {
                    workflowResourceId = item.ParentId;
                    break;
                }
                #region 更新已有流程资源的可视性
                IDictionary<string, DeployActivityDefinition> dictActivities = ToDictionary(deployActivities);
                foreach (ResourceProperty item in dict.Values)
                {
                    string key = item.Alias;
                    if (dictActivities.ContainsKey(key))
                    {
                        // 存在指定资源的流程步骤
                        if (item.Visible == false)
                        {
                            workflowResourceService.UpdateResourceVisible(item.ResourceId, true);
                        }
                    }
                    else
                    {
                        if (item.Visible == true)
                        {
                            workflowResourceService.UpdateResourceVisible(item.ResourceId, false);
                        }
                    }
                }
                #endregion
            }
            else
            {
                workflowResourceId = workflowResourceService.GetWorkflowResourceId(workflowName);
            }
            #endregion

            // 初始流程步骤资源索引.
            int activityIndex = 1000; 
            if (count > 0)
            {
                #region 取得现有的流程步骤资源编号的最大值.
                foreach (ResourceProperty item in dict.Values)
                {
                    int itemIndex = ResourceHelper.GetResourceActivityValue(item.ResourceId);
                    if (itemIndex > activityIndex)
                        activityIndex = itemIndex;
                }
                #endregion
            }
            int workflowIndex = 1;
            if (!string.IsNullOrEmpty(workflowResourceId))
            {
                workflowIndex = ResourceHelper.GetResourceWorkflowValue(workflowResourceId);
            }

            // 不存在该流程的资源时
            if (string.IsNullOrEmpty(workflowResourceId))
            {
                #region 新增流程资源与流程通用资源

                string maxResourceId = workflowResourceService.GetMaxResourceIdByParent( ResourceHelper.Workflow_ResourceId);
                
                if (!string.IsNullOrEmpty(maxResourceId))
                {
                    workflowIndex = ResourceHelper.GetResourceWorkflowValue(maxResourceId);
                    workflowIndex = workflowIndex + 1;
                }

                // 新增流程定义资源.
                workflowName = workflowName.ToLower();
                workflowResourceId = ResourceHelper.FormatWorkflowResourceId(workflowIndex);

                ResourceProperty item = new ResourceProperty(workflowResourceId, ResourceHelper.Workflow_ResourceId, workflowName, workflowName);
                item.Type = ResourceHelper.ResourceType_Workflow;
                item.SortIndex = 1;
                newResources.Add(item);

                // 新增通用流程资源名称组(排序索引从 2 开始).
                string[] resourceNames = ResourceHelper.Resources_WorkflowCommons;
                if (resourceNames != null)
                {
                    int length = resourceNames.Length;
                    for (int i = 0; i < length; i++)
                    {
                        string resourceName = resourceNames[i];
                        string activityResourceId = ResourceHelper.FormatWorkflowResourceId(workflowIndex, i);

                        item = new ResourceProperty(activityResourceId, workflowResourceId, resourceName, workflowName);
                        item.Type = ResourceHelper.ResourceType_Common;
                        item.SortIndex = i + 2;
                        newResources.Add(item);
                    }
                }
                #endregion
            }

            #region 更新流程步骤(活动)资源.

            activityIndex = activityIndex + 1;
            // 流程资源的排序索引(步骤资源的排序索引从 10 开始)
            int sortIndex = 10;  
            foreach (DeployActivityDefinition item in deployActivities)
            {
                // 初始步骤与完成步骤不需要创建资源.
                // 初始步骤以资源"提单"("0000")代替.
                if (item.State == 0 || item.State == 2)
                    continue;
                sortIndex++;
                string activityName = item.ActivityName.ToLower();
                string allocatorResource = item.AllocatorResource;
                string resourceId = string.Empty;

                // 当存在权限资源，为流程步骤分派资源赋值.
                if (dict.ContainsKey(activityName))
                {
                    ResourceProperty res = dict[activityName];
                    resourceId = res.ResourceId;
                    // 看是否属于"禁用资源".
                    resourceId = (string.IsNullOrEmpty(allocatorResource) ? (ResourceHelper.PrefixDisableResource + resourceId) : resourceId);
                    item.AllocatorResource = resourceId;
                    if (item.AssignmentAllocator != null)
                    {
                        item.AssignmentAllocator.AllocatorResource = resourceId;
                    }
                    continue;
                }

                // 创建步骤的新资源
                string activityResourceId = ResourceHelper.FormatWorkflowResourceId(workflowResourceId, activityIndex);

                resourceId = (string.IsNullOrEmpty(allocatorResource) ? (ResourceHelper.PrefixDisableResource + activityResourceId) : activityResourceId);
                item.AllocatorResource = resourceId;
                if (item.AssignmentAllocator != null)
                {
                    item.AssignmentAllocator.AllocatorResource = resourceId;
                }

                ResourceProperty resActivity = new ResourceProperty(activityResourceId, workflowResourceId, activityName, workflowName);
                resActivity.Type = ResourceHelper.ResourceType_Activity;
                resActivity.SortIndex = sortIndex;
                newResources.Add(resActivity);

                activityIndex++; // 自增 ActivityIndex
            }
            #endregion

            // 批量插入流程步骤资源.
            workflowResourceService.InsertResources(newResources);
        }

        /// <summary>
        /// 将集合转换为字典类型(key:流程步骤名称).
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        private static IDictionary<string, DeployActivityDefinition> ToDictionary(ICollection<DeployActivityDefinition> activities)
        {
            if (activities == null || activities.Count == 0)
                return new Dictionary<string, DeployActivityDefinition>();
            IDictionary<string, DeployActivityDefinition> dict = new Dictionary<string, DeployActivityDefinition>();
            foreach (DeployActivityDefinition item in activities)
            {
                string activityName = item.ActivityName.ToLower();
                if (!dict.ContainsKey(activityName))
                {
                    dict.Add(activityName, item);
                }
            }
            return dict;
        }

        #endregion
    }
}
