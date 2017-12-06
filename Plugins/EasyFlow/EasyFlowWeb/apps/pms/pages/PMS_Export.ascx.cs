using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Aspose.Cells;
using System.IO;
using System.Threading;
using Botwave.XQP.Util;
using Botwave.Extension.IBatisNet;





public partial class apps_pms_pages_PMS_Export : System.Web.UI.UserControl
{
    private string[] a_zArrary = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ" };
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger("apps_pms_pages_PMS_Export");
    #region 属性
    /// <summary>
    /// 导出当前页的数据
    /// </summary>
    public DataTable ExplortData
    {
        get
        {
            DataTable dt = null;
            if (ViewState["ExplortData"] != null)
            {
                dt = ViewState["ExplortData"] as DataTable;
            }
            return dt;
        }
        set
        {
            ViewState["ExplortData"] = value;
        }
    }
    /// <summary>
    /// 导出模板路径
    /// </summary>
    public string ExplortPath
    {
        get
        {
            string dt = null;
            if (ViewState["ExplortPath"] != null)
            {
                dt = (string)ViewState["ExplortPath"];
            }
            return dt;
        }
        set
        {
            ViewState["ExplortPath"] = value;
        }
    }

    public string ByGroup
    {
        get
        {
            string byGropu = string.Empty;
            if (ViewState["ByGroup"] != null)
            {
                byGropu = (string)ViewState["ByGroup"];
            }
            return byGropu;
        }
        set
        {
            ViewState["ByGroup"] = value;
        }
    }

    public string ByOrder
    {
        get
        {
            string byOrder = string.Empty;
            if (ViewState["ByOrder"] != null)
            {
                byOrder = (string)ViewState["ByOrder"];
            }
            return byOrder;
        }
        set
        {
            ViewState["ByOrder"] = value;
        }
    }
    public DataTable AllTable
    {
        get
        {
            DataTable alltable = null;
            if (ViewState["AllTable"] != null)
            {
                alltable = (DataTable)ViewState["AllTable"];
            }
            return alltable;
        }
        set
        {
            ViewState["AllTable"] = value;
        }
    }

    public string ExecSql
    {
        get
        {
            string execSql = null;
            if (ViewState["execSql"] != null)
            {
                execSql = (string)ViewState["execSql"];
            }
            return execSql;
        }
        set
        {
            ViewState["execSql"] = value;
        }
    }

    public int GroupType
    {
        get
        {
            int groupType = 0;
            if (ViewState["GroupType"] != null)
            {
                groupType = (int)ViewState["GroupType"];
            }
            return groupType;
        }
        set
        {
            ViewState["GroupType"] = value;
        }
    }

    public delegate void DelegateSeachHander(object serder, EventArgs e);


    //添加一个事件
    public event DelegateSeachHander Seach;//添加事件到句柄    /// <summary>

    /// 导出所需的表
    /// </summary>
    public string TableName
    {
        get
        {
            string tableName = string.Empty;
            if (ViewState["TableName"] != null)
            {
                tableName = ViewState["TableName"].ToString();
            }
            return tableName;
        }
        set
        {
            ViewState["TableName"] = value;
        }
    }

    /// <summary>
    /// 导出所需的条件
    /// </summary>
    public string GetDataWhere
    {
        get
        {
            string getDataWhere = string.Empty;
            if (ViewState["GetDataWhere"] != null)
            {
                getDataWhere = ViewState["GetDataWhere"].ToString();
            }
            return getDataWhere;
        }
        set
        {
            ViewState["GetDataWhere"] = value;
        }
    }

    /// <summary>
    /// 需要导出的英文字段
    /// </summary>
    public string ExplortFilter
    {
        get
        {
            string explortFilter = string.Empty;
            if (ViewState["ExplortFilter"] != null)
            {
                explortFilter = ViewState["ExplortFilter"].ToString();
            }

            return explortFilter;
        }
        set
        {
            ViewState["ExplortFilter"] = value;
        }
    }

    /// <summary>
    /// 需要导出的英文字段扩展
    /// </summary>
    public string ExplortFilterEX
    {
        get
        {
            string explortFilterEX = string.Empty;
            if (ViewState["ExplortFilterEX"] != null)
            {
                explortFilterEX = ViewState["ExplortFilterEX"].ToString();
            }
            return explortFilterEX;
        }
        set
        {
            ViewState["ExplortFilterEX"] = value;
        }
    }

