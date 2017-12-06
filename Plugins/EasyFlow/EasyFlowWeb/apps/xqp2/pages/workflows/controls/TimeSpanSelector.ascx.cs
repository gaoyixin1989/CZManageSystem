using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class apps_xqp2_pages_workflows_controls_TimeSpanSelector : System.Web.UI.UserControl
{
    public int Hours
    {
        get { return Convert.ToInt32(this.ddlSelectHours.SelectedValue); }
        set { this.ddlSelectHours.SelectedIndex = value; }
    }

    public int Minutes
    {
        get { return Convert.ToInt32(this.ddlSelectMinutes.SelectedValue); }
        set { this.ddlSelectMinutes.SelectedIndex = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public int GetHours()
    {
        return Convert.ToInt32(this.ddlSelectHours.SelectedValue);
    }

    public int GetMinutes()
    {
        return Convert.ToInt32(this.ddlSelectMinutes.SelectedValue);
    }
}
