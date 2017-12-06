using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Security;

public partial class apps_gmcc_Head : Botwave.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            LoginUser user = LoginHelper.User;
            if (user != null)
            {
                ltlGreetingText.Text = user.RealName;
            }
        }
    }
}
