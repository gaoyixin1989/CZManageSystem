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

public partial class test_TestDesigner : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnDeploy_Click(object sender, EventArgs e)
    {
        string key = txtKey.Text.Trim();
        string data = txtData.Text.Trim();

        Test.WorkflowDesignerService wds = new Test.WorkflowDesignerService();
        Response.Write(HttpUtility.HtmlEncode(wds.SaveWorkflowXml(key, data)));
    }
}
