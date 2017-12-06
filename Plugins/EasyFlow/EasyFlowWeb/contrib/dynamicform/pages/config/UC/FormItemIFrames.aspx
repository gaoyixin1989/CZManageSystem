<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_config_UC_FormItemIFrames" Codebehind="FormItemIFrames.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>//为[所属系统1][F10]设置扩展设置 </title>
</head>
<body style="repeat-y left;">
    <style>
        body
        {
            font-family: Arial, simsun;
            font-size: 12px;
            margin: 0;
            padding: 0;
            color: #333;
            overflow-y: scroll;
            overflow-y: inherit;
        }
        input, textarea, select, button
        {
            color: #000;
            margin: 0;
            vertical-align: middle;
        }
        form, ul, ol, li
        {
            margin: 0;
            padding: 0;
            list-style: none;
        }
        img
        {
            border: 0;
        }
        h1, h2, h3, h4, h5
        {
            margin: 0;
            padding: 0;
            font-size: 12px;
        }
        p
        {
            margin: 0 0 8px;
        }
        a:visited, a:link
        {
            text-decoration: underline;
            color: #333;
        }
        a:hover
        {
            color: #F50 !important;
            text-decoration: underline;
        }
        .clear
        {
            clear: both;
            height: 1px;
            font-size: 0;
            overflow: hidden;
        }
        .expand
        {
            width: 790px;
        }
        .right_e
        {
            float: left;
            margin-top: 20px;
        }
        .right_e dl
        {
            margin: 0;
            padding-bottom: 20px;
        }
        .right_e dt
        {
            margin: 0;
            font-weight: bold;
            color: #247ecf;
            padding-bottom: 10px;
            _padding-bottom: 6px;
        }
        .right_e dd
        {
            margin: 0;
            line-height: 18px;
        }
        .expand_input
        {
            width: 98%;
            border: 1px solid #cdcdcd;
        }
        table
        {
            width: 100%;
            border-collapse: collapse;
        }
        th
        {
            background: #f4f4f4;
            padding: 5px 8px;
            border: 1px solid #eaeaea;
            vertical-align: middle;
        }
        td
        {
            border: 1px solid #eaeaea;
            padding: 5px 8px;
        }
        .expand_btn
        {
            margin-top: 15px;
        }
        .expand_btn_a
        {
            background: url(<%=AppPath%>App_Themes/new/images/expand_btn.gif) repeat-x left top;
            height: 30px;
            line-height: 30px;
            border: none;
            text-align: center;
            width: 97px;
        }
        .expand_btn_b
        {
            background: url(<%=AppPath%>App_Themes/new/images/expand_btn.gif) repeat-x left -30px;
            height: 30px;
            line-height: 30px;
            border: none;
            text-align: center;
            width: 69px;
        }
    </style>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div class="right_e">
            <dl>
                <dt>嵌入页面设置<span style="color: Red"><asp:Literal ID="ltlInfo" runat="server"></asp:Literal></span></dt>
                <dd>
                    说明：<br />
                    参数："#WorkflowName#"(流程名称),<br />
                    "#Wiid#"(工单ID), "#Title#"(工单标题), "#SheetId#"(工单受理号),
                    <br />
                    "#CurrentUser#"(当前登录用户), "#DpId#"(当前用户部门ID),<br />
                    "#Aiid#"(当前步骤实例ID), "#ActivityName#"(流程活动步骤)。
                    <br />
                    例子1：http://XXX.html?No=#FName# <br />
                    说明：FName是本表单中的任意字段名，如F1，F2 ...<br />
                    例子2：http://XXX.html?No=#Wiid#&ID=#DpId# <br />
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 20%" rowspan="2">
                                <h2>公共窗体：</h2>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPubUrl" CssClass="expand_input" runat="server" ToolTip="输入URL"></asp:TextBox><br />
                                &nbsp;
                                
                            </td>
                            <td rowspan="4" style="width:15%">
                                <asp:Button ID="btnAdd" CssClass="expand_btn_b" Text="添加" runat="server" 
                                    onclick="btnAdd_Click"/>
                            </td>
                        </tr>
                        <tr>
                        <td style="width:40px;">高度：</td><td><asp:TextBox ID="txtPubH" CssClass="expand_input" runat="server" ToolTip="输入高度" onkeyup="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}" onafterpaste="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}"></asp:TextBox>
                        </td>
                        <td style="width:40px;">宽度：</td><td><asp:TextBox ID="txtPubW" CssClass="expand_input" runat="server" ToolTip="输入宽度" onkeyup="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}" onafterpaste="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                <asp:DropDownList ID="ddlActivityName" CssClass="expand_input" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtActivityName" CssClass="expand_input" runat="server" ToolTip="输入URL"></asp:TextBox><br />
                                
                            </td>
                        </tr>
                        <tr>
                        <td style="width:40px;">高度：</td><td><asp:TextBox ID="txtActH" CssClass="expand_input" runat="server" ToolTip="输入高度" onkeyup="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}" onafterpaste="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}"></asp:TextBox></td>
                         <td style="width:40px;">宽度：</td><td><asp:TextBox ID="txtActW" CssClass="expand_input" runat="server" ToolTip="输入宽度" onkeyup="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}" onafterpaste="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                            <asp:UpdatePanel ID="updatepanelActivities" runat="server" UpdateMode="Conditional"
                                    RenderMode="Inline">
                                    <ContentTemplate>
                                <asp:ListBox Rows = "10" CssClass="expand_input" ID="lblLibrary" runat="server" width="100%">
                                </asp:ListBox>
                                </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <input type="hidden" id="hidJson" runat="server" value="" />
                                <br />
                                <asp:Button ID="btnDelete" CssClass="expand_btn_b" runat="server" Text="删除" 
                                    onclick="btnDelete_Click" />
                            </td>
                        </tr>
                    </table>
                </dd>
                <div class="expand_btn">
                    <asp:Button ID="btnSave" Text=" 保存 " class="expand_btn_b" runat="server" 
                        OnClick="btnSave_Click" />&nbsp;
                    <asp:Button ID="btnSaveClose" Text=" 保存并关闭" class="expand_btn_a" runat="server" 
                        OnClick="btnSaveClose_Click" /></div>
            </dl>
        </div>
        <div class="clear">
        </div>
    </div>

    <script type="text/javascript" language="javascript">
        var arr = new Array();
        function selectValue() {
            //arr.push({});
            if (arr.length == 0) {

                if ($("#<%=txtPubUrl.ClientID %>").val() != "") {
                    var num = Math.random();
                    arr.push({ num: num, type: "0", url: $("#<%=txtPubUrl.ClientID %>").val() });
                    $("#<%=lblLibrary.ClientID %>").append("<option  num=\"" + num + "\" value=\"0\">公共Iframe：" + $("#<%=txtPubUrl.ClientID %>").val() + "</option>");
                }
                if ($("#<%=ddlActivityName.ClientID%>").get(0).selectedIndex > 0 && $("#<%=txtActivityName.ClientID %>").val() != "") {
                    var num = Math.random();
                    arr.push({ num: num, type: $("#<%=ddlActivityName.ClientID%>").val(), url: $("#<%=txtActivityName.ClientID %>").val() });
                    $("#<%=lblLibrary.ClientID %>").append("<option num=\"" + num + "\" value=\"" + $("#<%=ddlActivityName.ClientID%>").val() + "\">" + $("#<%=ddlActivityName.ClientID%>").val() + "：" + $("#<%=txtActivityName.ClientID %>").val() + "</option>");
                }
            }
            else {
                var caninsert1 = true;
                var caninsert2 = true;
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].type == "0") {
                        caninsert1 = false;
                    }
                    else if ($("#<%=ddlActivityName.ClientID%>").val() == arr[i].type) {
                        caninsert2 = false;
                    }
                }
                if (caninsert1 && $("#<%=txtPubUrl.ClientID %>").val() != "") {
                    var num = Math.random();
                    arr.push({ num: num, type: 0, url: $("#<%=txtPubUrl.ClientID %>").val() });
                    $("#<%=lblLibrary.ClientID %>").append("<option num=\"" + num + "\"  value=\"" + 0 + "\">公共Iframe：" + $("#<%=txtPubUrl.ClientID %>").val() + "</option>");
                }
                if (caninsert2 && $("#<%=ddlActivityName.ClientID %>").get(0).selectedIndex > 0 && $("#<%=txtActivityName.ClientID %>").attr("value") != "") {
                    var num = Math.random();
                    arr.push({ num: num, type: $("#<%=ddlActivityName.ClientID %>").val(), url: $("#<%=txtActivityName.ClientID %>").val() });
                    $("#<%=lblLibrary.ClientID %>").append("<option num=\"" + num + "\"  value=\"" + $("#<%=ddlActivityName.ClientID%>").val() + "\">" + $("#<%=ddlActivityName.ClientID%>").val() + "：" + $("#<%=txtActivityName.ClientID%>").val() + "</option>");
                }
            }
        }

        $(function() {
            $("#<%=btnAdd.ClientID %>").click(function() {
                if ($("#<%=txtPubUrl.ClientID %>").val() != "") {
                    if ($("#<%=txtPubH.ClientID %>").val() == "") {
                        alert("请输入高度。");
                        return false;
                    }
                    if ($("#<%=txtPubH.ClientID %>").val() < 0) {
                        alert("高度不能小于0。");
                        return false;
                    }
                    if ($("#<%=txtPubW.ClientID %>").val() == "") {
                        alert("请输入宽度。");
                        return false;
                    }
                    if ($("#<%=txtPubW.ClientID %>").val() < 0) {
                        alert("宽度不能小于0。");
                        return false;
                    }
                }
                if ($("#<%=ddlActivityName.ClientID %>").val() != "" && $("#<%=txtActivityName.ClientID %>").val() != "") {
                    if ($("#<%=txtActH.ClientID %>").val() == "") {
                        alert("请输入高度。");
                        return false;
                    }
                    if ($("#<%=txtActH.ClientID %>").val() < 0) {
                        alert("高度不能小于0。");
                        return false;
                    }
                    if ($("#<%=txtActW.ClientID %>").val() == "") {
                        alert("请输入宽度。");
                        return false;
                    }
                    if ($("#<%=txtActW.ClientID %>").val() < 0) {
                        alert("宽度不能小于0。");
                        return false;
                    }
                }
            });
        });
    </script>

    </form>
</body>
</html>
