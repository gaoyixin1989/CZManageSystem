using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Linq;
using Botwave.DynamicForm.Extension.Implements;
using Spring.Context.Support;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Extension.Domain;
using System.Text;
using Botwave.Web;
using Botwave.Workflow.Routing.Implements;
using System.Data;
using Botwave.Workflow.Routing.Domain;
using Botwave.DynamicForm.Extension.Util;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Commons;
using System.Collections;

namespace Botwave.DynamicForm.Extension.WebServices
{
    /// <summary>
    ///GetDataAjaxService 的摘要说明
    /// </summary>
    [WebService(Namespace = "Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]
    public class GetDataAjaxService : System.Web.Services.WebService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(GetDataAjaxService));
        #region properties
        private IGetDataService getDataService;
        private IGetOuterDataHandler getOuterDataHandler;
        private IJsLibraryService jsLibraryService;
        private IFormDefinitionService formDefinitionService;
        private IFormInstanceService formInstanceService;
        private IFormItemIFramesService formItemIFramesService;
        private IItemDataListSettingService itemDataListSettingService;
        private IDataListDefinitionService dataListDefinitionService;
        private IDataListInstanceService dataListInstanceService;
        private IActivityRulesService activityRulesService;
        private IActivityDefinitionService activityDefinitionService;

        public IFormDefinitionService FormDefinitionService
        {
            get { return formDefinitionService; }
            set { formDefinitionService = value; }
        }

        public IGetDataService GetDataService
        {
            get { return getDataService; }
            set { getDataService = value; }
        }

        public IGetOuterDataHandler GetOuterDataHandler
        {
            get { return getOuterDataHandler; }
            set { getOuterDataHandler = value; }
        }

        public IJsLibraryService JsLibraryService
        {
            get { return jsLibraryService; }
            set { jsLibraryService = value; }
        }

        public IFormItemIFramesService FormItemIFramesService
        {
            set { this.formItemIFramesService = value; }
        }
        /// <summary>
        /// 表单实例服务.
        /// </summary>
        public IFormInstanceService FormInstanceService
        {
            set { this.formInstanceService = value; }
        }

        public IItemDataListSettingService ItemDataListSettingService
        {
            set { this.itemDataListSettingService = value; }
        }

        public IDataListDefinitionService DataListDefinitionService
        {
            set { this.dataListDefinitionService = value; }
        }

        public IDataListInstanceService DataListInstanceService
        {
            set { this.dataListInstanceService = value; }
        }

        public IActivityRulesService ActivityRulesService
        {
            set { this.activityRulesService = value; }
        }

        public IActivityDefinitionService ActivityDefinitionService
        {
            set { this.activityDefinitionService = value; }
        }
        #endregion
        public GetDataAjaxService()
        {

            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
            this.getDataService = (WebApplicationContext.Current["getDataService"] as IGetDataService);
            this.getOuterDataHandler = (WebApplicationContext.Current["getOuterDataHandler"] as IGetOuterDataHandler);
            this.jsLibraryService = (WebApplicationContext.Current["jsLibraryService"] as IJsLibraryService);
            this.formDefinitionService = (WebApplicationContext.Current["formDefinitionService"] as IFormDefinitionService);
            this.formInstanceService = (WebApplicationContext.Current["formInstanceService"] as IFormInstanceService);
            this.formItemIFramesService = (WebApplicationContext.Current["formItemIFramesService"] as IFormItemIFramesService);
            this.itemDataListSettingService = (WebApplicationContext.Current["itemDataListSettingService"] as IItemDataListSettingService);
            this.dataListDefinitionService = (WebApplicationContext.Current["dataListDefinitionService"] as IDataListDefinitionService);
            this.dataListInstanceService = (WebApplicationContext.Current["dataListInstanceService"] as IDataListInstanceService);
            this.activityRulesService = (WebApplicationContext.Current["activityRulesService"] as IActivityRulesService);
            this.activityDefinitionService = (WebApplicationContext.Current["activityDefinitionService"] as IActivityDefinitionService);
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(Description = "获取数据集合")]
        public string getDataSet(string workflowId, string workflowInstanceId)
        {
            FormDefinition formDefinition = formDefinitionService.GetFormDefinitionByFormInstanceId(new Guid(workflowInstanceId));
            if (formDefinition == null)
                formDefinition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", new Guid(workflowId));

            if (formDefinition != null)
            {
                IList<FormItemExtension> formItemExtensions = getDataService.GetFormItemExtensionSettingsByFormdefinitionId(formDefinition.Id);
                IDictionary<Guid, FormItemExtension> extensionItems = new Dictionary<Guid, FormItemExtension>();
                if (formItemExtensions.Count == 0)
                    return string.Empty;
                foreach (FormItemExtension formItemExtension in formItemExtensions)
                {
                    if (!extensionItems.ContainsKey(formItemExtension.FormItemDefinitionId))
                        extensionItems.Add(formItemExtension.FormItemDefinitionId, formItemExtension);
                }
                StringBuilder json = new StringBuilder();
                IList<FormItemDefinition> formItemDefinitions = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(formDefinition.Id);
                if (formItemExtensions.Count == 0)
                    return string.Empty;
                foreach (FormItemDefinition item in formItemDefinitions)
                {
                    if (extensionItems.ContainsKey(item.Id))
                        json.Append("{\"Key\":\"" + item.FName + "\", \"ID\":\"" + item.Id + "\",\"Type\":\"" + item.ItemType + "\"},");
                }
                json = json.Remove(json.Length - 1, 1);
                //json.Append("}");
                return json.ToString();
            }
            return string.Empty;
        }

        [WebMethod(Description = "获取外部数据集合")]
        public string getOuterDataSet(string id, string fName, string dataType, string[] dataSet)
        {
            FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(id));
            if (formItemExtension != null)
            {
                IDictionary<string, string> dict = new Dictionary<string, string>();
                foreach (string str in dataSet)
                {
                    if (!dict.ContainsKey(str.Split('$')[0]))
                        dict.Add(str.Split('$')[0], str.Split('$')[1]);
                }
                //switch (dataType)
                //{
                //    case "Text":
                //        break;
                //    case "TextArea":
                //        break;
                //    case "CheckBoxList":
                //        break;
                //    case "DropDownList":
                //        break;
                //    case "RadioButtonList":
                //        break;
                //    default:
                //        break;

                //}
                return getOuterDataHandler.getDataJson(formItemExtension, dataType, dict);
            }
            return string.Empty;
        }

