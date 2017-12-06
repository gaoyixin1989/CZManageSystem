using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm.Extension.Domain;
using System.Data.SqlClient;
using System.Data;

namespace Botwave.DynamicForm.Extension.Contracts
{
    /// <summary>
    /// 表单设计导入导出部署服务接口
    /// </summary>
    public class FormDefinitionDeployService : IFormDefinitionDeployService
    {
        private IFormDefinitionService formDefinitionService;
        private IGetDataService getDataService;
        private IFormItemIFramesService formItemIFramesService;
        private IItemDataListSettingService itemDataListSettingService;
        private IDataListDefinitionService dataListDefinitionService;

        public IFormDefinitionService FormDefinitionService
        {
            set { formDefinitionService = value; }
            get { return formDefinitionService; }
        }

        public IGetDataService GetDataService
        {
            get { return getDataService; }
            set { getDataService = value; }
        }

        public IFormItemIFramesService FormItemIFramesService
        {
            set { formItemIFramesService = value; }
        }

        public IItemDataListSettingService ItemDataListSettingService
        {
            get { return itemDataListSettingService; }
            set { itemDataListSettingService = value; }
        }

        public IDataListDefinitionService DataListDefinitionService
        {
            get { return dataListDefinitionService; }
            set { dataListDefinitionService = value; }
        }
        public FormResult Import(string username, int action, string xml)
        {
            FormItemDefinition item;
            XElement formDefinitionXml;
            FormResult result = new FormResult();
            FormDefinition definition = new FormDefinition();
            FormDefinitionsInExternals definitionsInExternals = new FormDefinitionsInExternals();
            //表单项定义集合
            IList<FormItemDefinition> listItem = new List<FormItemDefinition>();
            //表单项定义Key集合
            IList<string> listItemKey = new List<string>();
            Botwave.Workflow.Domain.WorkflowDefinition wf = null;//流程定义
            //旧表单定义
            FormDefinition oldDefinition;
            //旧的表单项定义ID
            IDictionary<string, Guid> dicItemIds = new Dictionary<string, Guid>();
            if (action != 0 && action != 1)
            {
                result.Message = "action参数出错";
                result.Success = 0;
                return result;
            }
            if (xml.Trim() == string.Empty)
            {
                result.Message = "xml参数不能为空";
                result.Success = 0;
                return result;
            }

            try
            {
                formDefinitionXml = XElement.Parse(xml);//将string解析为XML
            }
            catch (Exception ex)
            {
                result.Message = string.Format("无法解析XML：{0}", ex.ToString());
                result.Success = 0;
                return result;
            }


            #region 表单基本属性
            XAttribute formusernameAttr = formDefinitionXml.Attribute("username");
            XAttribute idAttr = formDefinitionXml.Attribute("id");//表单ID
            XAttribute nameAttr = formDefinitionXml.Attribute("name");//流程名称
            XAttribute titleAttr = formDefinitionXml.Attribute("title");//表单名称

            XAttribute entityTypeAttr = formDefinitionXml.Attribute("entityType");
            XAttribute entityIDAttr = formDefinitionXml.Attribute("entityID");//流程ID
            #endregion

            #region 表单说明
            XElement commentElement = formDefinitionXml.Element("comment");
            #endregion

            #region 表单模板
            XElement templateElement = formDefinitionXml.Element("template");
            XElement wapTemplateElement = formDefinitionXml.Element("waptemplate");
            #endregion

            try
            {
                //表单模板及定义
                string comment = commentElement.Value;
                string template = templateElement.Value;
                Guid WorkFlowEntityId = Guid.Empty;
                definition.Creator = formusernameAttr.Value;
                ////////////////////////////Vincen
                //string[] arrstrTemp = nameAttr.Value.Split("表单".ToCharArray());
                string arrstrTemp = nameAttr.Value;
                //if (arrstrTemp.Length > 1)
                //{
                //    WorkFlowEntityId = formDefinitionService.GetWorkFlowIDByWorkflowname(arrstrTemp[0]);
                //}

                if (!string.IsNullOrEmpty(arrstrTemp))
                {
                    wf = IBatisMapper.Mapper.QueryForObject<Botwave.Workflow.Domain.WorkflowDefinition>("bwwf_Workflows_Select_By_WorkflowName", arrstrTemp);
                    if (wf == null)
                    {
                        result.Success = 0;
                        result.Message = string.Format("找不到与表单对应的流程！");
                        return result;
                    }
                    WorkFlowEntityId = wf.WorkflowId;
                }
                ////////////////
                //获取最新的表单ID  KangLiu 修改 20110721
                //definition.Id = action == 0 ? formDefinitionService.GetFormDefinitionsInExternalsByEntityId(new Guid(WorkFlowEntityId)).FormDefinitionId : new Guid(idAttr.Value);
                //definition.Id = action == 0 ? formDefinitionService.GetFormDefinitionsInExternalsByEntityId(new Guid(WorkFlowEntityId)).FormDefinitionId : Guid.NewGuid();
                //definition.Id = action == 0 ? formDefinitionService.GetFormDefinitionsInExternalsByEntityId(new Guid(entityIDAttr.Value)).FormDefinitionId : Guid.NewGuid();
                //definition.Id = action == 0 ? new Guid(idAttr.Value) : Guid.NewGuid();
                ///////////////获取最新的表单ID并加以判断  KangLiu 修改 20111205
                oldDefinition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", WorkFlowEntityId);
                if (action == 0)
                {
                    
                    if (oldDefinition != null)
                        definition.Id = oldDefinition.Id;
                    else
                    {
                        result.Success = 0;
                        result.Message = string.Format("找不到新版本的表单实例，请选择部署新版本！");
                        return result;
                    }
                }
                else
                    definition.Id = Guid.NewGuid();
                ///////////////
                definition.Name = titleAttr.Value;
                definition.Comment = comment;
                definition.TemplateContent = template;
                definition.LastModifier = formusernameAttr.Value;
                //表单流程关系
                definitionsInExternals.EntityId = new Guid(entityIDAttr.Value);
                definitionsInExternals.EntityType = entityTypeAttr.Value;
                definitionsInExternals.FormDefinitionId = definition.Id;

                if (action == 0)
                {
                    IList<FormItemDefinition> listOleItem = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(definition.Id);
                    foreach (FormItemDefinition formItemDefinition in listOleItem)
                    {
                        if (!dicItemIds.ContainsKey(formItemDefinition.FName)) dicItemIds.Add(formItemDefinition.FName, formItemDefinition.Id);
                    }
                }

                //判断当前表单版本是否为原始版本 KangLiu修改于2011-7-21
                //if (action == 1)
                //{

                //    if (formDefinitionService.GetFormDefinitionByName(definition.Name).Version > 1)
                //    {

                //        result.Success = 0;
                //        result.Message = definition.Name + "：表单设计已存在,本次导入失败！";
                //        return result;
                //    }


                //}
                #region 表单项目定义
                XElement fields = formDefinitionXml.Element("fields");
                if (fields != null)
                {
                    IEnumerable<XElement> elements = fields.Elements("item");
                    foreach (XElement xElement in elements)
                    {
                        item = new FormItemDefinition();
                        string left = xElement.Attribute("left").Value == string.Empty ? "0" : xElement.Attribute("left").Value;
                        string top = xElement.Attribute("top").Value == string.Empty ? "0" : xElement.Attribute("top").Value;
                        string width = xElement.Attribute("width").Value == string.Empty ? "0" : xElement.Attribute("width").Value;
                        string height = xElement.Attribute("height").Value == string.Empty ? "0" : xElement.Attribute("height").Value;
                        bool rowExclusive = xElement.Attribute("rowExclusive").Value == string.Empty || xElement.Attribute("rowExclusive").Value == "0" ? false : true;
                        bool require = xElement.Attribute("require").Value == string.Empty || xElement.Attribute("require").Value == "0" ? false : true;
                        string fname = xElement.Attribute("key").Value.Trim();
                        switch (action)
                        {
                            case 0://更新时使用旧的表单项ID,如果旧的没有说明是新增加的表单项
                                item.Id = dicItemIds.ContainsKey(fname) ? dicItemIds[fname] : Guid.NewGuid();
                                break;
                            case 1:
                                item.Id = Guid.NewGuid();
                                break;
                        }


                        item.FormDefinitionId = definition.Id;
                        item.FName = fname;
                        item.Name = xElement.Attribute("name").Value;
                        item.Comment = xElement.Attribute("comment").Value;
                        item.ItemDataType = (FormItemDefinition.DataType)int.Parse(xElement.Attribute("itemDataType").Value);
                        item.ItemType = (FormItemDefinition.FormItemType)int.Parse(xElement.Attribute("itemType").Value);
                        item.DataSource = xElement.Attribute("dataSource").Value;
                        item.DataBinder = xElement.Attribute("dataBinder").Value;
                        item.DefaultValue = xElement.Attribute("defaultValue").Value;
                        item.Left = int.Parse(left);
                        item.Top = int.Parse(top);
                        item.Width = int.Parse(width);
                        item.Height = int.Parse(height);
                        item.RowExclusive = rowExclusive;
                        item.Require = require;
                        item.ValidateType = xElement.Attribute("validateType").Value;
                        item.MaxVal = xElement.Attribute("maxVal").Value;
                        item.MinVal = xElement.Attribute("minVal").Value;
                        item.Op = xElement.Attribute("op").Value;
                        item.OpTarget = xElement.Attribute("opTarget").Value;
                        item.ErrorMessage = xElement.Attribute("errorMessage").Value;
                        item.ShowSet = xElement.Attribute("showSet").Value;
                        item.ReadonlySet = xElement.Attribute("readonlySet").Value;
                        //item.WriteSet = xElement.Attribute("writeSet").Value;
                        listItem.Add(item);
                        if (listItemKey.Contains(item.FName))
                        {
                            result.Success = 0;
                            result.Message = string.Format("存在相同表单项KEY：{0},本次导入失败", item.FName);
                            return result;
                        }
                        else listItemKey.Add(item.FName);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                result.Success = 0;
                result.Message = string.Format("解析XML时出错：{0}", ex.ToString());
                return result;
            }

            try
            {
                switch (action)
                {
                    case 0://更新
                        {
                            ////部署表单模板
                            formDefinitionService.UpdateFormDefinitionTemplate(definition.Id, definition.TemplateContent);

                            //更新表单项定义
                            foreach (FormItemDefinition itemDefinition in listItem)
                            {
                                if (dicItemIds.ContainsKey(itemDefinition.FName))
                                {
                                    formDefinitionService.UpdateItem(itemDefinition);
                                }
                                else
                                {   //原来没有则追加表单项定义
                                    formDefinitionService.AppendItemToForm(itemDefinition);
                                }
                            }

                        }
                        break;
                    case 1://部署新版本
                        {
                            //获取当前表单版本 KangLiu修改于2011-7-21
                            definition.Version = formDefinitionService.GetFormDefinitionByName(definition.Name).Version;
                            ////部署表单模板及定义  会返回新的表单ID
                            Guid newFormDefinitionId = formDefinitionService.SaveFormDefinition(definition);
                            //string[] arrstr = nameAttr.Value.Split("表单".ToCharArray());
                            //if (arrstr.Length > 1)
                            //{
                            //    definitionsInExternals.EntityId = formDefinitionService.GetWorkFlowIDByWorkflowname(arrstr[0]);
                            //}
                            string arrstrTemp = nameAttr.Value.Replace("表单", "");
                            if (!string.IsNullOrEmpty(arrstrTemp))
                            {

                                if (wf == null)
                                {
                                    result.Success = 0;
                                    result.Message = string.Format("找不到与表单对应的流程！");
                                    return result;
                                }
                                definitionsInExternals.EntityId = wf.WorkflowId;
                            }
                            //部署流程表单关系
                            //definitionsInExternals.FormDefinitionId = newFormDefinitionId.ToString();
                            //部署流程表单关系 KangLiu修改于2011-7-21
                            definitionsInExternals.FormDefinitionId = newFormDefinitionId;
                            formDefinitionService.AssociateFormDefinitionWithExternalEntity(definitionsInExternals, false);
                            Guid oldFormDefinitionId = oldDefinition == null ? Guid.Empty : oldDefinition.Id;
                            //部署表单项
                            SaveAppendItemToForm(listItem, newFormDefinitionId,oldFormDefinitionId);
                        }
                        break;
                }
                if (wapTemplateElement != null)//更新wap模板
                {
                    SqlParameter[] pa = new SqlParameter[2];
                    pa[0] = new SqlParameter("@formDefinitionId", SqlDbType.UniqueIdentifier);
                    pa[0].Value = definition.Id;
                    pa[1] = new SqlParameter("@TemplateContent", SqlDbType.NText);
                    pa[1].Value = wapTemplateElement.Value;
                    IBatisDbHelper.ExecuteScalar(CommandType.Text, "update bwdf_FormDefinitions set WapTemplateContent=@TemplateContent where id=@formDefinitionId ", pa);
                }
            }
            catch (Exception err)
            {
                result.Success = 0;
                result.Message = err.ToString();
                return result;
            }
            result.Message = action == 0 ? definition.Name + "：表单更新成功！" : definition.Name + "：部署新版本成功！";
            result.Success = 1;
            return result;
        }

        /// <summary>
        /// 使用新的表单ID保存表单项目定义
        /// </summary>
        /// <param name="list"></param>
        /// <param name="formDefinitionId"></param>
        private void SaveAppendItemToForm(IList<FormItemDefinition> list, Guid formDefinitionId, Guid oldFormDefinitionId)
        {
            IList<FormItemDefinition> newListItems = new List<FormItemDefinition>();
            IDictionary<string, string> idList = new Dictionary<string, string>();
            foreach (FormItemDefinition item in list)
            {
                string fName = item.FName;
                item.FormDefinitionId = formDefinitionId;
                newListItems.Add(item);
                idList.Add(fName, item.Id.ToString());
            }
            formDefinitionService.AppendItemsToForm(newListItems);

            #region 从上一版本中同步表单项扩展配置
            //获取旧表单配置
            FormDefinition definition = formDefinitionService.GetFormDefinitionById(oldFormDefinitionId, true);
            IList<FormItemDefinition> itemList = definition.Items;
            IDictionary<string, string> oldIdList = new Dictionary<string, string>();
            foreach (FormItemDefinition item in itemList)
            {
                string fName = item.FName;
                item.FormDefinitionId = formDefinitionId;
                oldIdList.Add(item.Id.ToString(),fName);
            }
            //插入datalist设置
            IDictionary<string, Guid> datalistDict = new Dictionary<string, Guid>();
            IList<DataListSetting> settings = itemDataListSettingService.GetDataListSettingByFormDefinitionId(oldFormDefinitionId);
            foreach (DataListSetting setting in settings)
            {
                IList<DataListItemDefinition> itemDefinitions = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(setting.FormItemDefinitionId);
                foreach (DataListItemDefinition itemDefinition in itemDefinitions)
                {
                    if (oldIdList.ContainsKey(itemDefinition.FormItemDefinitionId.ToString()))
                    {
                        string itemKey = oldIdList[itemDefinition.FormItemDefinitionId.ToString()];
                        if (idList.ContainsKey(itemKey))
                        {
                            itemDefinition.FormItemDefinitionId = new Guid(idList[itemKey]);
                            itemDefinition.Id = Guid.NewGuid();
                        }
                    }
                    
                }

                if (oldIdList.ContainsKey(setting.FormItemDefinitionId.ToString()))
                {
                    string settingKey = oldIdList[setting.FormItemDefinitionId.ToString()];
                    if (idList.ContainsKey(settingKey))
                    {
                        setting.FormItemDefinitionId = new Guid(idList[settingKey]);
                        itemDataListSettingService.DataListSettingInsert(setting);
                        dataListDefinitionService.AppendItemsToForm(itemDefinitions, datalistDict);
                    }
                }
            }

            //插入扩展项设置
            IList<FormItemExtension> extensionlist = getDataService.GetFormItemExtensionSettingsByFormdefinitionId(oldFormDefinitionId);
            foreach (FormItemExtension item in extensionlist)
            {
                if (oldIdList.ContainsKey(item.FormItemDefinitionId.ToString()))
                {
                    string extensionKey = oldIdList[item.FormItemDefinitionId.ToString()];
                    if (idList.ContainsKey(extensionKey))
                    {
                        item.FormItemDefinitionId = new Guid(idList[extensionKey]);
                        getDataService.InserFormItemExtension(item);
                    }
                }
            }

            //插入iframe设置
            IList<FormItemIFrames> iFrames = formItemIFramesService.SelectByFormDefinitionId(oldFormDefinitionId);
            foreach (FormItemIFrames item in iFrames)
            {
                if (oldIdList.ContainsKey(item.FormItemDefinitionId.ToString()))
                {
                    string framesKey = oldIdList[item.FormItemDefinitionId.ToString()];
                    if (idList.ContainsKey(framesKey))
                    {
                        item.FormItemDefinitionId = new Guid(idList[framesKey]);
                        formItemIFramesService.Create(item);
                    }
                }
            }
            #endregion
        }

        public FormResult Export(Guid workflowId)
        {
            FormResult result = new FormResult();
            result.Success = 1;
            FormDefinition formDefinition;
            FormDefinitionsInExternals definitionsInExternals;
            Botwave.Workflow.Domain.WorkflowDefinition wf = null;
            try
            {
               
                wf = IBatisMapper.Mapper.QueryForObject<Botwave.Workflow.Domain.WorkflowDefinition>("bwwf_Workflows_Select_By_Id", workflowId);
                if (wf == null)
                {
                    result.Success = 0;
                    result.Message = string.Format("流程定义不存在！");
                    return result;
                }
                formDefinition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", workflowId);
                if (formDefinition == null)
                {
                    result.Message = string.Format("表单不存在！");
                    result.Success = 0;
                    return result;
                }
                //获取wap版本模板
                SqlParameter[] pa = new SqlParameter[1];
                pa[0] = new SqlParameter("@formDefinitionId", SqlDbType.UniqueIdentifier);
                pa[0].Value = formDefinition.Id;
                object WapTemplateContent = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select WapTemplateContent from bwdf_FormDefinitions where id=@formDefinitionId ", pa);
                //string _wapTemplateContent = Botwave.Commons.DbUtils.ToString(result);
                //使用名称获取当前版本的表单流程关系
                //definitionsInExternals = formDefinitionService.(formDefinitionName);

                StringBuilder formDefinitionXml = new StringBuilder();
                formDefinitionXml.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                formDefinitionXml.AppendLine(
                    string.Format(
                        "<form username=\"{0}\" id=\"{1}\" name=\"{2}\" title=\"{4}\" entityType=\"Form_Workflow\" entityID=\"{3}\">",
                        formDefinition.Creator, formDefinition.Id, wf.WorkflowName, workflowId,formDefinition.Name));
                formDefinitionXml.AppendLine(string.Format(@"<comment><![CDATA[{0}]]></comment>", formDefinition.Comment));
                formDefinitionXml.AppendLine(string.Format(@"<template><![CDATA[{0}]]></template>", formDefinition.TemplateContent));
                formDefinitionXml.AppendLine(string.Format(@"<waptemplate><![CDATA[{0}]]></waptemplate>", WapTemplateContent));
                formDefinitionXml.AppendLine("<fields>");

                IList<FormItemDefinition> listOleItem = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(formDefinition.Id);
                foreach (FormItemDefinition item in listOleItem)
                {
                    string itemStr = string.Format("<item key=\"{0}\" name=\"{1}\" itemDataType=\"{2}\" itemType=\"{3}\" dataSource=\"{4}\" dataBinder=\"{5}\" defaultValue=\"{6}\" comment=\"{7}\" left=\"{8}\" top=\"{9}\" width=\"{10}\" height=\"{11}\" rowExclusive=\"{12}\" require=\"{13}\" validateType=\"{14}\" maxVal=\"{15}\" minVal=\"{16}\" op=\"{17}\" opTarget=\"{18}\" errorMessage=\"{19}\" showSet=\"{20}\" writeSet=\"{21}\" readonlySet=\"{22}\" />",
                      item.FName, item.Name, (int)item.ItemDataType, (int)item.ItemType, item.DataSource, item.DataBinder, item.DefaultValue, item.Comment, item.Left, item.Top, item.Width, item.Height, item.RowExclusive, item.Require, item.ValidateType, item.MaxVal, item.MinVal, item.Op, item.OpTarget, item.ErrorMessage, item.ShowSet, string.Empty, item.ReadonlySet);
                    formDefinitionXml.AppendLine(itemStr);
                }
                formDefinitionXml.AppendLine("</fields></form>");
                result.Data = formDefinitionXml.ToString();
            }
            catch (Exception ex)
            {
                result.Success = 0;
                result.Message = ex.ToString();
            }
            return result;
        }
    }
}
