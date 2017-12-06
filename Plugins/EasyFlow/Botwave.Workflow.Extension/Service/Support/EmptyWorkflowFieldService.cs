using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Extension.Domain;

namespace Botwave.Workflow.Extension.Service.Support
{
    /// <summary>
    /// 流程字段服务的空实现类.
    /// </summary>
    public class EmptyWorkflowFieldService : IWorkflowFieldService
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(EmptyWorkflowFieldService));

        #region IWorkflowFieldService 成员

        /// <summary>
        /// 获取指定流程以及指定字段名称的字段信息.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public FieldInfo GetField(Guid workflowId, string fieldName)
        {
            return null;
        }

        /// <summary>
        /// 获取指定流程的可控制字段信息列表.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public IList<FieldInfo> GetControllableFields(Guid workflowId)
        {
            return new List<FieldInfo>();
        }

        /// <summary>
        /// 更新字段控制信息列表.
        /// </summary>
        /// <param name="items"></param>
        public void UpdateFieldControls(IList<FieldControlInfo> items)
        {
            if (items == null || items.Count == 0)
                return;
            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                foreach (FieldControlInfo item in items)
                {
                    if (item.Id <= 0) // -1 时，为新增
                    {
                        // 新增
                    }
                    else
                    {
                        // 更新.

                    }
                }
                IBatisMapper.Mapper.CommitTransaction();
            }
            catch
            {
                IBatisMapper.Mapper.RollBackTransaction();
            }
        }

        /// <summary>
        /// 获取指定流程活动指定字段的字段控制信息列表.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="activityName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public IList<FieldControlInfo> GetFieldControls(string workflowName, string activityName, string fieldName)
        {
            return new List<FieldControlInfo>();
        }

        #endregion
    }
}
