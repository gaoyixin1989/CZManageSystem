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

public partial class main_Left : Botwave.Web.PageBase
{
    private IList<string> menuControls;

    public IList<string> MenuControls
    {
        set { menuControls = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadMenuControls();
        }
    }

    private void LoadMenuControls()
    {
        if (null != menuControls)
        {
            foreach (string path in menuControls)
            {
                Control ctl = this.LoadControl(path);
                this.phMenu.Controls.Add(ctl);
            }
        }        
    }
}