    /// <summary>
    /// 需要导出的中文字段
    /// </summary>
    public string ZHFilter
    {
        get
        {
            string zhFilter = string.Empty;
            if (ViewState["ZHFilter"] != null)
            {
                zhFilter = ViewState["ZHFilter"].ToString();
            }
            return zhFilter;
        }
        set
        {
            ViewState["ZHFilter"] = value;
        }
    }

    /// <summary>
    /// 总记录数
    /// </summary>
    public int TotoalCount
    {
        get
        {
            int totoalCount = 0;
            if (ViewState["TotoalCount"] != null)
            {
                totoalCount = Convert.ToInt32(ViewState["TotoalCount"]);
            }
            return totoalCount;
        }
        set
        {
            ViewState["TotoalCount"] = value;
        }
    }

    /// <summary>
    /// 导出调用的存储过程类型
    /// </summary>
    public int ExportType
    {
        get
        {
            int exportType = 0;
            if (ViewState["ExportType"] != null)
            {
                exportType = Convert.ToInt32(ViewState["ExportType"]);
            }
            return exportType;
        }
        set
        {
            ViewState["ExportType"] = value;
        }
    }

    /// <summary>
    /// 主键
    /// </summary>
    public string KeyFilter
    {
        get
        {
            string keyFilter = string.Empty;
            if (ViewState["KeyFilter"] != null)
            {
                keyFilter = ViewState["KeyFilter"].ToString();
            }
            return keyFilter;
        }
        set
        {
            ViewState["KeyFilter"] = value;
        }
    }

    /// <summary>
    /// 营销编码
    /// </summary>
    public string Code
    {
        get
        {
            string code = string.Empty;
            if (ViewState["Code"] != null)
            {
                code = ViewState["Code"].ToString();
            }
            return code;
        }
        set
        {
            ViewState["Code"] = value;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(Request.Form["getData"]))
        {
            OutAllByText();
        }
        else if (!string.IsNullOrEmpty(Request.Form["loading"]))
        {
            if (th != null && th.ThreadState == ThreadState.Stopped && sbAllText.Length > 0)
            {
                Response.Write(threadFlag);

                Response.End();
            }
            else if (th == null || th.ThreadState != ThreadState.Background)
            {
                btn_All_Export_To_Txt_Click(null, null);
            }
        }


    }



