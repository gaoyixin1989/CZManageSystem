using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using Botwave.Web;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.Workflow.Extension.Util;

public partial class contrib_dynamicform_pages_FormIndex : Botwave.Security.Web.PageBase
{
    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");

    public IFormDefinitionService FormDefinitionService
    {
        set { this.formDefinitionService = value; }
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    protected void BindData()
    {
        rpFormList.DataSource = formDefinitionService.ListFormDefinitionsByEntityType("Form_Template").Where(f=>f.Enabled==true);
        rpFormList.DataBind();
    }

    protected void rpFormList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Guid fid = new Guid(e.CommandArgument.ToString());
        if (e.CommandName == "Delete")
        {
            string userName = CurrentUserName;
            formDefinitionService.RemoveFormDefinition(fid, userName);
            BindData();
        }
    }
}
