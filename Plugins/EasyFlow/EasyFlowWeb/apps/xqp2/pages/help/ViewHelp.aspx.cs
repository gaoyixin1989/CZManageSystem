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

using Botwave.XQP.Domain;

public partial class apps_xqp2_pages_help_ViewHelp : Botwave.Security.Web.PageBase
{
    private readonly string returnURL = string.Format("{0}apps/xqp2/pages/help/default.aspx", AppPath);
    public int HelpId
    {
        get
        {
            if (null != ViewState["HelpId"] &&
                !String.IsNullOrEmpty(ViewState["HelpId"].ToString()))
                return Botwave.Commons.DbUtils.ToInt32(ViewState["HelpId"]);
            return 0;
        }
        set { ViewState["HelpId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = Request["Id"];
            if (!String.IsNullOrEmpty(id))
                HelpId = Botwave.Commons.DbUtils.ToInt32(id);

            InitilizeData();
        }
    }

    private void InitilizeData()
    {
        Help helpEntity = Help.LoadById(HelpId);
        if (null == helpEntity)
            return;

        lblTitle.Text = helpEntity.Title;
        ltlContent.Text = helpEntity.Content;
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Help helpEntity = new Help();
        helpEntity.Id = HelpId;

        helpEntity.Delete();

        ShowSuccess("删除成功", returnURL);
    }
}
