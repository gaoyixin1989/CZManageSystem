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

public partial class contrib_msg_WapSuccess : Botwave.XQP.Web.Security.PageBase
{
    private string defaultMessage = "操作成功！";

    public string DefaultMessage
    {
        set { defaultMessage = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string successMessage = CurrentMessage;
            if (successMessage.Length == 0)
            {
                successMessage = defaultMessage;
            }
            this.ltlMessage.Text = successMessage;

            string returnUrl = Request.QueryString["returnUrl"];    //returnUrl.Replace("*", "&")
            if (string.IsNullOrEmpty(returnUrl) && null != Request.UrlReferrer)
            {
                returnUrl = Request.UrlReferrer.PathAndQuery;
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                this.btnBack.Attributes["onclick"] = string.Format("return redirectUrl('{0}')", returnUrl);
            }
            else
            {
                this.btnBack.Attributes["onclick"] = "history.go(-1);";
            }


            CurrentMessage = string.Empty;
        }
    }
}
