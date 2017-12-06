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


public partial class contrib_report_pages_TemplateConfig : Botwave.Security.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TemplateConfig1.ReportID = Botwave.Commons.DbUtils.ToInt32(Request["id"], 0);
    }
}
