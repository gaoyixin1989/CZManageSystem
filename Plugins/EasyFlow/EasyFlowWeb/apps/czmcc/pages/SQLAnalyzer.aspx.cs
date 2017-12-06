using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class apps_czmcc_pages_SQLAnalyzer : Botwave.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Botwave.Security.LoginHelper.UserName != "admin")
        {
            Response.Write("您没有访问权限。不能使用这个工具。");
            Response.End();
        }
    }

    protected void buttonExecute_Click(object sender, EventArgs e)
    {
        string command = this.txtCommand.Text;
        this.labelError.Text = "";
        this.labelResult.Text = "";
        this.holderResult.Controls.Clear();
        if (string.IsNullOrEmpty(command))
        {
            labelError.Text = "请输入命令语句。";
            return;
        }
        bool isQuery = this.radioTypes.SelectedValue == "0";
        try
        {
            if (isQuery)
            {
                DataSet ds = Botwave.Commons.SqlHelper.ExecuteDataset(CommandType.Text, command);
                this.labelResult.Text = string.Format("共返回表数：{0}", ds == null ? 0 : ds.Tables.Count);
                if (ds != null)
                {
                    foreach (DataTable table in ds.Tables)
                    {
                        Label header = new Label();
                        header.Text = string.Format("<h3># {0}</h3>", string.IsNullOrEmpty(table.TableName) ? "表" : table.TableName);

                        this.holderResult.Controls.Add(header);

                        GridView g = new GridView();
                        g.DataSource = table;
                        g.DataBind();

                        this.holderResult.Controls.Add(g);
                    }
                }
            }
            else
            {
                int rowAffected = Botwave.Commons.SqlHelper.ExecuteNonQuery(CommandType.Text, command);
                this.labelResult.Text = string.Format("共影响行数：{0}", rowAffected);
            }
        }
        catch (Exception ex)
        {
            this.labelError.Text = ex.ToString();
        }
    }
}
