using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Security.Web;
using System.Text;
using System.Web.Script.Serialization;
using System.Data;
using Botwave.XQP.Util;
using Botwave.XQP.Service;
public partial class apps_pms_pages_GetMarkData : PageBase
{
    JavaScriptSerializer js = new JavaScriptSerializer();
    #region  接口初始化
    private IUCS_MarketManageService ucs_MarketManageService;

    public IUCS_MarketManageService Ucs_MarketManageService
    {
        set { ucs_MarketManageService = value; }
    }
    #endregion
    string field; string value;
    protected void Page_Load(object sender, EventArgs e)
    {
        int total=0;
         field = Request.Form["text"];
          value = Request.Form["value"];
          if (string.IsNullOrEmpty(value)) { value = field; }
        string strWhere = Request.Form["strWhere"];
        string tableName = Request["tableName"];
        int pageIndex =Convert.ToInt32( Request.Form["index"]);
        string str = field;
        if (!string.IsNullOrEmpty(value)&&field.Trim()!=value.Trim()) { str = field + "," + value; }
        var dt = CommontUnit.Instance.GetUsersByPager(tableName, field, str, GetStrWhere(field, strWhere), field.Substring(0, field.ToLower().LastIndexOf(" as") > 0 ? field.ToLower().LastIndexOf(" as") : field.Length), pageIndex, 200, field.Substring(0, field.ToLower().LastIndexOf(" as") > 0 ? field.ToLower().LastIndexOf(" as") : field.Length) + "," + value.Substring(0, value.ToLower().LastIndexOf(" as") > 0 ? value.ToLower().LastIndexOf(" as") : value.Length), ref  total);
        Response.Write(GetHtml(dt));
        Response.End();
         
    }
    public string GetHtml(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();
        field = field.Substring((field.ToLower().LastIndexOf(" as") > 0 ? field.ToLower().LastIndexOf(" as") : -3) + 3).Trim();
        value = value.Substring((value.ToLower().LastIndexOf(" as") > 0 ? value.ToLower().LastIndexOf(" as") : -3) + 3).Trim();
        if (string.IsNullOrEmpty(value))
        {
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<li title='"+row[field]+"' hid='" + row[field] + "'>" + row[field] + "</li>");
            }
        }
        else
        {
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<li title='"+row[field]+"' hid='" + row[value] + "'>" + row[field] + "</li>");
            }  
        }
        return sb.ToString();
    }
    public string GetStrWhere(string field,string str)
    {
        StringBuilder sb=new StringBuilder();
        field = field.Substring(0, field.ToLower().LastIndexOf(" as") > 0 ? field.ToLower().LastIndexOf(" as") : field.Length);
        sb.Append(field).Append(" like ").Append("'%").Append(str).Append("%'");
        if (!string.IsNullOrEmpty(Request.QueryString["dpid"]))
        {
            sb.Append("and dptype='" + Request.QueryString["dpid"] + "'");
        }
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            sb.Append("and MARK_PLAN_CD='" + Request.QueryString["id"] + "'");
        }
        if (!string.IsNullOrEmpty(Request.Form["fieldWhere"]))
        {
           // sb.Append(" and ").Append(Request.Form["fieldWhere"]);
            sb.Append(" and ").Append(Botwave.XQP.Commons.XQPHelper.DecodeBase64("UTF-8",Request.Form["fieldWhere"]));
        }
        return sb.ToString();
    }
}