using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm.Extension.Domain;
using System.Data.OracleClient;
using System.Data;
using System.Configuration;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.Commons;
using System.Data.SqlClient;

namespace Botwave.DynamicForm.Extension.Contracts
{
    public class DataListDefinitionService : IDataListDefinitionService
    {
        #region IFormDefinitionService Members

        private void AppendItemToForm(DataListItemDefinition item)
        {
            IBatisMapper.Insert("bwdf_DataListItemDefinition_Insert", item);
        }

        public void AppendItemsToForm(IList<DataListItemDefinition> items, IDictionary<string,Guid> dict)
        {
            IBatisMapper.Mapper.BeginTransaction();
            try
            {
                foreach (DataListItemDefinition item in items)
                {
                    if (dict.ContainsKey(item.FName))
                        dict.Remove(item.FName);
                    //if (IsItemExists(item.FormItemDefinitionId, item.FName))
                    if(IsItemExists(item.Id))
                        UpdateItem(item);
                    else
                        AppendItemToForm(item);
                }
                foreach (KeyValuePair<string, Guid> pair in dict)
                {
                    RemoveItemFromForm(pair.Value);
                }
                IBatisMapper.Mapper.CommitTransaction();
            }
            catch(Exception ex)
            {
                IBatisMapper.Mapper.RollBackTransaction();
                throw new AppException("保存表单DataList设置出错。"+ex.ToString());
            }
        }

        private void RemoveItemFromForm(Guid id)
        {
            IBatisMapper.Delete("bwdf_DataListItemDefinition_Delete", id);
        }

        public void RemoveItemsFromForm(IList<DataListItemDefinition> items)
        {
            foreach (DataListItemDefinition item in items)
                RemoveItemFromForm(item.Id);
        }

        private void UpdateItem(DataListItemDefinition item)
        {
            IBatisMapper.Update("bwdf_DataListItemDefinition_Update", item);
        }

        private bool IsItemExists(Guid formItemDefinitionId, string itemName)
        {
            Hashtable ht = new Hashtable();
            ht.Add("FormItemDefinitionId", formItemDefinitionId);
            ht.Add("FName", itemName);
            IList<DataListItemDefinition> list = IBatisMapper.Select<DataListItemDefinition>("bwdf_DataListItemDefinition_Select_By_FormItemDefinitionIdAndName", ht);
            return (list.Count > 0);
        }

        public bool IsItemExists(Guid itemId)
        {
            IList<DataListItemDefinition> list = IBatisMapper.Select<DataListItemDefinition>("bwdf_DataListItemDefinition_Select_By_Id", itemId);
            return (list.Count > 0);
        }

        public IList<DataListItemDefinition> GetDataListItemDefinitionsByFormItemDefinitionId(Guid formItemDefinitionId)
        {
            IList<DataListItemDefinition> list = IBatisMapper.Select<DataListItemDefinition>("bwdf_DataListItemDefinition_Select_By_FormItemDefinitionId", formItemDefinitionId);
            return list;
        }

        //public DataListItemDefinition GetFormItemDefinitionById(Guid itemId)
        //{
        //    IList<DataListItemDefinition> list = IBatisMapper.Select<DataListItemDefinition>("bwdf_ItemDefinitions_Select_By_Id", itemId.ToString().ToUpper());
        //    if (list.Count > 0)
        //    {
        //        return list[0];
        //    }
        //    return null;
        //}
        public void WapTemplateContentUpdate(Guid formDefinitionId, string TemplateContent)
        {
            SqlParameter[] pa = new SqlParameter[2];
            pa[0] = new SqlParameter("@formDefinitionId", SqlDbType.UniqueIdentifier);
            pa[0].Value = formDefinitionId;
            pa[1] = new SqlParameter("@TemplateContent", SqlDbType.NText);
            pa[1].Value = TemplateContent;
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "update bwdf_FormDefinitions set WapTemplateContent=@TemplateContent where id=@formDefinitionId ", pa);
        }
        #endregion

        #region IFormDefinitionService 成员    

        //public  System.Data.DataTable ListFormDefinitionByEntityType(string entityType)
        //{
        //    string sqlText = "select * from bwdf_FormDefinitionInExternals fe Left JOIN bwdf_FormDefinitions fd  On fe.FormDefinitionId =fd.Id Where EntityType='{0}'";
        //    using (System.Data.DataTable dt = IBatisDbHelper.ExecuteDataset(System.Data.CommandType.Text, string.Format(sqlText, entityType)).Tables[0])
        //    {
        //        return dt;
        //    }
           
        //}

        #endregion

        
    }
}