    /// <summary>
    /// 当前页导出到Excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_This_Expor_To_Exc_Click(object sender, EventArgs e)
    {
        if (ExplortData != null)
        {

            if (ExplortData.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(ExplortPath))
                {

                    string[] s = Gets(ExplortFilter);
                    string[] s2 = ZHFilter.Split(',');
                    //ExcelHelp.Export2Excel(DateTime.Now.ToString("yyyyMMddHHssmm"), ExplortData, s, s2);

                    Workbook workbook = new Workbook();

                    Worksheet sheet = (Worksheet)workbook.Worksheets[0];

                    for (int i = 0; i < s2.Length; i++)
                    {
                        if (!s2[i].Contains("thres"))
                        {
                            sheet.Cells[a_zArrary[i] + "1"].PutValue(s2[i]);
                        }

                    }

                    int RowNo = 2;
                    for (int i = 0; i < ExplortData.Rows.Count; i++)
                    {

                        for (int j = 0; j < s.Length; j++)
                        {
                            if (!s[j].Contains("thres"))
                            {
                                sheet.Cells[a_zArrary[j] + RowNo].PutValue(ExplortData.Rows[i][s[j].Trim().Trim('\'')]);
                            }
                        }

                        RowNo++;

                    }
                    //String filename = string.Format("{0}.xls", Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHssmm")); //文件默认命名方式，可以自定义

                    //Response.ContentType = "application/ms-excel;charset=utf-8";

                    //Response.AddHeader("content-disposition", "attachment; filename=" + filename);

                    //System.IO.MemoryStream memStream = workbook.SaveToStream();

                    //Response.BinaryWrite(memStream.ToArray());//$(document.body).hideLoading();

                    //Response.End();
                    //   Response.Write("<script>alert('')</script>");
                    ExportToExcel(Response, ExplortData);
                }
                else
                {
                    WorkbookDesigner designer = new WorkbookDesigner();
                    designer.Open(MapPath(ExplortPath));
                    ExplortData.TableName = "Table";
                    foreach (DataColumn col in ExplortData.Columns)
                    {
                        if (col.ColumnName.StartsWith("GROUP_VEST_LVL_"))
                        {
                            col.ColumnName = "GROUP_VEST_LVL_1";
                        }
                    }
                    //数据源 
                    designer.SetDataSource(ExplortData);

                    designer.Process();

                    designer.Save(System.Web.HttpUtility.UrlEncode(Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHssmm") + ".xls", System.Text.Encoding.UTF8), SaveType.OpenInExcel, FileFormatType.Excel2003, Response);

                }
            }
            else
            {
                CommontUnit.Instance.Message("需要导出的数据不能为空", Response);
                return;
            }
        }
        else
        {
            CommontUnit.Instance.Message("需要导出的数据不能为空", Response);
            return;
        }
    }


    string[] Gets(string field)
    {
        bool flage = field.IndexOf('|') > 0;
        string[] s;
        if (flage)
        {
            s = field.Split('|');
        }
        else { s = field.Split(','); }


        for (int i = 0; i < s.Length; i++)
        {
            int index = s[i].ToLower().LastIndexOf("as ");
            //   int index2 = s[i].ToLower().IndexOf("numeric(");
            if (index > 0 && !s[i].Contains("thres"))
            {
                s[i] = s[i].Substring(index + 3, s[i].Length - index - 3).Trim();
            }
            //else if (index2 > 0)
            //{
            //    s[i] = s[i].Substring(index2 + 24, s[i].Length - index2 - 24).Trim();
            //}
        }


        return s;
    }

    /// <summary>
    /// 所有数据导出到Excle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_All_Export_To_Exc_Click(object sender, EventArgs e)
    {
        if (TotoalCount > 65536)
        {

            CommontUnit.Instance.Message("数据量太大，请选择导出.txt", Response);
            return;
        }
        DataTable AllData =new DataTable();//= string.IsNullOrEmpty(ExecSql) ? GetData(TableName, GetDataWhere, ExplortFilter.Replace("|", ","), ZHFilter) : IBatisDbHelper.ExecuteDataset(CommandType.Text, ExecSql).Tables[0];
        if (string.IsNullOrEmpty(ExecSql))
        {
            AllData = GetData(TableName, GetDataWhere, ExplortFilter.Replace("|", ","), ZHFilter);
        }
        else
        {
            btn_This_Expor_To_Exc_Click(sender, e);
        }
        if (string.IsNullOrEmpty(ExplortPath))
        {

            string[] s = Gets(ExplortFilter);
            string[] s2 = ZHFilter.Split(',');
            //ExcelHelp.Export2Excel(DateTime.Now.ToString("yyyyMMddHHssmm"), AllData, s, s2);
            Workbook workbook = new Workbook();

            Worksheet sheet = (Worksheet)workbook.Worksheets[0];

            for (int i = 0; i < s2.Length; i++)
            {
                if (!s2[i].Contains("thres"))
                {
                    sheet.Cells[a_zArrary[i] + "1"].PutValue(s2[i]);
                }
            }

            int RowNo = 2; int k = 0;
            for (int i = 0; i < AllData.Rows.Count; i++)
            {

                for (int j = 0; j < s.Length; j++)
                {
                    if (!s[j].Contains("thres") && !s[j].Contains("HIDE_"))
                    {
                        sheet.Cells[a_zArrary[j - k] + RowNo].PutValue(AllData.Rows[i][s[j].Trim().Trim('\'')]);
                    }
                    else
                    {
                        k++;
                    }
                   
                }
                k = 0;
                RowNo++;
            }
            //String filename = string.Format("{0}.xls", Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHssmm")); //文件默认命名方式，可以自定义

            //Response.ContentType = "application/ms-excel;charset=utf-8";

            //Response.AddHeader("content-disposition", "attachment; filename=" + filename);

            //System.IO.MemoryStream memStream = workbook.SaveToStream();

            //Response.BinaryWrite(memStream.ToArray());

            //Response.End();
            ExportToExcel(Response,AllData);
        }
        else
        {
            WorkbookDesigner designer = new WorkbookDesigner();

            designer.Open(MapPath(ExplortPath));
            foreach (DataColumn col in AllData.Columns)
            {
                if (col.ColumnName.StartsWith("GROUP_VEST_LVL_"))
                {
                    col.ColumnName = "GROUP_VEST_LVL_1";
                }
            }
            //数据源 
            designer.SetDataSource(AllData);

            designer.Process();

            designer.Save(System.Web.HttpUtility.UrlEncode(Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHssmm") + ".xls", System.Text.Encoding.UTF8), SaveType.OpenInExcel, FileFormatType.Excel2003, Response);
        }


    }

    /// <summary>
    ///  所有数据导出为TXT
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_All_Export_To_Txt_Click(object sender, EventArgs e)
    {
        threadFlag = false;
        DataTable AllData = GetData(TableName, GetDataWhere, ExplortFilter, ZHFilter);
        StringBuilder sb = new StringBuilder();
        if (AllData.Rows.Count > 0)
        {
            for (int i = 0; i < AllData.Rows.Count; i++)
            {
                for (int j = 0; j < AllData.Columns.Count; j++)
                {
                    sb.Append(AllData.Rows[i][j]);

                    if (j != AllData.Columns.Count - 1)
                        sb.Append("|");
                }
                sb.Append("\r\n");
            }
            Page.Response.Clear();
            Page.Response.Buffer = true;
            Page.Response.Charset = "gb2312";
            Page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".txt");
            Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文 
            Response.ContentType = "text/plain";//设置输出文件类型为txt文件。  
            this.EnableViewState = false;
            System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
            Page.Response.Write(sb.ToString().TrimEnd('|'));
            threadFlag = true;
            Page.Response.End();
        }
        AllData.Dispose();
        finishProgress();
        //object obj = "";
        //if (!string.IsNullOrEmpty(Request.QueryString["menuid"]))
        //{
        //    obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT name FROM cz_WorkflowInterfaces cwi WHERE cwi.id={0}", Request.QueryString["menuid"]));
        //}
        //title = obj == "" ? "资料下载" : obj.ToString();
        //th = new Thread(Threading);
        //th.IsBackground = true;
        //th.Start();
    }
    public static Thread th;
    public static StringBuilder sbAllText;
    public static bool threadFlag;
    public static string fieldName;
    public static string title;
    public void Threading()
    {
        threadFlag = false;
        //   string sql = GetSql();
        //DataTable AllData = GetData(TableName, GetDataWhere, ExplortFilter.Replace("|", ","), ZHFilter);


        //sbAllText = new StringBuilder();
        //if (AllData.Rows.Count > 0)
        //{
        //    for (int i = 0; i < AllData.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < AllData.Columns.Count; j++)
        //        {
        //            sbAllText.Append(AllData.Rows[i][j]);

        //            if (j != AllData.Columns.Count - 1)
        //                sbAllText.Append("|");
        //        }
        //        sbAllText.Append("\r\n");
        //    }
        //}

        TakeProTextAll();



        th.Abort();
        th = null;

    }

    void TakeProTextAll()
    {
        IDbDataParameter[] paramSet = IBatisDbHelper.CreateParameterSet(9);

        fieldName = title + "_" + DateTime.Now.ToString("yyyyMMddHHssmm");
        paramSet[0].ParameterName = "@shellSql";
        paramSet[0].Value = GetSql().Replace((char)13,(char)0).Replace((char)10,(char)0);
        paramSet[1].ParameterName = "@WinRarUrl";
        paramSet[1].Value = ConfigurationManager.AppSettings["WinRarUrl"];
        paramSet[2].ParameterName = "@FileUrl";
        paramSet[2].Value = ConfigurationManager.AppSettings["FileUrl"];
        paramSet[3].ParameterName = "@FileName";
        paramSet[3].Value = fieldName;
        paramSet[4].ParameterName = "@ServerName";
        paramSet[4].Value = ConfigurationManager.AppSettings["ServerName"];
        paramSet[5].ParameterName = "@ServerSa";
        paramSet[5].Value = ConfigurationManager.AppSettings["ServerSa"];
        paramSet[6].ParameterName = "@ServerPassword";
        paramSet[6].Value = ConfigurationManager.AppSettings["ServerPassword"];
        paramSet[7].ParameterName = "@shellSqlHeard";
        paramSet[7].Value = GetTableHead();
        paramSet[8].ParameterName = "@isHread";
        paramSet[8].Value = "1";
        try
        {
            IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Ucs_Export_TxtRAR", paramSet);
            threadFlag = true;
        }
        catch (Exception e)
        {
            log.Error(e.Message);
        }
    }


    string GetSql()
    {
        if (!string.IsNullOrEmpty(ExecSql))
        {
            return ExecSql;
        }
        StringBuilder sb = new StringBuilder();
        string field = !string.IsNullOrEmpty(ByGroup) ? GetField(ExplortFilter) : ExplortFilter;
        //   sb.Append(GetTableHead());
        // field = GetChineaseTableHeadField(field);
        // GetTableHead()
        sb.Append("select ").Append(field.Replace("|", ",")).Append(" from pms_zq.dbo.").Append(TableName).Append(" where ").Append(GetDataWhere);
        if (!string.IsNullOrEmpty(ByGroup))
        {
            sb.Append(" group by ").Append(ByGroup);
        }
        sb.Replace("dbo.fn_", "pms_zq.dbo.fn_").Replace("[dbo].[fn_", "pms_zq.dbo.[fn_");
        return sb.ToString();
    }
    string GetChineaseTableHeadField(string field)
    {
        string[] fid = field.Contains("|") ? field.Split('|') : field.Split(',');

        string[] ch = this.ZHFilter.Split(',');
        StringBuilder sb = new StringBuilder();
        for (var i = 0; i < ch.Length; i++)
        {
            int index = fid[i].ToLower().LastIndexOf(" as ");
            if (index > 0)
            {
                fid[i] = fid[i].Substring(0, index + 4) + "'" + ch[i] + "'";
            }
            else
            {
                fid[i] += " as '" + ch[i] + "'";
            }

            sb.Append(fid[i]);
            if (i != ch.Length - 1)
            {
                sb.Append(",");
            }
        }
        return sb.ToString();
    }

    string GetTableHead()
    {
        string[] text = this.ZHFilter.Split(',');
        StringBuilder sb = new StringBuilder();
        sb.Append("select ");
        for (var i = 0; i < text.Length; i++)
        {
            sb.Append("'");
            sb.Append(text[i]);
            sb.Append("'");
            if (i != text.Length - 1)
            {
                sb.Append(",");
            }
        }

        return sb.ToString();
    }
    void OutAllByText()
    {
    }


    private DataTable GetData(string tableName, string whereStr, string fileter, string zhfilter)
    {
        DataTable AllData = new DataTable();
        if (tableName.Trim() == string.Empty)
        {
            CommontUnit.Instance.Message("需要导出的数据不能为空", Response);
            return AllData;
        }
        if (ExportType == 0)
        {
            int recordCount = 0;
            if (!string.IsNullOrEmpty(ByGroup))
            {
                AllData = CommontUnit.Instance.GetUsersByPager(tableName, KeyFilter, GetField(ExplortFilter).Replace('|', ','), whereStr, ByOrder, 0, TotoalCount, ByGroup, ref recordCount);
            }

            else if (ExplortFilterEX == string.Empty)
            {
                AllData = CommontUnit.Instance.GetUsersByPager(tableName, KeyFilter, ExplortFilter.Replace("|", ","), whereStr, "", 0, TotoalCount, ref recordCount);
            }
            else
            {
                AllData = CommontUnit.Instance.GetUsersByPager(tableName, KeyFilter, ExplortFilterEX, whereStr, "", 0, TotoalCount, ref recordCount);
            }
        }
        return AllData;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Seach != null)
        {
            Seach(this, new EventArgs());
        }
        CommontUnit.Instance.Javascript("$(document.body).hideLoading();", Response);
    }
    /// <summary>
    /// 弹出窗口
    /// </summary>
    /// <param name="Msg"></param>
    private void msg(string Msg)
    {
        Response.Write("<script>alert('" + Msg + "');</script>");
    }

    protected void btn_This_Expor_To_Txt_Click(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        if (ExplortData != null)
        {
            if (ExplortData.Rows.Count > 0)
            {
                for (int i = 0; i < ExplortData.Rows.Count; i++)
                {

                    for (int j = 0; j < ExplortData.Columns.Count; j++)
                    {
                        sb.Append(ExplortData.Rows[i][j]);
                        if (j != ExplortData.Columns.Count - 1)
                            sb.Append("|");
                    }
                    if (ExplortFilter.Replace("|", ",").Split(',').Length > 2 || ExplortFilter.IndexOf("*") > -1)
                    {

                        sb.Append("\r\n");
                        continue;
                    }
                }
                Page.Response.Clear();
                Page.Response.Buffer = true;
                Page.Response.Charset = "gb2312";
                Page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".txt");
                Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文 
                Response.ContentType = "text/plain";//设置输出文件类型为txt文件。  
                this.EnableViewState = false;
                System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
                System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
                Page.Response.Write(sb.ToString().TrimEnd('|'));
                Page.Response.End();
            }
            ExplortData.Dispose();
            CommontUnit.Instance.Javascript("$(document.body).hideLoading();", Response);
        }
    }

