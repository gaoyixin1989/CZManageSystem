using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.DynamicForm.Extension.Domain;

namespace Botwave.DynamicForm.Extension.Implements
{
    public interface IDataListDefinitionService
    {
        /// <summary>
        /// 批量插入表单项
        /// </summary>
        /// <param name="items"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        void AppendItemsToForm(IList<DataListItemDefinition> items, IDictionary<string, Guid> dict);

        /// <summary>
        /// 根据表单定义ID获取datalist表单项定义
        /// </summary>
        /// <param name="formItemDefinitionId"></param>
        /// <returns></returns>
        IList<DataListItemDefinition> GetDataListItemDefinitionsByFormItemDefinitionId(Guid formItemDefinitionId);

        /// <summary>
        /// 删除datalist表单项定义
        /// </summary>
        /// <param name="items"></param>
        void RemoveItemsFromForm(IList<DataListItemDefinition> items);

        /// <summary>
        /// 更新wap模板
        /// </summary>
        /// <param name="formDefinitionId"></param>
        /// <param name="TemplateContent"></param>
        void WapTemplateContentUpdate(Guid formDefinitionId, string TemplateContent);
    }
}
