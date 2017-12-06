<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_config_UC_FormItemsRules" Codebehind="FormItemsRules.aspx.cs" %>

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
                <dt>字段规则设置<span style="color: Red"><asp:Literal ID="ltlInfo" runat="server"></asp:Literal></span></dt>
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
                                <asp:HiddenField ID="hidFName" runat="server" />
                            </td>
                            <td>
                                输入项
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
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 30%">
                                当<span style="color: Red;"><asp:Literal ID="ltlFatherN" runat="server"></asp:Literal>
                                    <asp:Literal ID="ltlFatherF" runat="server"></asp:Literal></span>为
                            </td>
                            <td colspan="2">
                                <input type="text" id="txtFather" class="expand_input" />
                            </td>
                            <td rowspan="3" style="width:15%">
                                <input type="button" id="btnAdd" class="expand_btn_b" value="添加" onclick="selectValue()" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h2>
                                    字段名称：</h2>
                            </td>
                            <td style="width: 30%">
                                <asp:DropDownList ID="ddlCName" CssClass="expand_input" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <select id="ddlFType" class="expand_input">
                                    <option value="undo">不做任何操作</option>
                                    <option value="show">显示</option>
                                    <option value="hide">隐藏</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h2>
                                    ID名称：</h2>
                            </td>
                            <td>
                                <input type="text" id="txtIdName" class="expand_input" title="ID名称" />
                            </td>
                            <td>
                                <select id="ddlIDType" class="expand_input">
                                    <option value="undo">不做任何操作</option>
                                    <option value="show">显示</option>
                                    <option value="hide">隐藏</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <select size="10" class="expand_input" multiple="multiple" id="lblLibrary" width="100%">
                                </select>
                                <input type="hidden" id="hidJson" runat="server" value="" />
                                <br />
                                <input type="button" id="btnDelete" class="expand_btn_b" value="删除" />
                            </td>
                        </tr>
                    </table>
                </dd>
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
            if ($("#txtFather").val() != "") {
                var caninsert = true;
                if (arr.length > 0) {
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i].FName == $("#<%=hidFName.ClientID %>").val() && arr[i].FValue == $("#txtFather").val() && arr[i].CName == $("#<%=ddlCName.ClientID %>").val().split('$')[0] && arr[i].CType == $("#ddlFType").val() && arr[i].IdName == $("#txtIdName").val() && arr[i].IdType == $("#ddlIDType").val()) {
                            caninsert = false;
                            break;
                        }
                    }
                }
                if (caninsert) {
                    var num = Math.random();
                    arr.push({ Index: num, FName: $("#<%=hidFName.ClientID %>").val(), FValue: $("#txtFather").val(), CName: $("#<%=ddlCName.ClientID %>").val().split('$')[0], CType: $("#ddlFType").val(), CitemType: $("#<%=ddlCName.ClientID %>").val().split('$')[1], IdName: $("#txtIdName").val(), IdType: $("#ddlIDType").val() });
                    $("#lblLibrary").append("<option value=\"" + num + "\">当[" + $("#<%=hidFName.ClientID %>").val() + "]为" + "[" + $("#txtFather").val() + "]：[" + $("#<%=ddlCName.ClientID %>").val().split('$')[0] + "][" + $("#ddlFType").val() + "]：[" + $("#txtIdName").val() + "][" + $("#ddlIDType").val() + "]</option>");
                }
            }
        }

        function checkValue() {
            if ($("#<%=radWay_1.ClientID %>").attr("checked")) {
                if (arr.length > 0) {
                    var jsonStr = "[";
                    for (var i = 0; i < arr.length; i++) {
                        jsonStr += "{\"ParentName\":\"" + arr[i].FName + "\",\"ParentValue\":\"" + arr[i].FValue + "\",\"CName\":\"" + arr[i].CName + "\",\"CType\":\"" + arr[i].CType + "\",\"CitemType\":\"" + arr[i].CitemType + "\",\"IdName\":\"" + arr[i].IdName + "\",\"IdType\":\"" + arr[i].IdType + "\"},";
                    }
                    jsonStr = jsonStr.substr(0, jsonStr.length - 1);
                    jsonStr += "]";
                    $("#<%=hidJson.ClientID %>").attr("value", jsonStr);
                    return true;
                }
                else {
                    alert("没有增加规则。");
                    return false;
                }
            }
            else if ($("#<%=radWay_0.ClientID %>").attr("checked")) {
                return true;
            }
            else {
                alert("没有选择联动方式。");
                return false;
            }
            return true;
        }

        $(function() {
            var jsonstr = $("#<%=hidJson.ClientID %>").val();
            if (jsonstr != "" && jsonstr != 'undefined') {
                var json = eval(jsonstr);
                arr = new Array();
                for (var i = 0; i < json.length; i++) {
                    var num = Math.random();
                    var obj = json[i];
                    $("#lblLibrary").append("<option value=\"" + num + "\">当[" + obj.ParentName + "]为" + "[" + obj.ParentValue + "]：[" + obj.CName + "][" + obj.CType + "]：[" + obj.IdName + "][" + obj.IdType + "]</option>");
                    arr.push({ Index: num, FName: obj.ParentName, FValue: obj.ParentValue, CName: obj.CName, CType: obj.CType, CitemType:obj.CitemType, IdName: obj.IdName, IdType: obj.IdType });
                }
            }

            $("#btnDelete").click(function() {
                var temparr = new Array();
                $("#lblLibrary option:selected").each(function() {
                    $(this).remove();
                    //alert(arr.length);
                });
                $("#lblLibrary").each(function() {
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
        });
    </script>

    </form>
</body>
</html>
