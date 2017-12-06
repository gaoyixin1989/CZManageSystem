using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class xqp2_contrib_cms_pages_notice_Notices : Botwave.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.noticeList1.EntityType = Botwave.XQP.Commons.XQPHelper.EntityType_WorkflowNotice;
        }
    }
}
