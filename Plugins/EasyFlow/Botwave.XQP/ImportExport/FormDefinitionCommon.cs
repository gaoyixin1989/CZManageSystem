using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;

namespace Botwave.XQP.ImportExport
{
    /// <summary>
    /// 导入导出的公有类.
    /// </summary>
    public class FormDefinitionCommon
    {
        # region properties

        /// <summary>
        /// 动态表单的表单项类型.
        /// </summary>
        private static FormItemDefinition.FormItemType[] formItemType = {
                    FormItemDefinition.FormItemType.Text,
                    FormItemDefinition.FormItemType.TextArea,
                    FormItemDefinition.FormItemType.RadioButtonList,
                    FormItemDefinition.FormItemType.CheckBoxList,
                    FormItemDefinition.FormItemType.DropDownList,
                    FormItemDefinition.FormItemType.Date,
                    FormItemDefinition.FormItemType.File
                };
        /// <summary>
        /// 模板的题目类型.
        /// </summary>
        private static string[] templateItemType = {
                    "单行填空",
                    "多行填空",
                    "单选题",
                    "多选题",
                    "单选题(下拉框)",
                    "日期",
                    "附件题"
                };
        /// <summary>
        /// 动态表单的验证类型.
        /// </summary>
        private static string[] formValidateType = {
                    @"^[\s]",
                    @"^[\s\S]",
                    @"^\w{1,20}@\w{1,20}\.\w{1,10}$",
                    @"^13[0-9]{9}$",
                    @"^(0(10|2[0-57-9]|[3-9]\d{2})-)?\d{7,8}$",
                    @"^[1-9]\d{5}$",
                    @"^(([0-9]{14}[x0-9]{1})|([0-9]{17}[x0-9]{1}))$",
                    @"^[a-zA-Z\.-]+$",
                    @"^(-?[0-9]*[.]*[0-9]{0,3})$",
                    @"^[a-zA-Z0-9\.-]+$",
                    @"^-?[1-9]\d*$",
                    @"^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$",
                    @"^(ftp://|http://)?\w+(\.\w+)*(:\d+)?$",
                    @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$"
                };
        /// <summary>
        /// 模板的常用校验.
        /// </summary>
        private static string[] templateValidateType = {
                    "普通文字型",
                    "超长文字型",
                    "电子邮件",
                    "手机号码",
                    "电话号码",
                    "邮政编码",
                    "身份证号码",
                    "英文字母组成的字符串",
                    "数字组成的字符串",
                    "英文字母(数字)组成的字符串",
                    "整数",
                    "浮点数",
                    "URL地址",
                    "日期"
                };

        #endregion

        /// <summary>
        /// 通过(表单项)类型来获取对应的(模板)类型.
        /// </summary>
        public static readonly IDictionary<FormItemDefinition.FormItemType, string> FormItemTypeKeyDictionary = Set_ItemTypeFormKey_TemplateValue();
        /// <summary>
        /// 通过(模板)类型来获取对应的(表单项)类型.
        /// </summary>
        public static readonly IDictionary<string, FormItemDefinition.FormItemType> TemplateItemTypeKeyDictionary = Set_ItemTypeTemplateKey_FormValue();
        /// <summary>
        /// 通过(表单项)验证来获取对应的(模板)验证.
        /// </summary>
        public static readonly IDictionary<string, string> FormValidateTypeKeyDictionary = Set_FormValidateTypeKey_TemplateValidateTypeValue();
        /// <summary>
        /// 通过(模板)验证来获取对应的(表单项)验证.
        /// </summary>
        public static readonly IDictionary<string, string> TemplateValidateTypeKeyDictionary = Set_TemplateValidateTypeKey_FormValidateTypeValue();

        # region 设置字典值对.

        /// <summary>
        /// 设置 表单项为key,模板项为value.
        /// </summary>
        /// <returns></returns>
        private static IDictionary<FormItemDefinition.FormItemType, string> Set_ItemTypeFormKey_TemplateValue()
        {
            IDictionary<FormItemDefinition.FormItemType, string> result = new Dictionary<FormItemDefinition.FormItemType, string>();

            if (formItemType.Length == templateItemType.Length)
            {
                int length = formItemType.Length;
                for (int i = 0; i < length; i++)
                {
                    result.Add(formItemType[i], templateItemType[i]);
                }
            }
            else
                throw new Exception("表单项与模板项个数不一致.");

            return result;
        }
        /// <summary>
        /// 设置 模板项为key,表单项为value.
        /// </summary>
        /// <returns></returns>
        private static IDictionary<string, FormItemDefinition.FormItemType> Set_ItemTypeTemplateKey_FormValue()
        {
            IDictionary<string, FormItemDefinition.FormItemType> result = new Dictionary<string, FormItemDefinition.FormItemType>();

            if (templateItemType.Length == formItemType.Length)
            {
                int length = templateItemType.Length;
                for (int i = 0; i < length; i++)
                {
                    result.Add(templateItemType[i], formItemType[i]);
                }
            }
            else
                throw new Exception("模板项与表单项个数不一致.");

            return result;
        }

        /// <summary>
        /// 设置 表单校验为key,模板校验为value.
        /// </summary>
        /// <returns></returns>
        private static IDictionary<string, string> Set_FormValidateTypeKey_TemplateValidateTypeValue()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            if (formValidateType.Length == templateValidateType.Length)
            {
                int length = formValidateType.Length;
                for (int i = 0; i < length; i++)
                {
                    result.Add(formValidateType[i], templateValidateType[i]);
                }
            }
            else
                throw new Exception("模板项与表单项个数不一致.");

            return result;
        }

        /// <summary>
        /// 设置 模板校验为key,表单校验为value.
        /// </summary>
        /// <returns></returns>
        private static IDictionary<string, string> Set_TemplateValidateTypeKey_FormValidateTypeValue()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            if (templateValidateType.Length == formValidateType.Length)
            {
                int length = templateValidateType.Length;
                for (int i = 0; i < length; i++)
                {
                    result.Add(templateValidateType[i], formValidateType[i]);
                }
            }
            else
                throw new Exception("模板项与表单项个数不一致.");

            return result;
        }

        #endregion


        #region 模板验证

        /// <summary>
        /// 模板标题验证.
        /// </summary>
        public static bool CheckTitleName(DataTable dt, string[] titleNames)
        {
            foreach (string titleName in titleNames)
            {
                if (dt.Columns.IndexOf(titleName) < 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 根据标题判断模板值是否为空 
        /// 1：表示表示所指定列中所有列都不能为空，否则返回false
        /// 0：表示表示指定列中所有列不能同时为空，否则返回false
        /// </summary>
        /// <param name="objTitles"></param>
        /// <param name="andOr"></param>
        /// <returns></returns>
        public static bool CheckTitleContentIsNoEmpty(object[] objTitles, int andOr)
        {
            bool right = false;
            if (andOr == 1)
            {
                foreach (object obj in objTitles)
                {
                    if (obj == null && String.IsNullOrEmpty(obj.ToString()))
                    {
                        right = false;
                        break;
                    }
                }
            }
            else
            {
                foreach (object obj in objTitles)
                {
                    if (obj != null && !String.IsNullOrEmpty(obj.ToString()))
                    {
                        right = true;
                        break;
                    }
                }
            }
            return right;
        }

        #endregion
    }
}
