using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class test_testIbatis : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            //try
            //{
            //    //ExecuteNoQuery();
            //    ExecuteDataReader();
            //    Response.Write("Completed ..");
            //}
            //catch (Exception ex)
            //{
            //    Response.Write(ex.ToString());
            //}
        }
    }

    private void ExecuteDataReader()
    {
        for (int i = 0; i < 300; i++)
        {
            try
            {
                string sql = @"SELECT  TOP 1   ChannelNO, LinkMan, MobileNo FROM         xsjyinfo ORDER BY ChannelNO";
                using (IDataReader reader = Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteReader(CommandType.Text, sql))
                { 
                    
                }
               
            }
            catch (Exception ex)
            {
                Response.Write(i.ToString() + "<br />");
                Response.Write(ex.ToString());
            }
        }
        Response.Write("reader . ok");
    }
    private void ExecuteScalar()
    {
        string sql = @"SELECT  TOP 1   ChannelNO FROM         xsjyinfo ORDER BY ChannelNO";
        object scalar = Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
    }

    private void ExecuteNoQuery()
    {
        int i = 0;
        for (; i < 200; i++)
        {
            try
            {
                string sql = @"INSERT INTO xsjyinfo (ChannelNO, LinkMan, MobileNo) VALUES('{0}', '{1}', '13000000000')";
                sql = string.Format(sql, i.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"), Guid.NewGuid().ToString("N"));
                Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                Response.Write(i.ToString() + "<br />");
                Response.Write(ex.ToString());
            }
        }
    }
}
