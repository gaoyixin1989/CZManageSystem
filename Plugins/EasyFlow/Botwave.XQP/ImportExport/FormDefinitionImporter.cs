using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.IO;
using System.Web.UI;

using Botwave.Commons;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Renders;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.ImportExport
{
    /// <summary>
    /// 表单定义导入.
    /// </summary>
    public class FormDefinitionImporter : IFormDefinitionImporter
    {
        # region service properties

        private string sheetName = "Sheet1";
        private string[] titleNames = new string[] { "标题", "必填项", "题目类型", "常用校验", "备注", "选项" };
  
        private IFormDefinitionService formDefinitionService;
        private IRenderStrategy renderStrategy;      

        public IFormDefinitionService FormDefinitionService
        {
            set { this.formDefinitionService = value; }
        }
        public IRenderStrategy RenderStrategy
        {
            set { this.renderStrategy = value; }
        }

        # endregion

        #region IFormDefinitionImporter 成员

        /// <summary>
        /// 导入Excel.
        /// </summary>
         /// <returns></returns>
        public ImportExport.ActionResult Import(Guid formDefinitionId, string name, string filename)
        {
            ImportExport.ActionResult actionResult = new ImportExport.ActionResult();
            string userName = Botwave.Security.LoginHelper.UserName;
            try
            {
                DataSet ds = ExcelUtils.ReadExcel(filename, sheetName);
                if (!FormDefinitionCommon.CheckTitleName(ds.Tables[0], titleNames))
                {
                    actionResult.Success = false;
                    actionResult.Message = "模板格式不对！模板应包含字段：标题、必填项、题目类型、常用校验、备注、选项";
                    return actionResult;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    actionResult = Excel2Form(formDefinitionId, ds, name, userName);
                }
                else
                {
                    actionResult.Success = false;
                    actionResult.Message = "导入的模板内容为空,请填写好模板后再导入.";
                }
            }
            catch (Exception e)
            {
                actionResult.Success = false;
                actionResult.Message = e.Message;
            }
            return actionResult;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 将Excel模板转换为动态表单.
        /// </summary>
        private ImportExport.ActionResult Excel2Form(Guid formDefinitionId, DataSet ds, string formName, string creator)
        {
            ImportExport.ActionResult actionResult = new ImportExport.ActionResult();

            //验证用户上传的模板内容是否符合要求.
            string errorMessage = string.Empty;
            if (!CheckTemplateContent(ds.Tables[0], ref errorMessage))
            {
                actionResult.Success = false;
                actionResult.Message = errorMessage;
                return actionResult;
            }

            FormDefinition definition = new FormDefinition();
            Guid returnId = Guid.Empty;

            IBatisNet.DataMapper.ISqlMapper mapper = IBatisMapper.Mapper;

            try
            {
                mapper.BeginTransaction();
                if (Guid.Empty != formDefinitionId)
                {
                    definition = formDefinitionService.GetFormDefinitionById(formDefinitionId);
                }
                if (null == definition)
                {
                    definition = new FormDefinition();
                }
                definition.Name = formName;
                definition.Version = GetFormVersion(formName);
                definition.Creator = creator;
                if (Guid.Empty != definition.Id)
                {
                    //formDefinitionService.UpdateFormDefinition(definition);
                    this.UpdateFormDefinition(definition);
                    returnId = definition.Id;
                }
                else
                {
                    returnId = formDefinitionService.SaveFormDefinition(definition);
                }

                if (Guid.Empty != returnId)
                {
                    actionResult = Excel2FormItem(ds, returnId);
                    if (actionResult.Success)
                    {
                        CreateTemplateContent(returnId);
                    }

                    mapper.CommitTransaction();

                    return actionResult;
                }

                mapper.RollBackTransaction();
                actionResult.Success = false;
                actionResult.Message = "创建表单时出错, 请重试!";
            }
            catch (Exception e)
            {
                mapper.RollBackTransaction();
                actionResult.Success = false;
                actionResult.Message = e.Message;
            }
            return actionResult;
        }

        /// <summary>
        /// 将Excel模板转换为动态表单项.
        /// </summary>
        private ImportExport.ActionResult Excel2FormItem(DataSet ds, Guid formDefinitionId)
        {
            ImportExport.ActionResult actionResult = new ImportExport.ActionResult();
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                FormItemDefinition item = new FormItemDefinition();

                item.Name = dr["标题"].ToString();
                if (item.Name.Length == 0)
                {
                    continue;
                }
                int rowNum = dt.Rows.IndexOf(dr) + 1;
                item.Id = Guid.NewGuid();
                item.FormDefinitionId = formDefinitionId;
                item.FName = "F" + rowNum.ToString();                
                item.Comment = dr["备注"].ToString();
                string itemType = dr["题目类型"].ToString();
                if (!string.IsNullOrEmpty(itemType) && FormDefinitionCommon.TemplateItemTypeKeyDictionary.ContainsKey(itemType))
                    item.ItemType = FormDefinitionCommon.TemplateItemTypeKeyDictionary[itemType];

                if (item.ItemType == FormItemDefinition.FormItemType.TextArea)
                    item.ItemDataType = FormItemDefinition.DataType.Text;
                else if (item.ItemType == FormItemDefinition.FormItemType.File)
                    item.ItemDataType = FormItemDefinition.DataType.File;
                else item.ItemDataType = FormItemDefinition.DataType.String;

                string dataSource = dr["选项"].ToString();
                item.DataSource = (item.ItemType == FormItemDefinition.FormItemType.RadioButtonList ? dataSource.Replace("$$", ",") : dataSource);
                item.Top = rowNum;
                item.Require = (dr["必填项"].ToString() == "是" ? true : false);
                if (item.Require)
                {
                    if (item.ItemType == FormItemDefinition.FormItemType.RadioButtonList ||
                        item.ItemType == FormItemDefinition.FormItemType.CheckBoxList)
                    {
                        item.ValidateType = "group";
                        item.ErrorMessage = string.Format("请选择{0}!", item.Name);
                    }
                    else
                    {
                        item.ValidateType = "require";
                        item.ErrorMessage = string.Format("请输入{0}!", item.Name);
                    }
                }

                string validateType = dr["常用校验"].ToString();
                if (!string.IsNullOrEmpty(validateType) && !validateType.Equals("普通文字型"))
                {
                    if (FormDefinitionCommon.TemplateValidateTypeKeyDictionary.ContainsKey(validateType))
                        item.OpTarget = FormDefinitionCommon.TemplateValidateTypeKeyDictionary[validateType];
                    item.ValidateType = "custom";
                    item.Op = "match";
                    item.ErrorMessage = string.Format("{0}不符合要求,请重新输入!", item.Name);
                }

                if (!formDefinitionService.IsItemExists(formDefinitionId, item.FName))
                {
                    formDefinitionService.AppendItemToForm(item);
                }
            }

            actionResult.Success = true;
            actionResult.Message = "导入成功!";
            return actionResult;
        }

        /// <summary>
        /// 生成表单模板.
        /// </summary>
        private bool CreateTemplateContent(Guid FormDefinitionId)
        {
            FormDefinition definition = formDefinitionService.GetFormDefinitionById(FormDefinitionId, true);
            if (null == definition)
            {
                return false;
            }

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            try
            {
                renderStrategy.Render(htw, definition);
                string templateContent = sw.GetStringBuilder().ToString();
                formDefinitionService.UpdateFormDefinitionTemplate(definition.Id, templateContent);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return true;
        }

        #endregion

        #region
        /// <summary>
        /// 判断各题型对应的内容是否正确.
        /// </summary>
        private bool CheckTemplateContent(DataTable dt, ref string errorMessage)
        {
            foreach (DataRow dr in dt.Rows)
            {
                int rowNum = dt.Rows.IndexOf(dr) + 2;
                
                CheckTitle(dr, rowNum, ref errorMessage);

                string topicType = dr["题目类型"].ToString().Trim();
                switch (topicType)
                {
                    case "单选题":
                    case "单选题(选其它必另填)":
                    case "多选题":
                    case "多选题(选其它必另填)":
                    case "选择题":
                        CheckOptions(dr, rowNum, ref errorMessage);
                        break;
                    case "选择排序题":
                    case "多选题(至少[2]个答案)":
                    case "单选题(选[1]必另填)":
                    case "多选题(选[2]必另填)":
                    case "选择题(同时选[1|2]必另填)":
                    case "选择题(选其中[2|3]之一必另填)":
                    case "范围选择题(至少[1]至多[3]个答案)":
                    case "多项单行填空":
                    case "多项多行填空":
                        errorMessage += string.Format("第 {0} 行: 系统暂不支持 {1} 题型, 请选择其它题型 </br>", rowNum, topicType);
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
                return false;

            return true;
        }

        /// <summary>
        /// 判断标题.
        /// </summary>
        private void CheckTitle(DataRow dr, int rowNum, ref string errorMessage)
        {
            object[] objTitle = { dr["标题"] };
            if (FormDefinitionCommon.CheckTitleContentIsNoEmpty(objTitle, 1))
            {
                errorMessage += string.Format("第 {0} 行: 你暂未填写标题,请填写后再上传!", rowNum);
            }
        }
       
        /// <summary>
        /// 判断选项内容.
        /// </summary>
        private void CheckOptions(DataRow dr, int rowNum, ref string errorMessage)
        {
            object[] objTitle = { dr["选项"] };
            if (FormDefinitionCommon.CheckTitleContentIsNoEmpty(objTitle, 1))
            {
                errorMessage += string.Format("第 {0} 行: 你暂未填写 {1} 的选项内容,请填写后再上传!", rowNum, dr["标题"].ToString());
            }

            string option = dr["选项"].ToString().Trim();
            string[] options = StringUtils.Split(option, "$$");
            if (options.Length <= 1)
            {
                errorMessage += string.Format("第 {0} 行: {1} 的选项内容格式不正确,请以 $$ 作为分隔符分隔各选项内容!", rowNum, dr["标题"].ToString());
            }
        }
        #endregion


        public void UpdateFormDefinition(FormDefinition item)
        {

        }

        /// <summary>
        /// 获取最新的一个表单版本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetFormVersion(string name)
        {
            string sql = string.Format("select isnull(max(Version),0) as Version from bwdf_FormDefinitions where [Name]='{0}'", name);
            object value = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
            return DbUtils.ToInt32(value);
        }
    }
}