    /// <summary>
    /// 用Aspose.cells组件导出excel文档
    /// </summary>
    /// <param name="Response"></param>
    /// <param name="tb"></param>
    public void ExportToExcel(HttpResponse Response, DataTable tb)
    {
        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet = workbook.Worksheets[0]; //工作表 
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

        int Rownum = tb.Rows.Count;//表格行数  
        int TitleIndex = 0;
        string[] s = Gets(ExplortFilter);
        string[] s2 = ("序号,"+ZHFilter).Split(',');
        //ExcelHelp.Export2Excel(DateTime.Now.ToString("yyyyMMddHHssmm"), AllData, s, s2);

        for (int i = 0; i < s2.Length; i++)
        {
            if (!s2[i].Contains("thres"))
            {
                sheet.Cells[a_zArrary[i] + "1"].PutValue(s2[i]);
                sheet.Cells[a_zArrary[i] + "1"].SetStyle(styleHeader);
                sheet.Cells.SetRowHeight(0, 25);
                sheet.Cells.SetColumnWidth(i, 17);
            }
        }
        //foreach (DataColumn dc in tb.Columns)
        //{
        //    //生成行1 表头    			   
        //    cells[0, TitleIndex].PutValue(dc.ColumnName);//填写内容 
        //    cells[0, TitleIndex].SetStyle(styleHeader);
        //    cells.SetRowHeight(0, 25);
        //    cells.SetColumnWidth(TitleIndex, 17);
        //    TitleIndex++;
        //}

        for (int j = 0; tb != null && j < tb.Rows.Count; j++)
        {
            int i = j + 1;
            int columnIndex = 0;

            cells.SetRowHeight(0, 13);
            foreach (DataColumn dc in tb.Columns)
            {
                cells[i, columnIndex].PutValue(tb.Rows[j][dc.ColumnName].ToString());
                cells[i, columnIndex].SetStyle(style2);
                columnIndex++;
            }
        }

        string createtime = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHssmm");
        string doc_name = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.UTF8);
        doc_name += createtime + ".xls";
        Response.ContentType = "application/ms-excel;charset=utf-8";

