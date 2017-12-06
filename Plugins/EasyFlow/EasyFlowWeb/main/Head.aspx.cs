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
using Botwave.Security;

public partial class main_Head : Botwave.Web.PageBase
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
