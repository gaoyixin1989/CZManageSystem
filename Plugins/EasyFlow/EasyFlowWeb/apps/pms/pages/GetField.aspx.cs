using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Extension.IBatisNet;
using System.Data;
using System.Text;

public partial class apps_pms_pages_GetField : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

       var dt2= IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format("select   name   from   syscolumns   where   id=object_id('{0}')", Request.Form["tablename"])).Tables[0];
       StringBuilder sb3 = new StringBuilder();
       for (int i = 0; i < dt2.Rows.Count; i++)
       {
           sb3.Append(dt2.Rows[i]["name"].ToString().Trim());
               if(i!=dt2.Rows.Count-1)
               {
                   sb3.Append(",");
               }
       }
       Response.Write(sb3.ToString());
       Response.End();
    }
}