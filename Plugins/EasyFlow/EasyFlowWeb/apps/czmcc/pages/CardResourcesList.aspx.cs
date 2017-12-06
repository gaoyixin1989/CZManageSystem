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

using Botwave.Workflow.Practices.CZMCC.Service.Impl;

public partial class apps_czmcc_pages_CardResourcesList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
        }
    }

    private void LoadData()
    {
        ResourcesExecutionService re = new ResourcesExecutionService();
        DataTable dt = re.GetResourcesByTypeNew(2);

        rptCardList.DataSource = dt;
        rptCardList.DataBind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int cCount = 0;
        bool IsFalg = false;
        string rModel = "";
        string sNum = "";
        string strText;
        string rId = "";
        string checkedValues = Request.Form["chkbox_resources"];

        foreach (Control c in rptCardList.Controls)
        {
            HiddenField hidID = (HiddenField)c.FindControl("hidID");
            Label lbModel = (Label)c.FindControl("lbModel");
            Label lbNumber = (Label)c.FindControl("lbNumber");
            string hiddenValue = hidID.Value.Trim();
            if (hiddenValue.Equals(checkedValues, StringComparison.CurrentCultureIgnoreCase))
            {
                cCount++;
                rModel = lbModel.Text.Trim();
                sNum = lbNumber.Text.Trim();
                rId = hiddenValue;
                IsFalg = true;
            }
        }
        strText = rModel + "/" + sNum;
        if (IsFalg == false)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "", "<script language=javascript>alert('请选择电脑型号!');</script>");
            return;
        }
        if (cCount > 1)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "", "<script language=javascript>alert('只能请选择一种电脑型号!');</script>");
            return;
        }
        Response.Write("<script>window.opener.document.getElementById('bwdf_txt_F3').value='" + strText + "' </script>");
        Response.Write("<script>window.opener.document.getElementById('bwdf_txt_F8').value='" + rId + "';window.close(); </script>");
    }
}
