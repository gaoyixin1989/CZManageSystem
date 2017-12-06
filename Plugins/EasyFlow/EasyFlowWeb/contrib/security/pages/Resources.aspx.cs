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

public partial class contrib_security_pages_Resources : Botwave.Security.Web.PageBase
{
    private IResourceService resourceService=(IResourceService)Ctx.GetObject("resourceService");

    public IResourceService ResourceService
    {
        get { return resourceService; }
        set { resourceService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    private void Bind()
    {
        IList<ResourceInfo> groups = resourceService.GetResourcesByParentId("00");
        this.ddlParentResources.DataSource = groups;
        this.ddlParentResources.DataTextField = "Alias";
        this.ddlParentResources.DataValueField = "ResourceId";
        this.ddlParentResources.DataBind();

        if (this.ddlParentResources.Items.Count > 0)
        {
            this.BindChildResources(this.ddlParentResources.Items[0].Value);
        }

        //groups = resourceService.GetResourcesByParentId("");
    }

    private void BindChildResources(string parentId)
    {
        this.rptResources.DataSource = resourceService.GetResourcesByParentId(parentId);
        this.rptResources.DataBind();
    }

    protected void ddlParentResources_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindChildResources(this.ddlParentResources.SelectedValue);
    }
}