        Response.AddHeader("content-disposition", "attachment; filename=" + doc_name);

        System.IO.MemoryStream memStream = workbook.SaveToStream();

        Response.BinaryWrite(memStream.ToArray());

        Response.End();
        //workbook.Save(doc_name, Aspose.Cells.SaveType.OpenInExcel, Aspose.Cells.FileFormatType.Excel2003, Response);
    }
    //public static void ExportEasy(DataTable dtSource, string strFileName)
    //{
    //    HSSFWorkbook workbook = new HSSFWorkbook();
    //    HSSFSheet sheet = workbook.CreateSheet();

    //    //填充表头
    //    HSSFRow dataRow = sheet.CreateRow(0);
    //    foreach (DataColumn column in dtSource.Columns)
    //    {
    //        dataRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
    //    }


    //    //填充内容
    //    for (int i = 0; i < dtSource.Rows.Count; i++)
    //    {
    //        dataRow = sheet.CreateRow(i + 1);
    //        for (int j = 0; j < dtSource.Columns.Count; j++)
    //        {
    //            dataRow.CreateCell(j).SetCellValue(dtSource.Rows[i][j].ToString());
    //        }
    //    }


    //    //保存
    //    using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
    //    {
    //        workbook.Write(fs);
    //    }
    //    workbook.Dispose();
    //}
    string defaultArea = ConfigurationManager.AppSettings["DefaultArea"];
    string defaultBrnd = ConfigurationManager.AppSettings["DefaultBrnd"];
    protected string GetField(string field)
    {
        StringBuilder sb = new StringBuilder(field);
        return sb.ToString();
    }
    private void finishProgress()
    {
        string jsBlock = "<script>hide();</script>";//
        Response.Write(jsBlock);
        Response.Flush();
    }
    //private void beginProgress()
    //{
    //    //根据ProgressBar.htm显示进度条界面
    //    string templateFileName = Path.Combine(Server.MapPath("."), "ProgressBar.htm");
    //    StreamReader reader = new StreamReader(@templateFileName, System.Text.Encoding.GetEncoding("GB2312"));
    //    string html = reader.ReadToEnd();
    //    reader.Close();
    //    Response.Write(html);
    //    Response.Flush();
    //}


}