using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.DynamicForm.Extension.Domain;

namespace Botwave.DynamicForm.Extension.Implements
{
    public interface IGetDataService
    {
        FormItemExtension GetFormItemExtensionById(Guid formItemDefinitionId);

        IList<FormItemExtension> GetFormItemExtensionSettingsByFormdefinitionId(Guid entityId);

        IList<FormItemExtension> GetFormItemExtensionSettingsByFormdefinitionIdAndGetDataType(Guid formDefinitionId, int getDataType);

        void InserFormItemExtension(FormItemExtension formItemExtension);

        int UpdateFormItemExtension(FormItemExtension formItemExtension);

        bool ExistFormItemExtension(Guid formItemDefinitionId);

        int DeleteFormItemExtensionById(Guid formItemDefinitionId);
    }
}
