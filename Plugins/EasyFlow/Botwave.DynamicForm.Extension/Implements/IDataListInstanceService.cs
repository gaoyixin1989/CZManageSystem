using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.DynamicForm.Extension.Domain;
using System.Collections;
using System.Web;

namespace Botwave.DynamicForm.Extension.Implements
{
    public interface IDataListInstanceService
    {
        /// <summary>
        /// 根据表单ID获取datalist表单实例
        /// </summary>
        /// <param name="formInstanceId"></param>
        /// <returns></returns>
        IList<DataListItemInstance> GetDataListItemInstancesByFormInstanceId(Guid formInstanceId);

        /// <summary>
        /// 根据表单ID和表单定义ID获取datalist表单实例
        /// </summary>
        /// <param name="formInstanceId"></param>
        /// <param name="formItemDefinitionId"></param>
        /// <param name="withDefinitions"></param>
        /// <returns></returns>
        IList<DataListItemInstance> GetDataListItemInstancesByFormInstanceIdAndFormItemDefinitionId(Guid formInstanceId, Guid formItemDefinitionId, bool withDefinitions);
        /// <summary>
        /// 根据DataList定义项ID取出DataList表单项实列
        /// </summary>
        /// <param name="formInstanceId">表单实例ID</param>
        /// <param name="dlItemDefinitionId">dataList定义ID</param>
        /// <returns></returns>
        IList<DataListItemInstance> GetDlItemInstancesByDlItemDefId(Guid formInstanceId, Guid dlItemDefinitionId);
        /// <summary>
        /// 保存表单实例.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="contextVariables"></param>
        /// <returns></returns>
        void SaveDataListInstance(Guid workflowId, Guid workflowInstanceId, IDictionary<string, object> contextVariables);

        /// <summary>
        /// 保存表单实例.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="contextVariables"></param>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        void SaveDataListInstance(Guid workflowId, Guid workflowInstanceId, IDictionary<string, object> contextVariables, HttpRequest request);
    }
}
