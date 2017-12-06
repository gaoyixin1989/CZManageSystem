using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Extension.Domain;

namespace Botwave.Workflow.Extension.Service
{
    /// <summary>
    /// 流程字段控制服务接口.
    /// </summary>
    public interface IWorkflowFieldService
    {
        #region field info

        /// <summary>
        /// 获取字段信息.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        FieldInfo GetField(Guid workflowId, string fieldName);

        /// <summary>
        /// 获取可控制字段（即单选框类型字段）列表.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        IList<FieldInfo> GetControllableFields(Guid workflowId);

        #endregion

        #region field control info

        /// <summary>
        /// 更新字段控制列表数据.
        /// </summary>
        /// <param name="items"></param>
        void UpdateFieldControls(IList<FieldControlInfo> items);

        /// <summary>
        /// 获取指定流程与指定步骤以及指定字段名称(fieldName)的字段控制信息.
        /// </summary>
        /// <param name="workflowName">流程名称.</param>
        /// <param name="activityName">流程步骤名称.</param>
        /// <param name="fieldName">字段名称.</param>
        /// <returns></returns>
        IList<FieldControlInfo> GetFieldControls(string workflowName, string activityName, string fieldName);

        #endregion
    }
}
