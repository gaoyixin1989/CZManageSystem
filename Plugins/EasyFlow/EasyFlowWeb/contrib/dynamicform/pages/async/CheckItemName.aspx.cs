using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Web;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;

public partial class contrib_dynamicform_pages_async_CheckItemName : Botwave.Web.PageBase
{
    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");

    public IFormDefinitionService FormDefinitionService
    {
        set { this.formDefinitionService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string fname = Request.QueryString["fname"];
        string fid = Request.QueryString["fid"];
        if (!(String.IsNullOrEmpty(fname) && String.IsNullOrEmpty(fid)))
        {
            Guid formId = new Guid(fid);
            bool isExists = formDefinitionService.IsItemExists(formId, fname);
            Response.Clear();
            Response.Write(isExists.ToString().ToLower());
            Response.End();
        }
        else
        {
            Response.Clear();
            Response.Write("false");
            Response.End();
        }
    }
}
