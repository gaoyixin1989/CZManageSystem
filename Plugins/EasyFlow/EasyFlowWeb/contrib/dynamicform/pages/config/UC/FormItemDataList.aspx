<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="contrib_dynamicform_pages_config_UC_FormItemDataList" Codebehind="FormItemDataList.aspx.cs" %>

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
            text-decoration: none;
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
                <dt>DataList设置<span style="color: Red"><asp:Literal ID="ltlInfo" runat="server"></asp:Literal></span></dt>
                <dd>
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <th style="width: 80px;">
                                动态添加行：
                            </th>
                            <td>
                                <asp:CheckBox runat="server" ID="chkNoLimit" Text="动态添加" />
                                
                            </td>
                            <th style="width: 80px;">
                                行数：
                            </th>
                            <td>
                                <asp:TextBox ID="txtPubH" CssClass="expand_input" runat="server" ToolTip="输入行数" onkeyup="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}"
                                    onafterpaste="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}"></asp:TextBox>
                                <asp:TextBox style="display:none" ID="txtPubW" CssClass="expand_input" runat="server" ToolTip="输入列数" onkeyup="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}"
                                    onafterpaste="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}"></asp:TextBox>
                            </td>
                            <td>
                                <input type="button" id="btnAdd" class="expand_btn_b" value="添加列" onclick="createTable()" />
                            </td>
                        </tr>
                        <tr>
                            <th colspan="5" style="margin: 0 0 0 0">
                                参数设置
                            </th>
                        </tr>
                        <tr>
                            <td colspan="5" style="margin: 0 0 0 0">
                                <div id="divTable" runat="server" style="height: 100%; width: 100%; overflow: auto;
                                    margin: 0 0 0 0">
                                    <table style="background-color: #fff; width:820px" id="tblList">
                                        <thead>
                                            <tr>
                                                <th style="width:40px">
                                                    序号
                                                </th>
                                                <th style="width:240px">
                                                    
                                                        列名
                                                </th>
                                                <th style="width:120px">
                                                    字段类型
                                                </th>
                                                <th style="width:120px">
                                                    数据源
                                                </th>
                                                <th style="width:80px">
                                                    是否必填
                                                </th>
                                                <%--<th style="width:80px">
                                                    验证类型
                                                </th>--%>
                                                <th style="width:120px">
                                                    错误信息
                                                </th>
                                                <th style="width:120px">
                                                    操作
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Literal ID="ltlTr" runat="server"></asp:Literal>
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </dd>
                <div class="expand_btn">
                    <asp:HiddenField ID="hidIntroduceJson" runat="server" Value="" />
                    <asp:Button ID="btnSave" Text=" 保存 " class="expand_btn_b" runat="server" OnClick="btnSave_Click" OnClientClick="return checkRow()" />&nbsp;
                    <asp:Button ID="btnSaveClose" Text=" 保存并关闭" class="expand_btn_a" runat="server" OnClick="btnSaveClose_Click" OnClientClick="return checkRow()" />&nbsp;
                    <asp:Button ID="btnDelete" Text=" 删除" class="expand_btn_b" runat="server" OnClick="btnDelete_Click" />&nbsp;</div>
            </dl>
        </div>
        <div class="clear">
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            if ($("#<%=chkNoLimit.ClientID %>").attr("checked")) {
                $("#<%=txtPubH.ClientID %>").val(-1);
                $("#<%=txtPubH.ClientID %>").hide();
            }
            else {
                $("#<%=txtPubH.ClientID %>").show();
            }
            $("#<%=chkNoLimit.ClientID %>").click(function () {
                if ($(this).attr("checked")) {
                    $("#<%=txtPubH.ClientID %>").val(-1);
                    $("#<%=txtPubH.ClientID %>").hide();
                }
                else {
                    $("#<%=txtPubH.ClientID %>").show();
                }
            })
        })

        var arr = new Array();
        function selectValue() {
            
        }

        function createTable() {
            //if (checkRow()) {
                var div = document.getElementById("<%=divTable.ClientID %>");
                var tableLength = 0;
                var insertCount = 0;
                var table;
                insertCount = $("#<%=txtPubW.ClientID %>").val();
                table = document.createElement("table"); //创建table
                /*if (div.innerHTML.length > 0) {
                    table = div.getElementsByTagName("table")[0];
                    tableLength = table.rows.length;
                    if (tableLength > $("#<%=txtPubW.ClientID %>").val()) {
                        for (var i = tableLength - 1; i >= $("#<%=txtPubW.ClientID %>").val(); i--) {
                            table.deleteRow(i);
                        }
                    }
                }
                else {

                    table = document.createElement("table"); //创建table
                    insertCount = $("#<%=txtPubW.ClientID %>").val();
                }
                //var row = table.insertRow(); //创建一行
                if (tableLength < $("#<%=txtPubW.ClientID %>").val()) {

                    insertCount = $("#<%=txtPubW.ClientID %>").val() - tableLength;
                }*/
                var index = 0;
                
                //div.innerHTML = "";
                for (var i = 0; i < 1; i++) {
                    var tblIntroduceLen = $("#tblList tr").length;

                    var rowTemplate = "<tr><td>"+tblIntroduceLen+"</td><td><input type='text' class='expand_input' name='add_bwdf_title_" + index + "' value=''/></td>"
                    + "<td><select class=\"expand_input\" name=\"add_bwdf_list_" + index + "\"><option value=\"0\">单行输入</option><option value=\"1\">多行输入</option><option value=\"6\">日期</option><option value=\"2\">下拉框</option><option value=\"4\">复选框</option><option value=\"5\">单选框</option></select></td>"
                    + "<td><input type='text' class='expand_input' name='add_source_" + index + "' value=''/></td>"
                    + "<td><input type='checkbox' name='chkRequire_" + i + "' value='true'/></td>"
                    //+ "<td><select class=\"expand_input\" name=\"add_ddlValidateType_" + index + "\"><option value=\"\">不需要</option><option value=\"require\">非空</option><option value=\"group\">必选</option></select></td>"
                    + "<td><input type='text' class='expand_input' name='add_errmsg_" + index + "' value=''/></td>"
                    + "<td><input type='hidden' class='expand_input' value='add'/><a action=\"up\" onclick=\"moveUp(this)\" href=\"javascript:void(0)\">上移</a>&nbsp;<a action=\"down\"  onclick=\"moveDown(this)\" href=\"javascript:void(0)\">下移</a>&nbsp;<a href='#' onclick='del(this)' a_del='1'>删除</a></td>";
                    + "</tr>";
                    var row = table.insertRow(); //创建一行

                    /*var cell = row.insertCell();  //创建一个单元
                    cell.width = "50px"; //更改cell的各种属性
                    cell.style.backgroundColor = "#999999";
                    cell.innerHTML = "<h2>列" + (i + 1) + "：</h2>";

                    var cell = row.insertCell();  //创建一个单元
                    cell.width = "100px"; //更改cell的各种属性
                    cell.style.backgroundColor = "#999999";
                    cell.innerHTML = "<h2>字段意义：</h2>";

                    var cell = row.insertCell();  //创建一个单元
                    cell.width = "200px"; //更改cell的各种属性
                    cell.style.backgroundColor = "#999999";
                    var input = document.createElement("input");
                    input.type = "text";
                    input.className = "expand_input"
                    input.name = "bwdf_title_" + index;
                    cell.appendChild(input);

                    var cell = row.insertCell();  //创建一个单元
                    cell.width = "100px"; //更改cell的各种属性
                    cell.style.backgroundColor = "#999999";
                    cell.innerHTML = "<h2>字段类型：</h2>";

                    cell = row.insertCell();  //创建一个单元 
                    cell.width = "250px"; //更改cell的各种属性
                    cell.style.backgroundColor = "#999999";
                    var select = document.createElement("select");
                    select.className = "expand_input";
                    var option = document.createElement("option");
                    select.id = "bwdf_list_" + index;
                    select.name = "bwdf_list_" + index;
                    option.value = "Text";
                    option.innerHTML = "输入框";
                    select.appendChild(option);
                    option = document.createElement("option");
                    option.value = "Date";
                    option.innerHTML = "日期";
                    select.appendChild(option);
                    option = document.createElement("option");
                    option.value = "CheckBoxList";
                    option.innerHTML = "复选框";
                    select.appendChild(option);
                    option = document.createElement("option");
                    option.value = "DropDownList";
                    option.innerHTML = "下拉框";
                    select.appendChild(option);
                    option = document.createElement("option");
                    option.value = "RadioButtonList";
                    option.innerHTML = "单选框";
                    select.appendChild(option);
                    cell.appendChild(select);*/
                    $("#tblList tbody").append(rowTemplate);
                    //index++;
                    index++
                }
                //div.innerHTML.length = 0;
                //div.appendChild(table);
            //}
            }

            function moveUp(obj) {
                var tr = $(obj).parent().parent();
                var sort = tr.find("td").eq(0).html()
                if (sort > 1) {
                    var prevTr = tr.prev();
                    tr.insertBefore(prevTr);
                    var trlist = $("#tblList tr");
                    for (var i = 1; i < trlist.length; i++) {
                        $(trlist[i]).find("td").eq(0).html(i);
                    }
                }
            }

            function moveDown(obj) {
                var tr = $(obj).parent().parent();
                var sort = tr.find("td").eq(0).html()
                var tblIntroduceLen = $("#tblList tr").length;
                if (sort < tblIntroduceLen) {
                    var nextTr = tr.next();
                    tr.insertAfter(nextTr);
                    var trlist = $("#tblList tr");
                    for (var i = 1; i < trlist.length; i++) {
                        $(trlist[i]).find("td").eq(0).html(i);
                    }
                }
            }

        function del(obj) {
            if ($(obj).attr("a_del")) {
                //i--;
            }

            obj.parentNode.parentNode.parentNode.removeChild(obj.parentNode.parentNode);
        }
        function checkRow() {
            if ($("#<%=txtPubH.ClientID %>").val() == "" && !$("#<%=chkNoLimit.ClientID %>").attr("checked")) {
                alert("请输入行数。");
                return false;
            }
            if ($("#<%=txtPubH.ClientID %>").val() < 0 && !$("#<%=chkNoLimit.ClientID %>").attr("checked")) {
                alert("行数不能小于0。");
                return false;
            }
            var canAdd=true;
            $("divTable input").each(function(){
                if($(this).val() == "")
                canAdd=false;
            });
            if (!canAdd) {
                //alert("字段定义不能为空。");
                //return false;
            }


            var trIntroduceList = $("#tblList tr");
            if (trIntroduceList.length < 2) {
                alert("列定义不能为空。");
                return false;
            }
            var typeid = $("#hidTypeId").val();
            var sbIntroduce = new Array();
            for (var i = 1; i < trIntroduceList.length; i++) {
                var tr = trIntroduceList[i];
                var sort = $(tr).find("td").eq(0).html();
                var title = $(tr).find("td").eq(1).find("input").eq(0).val();
                if (title == "") {
                    alert("字段定义不能为空。");

                    return false;
                }
                var itemtype = $(tr).find("td").eq(2).find("select").eq(0).val();
                var datasource = $(tr).find("td").eq(3).find("input").eq(0).val();
                var require = $(tr).find("td").eq(4).find("input").eq(0).attr("checked") ? "true" : "false";
                //var validatetype = $(tr).find("td").eq(5).find("select").eq(0).val();
                var id = $(tr).find("td").eq(6).find("input").eq(0).val();
                var errmsg = $(tr).find("td").eq(5).find("input").eq(0).val();
                sbIntroduce.push("{\"id\":\"" + id + "\",\"sort\":\"" + sort + "\",\"title\":\"" + title + "\",\"itemtype\":\"" + itemtype + "\",\"datasource\":\"" + datasource + "\",\"require\":\"" + require + "\",\"validatetype\":\"\", \"errmsg\":\""+errmsg+"\"}");
                $("#<%=txtPubW.ClientID %>").attr("value", i);
            }
            
            $("#<%=hidIntroduceJson.ClientID %>").val("[" + sbIntroduce.join(",") + "]");

            return true;
        }
    </script>
    </form>
</body>
</html>
