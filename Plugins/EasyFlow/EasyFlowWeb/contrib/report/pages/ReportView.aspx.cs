using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Botwave.Report.Common;
using Botwave.Report.DataAccess;
using Botwave.Report.DrawGrapBase;
using ZedGraph;
using Botwave.Commons;


public partial class contrib_report_ReportView : Botwave.Security.Web.PageBase
{
    /// <summary>
    /// 报表ID
    /// </summary>
    protected int ReportID
    {
        get
        {
            int id;
            int.TryParse(Request.QueryString["id"], out id);
            return id;
        }
    }
    protected string SqlWhere
    {
        get
        {
            string wt = DbUtils.ToString(Request.QueryString["where"]);
            wt.Replace(":", "=");
            return wt;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
