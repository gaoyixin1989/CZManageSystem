using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm.Extension.Domain;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;

namespace Botwave.DynamicForm.Extension.Contracts
{
    public class DataListInstanceService : IDataListInstanceService
    {
        private IDataListDefinitionService dataListDefinitionService;
        private IItemDataListSettingService itemDataListSettingService;
        private IFormDefinitionService formDefinitionService;

        public IDataListDefinitionService DataListDefinitionService
        {
            set { this.dataListDefinitionService = value; }
        }

        public IItemDataListSettingService ItemDataListSettingService
        {
            set { this.itemDataListSettingService = value; }
        }

        public IFormDefinitionService FormDefinitionService
        {
            set { this.formDefinitionService = value; }
        }
        #region IFormInstanceService Members

        /// <summary>
        /// 保存表单实例.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="contextVariables"></param>
        /// <returns></returns>
        public void SaveDataListInstance(Guid workflowId, Guid workflowInstanceId, IDictionary<string, object> contextVariables)
        {
            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                //创建表单项实例

                DataListItemInstance itemInstance = new DataListItemInstance();
                FormDefinition formDefinition = formDefinitionService.GetFormDefinitionByFormInstanceId(workflowInstanceId);
                if (formDefinition == null)
                    formDefinition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", workflowId);
                IList<DataListSetting> settings = itemDataListSettingService.GetDataListSettingByFormDefinitionId(formDefinition.Id);
                //deleteFormByInstanceId(workflowInstanceId);
                foreach (DataListSetting setting in settings)
                {
                    IList<DataListItemDefinition> itemDefinitionList = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(setting.FormItemDefinitionId);

                    IDictionary<string, FormDataListContext> dictDataSet = GetDataSet(contextVariables, setting, itemDefinitionList);
                    if (dictDataSet.Count > 0)
                    {
                        dictDataSet = SaveForm(workflowInstanceId, setting, dictDataSet);

                        for (int i = 0; i < setting.Rows; i++)
                        {
                            foreach (DataListItemDefinition itemDefinition in itemDefinitionList)
                            {
                                string key=itemDefinition.FName+"_r"+i.ToString();
                                if (dictDataSet.ContainsKey(key))
                                {
                                    if (!string.IsNullOrEmpty(dictDataSet[key].Value))//数据不为空时才插入数据
                                    {
                                        itemInstance.Id = Guid.NewGuid();
                                        itemInstance.DataListItemDefinitionId = itemDefinition.Id;
                                        itemInstance.FormInstanceId = workflowInstanceId;
                                        itemInstance.RowNumber = i;
                                        itemInstance.ColumnNumber = itemDefinition.ColumnNumber;
                                        itemInstance.Value = dictDataSet[key].Value;
                                        IBatisMapper.Insert("bwdf_DataListItemInstances_Insert", itemInstance);
                                    }
                                }
                            }
                        }
                    }
                }
                
                IBatisMapper.Mapper.CommitTransaction();
                
            }
            catch(Exception ex)
            {
                IBatisMapper.Mapper.RollBackTransaction();
                throw new AppException("保存表单实例出错", ex);
            }
        }

