using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Botwave.Report;
using Botwave.Web.Controls;
using Botwave.Commons;

public partial class contrib_report_TableView : System.Web.UI.UserControl
{
    #region
    public string TableName
    {
        set { tbName.Text = value; }
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
    private const int TablePageSize = 20;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tbList.CssClass = "tblClass";
        }
    }
    /// <summary>
    /// 响应列表排序功能的JS触发的回调事件.
    /// </summary>
    /// <param name="orderField"></param>
    public void SortData(string orderField)
    {
        string oldOrderBy = DbUtils.ToString(ViewState["REPORTORDERBY"]);
        string sc = "asc";
        string field = "";
        string[] ss = oldOrderBy.Split(' ');
        if (ss.Length > 1)
        {
            field = ss[0];
            sc = ss[1];
        }
        if (field == orderField)
            sc = sc == "asc" ? "desc" : "asc";
        ViewState["REPORTORDERBY"] = string.Format("{0} {1}", orderField, sc);

        listPager.CurrentPageIndex = 0;
        BindData(0);
    }
    /// <summary>
    /// 绑定数据到列表.
    /// </summary>
    public DataTable BindData()
    {
        return BindData(listPager.CurrentPageIndex);
    }
    private DataTable BindData(int pageIndex)
    {
        DataTable dt = ReportViewDAL.GetDataSetBySqlScript(SqlScript).Tables[0];
        if (ShowFields == null)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                s.AppendFormat(",{0}", dt.Columns[i].ColumnName);
            }
            if (s.Length > 0)
                s.Remove(0, 1);
            ShowFields = s.ToString();
        }

        string[] dataFields = ShowFields.Replace("[", "").Replace("]", "").Split(',');

        tbList.Rows.Clear();
        TableHelper.BindTable(tbList, FirstTitles, SecondTitles, ThirdTitles, dataFields, new DataTable());
        for (int i = 0; i < tbList.Rows[0].Cells.Count; i++)
        {
            dt.Columns[i].ColumnName = tbList.Rows[tbList.Rows.Count - 1].Cells[i].Text;
        }
        DataView dv = dt.DefaultView;
        dv.Sort = DbUtils.ToString(ViewState["REPORTORDERBY"]);

        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = dv;
        pds.AllowPaging = true;
        pds.PageSize = TablePageSize;
        pds.CurrentPageIndex = pageIndex;

        IEnumerator data = pds.GetEnumerator();
        while (data.MoveNext())
        {
            DataRowView drv = (DataRowView)data.Current;
            Append2Table(tbList, drv);
        }

        listPager.TotalRecordCount = dt.Rows.Count;
        listPager.ItemsPerPage = TablePageSize;
        listPager.CurrentPageIndex = pageIndex;
        listPager.DataBind();

        return dt;
    }
    private void Append2Table(Table table, DataRowView drv)
    {
        object[] objs = drv.Row.ItemArray;

        TableRow tr = new TableRow();
        foreach (object obj in objs)
        {
            TableCell tc = new TableCell();
            tc.Text = obj.ToString();
            tr.Cells.Add(tc);
        }
        table.Rows.Add(tr);
    }

    protected void listPager_PageIndexChanged(object sender, PageChangedEventArgs e)
    {
        BindData(listPager.CurrentPageIndex);
    }
    protected void btnDownLoad_Click(object sender, EventArgs e)
    {
        //导出DatTable数据到Excle，不需要预定义模板，完全动态实现，有待实现
    }
}
