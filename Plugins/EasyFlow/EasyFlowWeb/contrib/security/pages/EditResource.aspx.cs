using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;

public partial class contrib_security_pages_EditResource : Botwave.Web.PageBase
{
    private static string GroupParentId = "00";

    private IResourceService resourceService = (IResourceService)Ctx.GetObject("resourceService");

    public IResourceService ResourceService
    {
        get { return resourceService; }
        set { resourceService = value; }
    }

    public string Command
    {
        get { return ViewState["Command"].ToString(); }
        set { ViewState["Command"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindParents();
            if (Request.QueryString["resourceId"] != null)
            {
                string resourceId = Request.QueryString["resourceId"];

                ltlTitle.Text = "编辑";
                txtResourceId.Enabled = false;
                if (Request.QueryString["action"] != null && Request.QueryString["action"] == "del")
                {
                    Command = "Delete";
                    this.DeleteReource(resourceId);
                }
                else
                {
                    Command = "Edit";
                    btnEdit.Text = "保存";
                    btnEdit.CssClass = "btn_sav";
                    this.LoadResource(resourceId);
                }
            }
            else
            {
                ltlTitle.Text = "新增";
                Command = "Insert";
            }
        }
    }

    private void BindParents()
    {
        this.ddlParents.DataSource = resourceService.GetResourcesByParentId("00");
        this.ddlParents.DataTextField = "Alias";
        this.ddlParents.DataValueField = "ResourceId";
        this.ddlParents.DataBind();
        this.ddlParents.Attributes["onchange"] = "setResourceId(this);";
    }

    private void LoadResource(string resourceId)
    {
        ResourceInfo item = resourceService.GetResourceById(resourceId);
        if (item == null)
        {
            ShowError("资源不存在！", AppPath + "contrib/security/pages/resources.aspx");
            //throw new Exception("资源不存在！");
        }
        this.txtResourceId.Text = item.ResourceId;
        this.txtAlias.Text = item.Alias;
        this.txtName.Text = item.Name;
        this.chkboxEnabled.Checked = item.Enabled;

        foreach (ListItem litem in this.ddlParents.Items)
        {
            if (litem.Value == item.ParentId)
                litem.Selected = true;
        }

        foreach (ListItem litem in this.ddlTypes.Items)
        {
            if (litem.Value == item.Type)
                litem.Selected = true;
        }

        if (item.ParentId == "00")
        {
            this.chkboxEnabled.Enabled = false;
            this.ddlParents.Enabled = false;
        }
    }

    private void DeleteReource(string resourceId)
    {
        if (resourceService.GetResourceCountByParentId(resourceId) > 0)
        {
            ShowError("该资源存在有子资源，请先移动或者删除子资源后，再删除该资源！");
            //throw new Exception("该资源存在有子资源，请先移动或者删除子资源后，再删除该资源！");
        }
        resourceService.DeleteByResourceId(resourceId);
        ShowSuccess("成功删除资源.", AppPath + "contrib/security/pages/resources.aspx");
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string resourceId = txtResourceId.Text.Trim();
        string alias = txtAlias.Text;
        string name = txtName.Text.ToLower();

        ResourceInfo item = new ResourceInfo();
        item.ResourceId = resourceId;
        item.Alias = alias;
        item.Name = name;
        item.ParentId = string.IsNullOrEmpty(ddlParents.SelectedValue) ? GroupParentId : this.ddlParents.SelectedValue;
        item.Type = ddlTypes.SelectedValue;
        item.Enabled = chkboxEnabled.Checked;

        ResourceInfo source = resourceService.GetResourceById(resourceId);

        if (this.Command == "Edit")
        {
            resourceService.UpdateResource(item);
            ShowSuccess("修改资源成功!", AppPath + "contrib/security/pages/resources.aspx");
        }
        else if (this.Command == "Insert")
        {
            if (source != null || resourceService.ResourceIsExists(name, alias))
            {
                ShowError("资源已经存在，请重新填写");
                //throw new Exception("资源已经存在，请重新填写");
            }

            resourceService.InsertResource(item);
            ShowSuccess("新增资源成功!", AppPath + "contrib/security/pages/resources.aspx");
        }
    }
}
