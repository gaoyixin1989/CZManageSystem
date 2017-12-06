<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_maintenance_transfertask" Codebehind="transfertask.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security" Namespace="Botwave.Security.Web.Controls" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.XQP" Namespace="Botwave.XQP.Web.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="toolkitScriptManager1" runat="server" />
    <%--<div class="titleContent">
        您当前位置：<a href="<%=AppPath%>contrib/workflow/pages/default.aspx" >首页</a> &gt; <span class="cRed">
        工单管理</span>
    </div>--%>
	<div class="searchBar">
	    <div class="titleContent">
            <h3><span style="font-weight:bolder;color:Black;">任务转移</span></h3>
            <%--<h3><span style="font-size:11px;"><a href="SearchHistory.aspx">历史工单管理</a></span></h3>--%>
        </div>
		<div class="searchTitle">
			<h4>任务查询<asp:Literal ID="ltlSearchName" runat="server"></asp:Literal></h4>
			<button onclick="return showSearch(this);" title="收缩"><span>折叠</span></button>
		</div>
		<div class="searchInfo" id="searchBody">
		    <table cellpadding="3" cellspacing="1">
		        <tr>
		            <td>处理时间：</td>
		            <td> 
		                <bw:DateTimePicker ID="dtpStart1" runat="server" InputBoxCssClass="inputbox" IsRequired="false" ExpressionValidatorText="*" ExpressionErrorMessage="工单发起时间的开始时间日期格式错误." />
		                <%--<asp:TextBox ID="dtpStart1" runat="server" Width="90px" onClick="WdatePicker()" style="border:#999 1px solid;height:20px;background:#fff url(../../../My97DatePicker/skin/datePicker.gif) no-repeat right;"></asp:TextBox>--%>
		            </td>
		            <td>至</td>
		            <td>
		                <bw:DateTimePicker ID="dtpStart2" runat="server" InputBoxCssClass="inputbox"  IsRequired="False" ExpressionValidatorText="*" ExpressionErrorMessage="工单发起时间的截止时间日期格式错误." />
		               <%-- <asp:TextBox ID="dtpStart2" runat="server" Width="90px" onClick="WdatePicker()" style="border:#999 1px solid;height:20px;background:#fff url(../../../My97DatePicker/skin/datePicker.gif) no-repeat right;"></asp:TextBox>--%>
		            </td>
		        </tr>
		        <tr>
		            <td>流程：</td>
		            <td>
		                <asp:DropDownList ID="ddlWorkflowList" runat="server" AutoPostBack="True" onselectedindexchanged="ddlWorkflowList_SelectedIndexChanged">
		                    <asp:ListItem Value="" Text="－ 全部 －"></asp:ListItem>
		                </asp:DropDownList>
		            </td>
		            <td>步骤：</td>
		            <td>
		                <asp:UpdatePanel ID="updatepanelActivities" runat="server" UpdateMode="Conditional" RenderMode="Inline">
		                    <ContentTemplate>
		                    <asp:DropDownList ID="ddlActivityList" runat="server">
		                        <asp:ListItem Value="" Text="－ 全部 －"></asp:ListItem>
		                    </asp:DropDownList>
		                    </ContentTemplate>
		                    <Triggers>
		                        <asp:AsyncPostBackTrigger ControlID="ddlWorkflowList" EventName="selectedindexchanged" />
		                    </Triggers>
		                </asp:UpdatePanel>
		            </td>
		        </tr>
		        <tr>
		            <td>标题关键字：</td>
		            <td>
		                <asp:TextBox ID="txtTitleKeywords" runat="server" CssClass="inputbox"></asp:TextBox>
		                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtenderTitle" runat="server" 
                            BehaviorID="TitleAutoCompleteEx"  TargetControlID="txtTitleKeywords" 
                            ServicePath="<%=AppPath%>contrib/workflow/pages/WorkflowAjaxService.asmx" ServiceMethod="GetCompletionWorkflowTitles" 
                            MinimumPrefixLength="2" CompletionInterval="1000" 
                            EnableCaching="true" CompletionSetCount="20" DelimiterCharacters=";, :"
                            CompletionListCssClass="autocomplete_completionListElement" 
                            CompletionListItemCssClass="autocomplete_listItem" 
                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                            <Animations>
                                <OnShow>
                                    <Sequence>
                                        <OpacityAction Opacity="0" />
                                        <HideAction Visible="true" />
                                        <ScriptAction Script="
                                            // Cache the size and setup the initial size
                                            var behavior = $find('TitleAutoCompleteEx');
                                            if (!behavior._height) {
                                                var target = behavior.get_completionList();
                                                behavior._height = target.offsetHeight - 2;
                                                target.style.height = '0px';
                                            }" />
                                        <Parallel Duration=".4">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('TitleAutoCompleteEx')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <Parallel Duration=".4">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('TitleAutoCompleteEx')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                            </Animations>
                        </ajaxToolkit:AutoCompleteExtender>
		            </td>
		            <td>受理号：</td>
		            <td>
		                <asp:TextBox ID="txtWorkId" runat="server" CssClass="inputbox"></asp:TextBox>
		            </td>
		        </tr>
		        <tr style="display:none">
		            <td>内容关键字：</td>
		            <td colspan="3">
		                <asp:TextBox ID="txtContentKeywords" runat="server" CssClass="inputbox"></asp:TextBox>
		            </td>
		        </tr>
                <tr>
		            <td>被转移用户：</td>
		            <td colspan="3">
		                <asp:TextBox ID="txtFromUser" runat="server" CssClass="inputbox" Width="300px"></asp:TextBox>
                        <asp:HiddenField ID="hidFromUser" runat="server" Value="" />
                        <input type='button' id="btnActor" style='cursor: pointer; background: url(<%=AppPath%>App_Themes/gmcc/img/btnse01.jpg);border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;' />
		                <span style="color:Red">*可选择多个用户</span>
		            </td>
		            
		        </tr>
                <tr>
                    <td>转移对象：</td>
		            <td colspan="3">
		                <asp:TextBox ID="txtToUser" runat="server" CssClass="inputbox"></asp:TextBox><span style="color:Red">*</span>
		                <asp:HiddenField ID="hidToUser" runat="server"  Value=""/>
                        <input type='button' id="btnToUser" style='cursor: pointer; background: url(<%=AppPath%>App_Themes/gmcc/img/btnse01.jpg);border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;' />
		            </td>
                </tr>
		    </table>
			<div class="divSubmit">
				<asp:Button ID="btnSearch" Text="待办" CssClass="btn_query" runat="server" 
                    onclick="btnSearch_Click" ToolTip="查询被转移用户的待办" />&nbsp;
                <asp:Button ID="btnSearchDone" Text="已办" CssClass="btn_query" runat="server" 
                     ToolTip="查询被转移用户的已办" 
                    onclick="btnSearchDone_Click" />&nbsp;
			</div>
            <div class="divSubmit">
				 <asp:Button ID="btnTransferTodo" runat="server"
                         Text="转移待办" CssClass="btnClass2l" onclick="btnTransferTodo_Click" OnClientClick="return beforeTransfer()" />
                    &nbsp;
                        <asp:Button ID="btnTransferDone" runat="server"
                         Text="转移已办" CssClass="btnClass2l" onclick="btnTransferDone_Click" OnClientClick="return beforeTransfer()" />
			</div>
			<div>
			    <asp:ValidationSummary ID="vsummary1" runat="server" ShowMessageBox="true" ShowSummary="false" />
			</div>
		</div>
	</div>
	<div class="dataList" id="divResults" runat="server">
		<div class="showControl">
			<h4>查询结果</h4><button onclick="return showContent(this,'dataDiv1');" title="收缩"><span>
            折叠</span></button>
		</div>
		<div id="dataDiv1" style="overflow-x:auto">
			<div class="dataTable" id="dataTable1" style="min-width: 1280px">
				<table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align:center;">
		            <tr>
                        <th><input type="checkbox" title="全选" id="chkAll"/></th>
                        <th width="6%">类型</th>
                        <th width="25%">标题</th>
                        <th style="width:90px;">受理号</th>
                        <th style="width:60px;">发起人</th>
                        <th width="13%">当前步骤</th>
                        <th style="width:65px;">当前处理人</th>
                        <th width="13%"><asp:LinkButton ID="StartedTime" Runat="server" Text="创建日期" CommandName="StartedTime"></asp:LinkButton></th>
                                <th width="13%"><asp:LinkButton ID="FinishedTime" Runat="server" Text="处理时间" CommandName="FinishedTime"/></th>
                        <th style="width:40px">操作</th>
			        </tr>
				    <asp:Repeater ID="listResults" runat="server" 
                        onitemdatabound="listResults_ItemDataBound" 
                        onitemcommand="listResults_ItemCommand">
				        <ItemTemplate>
    				        <tr style="text-align:center;">
                                <td><input type="checkbox" title="选择以进行转移" name="chk" value="<%# Eval("WorkflowInstanceId")%>"/></td>
                                <%--<td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>--%>
    				            <td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>
    				            <td style="text-align:left;">
    				                <a href='process.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'><%# Eval("Title") %></a>
    				            </td>
    				            <td><%# Eval("SheetId")%></td>
    				            <td><%# Eval("CreatorName")%></td>
    				            <td><%# Eval("ActivityName")%></td>
    				            <td><%# FormatActors(Eval("CurrentActors").ToString())%></td>
    				            <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
    				            <td><%# Eval("FinishedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
    				            <td style="width:40px;">
    				                <input type="hidden" value="<%# Eval("WorkflowInstanceId") %>" name="inputWiid" />
    				                <asp:LinkButton ID="lbtnDelete" CommandArgument='<%# Eval("WorkflowInstanceId") %>' CssClass="ico_del" CommandName="delete" Text="转移" runat="server" OnClientClick="if(confirm('此操作将会删除表单的所有数据，您确实要删除吗？')){$('body').showLoading();}else{return false}"/>
    				            </td>
    				        </tr>				            
				        </ItemTemplate>
				        <AlternatingItemTemplate>
    				        <tr class="trClass" style="text-align:center;">
                                <td><input type="checkbox" title="选择以进行转移" name="chk" value="<%# Eval("WorkflowInstanceId")%>"/></td>
                                <td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>
    				            <td style="text-align:left;">
    				                <a href='process.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'><%# Eval("Title") %></a>
    				            </td>
    				            <td><%# Eval("SheetId")%></td>
    				            <td><%# Eval("CreatorName")%></td>
    				            <td><%# Eval("ActivityName")%></td>
    				            <td><%# FormatActors(Eval("CurrentActors").ToString())%></td>
    				            <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
    				            <td><%# Eval("FinishedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
    				            <td style="width:40px;">
    				                <input type="hidden" value="<%# Eval("WorkflowInstanceId") %>" name="inputWiid" />
    				                <asp:LinkButton ID="lbtnDelete" CommandArgument='<%# Eval("WorkflowInstanceId") %>' CssClass="ico_del" CommandName="delete" Text="转移" runat="server" OnClientClick="if(confirm('您确定转移吗？')){$('body').showLoading();}else{return false}"/>
    				            </td>
    				        </tr>	
				        </AlternatingItemTemplate>
				    </asp:Repeater>
				</table>
                <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
                    <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                        Font-Size="9pt" ItemsPerPage="20" PagerStyle="NumericPages" BorderWidth="0px"
                        OnPageIndexChanged="listPager_PageIndexChanged" />
                </div>
		    </div>
		 </div>
	</div>
