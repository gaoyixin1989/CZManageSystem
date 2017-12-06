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
using ZedGraph.Web;
using System.Drawing;
using ZedGraph;
using Botwave.Report.DrawGrapBase;
using System.Collections.Generic;
using Botwave.Report.Common;
using Botwave.Report.DataAccess;
using Botwave.Commons;
using Botwave.Report;

public partial class contrib_report_pages_bwgrap : System.Web.UI.Page
{
    #region Public Property
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
    private string _title;
    /// <summary>
    /// 统计图的名称.
    /// </summary>
    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }
    private string _xAxisTitle;
    /// <summary>
    /// 横轴的名称（饼图不需要）.
    /// </summary>
    public string XAxisTitle
    {
        get { return _xAxisTitle; }
        set { _xAxisTitle = value; }
    }
    private string _yAxisTitle;
    /// <summary>
    /// 纵轴的名称（饼图不需要）.
    /// </summary>
    public string YAxisTitle
    {
        get { return _yAxisTitle; }
        set { _yAxisTitle = value; }
    }
    private ChartType _type;
    /// <summary>
    /// 显示的曲线类型：Line,Bar,Pie
    /// </summary>
    public ChartType Type
    {
        get { return _type; }
        set { _type = value; }
    }
    private DataTable _dataSource;
    /// <summary>
    /// 数据源.
    /// </summary>
    public DataTable DataSource
    {
        get { return _dataSource; }
        set { _dataSource = value; }
    }
    private string _xFieldNames;
    /// <summary>
    /// 用于填充X轴的数据字段（PIE图时作说明字段）.
    /// </summary>
    public string XFieldNames
    {
        get { return _xFieldNames; }
        set { _xFieldNames = value; }
    }
    private string _yFieldNames;
    /// <summary>
    /// 用于填充Y轴的数据字段（多个字段用“,”号分隔开）.
    /// </summary>
    public string YFieldNames
    {
        get { return _yFieldNames; }
        set { _yFieldNames = value; }
    }
    private string _tagNames;
    /// <summary>
    /// 标识内容集的说明（多个内容集用“,”号分隔开）.
    /// </summary>
    public string TagNames
    {
        get { return _tagNames; }
        set { _tagNames = value; }
    }
    private int _axisType = 3;
    /// <summary>
    /// X坐标轴类型，linear(0)是默认值，Date(2)是用于x轴是日期的数据，Text(3)会忽略x上的数据.
    /// </summary>
    public int AxisType
    {
        get { return _axisType; }
        set { _axisType = value; }
    }

    private string[] _xAxisLables;
    /// <summary>
    /// 拥有填充X轴的内容
    /// </summary>
    public string[] XAxisLables
    {
        get { return _xAxisLables; }
        set { _xAxisLables = value; }
    }

    private int _width = 700;
    /// <summary>
    /// 统计图的宽度.
    /// </summary>
    public int Width
    {
        get { return _width; }
        set { _width = value; }
    }

    private int _height = 360;
    /// <summary>
    /// 统计图的高度.
    /// </summary>
    public int Height
    {
        get { return _height; }
        set { _height = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        zedGraphControl.Width = Width;
        zedGraphControl.Height = Height;
        zedGraphControl.RenderGraph += new ZedGraphWebControlEventHandler(zedGraphControl_RenderGraph);

        if (!IsPostBack)
        {
            List<GrapConfig> gcs = DrawGrapDAL.ListReportGrap(ReportID);
            if (gcs.Count == 0)
            {
                zedGraphControl.Visible = false;
                return;
            }
            ReportEntity report = ReportDAL.GetReportByID(ReportID);
            foreach (GrapConfig gc in gcs)
            {
                AxisType = gc.XType.Value;
                TagNames = gc.TagNames;
                Title = gc.Title;
                Type = (ChartType)gc.Type;
                Width = DbUtils.ToInt32(gc.Width);
                XAxisTitle = gc.XTitle;
                XFieldNames = gc.XFieldName;
                YAxisTitle = gc.YTitle;
                YFieldNames = gc.YFieldNames;
                Height = DbUtils.ToInt32(gc.Height);

                string sql = report.ReportSql;             
                if (report.SourceType != 3 && sqlWhere.Length > 0)
                {
                    sql += sql.Contains(" WHERE ") ? " and " + sqlWhere : " WHERE " + sqlWhere.Trim();
                }

                DataSource = DataHelper.GetSourceData(sql, ReportID, report.SourceType, sqlWhere);
                Draw();
            }
        }
    }
    private MasterPane tempPane;
    /// <summary>
    /// 图表处理
    /// </summary>
    private void Draw()
    {
        GraphPane myPane = new GraphPane();

        if (!string.IsNullOrEmpty(Title))
            myPane.Title.Text = Title;
        if (!string.IsNullOrEmpty(XAxisTitle))
            myPane.XAxis.Title.Text = XAxisTitle;
        if (!string.IsNullOrEmpty(YAxisTitle))
            myPane.YAxis.Title.Text = YAxisTitle;

        myPane.XAxis.Type = (AxisType)AxisType;
        string[] xlable = new string[DataSource.Rows.Count];
        for (int i = 0; i < DataSource.Rows.Count; i++)
        {
            xlable[i] = DataSource.Rows[i][XFieldNames].ToString();
        }
        myPane.XAxis.Scale.TextLabels = xlable;

        switch (Type)
        {
            case ChartType.Line:
                ChartHelper.DrawLine(myPane, DataSource, XFieldNames, YFieldNames, TagNames);
                break;
            case ChartType.Bar:
                ChartHelper.DrawBar(myPane, DataSource, XFieldNames, YFieldNames, TagNames);
                break;
            case ChartType.Pie:
                ChartHelper.DrawPie(myPane, DataSource, XFieldNames, YFieldNames);
                break;
            case ChartType.BarLine:
                ChartHelper.DrawBarLine(myPane, DataSource, XFieldNames, YFieldNames, TagNames);
                break;
            default:
                break;
        }
        if (tempPane == null)
            tempPane = new MasterPane();
        tempPane.Add(myPane);
    }
    /// <summary>
    /// 画板处理
    /// </summary>
    /// <param name="webObject"></param>
    /// <param name="g"></param>
    /// <param name="pane"></param>
    private void zedGraphControl_RenderGraph(ZedGraphWeb webObject, Graphics g, MasterPane pane)
    {
        if (tempPane != null)
        {
            pane.PaneList.RemoveAt(0);
            for (int i = 0; i < tempPane.PaneList.Count; i++)
            {
                pane.Add(tempPane[i]);
            }
        }
    }
}