        [WebMethod(Description = "获取js格式校验")]
        public string GenerJSFunction(string id, string dataType, string[] dataSet)
        {
            FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(id));
            if (formItemExtension != null)
            {
                IDictionary<string, string> dict = new Dictionary<string, string>();
                foreach (string str in dataSet)
                {
                    if (!dict.ContainsKey(str.Split('$')[0]))
                        dict.Add(str.Split('$')[0], str.Split('$')[1]);
                }
                if (formItemExtension.ValidateType > 0)
                {
                     JsLibrary jsLibary=null;
                     string appPath = WebUtils.GetAppPath();
                     switch (dataType)
                     {
                         case "Text":
                             jsLibary = jsLibraryService.GetLibraryById(formItemExtension.ValidateType);
                             if (jsLibary != null)
                             {
                                 return "{\"Type\":\"" + dataType + "\",\"EventName\":\"" + jsLibary.Events + "\",\"FunctionName\":\"" + formItemExtension.ValidateFunction + "\",\"src\":\"" + Server.UrlPathEncode(jsLibary.FileName) + "\",\"Value\":\"\",\"UseTo\":\"JavaScript\"}";
                             }
                             break;
                         case "TextArea":
                             jsLibary = jsLibraryService.GetLibraryById(formItemExtension.ValidateType);
                             if (jsLibary != null)
                             {
                                 return "{\"Type\":\"" + dataType + "\",\"EventName\":\"" + jsLibary.Events + "\",\"FunctionName\":\"" + formItemExtension.ValidateFunction + "\",\"src\":\"" + Server.UrlPathEncode(jsLibary.FileName) + "\",\"Value\":\"\",\"UseTo\":\"JavaScript\"}";
                             }
                             break;
                         case "CheckBoxList":
                             break;
                         case "DropDownList":
                             break;
                         case "RadioButtonList":
                             break;
                         default:
                             break;

                     }
                }
            }
            return string.Empty;
        }

        [WebMethod(Description = "判断字段是否存在字段联动设置")]
        public string ExistItemsLinkage(string id)
        {
            FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(id));
            if (formItemExtension != null)
            {
                if (formItemExtension.ItemsLinkageType > 0)
                {
                    return "true";
                }
            }
            return "false";
        }

        [WebMethod(Description = "获取js字段联动")]
        public string GenerItemsLinkage(string id, string dataType)
        {
            FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(id));
            if (formItemExtension != null)
            {
                if (formItemExtension.ItemsLinkageType > 0)
                {
                    if (formItemExtension.ItemsLinkageType == 1)
                        return formItemExtension.ItemsLinkageJson;
                    //switch (dataType)
                    //{
                    //    case "DropDownList":
                    //        if(formItemExtension.ItemsLinkageType == 1)
                    //        return formItemExtension.ItemsLinkageJson;
                    //        else if (formItemExtension.ItemsLinkageType == 2)
                    //        {
 
                    //        }
                    //        break;
                    //    default:
                    //        break;

                    //}
                }
            }
            return string.Empty;
        }

        [WebMethod(Description = "加密字符串")]
        public string EncryptStrings(string workflowInstanceId, string id, string dataType)
        {
            try
            {
                IList<FormItemInstance> instnaceList = formInstanceService.GetFormItemInstancesByFormInstanceId(new Guid(workflowInstanceId), true);
                foreach (FormItemInstance instnace in instnaceList)
                {
                    if (instnace.FormItemDefinitionId.ToString() == id)
                    {
                        FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(id));
                        if (!string.IsNullOrEmpty(formItemExtension.DataEncode))
                        {
                            string dataEncode = formItemExtension.DataEncode;
                            int start = Convert.ToInt32(dataEncode.Split(':', '：')[0]);
                            int end = Convert.ToInt32(dataEncode.Split(':', '：')[1]);
                            string oldvalue = string.IsNullOrEmpty(instnace.Value) ? instnace.TextValue : instnace.Value;
                            string value = string.IsNullOrEmpty(instnace.Value) ? instnace.TextValue : instnace.Value;
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (value.Length > start)
                                {
                                    if (value.Substring(start - 1).Length > end)
                                    {
                                        string encodeStr = value.Substring(start - 1, value.Length - end - start + 1);
                                        char[] chars = encodeStr.ToCharArray();
                                        for (int i = 0; i < chars.Length; i++)
                                        {
                                            chars[i] = '*';
                                        }
                                        encodeStr = new string(chars);
                                        string startVal = value.Substring(0, start - 1);
                                        string endVal = value.Substring(value.Length - end, end);
                                        value = startVal + encodeStr + endVal;
                                        //return "{\"Type\":\"" + dataType + "\",\"EventName\":\"\",\"FunctionName\":\"\",\"src\":\"\",\"Value\":\"" + value + "\",\"Antetype\":\"" + (string.IsNullOrEmpty(instnace.Value) ? instnace.TextValue : instnace.Value) + "\",\"UseTo\":\"EncryptStrings\"}";
                                        return "{\"Type\":\"" + dataType + "\",\"EventName\":\"\",\"FunctionName\":\"\",\"src\":\"\",\"Value\":\"" + value + "\",\"OldValue\":\"" + oldvalue + "\",\"UseTo\":\"EncryptStrings\"}";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error at EncryptStrings："+ex.ToString());
                return string.Empty;
            }
            return string.Empty;
        }

        [WebMethod(Description = "获取js字段联动")]
        public string GenerItemsRules(string id)
        {
            FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(id));
            if (formItemExtension != null)
            {
                if (formItemExtension.ItemsRulesType > 0)
                {
                    return formItemExtension.ItemsRulesJson;
                }
            }
            return string.Empty;
        }

        [WebMethod(Description = "获取Iframe设置集合")]
        public string GetIFramesSetting(string workflowId, string workflowInstanceId)
        {
            FormDefinition formDefinition = formDefinitionService.GetFormDefinitionByFormInstanceId(new Guid(workflowInstanceId));
            if (formDefinition == null)
                formDefinition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", new Guid(workflowId));

            if (formDefinition != null)
            {
                IList<FormItemDefinition> formItemDefinitions = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(formDefinition.Id);
                IList<FormItemIFrames> formItemIFrameses = formItemIFramesService.SelectByFormDefinitionId(formDefinition.Id);
                IDictionary<Guid, FormItemDefinition> definitionItems = new Dictionary<Guid, FormItemDefinition>();
                StringBuilder json = new StringBuilder();
                if (formItemIFrameses.Count == 0 || formItemDefinitions.Count == 0)
                    return string.Empty;
                foreach (FormItemDefinition item in formItemDefinitions)
                {
                    if (!definitionItems.ContainsKey(item.Id))
                        definitionItems.Add(item.Id, item);
                    
                }
                foreach (FormItemIFrames formItemIFrames in formItemIFrameses)
                {
                    if (definitionItems.ContainsKey(formItemIFrames.FormItemDefinitionId) && formItemIFrames.Enabled == 1)
                        json.Append("{\"Key\":\"" + definitionItems[formItemIFrames.FormItemDefinitionId].FName + "\", \"ID\":\"" + definitionItems[formItemIFrames.FormItemDefinitionId].Id + "\",\"Type\":\"" + definitionItems[formItemIFrames.FormItemDefinitionId].ItemType + "\",\"SettingType\":\"" + formItemIFrames.SettingType + "\",\"ActivityName\":\"" + formItemIFrames.ActivityName + "\",\"Height\":\"" + formItemIFrames.Height + "\",\"Width\":\"" + formItemIFrames.Width + "\"},");
                }
                
                
                if (formItemIFrameses.Count == 0)
                    return string.Empty;

                if (json.Length > 0)
                    json = json.Remove(json.Length - 1, 1);
                //json.Append("}");
                return json.ToString();
            }
            return string.Empty;
        }

        [WebMethod(Description = "获取Iframe设置")]
        public string GenerIFramesSetting(string id, string activityname,int type, string[] dataSet)
        {
            if (type == 0)
            {
                FormItemIFrames formItemIFrames = formItemIFramesService.LoadByFormItemDefinitionIdAndType(new Guid(id), 0);
                if (formItemIFrames != null)
                {
                    IDictionary<string, string> dicts = new Dictionary<string, string>();
                    foreach (string str in dataSet)
                    {
                        if (!dicts.ContainsKey(str.Split('$')[0]))
                            dicts.Add(str.Split('$')[0], str.Split('$')[1]);
                    }
                    foreach (KeyValuePair<string, string> dict in dicts)
                    {
                        formItemIFrames.AccessUrl = formItemIFrames.AccessUrl.Replace(dict.Key, Server.UrlEncode(dict.Value));
                    }
                    return formItemIFrames.AccessUrl;
                }
                else
                    return string.Empty;

            }
            else
            {
                FormItemIFrames formItemIFrames = formItemIFramesService.LoadByFormItemDefinitionIdAndName(new Guid(id), activityname);
                if (formItemIFrames != null)
                {
                    IDictionary<string, string> dicts = new Dictionary<string, string>();
                    foreach (string str in dataSet)
                    {
                        if (!dicts.ContainsKey(str.Split('$')[0]))
                            dicts.Add(str.Split('$')[0], str.Split('$')[1]);
                    }
                    foreach (KeyValuePair<string, string> dict in dicts)
                    {
                        formItemIFrames.AccessUrl = formItemIFrames.AccessUrl.Replace(dict.Key, Server.UrlEncode(dict.Value));
                    }
                    return formItemIFrames.AccessUrl;
                }
                else
                    return string.Empty;
            }
        }

        [WebMethod(Description = "获取DataList设置集合")]
        public string GetDataListSettings(string workflowId, string workflowInstanceId)
        {
            FormDefinition formDefinition = formDefinitionService.GetFormDefinitionByFormInstanceId(new Guid(workflowInstanceId));
            if (formDefinition == null)
                formDefinition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", new Guid(workflowId));

            if (formDefinition != null)
            {
                IList<DataListSetting> settings = itemDataListSettingService.GetDataListSettingByFormDefinitionId(formDefinition.Id);
                IList<FormItemDefinition> formItemDefinitions = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(formDefinition.Id);
                IDictionary<Guid, FormItemDefinition> definitionItems = new Dictionary<Guid, FormItemDefinition>();
                StringBuilder json = new StringBuilder();
                if (settings.Count == 0 || formItemDefinitions.Count == 0)
                    return string.Empty;
                foreach (FormItemDefinition item in formItemDefinitions)
                {
                    if (!definitionItems.ContainsKey(item.Id))
                        definitionItems.Add(item.Id, item);

                }
                foreach (DataListSetting setting in settings)
                {
                    if (definitionItems.ContainsKey(setting.FormItemDefinitionId))
                    {
                        if (definitionItems[setting.FormItemDefinitionId].ItemType.Equals(FormItemDefinition.FormItemType.Html))
                        {
                            json.Append("{\"Wid\":\"" + workflowId + "\",\"Wiid\":\"" + workflowInstanceId + "\",\"Key\":\"" + definitionItems[setting.FormItemDefinitionId].FName + "\", \"ID\":\"" + definitionItems[setting.FormItemDefinitionId].Id + "\",\"Type\":\"" + definitionItems[setting.FormItemDefinitionId].ItemType + "\",\"SettingType\":\"" + setting.Type + "\",\"Columns\":\"" + setting.Columns + "\",\"Rows\":\"" + setting.Rows + "\"},");
                        }
                    }

                }
                if (json.Length > 0)
                    json = json.Remove(json.Length - 1, 1);
                //json.Append("}");
                return json.ToString();
            }
            return string.Empty;
        }

        [WebMethod(Description = "获取DataList设置")]
        public string GenerDataLists(string workflowinstanceid, string formItemDefinitionId, string fName, int type, int columns, int rows, string[] dataSet)
        {
            IList<DataListItemDefinition> itemDefinitions = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(new Guid(formItemDefinitionId));
            IList<DataListItemInstance> itemInstances = dataListInstanceService.GetDataListItemInstancesByFormInstanceIdAndFormItemDefinitionId(new Guid(workflowinstanceid), new Guid(formItemDefinitionId), true);
            IDictionary<int, DataListItemDefinition> dict = new Dictionary<int, DataListItemDefinition>();
            IDictionary<string, string> itemInstanceDict = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            StringBuilder tableStr = new StringBuilder("<table id=\"TB_FORM_DATA_LIST_" + fName + "\" class=\"tblGrayClass grayBackTable table\" style=\"word-break: break-all; text-align:center\" cellspacing=\"1\" cellpadding=\"4\" border=\"0\"><thead>");
            foreach (DataListItemDefinition itemDefinition in itemDefinitions)
            {
                if (itemDefinition.ColumnNumber < columns)
                    dict.Add(itemDefinition.ColumnNumber, itemDefinition);
            }
            if (null != itemInstances && itemInstances.Count > 0)
            {
                foreach (DataListItemInstance itemInstance in itemInstances)
                {
                    string val = String.IsNullOrEmpty(itemInstance.Value) ? "" : itemInstance.Value;
                    if (!itemInstanceDict.ContainsKey(itemInstance.Definition.FName + "_r" + itemInstance.RowNumber.ToString()))
                        itemInstanceDict.Add(itemInstance.Definition.FName + "_r" + itemInstance.RowNumber.ToString(), val);
                }
                //当设置为动态增加时获取最大的行数
                if(type==1)
                    rows = itemInstances.OrderByDescending(i=>i.RowNumber).First().RowNumber+1;
            }
            //当DataList中没有数据时才自动填充
            else
            {
                FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(formItemDefinitionId));
                if(formItemExtension!=null)
                if (formItemExtension.GetDataType > 1)
                {
                    IDictionary<string, string> dictDataSet = new Dictionary<string, string>();
                    foreach (string str in dataSet)
                    {
                        if (!dictDataSet.ContainsKey(str.Split('$')[0]))
                            dictDataSet.Add(str.Split('$')[0], str.Split('$')[1]);
                    }
                    dt = getOuterDataHandler.GetDataTableBySQL(formItemExtension.GetDataSource, formItemExtension.SourceString, dictDataSet);
                    if (type == 1)
                        rows = dt.Rows.Count;
                }
               
            }
            
                tableStr.AppendLine("<tr>");
                for (int j = 0; j < columns; j++)
                {
                    if (dict.ContainsKey(j))
                    {
                        //tableStr.AppendLine("<th style=\"width:150px\"><b>" + dict[j].Name + "</b></th>");
                        tableStr.AppendLine("<th><b>" + dict[j].Name + "</b></th>");
                    }
                }
                if (type == 1)
                {
                    tableStr.AppendLine("<th><a href='javascript:void(0)' onclick='ItemDataList.AddRow(\"TB_FORM_DATA_LIST_" + fName + "\",\""+formItemDefinitionId+"\",\""+fName+"\")'>添加</a></th>");
                }
                tableStr.AppendLine("</tr></thead><tbody>");
                for (int i = 0; i < rows; i++)
                {
                    tableStr.AppendLine("<tr>");
                    for (int j = 0; j < columns; j++)
                    {
                        if (dict.ContainsKey(j))
                        {
                            string val = !itemInstanceDict.ContainsKey(dict[j].FName + "_r" + i.ToString()) ? "" : itemInstanceDict[dict[j].FName + "_r" + i.ToString()];
                            //当可自动获取外部数据源
                            if (string.IsNullOrEmpty(val) && dt.Rows.Count > i && dt.Columns.Count > j)
                            {
                                DataRow dw = dt.Rows[i];
                                val = DbUtils.ToString(dw[j]);
                            }
                            switch (dict[j].ItemType)
                            {
                                case DataListItemDefinition.FormItemType.Text:
                                    if (dict[j].Require)
                                        tableStr.AppendLine("<td><input id=\"bwdf_txt_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_txt_" + dict[j].FName + "_r" + i + "\" value=\"" + val + "\" type=\"text\" require=\"true\" validatetype=\"" + dict[j].ValidateType + "\" msg=\"" + dict[j].ErrorMessage + "\" /><span class=\"require\">*</span></td>");
                                    else
                                        tableStr.AppendLine("<td><input id=\"bwdf_txt_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_txt_" + dict[j].FName + "_r" + i + "\" value=\"" + val + "\" type=\"text\" /></td>");
                                    break;
                                case DataListItemDefinition.FormItemType.TextArea:
                                    if (dict[j].Require)
                                        tableStr.AppendLine("<td><textarea class=\"inputbox\" id=\"bwdf_txa_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_txa_" + dict[j].FName + "_r" + i + "\" onkeyup=\"adjustTextArea(this)\" require=\"true\" validatetype=\"" + dict[j].ValidateType + "\" msg=\"" + dict[j].ErrorMessage + "\">" + val + "</textarea><span class=\"require\">*</span></td>");
                                    else
                                        tableStr.AppendLine("<td><textarea class=\"inputbox\" id=\"bwdf_txa_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_txa_" + dict[j].FName + "_r" + i + "\" onkeyup=\"adjustTextArea(this)\">" + val + "</textarea></td>");
                                    break;
                                case DataListItemDefinition.FormItemType.Date:
                                    if (dict[j].Require)
                                        tableStr.AppendLine("<td><input id=\"bwdf_dat_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_dat_" + dict[j].FName + "_r" + i + "\" value=\"" + val + "\" type=\"text\" require=\"true\" validatetype=\"" + dict[j].ValidateType + "\" msg=\"" + dict[j].ErrorMessage + "\" /><span class=\"ico_pickdate\" title=\"点击选择日期\" onclick='return showCalendar(\"bwdf_dat_" + dict[j].FName + "_r" + i + "\", \"%Y-%m-%d\", \"24\", true);'> </span><span class=\"require\">*</span></td>");
                                    else
                                        tableStr.AppendLine("<td><input id=\"bwdf_dat_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_dat_" + dict[j].FName + "_r" + i + "\" value=\"" + val + "\" type=\"text\" /><span class=\"ico_pickdate\" title=\"点击选择日期\" onclick='return showCalendar(\"bwdf_dat_" + dict[j].FName + "_r" + i + "\", \"%Y-%m-%d\", \"24\", true);'> </span></td>");
                                    break;
                                case DataListItemDefinition.FormItemType.CheckBoxList:
                                    string[] chkSource = dict[j].DataSource.Split(',');
                                    val = ","+val + ",";
                                    tableStr.AppendLine("<td>");
                                    for (int k = 0; k < chkSource.Length; k++)
                                    {
                                        if (k == chkSource.Length - 1 && dict[j].Require)
                                            tableStr.AppendLine("<input type=\"checkbox\" id=\"bwdf_cbl_" + dict[j].FName + "_r" + i + "_" + k + "\" name=\"bwdf_cbl_" + dict[j].FName + "_r" + i + "\" value=\"" + chkSource[k] + "\" " + (val.IndexOf(","+chkSource[k]+",") > -1 ? "checked" : "") + " require=\"true\" validatetype=\"" + dict[j].ValidateType + "\" msg=\"" + dict[j].ErrorMessage + "\"><span>" + chkSource[k] + "</span><span class=\"require\">*</span>");
                                        else
                                            tableStr.AppendLine("<input type=\"checkbox\" id=\"bwdf_cbl_" + dict[j].FName + "_r" + i + "_" + k + "\" name=\"bwdf_cbl_" + dict[j].FName + "_r" + i + "\" value=\"" + chkSource[k] + "\" " + (val.IndexOf("," + chkSource[k] + ",") > -1 ? "checked" : "") + "><span>" + chkSource[k] + "</span>");
                                    }
                                    tableStr.AppendLine("</td>");
                                    break;
                                case DataListItemDefinition.FormItemType.DropDownList:
                                    tableStr.AppendLine("<td>");
                                    string[] ddlSource = dict[j].DataSource.Split(',');
                                    if (dict[j].Require)
                                        tableStr.AppendLine("<select id=\"bwdf_ddl_" + dict[j].FName + "_r" + i + "\" class=\"form_ddl\" msg=\"" + dict[j].ErrorMessage + "\" require=\"true\" validatetype=\"" + dict[j].ValidateType + "\" name=\"bwdf_ddl_" + dict[j].FName + "_r" + i + "\">");
                                    else
                                        tableStr.AppendLine("<select id=\"bwdf_ddl_" + dict[j].FName + "_r" + i + "\" class=\"form_ddl\" name=\"bwdf_ddl_" + dict[j].FName + "_r" + i + "\">");
                                    for (int k = 0; k < ddlSource.Length; k++)
                                    {
                                        tableStr.AppendLine("<option value=\"" + ddlSource[k] + "\" " + (val.Equals(ddlSource[k]) ? "selected" : "") + " >" + ddlSource[k] + "</option>");
                                    }
                                    tableStr.AppendLine("<select>");
                                    if (dict[j].Require)
                                        tableStr.AppendLine("<span class=\"require\">*</span>");
                                    tableStr.AppendLine("</td>");
                                    break;
                                case DataListItemDefinition.FormItemType.RadioButtonList:
                                    tableStr.AppendLine("<td>");
                                    string[] radSource = dict[j].DataSource.Split(',');
                                    for (int k = 0; k < radSource.Length; k++)
                                    {
                                        if (k == radSource.Length - 1 && dict[j].Require)
                                            tableStr.AppendLine("<input type=\"radio\" id=\"bwdf_rbl_" + dict[j].FName + "_r" + i + "_" + k + "\" name=\"bwdf_rbl_" + dict[j].FName + "_r" + i + "\" value=\"" + radSource[k] + "\" " + (val.Equals(radSource[k]) ? "checked" : "") + " require=\"true\" validatetype=\"" + dict[j].ValidateType + "\" msg=\"" + dict[j].ErrorMessage + "\"><span>" + radSource[k] + "</span><span class=\"require\">*</span>");
                                        else
                                            tableStr.AppendLine("<input type=\"radio\" id=\"bwdf_rbl_" + dict[j].FName + "_r" + i + "_" + k + "\" name=\"bwdf_rbl_" + dict[j].FName + "_r" + i + "\" value=\"" + radSource[k] + "\" " + (val.Equals(radSource[k]) ? "checked" : "") + "><span>" + radSource[k] + "</span>");
                                    }
                                    tableStr.AppendLine("</td>");
                                    break;
                            }
                        }
                    }
                    if (type == 1)
                    {
                        tableStr.AppendLine("<td><a href='javascript:void(0)' onclick='ItemDataList.DelRow(this,\"TB_FORM_DATA_LIST_" + fName + "\",\"" + fName + "\")' a_del='1'>删除</a></td>");
                    }
                    tableStr.AppendLine("</tr>");
                }
                tableStr.AppendLine("</tbody></table>");
                ////绑定行数，用于动态行后台数据保存绑定
                tableStr.AppendLine("<input type=\"hidden\" id=\"hid_DataList_rowcnt" + fName + "\" name=\"hid_DataList_rowcnt" + fName + "\" value=\"" + rows + "\"/>");
                return tableStr.ToString();
            //return string.Empty;
        }

        [WebMethod(Description = "DataList添加行")]
        public string AddRow(string formItemDefinitionId, string fName, int rows)
        {
            IList<DataListItemDefinition> itemDefinitions = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(new Guid(formItemDefinitionId));
            //IList<DataListItemInstance> itemInstances = dataListInstanceService.GetDataListItemInstancesByFormInstanceIdAndFormItemDefinitionId(new Guid(workflowinstanceid), new Guid(formItemDefinitionId), true);
            IDictionary<int, DataListItemDefinition> dict = new Dictionary<int, DataListItemDefinition>();
            IDictionary<string, string> itemInstanceDict = new Dictionary<string, string>();
            StringBuilder tableStr = new StringBuilder();
            foreach (DataListItemDefinition itemDefinition in itemDefinitions)
            {
                //if (itemDefinition.ColumnNumber < columns)
                dict.Add(itemDefinition.ColumnNumber, itemDefinition);
            }
            int columns = dict.Count;
            int i = rows;
            tableStr.AppendLine("<tr>");
            for (int j = 0; j < columns; j++)
            {
                if (dict.ContainsKey(j))
                {
                    string val = "";
                    switch (dict[j].ItemType)
                    {
                        case DataListItemDefinition.FormItemType.Text:
                            if (dict[j].Require)
                                tableStr.AppendLine("<td><input id=\"bwdf_txt_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_txt_" + dict[j].FName + "_r" + i + "\" value=\"" + val + "\" type=\"text\" require=\"true\" validatetype=\"" + dict[j].ValidateType + "\" msg=\"" + dict[j].ErrorMessage + "\" /><span class=\"require\">*</span></td>");
                            else
                                tableStr.AppendLine("<td><input id=\"bwdf_txt_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_txt_" + dict[j].FName + "_r" + i + "\" value=\"" + val + "\" type=\"text\" /></td>");
                            break;
                        case DataListItemDefinition.FormItemType.TextArea:
                            if (dict[j].Require)
                                tableStr.AppendLine("<td><textarea class=\"inputbox\" id=\"bwdf_txa_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_txa_" + dict[j].FName + "_r" + i + "\" onkeyup=\"adjustTextArea(this)\" require=\"true\" validatetype=\"" + dict[j].ValidateType + "\" msg=\"" + dict[j].ErrorMessage + "\">" + val + "</textarea><span class=\"require\">*</span></td>");
                            else
                                tableStr.AppendLine("<td><textarea class=\"inputbox\" id=\"bwdf_txa_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_txa_" + dict[j].FName + "_r" + i + "\" onkeyup=\"adjustTextArea(this)\">" + val + "</textarea></td>");
                            break;
                        case DataListItemDefinition.FormItemType.Date:
                            if (dict[j].Require)
                                tableStr.AppendLine("<td><input id=\"bwdf_dat_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_dat_" + dict[j].FName + "_r" + i + "\" value=\"" + val + "\" type=\"text\" /><span class=\"ico_pickdate\" title=\"点击选择日期\" onclick='return showCalendar(\"bwdf_dat_" + dict[j].FName + "_r" + i + "\", \"%Y-%m-%d\", \"24\", true);'> </span><span class=\"require\">*</span></td>");
                            else
                                tableStr.AppendLine("<td><input id=\"bwdf_dat_" + dict[j].FName + "_r" + i + "\" name=\"bwdf_dat_" + dict[j].FName + "_r" + i + "\" value=\"" + val + "\" type=\"text\" /><span class=\"ico_pickdate\" title=\"点击选择日期\" onclick='return showCalendar(\"bwdf_dat_" + dict[j].FName + "_r" + i + "\", \"%Y-%m-%d\", \"24\", true);'> </span></td>");
                            break;
                        case DataListItemDefinition.FormItemType.CheckBoxList:
                            string[] chkSource = dict[j].DataSource.Split(',');
                            val = val + ",";
                            tableStr.AppendLine("<td>");
                            for (int k = 0; k < chkSource.Length; k++)
                            {
                                if (k == chkSource.Length - 1 && dict[j].Require)
                                    tableStr.AppendLine("<input type=\"checkbox\" id=\"bwdf_cbl_" + dict[j].FName + "_r" + i + "_" + k + "\" name=\"bwdf_cbl_" + dict[j].FName + "_r" + i + "\"_" + k + "\" value=\"" + chkSource[k] + "\" " + (val.IndexOf(chkSource[k]) > -1 ? "checked" : "") + " require=\"true\" validatetype=\"" + dict[j].ValidateType + "\" msg=\"" + dict[j].ErrorMessage + "\"><span>" + chkSource[k] + "</span><span class=\"require\">*</span>");
                                else
                                    tableStr.AppendLine("<input type=\"checkbox\" id=\"bwdf_cbl_" + dict[j].FName + "_r" + i + "_" + k + "\" name=\"bwdf_cbl_" + dict[j].FName + "_r" + i + "\"_" + k + "\" value=\"" + chkSource[k] + "\" " + (val.IndexOf(chkSource[k]) > -1 ? "checked" : "") + "><span>" + chkSource[k] + "</span>");
                            }
                            tableStr.AppendLine("</td>");
                            break;
                        case DataListItemDefinition.FormItemType.DropDownList:
                            tableStr.AppendLine("<td>");
                            string[] ddlSource = dict[j].DataSource.Split(',');
                            if (dict[j].Require)
                                tableStr.AppendLine("<select id=\"bwdf_ddl_" + dict[j].FName + "_r" + i + "\" class=\"form_ddl\" msg=\"" + dict[j].ErrorMessage + "\" require=\"true\" validatetype=\"" + dict[j].ValidateType + "\" name=\"bwdf_ddl_" + dict[j].FName + "_r" + i + "\"><span class=\"require\">*</span>");
                            else
                                tableStr.AppendLine("<select id=\"bwdf_ddl_" + dict[j].FName + "_r" + i + "\" class=\"form_ddl\" name=\"bwdf_ddl_" + dict[j].FName + "_r" + i + "\">");
                            for (int k = 0; k < ddlSource.Length; k++)
                            {
                                tableStr.AppendLine("<option value=\"" + ddlSource[k] + "\" " + (val.Equals(ddlSource[k]) ? "selected" : "") + " >" + ddlSource[k] + "</option>");
                            }
                            tableStr.AppendLine("<select>");
                            tableStr.AppendLine("</td>");
                            break;
                        case DataListItemDefinition.FormItemType.RadioButtonList:
                            tableStr.AppendLine("<td>");
                            string[] radSource = dict[j].DataSource.Split(',');
                            for (int k = 0; k < radSource.Length; k++)
                            {
                                if (k == radSource.Length - 1 && dict[j].Require)
                                    tableStr.AppendLine("<input type=\"radio\" id=\"bwdf_rbl_" + dict[j].FName + "_r" + i + "_" + k + "\" name=\"bwdf_rbl_" + dict[j].FName + "_r" + i + "\"_" + k + "\" value=\"" + radSource[k] + "\" " + (val.Equals(radSource[k]) ? "checked" : "") + " require=\"true\" validatetype=\"" + dict[j].ValidateType + "\" msg=\"" + dict[j].ErrorMessage + "\"><span>" + radSource[k] + "</span><span class=\"require\">*</span>");
                                else
                                    tableStr.AppendLine("<input type=\"radio\" id=\"bwdf_rbl_" + dict[j].FName + "_r" + i + "_" + k + "\" name=\"bwdf_rbl_" + dict[j].FName + "_r" + i + "\"_" + k + "\" value=\"" + radSource[k] + "\" " + (val.Equals(radSource[k]) ? "checked" : "") + "><span>" + radSource[k] + "</span>");
                            }
                            tableStr.AppendLine("</td>");
                            break;
                    }
                }
            }
            tableStr.AppendLine("<td><a href='javascript:void(0)' onclick='ItemDataList.DelRow(this,\"TB_FORM_DATA_LIST_" + fName + "\",\"" + fName + "\")' a_del='1'>删除</a></td>");

            tableStr.AppendLine("</tr>");
            return tableStr.ToString();
        }

        #region activityrules
        [WebMethod(Description = "获取路由规则设置")]
        public string GenerRules(string workflowinstanceid, string workflowid,string activityname,string nextactivityname, string actor, string[] dataSet, bool isInit)
        {
            RulesDetail rulesDetail = activityRulesService.GetNextActivityRules(workflowid,activityname,nextactivityname);
            if (rulesDetail == null)
                return "{\"result\":\"none\"}";
            IDictionary<string, object> dict = new Dictionary<string, object>();
            foreach (string str in dataSet)
            {
                string key = str.Split('$')[0];
                key = key.Substring(1, key.Length - 2);
                if (!dict.ContainsKey(key))
                    dict.Add(key, str.Split('$')[1]);
            }
            DataTable dtPreview = activityRulesService.GetInstanceTable(new Guid(workflowid),new Guid(workflowinstanceid),actor,dict,isInit);
            bool isShow = activityRulesService.GetActivityRulesAnalysisResult(rulesDetail,dtPreview);
            return "{\"result\":\""+isShow.ToString().ToLower()+"\",\"AssemblyInfo\":\"" + rulesDetail.FieldsAssemble + "\"}";
        }

        [WebMethod(Description = "获取子流程路由规则设置")]
        public string GenerRelationRules(string workflowinstanceid, string workflowid, string activityname, string actor, bool isInit)
        {
            ActivityDefinition activityDefinition = activityDefinitionService.GetActivitiesByWorkflowId(new Guid(workflowid)).Where(a => a.ActivityName == activityname).First();
            if (activityDefinition == null)
                return "{\"result\":\"none\"}";
            DataTable rSettings = APIServiceSQLHelper.QueryForDataSet("cz_WorkflowRelationSettingTable_Select_By_Aid", activityDefinition.ActivityId);
            if (rSettings.Rows.Count == 0)
                return "{\"result\":\"none\"}";
            int operateType = DbUtils.ToInt32(rSettings.Rows[0]["OperateType"]);
            if (operateType == 0)
                return "{\"result\":\"none\"}";
            //获取子流程工单集合
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", new Guid(workflowid));
            parameters.Add("WorkflowInstanceId", new Guid(workflowinstanceid));
            DataTable RelationWorkflowInstanceState = APIServiceSQLHelper.QueryForDataSet("cz_RelationWorkflowInstanceState_Select", parameters);
            if (operateType == 1)
            {
                foreach (DataRow dw in RelationWorkflowInstanceState.Rows)
                {
                    if (DbUtils.ToInt32(dw["state"]) == 1)
                    {//子流程工单正在流转
                        return "{\"result\":\"false\",\"AssemblyInfo\":\"\"}";
                    }
                }
            }
            else
            {
                RulesDetail rulesDetail = activityRulesService.GetRelationRules(workflowid, activityname)[0];
                if (rulesDetail == null)
                    return "{\"result\":\"none\"}";
                IDictionary<string, object> dict = new Dictionary<string, object>();
                bool isShow = true;
                /**
                 * 逐个工单遍历，获取工单中的字段内容，根据字段内容过滤规则，只要有一个规则不符合就立刻跳出循环 
                 */
                foreach (DataRow dw in RelationWorkflowInstanceState.Rows)
                {
                    DataTable dtPreview = activityRulesService.GetInstanceTable(new Guid(workflowid), new Guid(DbUtils.ToString(dw["workflowinstanceid"])), actor, dict, isInit);
                    isShow = activityRulesService.GetActivityRulesAnalysisResult(rulesDetail, dtPreview);
                    if (!isShow)
                        break;
                }
                return "{\"result\":\"" + isShow.ToString().ToLower() + "\",\"AssemblyInfo\":\"" + rulesDetail.FieldsAssemble + "\"}";
            }
            return "{\"result\":\"none\"}";
        }
        #endregion
    }
}