<script language="javascript" type="text/javascript">
    $(function () {
        var aiids = "";
        /*$('#tblId1 tr').each(function() {
        var aiid = $(this).children('td').eq(8).html();
        if (aiid != null)
        aiids += aiid + ",";
        });
        jQuery.post("../../../../../contrib/workflow/pages/GetReminderTime.ashx", { "aiids": aiids }, function(data) {
        $.each(data.Table, function(idx, item) {
        $('#tblId1 tr').each(function() {
        var aiid = $(this).children('td').eq(8).html();
        if (aiid != null && aiid == item.WORKFLOWINSTANCEID) {
        $(this).children('td').eq(6).html(item.USEDTIME);
        if (item.USEDTIME < 0) {
        //$(this).css("font-weight","bold");
        //                $(this).css("color","Red");
        //                $((this).children('td')).next("a").removeClass("blue");
        //                $((this).children('td')).next("a").css("color","Red");
        $(this).children('td').eq(6).css("color", "Red");
        //$(this).children('td').eq(6).css("font-weight","bold");
        }
        }
        });
        });
        }, "json");*/

        $("input[name='inputWiid']").each(function () {
            //$(this).next("<a>").attr("href", "<%=AppPath %>apps/xqp2/pages/workflows/maintenance/process.aspx?wiid=" + $(this).val());
        });
        $("#chkAll").click(function () {
            if ($(this).attr("checked"))
                $("input[name='chk']").attr("checked", "checked")
            else {
                $("input[name='chk']").removeAttr("checked")
            }
        });
        $("#btnActor").click(function () {
            showDiv({ isorganization: 'False', tableName: 'bw_users', text: 'realname', value: 'username', fieldWhere: '' }, '<%=AppPath%>apps/pms/pages/GetMarkData.aspx', { hide: '<%=hidFromUser.ClientID%>', text: '<%=txtFromUser.ClientID%>' });
            $("img[alt='点击可以关闭']").attr("src", "<%=AppPath %>App_Themes/gmcc/img/close_1.jpg")
        });
        $("#btnToUser").click(function () {
            showDiv({ isorganization: 'False', tableName: 'bw_users', text: 'realname', value: 'username', fieldWhere: '', issimple: true }, '<%=AppPath%>apps/pms/pages/GetMarkData.aspx', { hide: '<%=hidToUser.ClientID%>', text: '<%=txtToUser.ClientID%>' });
            $("img[alt='点击可以关闭']").attr("src", "<%=AppPath %>App_Themes/gmcc/img/close_1.jpg")
        });
        $("#<%=btnSearch.ClientID %>,#<%=btnSearchDone.ClientID %>").click(function () {
            if ($("#<%=hidFromUser.ClientID %>").val() == "") {
                alert("请选择被转移用户后再查询。");
                return false;
            }
            else {
                $('body').showLoading()
                return true;
            }
        });
    });
    function beforeTransfer() {
        if ($("#<%=hidToUser.ClientID %>").val() == "") {
            alert("请选择转移对象。");
            return false;
        }
        var sheetId = $("input[name='chk'][checked]");
        //alert(sheetId)
        if (sheetId.length == 0) {
            /*if (confirm("不选择工单将会转移处理人的所有任务，您确实要转移吗？")) {

                $('body').showLoading()
                return true;
            }*/
            alert("请选择要转移的任务");
            return false;
        }
        else if (confirm("您确实要转移选中的任务吗？")) {
            $('body').showLoading()
            return true;
        }
        return false;
    }
</script>
</asp:Content>
