using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using Aspose.Cells;
using System.Collections;

namespace Botwave.XQP.Commons
{
    public static class ExcelHelper
    {
        /// <summary> 
        /// 将DataTable中的数据导出到指定的Excel文件中 
        /// </summary> 
        /// <param name="page">Web页面对象</param> 
        /// <param name="tab">包含被导出数据的DataTable对象</param> 
        /// <param name="FileName">Excel文件的名称</param> 
        public static void Export(System.Web.UI.Page page, System.Data.DataTable tb, string FileName)
        {
            FileName = System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
            Workbook workbook = new Workbook(); //工作簿 
            Worksheet sheet = workbook.Worksheets[0]; //工作表 
            sheet.Name = tb.TableName;
            sheet.PageSetup.Orientation = Aspose.Cells.PageOrientationType.Landscape;
            sheet.PageSetup.LeftMargin = 0.5;

            Cells cells = sheet.Cells;//单元格 

            //为表头设置样式     
            Aspose.Cells.Style styleHeader = workbook.Styles[workbook.Styles.Add()];//新增样式 
            styleHeader.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            styleHeader.Font.Name = "宋体";//文字字体 
            styleHeader.Font.Size = 10;//文字大小 
            styleHeader.Font.IsBold = true;//粗体 
            styleHeader.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleHeader.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleHeader.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleHeader.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;


            //为表头设置样式     
            Aspose.Cells.Style styleHeader1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            styleHeader1.HorizontalAlignment = TextAlignmentType.Left;//文字居中 
            styleHeader1.Font.Name = "宋体";//文字字体 
            styleHeader1.Font.Size = 10;//文字大小 
            styleHeader1.Font.IsBold = false;//粗体 
            styleHeader1.IsTextWrapped = true;//单元格内容自动换行 
            styleHeader1.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleHeader1.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleHeader1.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleHeader1.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            //样式2 
            Aspose.Cells.Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            style2.Font.Name = "宋体";//文字字体 
            style2.Font.Size = 10;//文字大小 
            style2.Font.IsBold = false;//粗体 
            style2.IsTextWrapped = true;//单元格内容自动换行 
            style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            int Rownum = tb.Rows.Count;
            int iii = 0;
            for (int i = 0; tb != null && i < tb.Columns.Count; i++)
            {
                    cells[0, iii].PutValue(tb.Columns[i].ColumnName);//表头 
                cells[0, iii].SetStyle(styleHeader);
                cells.SetRowHeight(0, 25);
                cells.SetColumnWidth(iii, 15);
                iii++;
            }

            for (int j = 0; tb != null && j < tb.Rows.Count; j++)
            {
                int i = j + 1;

                int iiii = 0;
                for (int ii = 0; ii < tb.Columns.Count; ii++)//内容
                {
                    cells[i, iiii].PutValue(tb.Rows[j][tb.Columns[ii].ColumnName].ToString());
                    cells[i, iiii].SetStyle(style2);
                    cells.SetRowHeight(0, 13);
                    iiii++;
                }
            }
            string createtime = DateTime.Now.ToString("yyyy-MM-dd");
            string doc_name = FileName;
            doc_name += createtime + ".xls";
            workbook.Save(doc_name, Aspose.Cells.SaveType.OpenInExcel, Aspose.Cells.FileFormatType.Excel2003, page.Response);
        }
        private static bool DownFile(System.Web.HttpResponse Response, string fileName, string fullPath)
        {
            try
            {
                Response.ContentType = "application/octet-stream";

                Response.AppendHeader("Content-Disposition", "attachment;filename=" +
                HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ";charset=GB2312");
                System.IO.FileStream fs = System.IO.File.OpenRead(fullPath);
                long fLen = fs.Length;
                int size = 102400;//每100K同时下载数据 
                byte[] readData = new byte[size];//指定缓冲区的大小 
                if (size > fLen) size = Convert.ToInt32(fLen);
                long fPos = 0;
                bool isEnd = false;
                while (!isEnd)
                {
                    if ((fPos + size) > fLen)
                    {
                        size = Convert.ToInt32(fLen - fPos);
                        readData = new byte[size];
                        isEnd = true;
                    }
                    fs.Read(readData, 0, size);//读入一个压缩块 
                    Response.BinaryWrite(readData);
                    fPos += size;
                }
                fs.Close();
                System.IO.File.Delete(fullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary> 
        /// 将指定Excel文件中的数据转换成DataSet集合对象，供应用程序进一步处理 
        /// </summary> 
        /// <param name="filePath"></param> 
        /// <returns></returns> 
        public static System.Data.DataSet ImportToDataSet(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            string connectionStr = string.Empty;
            
            bool canOpen = false;
            ArrayList arr = new ArrayList();
            DataSet ds = new DataSet();
            if (extension.Equals(".xls"))
            {
                arr = ExcelSheetName(filePath);
                connectionStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended properties='Excel 8.0;HDR=Yes;IMEX=1;'", filePath);
            }
            else if (extension.Equals("xlsx"))
            {
                arr = ExcelSheetName_2007(filePath);
                connectionStr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", filePath);
            }
            
            OleDbConnection conn = new OleDbConnection(connectionStr);

            try//尝试数据连接是否可用 
            {
                conn.Open();
                //conn.Close();
                canOpen = true;
            }
            catch { }

            if (canOpen)
            {
                try//如果数据连接可以打开则尝试读入数据 
                {
                    foreach (string sheetName in arr)
                    {
                        System.Data.DataTable rs = new System.Data.DataTable();
                        OleDbCommand myOleDbCommand = new OleDbCommand("SELECT * FROM [" + sheetName + "]", conn);
                        OleDbDataAdapter myData = new OleDbDataAdapter(myOleDbCommand);
                        myData.Fill(rs);
                        rs.TableName = sheetName.Substring(0,sheetName.Length-1);
                        ds.Tables.Add(rs);
                    }
                    conn.Close();
                }
                catch(Exception ex)//如果数据连接可以打开但是读入数据失败，则从文件中提取出工作表的名称，再读入数据 
                {
                    if (conn.State.Equals(ConnectionState.Open))
                        conn.Close();
                    throw ex;
                }
            }
            return ds;
        }

        /// <summary>
        /// 用aspose组件读取excel
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static DataTable ReadExcel(String strFileName)
        {
            Workbook book = new Workbook();
            book.Open(strFileName);
            Worksheet sheet = book.Worksheets[0];
            Cells cells = sheet.Cells;

            return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
        } 
        /// <summary> 
        /// 将指定Html字符串的数据转换成DataTable对象 --根据“<tr><td>”等特殊字符进行处理 
        /// </summary> 
        /// <param name="tmpHtml">Html字符串</param> 
        /// <returns></returns> 
        private static DataTable GetDataTableFromString(string tmpHtml)
        {
            string tmpStr = tmpHtml;
            DataTable TB = new DataTable();
            //先处理一下这个字符串，删除第一个<tr>之前合最后一个</tr>之后的部分 
            int index = tmpStr.IndexOf("<tr");
            if (index > -1)
                tmpStr = tmpStr.Substring(index);
            else
                return TB;

            index = tmpStr.LastIndexOf("</tr>");
            if (index > -1)
                tmpStr = tmpStr.Substring(0, index + 5);
            else
                return TB;

            bool existsSparator = false;
            char Separator = Convert.ToChar("^");

            //如果原字符串中包含分隔符“^”则先把它替换掉 
            if (tmpStr.IndexOf(Separator.ToString()) > -1)
            {
                existsSparator = true;
                tmpStr = tmpStr.Replace("^", "^$&^");
            }

            //先根据“</tr>”分拆 
            string[] tmpRow = tmpStr.Replace("</tr>", "^").Split(Separator);

            for (int i = 0; i < tmpRow.Length - 1; i++)
            {
                DataRow newRow = TB.NewRow();

                string tmpStrI = tmpRow[i];
                if (tmpStrI.IndexOf("<tr") > -1)
                {
                    tmpStrI = tmpStrI.Substring(tmpStrI.IndexOf("<tr"));
                    if (tmpStrI.IndexOf("display:none") < 0 || tmpStrI.IndexOf("display:none") > tmpStrI.IndexOf(">"))
                    {
                        tmpStrI = tmpStrI.Replace("</td>", "^");
                        string[] tmpField = tmpStrI.Split(Separator);

                        for (int j = 0; j < tmpField.Length - 1; j++)
                        {
                            tmpField[j] = RemoveString(tmpField[j], "<font>");
                            index = tmpField[j].LastIndexOf(">") + 1;
                            if (index > 0)
                            {
                                string field = tmpField[j].Substring(index, tmpField[j].Length - index);
                                if (existsSparator) field = field.Replace("^$&^", "^");
                                if (i == 0)
                                {
                                    string tmpFieldName = field;
                                    int sn = 1;
                                    while (TB.Columns.Contains(tmpFieldName))
                                    {
                                        tmpFieldName = field + sn.ToString();
                                        sn += 1;
                                    }
                                    TB.Columns.Add(tmpFieldName);
                                }
                                else
                                {
                                    newRow[j] = field;
                                }
                            }//end of if(index>0) 
                        }

                        if (i > 0)
                            TB.Rows.Add(newRow);
                    }
                }
            }

            TB.AcceptChanges();
            return TB;
        }

        /// <summary> 
        /// 从指定Html字符串中剔除指定的对象 
        /// </summary> 
        /// <param name="tmpHtml">Html字符串</param> 
        /// <param name="remove">需要剔除的对象--例如输入"<font>"则剔除"<font ???????>"和"</font>>"</param> 
        /// <returns></returns> 
        public static string RemoveString(string tmpHtml, string remove)
        {
            tmpHtml = tmpHtml.Replace(remove.Replace("<", "</"), "");
            tmpHtml = RemoveStringHead(tmpHtml, remove);
            return tmpHtml;
        }
        /// <summary> 
        /// 只供方法RemoveString()使用 
        /// </summary> 
        /// <returns></returns> 
        private static string RemoveStringHead(string tmpHtml, string remove)
        {
            //为了方便注释，假设输入参数remove="<font>" 
            if (remove.Length < 1) return tmpHtml;//参数remove为空：不处理返回 
            if ((remove.Substring(0, 1) != "<") || (remove.Substring(remove.Length - 1) != ">")) return tmpHtml;//参数remove不是<?????>：不处理返回 

            int IndexS = tmpHtml.IndexOf(remove.Replace(">", ""));//查找“<font”的位置 
            int IndexE = -1;
            if (IndexS > -1)
            {
                string tmpRight = tmpHtml.Substring(IndexS, tmpHtml.Length - IndexS);
                IndexE = tmpRight.IndexOf(">");
                if (IndexE > -1)
                    tmpHtml = tmpHtml.Substring(0, IndexS) + tmpHtml.Substring(IndexS + IndexE + 1);
                if (tmpHtml.IndexOf(remove.Replace(">", "")) > -1)
                    tmpHtml = RemoveStringHead(tmpHtml, remove);
            }
            return tmpHtml;
        }

        /// <summary> 
        /// 将指定Excel文件中读取第一张工作表的名称 
        /// </summary> 
        /// <param name="filePath"></param> 
        /// <returns></returns> 
        public static string GetSheetName(string filePath)
        {
            string sheetName = "";

            System.IO.FileStream tmpStream = File.OpenRead(filePath);
            byte[] fileByte = new byte[tmpStream.Length];
            tmpStream.Read(fileByte, 0, fileByte.Length);
            tmpStream.Close();

            byte[] tmpByte = new byte[]{Convert.ToByte(11),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0), 
 Convert.ToByte(11),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0), 
 Convert.ToByte(30),Convert.ToByte(16),Convert.ToByte(0),Convert.ToByte(0)};

            int index = GetSheetIndex(fileByte, tmpByte);
            if (index > -1)
            {

                index += 16 + 12;
                System.Collections.ArrayList sheetNameList = new System.Collections.ArrayList();

                for (int i = index; i < fileByte.Length - 1; i++)
                {
                    byte temp = fileByte[i];
                    if (temp != Convert.ToByte(0))
                        sheetNameList.Add(temp);
                    else
                        break;
                }
                byte[] sheetNameByte = new byte[sheetNameList.Count];
                for (int i = 0; i < sheetNameList.Count; i++)
                    sheetNameByte[i] = Convert.ToByte(sheetNameList[i]);

                sheetName = System.Text.Encoding.Default.GetString(sheetNameByte);
            }
            return sheetName;
        }
        /// <summary> 
        /// 只供方法GetSheetName()使用 
        /// </summary> 
        /// <returns></returns> 
        private static int GetSheetIndex(byte[] FindTarget, byte[] FindItem)
        {
            int index = -1;

            int FindItemLength = FindItem.Length;
            if (FindItemLength < 1) return -1;
            int FindTargetLength = FindTarget.Length;
            if ((FindTargetLength - 1) < FindItemLength) return -1;

            for (int i = FindTargetLength - FindItemLength - 1; i > -1; i--)
            {
                System.Collections.ArrayList tmpList = new System.Collections.ArrayList();
                int find = 0;
                for (int j = 0; j < FindItemLength; j++)
                {
                    if (FindTarget[i + j] == FindItem[j]) find += 1;
                }
                if (find == FindItemLength)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        #region ExcelSheetName
        /// <summary>
        /// 获得Excel中的所有sheetname。 filepath 为绝对路径
        /// </summary>
        /// <param name="filepath">绝对路径</param>
        /// <returns>ArrayList</returns>
        public static ArrayList ExcelSheetName(string filepath)
        {
            ArrayList al = new ArrayList();
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable sheetNames = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            conn.Close();
            foreach (DataRow dr in sheetNames.Rows)
            {
                al.Add(dr[2]);
            }
            return al;
        }

        public static ArrayList ExcelSheetName_2007(string filepath)
        {
            ArrayList al = new ArrayList();
            string strConn;
            strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=Excel 12.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable sheetNames = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            conn.Close();
            foreach (DataRow dr in sheetNames.Rows)
            {
                if (dr[2].ToString().Trim() != "_xlnm#_FilterDatabase")
                    al.Add(dr[2]);
            }
            return al;
        }
        #endregion
    }
}
