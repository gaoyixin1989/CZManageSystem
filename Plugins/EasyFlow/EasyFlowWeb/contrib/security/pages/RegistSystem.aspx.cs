using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;

public partial class contrib_security_pages_RegistSystem : Botwave.Security.Web.PageBase
{
    public string ID {
        get { return (string)ViewState["ID"]; }
        set { ViewState["ID"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id=Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id))
            {
                ID = id;
                Button1.Text = "修改";
                Button2.Visible = true;
                LoadInfo(id);
            }
            this.LoadData();
        }
    }

    private void LoadInfo(string id)
    {
        CZRegistSystem item = CZRegistSystem.SelectById(new Guid(id));
        txtportal.Text = item.SystemName;
        txtName.Text = item.RealName;
        txtPhone.Text = item.Tel;
        txtEmail.Text = item.Email;
    }

    private void LoadData()
    {
        IList<CZRegistSystem> items = CZRegistSystem.Select();
        this.usersRepeater.DataSource = items;
        usersRepeater.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        CZRegistSystem item = new CZRegistSystem();
        item.SystemId = Guid.NewGuid();
        item.SystemName = txtportal.Text;
        item.RealName = txtName.Text ;
        if (txtpassword.Text == txtpassword2.Text)
            item.Password = Botwave.Commons.TripleDESHelper.Encrypt(txtpassword.Text);
        item.Email = txtEmail.Text;
        item.CreatedTime = DateTime.Now;
        item.LastModTime = DateTime.Now;
        item.Creator = CurrentUserName;
        item.LastModifier = CurrentUserName;
        item.Tel = txtPhone.Text;
        item.Status = 1;
        if (!string.IsNullOrEmpty(ID))
        {
            item.SystemId = new Guid(ID);
            CZRegistSystem.Update(item);
        }
        else
            CZRegistSystem.Insert(item);
        Response.Redirect("RegistSystem.aspx");
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("RegistSystem.aspx");
    }

    protected void usersRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            CZRegistSystem item = new CZRegistSystem();
            item.Status = -1;
            item.LastModifier = CurrentUserName;
            item.LastModTime = DateTime.Now;
            CZRegistSystem.Update(item);
            Response.Redirect("RegistSystem.aspx");
        }
    }
}
