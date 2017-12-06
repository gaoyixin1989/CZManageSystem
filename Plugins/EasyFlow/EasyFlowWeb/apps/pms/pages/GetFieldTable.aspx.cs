using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Botwave.Extension.IBatisNet;

public partial class apps_pms_pages_GetFieldTable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<tr><th>字段名</th><th>类型</th><th>字段说明</th></tr>");
        DataTable dt = Gettable();
        if (dt == null || dt.Rows.Count == 0)
        {
            Response.End();
        }
        else {
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr><td> ").Append(row["字段名"]).Append("</td><td> ").Append(row["类型"]).Append("</td><td> ").Append(row["字段说明"]).Append("</td>");
            }
            Response.Write(sb.ToString());
         }
        Response.End();
 

 

    }




    DataTable Gettable()
    {
        string str = string.Format(@" SELECT 
 (case when a.colorder=1 then d.name else '' end) 表名,
a.colorder 字段序号,
a.name 字段名,
 (case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end) 标识,
 (case when (SELECT count(*) 
 FROM sysobjects 
 WHERE (name in (SELECT name
 FROM sysindexes
 WHERE (id = a.id) AND (indid in (SELECT indid
 FROM sysindexkeys
 WHERE (id = a.id) AND (colid in (SELECT colid
 FROM syscolumns
 WHERE (id = a.id) AND (name = a.name)
 )
 )
 )
 )
 )
 ) AND (xtype = 'PK') 
 ) > 0 then '√' else '' end) 主键,
b.name 类型,
a.length 占用字节数,
COLUMNPROPERTY(a.id,a.name,'PRECISION') as 长度,
isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as 小数位数,
 (case when a.isnullable=1 then '√'else '' end) 允许空,
isnull(e.text,'') 默认值,
isnull(g.[value],'') AS 字段说明    

FROM  syscolumns a 
 left join systypes b on a.xtype=b.xusertype
 inner join sysobjects d on a.id=d.id  and  d.xtype='U' and d.name<>'dtproperties'
 left join syscomments e on a.cdefault=e.id
 left join sys.extended_properties g on a.id=g.major_id AND a.colid = g.minor_id  
where d.name ='{0}'
order by a.id,a.colorder 
",Request.QueryString["tablename"]);
       return IBatisDbHelper.ExecuteDataset(CommandType.Text, str).Tables[0];
    }
}