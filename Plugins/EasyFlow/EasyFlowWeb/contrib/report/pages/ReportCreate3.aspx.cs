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
using Botwave.Report.DataAccess;

public partial class contrib_report_pages_ReportCreate3 : Botwave.Security.Web.PageBase
{
    private int ReportID
    {
        get { return DbUtils.ToInt32(Request.QueryString["id"], 0); }
    }
    private DataTable ItemTable
    {
        get { return (DataTable)ViewState["ReportSPItem"]; }
        set { ViewState["ReportSPItem"] = value; }
    }
    protected string GetTypeName(string typeValue)
    {
        switch (typeValue)
        {
            case "string":
                return "字符型";
            case "bool":
                return "布尔型";
            case "int":
                return "整型";
            case "datetime":
                return "时间型";
            case "float":
                return "浮点型";
            case "uniqueidentifier":
                return "GUID型";
            default:
                return "字符型";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindItem();
        }
    }
    private void BindItem()
    {
        if (ReportID == 0)
            return;
        ReportEntity report = ReportDAL.GetReportByID(ReportID);
        txtReportName.Text = report.Name;
        txtRemark.Text = report.Remark;
        txtSP.Text = report.ReportSql;

        DataTable dt = ReportItemDAL.GetListByReportID(ReportID).Tables[0];

        dgItem.DataSource = dt;
        dgItem.DataBind();

        ItemTable = dt;
    }
    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        DataTable dt = ItemTable;
        if (dt == null || dt.Columns.Count == 0)
        {
            dt = new DataTable();
            dt.Columns.Add("Parameter", typeof(string));
            dt.Columns.Add("DataType", typeof(string));
            dt.Columns.Add("DefaultValue", typeof(string));
        }
        DataRow dr = dt.NewRow();
        dr["Parameter"] = txtParameter.Text.Trim();
        dr["DataType"] = ddlDataType.SelectedValue;
        dr["DefaultValue"] = txtDefaultValue.Text.Trim();
        dt.Rows.Add(dr);

        dgItem.DataSource = dt;
        dgItem.DataBind();

        ItemTable = dt;
    }
    protected void btnSaveSql_Click(object sender, EventArgs e)
    {
        ReportEntity report = new ReportEntity();
        report.Creator = CurrentUserName;
        report.Name = txtReportName.Text.Trim();
        report.Remark = txtRemark.Text.Trim();
        report.SourceType = 3;
        report.ReportSql = txtSP.Text.Trim();
        report.Id = ReportID;

        DataTable dt = ItemTable;

        bool result = ReportItemDAL.SaveReportInfo(report, dt);
        if (result)
            Response.Write("<script>alert('提交成功');window.location.href = 'ReportList.aspx';</script>");
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        StringBuilder sqlData = new StringBuilder();
        sqlData.AppendFormat("exec {0} ", txtSP.Text.Trim());
        DataTable dt = ItemTable;
        foreach (DataRow dr in dt.Rows)
        {
            switch (dr["DataType"].ToString())
            {
                case "int":
                case "bool":
                case "float":
                case "uniqueidentifier":
                    sqlData.AppendFormat("{0},", dr["DefaultValue"].ToString());
                    break;
                default:
                    sqlData.AppendFormat("'{0}',", dr["DefaultValue"].ToString());
                    break;
            }
        }
        if (dt.Rows.Count > 0)
            sqlData.Remove(sqlData.Length - 1, 1);

        DataSet ds = ReportDAL.ExecuteReportSql(sqlData.ToString());

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
    protected void dgItem_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        DataTable dt = ItemTable;
        dt.Rows.RemoveAt(e.Item.ItemIndex);

        dgItem.DataSource = dt;
        dgItem.DataBind();

        ItemTable = dt;
    }
}
