using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Workflow.Extension.Service;

public partial class apps_xqp2_pages_extension_Default : Botwave.Security.Web.PageBase
{
    private static readonly string defaultContentUrl = AppPath+ "contrib/workflow/pages/default.aspx";
    public string ContentUrl = defaultContentUrl;

    private IWorkflowNoticeService workflowNoticeService = (IWorkflowNoticeService)Ctx.GetObject("workflowNoticeService");
    public IWorkflowNoticeService WorkflowNoticeService
    {
        get { return workflowNoticeService; }
        set { workflowNoticeService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (CurrentUser == null)
                Response.Redirect(AppPath + "contrib/security/pages/login.aspx");

            this.Title = string.Format("{0} - 工作台", AppName);
            string url = Request.QueryString["url"];
            if (string.IsNullOrEmpty(url))
            {
                url = defaultContentUrl;
            }
            this.ContentUrl = url;
            this.PopupNotice();
        }
    }
        
    /// <summary>
    /// 弹出公告窗口
    /// </summary>
    protected void PopupNotice()
    {
        if (workflowNoticeService != null)
            workflowNoticeService.PopupWorkflowNotices(this, Guid.Empty.ToString(), true);
    }
}
