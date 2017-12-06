<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_gmcc_Login" Codebehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统登录</title>
    <%--<script type="text/javascript" language="javascript" src="../scripts/Base64.js"></script>--%>
</head>
<body id="login" scroll="no">
    <div id="container">
        <div id="header"><h1>系统登录</h1></div>
        <div id="content">
            <form id="form1" runat="server">
                    <table cellpadding="0" cellspacing="0" style="padding-top:120px">
                        <tr>
                            <th>用户名：</th>
                            <td><input type="text" id="txtUserName" runat="server" /></td>
                        </tr>
                        <tr>
                            <th>密　码：</th>
                            <td><input type="password" id="txtPassword" runat="server" /></td>
                        </tr>
                        <tfoot>
                            <tr>
                                <th></th>
                                <td>
                                    <div id="divMessage" runat="server"></div>
                                    <asp:ImageButton ID="btnLogin" runat="server" Width="50" Height="20" onclick="btnLogin_Click" OnClientClick="setValue()" />
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                    
            </form>
        </div>
        <div id="footer">
            <p>
                Copyright @ 2014 ucsmy all rights reserved.
            </p>
        </div>
    </div>    
</body>

</html>
