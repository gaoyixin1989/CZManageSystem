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

using Botwave.Web;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.XQP.Util;

public partial class apps_pms_pages_UCS_FromManager : Botwave.Security.Web.PageBase
{
    protected string keywords;
    protected Guid? roleId;
 


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int total=0;
            SearchUsers2(total, 0);
        }
    }
    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.SearchUsers2(listPager.TotalRecordCount, e.NewPageIndex);
    }

    private void SearchUsers2(int recordCount, int pageIndex)
    {
        var results = CommontUnit.Instance.GetUsersByPager("Ucs_Reportforms", string.Empty, "*", GetStrWhere(), "formname desc", pageIndex, listPager.ItemsPerPage, string.Empty, ref recordCount);
        this.usersRepeater.DataSource = results;
        this.usersRepeater.DataBind();
        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.SearchUsers2(listPager.TotalRecordCount, 0);
    }
    public string GetStrWhere()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("1=1 and formtype=1 ");
        if (!string.IsNullOrEmpty(Request.Form["txtKeyword"]))
        {
            sb.Append( "and formname like '%" + Request.Form["txtKeyword"] + "%' or datasource like '%" + Request.Form["txtKeyword"] + "%'");
        }
     
        return sb.ToString();
    }

  
}
