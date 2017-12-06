using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.Commons;
using System.Web;

namespace Botwave.XQP.ImportExport
{
    /// <summary>
    /// 表单定义导出.
    /// </summary>
    public class FormDefinitionExporter : IFormDefinitionExporter
    {
        #region propertiecs

        private IFormDefinitionService formDefinitionService;
        public IFormDefinitionService FormDefinitionService
        {
            set { this.formDefinitionService = value; }
        }

        #endregion

        #region XlsFormDefinitionExporter 成员

        /// <summary>
        /// 导出表单定义.
        /// </summary>
        /// <param name="formDefinitionId">表单实例.</param>
        /// <param name="filename">模板地址.</param>
        /// <returns>生成文件地址.</returns>
        public string Export(Guid formDefinitionId, string filename)
        {
            FormDefinition formDefinition = new FormDefinition();
            formDefinition = formDefinitionService.GetFormDefinitionById(formDefinitionId);
            if (null == formDefinition)
            {
                throw new Exception("找不到表单定义.");
            }

            IList<FormItemDefinition> formItemDefinition = new List<FormItemDefinition>();
            formItemDefinition = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(formDefinitionId);
            DataTable dt = Form2Table(formItemDefinition);

            if (string.IsNullOrEmpty(filename))
            {
                throw new Exception("参数filename无效.");
            }
            else if (!System.IO.File.Exists(filename))
            {
                throw new Exception("模板不存在.");
            }
            string templateExcelFileName = filename;
            string sheetName = "Sheet1";
            string[] srcFieldNames = {"Name", "Require", "ItemType", "ValidateType", "Comment", "DataSource"};
            string[] destFieldNames = {"标题", "必填项", "题目类型", "常用校验", "备注", "选项"};
            string destExcelFileName = string.Empty;
            destExcelFileName = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/Temp/{0}.xls", formDefinition.Name, DateTime.Now));
            try
            {
                System.IO.File.SetAttributes(filename, System.IO.FileAttributes.Normal);
                Export2Excel(dt, templateExcelFileName, sheetName, srcFieldNames, destFieldNames, destExcelFileName);
                System.IO.File.SetAttributes(destExcelFileName, System.IO.FileAttributes.Normal);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return destExcelFileName;
        }        

        /// <summary>
        /// 动态表单转换为Table.
        /// </summary>
        /// <param name="formItemDefinition"></param>
        /// <returns></returns>
        private DataTable Form2Table(IList<FormItemDefinition> formItemDefinition)
        {
            DataTable dt = new DataTable("templateTable");
            dt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));//标题.
            dt.Columns.Add(new DataColumn("Require", Type.GetType("System.String")));//必填项.
            dt.Columns.Add(new DataColumn("ItemType", Type.GetType("System.String")));//题目类型.
            dt.Columns.Add(new DataColumn("ValidateType", Type.GetType("System.String")));//常用校验.
            dt.Columns.Add(new DataColumn("Comment", Type.GetType("System.String")));//备注.
            dt.Columns.Add(new DataColumn("DataSource", Type.GetType("System.String")));//选项.
            
            DataRow newRow;
            foreach (FormItemDefinition item in formItemDefinition)
            {
                newRow = dt.NewRow();
                newRow["Name"] = item.Name;
                newRow["Require"] = (item.Require ? "是" : "否").ToString();
                newRow["ItemType"] = GetItemType(item.ItemType);
                newRow["ValidateType"] = GetValidateType(item.OpTarget);
                newRow["Comment"] = item.Comment;
                newRow["DataSource"] = TransformDataSource(item.DataSource);

                dt.Rows.Add(newRow);
            }
            if (dt.Rows.Count > 0)
                return dt;
            else
                throw new Exception("表单没有表单项");
        }

        /// <summary>
        /// 获取类型字符串.
        /// </summary>
        /// <param name="formItemType"></param>
        /// <returns></returns>
        private string GetItemType(FormItemDefinition.FormItemType formItemType)
        {
            string result = string.Empty;
            try
            {
                result = FormDefinitionCommon.FormItemTypeKeyDictionary[formItemType];
            }
            catch
            {
                result = formItemType.ToString();
            }
            return result;
        }

        /// <summary>
        /// 获取验证字符串.
        /// </summary>
        /// <param name="formItemType"></param>
        /// <returns></returns>
        private string GetValidateType(string validateType)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(validateType))
            {
                try
                {
                    result = FormDefinitionCommon.FormValidateTypeKeyDictionary[validateType];
                }
                catch
                {
                    result = validateType.ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// 数据源分隔符替换.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        private string TransformDataSource(string dataSource)
        {
            if (dataSource.IndexOf("$$") != -1)
            {
                return dataSource;
            }
            return dataSource.Replace(",","$$");
        }

        #endregion

        public static void Export2Excel(DataTable dtSrc, string templateExcelFileName, string sheetName, string[] srcFieldNames, string[] destFieldNames, string destExcelFileName)
        {
            if (null == srcFieldNames
                || null == destFieldNames
                || srcFieldNames.Length != destFieldNames.Length
                || srcFieldNames.Length <= 0)
            {
                throw new ArgumentException("srcFieldNames/destFieldNames (null/empty/length) is invalid");
            }

            if (!System.IO.File.Exists(templateExcelFileName))
            {
                throw new ArgumentException(String.Format("文件{0}不存在", templateExcelFileName), "templateExcelFileName");
            }

            System.IO.File.Copy(templateExcelFileName, destExcelFileName, true);

            string strConn = String.Format(ExcelUtils.EXCEL_WRITE_CONN_STR, destExcelFileName);
            string strSelect = String.Format(ExcelUtils.EXCEL_SELECT_STR, sheetName);
            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbDataAdapter da = new OleDbDataAdapter(strSelect, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, sheetName);

            int fieldLength = srcFieldNames.Length;
            foreach (DataRow drSrc in dtSrc.Rows)
            {
                DataRow drNew = ds.Tables[sheetName].NewRow();
                for (int i = 0; i < fieldLength; i++)
                {
                    string value = DbUtils.ToString(drSrc[srcFieldNames[i]]);
                    drNew[destFieldNames[i]] = value != null && value.Length > 255 ? value.Substring(0, 255) : value;
                }
                ds.Tables[sheetName].Rows.Add(drNew);
            }

            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            cb.QuotePrefix = "[";
            cb.QuoteSuffix = "]";
            da.Update(ds, sheetName);
        }
	
    }
}