        /// <summary>
        /// 保存表单实例.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="contextVariables"></param>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public void SaveDataListInstance(Guid workflowId, Guid workflowInstanceId, IDictionary<string, object> contextVariables,HttpRequest request)
        {
            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                //创建表单项实例

                DataListItemInstance itemInstance = new DataListItemInstance();
                FormDefinition formDefinition = formDefinitionService.GetFormDefinitionByFormInstanceId(workflowInstanceId);
                if (formDefinition == null)
                    formDefinition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", workflowId);
                IList<DataListSetting> settings = itemDataListSettingService.GetDataListSettingByFormDefinitionId(formDefinition.Id);
                //deleteFormByInstanceId(workflowInstanceId);
                foreach (DataListSetting setting in settings)
                {
                    if (setting.Type == 1)
                    {
                        FormItemDefinition formItemDefinition = formDefinitionService.GetFormItemDefinitionById(setting.FormItemDefinitionId);
                        string fName = formItemDefinition.FName;
                        string rows = request.Form["hid_DataList_rowcnt"+fName];
                        setting.Rows = DbUtils.ToInt32(rows);
                    }
                    IList<DataListItemDefinition> itemDefinitionList = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(setting.FormItemDefinitionId);
                    
                    IDictionary<string, FormDataListContext> dictDataSet = GetDataSet(contextVariables, setting, itemDefinitionList);
                    if (dictDataSet.Count > 0)
                    {
                        dictDataSet = SaveForm(workflowInstanceId, setting, dictDataSet);

                        for (int i = 0; i < setting.Rows; i++)
                        {
                            foreach (DataListItemDefinition itemDefinition in itemDefinitionList)
                            {
                                string key = itemDefinition.FName + "_r" + i.ToString();
                                if (dictDataSet.ContainsKey(key))
                                {
                                    if (!string.IsNullOrEmpty(dictDataSet[key].Value))//数据不为空时才插入数据
                                    {
                                        itemInstance.Id = Guid.NewGuid();
                                        itemInstance.DataListItemDefinitionId = itemDefinition.Id;
                                        itemInstance.FormInstanceId = workflowInstanceId;
                                        itemInstance.RowNumber = i;
                                        itemInstance.ColumnNumber = itemDefinition.ColumnNumber;
                                        itemInstance.Value = dictDataSet[key].Value;
                                        IBatisMapper.Insert("bwdf_DataListItemInstances_Insert", itemInstance);
                                    }
                                }
                            }
                        }
                    }
                }

                IBatisMapper.Mapper.CommitTransaction();

            }
            catch (Exception ex)
            {
                IBatisMapper.Mapper.RollBackTransaction();
                throw new AppException("保存表单实例出错", ex);
            }
        }

        public IList<DataListItemInstance> GetDataListItemInstancesByFormInstanceId(Guid formInstanceId)
        {
            if (formInstanceId == Guid.Empty)
                return null;

            return IBatisMapper.Select<DataListItemInstance>("bwdf_DataListItemInstances_Select", formInstanceId);
        }


        private IList<DataListItemInstance> GetDlItemInstances(Hashtable condition)
        {

            return IBatisMapper.Select<DataListItemInstance>("bwdf_getDlItemInstances", condition);
        }

        public IList<DataListItemInstance> GetDlItemInstancesByDlItemDefId(Guid formInstanceId, Guid dlItemDefinitionId)
        {
            Hashtable condition = new Hashtable();
            condition["formInstanceId"] = formInstanceId;
            condition["dlItemDefinitionId"] = dlItemDefinitionId;
            return GetDlItemInstances(condition);

        }

        public IList<DataListItemInstance> GetDataListItemInstancesByFormInstanceIdAndFormItemDefinitionId(Guid formInstanceId, Guid formItemDefinitionId, bool withDefinitions)
        {
            if (formInstanceId == Guid.Empty)
                return null;

            IList<DataListItemInstance> itemInstanceList = IBatisMapper.Select<DataListItemInstance>("bwdf_DataListItemInstances_Select", formInstanceId);

            if (itemInstanceList.Count > 0 && withDefinitions)
            {
                IList<DataListItemDefinition> definitionList = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(formItemDefinitionId);

                foreach (DataListItemInstance itemInstance in itemInstanceList)
                {
                    foreach (DataListItemDefinition itemDefinition in definitionList)
                    {
                        if (itemInstance.DataListItemDefinitionId.Equals(itemDefinition.Id))
                        {
                            itemInstance.Definition = itemDefinition;
                            break;
                        }
                    }
                }
            }
            return itemInstanceList;
        }



        public void UpdateFormItemInstance(IList<DataListItemInstance> items)
        {
            foreach (DataListItemInstance item in items)
            {
                IBatisMapper.Update("bwdf_DataListItemInstances_Update", item);
            }            
        }

        public void DeleteFormItemInstance(IDictionary<string, DataListItemInstance> dict)
        {
            foreach (KeyValuePair<string,DataListItemInstance> pair in dict)
            {
                IBatisMapper.Update("bwdf_DataListItemInstances_Delete_By_Id", pair.Value.Id);
            }
        }

        public void SaveForm(Guid id, FormContext formContext)
        {
            //用字典来存储真实的表单项输入值，支持多选

            IDictionary<string, object> dict = formContext.Variables;
            //IList<DataListSetting> settings = itemDataListSettingService.GetDataListSettingByFormDefinitionId(id);
            //this.SaveForm(id, dict);
        }

        public IDictionary<string, FormDataListContext> SaveForm(Guid id,DataListSetting setting, IDictionary<string, FormDataListContext> dictDataSet)
        {
            FormDefinition formDefinition = formDefinitionService.GetFormDefinitionByFormInstanceId(id);
            if (formDefinition == null)
                throw new AppException("保存表单DataList出错，表单实例不存在。");

                if (setting != null)
                {
                    //获取表单项实例及定义
                    IList<DataListItemInstance> items = GetDataListItemInstancesByFormInstanceIdAndFormItemDefinitionId(id, setting.FormItemDefinitionId, true);
                    //内容实例字典，用于过滤表单中存在的实例
                    IDictionary<string, DataListItemInstance> dict = new Dictionary<string, DataListItemInstance>();
                    foreach (DataListItemInstance item in items)
                    {
                        dict.Add(item.Definition.FName+"_r"+item.RowNumber, item);
                    }
                    //用字典来存储真实的表单项输入值，支持多选

                    //更新表单实例，批量处理
                    for (int k = 0; k < setting.Rows; k++)
                    {
                        for (int i = 0, iCount = items.Count - 1; i <= iCount; i++)
                        {
                            //只有输入的表单中包含此表单项才进行处理

                            string key = items[i].Definition.FName + "_r" + k.ToString();
                            if (dictDataSet.ContainsKey(key))
                            {
                                //根据表单定义进行输入检测，若值不为空，则更新实例
                                if (dictDataSet[key].ColumnNumber == items[i].ColumnNumber && dictDataSet[key].RowNumber == items[i].RowNumber)
                                {
                                    items[i].Value = dictDataSet[key].Value;
                                    dictDataSet.Remove(key);
                                }
                                //从字典中移除该示例，表示存在该实例
                                dict.Remove(key);
                            }
                        }
                    }
                    
                    UpdateFormItemInstance(items);
                    //删除表单中不存在的实例
                    DeleteFormItemInstance(dict);
                }

            return dictDataSet;
        }

        /// <summary>
        /// 删除实例
        /// </summary>
        /// <param name="id"></param>
        private void deleteFormByInstanceId(Guid id)
        {
            IBatisMapper.Update("bwdf_DataListItemInstances_Delete", id);
        }
       #endregion

        #region Private Methods

        /// <summary>
        /// 替换双引号为单引号.
        /// </summary>
        /// <param name="objRe"></param>
        /// <returns></returns>
        private static string ReplaceHTML(object objHTML)
        {
            //return objHTML.ToString().Replace("\"", "'");
            return objHTML.ToString();
        }

        private IDictionary<string, FormDataListContext> GetDataSet(IDictionary<string, object> contextVariables, DataListSetting setting, IList<DataListItemDefinition> items)
        {
            IDictionary<string, FormDataListContext> dict = new Dictionary<string, FormDataListContext>();
            for (int i = 0; i < setting.Rows; i++)
            {
                for (int j = 0; j < setting.Columns; j++)
                {
                    string key = items[j].FName + "_r" + i.ToString();
                    if (contextVariables.ContainsKey(key))
                    {
                        if (!dict.ContainsKey(key))
                        {
                            FormDataListContext context = new FormDataListContext();
                            context.FName = key;
                            context.RowNumber = i;
                            context.ColumnNumber = j;
                            context.Value = contextVariables[key].ToString();
                            dict.Add(key, context);
                        }
                    }

                }
            }
            return dict;
        }

        #endregion


     
    }
}
