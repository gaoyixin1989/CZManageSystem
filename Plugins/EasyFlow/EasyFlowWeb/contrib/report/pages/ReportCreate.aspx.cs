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
using System.Collections.Generic;
using Botwave.Report.Common;
using Botwave.Commons;
using System.Text;

public partial class ReportCreate : Botwave.Security.Web.PageBase, ICallbackEventHandler
{
    public int ReportID
    {
        get { return DbUtils.ToInt32(Request.QueryString["id"], 0); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string strRefrence = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveDataFromServer", "context", "CallBackError", true);
            string strCallBack = "function CallBackToTheServer(arg, context) {" + strRefrence + "};";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallBackToTheServer", strCallBack, true);  // CallBackToTheServer，JS方法，发出回调请求

            InitPage();
            if (ReportID != 0)
            {
                //恢复数据
                RestoreModifyState();
            }
        }
    }
    private void InitPage()
    {
        ddlSource.Attributes["onchange"] = "DropSelectedChange(this)";
        //从XML读取配置信息
        //IList<ShowField> fields = ReportParser.ParseShowField();

        //从数据库读取配置信息
        ddlSource.DataSource = ReportConfigDAL.ListTableSet();
        ddlSource.DataBind();

        ddlSource.Items.Insert(0, new ListItem("--请选择--", ""));
    }
    public void RestoreModifyState()
    {
        ReportEntity report = ReportDAL.GetReportByID(ReportID);
        IList<ReportItem> reportItems = ReportDAL.GetReportItemByReportID(ReportID);

        txtReportName.Text = report.Name;
        txtRemark.Text = report.Remark;
        ddlSource.SelectedValue = report.TableName;

        DataTable dt = ReportConfigDAL.GetTableColumnByTable(report.TableName);
        column_list.InnerHtml = GetColumnSelectByTable(dt);

        foreach (ReportItem item in reportItems)
        {
            HtmlTableRow newRow = new HtmlTableRow();

            HtmlTableCell newCell = new HtmlTableCell();
            newCell.InnerHtml = string.Format("<input name=\"checkbox_name\" type=\"checkbox\" class=\"chk\" {0}/>", item.IsShowField ? "checked=\"checked\"" : "");
            newRow.Cells.Add(newCell);

            newCell = new HtmlTableCell();
            newCell.Attributes.Add("class", "column");
            newCell.Attributes.Add("onfocus", "ColumnFocus(this)");
            newCell.InnerText = item.FieldName;
            newRow.Cells.Add(newCell);

            newCell = new HtmlTableCell();
            newCell.Attributes.Add("class", "firstTitle");
            newCell.Attributes.Add("onfocus", "AliasFocus(this)");
            newCell.InnerText = item.FirstTitle;
            newRow.Cells.Add(newCell);

            newCell = new HtmlTableCell();
            newCell.Attributes.Add("class", "secondTitle");
            newCell.Attributes.Add("onfocus", "AliasFocus(this)");
            newCell.InnerText = item.SecondTitle;
            newRow.Cells.Add(newCell);

            newCell = new HtmlTableCell();
            newCell.Attributes.Add("class", "thirdTitle");
            newCell.Attributes.Add("onfocus", "AliasFocus(this)");
            newCell.InnerText = item.ThirdTitle;
            newRow.Cells.Add(newCell);

            newCell = new HtmlTableCell();
            newCell.Attributes.Add("class", "order");
            newCell.Attributes.Add("onfocus", "OrderFocus(this)");
            newCell.InnerText = item.Order;
            newRow.Cells.Add(newCell);

            newCell = new HtmlTableCell();
            newCell.Attributes.Add("class", "orderseq");
            newCell.Attributes.Add("onfocus", "OrderSeqFocus(this)");
            newCell.InnerText = item.OrderSeq == 0 ? "" : item.OrderSeq.ToString();
            newRow.Cells.Add(newCell);

            newCell = new HtmlTableCell();
            newCell.Attributes.Add("class", "group");
            newCell.Attributes.Add("onfocus", "GroupFocus(this)");
            newCell.InnerText = item.Group;
            newRow.Cells.Add(newCell);

            newCell = new HtmlTableCell();
            newCell.Attributes.Add("class", "condition");
            newCell.Attributes.Add("onfocus", "ConditionFocus(this)");
            newCell.InnerText = item.Condition;
            newRow.Cells.Add(newCell);

            newCell = new HtmlTableCell();
            newCell.Attributes.Add("class", "control");
            newCell.InnerHtml = "<input type=\"button\" value=\"+\" onclick=\"AddNewRow()\"/><input type=\"button\" value=\"-\" onclick=\"DelRow(this)\" />";
            newRow.Cells.Add(newCell);

            tblConfig.Rows.Insert(tblConfig.Rows.Count - 1, newRow);
        }
    }
    private string GetColumnSelectByTable(DataTable table)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<select id='column_select'>");
        foreach (DataRow dr in table.Rows)
        {
            sb.AppendFormat("<option value='{0}'>{1}</option>", dr["FieldName"].ToString(), dr["FieldAlias"].ToString());
        }
        sb.Append("</select>");
        return sb.ToString();
    }

    #region ICallbackEventHandler 成员
    private string sendClientData = "";
    public string GetCallbackResult()
    {
        return sendClientData;
    }

    public void RaiseCallbackEvent(string eventArgument)
    {
        string[] args = eventArgument.Split('※');

        switch (args[0])
        {
            case "drop":
                DataTable dt = ReportConfigDAL.GetTableColumnByTable(args[1]);
                sendClientData = "drop※" + GetColumnSelectByTable(dt); ;
                break;
            case "preview":
                ReportEntity report = new ReportEntity();
                report.TableName = args[1];

                // 插入新的item数据
                string[] reportStrArr = Server.UrlDecode(args[2]).Split('@');
                IList<ReportItem> items = new List<ReportItem>();
                ReportItem reportItem = null;

                StringBuilder first = new StringBuilder();
                StringBuilder second = new StringBuilder();
                StringBuilder third = new StringBuilder();
                string f = "";
                string s = "";
                string t = "";

                foreach (string item in reportStrArr)
                {
                    reportItem = new ReportItem();
                    string[] arr = item.Split('#');
                    reportItem.IsShowField = bool.Parse(arr[0]);
                    reportItem.Field = arr[1];
                    reportItem.FirstTitle = arr[2].Trim();
                    reportItem.SecondTitle = arr[3].Trim();
                    reportItem.ThirdTitle = arr[4].Trim();
                    reportItem.FieldName = GetFieldNameByItem(reportItem, arr[9]);
                    reportItem.Order = arr[5].Trim();
                    reportItem.OrderSeq = (arr[6].Trim().Length == 0) ? null : (int?)int.Parse(arr[6]);
                    reportItem.Condition = arr[7].Trim();
                    reportItem.Group = arr[8].Trim();
                    items.Add(reportItem);

                    if (f != reportItem.FirstTitle)
                    {
                        first.AppendFormat("|{0}", reportItem.FirstTitle);
                        if (reportItem.SecondTitle.Length > 0)
                            second.AppendFormat("|{0}", reportItem.SecondTitle);
                        if (reportItem.ThirdTitle.Length > 0)
                            third.AppendFormat("|{0}", reportItem.ThirdTitle);
                    }
                    else if (s != reportItem.SecondTitle)
                    {
                        second.AppendFormat(",{0}", reportItem.SecondTitle);
                        if (reportItem.ThirdTitle.Length > 0)
                            third.AppendFormat(",{0}", reportItem.ThirdTitle);
                    }
                    else if (reportItem.ThirdTitle.Length > 0)
                    {
                        third.AppendFormat(";{0}", reportItem.ThirdTitle);
                    }
                    f = reportItem.FirstTitle;
                    s = reportItem.SecondTitle;
                    t = reportItem.ThirdTitle;

                }

                if (first.Length > 0)
                    first.Remove(0, 1);
                if (second.Length > 0)
                    second.Remove(0, 1);
                if (third.Length > 0)
                    third.Remove(0, 1);

                report.FirstTitles = first.ToString();
                report.SecondTitles = second.ToString();
                report.ThirdTitles = third.ToString();
                string sqlData = ReportUtils.CombineSql(report, items);
                
                DataSet ds = ReportDAL.ExecuteReportSql(sqlData);
                StringBuilder sb = new StringBuilder("preview※");
                int columnLength = ds.Tables[0].Columns.Count;

                for (int i = 0; i < columnLength; i++)
                {
                    sb.AppendFormat("{0},", ds.Tables[0].Columns[i].ColumnName);
                }
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                sb.Append("|");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    for (int i = 0; i < columnLength; i++)
                    {
                        sb.AppendFormat("{0},", dr[i].ToString());
                    }
                    if (sb.Length > 0)
                        sb.Remove(sb.Length - 1, 1);
                    sb.Append("|");
                }
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                sendClientData = sb.ToString();
                break;
            default:
                break;
        }

    }

    #endregion

    #region 保存数据到数据库
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string savedataStr = hidReportItem.Value;

        if (!ReportUtils.ValideSafeSql(savedataStr))
        {
            Response.Write("<script>alert('配置语句中包含危险关键字，提交被拒绝。');</script>");
            return;
        }

        // 插入report数据
        ReportEntity report = new ReportEntity();
        report.Id = ReportID;
        report.Name = txtReportName.Text.Trim();
        report.Remark = txtRemark.Text.Trim();
        report.TableName = ddlSource.SelectedValue;
        report.Creator = CurrentUserName;
        report.SourceType = 1;
        int reportId = ReportDAL.SaveReport(report);

        // 清空原有的item数据
        ReportDAL.DeleteReportItemsByReportID(ReportID);

        // 插入新的item数据
        string[] reportStrArr = savedataStr.Split('@');
        IList<ReportItem> items = new List<ReportItem>();
        ReportItem reportItem = null;

        StringBuilder first = new StringBuilder();
        StringBuilder second = new StringBuilder();
        StringBuilder third = new StringBuilder();
        string f = "";
        string s = "";
        string t = "";

        foreach (string item in reportStrArr)
        {
            reportItem = new ReportItem();
            string[] arr = item.Split('#');
            reportItem.IsShowField = bool.Parse(arr[0]);
            reportItem.Field = arr[1];
            reportItem.FirstTitle = arr[2].Trim();
            reportItem.SecondTitle = arr[3].Trim();
            reportItem.ThirdTitle = arr[4].Trim();
            reportItem.FieldName = GetFieldNameByItem(reportItem, arr[9]);
            reportItem.Order = arr[5].Trim();
            reportItem.OrderSeq = (arr[6].Trim().Length == 0) ? null : (int?)int.Parse(arr[6]);
            reportItem.Condition = arr[7].Trim();
            reportItem.Group = arr[8].Trim();
            reportItem.ReportID = reportId;
            items.Add(reportItem);
            ReportDAL.SaveReportItem(reportItem);

            if (f != reportItem.FirstTitle)
            {
                first.AppendFormat("|{0}", reportItem.FirstTitle);
                if (reportItem.SecondTitle.Length > 0)
                    second.AppendFormat("|{0}", reportItem.SecondTitle);
                if (reportItem.ThirdTitle.Length > 0)
                    third.AppendFormat("|{0}", reportItem.ThirdTitle);
            }
            else if (s != reportItem.SecondTitle)
            {
                second.AppendFormat(",{0}", reportItem.SecondTitle);
                if (reportItem.ThirdTitle.Length > 0)
                    third.AppendFormat(",{0}", reportItem.ThirdTitle);
            }
            else if (reportItem.ThirdTitle.Length > 0)
            {
                third.AppendFormat(";{0}", reportItem.ThirdTitle);
            }
            f = reportItem.FirstTitle;
            s = reportItem.SecondTitle;
            t = reportItem.ThirdTitle;

        }

        if (first.Length > 0)
            first.Remove(0, 1);
        if (second.Length > 0)
            second.Remove(0, 1);
        if (third.Length > 0)
            third.Remove(0, 1);

        report.FirstTitles = first.ToString();
        report.SecondTitles = second.ToString();
        report.ThirdTitles = third.ToString();
        report.ReportSql = ReportUtils.CombineSql(report, items);
        report.Id = reportId;

        ReportDAL.UpdateReportSql(report);

        Response.Write("<script>alert('提交成功');window.location.href = 'ReportList.aspx';</script>");
    }

    private string GetFieldNameByItem(ReportItem item, string fileName)
    {
        if (item.ThirdTitle.Length > 0)
            return item.ThirdTitle;
        if (item.SecondTitle.Length > 0)
            return item.SecondTitle;
        if (item.FirstTitle.Length > 0)
            return item.FirstTitle;
        return fileName;
    }
    #endregion
}
