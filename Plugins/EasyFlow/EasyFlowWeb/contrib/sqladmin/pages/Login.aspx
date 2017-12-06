<%@ Page Language="C#" EnableTheming="false" StylesheetTheme="" EnableViewState="false" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Botwave.Commons" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    private const bool UseSqlUsers = false;
    private static string[,] PredefinedUsers = { { "matrix", "sdfEIf45(])=" } };
    
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUserName.Text.Trim();
        string password = txtPassword.Text.Trim();
        string gotoPage = txtGoTo.Text.Trim();
        if (username.Length > 0
            && password.Length > 0
            && gotoPage.Length > 0)
        {
            bool isValid = CheckUser(username, password);
            if (isValid)
            {
                Session["sqladmin:admin"] = true;
                Response.Redirect(gotoPage);
            }
            else
            {
                ltlResult.Text = "用户名/密码错误";
            }
        }
    }

    private bool CheckUser(string username, string password)
    {
        if (UseSqlUsers)
        {
            username = DbUtils.FilterSQL(username);
            string cmdText = String.Format("select [Password] from bw_SqlAdmin where UserName = '{0}'", username);
            object obj = SqlHelper.ExecuteScalar(CommandType.Text, cmdText);
            if (null != obj)
            {
                if (password == obj.ToString())
                {
                    return true;
                }
            }
        }
        else
        {
            for (int i = 0, ilen = PredefinedUsers.Length - 1; i <= ilen; i++)
            {
                if (username == PredefinedUsers.GetValue(i, 0).ToString()
                    && password == PredefinedUsers.GetValue(i, 1).ToString())
                {
                    return true;
                }
            }
        }
        
        return false;
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>sqladmin login</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
				<tr>
					<td>用户名：</td>
					<td><asp:TextBox id="txtUserName" runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvUserName" runat="server" ErrorMessage="*" ControlToValidate="txtUserName"></asp:RequiredFieldValidator></td>
				</tr>
				<tr>
					<td>密 码：</td>
					<td><asp:TextBox id="txtPassword" runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvPassword" runat="server" ErrorMessage="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator></td>
				</tr>
				<tr>
					<td>转到页：</td>
					<td><asp:TextBox id="txtGoTo" runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvGoTo" runat="server" ErrorMessage="*" ControlToValidate="txtGoTo"></asp:RequiredFieldValidator></td>
				</tr>
				<tr>
					<td colspan="2"><asp:Button id="btnLogin" runat="server" Text="Login" 
                            onclick="btnLogin_Click"></asp:Button>
                            <font color="red">
							<asp:Literal id="ltlResult" runat="server"></asp:Literal></font>
					</td>
				</tr>
			</table>
    </div>
    </form>
</body>
</html>
