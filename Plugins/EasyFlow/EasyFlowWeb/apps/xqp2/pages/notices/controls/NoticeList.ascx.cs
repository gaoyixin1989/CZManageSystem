using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;

public partial class apps_xqp2_pages_notices_controls_NoticeList : Botwave.Security.Web.UserControlBase//  System.Web.UI.UserControl
{
    private INoticeService noticeService = (INoticeService)Ctx.GetObject("noticeService");

    public INoticeService NoticeService
    {
        get { return noticeService; }
        set { noticeService = value; }
    }

    public string UserName
    {
        get { return (string)ViewState["UserName"]; }
        set { ViewState["UserName"] = value; }
    }

    public string EntityType
    {
        get { return (string)ViewState["EntityType"]; }
        set { ViewState["EntityType"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Search(0, 0);
        }
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.Search(listPager.TotalRecordCount, e.NewPageIndex + 1);
    }

    private void Search(int recordCount, int pageIndex)
    {
        string userName = this.UserName;
        string entityType = this.EntityType;
        bool? enabled = null;
        string enabledValue = Request.QueryString["enabled"];
        if (!string.IsNullOrEmpty(enabledValue))
            enabled = (enabledValue == "true" ? true : false);

        DataTable results = noticeService.GetNotices(userName, entityType, enabled, pageIndex, listPager.ItemsPerPage, ref recordCount);

        this.rptNotices.DataSource = results;
        this.rptNotices.DataBind();
        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
    }

    public static string FormatEnabled(object enabled)
    {
        if (enabled == null || enabled == DBNull.Value)
            return "未启用";
        return (enabled.ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? "已启用" : "未启用");
    }
}
