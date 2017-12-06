<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ConfigActivityRules" Codebehind="ConfigActivityRules.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server" />

    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="任务分派设置" runat="server"></asp:Literal></span></h3>
    </div>
    <div class="dataList">
        <div id="divSettings">
            <div style="padding-bottom:10px">
                说明：

                <ul style="list-style-type:decimal; padding-left:30px">
                    <li>规则命令可手动配置也可手动填写，命令格式遵循SQL语句的where条件；<br />
                    比较条件为 大于、小于、大于等于、小于等于 的字段类型必须为数字类型，例如：<br />
                    F1的比较条件为 大于、小于、大于等于、小于等于，则F1的类型必须为数字类型；<br />
                    规则示例：其中F1、F2、....为流程表单字段
                    <br />
                    单条件示例：F1='Test'、并条件示例：F1='Test' AND F2='Test'、或条件示例：F1='Test' OR F2='Test'；<br />
                    复杂条件示例：F1='Test' AND (F2='Test' OR F2='Test1') 等价于如下判断逻辑：<br />
                    if(F1=Test){if(F2=Test || F2=Test1)}
                    </li>
                </ul>
            </div>
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
                <tr>
                    <th style="width:17%;">当前步骤名称：</th>
                    <td style="width:82%;padding:5px 0 5px 5px">
                        <asp:Literal ID="ltlActivityName" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr style="display:none">
                    <th style="width:17%;">下行流程步骤：</th>
                    <td style="padding:5px 0 5px 0">
                        <div>
                            <table cellpadding="0" cellspacing="0" style="text-align:left;">
                                <tr>
                                    <td>
                                        
                                    </td>
                                    <th style="width:17%;">组织控制类型：</th>
                                    <td>
                                        <asp:RadioButton ID="radCur" runat="server" GroupName="organizationType" Text = "当前步骤" />
                                        <asp:RadioButton ID="radStart" runat="server" GroupName="organizationType" Text = "提单步骤" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        
                    </td>
                </tr>
                <tr>
                    <th>路由规则设置：</th>
                    <td style="padding:5px 0 5px 0">
                        <div>
                            <table cellpadding="0" cellspacing="0" style="text-align:center;">
                                <tr>
                                    <%--<td>如果</td>--%>
                                    <td><asp:DropDownList ID="drdlFName" runat="server" ></asp:DropDownList></td>
                                    <td><select id="drdlCondition"  name = "Condtion">
                                        <option value="=" selected = "selected">等于</option>
                                        <option value="&lt;&gt;">不等于</option>
                                        <option value="&gt;">大于</option>
                                        <option value="&lt;">小于</option>
                                        <option value="&gt;=">大于等于</option>
                                        <option value="&lt;=">小于等于</option>
                                        <option value="startwith">向后匹配</option>
                                        <option value="endwith">向前匹配</option>
                                        <option value="like">模糊匹配</option>
                                        <option value="notlike">不包含</option>
                                        </select></td>
                                    <td>
                                        <asp:UpdatePanel ID="udpGetContent" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                            <ContentTemplate>
                                                <asp:Literal ID = "ltlControl" runat="server"></asp:Literal>
                                                <%--<input type="text" id="txtVal" />--%>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="drdlFName" EventName="selectedindexchanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>            
                                    </td>
                                    <td><select id="ddlrelation"  name = "ralation">
                                        <option value="" selected = "selected">- 关联条件 -</option>
                                        <%--<option value="#AND#">并且</option>
                                        <option value="#OR#">或者</option>--%>
                                        <option value="AND">并且</option>
                                        <option value="OR">或者</option>
                                        </select></td>
                                    <td><input type="button" id="btnAdd" class="btn_add" value="添加" onclick="return AddConditions()" /></td>
                                    <td>
                                    
                                    </td>
                                </tr>
                             </table>
                        </div>
                    </td>
                </tr>
                
                <tr>
                    <th>路由规则描述：</th>
                    <td><asp:TextBox ID="txtDescription" TextMode="MultiLine" Width="450px" Height="70px" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>路由规则代码：</th>
                    <td style="padding:5px 0 5px 0">
                        <div id="divAssignControlTypes">
                            <table cellpadding="0" cellspacing="0" style="text-align:center;">
                                <tr>
                                    <td><asp:TextBox ID="txtCommandRules" TextMode="MultiLine" Width="450px" Height="70px" runat="server"></asp:TextBox></td>
                                    <%--<th>父规则:</th><td><asp:DropDownList ID="drdlPrerequest" runat="server"></asp:DropDownList></td>--%>
                                    <td></td>
                                </tr>
                             </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>下行步骤</th>
                    <td><asp:DropDownList ID="drdlNextActivity" runat="server">
                                        <asp:Listitem  value="F1">室经理审批</asp:Listitem>
                                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="2" align="center"><input type="button" id="btnClear" class="btn_del" value="清空" />
                                    <asp:Button ID="btn_Add" runat="server" CssClass="btn_sav" 
                            Text="保存" onclick="btn_Add_Click" />
                                    <input type="button" value="关闭" class="btnFWClass" onclick="window.location = '<%=Type?"../Management/ConfigWorkflow.aspx":"ConfigWorkflow.aspx"%>?wid=<%=WorkflowId %>';" />
                                    </td>
                </tr>
                </table>
             <div class="searchBar">
             <div class="searchTitle">
                <h4>
                    路由规则列表</h4>
                <button onclick="return showContent(this,'tblId1');" title="收缩">
                    <span>折叠</span></button>
            </div>
            </div>
            <table cellpadding="0" cellspacing="0"id="tblId1" class="tblClass" style="text-align:center;">
		            <tr>
                        <%--<th width="5%">序号</th>--%>
                        <th width="30%">标题</th>
                        <th >路由规则描述</th>
                        <th >路由规则代码</th>
                        <th>字段集</th>
                        <th style="width:15%;">下行步骤</th>
                        <th style="width:10%;">操作</th>
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
    				            <td style="text-align:left;"><%# Eval("condition") %></td>
                                <td style="text-align:left;"><%# Eval("FieldsAssemble")%></td>
    				            <td><%#Eval("NextActivityName")%></td>
    				            <td>
                                    <asp:LinkButton ID="btnEdit" CommandName="Edit" CommandArgument='<%# Eval("RuleId") %>' runat="server" class="ico_edit">编辑</asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" CommandName="Delete" CommandArgument='<%# Eval("RuleId") %>' runat="server"  OnClientClick="return confirm('确定要删除规则?');" class="ico_del">删除</asp:LinkButton>
    				            </td>
    				        </tr>				            
				        </ItemTemplate>
				        <AlternatingItemTemplate>
    				        <tr class="trClass" style="text-align:center;">
                                <%--<td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>--%>
                                <%--<td><%# Eval("row_num")%></td>--%>
                                <td style="text-align:left;"><%# Eval("title") %></td>
    				            <td style="text-align:left;">
    				                <%# Eval("description") %>
    				            </td>
    				            <%--<td style="text-align:left;"><%# Eval("condition") %></td>--%>
                                <td style="text-align:left;"><%# Eval("condition") %></td>
                                <td style="text-align:left;"><%# Eval("FieldsAssemble")%></td>
    				            <td><%#Eval("NextActivityName") %></td>
    				            <td>
                                    <asp:LinkButton ID="btnEdit" CommandName="Edit" CommandArgument='<%# Eval("RuleId") %>' runat="server" class="ico_edit">编辑</asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" CommandName="Delete" CommandArgument='<%# Eval("RuleId") %>' runat="server"  OnClientClick="return confirm('确定要删除规则?');" class="ico_del">删除</asp:LinkButton>
    				            </td>
    				            
    				        </tr>	
				        </AlternatingItemTemplate>
				    </asp:Repeater>
				</table>
            <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
                    <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                        Font-Size="9pt" ItemsPerPage="10" PagerStyle="NumericPages" BorderWidth="0px"
                        OnPageIndexChanged="listPager_PageIndexChanged" />
                </div>
        </div>
    </div>
    <script type="text/javascript" src="<%=AppPath %>res/js/Base64.js"></script>
    <script type="text/javascript" language="javascript">
    function AddConditions()
    {
        var fName = document.getElementById("<%= drdlFName.ClientID %>");
        var condition = document.getElementById("drdlCondition");
        var txtval = document.getElementById("txtVal");
        var relation = document.getElementById("ddlrelation");
        var discription = document.getElementById("<%= txtDescription.ClientID %>");
        var rules = document.getElementById("<%= txtCommandRules.ClientID %>");
        if(fName.value == "" || fName.value == null || txtval.value == "" || txtval.value == null)
        {
            alert("请填写比较条件！");
            txtval.focus();
            return false;
        }
        else
        {    
            var relationShip =  relation.options[relation.selectedIndex].value == "" ? "" : relation.options[relation.selectedIndex].text;
            var conditiontext = condition.options[condition.selectedIndex].text;
            var conditionvalue = condition.options[condition.selectedIndex].value;
            /*if(conditiontext != "等于" && conditiontext!="不等于" ){
                if(isNaN(txtval.value))
                {
                    alert("比较条件必须是数字！");
                    txtval.value = "";
                    txtval.focus();
                    return false;
                }
                discription.value += fName.options[fName.selectedIndex].text +" "+ condition.options[condition.selectedIndex].text +" "+ txtval.value +" ";
                //rules.value += "(FName = '"+ fName.options[fName.selectedIndex].value +"' and FNumber "+ condition.options[condition.selectedIndex].value +" "+ txtval.value +") ";
                rules.value += fName.options[fName.selectedIndex].value +" "+ condition.options[condition.selectedIndex].value +" "+ txtval.value +" ";
            }
            else{*/
                discription.value += fName.options[fName.selectedIndex].text +" "+ condition.options[condition.selectedIndex].text +" '"+ txtval.value +"' ";
                if (conditionvalue == "startwith")
                    rules.value += fName.options[fName.selectedIndex].value + " LIKE '" + txtval.value + "%' ";
                else if (conditionvalue == "endwith")
                    rules.value += fName.options[fName.selectedIndex].value + " LIKE '%" + txtval.value + "' ";
                else if (conditionvalue == "like")
                    rules.value += fName.options[fName.selectedIndex].value + " LIKE '%" + txtval.value + "%' ";
                else if (conditionvalue == "notlike")
                    rules.value += fName.options[fName.selectedIndex].value + " NOT LIKE '%" + txtval.value + "%' ";
                else
                    rules.value += fName.options[fName.selectedIndex].value +" "+ condition.options[condition.selectedIndex].value +" '"+ txtval.value +"' ";
            //}
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
            if ($("#<%=this.drdlNextActivity.ClientID %>").val() == "") {
                alert("请选择下行步骤！");
                return false;
            }
            else if (discription.value == "") {
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
        $("#<%=drdlFName.ClientID %>").change(function () {
            $.ajax({
                type: "post",
                dataType: "html",
                url: "<%=AppPath %>apps/xqp2/pages/workflows/ajax/RulesAjax.aspx",
                data: { wid: "<%=WorkflowId %>"
                    , fname: $("#<%=drdlFName.ClientID %>").val()
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