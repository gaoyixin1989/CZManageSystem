<%@ Page Language="C#" EnableTheming="false" StylesheetTheme="" EnableViewState="false" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Botwave.Commons" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    private const string TemplateConnectionString = "server={0};UID={1};PWD={2};database={3};Connection Reset=FALSE";

    private string GetConnectionString()
    {
        string server = txtServer.Text.Trim();
        string database = txtDatabase.Text.Trim();
        string uid = txtUserID.Text.Trim();
        string password = txtPassword.Text.Trim();
        if (server.Length > 0
            && database.Length > 0
            && uid.Length > 0)
        {
            return String.Format(TemplateConnectionString, server, uid, password, database);
        }
        return String.Empty;
    }

    private DataSet GetData()
    {
        string sql = txtSQL.Text.Trim();
        string conStr = GetConnectionString();
        DataSet ds = null;

        ltlResult.Text = "";
        if (conStr.Length > 0 && sql.Length > 0)
        {
            try
            {
                ds = SqlHelper.ExecuteDataset(conStr, CommandType.Text, sql);
            }
            catch (DataException ex)
            {
                Response.Write(ex.ToString());
                Response.End();
            }
        }
        return ds;
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (null == Session["sqladmin:admin"])
        {
            Response.Write("没权限别干坏事哦!");
            Response.End();
        }
    }
    
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        DataSet ds = GetData();
        if (null != ds)
        {
            dgList.DataSource = ds.Tables[0];
            dgList.DataBind();
        }
    }

    protected void btnExecute_Click(object sender, EventArgs e)
    {
        int retVal = 0;
        string sql = txtSQL.Text.Trim();
        string conStr = GetConnectionString();

        ltlResult.Text = "";
        if (conStr.Length > 0 && sql.Length > 0)
        {
            try
            {
                retVal = SqlHelper.ExecuteNonQuery(conStr, CommandType.Text, sql);
                ltlResult.Text = "受影响行数：" + retVal.ToString();
            }
            catch (DataException ex)
            {
                Response.Write(ex.ToString());
                Response.End();
            }
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataSet ds = GetData();
        if (null == ds)
        {
            Response.Write("没有找到任何数据");
            Response.End();
        }

        Response.ContentType = "application/ms-excel";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.AppendHeader("Content-Disposition", "attachment;filename=ReportData.xls");

        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        DataGrid dg = new DataGrid();
        dg.DataSource = ds;
        dg.DataBind();
        dg.RenderControl(htw);

        Response.Write(sw.ToString());
        Response.End();
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["sqladmin:admin"] = null;
        ltlResult.Text = "已注销";
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>sqladmin query</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
				<tr>
					<td>SQL Server 名称：</td>
					<td><asp:TextBox id="txtServer" runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvServer" runat="server" ErrorMessage="*" ControlToValidate="txtServer"></asp:RequiredFieldValidator></td>
				</tr>
				<tr>
					<td>数据库名称：</td>
					<td><asp:TextBox id="txtDatabase" runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvDatabase" runat="server" ErrorMessage="*" ControlToValidate="txtDatabase"></asp:RequiredFieldValidator></td>
				</tr>
				<tr>
					<td>用户名：</td>
					<td><asp:TextBox id="txtUserID" runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvUserID" runat="server" ErrorMessage="*" ControlToValidate="txtUserID"></asp:RequiredFieldValidator></td>
				</tr>
				<tr>
					<td>密 码：</td>
					<td><asp:TextBox id="txtPassword" runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvPassword" runat="server" ErrorMessage="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator></td>
				</tr>
				<tr>
					<td colspan="2"><asp:TextBox id="txtSQL" runat="server" TextMode="MultiLine" Rows="10" Columns="50"></asp:TextBox></td>
				</tr>
				<tr>
					<td align="right" colspan="2">
						<font color="red">
							<asp:Literal id="ltlResult" runat="server"></asp:Literal></font>
						<asp:Button ID="btnSelect" Runat="server" Text="Select" 
                            onclick="btnSelect_Click"></asp:Button>
						<asp:Button ID="btnExecute" Runat="server" Text="Execute" 
                            onclick="btnExecute_Click"></asp:Button>
						<asp:Button id="btnExport" runat="server" Text="Export" 
                            onclick="btnExport_Click"></asp:Button>
                        <asp:Button id="btnLogout" runat="server" Text="Logout" 
                            onclick="btnLogout_Click" CausesValidation="false"></asp:Button>
					</td>
				</tr>
			</table>
			<div>
					<asp:DataGrid id="dgList" runat="server" EnableViewState="False" AutoGenerateColumns="True"></asp:DataGrid>
			</div>
    </div>
    </form>
</body>
</html>
