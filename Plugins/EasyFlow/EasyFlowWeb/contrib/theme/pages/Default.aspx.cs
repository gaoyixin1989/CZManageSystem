using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Web;
using Botwave.Web.Themes;

public partial class contrib_theme_pages_Default : Botwave.Web.PageBase
{
    private string redirectPage = WebUtils.GetAppPath() + Botwave.GlobalSettings.Instance.DefaultPage;

    public string RedirectPage
    {
        set { redirectPage = value; }
    }

    public string ThemeName
    {
        get { return (string)ViewState["ThemeName"]; }
        set { ViewState["ThemeName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string themeName = WebUtils.GetThemeName();
            if (string.IsNullOrEmpty(themeName))
                themeName = ThemeContext.DefaultTheme;

            this.ThemeName = themeName;
            this.BindThemes(themeName);
        }
    }

    protected void btnSetTheme_Click(object sender, EventArgs e)
    {
        string themeName = this.hiddenTheme.Value;
        if(string.IsNullOrEmpty(themeName))
            themeName = ThemeContext.DefaultTheme;

        WebUtils.SetThemeName(themeName);
        Response.Redirect(redirectPage);
    }

    private void BindThemes(string themeName)
    {
        this.listThemes.RepeatColumns = ThemeContext.RepeatColumns;
        this.listThemes.DataSource = ThemeContext.Themes.Values;
        this.listThemes.DataBind();
    }
}
