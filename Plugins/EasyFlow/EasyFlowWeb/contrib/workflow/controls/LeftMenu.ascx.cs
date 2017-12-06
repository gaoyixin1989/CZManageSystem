using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class contrib_workflow_controls_LeftMenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        Botwave.Security.LoginUser user = Botwave.Security.LoginHelper.User;
        if (user != null)
        {
            this.lmv.UserResources = user.Resources;
        }
    }
}
