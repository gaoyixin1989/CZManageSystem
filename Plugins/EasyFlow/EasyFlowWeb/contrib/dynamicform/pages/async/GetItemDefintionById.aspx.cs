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

public partial class contrib_dynamicform_pages_async_GetItemDefintionById : Botwave.Web.PageBase
{
    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");

    public IFormDefinitionService FormDefinitionService
    {
        set { this.formDefinitionService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string Id = Request.QueryString["Id"];
        if (!String.IsNullOrEmpty(Id))
        {
            Guid itemId = new Guid(Id);
            FormItemDefinition item = formDefinitionService.GetFormItemDefinitionById(itemId);
            DataTable dt = new DataTable();
            dt = ItemToTable(item);
            string xml = DataTable2Xml(dt);
            ResponseXml(xml);
        }
    }

    private DataTable ItemToTable(FormItemDefinition item)
    {
        Type type = typeof(FormItemDefinition);
        System.Reflection.PropertyInfo[] ps = type.GetProperties();
        DataTable dt = new DataTable();
        for (int i = 0; i < ps.Length; i++)
        {
            dt.Columns.Add(ps[i].Name, ps[i].PropertyType);
        }

        DataRow row = dt.NewRow();
        for (int i = 0; i < ps.Length; i++)
        {
            row[i] = ps[i].GetValue(item, null);
        }
        dt.Rows.Add(row);
        return dt;
    }

    private void ResponseXml(string xml)
    {
        Response.Clear();
        Response.ContentType = "text/xml";
        Response.AddHeader("Cache-Control", "no-cache");
        Response.Write(xml);
        Response.End();
    }

    private static string DataTable2Xml(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<Table>");
        foreach (DataRow dr in dt.Rows)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                sb.Append("<");
                sb.Append(dc.ColumnName);
                sb.Append(">");

                if (null != dr[dc.ColumnName] && dr[dc.ColumnName] != DBNull.Value)
                {
                    sb.Append(dr[dc.ColumnName]);
                }
                else
                {
                    sb.Append("");
                }

                sb.Append("</");
                sb.Append(dc.ColumnName);
                sb.Append(">");
            }

        }
        sb.Append("</Table>");

        return sb.ToString();
    }
}
