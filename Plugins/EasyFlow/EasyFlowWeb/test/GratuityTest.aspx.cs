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
using Botwave.Workflow.Practices.CZMCC;

public partial class test_GratuityTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtURL.Text = GetServiceUrl();
        }
    }
    protected void btnTest_Click(object sender, EventArgs e)
    {
        string url = this.txtURL.Text.Trim();
        if (string.IsNullOrEmpty(url))
            url = GetServiceUrl();
        Test.GratuityFlowService tg = new Test.GratuityFlowService(url);

        string creator = this.txtCreator.Text.Trim();
        string actor = this.txtActor.Text.Trim();
        string title = this.txtTitle.Text;
        string content = this.txtcontent.Text;
        content = content.Replace("\r\n", "<br>");

        actor = string.IsNullOrEmpty(actor) ? "admin" : actor;
        tg.SendGratuityFlow(title, creator, creator, actor, "", "", 1, content);
    }

    protected void btn_Query1_Click(object sender, EventArgs e)
    {
        string url = this.txtURL.Text.Trim();
        if (string.IsNullOrEmpty(url))
            url = GetServiceUrl();
        Test.GratuityFlowService tg = new Test.GratuityFlowService(url);

        string employeeID = this.txtEmployeeID.Text.Trim();
        DataSet resultSet = tg.ApplyListDs(employeeID);
        foreach (DataTable table in resultSet.Tables)
        {
            GridView view = new GridView();
            view.Width = new Unit(1, UnitType.Percentage);
            view.DataSource = table;
            view.DataBind();
            result1.Controls.Add(view);
        }
    }

    protected void btn_Query2_Click(object sender, EventArgs e)
    {
        string url = this.txtURL.Text.Trim();
        if (string.IsNullOrEmpty(url))
            url = GetServiceUrl();
        Test.GratuityFlowService tg = new Test.GratuityFlowService(url);

        string applyID = this.txtApplyID.Text.Trim();
        DataSet resultSet = tg.ApplyRowDs(applyID);
        foreach (DataTable table in resultSet.Tables)
        {
            GridView view = new GridView();
            view.Width = new Unit(1, UnitType.Percentage);
            view.DataSource = table;
            view.DataBind();
            result2.Controls.Add(view);
        }
        Response.Write(resultSet.Tables[0].Rows[0]["AppID"].ToString());
    }

    protected string GetServiceUrl()
    {
        string url = Request.Url.AbsoluteUri;
        string path = Request.Url.AbsolutePath;
        int index = url.IndexOf(path, 7, StringComparison.OrdinalIgnoreCase);
        if (index > -1)
        {
            url = url.Substring(0, index) + Botwave.Web.WebUtils.GetAppPath() + "apps/czmcc/ws/GratuityFlowService.asmx";
        }
        return url;
    }
}
