<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="contrib_dynamicform_pages_config_UC_FormItemLinkage" Codebehind="FormItemLinkage.aspx.cs" %>

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
    <asp:ScriptManager ID="scriptManager1" runat="server"></asp:ScriptManager>
    <div>
        <div class="right_e">
            <dl>
                <dt>级联字段<span style="color:Red"><asp:Literal ID="ltlInfo" runat="server"></asp:Literal></span></dt>
                <dd>
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <th width="18%">
                                项目
                            </th>
                            <th width="30%">
                                采集
                            </th>
                            <th width="30%">
                                说明
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <h2>
                                    主菜单：</h2>
                            </td>
                            <td>
                                <asp:Literal ID="ltlName" runat="server"></asp:Literal>&nbsp;-&nbsp;<asp:Literal
                                    ID="ltlFName" runat="server"></asp:Literal>
                            </td>
                            <td>
                                输入项
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h2>
                                    联动项：</h2>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFName" CssClass="expand_input" runat="server" 
                                    onselectedindexchanged="ddlFName_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                要实现联动的字段
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
            <dl>
                <dt>联动方式</dt>
                <dt>
                    <asp:RadioButton ID="radWay_0" Text="不做任何设置。" runat="server" GroupName="pub" /></dt>
                <dd>
                不做任何设置
                </dd>
                <dt>
                    <asp:RadioButton ID="radWay_1" Text="通过选项联动。" runat="server" GroupName="pub" /></dt>
                <dd>
                    说明：<br />
                    1、选择联动字段</br>
                    2、选择或在输入框中填写主菜单的输入项</br>
                    3、选择或在输入框中填写联动字段的输入项</br>
                    4、如果联动字段的输入项选择“{GetOuterData}”，则表示联动字段从外部数据源中获取数据，数据获取配置请到“数据获取中”配置</br>
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 35%">
                                当<span style="color: Red;"><asp:Literal ID="ltlFatherN" runat="server"></asp:Literal>
                                    <asp:Literal ID="ltlFatherF" runat="server"></asp:Literal></span>为
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFather" runat="server" CssClass="expand_input">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtFather" runat="server" CssClass="expand_input"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="width: 35%">
                                <asp:UpdatePanel ID="updatepanelActivities" runat="server" UpdateMode="Conditional"
                                    RenderMode="Inline">
                                    <ContentTemplate>
                                        <span style="color: Red;">
                                            <asp:Literal ID="ltlChildrenN" runat="server"></asp:Literal>
                                            <asp:Literal ID="ltlChildrenF" runat="server"></asp:Literal></span>为
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlFName" EventName="selectedindexchanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlChildren" runat="server" onchange="setChildren()" CssClass="expand_input">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtChildren" runat="server" CssClass="expand_input"></asp:TextBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlFName" EventName="selectedindexchanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <input type="button" id="btnAdd" class="expand_btn_b" value="添加" onclick="selectValue()" />
                                <input type="button" id="btnDelete" class="expand_btn_b" value="删除" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <select size="10" class="expand_input" multiple="multiple" id="lblLibrary" width="100%">
                                </select>
                                <input type="hidden" id="hidJson" runat="server" value="" />
                            </td>
                        </tr>
                    </table>
                </dd>
                <dt style="display:none">
                    <asp:RadioButton ID="radWay_2" Text="通过SQL联动。" runat="server" GroupName="pub" /></dt>
                <dd>
                    <span style="display:none">例子：Select Addr From 商品表 WHERE No=#FName# <br />
                    说明：FName是本表单中的任意字段名，如F1，F2 ...<br />
                    数据源输入为SQLServer数据库连接字符串，不填表示本地数据库。<br /></span>
                    <table border="0" cellspacing="0" cellpadding="0" style="display:none">
                        <tr>
                            <th width="18%">
                                SQL语句
                            </th>
                            <td width="82%">
                                <textarea name="txtSQL" rows="5" class="expand_input" id="txtSQL" runat="server"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                数据源
                            </th>
                            <td>
                                <asp:TextBox ID="txtSqlConnection" runat="server" CssClass="expand_input"></asp:TextBox>
                            </td>
                        </tr>
                    </table></dd>
                    
                <div class="expand_btn">
                    <asp:Button ID="btnSave" Text=" 保存 " class="expand_btn_b" runat="server" OnClientClick="return checkValue();"
                        OnClick="btnSave_Click" />&nbsp;
                    <asp:Button ID="btnSaveClose" Text=" 保存并关闭" class="expand_btn_a" runat="server" OnClientClick="return checkValue();"
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
            if ($("#<%=ddlFather.ClientID %>").val() != "") {
                var caninsert = true;
                if (arr.length > 0) {
                    for (var i = 0; i < arr.length; i++) {
                        //if (arr[i].FValue == $("#<%=ddlFather.ClientID %> option:selected").text() && arr[i].CValue == $("#<%=ddlChildren.ClientID %> option:selected").text()) {
                        if (arr[i].FValue == $("#<%=txtFather.ClientID %>").val() && arr[i].CValue == $("#<%=txtChildren.ClientID %>").val() && arr[i].CName == $("#<%=ddlChildren.ClientID %>").val()) { 
                            caninsert = false;
                            break;
                        }
                        if (arr[i].FValue == $("#<%=txtFather.ClientID %>").val() && arr[i].CValue == "{GetOuterData}" && arr[i].CName == $("#<%=ddlChildren.ClientID %>").val()) {
                            caninsert = false;
                            alert("该字段已设置为从外部数据源中获取数据");
                            break;
                        }
                    }
                }
                if (caninsert) {
                    var num=Math.random();
                    //arr.push({ Index: num, FName: $("#<%=ddlFather.ClientID %>").val(), FValue: $("#<%=ddlFather.ClientID %> option:selected").text(), CName: $("#<%=ddlChildren.ClientID %>").val(), CValue: $("#<%=ddlChildren.ClientID %> option:selected").text() });
                    //$("#lblLibrary").append("<option value=\"" + num + "\">当[" + $("#<%=ddlFather.ClientID %>").val() + "]为" + "[" + $("#<%=ddlFather.ClientID %> option:selected").text() + "]：[" + $("#<%=ddlChildren.ClientID %>").val() + "]为[" + $("#<%=ddlChildren.ClientID %> option:selected").text() + "]</option>");
                    arr.push({ Index: num, FName: $("#<%=ddlFather.ClientID %>").val(), FValue: $("#<%=txtFather.ClientID %>").val(), CName: $("#<%=ddlChildren.ClientID %>").val(), CValue: $("#<%=txtChildren.ClientID %>").val() });
                    $("#lblLibrary").append("<option value=\"" + num + "\">当[" + $("#<%=ddlFather.ClientID %>").val() + "]为" + "[" + $("#<%=txtFather.ClientID %>").val() + "]：[" + $("#<%=ddlChildren.ClientID %>").val() + "]为[" + $("#<%=txtChildren.ClientID %>").val() + "]</option>");
                } 
            }
        }

        function checkValue() {
            if ($("#<%=radWay_1.ClientID %>").attr("checked")) {
                if (arr.length > 0) {
                    var jsonStr = "[";
                    for (var i = 0; i < arr.length; i++) {
                        jsonStr += "{\"ParentName\":\"" + arr[i].FName + "\",\"Parent\":\"" + arr[i].FValue + "\",\"FName\":\"" + arr[i].CName + "\",\"FValue\":\"" + arr[i].CValue + "\"},";

                    }
                    jsonStr = jsonStr.substr(0, jsonStr.length - 1);
                    jsonStr += "]";
                    $("#<%=hidJson.ClientID %>").attr("value", jsonStr);
                    //alert($("#<%=hidJson.ClientID %>").val());
                    return true;
                }
                else {
                    if ($("#<%=ddlFName.ClientID %>").val != "") {
                        alert("没有设置联动信息。");
                        return false;
                    }
                }
            }
            else if ($("#<%=radWay_2.ClientID %>").attr("checked")) {
                return true;
            }
            else if ($("#<%=radWay_0.ClientID %>").attr("checked")) {
                return true;
            }
            else{
                alert("没有选择联动方式。");
                return false;
            }
            return true;
        }

        function setChildren() {
            var cVal = $("#<%=ddlChildren.ClientID %> option:selected").text();
            $('#<%=txtChildren.ClientID %>').val(cVal)
            if (cVal == "{GetOuterData}") {
                $('#<%=txtChildren.ClientID %>').attr("onfocus", "this.blur()");
                $('#<%=txtChildren.ClientID %>').attr("readonly", "readonly");
            }
            else {
                $('#<%=txtChildren.ClientID %>').removeAttr("onfocus");
                $('#<%=txtChildren.ClientID %>').removeAttr("readonly");
            }
           
        }
        $(document).ready(function () {
            var jsonstr = $("#<%=hidJson.ClientID %>").val();
            if (jsonstr != "" && jsonstr != 'undefined') {
                var json = eval(jsonstr);
                arr = new Array();
                for (var i = 0; i < json.length; i++) {
                    var num = Math.random();
                    var obj = json[i];
                    $("#lblLibrary").append("<option value=\"" + num + "\">当[" + obj.ParentName + "]为" + "[" + obj.Parent + "]：[" + obj.FName + "]为[" + obj.FValue + "]</option>");
                    arr.push({ Index: num, FName: obj.ParentName, FValue: obj.Parent, CName: obj.FName, CValue: obj.FValue });
                }
            }

            $("#btnDelete").click(function () {
                var temparr = new Array();
                $("#lblLibrary option:selected").each(function () {
                    $(this).remove();
                    //alert(arr.length);
                });
                $("#lblLibrary").each(function () {
                    var caninsert = false;
                    var a = [];
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i].Index == $(this).val()) {
                            caninsert = true;
                            a = arr[i];
                            break;
                        }
                    }
                    if (caninsert)
                        temparr.push(a);
                });
                arr = temparr;
            });

            $('#<%=ddlFather.ClientID %>').change(function(){
                var fVal = $("#<%=ddlFather.ClientID %> option:selected").text()
                $('#<%=txtFather.ClientID %>').val(fVal)
            });
            $('#<%=ddlChildren.ClientID %>').change(function(){
                var cVal = $("#<%=ddlChildren.ClientID %> option:selected").text()
                $('#<%=txtChildren.ClientID %>').val(cVal)
            })
        });
    </script>

    </form>
</body>
</html>
