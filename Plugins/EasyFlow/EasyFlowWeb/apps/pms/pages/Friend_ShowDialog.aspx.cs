using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text;

public partial class apps_pms_pages_Friend_ShowDialog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            //Response.Write("<script>window.parent.returnval();</script>");
            if (Request.Files.Count > 0)
            {  StringBuilder sb = new StringBuilder();
                using (Stream fs = Request.Files[0].InputStream)
                {
                    if (Path.GetExtension(Request.Files[0].FileName).ToLower() != ".txt")
                    {
                        Response.Write("<script>alert('请选择.txt文件');</script>"); return;
                    } 
                  
                    using (StreamReader sr = new StreamReader(fs, Encoding.Default))//编码要写上，不然插入的是乱码！
                    {


                        try
                        {
                            string line = null;
                        
                            while ((line = sr.ReadLine()) != null)
                            {
                                //sb.Append(line).Append(",");
                                sb.Append("<li><div class='item_name' title='"+line+"' rel=" + line + "><div class='items' ></div><div class='item_del' onclick='$(this).parent().remove();' title='取消选择'></div>" + line + "</div></li>");


                            }
                           // sb.Remove(sb.Length-1,1);
                        }

                        catch (Exception)
                        {
                            Response.Write("<script>请选择正确的文件;</script>");
                            return;
                        }
                    }
                    
                }
                //msg("导入成功！");
                Response.Write("<script>window.parent.returnval(\"" + sb .ToString()+ "\");</script>");
            }
            else
            {
                Response.Write("<script>请选择.txt文件;</script>");
            }
        }
       
        //  Page.ClientScript.RegisterStartupScript(this.GetType(), "abc", "reBind();", true);
    }


}