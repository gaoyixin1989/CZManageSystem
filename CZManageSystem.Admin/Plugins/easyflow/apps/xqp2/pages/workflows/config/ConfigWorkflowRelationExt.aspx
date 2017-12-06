<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ConfigWorkflowRelationExt" Codebehind="ConfigWorkflowRelationExt.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="titleContent">
        <h3>
            <span>
                <asp:Literal ID="ltlTitle" Text="更多设置" runat="server" /></span></h3>
    </div>
    <div class="dataList">
        <div id="divSettings">
            <div style="padding-bottom: 10px">
                说明：
                <ul style="list-style-type: decimal; padding-left: 30px">
                    <li>输入用户时，只输入用户名(只输入一个用户名)，输入#Creator#表示子流程发起人为父流程的发起人，输入#CurrentActor#表示子流程的发起人为父流程当前步骤的处理人；</li>
                </ul>
            </div>
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 6px;">
                <tr>
                    <th style="width: 17%;">
                        当前步骤名称：
                    </th>
                    <td style="width: 82%; padding: 5px 0 5px 5px">
                        <asp:Literal ID="ltlActivityName" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr id="trUsers" runat="server" style="padding: 5px 0 5px 0;">
                    <th>
                        发起人设置：
                    </th>
                    <td>
                        <asp:TextBox ID="txtUsers" Width="120px" runat="server"></asp:TextBox>
                        <a href="javascript:void(0);" onclick="javascrpt:return openUserSelector2('<%=txtUsers.ClientID%>');">
                            选择用户</a>
                    </td>
                </tr>
                <tr id="trOrg" runat="server">
                    <th>
                        字段集设置：
                    </th>
                    <td>
                        <table cellpadding="0" cellspacing="0" style="text-align: center;">
                            <tr>
                                <%--<td>如果</td>--%>
                                <td>
                                    父流程字段：<asp:DropDownList ID="drdlFName" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    子流程字段：<asp:DropDownList ID="drdlRelationFName" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <input type="button" id="btnAdd" class="btn_add" value="添加" onclick="return AddConditions()" />
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="0" id="tblId1" class="tblClass" style="text-align: center;">
                            <thead>
                                <tr>
                                    <%--<th width="5%">序号</th>--%>
                                    <th width="30%">
                                        父流程字段
                                    </th>
                                    <th>
                                        子流程字段
                                    </th>
                                    <th>
                                        操作
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <%=TbHtml %>
                            </tbody>
                        </table>
                        <asp:HiddenField ID="hidXml" runat="server" Value="" />
                    </td>
                </tr>
                <tr>
                    <th>
                        父流程流转规则：
                    </th>
                    <td>
                        <div class="dataList">
                            <div id="div1">
                                <div style="padding-bottom: 10px">
                                    说明：
                                    <ul style="list-style-type: decimal; padding-left: 30px">
                                        <li>规则命令可手动配置也可手动填写，命令格式遵循SQL语句的where条件；<br />
                                            比较条件为 大于、小于、大于等于、小于等于 的字段类型必须为数字类型，例如：<br />
                                            F1的比较条件为 大于、小于、大于等于、小于等于，则F1的类型必须为数字类型；<br />
                                            规则示例：其中F1、F2、....为流程表单字段
                                            <br />
                                            单条件示例：F1='Test'、并条件示例：F1='Test' AND F2='Test'、或条件示例：F1='Test' OR F2='Test'；<br />
                                            复杂条件示例：F1='Test' AND (F2='Test' OR F2='Test1') 等价于如下判断逻辑：<br />
                                            if(F1=Test){if(F2=Test || F2=Test1)} </li>
                                    </ul>
                                </div>
                                <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 6px;">
                                    <tr>
                                        <th>
                                            子流程字段规则设置：
                                        </th>
                                        <td style="padding: 5px 0 5px 0">
                                            <div>
                                                <table cellpadding="0" cellspacing="0" style="text-align: center;">
                                                    <tr>
                                                        <%--<td>如果</td>--%>
                                                        <td>
                                                            <asp:DropDownList ID="drdlFieldName" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <select id="drdlCondition" name="Condtion">
                                                                <option value="=" selected="selected">等于</option>
                                                                <option value="&lt;&gt;">不等于</option>
                                                                <option value="&gt;">大于</option>
                                                                <option value="&lt;">小于</option>
                                                                <option value="&gt;=">大于等于</option>
                                                                <option value="&lt;=">小于等于</option>
                                                                <option value="startwith">向后匹配</option>
                                                                <option value="endwith">向前匹配</option>
                                                                <option value="like">模糊匹配</option>
                                                                <option value="notlike">不包含</option>
                                                            </select>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="udpGetContent" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                <ContentTemplate>
                                                                    <asp:Literal ID="ltlControl" runat="server"></asp:Literal>
                                                                    <%--<input type="text" id="txtVal" />--%>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="drdlFName" EventName="selectedindexchanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <select id="ddlrelation" name="ralation">
                                                                <option value="" selected="selected">- 关联条件 -</option>
                                                                <%--<option value="#AND#">并且</option>
                                        <option value="#OR#">或者</option>--%>
                                                                <option value="AND">并且</option>
                                                                <option value="OR">或者</option>
                                                            </select>
                                                        </td>
                                                        <td>
                                                            <input type="button" id="Button1" class="btn_add" value="添加" onclick="return AddRules()" />
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            路由规则描述：
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtDescription" TextMode="MultiLine" Width="450px" Height="70px"
                                                runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            路由规则代码：
                                        </th>
                                        <td style="padding: 5px 0 5px 0">
                                            <div id="divAssignControlTypes">
                                                <table cellpadding="0" cellspacing="0" style="text-align: center;">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtCommandRules" TextMode="MultiLine" Width="450px" Height="70px"
                                                                runat="server"></asp:TextBox>
                                                        </td>
                                                        <%--<th>父规则:</th><td><asp:DropDownList ID="drdlPrerequest" runat="server"></asp:DropDownList></td>--%>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <input type="button" id="btnClear" class="btn_del" value="清空" />
                                            <asp:Button ID="btn_Add" runat="server" CssClass="btn_sav" Text="保存" 
                                                onclick="btn_Add_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <div class="searchBar">
                                    <div class="searchTitle">
                                        <h4>
                                            子流程规则列表</h4>
                                        <button onclick="return showContent(this,'tblId1');" title="收缩">
                                            <span>折叠</span></button>
                                    </div>
                                </div>
                                <table cellpadding="0" cellspacing="0" id="Table1" class="tblClass" style="text-align: center;">
                                    <tr>
                                        <%--<th width="5%">序号</th>--%>
                                        <th width="30%">
                                            标题
                                        </th>
                                        <th>
                                            规则描述
                                        </th>
                                        <th>
                                            规则代码
                                        </th>
                                        <th>
                                            操作
                                        </th>
                                    </tr>
                                    <asp:Repeater ID="listResults" runat="server" 
                        onitemcommand="listResults_ItemCommand" >
				        <ItemTemplate>
    				        <tr style="text-align:center;">
                                <%--<td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>--%>
                                <%--<td><%# Eval("row_num")%></td>--%>
    				            <td style="text-align:left;"><%# Eval("title") %></td>
    				            <td style="text-align:left;">
    				                <%# Eval("description") %>
    				            </td>
    				            <td style="text-align:left;"><%# Eval("conditions") %></td>
    				            <td>
                                    <asp:LinkButton ID="btnEdit" CommandName="Edit" CommandArgument='<%# Eval("RuleId") %>' runat="server" class="ico_edit">编辑</asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" CommandName="Delete" CommandArgument='<%# Eval("RuleId") %>' runat="server"  OnClientClick="return confirm('确定要删除规则?');" class="ico_del">删除</asp:LinkButton>
    				            </td>
    				        </tr>				            
				        </ItemTemplate>
				        <AlternatingItemTemplate>
    				        <tr class="trClass" style="text-align:center;">
                                <td style="text-align:left;"><%# Eval("title") %></td>
    				            <td style="text-align:left;">
    				                <%# Eval("description") %>
    				            </td>
                                <td style="text-align:left;"><%# Eval("conditions") %></td>
    				            <td>
                                    <asp:LinkButton ID="btnEdit" CommandName="Edit" CommandArgument='<%# Eval("RuleId") %>' runat="server" class="ico_edit">编辑</asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" CommandName="Delete" CommandArgument='<%# Eval("RuleId") %>' runat="server"  OnClientClick="return confirm('确定要删除规则?');" class="ico_del">删除</asp:LinkButton>
    				            </td>
    				            
    				        </tr>	
				        </AlternatingItemTemplate>
				    </asp:Repeater>
                                </table>
                                
                            </div>
                        </div>
        </td> </tr> </table>
        <p align="center" style="margin-top: 10px">
            <asp:Button ID="btnSave" runat="server" CssClass="btn_sav" Text="保存" OnClick="btnSave_Click" />
            <input type="button" value="返回" class="btnFWClass" onclick="window.location='ConfigWorkflowRelation.aspx?wid=<%=WorkflowId %>';" />
        </p>
        <div>
            <asp:ValidationSummary ID="vsummary1" runat="server" ShowSummary="false" ShowMessageBox="true" />
        </div>
    </div>
    </div>
    <script type="text/javascript">
    <!--        //
        $(function () {
            $("#tblId1 a").click(function () {
                $(this).parent("td").parent("tr").remove();
            })
            $("#<%=btnSave.ClientID %>").click(function () {
                if ($("#<%=txtUsers.ClientID %>").val() == "") {
                    alert("请填写发起人")
                    return false;
                }
                var xml = "<Root>"
                $("#tblId1 tbody").children("tr").each(function () {
                    var val = $(this).children("td").eq(0).html();
                    var relation = $(this).children("td").eq(1).html();
                    xml += "<item FName=\"" + val + "\" RelationFName=\"" + relation + "\" />"
                });
                xml += "</Root>"
                $("#<%=hidXml.ClientID %>").val(xml);
            });
        });

        function AddConditions() {
            var fname = $("#<%=drdlFName.ClientID %>").val();
            var relationfname = $("#<%=drdlRelationFName.ClientID %>").val();
            if (fname == "")
                alert("请选择父流程字段")
            else if (relationfname == "")
                alert("请选择子流程字段")
            else {
                var canAdd = true;
                $("#tblId1 tbody").children("tr").each(function () {
                    var val = $(this).children("td").eq(0).html();
                    var relation = $(this).children("td").eq(1).html();
                    if (val == fname && relation == relationfname) {
                        alert("已存在相同字段映射设置")
                        canAdd = false;
                    }
                });
                if (canAdd)
                    $("#tblId1 tbody").append("<tr><td>" + fname + "</td><td>" + relationfname + "</td><td><a href=\"#\">删除</a></td></tr>");
            }
            $("#tblId1 a").click(function () {
                $(this).parent("td").parent("tr").remove();
            })
        }
        function openUserSelector(inputId) {
            //        var h = 500;
            //	    var w = 800;
            //	    var iTop = (window.screen.availHeight-30-h)/2;    
            //	    var iLeft = (window.screen.availWidth-10-w)/2; 
            //	    window.open('<%=AppPath%>contrib/security/pages/PopupUserPicker.aspx?inputid='+ inputId, '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');
            //	    return false;
            var h = 500;
            var w = 800;
            var user = "";
            if (state == 1) {
                user = $("#ctl00_cphBody_txtUsers").val(); //下行用户
            }
            else if (state == 2) {
                user = $("#ctl00_cphBody_txtUsersAssign").val(); //平转用户
            }
            else {
                //字段控制
                user = $("#" + inputId).val();
            }

            var iTop = (window.screen.availHeight - 30 - h) / 2;
            var iLeft = (window.screen.availWidth - 10 - w) / 2;
            window.open('<%=AppPath%>contrib/security/pages/PopupUserPicker.aspx?username=' + user + '&&inputid=' + inputId, '', 'height=' + h + ', width=' + w + ', top=' + iTop + ', left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');
            return false;
        }

        function openUserSelector2(inputId) {
            var h = 500;
            var w = 800;
            var user = "";
            //	    if (state == 1) {
            //	        user = $("#ctl00_cphBody_txtUsers").val(); //下行用户
            //	    }
            //	    else if (state == 2) {
            //	        user = $("#ctl00_cphBody_txtUsersAssign").val(); //平转用户
            //	    }
            //	    else
            user = $("#" + inputId).val();
            var iTop = (window.screen.availHeight - 30 - h) / 2;
            var iLeft = (window.screen.availWidth - 10 - w) / 2;
            window.open('<%=AppPath%>contrib/security/pages/PopupUserPicker.aspx?username=' + user + '&&inputid=' + inputId, '', 'height=' + h + ', width=' + w + ', top=' + iTop + ', left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');
            return false;
        }
    //-->
    </script>
    <script type="text/javascript" src="<%=AppPath %>res/js/Base64.js"></script>
    <script type="text/javascript" language="javascript">
        function AddRules() {
            var fName = document.getElementById("<%= drdlFieldName.ClientID %>");
            var condition = document.getElementById("drdlCondition");
            var txtval = document.getElementById("txtVal");
            var relation = document.getElementById("ddlrelation");
            var discription = document.getElementById("<%= txtDescription.ClientID %>");
            var rules = document.getElementById("<%= txtCommandRules.ClientID %>");
            if (fName.value == "" || fName.value == null || txtval.value == "" || txtval.value == null) {
                alert("请填写比较条件！");
                txtval.focus();
                return false;
            }
            else {
                var relationShip = relation.options[relation.selectedIndex].value == "" ? "" : relation.options[relation.selectedIndex].text;
                var conditiontext = condition.options[condition.selectedIndex].text;
                var conditionvalue = condition.options[condition.selectedIndex].value;
                discription.value += fName.options[fName.selectedIndex].text + " " + condition.options[condition.selectedIndex].text + " '" + txtval.value + "' ";
                if (conditionvalue == "startwith")
                    rules.value += fName.options[fName.selectedIndex].value + " LIKE '" + txtval.value + "%' ";
                else if (conditionvalue == "endwith")
                    rules.value += fName.options[fName.selectedIndex].value + " LIKE '%" + txtval.value + "' ";
                else if (conditionvalue == "like")
                    rules.value += fName.options[fName.selectedIndex].value + " LIKE '%" + txtval.value + "%' ";
                else if (conditionvalue == "notlike")
                    rules.value += fName.options[fName.selectedIndex].value + " NOT LIKE '%" + txtval.value + "%' ";
                else
                    rules.value += fName.options[fName.selectedIndex].value + " " + condition.options[condition.selectedIndex].value + " '" + txtval.value + "' ";
            }
            return true;
        }

        $(function () {
            var discription = document.getElementById("<%= txtDescription.ClientID %>");
            var rules = document.getElementById("<%= txtCommandRules.ClientID %>");
            var relation = document.getElementById("ddlrelation");
            $("#ddlrelation").change(function () {
                if ($(this).val() == "") {
                    discription.value += "";
                    rules.value += "";
                }
                else {
                    discription.value += "" + relation.options[relation.selectedIndex].text + " ";
                    rules.value += "" + $(this).val() + " ";
                }
            });

            $("#btnClear").click(function () {
                $("#<%=this.txtDescription.ClientID %>").attr("value", "");
                $("#<%=this.txtCommandRules.ClientID %>").attr("value", "");
                $("#ddlrelation").attr("value", "");
            });

            $("#<%=this.btn_Add.ClientID %>").click(function () {
                if (discription.value == "") {
                    alert("规则描述不能为空！");
                    return false;
                }
                else if (rules.value == "") {
                    alert("请填写正确的规则！");
                    return false;
                }
                var b = new Base64();
                rules.value = b.encode(rules.value);
                return true;
            });
            $("#<%=drdlFieldName.ClientID %>").change(function () {
                $.ajax({
                    type: "post",
                    dataType: "html",
                    url: "<%=AppPath %>apps/xqp2/pages/workflows/ajax/RulesAjax.aspx",
                    data: { wid: "<%=RelationWorkflowId %>"
                    , fname: $("#<%=drdlFieldName.ClientID %>").val()
                    },
                    async: true,
                    timeout: 300000,
                    success: function (data) {
                        $("#<%=udpGetContent.ClientID %>").html(data);
                    },
                    error: function () {
                    }
                });
            });
        });
    </script>
</asp:Content>
