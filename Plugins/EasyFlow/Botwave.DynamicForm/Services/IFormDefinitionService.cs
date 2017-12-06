using System;
using System.Collections.Generic;
using System.Text;
using Botwave.DynamicForm.Domain;
using System.Data;

namespace Botwave.DynamicForm.Services
{
    /// <summary>
    /// 表单定义服务接口，对表单定义以及表单项定义进行数据操作.
    /// </summary>
    public interface IFormDefinitionService
    {
        /// <summary>
        /// 创建表单定义.
        /// 如果存在,则版本加1,并设为当前版本.
        /// </summary>
        /// <param name="definiton"></param>
        /// <returns></returns>
        Guid SaveFormDefinition(FormDefinition definiton);

        /// <summary>
        /// 删除表单定义.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actor"></param>
        void RemoveFormDefinition(Guid id, string actor);

        /// <summary>
        /// 更新表单模板内容.
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="template"></param>
        void UpdateFormDefinitionTemplate(Guid fid, string template);

        /// <summary>
        /// 追加表单项到表单.
        /// </summary>
        /// <param name="item"></param>
        void AppendItemToForm(FormItemDefinition item);

        /// <summary>
        /// 追加表单项到表单.
        /// </summary>
        /// <param name="items"></param>
        void AppendItemsToForm(IList<FormItemDefinition> items);

        /// <summary>
        /// 从表单中移除表单项.
        /// </summary>
        /// <param name="item"></param>
        void RemoveItemFromForm(FormItemDefinition item);

        /// <summary>
        /// 从表单中移除表单项.
        /// </summary>
        /// <param name="items"></param>
        void RemoveItemsFromForm(IList<FormItemDefinition> items);

        /// <summary>
        /// 更新表单项.
        /// </summary>
        /// <param name="item"></param>
        void UpdateItem(FormItemDefinition item);

        /// <summary>
        /// 表单是否存在.
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        bool IsFormExists(string formName);

        /// <summary>
        /// 表单是否存在.
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        bool IsFormExists(Guid formId);

        /// <summary>
        /// 表单项是否存在.
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        bool IsItemExists(Guid formId, string itemName);

        /// <summary>
        /// 表单项是否存在.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        bool IsItemExists(string itemId);

        /// <summary>
        /// 获取表单定义.
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        FormDefinition GetFormDefinitionById(Guid formId);

        /// <summary>
        /// 获取表单项定义列表.
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="withItems">是否需要级联表单项.</param>
        /// <returns></returns>
        FormDefinition GetFormDefinitionById(Guid formId, bool withItems);

        /// <summary>
        /// 获取表单定义.
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        FormDefinition GetFormDefinitionByName(string formName);

        /// <summary>
        /// 获取表单定义.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        FormDefinition GetFormDefinitionByExternalEntity(string entityType, Guid entityId);

        /// <summary>
        /// 获取指定表单实例编号的表单定义对象.
        /// </summary>
        /// <param name="formInstanceId"></param>
        /// <returns></returns>
        FormDefinition GetFormDefinitionByFormInstanceId(Guid formInstanceId);

        /// <summary>
        /// 获取表单定义列表.
        /// </summary>
        /// <param name="withItems">是否需要级联表单项.</param>
        /// <returns></returns>
        IList<FormDefinition> ListFormDefinitions(bool withItems);

        /// <summary>
        /// 关联表单定义与外部实体.
        /// </summary>
        /// <param name="relationship"></param>
        /// <param name="isInsert"></param>
        void AssociateFormDefinitionWithExternalEntity(FormDefinitionsInExternals relationship, bool isInsert);

        /// <summary>
        /// 根据表单定义Id获取表单项列表.
        /// </summary>
        /// <param name="formDefinitionId"></param>
        /// <returns></returns>
        IList<FormItemDefinition> GetFormItemDefinitionsByFormDefinitionId(Guid formDefinitionId);

        /// <summary>
        /// 根据表单项Id获取表单项定义.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        FormItemDefinition GetFormItemDefinitionById(Guid itemId);

        /// <summary>
        /// 根据实体类型获取表单定义列表
        /// </summary>
        /// <param name="entityType">实体类型.</param>
        /// <returns></returns>
        IList<FormDefinition> ListFormDefinitionsByEntityType(string entityType);

        /// <summary>
        /// 根据实体类型返回表单的类表以及表单关联的关系
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>返回表单定义及与表单关联的DataTable</returns>
        DataTable ListFormDefinitionByEntityType(string entityType);
    }
}
