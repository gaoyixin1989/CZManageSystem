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

using System.IO;
using Commons.Collections;
using NVelocity.App;
using NVelocity;
using NVelocity.Runtime;
using NVelocity.Context;
using Botwave.Report.Common;
using Botwave.Report.DataAccess;
using Botwave.Commons;
using System.Collections.Generic;

public partial class contrib_report_controls_TemplateView : System.Web.UI.UserControl
{
    /// <summary>
    /// 报表ID
    /// </summary>
    private int ReportID
    {
        get { return DbUtils.ToInt32(Request.QueryString["id"], 0); }
    }
    /// <summary>
    /// 报表查询条件（客户端输入）.
    /// </summary>
    private string SqlWhere
    {
        get { return DbUtils.ToString(Request.QueryString["where"], ""); }
    }
    private IDictionary<string, string> _autoDataSource;
    /// <summary>
    /// 填充到模板的数据.
    /// </summary>
    public IDictionary<string, string> AutoDataSource
    {
        set { _autoDataSource = value; }
        get { return _autoDataSource; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (ReportID == 0)
                return;
            BindTemplate();
        }
    }

    /// <summary>
    /// 把模板数据绑定到页面.
    /// </summary>
    /// <param name="templateName"></param>
    private void BindTemplate()
    {
        #region 从模板文件中读取模板信息，暂由从数据库读取模板方法代替
        //string templateName = ReportID.ToString();
        //string path = string.Format("../res/Template/{0}.html", templateName);
        //if (!System.IO.File.Exists(Server.MapPath(path)))
        //{
        //    return;
        //}       
        //创建NVelocity引擎的实例对象.
        //VelocityEngine velocity = new VelocityEngine();      
        ////初始化该实例对象.
        //ExtendedProperties props = new ExtendedProperties();
        //props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
        ////可换成：props.AddProperty("resouce.loader","file"),以下的同道理.
        //props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, Path.GetDirectoryName(Request.PhysicalPath));
        //props.AddProperty(RuntimeConstants.INPUT_ENCODING, "gb2312");
        //props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "gb2312");
        //velocity.Init(props);
        ////从文件中读取模板
        //Template temp = velocity.GetTemplate(path);
        //IContext context = GetContextData();
        ////合并模板
        //StringWriter writer = new StringWriter();
        ////velocity.MergeTemplate(context, writer);
        //temp.Merge(context, writer);
        ////输入
        //lblView.Text = writer.ToString().Replace("\r\n", "<br/>");
        #endregion

        //从数据库中读取模板信息.
        TemplateConfig model = TemplateConfigDAL.GetModel(ReportID);
        if (model.TemplateText == null || model.TemplateText.Length == 0)
        {
            lblView.Text = "<table width=\"100%\"><tr><td align=\"right\"><input id=\"btnPrint\" onclick=\"printPage();\" type=\"button\" value=\"打印\" class=\"btn_query\" /><input type=\"button\" value=\"打印预览\" class=\"btn2\" onclick=\"window.open('bwprintpreview.aspx');\" /> <input type=\"button\" class=\"btn_sav\" value=\"导出\" onclick=\"exporExcel();\" /> <input type=\"button\" onclick=\"window.location.href('ReportList.aspx')\" value=\"返回\" class=\"btnReturnClass\" /></td></tr></table>";
        }
        else
            lblView.Text = model.TemplateText.Replace("\r\n", "");
        //VelocityEngine velocity = new VelocityEngine();
        //velocity.Init();

        //IContext context = GetContextData(model);
        //System.IO.StringWriter writer = new System.IO.StringWriter();
        //velocity.Evaluate(context, writer, null, model.TemplateText);

        //lblView.Text = writer.ToString().Replace("\r\n", "");
    }
    //private IContext GetContextData(TemplateConfig model)
    //{
    //    IContext context = new VelocityContext();
    //    if (AutoDataSource != null && AutoDataSource.Count > 0)
    //    {
    //        foreach (KeyValuePair<string, string> p in AutoDataSource)
    //        {
    //            context.Put(p.Key, p.Value);
    //        }
    //    }
    //    else
    //    {
    //        DataTable dt = SqlHelper.ExecuteDataset(CommandType.Text, model.ReportSql).Tables[0];
    //        string _text = model.TextField.Length > 0 ? model.TextField : dt.Columns[1].ColumnName;
    //        string _value = model.ValueField.Length > 0 ? model.ValueField : dt.Columns[0].ColumnName;
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            context.Put(dr[_text].ToString(), dr[_value].ToString());
    //        }
    //    }
    //    return context;
    //}
}
