<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_security_pages_LoginIndex" StylesheetTheme="common" Codebehind="LoginIndex.aspx.cs" %>
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
            <h1>潮州移动工作流平台</h1>
            <span>请输入您的用户名及密码，以登录应用系统。</span>
        </div>
        <div class="loginBody">
            <div class="userLogo">
                <img src="<%=securityPath%>res/img/userLogo1.gif" alt="" />
                <div id="divMessage" runat="server"></div>
            </div>
            <div class="formClass" style="padding-left:10px">
                <div class="middleInput" style="margin-top:8px">
                     <label style="display:inline; vertical-align:middle">用户名</label>
                    <input type="text" id="txtUserName" class="inputbox_focus" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" SetFocusOnError="true" ErrorMessage="(*不能为空)" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                </div>
                <div class="middleInput" style="margin-top:5px">
                     <label style="display:inline; vertical-align:middle">密　码</label>
                    <input type="password" id="txtPassword" class="inputbox" runat="server" />
                </div>
                <div class="smallInput" style="margin-top:5px">
                     <label style="display:inline; vertical-align:middle">验证码</label>
                    <input type="text" id="txtCode" maxlength="8" class="inputbox" runat="server" />
                    <img alt="看不清楚，请点击换一张." src="CheckCodeHandler.ashx" style="cursor:pointer" onclick="this.src='CheckCodeHandler.ashx?'+Math.random()" />
                    <asp:RequiredFieldValidator ID="rfvCode" runat="server" SetFocusOnError="true" ErrorMessage="(*不能为空)" ControlToValidate="txtCode"></asp:RequiredFieldValidator>
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
