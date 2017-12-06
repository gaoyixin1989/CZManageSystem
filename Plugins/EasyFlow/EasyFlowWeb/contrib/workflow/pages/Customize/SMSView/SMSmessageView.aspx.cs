using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;

public partial class contrib_workflow_pages_Customize_SMSView_SMSmessageView : Botwave.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            DataBind(0, 0);
    }

    protected string SubString(object content)
    {
        if (content == null || content == DBNull.Value)
            return string.Empty;
        string value = content.ToString();
        return value.Length > 20 ? (value.Substring(0, 20) + "...") : value;
    }

    private void DataBind(int recordCount, int pageIndex)
    {
        DateTime? beginDate = DbUtils.ToDateTime(this.dtpBeginDT.Text.Trim());
        DateTime? endDate = DbUtils.ToDateTime(this.dtpEndDT.Text.Trim());
        string receiver = this.txtReceiver.Text.Trim();
        string sheetId = this.txtSheetID.Text.Trim();

        this.rptResult.DataSource = Botwave.XQP.Commons.SMSHelper.GetSMSView(beginDate, endDate, receiver, sheetId, pageIndex, listPager.ItemsPerPage, ref recordCount);

        //this.rptResult.DataSource = Botwave.XQP.Commons.SMSHelper.GetSMSViewByStore(beginDate, endDate, receiver, sheetId, pageIndex, listPager.ItemsPerPage, ref recordCount);
        this.rptResult.DataBind();

        this.listPager.TotalRecordCount = recordCount;
        this.listPager.DataBind();
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.DataBind(listPager.TotalRecordCount, e.NewPageIndex);
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.listPager.CurrentPageIndex = -1;
        DataBind(0, 0);
    }
}
