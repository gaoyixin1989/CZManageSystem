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

public partial class ReportList : Botwave.Security.Web.PageBase
{
    private string advanceResource = "A019";

    /// <summary>
    /// 高级权限资源.
    /// </summary>
    public string AdvanceResource
    {
        get { return advanceResource; }
        set { advanceResource = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        Botwave.Security.LoginUser user = CurrentUser;
        string creator = user.UserName;

        IList<ReportEntity> reports = HasAdvanceReport(user) ? ReportDAL.GetReportList() : Botwave.XQP.Commons.XQPHelper.GetReportList(creator);
        GridView1.DataSource = reports;
        GridView1.DataBind();
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int reportId = (int)((GridView)sender).DataKeys[e.RowIndex].Value;
        if (ReportDAL.DeleteReportByID(reportId) > 0)
            Page.RegisterClientScriptBlock(Guid.NewGuid().ToString(), "<script>alert('删除成功');</script>");

        BindData();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((LinkButton)e.Row.Cells[8].Controls[0]).Attributes.Add("onclick", "javascript:return confirm('你真的要删除吗？');");
        }
    }

    protected bool HasAdvanceReport(Botwave.Security.LoginUser user)
    {
        if (user == null)
            return false;
        if (string.IsNullOrEmpty(this.advanceResource))
            return true;

        bool result = false;
        if (user.Properties.ContainsKey("Report_Advance_Enable"))
        {
            result = Convert.ToBoolean(user.Properties["Report_Advance_Enable"]);
        }
        else
        {
            result = user.Resources.ContainsKey(this.advanceResource);
            user.Properties["Report_Advance_Enable"] = result;
        }
        return result;
    }
}
