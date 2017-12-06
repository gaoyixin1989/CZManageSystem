using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Plugin;
using Botwave.DynamicForm.Services;

namespace Botwave.DynamicForm.IBatisNet
{
    public class FormInstanceService : IFormInstanceService
    {
        private IFormDefinitionService formDefinitionService;
        private IUploadFileHandler uploadFileHandler;

        public IFormDefinitionService FormDefinitionService
        {
            set { this.formDefinitionService = value; }
        }
        public IUploadFileHandler UploadFileHandler
        {
            set { uploadFileHandler = value; }
        }

        #region IFormInstanceService Members

        /// <summary>
        /// 创建表单实例.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="formDefinitionId"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public Guid CreateFormInstance(Guid Id, Guid formDefinitionId, string actor)
        {
            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                //创建表单实例
                FormInstance instance = new FormInstance();
                instance.Id = Id;
                instance.FormDefinitionId = formDefinitionId;
                instance.Creator = actor;
                instance.LastModifier = actor;
                IBatisMapper.Insert("bwdf_Instances_Insert", instance);
                //创建表单项实例

                FormItemInstance itemInstance = new FormItemInstance();
                IList<FormItemDefinition> itemDefinitionList = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(formDefinitionId);
                foreach (FormItemDefinition itemDefinition in itemDefinitionList)
                {
                    itemInstance.Id = Guid.NewGuid();
                    itemInstance.FormItemDefinitionId = itemDefinition.Id;
                    itemInstance.FormInstanceId = Id;
                    IBatisMapper.Insert("bwdf_ItemInstances_Insert", itemInstance);
                }
                IBatisMapper.Mapper.CommitTransaction();
            }
            catch(Exception ex)
            {
                IBatisMapper.Mapper.RollBackTransaction();
                throw new AppException("创建表单实例出错", ex);
            }
            return Id;
        }

        public void UpdateFormInstance(Guid id, string actor)
        {
            FormInstance formInstance = new FormInstance();
            formInstance.Id = id;
            formInstance.LastModifier = actor;
            formInstance.LastModTime = DateTime.Now;
            IBatisMapper.Update("bwdf_Instances_Update", formInstance);
        }

        public void RemoveFormInstance(Guid formInstanceId)
        {
            IBatisMapper.Delete("bwdf_Instances_Delete", formInstanceId);
        }

        public FormInstance GetFormInstanceById(Guid formInstanceId)
        {
            IList<FormInstance> list = IBatisMapper.Select<FormInstance>("bwdf_Instances_Select", formInstanceId);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        public FormInstance GetFormInstanceById(Guid formInstanceId, bool withItems)
        {
            IList<FormInstance> list = IBatisMapper.Select<FormInstance>("bwdf_Instances_Select", formInstanceId);
            if (list.Count > 0)
            {
                if (withItems)
                {
                    list[0].Items = GetFormItemInstancesByFormInstanceId(list[0].Id, true);
                }
                return list[0];
            }
            return null;
        }

        public IList<FormInstance> ListFormInstanceByFormDefinitionId(Guid formDefinitionId)
        {
            IList<FormInstance> list = IBatisMapper.Select<FormInstance>("bwdf_Instances_Select_By_FormDefinitionId", formDefinitionId);
            foreach (FormInstance instance in list)
            {
                instance.Items = GetFormItemInstancesByFormInstanceId(instance.Id, true);
            }
            return list;
        }

        public IList<FormItemInstance> GetFormItemInstancesByFormInstanceId(Guid formInstanceId)
        {
            if (formInstanceId == Guid.Empty)
                return null;

            return IBatisMapper.Select<FormItemInstance>("bwdf_ItemInstances_Select", formInstanceId);
        }

        public IList<FormItemInstance> GetFormItemInstancesByFormInstanceId(Guid formInstanceId, bool withDefinitions)
        {
            if (formInstanceId == Guid.Empty)
                return null;

            IList<FormItemInstance> itemInstanceList = IBatisMapper.Select<FormItemInstance>("bwdf_ItemInstances_Select", formInstanceId);

            if (itemInstanceList.Count > 0 && withDefinitions)
            {
                FormInstance formInstance = GetFormInstanceById(formInstanceId);
                IList<FormItemDefinition> definitionList = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(formInstance.FormDefinitionId);

                foreach (FormItemInstance itemInstance in itemInstanceList)
                {
                    foreach (FormItemDefinition itemDefinition in definitionList)
                    {
                        if (itemInstance.FormItemDefinitionId.Equals(itemDefinition.Id))
                        {
                            itemInstance.Definition = itemDefinition;
                            break;
                        }
                    }
                }
            }
            return itemInstanceList;
        }

        public void UpdateFormItemInstance(IList<FormItemInstance> items)
        {
            foreach (FormItemInstance item in items)
            {
                IBatisMapper.Update("bwdf_ItemInstances_Update", item);
            }            
        } 

        public void SaveForm(Guid id, FormContext formContext, string actor)
        {
            //用字典来存储真实的表单项输入值，支持多选

            IDictionary<string, object> dict = formContext.Variables;
            this.SaveForm(id, dict, actor);
        }

        public void SaveForm(Guid id, IDictionary<string, object> contextVariables, string actor)
        {
            //获取表单项实例及定义
            IList<FormItemInstance> items = GetFormItemInstancesByFormInstanceId(id, true);

            //用字典来存储真实的表单项输入值，支持多选

            //更新表单实例，批量处理

            for (int i = 0, iCount = items.Count - 1; i <= iCount; i++)
            {
                //只有输入的表单中包含此表单项才进行处理

                string key = items[i].Definition.FName;
                if (contextVariables.ContainsKey(key))
                {
                    //根据表单定义进行输入检测，若值改变，则更新实例

                    if (items[i].Value == contextVariables[key].ToString() || items[i].TextValue == ReplaceHTML(contextVariables[key]))
                    {
                        items.Remove(items[i]);
                        i--;
                        iCount--;
                        continue;
                    }

                    if (items[i].Definition.ItemDataType == FormItemDefinition.DataType.Text)
                    {
                        items[i].TextValue = ReplaceHTML(contextVariables[key]);
                    }

                    //附件类型表单项：上传附件至文件服务器并将文件名存值

                    else if (items[i].Definition.ItemDataType == FormItemDefinition.DataType.File)
                    {
                        string fileName = String.Empty;
                        if (null != uploadFileHandler)
                            fileName = uploadFileHandler.Upload(contextVariables[key]);

                        items[i].Value = fileName;
                    }
                    else
                    {
                        items[i].Value = contextVariables[key].ToString();
                        if (items[i].Definition.ItemDataType == FormItemDefinition.DataType.Decimal && !String.IsNullOrEmpty(contextVariables[key].ToString()))
                            items[i].DecimalValue = Decimal.Parse(contextVariables[key].ToString());
                    }
                }
            }

            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                UpdateFormItemInstance(items);
                if (items.Count > 0)
                    UpdateFormInstance(id, actor);
                IBatisMapper.Mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                IBatisMapper.Mapper.RollBackTransaction();
                throw new AppException("保存表单出错", ex);
            }
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

        #endregion
    }
}
