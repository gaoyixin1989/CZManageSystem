using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using Botwave.Web;

public partial class masters_NewPage : Botwave.Web.MasterPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        System.Web.UI.HtmlControls.HtmlGenericControl child = new System.Web.UI.HtmlControls.HtmlGenericControl("link");
        Page handler = (Page)HttpContext.Current.Handler;
        handler.Header.Controls.Clear();//清理

        child.Attributes.Add("href", AppPath + "App_Themes/new/Style.css");
        child.Attributes.Add("rel", "stylesheet");
        child.Attributes.Add("type", "text/css");
        handler.Header.Controls.Add(child);
        //child = new System.Web.UI.HtmlControls.HtmlGenericControl("link");
        //child.Attributes.Add("href", AppPath + "App_Themes/new/Tabs.css");
        //child.Attributes.Add("rel", "stylesheet");
        //child.Attributes.Add("type", "text/css");
        //handler.Header.Controls.Add(child);
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/common.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery-latest.pack.js");
    }
}
