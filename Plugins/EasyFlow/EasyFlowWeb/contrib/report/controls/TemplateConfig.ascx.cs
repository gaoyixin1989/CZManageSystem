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
using System.Text;
using Botwave.Report.DataAccess;
using System.Collections.Generic;
using Botwave.Report.Common;
using Botwave.Report;

public partial class contrib_report_controls_TemplateConfig : System.Web.UI.UserControl
{
    private int _reportID;
    /// <summary>
    /// 报表编号
    /// </summary>
    public int ReportID
    {
        get { return _reportID; }
        set { _reportID = value; }
    }
    /// <summary>
    /// 用于绑定动态数据的数据源
    /// </summary>
    public IDictionary<string, string> AutoDataSource
    {
        set
        {
            ddlData.Items.Clear();
            ddlData.DataSource = value;
            ddlData.DataTextField = "key";
            ddlData.DataValueField = "value";
            ddlData.DataBind();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlData.Attributes["onchange"] = "AddData2Template(this);";
            BindPage();
        }
    }
    private void BindPage()
    {
        TemplateConfig model = TemplateConfigDAL.GetModel(ReportID);
        FCKContent.Value = model.TemplateText;
        if (FCKContent.Value.Length == 0)
        {
            //添加HTML按钮控件：查询、打印和返回。
            FCKContent.Value = "<input type=\"button\" class=\"btn_query\" value=\"查询\" onclick=\"GetSqlWhere();\" /><table width=\"100%\"><tr><td align=\"right\"><input id=\"btnPrint\" onclick=\"printPage();\" type=\"button\" value=\"打印\" class=\"btn_query\" /><input type=\"button\" value=\"打印预览\" class=\"btn2\" onclick=\"window.open('bwprintpreview.aspx');\" /> <input type=\"button\" class=\"btn_sav\" value=\"导出\" onclick=\"exporExcel();\" /> <input type=\"button\" onclick=\"window.location.href('ReportList.aspx')\" value=\"返回\" class=\"btnReturnClass\" /></td></tr></table>";
        }

        ReportEntity report = ReportDAL.GetReportByID(ReportID);
        DataTable dt = null;
        if (report.SourceType != 3)
            dt = Botwave.Report.ReportViewDAL.GetDataSetBySqlScript(report.ReportSql).Tables[0];
        else
        {
            DataTable dtemp = ReportItemDAL.GetListByReportID(ReportID).Tables[0];
            IDictionary<string, string> para = new Dictionary<string, string>();
            foreach (DataRow dr in dtemp.Rows)
            {
                para.Add(dr["Parameter"].ToString(), string.Format("{0}|{1}", dr["DataType"].ToString(), dr["DefaultValue"].ToString()));
            }
            dt = ReportViewDAL.GetDataSetBySP(report.ReportSql, para).Tables[0];
        }
        DataTable dtData = new DataTable();
        dtData.Columns.Add("ColumnName", typeof(string));
        foreach (DataColumn dc in dt.Columns)
        {
            DataRow drData = dtData.NewRow();
            drData["ColumnName"] = dc.ColumnName;
            dtData.Rows.Add(drData);
        }
        ddlData.DataSource = dtData;
        ddlData.DataBind();
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        //目前数据保存到数据库，暂停保存模板文件
        //string path = string.Format("{0}\\contrib\\report\\res\\Template\\{1}.html", Server.MapPath("~"), ReportID.ToString());
        //using (StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding(936)))
        //{
        //    sw.Write(FCKContent.Value);
        //}

        TemplateConfig model = new TemplateConfig();
        model.ReportID = ReportID;
        model.TemplateText = FCKContent.Value;

        int i = TemplateConfigDAL.UpdateTemplateText(model);
        if (i > 0)
            Page.RegisterClientScriptBlock(Guid.NewGuid().ToString(), "<script>alert('提交成功');</script>");
    }
}
