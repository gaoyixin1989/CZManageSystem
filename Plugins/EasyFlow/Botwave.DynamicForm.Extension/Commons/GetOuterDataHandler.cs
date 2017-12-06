using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.DynamicForm.Extension.Domain;
using System.Collections;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using System.Data;
using System.Data.OracleClient;
using Botwave.Workflow.Domain;
using System.Data.SqlClient;

namespace Botwave.DynamicForm.Extension.Commons
{
    public class GetOuterDataHandler : IGetOuterDataHandler
    {
        #region properties
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(GetOuterDataHandler));
        private IFormDefinitionService formDefinitionService;
        private IGetDataService getDataService;

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
        #endregion

        public virtual string getDataJson(FormItemExtension item, string dataType, IDictionary<string, string> dict)
        {
            string dataSource = item.GetDataSource;
            string sourceString = item.SourceString;
            switch (item.GetDataType)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    return GetDataJsonBySQL(item.GetDataSource, item.SourceString, dataType, dict);
                    break;
                case 3:
                    return GetDataByWSJson(item.GetDataSource, item.SourceString, dataType, dict);
                    break;
                default:
                    break;
            }
            return string.Empty;
        }

        /// <summary>
        /// 自动计算 加减乘除
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual string GenerAutoFull(ActivityDefinition activityDefinition, Guid workflowInstanceid)
        {
            Guid entityId = activityDefinition.WorkflowId;
            FormDefinition formDefinition = formDefinitionService.GetFormDefinitionByFormInstanceId(workflowInstanceid);
            if (formDefinition == null)
                formDefinition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", entityId);
            if (formDefinition != null)
            {
                try
                {
                    IList<FormItemExtension> result = getDataService.GetFormItemExtensionSettingsByFormdefinitionIdAndGetDataType(formDefinition.Id, 1);
                    if (result.Count > 0)
                    {
                        //FormDefinition formDefinition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", entityId);
                        IList<FormItemDefinition> formItemDefinitions = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(formDefinition.Id);
                        IDictionary<string, string> dict = new Dictionary<string, string>();
                        StringBuilder scriptStr = new StringBuilder();
                        scriptStr.AppendLine("$(document).ready(function(){");
                        foreach (FormItemDefinition item in formItemDefinitions)
                        {
                            if (!dict.ContainsKey(item.FName))
                            {
                                if (item.ItemType == FormItemDefinition.FormItemType.Text)
                                    dict.Add(item.FName, "bwdf_" + FormItemDefinition.FormItemPrefix.Text + item.FName);
                                if (item.ItemType == FormItemDefinition.FormItemType.TextArea)
                                    dict.Add(item.FName, "bwdf_" + FormItemDefinition.FormItemPrefix.TextArea + item.FName);
                                if (item.ItemType == FormItemDefinition.FormItemType.DropDownList)
                                    dict.Add(item.FName, "bwdf_" + FormItemDefinition.FormItemPrefix.DropDownList + item.FName);
                                //dict.Add(item.FName, item.ItemType.ToString());
                            }
                        }

                        foreach (FormItemExtension item in result)
                        {
                            string sourceString = item.SourceString;
                            string fId = string.Empty;
                            FormItemDefinition formItemDefinition = formDefinitionService.GetFormItemDefinitionById(item.FormItemDefinitionId);
                            if (formItemDefinition.ItemType == FormItemDefinition.FormItemType.Text)
                                fId = "bwdf_" + FormItemDefinition.FormItemPrefix.Text + formItemDefinition.FName;
                            else if (formItemDefinition.ItemType == FormItemDefinition.FormItemType.TextArea)
                                fId = "bwdf_" + FormItemDefinition.FormItemPrefix.TextArea + formItemDefinition.FName;
                            else if (formItemDefinition.ItemType == FormItemDefinition.FormItemType.DropDownList)
                                fId = "bwdf_" + FormItemDefinition.FormItemPrefix.DropDownList + formItemDefinition.FName;
                            scriptStr.AppendLine("if($(\"#" + fId + "\").attr(\"contentEditable\") == \"true\" || " + activityDefinition.State + " == 0){");
                            foreach (KeyValuePair<string, string> temp in dict)
                            {

                                sourceString = sourceString.Replace("@" + temp.Key + "@", "parseFloat($('#" + temp.Value + "').val())");

                            }
                            foreach (KeyValuePair<string, string> temp in dict)
                            {

                                if (sourceString.IndexOf("parseFloat($('#" + temp.Value + "').val())") != -1)
                                {
                                    scriptStr.AppendLine(string.Format("$('#{0}').blur({1});", temp.Value, "function(){var calcVal = " + sourceString + "; calcVal=Math.round(calcVal*100)/100;  $('#" + fId + "').val(calcVal);}"));
                                }
                            }

                            scriptStr.AppendLine(" var calcTmpVal=" + sourceString + ";calcTmpVal=Math.round(calcTmpVal*100)/100;$(\"#" + fId + "\").attr(\"value\",calcTmpVal);}");

                        }
                        scriptStr.AppendLine("});");
                        //scriptStr.AppendLine("function setValue");
                        return scriptStr.ToString();
                    }
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    log.Error("GenerAutoFull Error" + ex.ToString());
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过sql获取数据源，返回DataTable集合
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="sql"></param>
        /// <param name="dataType"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySQL(string dataSource, string sql, IDictionary<string, string> dict)
        {
            sql = FilterSqlString(sql, dict);
            if (string.IsNullOrEmpty(dataSource))
            {
                try
                {
                    DataTable dtResult = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
                    return dtResult;
                }
                catch (Exception ex)
                {
                    log.Error("GetSQLData Error" + ex.ToString());
                    return new DataTable() ;
                }
            }
            else
            {
                SqlConnection conn = new SqlConnection(dataSource);
                try
                {
                    DataTable dtResult = IBatisDbHelper.ExecuteDataset(conn, CommandType.Text, sql, null).Tables[0];
                    return dtResult;
                }
                catch (Exception ex)
                {
                    if (conn.State.Equals(ConnectionState.Open))
                        conn.Close();
                    log.Error("GetSQLData Error" + ex.ToString());
                    return new DataTable();
                }
            }
        }
        /// <summary>
        /// 通过sql获取数据源
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="sql"></param>
        /// <param name="dataType"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        private string GetDataJsonBySQL(string dataSource, string sql, string dataType, IDictionary<string, string> dict)
        {
            sql = FilterSqlString(sql, dict);
            if (string.IsNullOrEmpty(dataSource))
            {
                try
                {
                    DataTable dtResult = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
                    if (dtResult.Rows.Count > 0)
                    {
                        switch (dataType)
                        {
                            case "Text":
                                return "{\"Type\":\""+dataType+"\",\"Value\":\""+DbUtils.ToString(dtResult.Rows[0][0]+"\"}");
                                break;
                            case "TextArea":
                                return "{Type:\"" + dataType + "\",\"Value\":\"" + DbUtils.ToString(dtResult.Rows[0][0] + "\"}");
                                break;
                            case "Date":
                                return "{Type:\"" + dataType + "\",\"Value\":\"" + DbUtils.ToString(dtResult.Rows[0][0] + "\"}");
                                break;
                            case "CheckBoxList":
                                StringBuilder sb = new StringBuilder("{\"Type\":\""+dataType+"\",\"Value\":[");
                                for (int i = 0; i < dtResult.Rows.Count; i++)
                                {
                                    sb.Append("{\"Value\":\""+DbUtils.ToString(dtResult.Rows[i][0])+"\"}," );
                                }
                                sb = sb.Remove(sb.Length - 1, 1);
                                sb.Append("]}");
                                    return sb.ToString();
                                    break;
                            case "DropDownList":
                                    StringBuilder sbs = new StringBuilder("{\"Type\":\""+dataType+"\",\"Value\":[");
                                    for (int i = 0; i < dtResult.Rows.Count; i++)
                                    {
                                        sbs.Append("{\"Value\":\""+DbUtils.ToString(dtResult.Rows[i][0])+"\"}," );
                                    }
                                    sbs = sbs.Remove(sbs.Length - 1, 1);
                                    sbs.Append("]}");
                                    return sbs.ToString();
                                break;
                            case "RadioButtonList":
                                StringBuilder sbr = new StringBuilder("{\"Type\":\""+dataType+"\",\"Value\":[");
                                for (int i = 0; i < dtResult.Rows.Count; i++)
                                {
                                    sbr.Append("{\"Value\":\""+DbUtils.ToString(dtResult.Rows[i][0])+"\"}," );
                                }
                                sbr = sbr.Remove(sbr.Length - 1, 1);
                                sbr.Append("]}");
                                return sbr.ToString();
                                break;
                            default:
                                return "{Type:\"" + dataType + "\",\"Value\":\"\"}";
                                break;

                        }
                    }
                    return "{\"Type\":\""+dataType+"\",\"Value\":\"\"}";
                }
                catch (Exception ex)
                {
                    log.Error("GetSQLData Error" + ex.ToString());
                    return "{\"Type\":\""+dataType+"\",\"Value\":\"\"}";
                }
            }
            else
            {
                SqlConnection conn = new SqlConnection(dataSource);
                try
                {
                    conn.Open();
                    DataTable dtResult = IBatisDbHelper.ExecuteDataset(conn, CommandType.Text, sql, null).Tables[0];
                    if (dtResult.Rows.Count > 0)
                    {
                        switch (dataType)
                        {
                            case "Text":
                                return "{Type:\"" + dataType + "\",\"Value\":\"" + DbUtils.ToString(dtResult.Rows[0][0] + "\"}");
                                break;
                            case "TextArea":
                                return "{Type:\"" + dataType + "\",\"Value\":\"" + DbUtils.ToString(dtResult.Rows[0][0] + "\"}");
                                break;
                            case "Date":
                                return "{Type:\"" + dataType + "\",\"Value\":\"" + DbUtils.ToString(dtResult.Rows[0][0] + "\"}");
                                break;
                            case "CheckBoxList":
                                StringBuilder sb = new StringBuilder("{Type:\"" + dataType + "\",\"Value\":[");
                                for (int i = 0; i < dtResult.Rows.Count; i++)
                                {
                                    sb.Append("{\"Value\":\"" + DbUtils.ToString(dtResult.Rows[i][0]) + "\"},");
                                }
                                sb = sb.Remove(sb.Length - 1, 1);
                                sb.Append("]}");
                                return sb.ToString();
                                break;
                            case "DropDownList":
                                StringBuilder sbs = new StringBuilder("{Type:\"" + dataType + "\",\"Value\":[");
                                for (int i = 0; i < dtResult.Rows.Count; i++)
                                {
                                    sbs.Append("{\"Value\":\"" + DbUtils.ToString(dtResult.Rows[i][0]) + "\"},");
                                }
                                sbs = sbs.Remove(sbs.Length - 1, 1);
                                sbs.Append("]}");
                                return sbs.ToString();
                                break;
                            case "RadioButtonList":
                                StringBuilder sbr = new StringBuilder("{Type:\"" + dataType + "\",\"Value\":[");
                                for (int i = 0; i < dtResult.Rows.Count; i++)
                                {
                                    sbr.Append("{\"Value\":\"" + DbUtils.ToString(dtResult.Rows[i][0]) + "\"},");
                                }
                                sbr = sbr.Remove(sbr.Length - 1, 1);
                                sbr.Append("]}");
                                return sbr.ToString();
                                break;
                            default:
                                return "{Type:\"" + dataType + "\",\"Value\":\"\"}";
                                break;

                        }
                    }
                    return "{\"Type\":\""+dataType+"\",\"Value\":\"\"}";
                }
                catch (Exception ex)
                {
                    if (conn.State.Equals(ConnectionState.Open))
                        conn.Close();
                    log.Error("GetSQLData Error" + ex.ToString());
                    return "{\"Type\":\""+dataType+"\",\"Value\":\"\"}";;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private string GetDataByWSJson(string dataSource, string methodAndParameters, string dataType, IDictionary<string, string> dict)
        {
            if (!string.IsNullOrEmpty(dataSource) && !string.IsNullOrEmpty(methodAndParameters))
            {
                try
                {
                    string method = methodAndParameters.Split(':', '：')[0];
                    string prams = methodAndParameters.Split(':', '：')[1];
                    ArrayList arr = new ArrayList();
                    foreach (string pram in prams.Split(',', '，'))
                    {
                        string pm = FilterWSParamsString(pram, dict);
                        arr.Add(pm);
                    }
                    string[] newArr = new string[arr.Count];
                    arr.CopyTo(newArr);
                    object result = WebServices.WebServicesHelper.InvokeWebService(dataSource, method, newArr);
                    if (result != null)
                    {
                        switch (dataType)
                        {
                            case "Text":
                                return "{\"Type\":\"" + dataType + "\",\"Value\":\"" + DbUtils.ToString(result + "\"}");
                                break;
                            case "TextArea":
                                return "{Type:\"" + dataType + "\",\"Value\":\"" + DbUtils.ToString(result + "\"}");
                                break;
                            case "Date":
                                return "{Type:\"" + dataType + "\",\"Value\":\"" + DbUtils.ToString(result + "\"}");
                                break;
                            case "CheckBoxList":
                                StringBuilder sb = new StringBuilder("{\"Type\":\"" + dataType + "\",\"Value\":[");
                                //for (int i = 0; i < dtResult.Rows.Count; i++)
                                //{
                                sb.Append(result);
                                //}
                                //sb = sb.Remove(sb.Length - 1, 1);
                                sb.Append("]}");
                                return sb.ToString();
                                break;
                            case "DropDownList":
                                StringBuilder sbs = new StringBuilder("{\"Type\":\"" + dataType + "\",\"Value\":[");
                                sbs.Append(result);
                                sbs = sbs.Remove(sbs.Length - 1, 1);
                                sbs.Append("]}");
                                return sbs.ToString();
                                break;
                            case "RadioButtonList":
                                StringBuilder sbr = new StringBuilder("{\"Type\":\"" + dataType + "\",\"Value\":[");
                                sbr.Append(result);
                                sbr = sbr.Remove(sbr.Length - 1, 1);
                                sbr.Append("]}");
                                return sbr.ToString();
                                break;
                            default:
                                return "{Type:\"" + dataType + "\",\"Value\":\"\"}";
                                break;

                        }
                    }
                    return "{\"Type\":\"" + dataType + "\",\"Value\":\"\"}";
                }
                catch (Exception ex)
                {
                    log.Error("GetSQLData Error" + ex.ToString());
                    return "{\"Type\":\"" + dataType + "\",\"Value\":\"\"}";
                }
            }
            return "{\"Type\":\"" + dataType + "\",\"Value\":\"\"}";
        }

        /// <summary>
        /// 过滤sql条件参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        private string FilterSqlString(string sql,IDictionary<string, string> dataSet)
        {
            foreach (KeyValuePair<string, string> dict in dataSet)
            {
                sql = sql.Replace(dict.Key, "'" + dict.Value + "'");
            }
            return sql;
        }


        /// <summary>
        /// 过滤WebService参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        private string FilterWSParamsString(string prams, IDictionary<string, string> dataSet)
        {
            foreach (KeyValuePair<string, string> dict in dataSet)
            {
                prams = prams.Replace(dict.Key, dict.Value);
            }
            return prams;
        }
    }
}
