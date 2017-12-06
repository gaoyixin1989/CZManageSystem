using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace CZManageSystem.Core.Helpers
{
    /// <summary>
    /// Class DataTableToXls
    /// </summary>
    public class DataTableToXls
    {
        /// <summary>
        /// XLSs to data table.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>DataTable.</returns>
        public static DataTable XlsToDataTable(string filePath)
        {
            if (!File.Exists(filePath))
                return null;
            try
            {
                string strConn;
                var extension = Path.GetExtension(filePath);
                if (extension != null && "xlsx".Equals(extension.Substring(1), StringComparison.OrdinalIgnoreCase))
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath +
                              ";Extended Properties='Excel 12.0;HDR=No;IMEX=1;'";
                else
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath +
                              ";Extended Properties='Excel 8.0;HDR=No;IMEX=1;'";

                //链接Excel
                var cnnxls = new OleDbConnection(strConn);
                cnnxls.Open();
                var schemaTable = cnnxls.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schemaTable != null)
                {
                    string tableName = schemaTable.Rows[0][2].ToString().Trim();

                    //读取Excel里面有 表tableName
                    var oda = new OleDbDataAdapter("select * from [" + tableName + "]", cnnxls);
                    var ds = new DataSet();
                    //将Excel里面有表内容装载到内存表中！
                    oda.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    dt.TableName = tableName;
                    return dt;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将内存中的DataTable转成Excel
        /// </summary>
        /// <param name="excelSavePath">Excel保存路径</param>
        /// <param name="sourceTable">内存中的DataTable</param>
        /// <param name="sheetName">在Excel中保存的Sheet名称</param>
        public static void TableToExcel(string excelSavePath, DataTable sourceTable, string sheetName)
        {
            string strConn;
            var extension = Path.GetExtension(excelSavePath);
            if (extension != null && "xlsx".Equals(extension.Substring(1), StringComparison.OrdinalIgnoreCase))
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + excelSavePath + ";" +
                          "Extended Properties=Excel 12.0;";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + excelSavePath + ";" +
                          "Extended Properties=Excel 8.0;";
            using (var conn = new OleDbConnection(strConn))
            {
                conn.Open();
                //检查表单是否已存在
                IEnumerable<string> sheets = GetSheets(conn);
                if (sheets.Any(t => t.Equals(sheetName, StringComparison.OrdinalIgnoreCase)))
                {
                    using (OleDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = string.Format("DROP TABLE {0} ", sheetName);
                        cmd.ExecuteNonQuery();
                    }
                }
                //创建表单
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("CREATE TABLE {0} ({1})", sheetName, BuildColumnsString(sourceTable));
                    cmd.ExecuteNonQuery();
                }
                using (var da = new OleDbDataAdapter(string.Format("SELECT * FROM {0}", sheetName), conn))
                {
                    var myCommandBuilder = new OleDbCommandBuilder(da);
                    da.InsertCommand = myCommandBuilder.GetInsertCommand();
                    da.InsertCommand.Connection = da.SelectCommand.Connection;
                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    //OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                    var dataTable = new DataTable(sheetName);
                    //读取表单（空表）
                    da.Fill(dataTable);
                    //为空表写数据
                    foreach (DataRow sRow in sourceTable.Rows)
                    {
                        DataRow nRow = dataTable.NewRow();
                        for (int i = 0; i < sourceTable.Columns.Count; i++)
                        {
                            if (sRow[i] is Byte[])
                                nRow[i] = "二进制数据";
                            else
                                nRow[i] = sRow[i];
                        }
                        dataTable.Rows.Add(nRow);
                    }
                    //更新表单
                    da.Update(dataTable);
                }
            }
        }

        /// <summary>
        /// 从<paramref name="sourceTable"/>构建字段字符串
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <returns></returns>
        private static string BuildColumnsString(DataTable sourceTable)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < sourceTable.Columns.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                string colName = sourceTable.Columns[i].ColumnName;
                sb.Append(colName);
                sb.Append(" ");
                //sb.Append("_ ");//为了避免系统关键字，将所有字段名后添加下划线
                sb.Append(SwitchToSqlType(sourceTable.Columns[i]));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将<paramref name="column"/>的DataType转成数据库关键字
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static string SwitchToSqlType(DataColumn column)
        {
            string typeFullName = column.DataType.FullName;
            switch (typeFullName)
            {
                case "System.Int32":
                    return "INTEGER";
                case "System.Int16":
                    return "SMALLINT";
                case "System.String":
                    return "TEXT";
                    //return string.Format("VARCHAR ({0})", column.MaxLength < 0 ? 250 : column.MaxLength);
                case "System.Int64":
                    return "BIGINT";
                case "System.Double":
                case "System.Float":
                case "System.Single":
                    return "REAL";
                case "System.Numeric":
                    return "NUMERIC";
                case "System.DateTime":
                    return "DATETIME";
                case "System.Decimal":
                    return "Decimal";
                default:
                    return "VARCHAR(50)";
            }
        }

        /// <summary>
        /// 获取EXCEL的所有表单
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetSheets(OleDbConnection conn)
        {
            //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等
            DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                new object[] {null, null, null, "Table"});
            //包含excel中表名的字符串数组
            if (dtSheetName != null)
            {
                var strTableNames = new string[dtSheetName.Rows.Count];
                for (int k = 0; k < dtSheetName.Rows.Count; k++)
                {
                    //DataRow row = dtSheetName.Rows[k];
                    strTableNames[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
                }
                return strTableNames;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="title"></param>
        /// <param name="objList"></param>
        /// <param name="excelPropertyNamesENG"></param>
        /// <param name="excelPropertyNamesCHI"></param>
        /// <returns></returns>
        public static System.IO.MemoryStream ExportExcel<T>(string title, List<T> objList, string[] excelPropertyNamesENG, string[] excelPropertyNamesCHI)
        {
            NPOI.SS.UserModel.IWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = workbook.CreateSheet("Sheet1");
            NPOI.SS.UserModel.IRow row;
            NPOI.SS.UserModel.ICell cell;
            NPOI.SS.UserModel.ICellStyle cellStyle;

            Dictionary<string, string> dicColumn = new Dictionary<string, string>();
            for (int i = 0; i < excelPropertyNamesENG.Length; i++)
            {
                dicColumn.Add(excelPropertyNamesENG[i], excelPropertyNamesCHI[i]);
            }
            cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            int rowNum = 0;
            #region 写入表头到文件中
            if (!string.IsNullOrEmpty(title))
            {
                cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                NPOI.SS.UserModel.IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 15;
                cellStyle.SetFont(font);
                row = sheet.CreateRow(rowNum);
                cell = row.CreateCell(0, NPOI.SS.UserModel.CellType.String);
                cell.SetCellValue(title);
                cell.CellStyle = cellStyle;
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, excelPropertyNamesENG.Length > 0 ? excelPropertyNamesENG.Length - 1 : 0));
                rowNum++;
            }
            #endregion
            if (objList.Count > 0)
            {
                Type type = objList[0].GetType();
                if (type != null)
                {
                    System.Reflection.PropertyInfo[] properties = type.GetProperties();
                    if (properties.Length > 0)
                    {
                        #region 写入列名到文件中
                        if (excelPropertyNamesENG.Length > 0)
                        {
                            row = sheet.CreateRow(rowNum);
                            int count = 0;
                            for (int m = 0; m < properties.Length; m++)
                            {
                                if (excelPropertyNamesENG.Contains(properties[m].Name))
                                {
                                    cell = row.CreateCell(count, NPOI.SS.UserModel.CellType.String);
                                    string displayName = GetDisplayNameByPropertyName(properties[m].Name, dicColumn);
                                    cell.SetCellValue(displayName == null ? "" : displayName);
                                    cell.CellStyle = cellStyle;
                                    count++;
                                }
                            }
                            rowNum++;
                        }
                        #endregion
                        #region 写入数据到文件中
                        if (excelPropertyNamesENG.Length > 0)
                        {
                            for (int i = 0; i < objList.Count; i++)
                            {
                                row = sheet.CreateRow(i + rowNum);
                                int count = 0;
                                for (int j = 0; j < properties.Length; j++)
                                {
                                    if (excelPropertyNamesENG.Contains(properties[j].Name))
                                    {
                                        cell = row.CreateCell(count);
                                        object obj = properties[j].GetValue(objList[i]);
                                        cell.SetCellValue(obj == null ? "" : obj.ToString());
                                        cell.CellStyle = cellStyle;
                                        count++;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            return ms;
        }

        public static string GetDisplayNameByPropertyName(string propertyName, Dictionary<string, string> dicColumn)
        {
            string result = null;
            foreach (KeyValuePair<string, string> dic in dicColumn)
            {
                if (dic.Key == propertyName)
                {
                    result = dic.Value;
                }
                continue;
            }
            return result;
        }
    }
}