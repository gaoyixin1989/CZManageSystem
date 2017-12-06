<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_pms_pages_UCS_FromManagerEdit" Codebehind="UCS_FromManagerEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <%--   <link href="<%=AppPath%>App_Themes/gmcc/icon.css" rel="stylesheet" type="text/css" />--%>
    <%--<link href="<%=AppPath%>App_Themes/gmcc/easyui.css" rel="stylesheet" type="text/css" />--%>
    <script src="../../../res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <%--<link href="<%=AppPath%>javascript/libs/skins/blue/style.css" rel="stylesheet" type="text/css" />--%>
    <script src="../../../res/js/jquery_custom.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function keyPress(obj) {
            if (!obj.value.match(/^[\+\-]?\d*?\.?\d*?$/)) {
                obj.value = '';
            }
            else {
                obj.t_value = obj.value;
            }
            if (obj.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/)) {
                obj.o_value = obj.value;
            }
        }

        function keyUp(obj) {

            if (!obj.value.match(/^[\+\-]?\d*?\.?\d*?$/)) {
                obj.value = '';
            } else {
                obj.t_value = obj.value;
            }
            if (obj.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/)) {
                obj.o_value = obj.value;
            }
        }

        function blur(obj) {
            if (!obj.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?|\.\d*?)?$/)) {
                obj.value = '';
            } else {
                if (obj.value.match(/^\.\d+$/)) {
                    obj.value = 0 + obj.value;
                }
                if (obj.value.match(/^\.$/)) {
                    obj.value = 0;
                    obj.o_value = obj.value;
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <script>
    </script>
    <%--<script src="../../../javascript/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>--%>
    <%--<script src="<%=AppPath%>javascript/libs/js/framework.js" type="text/javascript"></script>--%>
    <script src="<%=AppPath%>res/js/ChangeDate.js" type="text/javascript"></script>
    <link href="<%=AppPath%>res/js/jquery.editable-select.css"
        rel="stylesheet" type="text/css" />
    <script src="<%=AppPath%>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.js"
        type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.pack.js"
        type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/base64.js"
        type="text/javascript"></script>
    <script>        tableName = "bw_users";</script>
    <div class="">
        <div class="showControl">
            <h4>
            </h4>
        </div>
        <table class="tblGrayClass grayBackTable" cellspacing="1" style="margin-top: 6px; table-layout:fixed">
            <tr>
                <th style="width:13%">
                    报表名称
                </th>
                <td style="text-align: left">
                    <asp:TextBox ID="formname" Width="200" runat="server" MaxLength="50"></asp:TextBox><span><font
                        color="#FF0000">*</font></span>
                </td>
                <%--<th>
                  选择产品
                </th>
                <td style="text-align: left">
                    <asp:DropDownList ID="txtProduct" runat="server">
                    </asp:DropDownList>
                </td>--%>
                <th style="width:13%">
                    报表类型
                </th>
                <td style="text-align: left">
                    <select runat="server" id="type">
                        <option value="0">普通表(无任何权限过滤)</option>
                        <option value="3">权限统计报表 </option>
                    </select><span><font color="#FF0000"> *</font></span>
                </td>
            </tr>
            <tr>
                <th>
                    对应数据库表
                </th>
                <td style="text-align: left">
                    <select id="datasource" datasourceedite='1' vid="<%=Reportforms.datasource%>" name="datasource" onchange="getField(this);changedata();"
                         class="editable-select">
                        <optgroup label="---------表----------">
                            <%=sb.ToString() %></optgroup><optgroup label="----------视图---------"><%=sb2.ToString() %></optgroup>
                    </select>
                </td>
                <th>
                    排序条件
                </th>
                <td style="text-align: left">
                    <asp:TextBox ID="strOrder" Width="200" runat="server"></asp:TextBox><span><font color="#FF0000">
                        *</font></span>
                </td>
            </tr>
            <tr>
                <th>
                    分组字段
                </th>
                <td style="text-align: left">
                    <asp:TextBox ID="strGroup" Width="200" runat="server"></asp:TextBox>
                </td>
                <th>
                    权限过滤条件
                   
                </th>
                <td style="text-align: left">
                    <asp:TextBox ID="strRule" Width="200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    合计行展现
                </th>
                <td style="text-align: left">
                    <select runat="server" id="counttype"><option value="0">否</option><option value="1">是</option></select>
                </td>
                <th>
                    查询条件
                </th>
                <td style="text-align: left">
                    &nbsp;<asp:RadioButton ID="radAnd" runat="server" Text="And" GroupName="condition" />&nbsp;
                    <asp:RadioButton ID="radOr" runat="server" Text="Or" GroupName="condition" />
                     &nbsp;
                    <asp:TextBox ID="strWhere" Width="200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th style="width:13%">
                    报表对应字段
                </th>
                <td colspan="3" style="width:87%">
                    <div class="libox" style="width: 100%; max-height: 400px; overflow: scroll">
                        <input value="新增" type="button" style="float: left" onclick="add()">
                        <table id="tb_fromfield" class="tblGrayClass grayBackTable" style=" table-layout:fixed">
                            <tr>
                                <th style="width:150px">
                                    字段名称
                                </th>
                                <th style="width:230px">
                                    字段值
                                </th>
                                <th style="width:60px">
                                    排序
                                </th>
                                <th style="width:60px">
                                    是否查询
                                </th>
                                <th style="width:60px">
                                    是否显示
                                </th>
                                <th style="width:70px">
                                    是否查询条件
                                </th>
                                <th style="width:150px">
                                    字段类型
                                </th>
                                <th style="width:90px">
                                    作为查询条件的数据源
                                </th>
                                <th style="width:100px">
                                    查询条件显示的字段文本值
                                </th>
                                <th style="width:100px">
                                    查询条件显示的字段值
                                </th>
                                <th style="width:100px">
                                    查询字段过滤语句
                                </th>
                                <th style="width:70px">
                                    查询条件控件的位置
                                </th>
                                <th style="width:70px">
                                    查询条件控件位置的排序
                                </th>
                                <th style="width:70px">
                                    查询条件是否有组织控制
                                </th>
                                <th style="width:60px">
                                    能否统计
                                </th>
                                <th style="width:60px">
                                    计算方式
                                </th>
                                <th style="width:220px">
                                    跳转的链接地址
                                </th>
                                <th style="width:120px">
                                    传递的参数名称(,隔开)
                                </th>
                                <th style="width:40px">
                                    操作
                                </th>
                            </tr>
                            <asp:Repeater ID="fieldlist" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <input value='<%#Eval("fieldname")%>' name='<%#Eval("id") %>_fieldname' style="width: 90%" />
                                        </td>
                                        <td style="text-align: left">
                                            <%-- <input value='<%#Eval("field")%>' name='<%#Eval("id") %>_field' style="width: 100%" />--%>
                                            <select edite='1' vid="<%#HttpUtility.HtmlEncode(Eval("field").ToString())%>" name='<%#Eval("id") %>_field' id="<%#Eval("id") %>"
                                                style="width: 90%" class="editable-select">
                                                <%=sb3.ToString() %>
                                            </select>
                                        </td>
                                        <td>
                                            <input value='<%#Eval("fieldorder")%>' onkeyup="this.value=this.value.replace(/[^\d+.]/g,'')"
                                                name='<%#Eval("id") %>_fieldorder' style="width: 90%" />
                                        </td>
                                        <td>
                                            <select name='<%#Eval("id") %>_IsSelect' vid='<%#Eval("IsSelect") %>' style='width: 90%'>
                                                <option value='0'>否</option>
                                                <option value='1'>是</option>
                                            </select>
                                        </td>
                                        <td>
                                            <select name='<%#Eval("id") %>_IsShow' vid='<%#Eval("IsShow") %>' style='width: 90%'>
                                                <option value='0'>否</option>
                                                <option value='1'>是</option>
                                            </select>
                                        </td>
                                        <td>
                                            <select vid='<%#Eval("whereStrtype") %>' name='<%#Eval("id") %>_whereStrtype' style="width: 90%"
                                                onchange='getSerch(this);' type="whereStrtype">
                                                <option value="0">非查询条件</option>
                                                <option value="1">普通查询条件</option>
                                            </select>
                                        </td>
                                        <td>
                                            <select vid='<%#Eval("Fieldtype") %>' name='<%#Eval("id") %>_Fieldtype' style="width: 95%">
                                                <option value="1">字符串</option>
                                                <option value="2">日期(年-月)</option>
                                                <option value="3">日期(年-月-日)</option>
                                                <option value='8'>单选日期(年)</option>
                                                <option value='6'>单选日期(年-月)</option>
                                                <option value='7'>多选日期(年-月-日)</option>
                                            </select>
                                        </td>
                                        <td>
                                            <select vid='<%#Eval("datasource") %>' name='<%#Eval("id") %>_datasource' style="width: 90%">
                                                <optgroup label="---------表----------">
                                                    <%=sb.ToString() %></optgroup><optgroup label="----------视图---------"><%=sb2.ToString() %></optgroup>
                                            </select>
                                        </td>
                                        <td>
                                            <input value='<%#Eval("whereFieldText")%>' name='<%#Eval("id") %>_whereFieldText'
                                                style="width: 90%" />
                                        </td>
                                        <td>
                                            <input value='<%#Eval("whereFieldValue")%>' name='<%#Eval("id") %>_whereFieldValue'
                                                style="width: 90%" />
                                        </td>
                                        <td>
                                            <input value='<%#Eval("strWhere")%>' name='<%#Eval("id") %>_strWhere' style="width: 100%" />
                                        </td>
                                        <td>
                                            <select vid='<%#Eval("wherePositionType") %>' name='<%#Eval("id") %>_wherePositionType'
                                                style="width: 90%">
                                                <option value="1">上</option>
                                                <option value="2">下</option>
                                            </select>
                                        </td>
                                        <td>
                                            <input value='<%#Eval("whereOrder")%>' onkeyup="this.value=this.value.replace(/[^\d+.]/g,'')"
                                                name='<%#Eval("id") %>_whereOrder' style="width: 90%" />
                                        </td>
                                        <td>
                                            <select name='<%#Eval("id") %>_isorganization' vid='<%#Eval("isorganization") %>'
                                                style='width: 100%'>
                                                <option value='0'>否</option>
                                                <option value='1'>是</option>
                                            </select>
                                        </td>
                                        <td>
                                            <select name='<%#Eval("id") %>_IsCount' vid='<%#Eval("IsCount") %>' style='width: 90%'>
                                                <option value='0'>否</option>
                                                <option value='1'>是</option>
                                            </select>
                                        </td>
                                        <td>
                                            <select name='<%#Eval("id") %>_StatisticsType' vid='<%#Eval("StatisticsType") %>' style='width: 90%'>
                                                <option value='0'>请选择</option>
                                                <option value='1'>累加</option>
                                                <option value='2'>平均数</option>
                                            </select>
                                        </td>
                                        <td>
                                            <input value='<%#Eval("LinkUrl")%>' name='<%#Eval("id") %>_LinkUrl' style="width: 90%" />
                                        </td>
                                        <td>
                                            <input value='<%#Eval("parameter")%>' name='<%#Eval("id") %>_parameter' style="width: 90%" />
                                        </td>
                                        <td>
                                            <a href="#" onclick="del(this)">删除</a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <th style="width:13%">
                    字段说明
                </th>
                <td colspan="3">
                    <div>
                        <table class="tblClass" cellspacing="1" style="margin-top: 6px;" id="field_remark">
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button ID="buttonOK" runat="server" Text="保存" CssClass="btn_sav" OnClick="buttonOK_Click"
                        OnClientClick="return check();" />
                    <input type="button" value="返回" class="btnReturnClass" style="margin-left: 6px" onclick="window.location='UCS_FromManager.aspx';" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function del(obj) {
            if ($(obj).attr("a_del")) {
                index--;
            }

            obj.parentNode.parentNode.parentNode.removeChild(obj.parentNode.parentNode);
        }
        var index = 1;
        function add() {
            var d_order = $("#tb_fromfield").find("tr").length;
            var tr = $("<tr><td><input style='width: 100%' name='add_" + index + "_fieldname'/></td>"
            +"<td><select edite='1' class='editable-select' style='width:100%' name='add_" + index + "_field'></select></td>"
            +"<td><input style='width:100%' name='add_" + index + "_fieldorder' onkeyup=\"this.value=this.value.replace(/[^\\d+.]/g,'')\" value='" + d_order + "' /></td>"
            + "<td><select name='add_" + index + "_IsSelect' style='width:100%'><option value='1'>是</option><option value='0'>否</option></select></td>"
            +"<td><select name='add_" + index + "_IsShow' style='width:100%'><option value='1'>是</option><option value='0'>否</option></select></td>"
            +"<td><select name='add_" + index + "_whereStrtype' style='width:100%' onchange='getSerch(this)' type='whereStrtype' ><option value='0'>非查询条件</option><option value='1'>普通查询条件</option></select></td>"
            + "<td><select name='add_" + index + "_Fieldtype' style='width:100%'> <option value='1'>字符串</option> <option value='2'>日期(年-月)</option><option value='3'>日期(年-月-日)</option><option value='4'>预警字段</option><option value='8'>单选日期(年)</option><option value='6'>单选日期(年-月)</option><option value='7'>多选日期(年-月-日)</option></select></td>"
            +"<td><select name='add_" + index + "_datasource' style='width:100%' ><optgroup label='---------表----------'><%=sb.ToString()%></optgroup><optgroup label='----------视图---------'><%=sb2.ToString() %></optgroup></select></td>"
            +"<td><input style='width: 100%' name='add_" + index + "_whereFieldText'/></td><td><input style='width:100%' name='add_" + index + "_whereFieldValue'/></td><td><input style='width: 100%' name='add_" + index + "_strWhere'/></td>"
            + "<td><select name='add_" + index + "_wherePositionType' style='width:100%'> <option value='1'>上</option><option value='2'>下</option></select></td><td><input style='width:100%' name='add_" + index + "_whereOrder' onkeyup=\"this.value=this.value.replace(/[^\\d+.]/g,'')\"/></td>"
            + "<td><select name='add_" + index + "_isorganization' style='width:100%'><option value='0'>否</option><option value='1'>是</option></select></td>"
            + "<td><select name='add_" + index + "_IsCount' style='width: 100%'><option value='0'>否</option><option value='1'>是</option></select></td>"
            + "<td><select name='add_" + index + "_StatisticsType' style='width: 100%'><option value='0'>请选择</option><option value='1'>累加</option><option value='2'>平均数</option></select></td>"
            + "<td><input name='add_" + index + "_LinkUrl' style='width: 100%' /></td>"
            +"<td><input name='add_" + index + "_parameter' style='width: 100%' /></td>"
            +"<td><a href='#' onclick='del(this)' a_del='1'>删除</a></td></tr>").appendTo($("#tb_fromfield"));

            tr.find(".editable-select").editableSelect(
           {
               onSelect: function (list_item) {
                   this.select.val(this.text.val());
               }
           }

    );
            getField(document.getElementById("datasource"), tr.find(".editable-select")[0]);
            tr.find(".editable-select").css("width", "200px");
            index++;
        }

        function getSerch(obj) {
            var v = $("#datasource").val();

            var str = $(obj).attr("name").split("_");
            var head_str = str.length == 2 ? str[0] + "_" : str[0] + "_" + str[1] + "_";

            $("select[name='" + head_str + "datasource" + "']").val(v);
            var field = document.getElementsByName(head_str + "field")[0].value;
            if ($("input[name='" + head_str + "whereFieldText" + "']").val().length == 0)
                $("input[name='" + head_str + "whereFieldText" + "']").attr("value",field);
            if ($("input[name='" + head_str + "whereFieldValue" + "']").val().length == 0)
                $("input[name='" + head_str + "whereFieldValue" + "']").attr("value", field);

        }

        function check() {
            if (document.getElementById("<%=formname.ClientID%>").value == "") {
                alert("报表名称名称必填！");
                return false;
            }
            if (document.getElementById("<%=strOrder.ClientID%>").value == "") {
                alert("报表排序字段必填！");
                return false;
            }
            
            return true;

        }

        var checkedValeu = "";
        $(function () {
            // $("input[name='batch_type']").click();
            $("#datasource").val("<%=Reportforms.datasource %>");
            $("input[type='checkbox']").each(function (a, b) {
                if (this.value == "True") {
                    $(this).attr("checked", "Ture");
                }
            })
            $('.editable-select').editableSelect({

                onSelect: function (list_item) {
                    this.select.val(this.text.val());
                    if (this.text.attr("id") == "datasource")
                        getField(document.getElementById("datasource"));
                }
            })

            $("#tb_fromfield").find("select").each(function (a, b) {
                if ($(this).attr("vid") == "True") {
                    $(this).attr("vid", "1");
                } else if ($(this).attr("vid") == "False") {
                    $(this).attr("vid", "0");
                }
                $(this).val($(this).attr("vid"));
                if ($(this).attr("edite")) {
                    $(this).editableSelectInstances()[0].text[0].value = $(this).attr("vid");
                }


                // alert($("input[name='" + $(this).attr("name") + "'][type='input']").length);

                $("input[name='" + $(this).attr("name") + "']").val($(this).attr("vid"))
            })
            $("#datasource").val("<%=Reportforms.datasource%>");
            //alert($("#datasource").attr("datasourceedite"))
            //if ($("#datasource").attr("datasourceedite") == "1") {
            if ($("#datasource").editableSelectInstances()[0])
                $("#datasource").editableSelectInstances()[0].text[0].value = "<%=Reportforms.datasource%>";
            //alert($("#datasource").editableSelectInstances()[0].text[0].value)
            //}

            $.get("GetFieldTable.aspx", { tablename: $("#datasource").val() }, function (e) {
                $("#field_remark").html(e);
            });

            $("select type=['whereStrtype']").change(function () {
                getSerch(this);
            });
            $("#<%=type.ClientID %>").change(function () {
                if ($(this).val() == "3")
                    $("#<%=strRule.ClientID %>").show();
                else
                    $("#<%=strRule.ClientID %>").hide();
            });
            if ($("#<%=type.ClientID %>").val() == "3")
                $("#<%=strRule.ClientID %>").show();
            else
                $("#<%=strRule.ClientID %>").hide();

            $("#<%=buttonOK.ClientID %>").click(function () {
                $("#tb_fromfield input").each(function () {
                    var vl = $(this).val();
                    //$(this).HTMLEncode();
                    var b = new Base64();
                    vl = b.encode(vl);
                    $(this).val(vl);
                });
                $("#tb_fromfield select").each(function () {
                    var vl = $(this).val();
                    //$(this).HTMLEncode();
                    var b = new Base64();
                    vl = b.encode(vl);
                    $(this).val(vl);
                });
            });
        });
        function getField(obj, select) {

            // alert($(".editable-select").length)
            $.post("GetField.aspx", { tablename: $(obj).val() }, function (e) {
                var data = e;
                if (select != null) {
                    var v = data.split(",");
                    var instances = $(select).editableSelectInstances()[0];
                    instances.wrapper.find('ul').html("");
                    for (var i = 0; i < v.length; i++) {
                        instances.addOption(v[i]);
                    }
                }
                else {

                    $("select[edite]").each(function (a, b) {
                        var v = data.split(",");
                        var instances = $(this).editableSelectInstances()[0];
                        instances.wrapper.find('ul').html("");
                        for (var i = 0; i < v.length; i++) {
                            instances.addOption(v[i]);
                        }
                    })
                }
            })
            //  alert(("select[value='" + $("#datasource").val() + "']").length);


            $.get("GetFieldTable.aspx", { tablename: $(obj).val() }, function (e) {
                $("#field_remark").html(e);
            })
        }

        function changedata() {
            //  $("select[name$='datasource']").val($("#datasource").val());
        }
    </script>
</asp:Content>
