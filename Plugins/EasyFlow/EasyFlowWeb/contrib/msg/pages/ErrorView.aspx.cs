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
using Botwave.Web.Controls;
using Botwave.Commons;

public partial class contrib_exceptionLogger_ErrorView : Botwave.Security.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dgLogs.Attributes["style"] = "word-break:break-all";
        }
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool isNo = ddlType.SelectedValue.Equals("0");
        tdNo.Visible = isNo;
        tdDT.Visible = !isNo;
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Search(0);
    }
    protected void listPager_PageIndexChanged(object sender, PageChangedEventArgs e)
    {
        int pageIndex = e.NewPageIndex;
        pageIndex = pageIndex + 1;
        Search(pageIndex);
    }
    private void Search(int pageIndex)
    {
        if (ddlType.SelectedValue.Equals("0") && txtExceptionID.Text.Trim().Length == 0)
        {
            Response.Write("<script>alert('请输入出错编号');</script>");
            return;
        }
        if (ddlType.SelectedValue.Equals("1") && (dtpBegin.Text.Trim().Length == 0 || dtpEnd.Text.Trim().Length == 0))
        {
            Response.Write("<script>alert('请输入开始时间和结束时间');</script>");
            return;
        }
        try
        {
            string where = GetSearchCondition();
            int recordCount;
            DataSet ds = Botwave.Commons.ExceptionLogger.GetExceptionDataByPage(pageIndex, listPager.ItemsPerPage, out recordCount, where);
            dgLogs.DataSource = ds;
            dgLogs.DataBind();
            listPager.TotalRecordCount = recordCount;
            listPager.DataBind();
            ltlTotalRecordCount.Text = recordCount.ToString();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    private string GetSearchCondition()
    {
        string where = "";
        if (ddlType.SelectedValue.Equals("0"))
        {
            where = "ID=" + txtExceptionID.Text;
        }
        else
        {
            where = string.Format("ExceptionTime between '{0}' and '{1}'", dtpBegin.Text, dtpEnd.Text);
        }
        return where;
    }
    protected void btnExport2File_Click(object sender, EventArgs e)
    {
        string where = " where " + GetSearchCondition();
        DataSet ds = Botwave.Commons.ExceptionLogger.ExecSqlScriptForDataSet(where);
        DataGrid dg = new DataGrid();
        dg.DataSource = ds;
        dg.DataBind();

        ExcelUtils.Export2Excel(dg, "报错数据_" + DateTime.Today.ToString("yyyyMMdd"));
    }
    protected void dgLogs_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemIndex > -1)
        {
            string st = DataBinder.Eval(e.Item.DataItem, "StackTrace").ToString();
            Label lbl = (Label)e.Item.FindControl("lblStackTrace");
            if (st.Length > 30)
            {
                lbl.Text = st.Substring(0, 30) + "...";
                lbl.ToolTip = st;
            }
            else
                lbl.Text = st;
        }
    }
}
