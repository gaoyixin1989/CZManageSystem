<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_pms_pages_UCS_ImgFromManagerEdit" Codebehind="UCS_ImgFromManagerEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <%--   <link href="<%=AppPath%>App_Themes/gmcc/icon.css" rel="stylesheet" type="text/css" />--%>
    <%--<link href="<%=AppPath%>App_Themes/gmcc/easyui.css" rel="stylesheet" type="text/css" />--%>
    <style type="text/css">
        .div_title
        {
            padding-top: 3px;
            text-align: left;
            font-weight: bold;
            background-color: #F7F7F7;
            color: #666666;
            height: 18px;
            background-image: url(../../../App_Themes/gmcc/img/hbar-bg.gif);
        }
        .tb_content2
        {
            border-bottom: 0px solid #c9cbd3;
            border-top: 1px solid #c9cbd3;
            border-left: 0px solid #c9cbd3;
            border-right: 1px solid #c9cbd3;
            margin: 5px auto;
            width: 90%;
        }
        
        .tb_content
        {
            text-align: center;
            background-color: #fff;
        }
        .tb_content td
        {
            border-bottom: 0px solid #c9cbd3;
            border-top: 0px solid #c9cbd3;
            border-left: 0px solid #c9cbd3;
            border-right: 0px solid #c9cbd3;
        }
        .tb_content2 th
        {
            text-align: center;
            border-bottom: 1px solid #c9cbd3;
            border-top: 0px solid #c9cbd3;
            border-left: 1px solid #c9cbd3;
            border-right: 0px solid #c9cbd3;
            background-color: #D5DEDB;
            font-weight: bold;
            color: #666666;
        }
        .tb_content2 td
        {
            border-bottom: 1px solid #c9cbd3;
            border-top: 0px solid #c9cbd3;
            border-left: 1px solid #c9cbd3;
            border-right: 0px solid #c9cbd3;
            text-align: center;
        }
        .div_img
        {
            height: 250px;
            width: 70%;
            margin: 0px auto;
        }
        .tb_content td
        {
            white-space: nowrap;
            font-size: 9pt;
            text-align: center;
        }
        th
        {
            white-space: nowrap;
        }
        .div_content1
        {
            margin: 5px 4px -5px 5px;
            border: 1px solid #c9cbd3;
            border-bottom: 0px solid #c9cbd3;
            max-height: 140px;
        }
        
        .div_content4
        {
            border: 1px solid #c9cbd3;
            margin: 0px 4px 5px 5px;
            border-top: 0px;
        }
        .div_inco
        {
            margin-left: 2px;
        }
        .right
        {
            margin-right: 0px;
        }
    </style>
    <script src="<%=AppPath%>javascript/jquery-1.7.2.min.js" type="text/javascript"></script>
    <%--<link href="<%=AppPath%>javascript/libs/skins/blue/style.css" rel="stylesheet" type="text/css" />--%>
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
    <script src="<%=AppPath%>res/js/ChangeDate.js" type="text/javascript"></script>
    <link href="<%=AppPath%>res/js/jquery.editable-select.css"
        rel="stylesheet" type="text/css" />
    <script src="<%=AppPath%>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.js"
        type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.pack.js"
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
                    图表名称
                </th>
                <td style="text-align: left">
                    <asp:TextBox ID="formname" Width="200" runat="server" MaxLength="50"></asp:TextBox><span><font
                        color="#FF0000">*</font></span>
                </td>
                <th style="width:13%">
                    只显示图表
                </th>
                <td style="text-align: left">
                    <select runat="server" id="ddlOnly"><option value="1">是</option><option value="0">否</option></select>
                </td>
            </tr>
            <tr>
                
                <th style="width:13%">
                  图表类型
                </th>
                <td style="text-align: left">
                     <select runat="server" id="formtype">
                        <option value="2">普通图表</option>
                        <%--<option value="3">多线条图表</option>--%>
                        <option value="4">行维度线图</option>
                        <option value="5">行维度柱状图</option>
                     </select>
                </td>
                <th style="width:13%">
                    报表类型
                </th>
                <td style="text-align: left">
                    <select runat="server" id="type">
                        <option value="1">普通表(无任何权限过滤)</option>
                        <option value="3">权限统计报表 </option>
                    </select><span><font color="#FF0000"> *</font></span>
                </td>
            </tr>
            <tr>
                <th style="width:13%">
                    对应数据库表
                </th>
                <td style="text-align: left">
                    <select id="datasource" name="datasource" onchange="getField(this);changedata();"
                        vid="<%=Reportforms.datasource%>">
                        <optgroup label="---------表----------">
                            <%=sb.ToString() %></optgroup><optgroup label="----------视图---------"><%=sb2.ToString() %></optgroup>
                    </select>
                </td>
                <th style="width:13%">
                    排序条件
                </th>
                <td style="text-align: left">
                    <asp:TextBox ID="strOrder" Width="200" runat="server"></asp:TextBox><span><font color="#FF0000">
                        *</font></span>
                </td>
            </tr>
            <tr>
                <th style="width:13%">
                    分组字段
                </th>
                <td style="text-align: left">
                    <asp:TextBox ID="strGroup" Width="200" runat="server"></asp:TextBox>
                </td>
                <th style="width:13%">
                    权限过滤条件
                   
                </th>
                <td style="text-align: left">
                    <asp:TextBox ID="strRule" Width="200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th style="width:13%">
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
                <td colspan="3">
                    <div class="libox" style="width: 100%; max-height: 400px; overflow: scroll">
                        <input value="新增" type="button" style="float: left" onclick="add()"/>
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
                                <th style="width:150px">
                                    图形中类型
                                </th style="width:60px">
                                <th style="width:60px">
                                    坐标轴
                                </th>
                                <th style="width:60px">
                                    能否统计
                                </th>
                                <th style="width:60px">
                                    计算方式
                                </th>
                                <th style="width:220px">
                                    链接地址
                                </th>
                                <th style="width:120px">
                                    传递的参数名称
                                </th>
                                <th style="width:40px">
                                    操作
                                </th>
                            </tr>
                            <asp:Repeater ID="fieldlist" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <input value='<%#Eval("fieldname")%>' name='<%#Eval("id") %>_fieldname' style="width: 100%" />
                                        </td>
                                        <td style="text-align: left">
                                            <%-- <input value='<%#Eval("field")%>' name='<%#Eval("id") %>_field' style="width: 100%" />--%>
                                            <select edite='1' vid="<%#Eval("field")%>" name='<%#Eval("id") %>_field' id="<%#Eval("id") %>"
                                                style="width: 200px" class="editable-select">
                                                <%=sb3.ToString() %>
                                            </select>
                                        </td>
                                        <td>
                                            <input value='<%#Eval("fieldorder")%>' onkeyup="this.value=this.value.replace(/[^\d+.]/g,'')"
                                                name='<%#Eval("id") %>_fieldorder' style="width: 100%" />
                                        </td>
                                        <td>
                                            <select name='<%#Eval("id") %>_imgtype' vid='<%#Eval("imgtype") %>' style='width: 100%'>
                                                <option value=''>请选择</option>
                                                <option value='spline'>线</option>
                                                <option value='column'>柱</option>
                                                <option value='pie'>饼</option>
                                            </select>
                                        </td>
                                        <td>
                                            <select name='<%#Eval("id") %>_Axis' vid='<%#Eval("Axis") %>' style='width: 100%'>
                                                <option value=''>请选择</option>
                                                <option value='1'>x轴</option>
                                                <option value='2'>y轴</option>
                                            </select>
                                        </td>
                                        <td>
                                            <select name='<%#Eval("id") %>_IsCount' vid='<%#Eval("IsCount") %>' style='width: 100%'>
                                                <option value='0'>否</option>
                                                <option value='1'>是</option>
                                            </select>
                                        </td>
                                        <td>
                                            <select name='<%#Eval("id") %>_StatisticsType' vid='<%#Eval("StatisticsType") %>' style='width: 100%'>
                                                <option value='0'>请选择</option>
                                                <option value='1'>累加</option>
                                                <option value='2'>平均数</option>
                                            </select>
                                        </td>
                                        <td>
                                            <input value='<%#Eval("LinkUrl")%>' name='<%#Eval("id") %>_LinkUrl' style="width: 100%" />
                                        </td>
                                        <td>
                                            <input value='<%#Eval("parameter")%>' name='<%#Eval("id") %>_parameter' style="width: 100%" />
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
                <td colspan="4" align="center">
                    <input type="button" value="预览" class="btn_sav" onclick="preSubmit()" />
                    <asp:Button ID="buttonOK" runat="server" Text="保存" CssClass="btn_sav" OnClick="buttonOK_Click"
                        OnClientClick="return check();" />
                    <input type="button" value="返回" class="btnReturnClass" style="margin-left: 6px" onclick="window.location='UCS_ImgFromManager.aspx';" />
                </td>
            </tr>
        </table>
        <table id="main_tb" class="tb_content" width="99%" cellpadding="0" cellspacing="0">
        </table>
    </div>
    <input type="hidden" name="isifr" id="isifr" />
    <iframe id="ifr" name="ifr" style="display: none"></iframe>
    <script src="../../../res/js/highcharts.js" type="text/javascript"></script>
    <script src="../../../javascript/jquery.showLoading.js" type="text/javascript"></script>
    <script type="text/javascript">
        function preSubmit() {
            if (check()) {
              //  $(document.body).showLoading();
                $("#isifr").val("1");
                  
                document.forms[0].target = "ifr";
                document.forms[0].submit();
                $("#isifr").val("0");
                  document.forms[0].target="";
        
                return true;
            }
            else {
                return false;
            }
        }
        function preview(result) {
            
            var html = result.split('|')[0];
            var obj = eval("(" + result.split('|')[1] + ")");
       
       //加载表格
            $("#main_tb").html(html);
      //加载图形
            new Highcharts.Chart(obj);
            $(document.body).hideLoading();
    }

        function del(obj) {
            if ($(obj).attr("a_del")) {
                index--;
            }

            obj.parentNode.parentNode.parentNode.removeChild(obj.parentNode.parentNode);
        }
        var index=1;
        function add() {
            var d_order = $("#tb_fromfield").find("tr").length;
            var tr = $("<tr><td><input style='width: 100%' name='add_" + index + "_fieldname'/></td>"
            +"<td><select edite='1' class='editable-select' style='width:100%' name='add_" + index + "_field'></select></td>"
            +"<td><input style='width:100%' name='add_" + index + "_fieldorder' onkeyup=\"this.value=this.value.replace(/[^\\d+.]/g,'')\" value='" + d_order + "' /></td>"
            +"<td><select name='add_" + index + "_imgtype'  style='width: 100%'><option value=''>请选择</option><option value='spline'>线</option><option value='column'>柱</option><option value='pie'>饼</option></select></td>"
            +"<td><select name='add_" + index + "_Axis'  style='width: 100%'><option value=''>请选择</option><option value='1'>x轴</option><option value='2'>y轴</option></select></td>"
            +"<td><select name='add_" + index + "_IsCount' style='width: 100%'><option value='0'>否</option><option value='1'>是</option></select></td>"
            +"<td><select name='add_" + index + "_StatisticsType' style='width: 100%'><option value='0'>请选择</option><option value='1'>累加</option><option value='2'>平均数</option></select></td>"
            +"<td><input style='width: 100%' name='add_" + index + "_LinkUrl'/></td>"
            +"<td><input name='add"+index+"_parameter' style='width: 100%' /></td>"
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
            var head_str =str.length==2?str[0] + "_": str[0] + "_" + str[1] + "_";
            $("select[name='" + head_str + "datasource" + "']").val(v);
            var field = $("input[name='" + head_str + "field" + "']").val();
            if ($("input[name='" + head_str + "whereFieldText" + "']").val().length==0)
                $("input[name='" + head_str + "whereFieldText" + "']").val(field);
            if ($("input[name='" + head_str + "whereFieldValue" + "']").val().length == 0)
            $("input[name='" + head_str + "whereFieldValue" + "']").val(field);
 
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
            var xcnt = 0 ;var ycnt = 0;
            $.each($("select[name$='Axis']"), function (a, b) {
             
                if ($(this).val() == "1") {
                    xcnt++;
                }
                if ($(this).val() == "2") {
                    ycnt++;
                }
            })
            if (xcnt == 0) {
                alert("请选择x轴");return false;
            }
            if (xcnt > 1) {
                alert("只能选择1个x轴");return false;
            }
            if (ycnt > 2 && $("#<%=formtype.ClientID %>").val()=="2") {
                alert("最多只能选择两个y轴");return false;
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
            $('.editable-select').editableSelect(
            {
               onSelect: function (list_item) {
                   this.select.val(this.text.val());
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
            $("#datasource").val($("#datasource").attr("vid"));
            <%if(!string.IsNullOrEmpty(Request.QueryString["Id"])){ %>
            preSubmit();
            <%} %>

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
                    $(this).HTMLEncode();
                });
                $("#tb_fromfield select").each(function () {
                    var vl = $(this).val();
                    $(this).HTMLEncode();
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
        }
  
        function changedata() { 
          //  $("select[name$='datasource']").val($("#datasource").val());
        }
    </script>
</asp:Content>
