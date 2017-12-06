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
using Botwave.Commons;
using Botwave.Report.Common;
using System.Text;

public partial class contrib_report_pages_ReportCreate2 : Botwave.Security.Web.PageBase
{
    private int ReportID
    {
        get { return DbUtils.ToInt32(Request.QueryString["id"], 0); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Botwave.Web.WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Base64.js");
        if (!IsPostBack)
        {
            BindData();
        }
    }
    // 还原报表名称
    private void BindData()
    {
        if (ReportID == 0)
            return;
        ReportEntity report = ReportDAL.GetReportByID(ReportID);
        if (report.SourceType == 2)
        {
            txtReportName.Text = report.Name;
            txtRemark.Text = report.Remark;
            txtSQL.Text = report.ReportSql;
        }
    }
    protected void btnSaveSql_Click(object sender, EventArgs e)
    {
        string sql = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8",txtSQL.Text.Trim());
        if (!ReportUtils.ValideSafeSql(sql))
        {
            Response.Write("<script>alert('SQL语句中包含危险关键字，提交被拒绝。');</script>");
            return;
        }
        ReportEntity report = new ReportEntity();
        report.Creator = CurrentUserName;
        report.Name = txtReportName.Text.Trim();
        report.Remark = txtRemark.Text.Trim();
        report.SourceType = 2;
        report.ReportSql = sql;
        report.Id = ReportID;

        ReportDAL.SaveReport(report);
        Response.Write("<script>alert('提交成功');window.location.href = 'ReportList.aspx';</script>");
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string sqlData = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8",txtSQL.Text);
        txtSQL.Text = sqlData;
        if (!ReportUtils.ValideSafeSql(sqlData))
        {
            Response.Write("<script>alert('SQL语句中包含危险关键字，提交被拒绝。');</script>");
            return;
        }
        DataSet ds = ReportDAL.ExecuteReportSql(sqlData);

        StringBuilder sb = new StringBuilder();
        sb.Append("<html>");
        sb.Append("<head><title>报表数据预览</title>");
        sb.Append("<style>table{BORDER-COLLAPSE: collapse;border: 1px solid black;}");
        sb.Append("table th,table td{border:1px solid #999999;empty-cells:show;}");
        sb.Append("</style></head>");
        sb.Append("<body><div class='div_pre_grid'>");

        sb.Append("<table width='100%' cellspacing='0' border='1'>");
        sb.Append("<tr>");
        foreach (DataColumn dc in ds.Tables[0].Columns)
        {
            sb.AppendFormat("<th>{0}</th>", dc.ColumnName);
        }
        sb.AppendFormat("</tr>");
        int columnLength = ds.Tables[0].Columns.Count;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            sb.Append("<tr>");

            for (int j = 0; j < columnLength; j++)
            {
                sb.AppendFormat("<td>{0}</td>", dr[j].ToString());
            }
            sb.Append("</tr>");
        }
        sb.Append("</table>");
        sb.Append("</div></body></html>");

        StringBuilder script = new StringBuilder();
        script.Append("<script>var newwindow = window.open(\"ReportPreview.html\", \"\", \"\");");
        script.AppendFormat("newwindow.document.write(\"{0}\");", sb.ToString());
        script.Append("newwindow.document.close();</script>");

        Page.RegisterClientScriptBlock(Guid.NewGuid().ToString(), script.ToString());
    }
}