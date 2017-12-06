using System;
using System.Collections.Generic;
using System.Text;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Services
{
    /// <summary>
    /// 表单实例服务接口.
    /// </summary>
    public interface IFormInstanceService
    {
        /// <summary>
        /// 创建表单实例.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="fromDefinitionId"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        Guid CreateFormInstance(Guid Id, Guid fromDefinitionId, string actor);

        /// <summary>
        /// 更新表单实例.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actor"></param>
        void UpdateFormInstance(Guid id, string actor);

        /// <summary>
        /// 删除表单实例.
        /// </summary>
        /// <param name="formInstanceId"></param>
        void RemoveFormInstance(Guid formInstanceId);

        /// <summary>
        /// 获取表单实例.
        /// </summary>
        /// <param name="formInstanceId"></param>
        /// <returns></returns>
        FormInstance GetFormInstanceById(Guid formInstanceId);

        /// <summary>
        /// 获取表单实例.
        /// </summary>
        /// <param name="formInstanceId"></param>
        /// <param name="withItems">是否需要级联表单项实例.</param>
        /// <returns></returns>
        FormInstance GetFormInstanceById(Guid formInstanceId, bool withItems);

        /// <summary>
        /// 根据表单实例Id获取表单项实例.
        /// </summary>
        /// <param name="formInstanceId"></param>
        /// <returns></returns>
        IList<FormItemInstance> GetFormItemInstancesByFormInstanceId(Guid formInstanceId);

        /// <summary>
        /// 根据表单实例Id获取表单项实例.
        /// </summary>
        /// <param name="formInstanceId"></param>
        /// <param name="withDefinitions">是否需要级联表单项定义.</param>
        /// <returns></returns>
        IList<FormItemInstance> GetFormItemInstancesByFormInstanceId(Guid formInstanceId, bool withDefinitions);

        /// <summary>
        /// 批量更新表单项实例.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        void UpdateFormItemInstance(IList<FormItemInstance> items);

        /// <summary>
        /// 根据表单定义ID，获取表单实例列表.
        /// </summary>
        /// <param name="formDefinitionId"></param>
        /// <returns></returns>
        IList<FormInstance> ListFormInstanceByFormDefinitionId(Guid formDefinitionId);

        /// <summary>
        /// 保存表单内容.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formContext"></param>
        /// <param name="actor"></param>
        void SaveForm(Guid id, FormContext formContext, string actor);

        /// <summary>
        /// 保存表单内容.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contextVariables"></param>
        /// <param name="actor"></param>
        void SaveForm(Guid id, IDictionary<string, object> contextVariables, string actor);
    }
}
