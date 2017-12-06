using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;

public partial class apps_xqp2_app_List : Botwave.Web.PageBase
{
    private readonly string appAccessVirtualDomain = "{%AppDomain%}";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadData();
    }

    protected void LoadData()
    {
        rpAppsList.DataSource = Apps.Select();
        rpAppsList.DataBind();
    }

    protected void rpAppsList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            if (e.CommandArgument != null)
            {
                int appId = Int32.Parse(e.CommandArgument.ToString());
                Apps app = Apps.LoadById(appId);
                app.Delete();
            }
        }
        LoadData();
    }

    public string AppDoaminReplace(string appAccessUrl)
    {
        return appAccessUrl.Replace(appAccessVirtualDomain, Botwave.GlobalSettings.Instance.Address);
    }
}
