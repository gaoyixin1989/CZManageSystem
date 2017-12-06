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
using Botwave.Workflow.Practices.CZMCC.Service.Impl;

public partial class apps_czmcc_pages_BorrowCardWorkFlow : Botwave.Web.PageBase
{
    protected string keywords;
    private Guid _resourId;
    public Guid ResourId
    {
        get { return _resourId; }
        set { _resourId = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string reid = Request.QueryString["reId"];
        if (string.IsNullOrEmpty(reid))
            ShowError(Botwave.Web.MessageHelper.Message_ArgumentException);

        Guid resourceId = new Guid(reid);
        this.ResourId = resourceId;
        if (!IsPostBack)
        {
            LoadData(resourceId);
        }
    }

    private void  LoadData(Guid resourceId)
    { 
        ResourcesExecutionService re = new ResourcesExecutionService();

        DataRow row = re.GetResourceInfo(resourceId);
        if (row == null)
            ShowError(Botwave.Web.MessageHelper.Message_ArgumentException);
        this.ltlEDGENumber.Text = Botwave.Commons.DbUtils.ToString(row["ResourcesModel"]);
        this.ltlSIMNumber.Text = Botwave.Commons.DbUtils.ToString(row["SerialNumber"]);
        this.ltlEDGEStatus.Text = (Botwave.Commons.DbUtils.ToInt32(row["State"]) == 1 ? "已申请" : "未申请");

        DataTable result = re.GetCurrentBorrowInfo(resourceId);
        if(result.Rows.Count > 0)
        {
            row = result.Rows[0];
            this.ltlResourceInfo.Text = BindOperator(DbUtils.ToString(row["CreatorName"]), DbUtils.ToString(row["DpFullName"]));
        }
        this.Search(0, 0, re);
    }

    private string BindOperator(string actorName, string deptName)
    {
        if (string.IsNullOrEmpty(actorName))
            return string.Empty;

        string actorText = string.Format("<th style=\"width:13%\">当前借用人：</th><td style=\"width:37%;\">{0}</td>", actorName);
        string deptText = string.IsNullOrEmpty(deptName) ? "<th style=\"width:13%;\"></th><td style=\"width:37%;\"></td>" : string.Format("<th style=\"width:13%\">所属部门：</th><td style=\"width:37%;\">{0}</td>", deptName);
        return string.Format("<tr>{0}\r\n{1}</tr>", actorText, deptText);
    }

    private void Search(int recordCount, int pageIndex)
    {
        Search(recordCount, pageIndex, new ResourcesExecutionService());
    }

    private void Search(int recordCount, int pageIndex, ResourcesExecutionService re)
    {
        if (re == null)
            re = new ResourcesExecutionService();
        keywords = string.Empty;

        string workFlowName = txtWorkFlowName.Text.Trim();

        if (!string.IsNullOrEmpty(workFlowName))
        {
            keywords += string.Format(" AND (Title like '%{0}%')", workFlowName);
        }

        DataTable dt = re.GetBorrowWorkFlow(ResourId, keywords, pageIndex, listPager.ItemsPerPage, ref recordCount);

        gvBorrowList.DataSource = dt;
        gvBorrowList.DataBind();
        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.Search(listPager.TotalRecordCount, e.NewPageIndex);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search(0, 0);
    }
}
