<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_gmcc_pages_LoginIndex" Codebehind="LoginIndex.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>系统登录</title>
</head>
<body id="login" scroll="no">
    <div id="container">
        <div id="header">
            <h1>
                系统登录</h1>
        </div>
        <div id="content">
            <form id="form1" runat="server">
            <table cellpadding="0" cellspacing="0" style="padding-top: 90px">
                <tr>
                    <th>
                        用户名：
                    </th>
                    <td>
                        <%--<input type="text" id="txtUserName" runat="server" autocomplete="off" />--%>
                        <input type="text" id="<%=textfield %>" onkeyup="keyUp(this)" autocomplete="off" />
                    </td>
                </tr>
                <tr>
                    <th>
                        密 码：
                    </th>
                    <td>
                        <%--<input type="password" id="txtPassword" runat="server" autocomplete="off" />--%>
                        <input type="password" id="<%=password1%>" />
                        <input type="hidden" name="<%=txtRoom %>" id="<%=txtRoom %>" />
                        <input type="hidden" id="txtCode" style="width: 52px;" maxlength="8" runat="server" />
                    </td>
                </tr>
                <tr valign="top">
                    <th>
                        验证码：
                    </th>
                    <td style="text-align: left">
                        <%--<input type="text" id="txtCode" style="width:52px;" maxlength="8" runat="server" />--%>
                        <input type="text" id="txtCheckCode" style="width: 52px;" runat="server" />
                        <div style="display: inline; position: absolute" title="换验证码，请点击.">
                            <img alt="看不清楚，请点击换一张." style="cursor: pointer;" src="<%=AppPath%>contrib/security/pages/checkCodeHandler.ashx"
                                onclick="this.src='<%=AppPath%>contrib/security/pages/checkCodeHandler.ashx?'+Math.random()" />
                        </div>
                    </td>
                </tr>
                <tfoot>
                    <tr>
                        <th>
                        </th>
                        <td>
                            <div id="divMessage" runat="server">
                            </div>
                            <asp:ImageButton ID="btnLogin" runat="server" Width="50" Height="20" OnClick="btnLogin_Click"
                                OnClientClick="setValue()" />
                        </td>
                    </tr>
                </tfoot>
            </table>
            <script type="text/javascript">
                document.getElementById("<%=textfield %>").focus();
                function hiddenPass(e) {
                    e = e ? e : window.event;
                    var kcode = e.which ? e.which : e.keyCode;
                    var pass = document.getElementById("password1");
                    var j_pass = document.getElementById("<%=txtRoom %>");
                    if (kcode != 13) {
                        var keychar = String.fromCharCode(kcode);
                        j_pass.value = j_pass.value + keychar;
                        j_pass.value = j_pass.value.substring(0, pass.length);
                    }
                }
                function setValue() {
                    var b = new Base64();
                    var u = document.getElementById("<%=textfield %>");
                    var p = document.getElementById("<%=txtRoom%>");
                    var c = document.getElementById("<%=password1%>");
                    var cd = document.getElementById("<%=txtCheckCode.ClientID%>");
                    //u.value = b.encode(u.value);
                    var username = u.value + $.md5("<%=randomStr %>") + c.value;
                    //alert(username.length)
                    var pwd = $.md5(c.value);
                    c.value = "";
                    u.value = "";
                    pwd = $.md5(username + pwd + "<%=randomStr %>");
                    p.value = b.encode(username + pwd);
                    cd.value = cd.value.toUpperCase();
                    cd.value = $.md5(cd.value);
                }
                function keyUp(obj) {

                    if (!obj.value.match(/^[a-zA-Z0-9_]+$/)) {
                        obj.value = '';
                    } else {
                        obj.t_value = obj.value;
                    }
                    if (obj.value.match(/^[a-zA-Z][a-zA-Z0-9_]+$/)) {
                        obj.o_value = obj.value;
                    }
                }
            </script>
            </form>
        </div>
        <div id="footer">
            <p>
                Copyright @ 2014 ucsmy all rights reserved. | 粤ICP备07018313号-2
            </p>
        </div>
    </div>
</body>
</html>
