<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_security_pages_Login" Codebehind="Login.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>系统登录</title>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="loginClass">
        <div class="loginHead">
            <h1><%=AppName%></h1>
            <span>请输入您的用户名及密码，以登录应用系统。</span>
        </div>
        <div class="loginBody">
            <div class="userLogo">
                <img src="<%=securityPath%>res/img/userLogo1.gif" alt="" />
                <div id="divMessage" runat="server"></div>
            </div>
            <div class="formClass">
                <div class="userInput">
                    <label for="rfvUserName">
                        用户名<asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="(*不能为空)" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                    </label>
                    <input type="text" id="txtUserName" class="inputbox_focus" runat="server" />
                </div>
                <div class="userInput">
                    <label for="password">密　码</label>
                    <input type="password" id="txtPassword" class="inputbox" runat="server" />
                </div>
                <div class="btnSumbit">
                    <asp:Button ID="btnLogin" Text="登录" runat="server" onclick="btnLogin_Click" CssClass="btnLogin"  />
                    <%--<a href="Regist.aspx">新用户注册</a>--%>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

