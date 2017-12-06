using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm.Domain;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 表单字段定义.
    /// </summary>
    public class FormField : Botwave.DynamicForm.Domain.FormItemInstance
    {
        public const string VariblePattern = "#{0}#";
        private FormItemDefinition.DataType _itemDataType;
        private string _fieldName;

        /// <summary>
        /// 表单项数据类型.
        /// </summary>
        public  FormItemDefinition.DataType ItemDataType
        {
            get { return _itemDataType; }
            set { _itemDataType = value; }
        }

        /// <summary>
        /// 字段名称.
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        /// <summary>
        /// 获取指定流程实例的字段数据列表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static IList<FormField> GetFieldsByWorkflowInstanceId(Guid workflowInstanceId)
        {
            return IBatisMapper.Select<FormField>("bwdf_MessageTemplate_Select_Fields_By_WorkflowInstanceId", workflowInstanceId);
        }

        /// <summary>
        /// 变量正则表达式.
        /// </summary>
        private static Regex FiledRegex = new Regex("#[^#]+#", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        
        /// <summary>
        /// 格式化内容.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static string FormatMessage(string message,  Guid workflowInstanceId)
        {
            if (!FiledRegex.IsMatch(message))
                return message;
            return FormatMessage(message, FormField.GetFieldsByWorkflowInstanceId(workflowInstanceId));
        }

        /// <summary>
        /// 格式化内容.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string FormatMessage(string message, IList<FormField> items)
        {
            if (string.IsNullOrEmpty(message) || items == null || items.Count == 0)
                return message;

            foreach (FormField item in items)
            {
                if (!FiledRegex.IsMatch(message))
                    break;
                string varibleName = string.Format(VariblePattern, item.FieldName);
                Regex regex = new Regex(varibleName, RegexOptions.IgnoreCase);

                string value = string.Empty;
                if (item.ItemDataType == FormItemDefinition.DataType.Text)
                    value = String.IsNullOrEmpty(item.TextValue) ? "" : item.TextValue;
                else if (item.ItemDataType == FormItemDefinition.DataType.Decimal)
                    value = String.IsNullOrEmpty(item.DecimalValue.ToString()) ? "" : item.DecimalValue.ToString();
                else
                    value = String.IsNullOrEmpty(item.Value) ? string.Empty : item.Value;

                message = regex.Replace(message, value);
            }
            return message;
        }

        public static bool ExistField(Guid formDefinitionID, string fieldName, Guid itemID)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("FormDefinitionId", formDefinitionID);
            parameters.Add("FName", fieldName);
            parameters.Add("ID", itemID);

            int result = IBatisMapper.Mapper.QueryForObject<int>("bwdf_FormFields_Select_Count_By_FieldName", parameters);
            return (result >= 1);
        }
    }
}
