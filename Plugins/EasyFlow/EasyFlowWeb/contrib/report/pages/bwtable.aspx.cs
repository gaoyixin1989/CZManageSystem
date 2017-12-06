using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Botwave.Report.Common;
using Botwave.Commons;
using Botwave.Report;
using System.Text;
using System.Collections.Generic;
using Botwave.Report.DataAccess;

public partial class contrib_report_pages_bwtable : System.Web.UI.Page
{
    #region
    /// <summary>
    /// 报表ID
    /// </summary>
    private int ReportID
    {
        get { return DbUtils.ToInt32(Request.QueryString["id"], 0); }
    }
    private string sqlWhere
    {
        get { return DbUtils.ToString(Request.QueryString["where"], ""); }
    }
    /// <summary>
    /// 报表排序字段
    /// </summary>
    private string ReportOrderBy
    {
        get { return DbUtils.ToString(ViewState["REPORTORDERBY"]); }
        set { ViewState["REPORTORDERBY"] = value; }
    }
    /// <summary>
    /// 第一行列头显示名称，格式如 new string[]{"08年","09年","10年"}
    /// </summary>
    public string[] FirstTitles
    {
        get { return ViewState["FirstTitles"] == null ? null : (string[])ViewState["FirstTitles"]; }
        set { ViewState["FirstTitles"] = value; }
    }
    /// <summary>
    /// 第二列列头显示名词，格式如 new string[]{"春季,夏季","秋季","冬季"}
    /// </summary>
    public string[] SecondTitles
    {
        get { return ViewState["SecondTitles"] == null ? null : (string[])ViewState["SecondTitles"]; }
        set { ViewState["SecondTitles"] = value; }
    }
    /// <summary>
    /// 第三列显示名词，格式如 new string[]{"一月;二月,六月;七月","九月;十月","十二月"}
    /// </summary>
    public string[] ThirdTitles
    {
        get { return ViewState["ThirdTitles"] == null ? null : (string[])ViewState["ThirdTitles"]; }
        set { ViewState["ThirdTitles"] = value; }
    }
    /// <summary>
    /// 用于从数据库中读取数据集以填充列表的SQL脚本.
    /// </summary>
    public string SqlScript
    {
        get { return ViewState["SqlScript"] == null ? null : (string)ViewState["SqlScript"]; }
        set { ViewState["SqlScript"] = value; }
    }
    /// <summary>
    /// 报表的列头（最靠近数据列一行的列头）格式如：a,b,c
    /// </summary>
    public string ShowFields
    {
        get { return ViewState["ShowFields"] == null ? null : (string)ViewState["ShowFields"]; }
        set { ViewState["ShowFields"] = value; }
    }
    /// <summary>
    /// 报表数据源类型：1 配置；2 SQL；3 存储过程. 
    /// </summary>
    public int SourceType
    {
        get { return DbUtils.ToInt32(ViewState["SourceType"], 1); }
        set { ViewState["SourceType"] = value; }
    }
    private const int TablePageSize = 20;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (ReportID == 0)
                return;
            ReportEntity report = ReportDAL.GetReportByID(ReportID);
            lblName.Text = report.Name;

            string sql = report.ReportSql.Trim();
            if (report.SourceType != 3 && sqlWhere.Length > 0)
                sql += sql.Contains(" WHERE ") ? " and " + sqlWhere : " WHERE " + sqlWhere.Trim();

            SourceType = report.SourceType;
            SqlScript = sql;
            FirstTitles = report.FirstTitles.Split('|');
            SecondTitles = report.SecondTitles.Split('|');
            ThirdTitles = report.ThirdTitles.Split('|');
            ShowFields = DataHelper.GetTableShowField(report);

            BindData(0);
        }
        else
        {
            string et = DbUtils.ToString(Request["__EVENTTARGET"]);
            string eg = DbUtils.ToString(Request["__EVENTARGUMENT"]);

            if (et == "BWTABLEORDER")
                SortData(eg);
            else if (et == "EXPORTEXCEL")
                Export2Excel();
        }
    }
    /// <summary>
    /// 响应列表排序功能的JS触发的回调事件.
    /// </summary>
    /// <param name="orderField"></param>
    private void SortData(string orderField)
    {
        ReportOrderBy = DataHelper.ActionOrderByField(orderField, ReportOrderBy);

        listPager.CurrentPageIndex = 0;
        BindData(0);
    }
    /// <summary>
    /// 把列表数据导出到Excel.
    /// </summary>
    private void Export2Excel()
    {
        //导出DatTable数据到Excle，不需要预定义模板，完全动态实现，有待实现
        //目前为SZ系统需求，增加导出功能，不作任何处理，只需导出默认字段和数据       
        DataGrid dg = new DataGrid();
        dg.DataSource = DataHelper.GetSourceData(SqlScript, ReportID, SourceType, sqlWhere);
        dg.DataBind();

        Botwave.XQP.Commons.XQPHelper.ExportExcel(dg, lblName.Text);
        dg.Dispose();
    }
    /// <summary>
    /// 绑定数据到列表.
    /// </summary>
    private void BindData(int pageIndex)
    {
        //读出原始数据.
        DataTable dt = DataHelper.GetSourceData(SqlScript, ReportID, SourceType, sqlWhere);
        if (ShowFields == null)
        {
            ShowFields = DataHelper.GetTableShowField(dt);
        }
        string[] dataFields = ShowFields.Replace("[", "").Replace("]", "").Split(',');

        tbList.Rows.Clear();

        //绑定列头.
        TableHelper.BindTable(tbList, FirstTitles, SecondTitles, ThirdTitles, dataFields, new DataTable());
        for (int i = 0; i < tbList.Rows[0].Cells.Count; i++)
        {
            dt.Columns[i].ColumnName = tbList.Rows[tbList.Rows.Count - 1].Cells[i].Text;
        }

        //绑定分页之后的内容.
        DataView dv = dt.DefaultView;
        dv.Sort = ReportOrderBy;

        DataHelper.PagingDataSource(dv, TablePageSize, pageIndex, tbList);

        //分页控件赋值.
        listPager.TotalRecordCount = dt.Rows.Count;
        listPager.ItemsPerPage = TablePageSize;
        listPager.CurrentPageIndex = pageIndex;
        listPager.DataBind();
    }


    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        BindData(listPager.CurrentPageIndex);
    }
}
