using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.DynamicForm.Extension.Domain;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.Extension.IBatisNet;
using System.Security.Policy;
using System.Collections;

namespace Botwave.DynamicForm.Extension.Contracts
{
    public class GetDataService : IGetDataService
    {
        public FormItemExtension GetFormItemExtensionById(Guid formItemDefinitionId)
        {
            return IBatisMapper.Load<FormItemExtension>("bwdf_FormItemExtension_Select_By_Id", formItemDefinitionId);
        }

        public IList<FormItemExtension> GetFormItemExtensionSettingsByFormdefinitionId(Guid formdefinitionId)
        {
            IList<FormItemExtension> result = IBatisMapper.Select<FormItemExtension>("bwdf_FormItemExtension_Settings_Select_By_FormdefinitionId", formdefinitionId);
            if (result == null)
                return new List<FormItemExtension>();
            return result;
        }

        public IList<FormItemExtension> GetFormItemExtensionSettingsByFormdefinitionIdAndGetDataType(Guid formDefinitionId, int getDataType)
        {
            Hashtable ht = new Hashtable();
            ht.Add("FormdefinitionId", formDefinitionId);
            ht.Add("GetDataType", getDataType);
            IList<FormItemExtension> result = IBatisMapper.Select<FormItemExtension>("bwdf_FormItemExtension_Settings_Select_By_FormdefinitionId_GetDataType", ht);
            if (result == null)
                return new List<FormItemExtension>();
            return result;
        }

        public void InserFormItemExtension(FormItemExtension formItemExtension)
        {
           IBatisMapper.Insert("bwdf_FormItemExtension_Insert", formItemExtension);
        }

        public int UpdateFormItemExtension(FormItemExtension formItemExtension)
        {
            return IBatisMapper.Update("bwdf_FormItemExtension_Update", formItemExtension);
        }

        public bool ExistFormItemExtension(Guid formItemDefinitionId)
        {
            return IBatisMapper.Load<int>("bwdf_FormItemExtension_Is_Exist", formItemDefinitionId) > 0;
        }

        public int DeleteFormItemExtensionById(Guid formItemDefinitionId)
        {
            return IBatisMapper.Delete("bwdf_FormItemExtension_Delete", formItemDefinitionId);
        }
    }
}
