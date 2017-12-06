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

public partial class xqp2_contrib_cms_pages_notice_PopupNotice : Botwave.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            noticeView1.PreRenderNotice += new  apps_xqp2_pages_notices_controls_NoticeView.PreRenderNoticeHandler(OnPreRenderNotice);
            noticeView1.EntityContentVisible = true;
            noticeView1.EntityContentHeaderText = "所属流程";
        }
    }

    protected void OnPreRenderNotice(Botwave.XQP.Domain.Notice sender)
    {
        if (sender.EntityId == Guid.Empty.ToString())
            sender.EntityId = "全部";
    }
}
