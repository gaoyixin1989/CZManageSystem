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

public partial class apps_xqp2_pages_workflows_designer_Designer : System.Web.UI.Page
{
    protected static string RedirectUrl = Botwave.Web.WebUtils.GetAppPath() + "apps/xqp2/pages/workflows/config/ConfigWorkflow.aspx?wid=";

    public string WorkflowKey
    {
        get { return (string)ViewState["WorkflowKey"]; }
        set { ViewState["WorkflowKey"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.WorkflowKey = Guid.Empty.ToString();
            if (Request.QueryString["wid"] != null)
            {
                this.WorkflowKey = Server.HtmlEncode(this.Request.QueryString["wid"]);
            }
            else
            {
                if (Request.QueryString["m"] == null)
                {
                    Response.Write("参数错误！");
                    //Response.Redirect("../../workflows/workflowDeploy.aspx");
                }
            }
        }
    }
}
