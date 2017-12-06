<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_security_pages_Register" Codebehind="Register.aspx.cs" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="cphHead" runat="server">
    <title>用户注册 - 填写注册资料</title>
</head>
<body style="font-family:微软雅黑, Arial, 新宋体">
    <script type="text/javascript" src="../../../res/js/jquery-latest.pack.js"></script>
    <div class="topheader_wrapper">
        <div class="topHeader">
            <!--顶部导航 / TopNav-->
            <div class="topNav">
                广州市博汇数码科技有限公司
            </div>
            <!--头部 / Header-->
            <div id="header">
                <h1><span>广州市博汇数码科技有限公司</span></h1>
                <h2 class="xqp"> </h2>
            </div>
        </div>
    </div>
    
    <div class="mainContent" style="margin:5px 10px 5px 10px; background:none !important;">
        <div class="content2" style="padding:10px">
            <form id="form1" runat="server">
            <table border="0" cellspacing="0" cellpadding="0" align="center">
                <tr>
                    <td height="300" valign="top">
                        <div style="padding: 10px; height: 300px; width: 530px; margin: 50px 0px 10px 0px; text-align: left;
                            overflow: auto; background: #F0F6FC; color: #8096AA; border: 1px solid #74A8D6;">
                            <h4 class="content_sort" style="margin:5px">必填资料</h4>
                            <table id='table1' cellpadding="0" cellspacing="0" style="font-size:12px">
                                <tr>
                                    <td width="30%" align="right">帐号：</td>
                                    <td width="70%" align="left">
                                        <asp:TextBox ID="txtportal" runat="server"></asp:TextBox>
                                        <span style="color: #ff3300">*</span>
                                        <span id="_login1"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">密码：</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtpassword" TextMode="Password" runat="server"></asp:TextBox>
                                        <span style="color: #ff3300">*</span>
                                        <span id="_pwd1"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30%" align="right">确认密码：</td>
                                    <td width="70%" align="left">
                                        <asp:TextBox ID="txtpassword2" TextMode="Password" runat="server"></asp:TextBox>
                                        <span style="color: #ff3300">*</span>
                                        <span id="_pwd2"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">昵称：</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                        <span style="color: #ff3300">*</span>
                                        <span id="_nick1"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30%" align="right">E-Mail：</td>
                                    <td width="70%" align="left">
                                        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                        <span style="color: #ff3300">*</span>
                                        <span id="_mail1"></span>
                                    </td>
                                </tr>
                            </table>
                            <h4 class="content_sort" style="margin: 5px;">选填资料</h4>
                            <table id='table2' cellpadding="0" cellspacing="0" style="font-size:13px">
                                <tr>
                                    <td width="30%" align="right">手机号码：</td>
                                    <td width="70%" align="left">
                                        <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                                        <span id="_mobile"></span>
                                    </td>
                                </tr>
                                <tr class="trClass">
                                    <td align="right">固定电话：</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <div style="text-align: center">
                <asp:Button ID="Button1" runat="server" Text="注册" SkinID="reg" CssClass="btn" 
                    OnClientClick="return checkall();" onclick="Button1_Click" />
                <input id="Reset1" type="reset" value="清空" class="btn" />
            </div>
            </form>
        </div>
    </div>
    
    <div class="footer_wrapper">
        <div class="footer">
            版权所有 <a href="http://www.botwave.com">广州博汇数码科技有限公司</a>  <span >版本号：2.081210.1200</span>
        </div>
    </div>
    
    <script language="javascript" type="text/javascript">
          $(function() {
              if (parent && parent.topFrame) {
                  parent.topFrame.setNavBar($("#navbar").html());
              }
              $("input:text").each(function() {
                  this.className = "inputbox";
              });
              $("input:password").each(function() {
                  this.className = "inputbox";
              });
              $("input:password").focus(function() {
                  this.className = "inputbox_focus";
              });
              $("input:password").blur(function() {
                  this.className = "inputbox";
              });
              
              $("textarea").each(function() {
                  this.className = "inputbox";
              });
              $("input:text").focus(function() {
                  this.className = "inputbox_focus";
              });
              $("input:text").blur(function() {
                  this.className = "inputbox";
              });
              $("textarea").focus(function() {
                  this.className = "inputbox_focus";
              });
              $("textarea").blur(function() {
                  this.className = "inputbox";
              });
          });
    </script> 
    <script language="javascript" type="text/javascript">
        $(function() {
            $("#table1,#table2").addClass("tblClass").find("tr:odd").addClass("trClass").end().find("td span").addClass("tips").end().find("td input:not(intput[type=radio])").width("180px");
        });
        $(function() {
            $('#choose').toggle(
                function() { $('#infoadd').show(); $('#choose').attr("src", "<%=AppPath%>App_Themes/common/img/ico_close.gif").attr("alt", "收起"); },
                function() { $('#infoadd').hide(); $('#choose').attr("src", "<%=AppPath%>App_Themes/common/img/ico_open.gif").attr("alt", "显示"); }
            );
        });
        $(function() {
            $("#<%= txtportal.ClientID %>").focus(function() { 
                $("#_login1").html("英文数字4~20位.").css("color", "gray");
            });
            $("#<%= txtportal.ClientID %>").blur(function() { CheckPortal() });

            $("#<%= txtpassword.ClientID %>").focus(function() { 
                $("#_pwd1").html("密码6~12位.").css("color", "gray");
            });
            $("#<%= txtpassword.ClientID %>").blur(function() { CheckPWD(this.id, '_pwd1'); });

            $("#<%= txtpassword2.ClientID %>").focus(function() {
                $("#_pwd2").html("密码6~12位.").css("color", "gray");
             });
            $("#<%= txtpassword2.ClientID %>").blur(function() { CheckPWD(this.id, '_pwd2'); });

            $("#<%= txtEmail.ClientID %>").focus(function() { 
                $("#_mail1").html("找回密码的唯一途径.").css("color", "gray");
            });
            $("#<%= txtEmail.ClientID %>").blur(function() { EmailCheck(this.id) });

            $("#<%= txtName.ClientID %>").focus(function() {
                $("#_nick1").html("系统登陆后显示的姓名.").css("color", "gray");
            });
            $("#<%= txtName.ClientID %>").blur(function() { nameCheck(this.id); });

            $("#<%= txtMobile.ClientID %>").focus(function() { 
                $("#_mobile").html("11位手机号码.").css("color", "gray");
            });
            $("#<%= txtMobile.ClientID %>").blur(function() { mobileCheck(this.id); });
        });

        ////portal check
        function CheckPortal() {
            //check input
            var loginName = $('#<%= txtportal.ClientID %>').val();
            if (loginName == "") {
                $('#_login1').html("不能为空.").css("color", "red");
                return false;
            }
            if (/[^\u4e00-\u9fa5\w]/.test(loginName)) {
                $('#_login1').html("提示：不能输入非法字符").css("color", "red");
                return false;
            }
            if (!isChinese(loginName)) {
                $('#_login1').html("提示：只能英文、数字4~20位.").css("color", "red");
                return false;
            }
            if (loginName.length > 20 || loginName.length < 4) {
                $('#_login1').html("提示：长度是4~20位.").css("color", "red");
                return false;
            }
           
            //check loginName   
            $.get('RegisterCheck.ashx?name=' + escape(loginName),
            function(data) {
                if (data == "1") {
                    $('#_login1').html("√").css("color", "green");
                }
                else if (data == "0") {
                    $('#_login1').html("错误：已存在的用户").css("color", "red");
                    return false;
                }
                else {
                    $('#_login1').html("错误：服务连接超时").css("color", "red");
                    return false;
                }
            })
        }
        ////password Check
        function CheckPWD(txtid, pan) {
            var txt = $('#' + txtid).val();

            if (txt == "") {
                $('#' + pan).html("不能为空.").css("color", "red");
                return false;
            }

            if (!isChinese(txt)) {
                $('#' + pan).html("只能英文、数字6~12位.").css("color", "red");
                return false;
            }
            if (txt.length > 12 || txt.length < 6) {
                $('#' + pan).html("长度是6~12位.>").css("color", "red");
                return false;
            }
            if (pan == "_pwd2" && txt != $("#<%= txtpassword.ClientID %>").val()) {
                $('#' + pan).html("两次密码输入不统一.").css("color", "red");
                return false;
            }
            $('#' + pan).html("√").css("color", "green");
        }

        ////check email
        function EmailCheck(txtid) {
            var txt = $('#' + txtid).val();

            if (txt == "") {
                $('#_mail1').html("不能为空.").css("color", "red");
                return false;
            }
            if (!isEmail(txt)) {
                $('#_mail1').html("格式错误.").css("color", "red");
                return false;
            }
            $('#_mail1').html("√").css("color", "green");
        }
        ////namecheck
        function nameCheck(txtid) {
            var txt = $('#' + txtid).val();
            if (txt == "") {
                $('#_nick1').html("不能为空.").css("color", "red");
                return false;
            }
            if (txt.length > 12 || txt.length < 2) {
                $('#_nick1').html("长度是2~12位.").css("color", "red");
                return false;
            }
            $('#_nick1').html("√").css("color", "green");
        }
        ////mobileCheck
        function mobileCheck(txtid) {
            var txt = $('#' + txtid).val();
            if (txt == "") {
                $('#_mobile').html("");
                return;
            }
            if (txt.length != 11) {
                $('#_mobile').html("长度11位.").css("color", "red");
                return false;
            }
            $('#_mobile').html("√").css("color", "green");
        }

        //chn
        function isChinese(name) {
            if (name.length == 0)
                return true;
            for (i = 0; i < name.length; i++) {
                if (name.charCodeAt(i) > 128)
                    return false;
            }
            return true;
        }
        //email
        function isEmail(str) {
            if (/\w+@\w+(\.\w+)+/ig.test(str))
                return true;
            return false;
        }
        function checkall() {
            if ($('#_login1').html() == "√" && $('#_pwd1').html() == "√" && $('#_pwd2').html() == "√" && $('#_nick1').html() == "√" &&
             $('#_mail1').html() == "√") {
                return true;
            }
            else{
                alert("请将注册资料填写完整.");
                return false;
            }
        }        
    </script>
</body>    
</html>
