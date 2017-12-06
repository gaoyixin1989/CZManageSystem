using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.XQP.Domain;
using Botwave.Workflow.Extension.Util;

public partial class apps_czmcc_pages_WorkflowAttentions : Botwave.Web.PageBase
{
    public string AttentionType
    {
        get { return ViewState["AttentionType"] == null ? null : (string)ViewState["AttentionType"]; }
        set { ViewState["AttentionType"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.AttentionType = Request.QueryString["type"];
            if (this.AttentionType == null)
                this.AttentionType = "0,1";
            Search(0, 0);
        }
    }

    protected string IsChecked(int type)
    {
        string value = this.AttentionType;
        if (value == null || value.IndexOf(type.ToString()) > -1)
            return " checked=\"checked\"";
        return "";
    }

    protected void Search(int recordCount, int pageIndex)
    {
        string keywords = Request.QueryString["key"];
        if (!IsPostBack)
        {
            txtKeywords.Text = keywords;
        }
        string type = Request.QueryString["type"];

        DataTable source = CZWorkflowAttention.GetView(Botwave.Security.LoginHelper.UserName, type, keywords, pageIndex, listPager.ItemsPerPage, ref recordCount);
        rptList.DataSource = source;
        rptList.DataBind();

        this.listPager.TotalRecordCount = recordCount;
        this.listPager.DataBind();
    }

    protected string FormatWorkflowActor(object value)
    {
        if (value == null || value == DBNull.Value)
            return string.Empty;
        return WorkflowUtility.FormatWorkflowActor(value.ToString());
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        Search(listPager.TotalRecordCount, e.NewPageIndex);
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = e.Item.DataItem as DataRowView;
        int type = DbUtils.ToInt32(row["Type"]);
        if (type == 1)
            return;

        // 待办的过期处理.
        DateTime? expectFinishedTime = DbUtils.ToDateTime(row["ExpectFinishedTime"]);
        if (!expectFinishedTime.HasValue)
            return;
        if (expectFinishedTime <= DateTime.Now)
        {
            HtmlTableRow listRow = e.Item.FindControl("listRow") as HtmlTableRow;
            if (listRow != null)
            {
                string cssClass = listRow.Attributes["class"];
                listRow.Attributes["class"] = (string.IsNullOrEmpty(cssClass) ? string.Empty : cssClass + " ") + "expired";
            }
        }
    }

    protected void buttonQuery_Click(object sender, EventArgs e)
    {
        string key = HttpUtility.UrlEncode(this.txtKeywords.Text.Trim());
        string type = Request["chkDataTypes"];

        Response.Redirect(string.Format("WorkflowAttentions.aspx?key={0}&type={1}", key, type));
    }

    protected void btnRecycle_Click(object sender, EventArgs e)
    {
        string ids = Request["chkboxRptList"];

        CZWorkflowAttention.Delete(ids == null ? null : ids.Split(','));

        Response.Redirect(Request.Url.ToString());
    }
}
