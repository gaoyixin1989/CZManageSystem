using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Botwave.XQP.Domain;
using Botwave.Commons;

public partial class apps_xqp2_pages_help_EditHelp : Botwave.Security.Web.PageBase
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
        IList<Help> listHelp = Help.Select();
        foreach (Help item in listHelp)
        {
            ddlHelps.Items.Add(new ListItem(item.Title, item.Id.ToString()));
        }

        ddlHelps.Items.Insert(0, new ListItem("", "0"));

        //LOADDATA WHEN EDIT HELP
        Help helpEntity = Help.LoadById(HelpId);
        if (null == helpEntity)
            return;

        ltlTitle.Text = "编辑";
        txtTitle.Text = helpEntity.Title;
        FCKContent.Value = helpEntity.Content;
        ddlHelps.SelectedValue = helpEntity.ParentId.ToString();
        txtShowOrder.Text = helpEntity.ShowOrder.ToString();
    }    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Help helpEntity = new Help();
        helpEntity.Title = txtTitle.Text.Trim();
        helpEntity.Content = FCKContent.Value.ToString();
        helpEntity.ParentId = DbUtils.ToInt32(ddlHelps.SelectedValue.Trim());
        helpEntity.ShowOrder = (string.IsNullOrEmpty(txtShowOrder.Text) ? 1 : DbUtils.ToInt32(txtShowOrder.Text.Trim()));

        if (HelpId.Equals(0))
        {
            helpEntity.Create();
            ShowSuccess("新增帮助成功", returnURL);
        }
        else
        {
            helpEntity.Id = HelpId;
            helpEntity.Update();
            ShowSuccess("修改帮助成功", returnURL);
        }
    }
}
