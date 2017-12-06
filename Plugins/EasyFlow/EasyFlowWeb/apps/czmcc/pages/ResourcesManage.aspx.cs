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

using Botwave.Workflow.Practices.CZMCC.Service.Impl;

public partial class apps_czmcc_pages_ResourcesManage : Botwave.Web.PageBase
{
    private static readonly string ReturnUrl = AppPath + "apps/czmcc/pages/ResourcesManage.aspx";
    ResourcesExecutionService re = new ResourcesExecutionService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindResources();
            BindCardResources();
        }
    }

    protected void BindResources()
    {
        DataTable dt = re.GetResourcesByType(1);
        gvResourcesList.DataSource = dt;
        gvResourcesList.DataBind();
    }

    protected void BindCardResources()
    {
        DataTable dt = re.GetResourcesByType(2);
        gvCard.DataSource = dt;
        gvCard.DataBind();
    }

    #region NoteBook
    protected void gvResourcesList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvResourcesList.EditIndex = e.NewEditIndex;
        BindResources();
    }
    protected void gvResourcesList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox txtModel = (TextBox)gvResourcesList.Rows[e.RowIndex].Cells[0].Controls[0];
        TextBox txtsNum = (TextBox)gvResourcesList.Rows[e.RowIndex].Cells[1].Controls[0];
        Guid ID = new Guid(gvResourcesList.DataKeys[e.RowIndex].Value.ToString());

        re.UpdateResourcesInfo(ID,1,txtModel.Text,txtsNum.Text,0);

        gvResourcesList.EditIndex = -1;
        BindResources();
    }
    protected void gvResourcesList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvResourcesList.EditIndex = -1;
        BindResources();
    }

    protected void gvResourcesList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Guid ID = new Guid(this.gvResourcesList.DataKeys[e.RowIndex].Value.ToString());

        re.DeleteResourcesInfo(ID);
        BindResources();
    }

    protected void gvResourcesList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbState = e.Row.FindControl("lbState") as Label;
            DataRowView dr = (DataRowView)e.Row.DataItem;

            if (dr.Row["State"].ToString() == "0")
            {
                lbState.Text = "未申请";
            }
            else if (dr.Row["State"].ToString() == "1")
            {
                lbState.Text = "已申请";
            }
        }
    }
    #endregion

    #region Card
    protected void gvCard_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvCard.EditIndex = e.NewEditIndex;
        BindCardResources();
    }
    protected void gvCard_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox txtModel = (TextBox)gvCard.Rows[e.RowIndex].Cells[0].Controls[0];
        TextBox txtsNum = (TextBox)gvCard.Rows[e.RowIndex].Cells[1].Controls[0];
        Guid ID = new Guid(gvCard.DataKeys[e.RowIndex].Value.ToString());

        re.UpdateResourcesInfo(ID, 2, txtModel.Text, txtsNum.Text, 0);

        gvCard.EditIndex = -1;
        BindCardResources();
    }
    protected void gvCard_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvCard.EditIndex = -1;
        BindCardResources();
    }

    protected void gvCard_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Guid ID = new Guid(this.gvCard.DataKeys[e.RowIndex].Value.ToString());

        re.DeleteResourcesInfo(ID);
        BindCardResources();
    }

    protected void gvCard_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbState = e.Row.FindControl("lbState") as Label;
            DataRowView dr = (DataRowView)e.Row.DataItem;

            if (dr.Row["State"].ToString() == "0")
            {
                lbState.Text = "未申请";
            }
            else if (dr.Row["State"].ToString() == "1")
            {
                lbState.Text = "已申请";
            }
        }
    }
    #endregion

    protected void btnAddGroup_Click(object sender, EventArgs e)
    {
        int type = Convert.ToInt32(ddlReType.SelectedValue);
        string rMode = txtReModel.Text.Trim();
        string sNum = txtSerialNumber.Text.Trim();

        bool res = re.SaveResourcesInfo(type,rMode,sNum);

        if (res == false)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "", "<script language=javascript>alert('已存在此资源!!');</script>");
        }
        else
        {
            Response.Redirect(ReturnUrl);
        }
       
    }
}
