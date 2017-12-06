<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_gmcc_pages_LoginMobile" CodeBehind="LoginMobile.aspx.cs" %>

<!DOCTYPE html>
<html lang="zh-cn" manifest="<%=AppPath %>res/mobilecache.mf">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>流程易（EF）－中国移动广东公司潮州分公司 - 系统登录</title>
    <!-- Bootstrap -->
</head>
<body role="document">
    <form id="form1" runat="server">
        <div class="page-header">
            <!-- Fixed navbar -->
            <div class="navbar navbar-inverse navbar-fixed-top ui-header" role="navigation">
                <h1 class="text-center ui-title" style="color: rgb(69, 125, 179);">系统登录
                </h1>
            </div>
        </div>
        <div class="container theme-showcase" role="main">
            <div class="form-group">
                <label for="exampleInputUser">
                    用户名</label>
                <%--<input type="text" class="form-control" runat="server" id="txtUserName" placeholder="用户名" />--%>
                <input type="text" id="<%=textfield %>" class="form-control" onkeyup="keyUp(this)" autocomplete="off" placeholder="用户名" />
            </div>
            <div class="form-group">
                <label for="exampleInputPassword1">
                    密 码</label>
                <%--<input type="password" class="form-control" runat="server" id="txtPassword" placeholder="密码" />--%>
                <input type="password" id="<%=password1%>" class="form-control" placeholder="密码" autocomplete="off" />
                <input type="hidden" name="<%=txtRoom %>" id="<%=txtRoom %>" />
                <input type="hidden" id="txtCode" runat="server" />
            </div>
            <div class="form-group">
                <label for="exampleInputPassword1">
                    验证码</label>
                <div class="row">
                    <div class="col-xs-6 col-md-6">
                        <div class="form-group">
                            <label for="txtCheckCode" class="sr-only">
                                CheckCode</label>
                            <input type="text" id="txtCheckCode" class="form-control" runat="server" placeholder="验证码" />
                        </div>
                    </div>
                    <div class="col-xs-6 col-md-6">
                        <div class="form-group">
                            <label for="txtCheckCode" class="sr-only">
                                CheckCode</label>
                            <img alt="看不清楚，请点击换一张." style="cursor: pointer; height: 2.8em" src="<%=AppPath%>contrib/security/pages/checkCodeHandler.ashx"
                                onclick="this.src='<%=AppPath%>contrib/security/pages/checkCodeHandler.ashx?'+Math.random()" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="alert alert-danger" role="alert" id="divMessage" runat="server" style="display: none">
            </div>
            <asp:LinkButton CssClass="btn btn-info btn-block" ID="btnLogin" runat="server" Text="登 录"
                OnClick="btnLogin_Click" />
            <div class="container" style="padding: 45px 15px 15px">
                <p class="text-center">
                    Copyright @ 2014 ucsmy all rights reserved.
                </p>
            </div>
        </div>
        <script type="text/javascript">
            $(document).ready(function () {
                $("#<%=btnLogin.ClientID %>").click(function () {
                $("div[role='main']").showLoading(); setValue();
            });

            document.onkeydown = function (e) {
                var ev = document.all ? window.event : e;
                if (ev.keyCode == 13) {

                    document.getElementById("<%=btnLogin.ClientID %>").click();

                }
            }


        });

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
            // Check if a new cache is available on page load. 
        if (window.addEventListener) {
            window.addEventListener('load', function (e) {
                if (window.applicationCache) {
                    window.applicationCache.addEventListener('updateready', function (e) {
                        if (window.applicationCache.status == window.applicationCache.UPDATEREADY) {
                            // Browser downloaded a new app cache. 
                            // Swap it in and reload the page to get the new hotness. 
                            window.applicationCache.swapCache();
                            window.applicationCache.update();
                            if (confirm('网站有新版本. 是否更新?')) {
                                window.location.reload();
                            }
                        } else {
                            // Manifest didn't changed. Nothing new to server. 
                        }
                    }, false);
                }
            }, false);
        }
        </script>
    </form>
</body>
</html>
