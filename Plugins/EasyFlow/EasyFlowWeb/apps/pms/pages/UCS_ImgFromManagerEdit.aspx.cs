using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Botwave.Extension.IBatisNet;
using Botwave.Security.Web;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;

public partial class apps_pms_pages_UCS_ImgFromManagerEdit : PageBase
{
    #region 初始化
    private IUcs_ReportformsService ucs_ReportformsService = (IUcs_ReportformsService)Ctx.GetObject("ucs_ReportformsService");

    protected IUcs_ReportformsService Ucs_ReportformsService
    {
        get { return ucs_ReportformsService; }
        set { ucs_ReportformsService = value; }
    }
    #endregion
   protected Ucs_Reportforms Reportforms=new Ucs_Reportforms();
   Guid id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Form["isifr"] == "1")
        {
          
                Preview();
            
           
        }

        id = new Guid(string.IsNullOrEmpty(Request.QueryString["Id"]) ? Guid.Empty.ToString() : Request.QueryString["Id"]);
        Reportforms = ucs_ReportformsService.GetReprotformsByid(id, Guid.Empty);
        BindSelectList();
        if (id!=Guid.Empty)
        {
        //this.txtCMCC_BRANCH_CD.ReadOnly = true;
            if (!IsPostBack)
            {
                fieldlist.DataSource = Reportforms.FieldList;
                fieldlist.DataBind();
                this.formname.Text = Reportforms.formname;
                this.strOrder.Text = Reportforms.strOrder;
                this.strGroup.Text = Reportforms.strGroup;
                // this.txtCMCC_BRANCH_CD.Text = item.CMCC_BRANCH_CD;
                if (!string.IsNullOrEmpty(Reportforms.strWhere))
                {
                    this.strWhere.Text = (Reportforms.strWhere.StartsWith("and") || Reportforms.strWhere.StartsWith("or ")) ? Reportforms.strWhere.Substring(3) : Reportforms.strWhere;
                    radAnd.Checked = Reportforms.strWhere.StartsWith("and");
                    radOr.Checked = Reportforms.strWhere.StartsWith("or ");
                }
                this.formtype.Value = Reportforms.formtype.ToString();
                this.counttype.Value = Reportforms.Statistics ? "1":"0";
                this.type.Value = Reportforms.type.ToString();
                this.strRule.Text = Reportforms.FilterRule;
                this.ddlOnly.Value = Reportforms.ImageOnly ? "1" : "0";
            }
        }
    }

    void Preview()
    {
        Reportforms.formname = this.formname.Text.Trim();
        Reportforms.strOrder = this.strOrder.Text.Trim();
        Reportforms.type = 1;
        Reportforms.datasource = Request.Form["datasource"];
        Reportforms.updatetime = DateTime.Now;
        Reportforms.lasthandlers = CurrentUser.UserName;
        Reportforms.strGroup = this.strGroup.Text.Trim();
        Reportforms.strWhere = this.strWhere.Text;
        if (!string.IsNullOrEmpty(Reportforms.strWhere))
        {
            if (!radAnd.Checked && !radOr.Checked)
                Reportforms.strWhere = "and " + Reportforms.strWhere;
            else if (radOr.Checked)
                Reportforms.strWhere = "or  " + Reportforms.strWhere;
            else if (radAnd.Checked)
                Reportforms.strWhere = "and " + Reportforms.strWhere;
        }
        Reportforms.formtype = Convert.ToInt32(formtype.Value);
        Reportforms.Statistics = counttype.Value == "1" ? true : false;
        Reportforms.ImageOnly = ddlOnly.Value == "1" ? true : false;
        Reportforms.FilterRule = strRule.Text;
           Response.Write(string.Format("<script>    window.parent.preview('{0}')</script>", ucs_ReportformsService.PreView(Reportforms, Request.Form)+"|"));
        Response.End();
    }


    protected StringBuilder sb = new StringBuilder();
    protected StringBuilder sb2 = new StringBuilder();
    protected StringBuilder sb3 = new StringBuilder();
    protected StringBuilder sb4 = new StringBuilder();
    void BindSelectList()
    {
      var dt = Ucs_ReportformsService.GetAllTableName();
      var dt2 = ucs_ReportformsService.GetAllFieldName(Reportforms.datasource);
      int i = 0;
      for (i = 0; i < dt.Rows.Count; i++)
      {
          if (dt.Rows[i]["xtype"].ToString().Trim() == "U")
          {
              sb.Append("<option value='").Append(dt.Rows[i]["name"].ToString().Trim()).Append("'>").Append(dt.Rows[i]["name"]).Append("</option>");
          }
          else 
          {
              sb2.Append("<option value='").Append(dt.Rows[i]["name"].ToString().Trim()).Append("'>").Append(dt.Rows[i]["name"]).Append("</option>");
          }
      }
      for (i = 0; i < dt2.Rows.Count; i++)
      {
          sb3.Append("<option value='").Append(dt2.Rows[i]["name"].ToString().Trim()).Append("'>").Append(dt2.Rows[i]["name"]).Append("</option>");
      }
        //  sb.Append("</optgroup>");
    }
    protected void buttonOK_Click(object sender, EventArgs e)
    {
        Reportforms.formname = this.formname.Text.Trim();
        Reportforms.strOrder = this.strOrder.Text.Trim();
        Reportforms.type = int.Parse(type.Value);
        Reportforms.datasource = Request.Form["datasource"];
        Reportforms.updatetime = DateTime.Now;
        Reportforms.lasthandlers = CurrentUser.UserName;
        Reportforms.strGroup = this.strGroup.Text.Trim();
        Reportforms.strWhere = this.strWhere.Text;
        if (!string.IsNullOrEmpty(Reportforms.strWhere))
        {
            if (!radAnd.Checked && !radOr.Checked)
                Reportforms.strWhere = "and " + Reportforms.strWhere;
            else if (radOr.Checked)
                Reportforms.strWhere = "or  " + Reportforms.strWhere;
            else if (radAnd.Checked)
                Reportforms.strWhere = "and " + Reportforms.strWhere;
        }
        Reportforms.formtype =Convert.ToInt32( formtype.Value);
        Reportforms.Statistics = counttype.Value == "1" ? true : false;
        Reportforms.FilterRule = strRule.Text;
        Reportforms.ImageOnly = ddlOnly.Value == "1" ? true : false;
        if (Reportforms.id == Guid.Empty)
        {
            ucs_ReportformsService.InsertForm(Reportforms,Request.Form);
        }
        else
        {
            ucs_ReportformsService.UpdateForm(Reportforms,Request.Form);
        }
          ShowSuccess(Botwave.Web.MessageHelper.Message_Success, AppPath + "apps/pms/pages/UCS_ImgFromManager.aspx");
                return;
        //}
    }
    protected void Botton_txtHostry(object sender, EventArgs e)
    {
        Response.Redirect("UCS_FromManager.aspx");
    }
    private void Load()
    {
        // this.txtProduct.Items.Add(new ListItem("神州行", "神州行"));
        // this.txtProduct.Items.Add(new ListItem("全球通", "全球通"));
        //this.txtProduct.Items.Add(new ListItem("动感地带", "动感地带"));
    }




}