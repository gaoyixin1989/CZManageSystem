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

public partial class main_Default : Botwave.Web.PageBase
{
    private static readonly string defaultContentUrl = "Content.aspx";

    public string ContentUrl = defaultContentUrl;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string url = Request.QueryString["url"];
            if (string.IsNullOrEmpty(url))
            {
                url = defaultContentUrl;
            }
            this.ContentUrl = url;
        }
    }
}
