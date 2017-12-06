using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.IBatisNet
{
    public class FormDefinitionService : Botwave.DynamicForm.Services.IFormDefinitionService
    {
        #region IFormDefinitionService Members

        public Guid SaveFormDefinition(FormDefinition definition)
        {
            if (IsFormExists(definition.Name))
            {
                definition.Version++;
            }

            definition.Id = Guid.NewGuid();

            IBatisMapper.Insert("bwdf_Definitions_Insert", definition);

            return definition.Id;
        }

        public void RemoveFormDefinition(Guid id, string actor)
        {
            FormDefinition form = new FormDefinition();
            form.Id = id;
            form.LastModifier = actor;
            form.LastModTime = DateTime.Now;
            form.Enabled = false;
            IBatisMapper.Update("bwdf_Definitions_Remove", form);
        }

        public void UpdateFormDefinitionTemplate(Guid fid, string template)
        {
            Hashtable ht = new Hashtable();
            ht.Add("Id", fid);
            ht.Add("TemplateContent", template);
            IBatisMapper.Update("bwdf_Definitions_Update_Template", ht);
        }

        public void AppendItemToForm(FormItemDefinition item)
        {
            IBatisMapper.Insert("bwdf_ItemDefinitions_Insert", item);
        }

        public void AppendItemsToForm(IList<FormItemDefinition> items)
        {
            foreach (FormItemDefinition item in items)
                AppendItemToForm(item);
        }

        public void RemoveItemFromForm(FormItemDefinition item)
        {
            IBatisMapper.Delete("bwdf_ItemDefinitions_Delete", item);
        }

        public void RemoveItemsFromForm(IList<FormItemDefinition> items)
        {
            foreach (FormItemDefinition item in items)
                RemoveItemFromForm(item);
        }

        public void UpdateItem(FormItemDefinition item)
        {
            IBatisMapper.Update("bwdf_ItemDefinitions_Update", item);
        }

        public bool IsFormExists(string formName)
        {
            IList<FormDefinition> list = IBatisMapper.Select<FormDefinition>("bwdf_Definitions_Select_By_Name", formName);
            return (list.Count > 0);
        }

        public bool IsFormExists(Guid formId)
        {
            IList<FormDefinition> list = IBatisMapper.Select<FormDefinition>("bwdf_Definitions_Select", formId);
            return (list.Count > 0);
        }

        public bool IsItemExists(Guid formId, string itemName)
        {
            Hashtable ht = new Hashtable();
            ht.Add("FormDefinitionId", formId);
            ht.Add("FName", itemName);
            IList<FormItemDefinition> list = IBatisMapper.Select<FormItemDefinition>("bwdf_ItemDefinitions_Select_By_FormIdAndName", ht);
            return (list.Count > 0);
        }

        public bool IsItemExists(string itemId)
        {
            IList<FormItemDefinition> list = IBatisMapper.Select<FormItemDefinition>("bwdf_ItemDefinitions_Select", itemId);
            return (list.Count > 0);
        }

        public FormDefinition GetFormDefinitionById(Guid formId)
        {
            IList<FormDefinition> list = IBatisMapper.Select<FormDefinition>("bwdf_Definitions_Select", formId);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        public FormDefinition GetFormDefinitionById(Guid formId, bool withItems)
        {
            IList<FormDefinition> list = IBatisMapper.Select<FormDefinition>("bwdf_Definitions_Select", formId);
            if (list.Count > 0)
            {
                if (withItems)
                {
                    list[0].Items = GetFormItemDefinitionsByFormDefinitionId(list[0].Id);
                }
                return list[0];
            }
            return null;
        }

        public FormDefinition GetFormDefinitionByName(string formName)
        {
            IList<FormDefinition> list = IBatisMapper.Select<FormDefinition>("bwdf_Definitions_Select_By_Name", formName);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        public FormDefinition GetFormDefinitionByExternalEntity(string entityType, Guid entityId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("EntityType", entityType);
            ht.Add("EntityId", entityId);
            IList<FormDefinition> list = IBatisMapper.Select<FormDefinition>("bwdf_Definitions_Select_By_ExternalEntity", ht);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        public FormDefinition GetFormDefinitionByFormInstanceId(Guid formInstanceId)
        {
            IList<FormDefinition> list = IBatisMapper.Select<FormDefinition>("bwdf_Definitions_Select_By_FormInstanceId", formInstanceId);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        public IList<FormDefinition> ListFormDefinitions(bool withItems)
        {
            IList<FormDefinition> list = IBatisMapper.Select<FormDefinition>("bwdf_Definitions_Select", null);

            if (withItems)                
            {
                foreach (FormDefinition definition in list)
                {
                    definition.Items = GetFormItemDefinitionsByFormDefinitionId(definition.Id);
                }
            }
            return list;
        }

        public void AssociateFormDefinitionWithExternalEntity(FormDefinitionsInExternals relationship, bool isInsert)
        {
            if (isInsert)
                IBatisMapper.Insert("bwdf_DefinitionInExternalEntity_Insert", relationship);

            else
            {
                if (null != GetFormDefinitionByExternalEntity(relationship.EntityType, relationship.EntityId))
                    IBatisMapper.Update("bwdf_DefinitionInExternalEntity_Update_By_EntityId", relationship);
                else
                    IBatisMapper.Update("bwdf_DefinitionInExternalEntity_Update_By_FormDefinitionId", relationship);
            }
        }

        public IList<FormItemDefinition> GetFormItemDefinitionsByFormDefinitionId(Guid formDefinitionId)
        {
            IList<FormItemDefinition> list = IBatisMapper.Select<FormItemDefinition>("bwdf_ItemDefinitions_Select_By_FormId", formDefinitionId);
            return list;
        }

        public FormItemDefinition GetFormItemDefinitionById(Guid itemId)
        {
            IList<FormItemDefinition> list = IBatisMapper.Select<FormItemDefinition>("bwdf_ItemDefinitions_Select_By_Id", itemId);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        #endregion

        #region IFormDefinitionService 成员


        public IList<FormDefinition> ListFormDefinitionsByEntityType(string entityType)
        {
            IList<FormDefinition> list = IBatisMapper.Select<FormDefinition>("bwdf_Definitions_Select_By_ExternalEntityType", entityType);
            return list;
        }
       

        public  System.Data.DataTable ListFormDefinitionByEntityType(string entityType)
        {
            string sqlText = "select * from bwdf_FormDefinitionInExternals fe Left JOIN bwdf_FormDefinitions fd  On fe.FormDefinitionId =fd.Id Where EntityType='{0}'";
            using (System.Data.DataTable dt = IBatisDbHelper.ExecuteDataset(System.Data.CommandType.Text, string.Format(sqlText, entityType)).Tables[0])
            {
                return dt;
            }
           
        }

        #endregion
    }
}
