<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_maintenance_Search" Codebehind="Search.aspx.cs" %>
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
            <h3><span style="font-weight:bolder;color:Black;">工单管理</span></h3>
            <%--<h3><span style="font-size:11px;"><a href="SearchHistory.aspx">历史工单管理</a></span></h3>--%>
        </div>
		<div class="searchTitle">
			<h4>组合条件查询<asp:Literal ID="ltlSearchName" runat="server"></asp:Literal></h4>
			<button onclick="return showSearch(this);" title="收缩"><span>折叠</span></button>
		</div>
		<div class="searchInfo" id="searchBody">
		    <table cellpadding="3" cellspacing="1">
		        <tr>
		            <td>工单发起时间：</td>
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
		            <td>发起人：</td>
		            <td>
		                <asp:TextBox ID="txtCreator" runat="server" CssClass="inputbox"></asp:TextBox>
		                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtenderCreator" runat="server" 
                            BehaviorID="CreatorAutoCompleteEx"  TargetControlID="txtCreator" 
                            ServicePath="<%=AppPath%>contrib/workflow/pages/WorkflowAjaxService.asmx" ServiceMethod="GetCompletionUserNames" 
                            MinimumPrefixLength="1" CompletionInterval="1000" 
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
                                            var behavior = $find('CreatorAutoCompleteEx');
                                            if (!behavior._height) {
                                                var target = behavior.get_completionList();
                                                behavior._height = target.offsetHeight - 2;
                                                target.style.height = '0px';
                                            }" />
                                        <Parallel Duration=".4">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('CreatorAutoCompleteEx')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <Parallel Duration=".4">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('CreatorAutoCompleteEx')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                            </Animations>
                        </ajaxToolkit:AutoCompleteExtender>
		            </td>
		            <td>处理人：</td>
		            <td>
		                <asp:UpdatePanel ID="updatepanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
		                    <ContentTemplate>
		                <asp:TextBox ID="txtActor" runat="server" CssClass="inputbox"></asp:TextBox>
		                <ajaxToolkit:AutoCompleteExtender ID="autoCompleteActor" runat="server" 
                            BehaviorID="ActorAutoCompleteEx"  TargetControlID="txtActor" 
                            ServicePath="<%=AppPath%>contrib/workflow/pages/WorkflowAjaxService.asmx" ServiceMethod="GetCompletionUserNames" 
                            MinimumPrefixLength="1" CompletionInterval="1000" 
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
                                            var behavior = $find('ActorAutoCompleteEx');
                                            if (!behavior._height) {
                                                var target = behavior.get_completionList();
                                                behavior._height = target.offsetHeight - 2;
                                                target.style.height = '0px';
                                            }" />
                                        <Parallel Duration=".4">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('ActorAutoCompleteEx')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <Parallel Duration=".4">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('ActorAutoCompleteEx')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                            </Animations>
                        </ajaxToolkit:AutoCompleteExtender>
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
		        <tr>
		            <td>内容关键字：</td>
		            <td colspan="3">
		                <asp:TextBox ID="txtContentKeywords" runat="server" CssClass="inputbox"></asp:TextBox>
		            </td>
		        </tr>
		    </table>
			<div class="divSubmit">
				<asp:Button ID="btnSearch" Text="搜索" CssClass="btn_query" runat="server" 
                    onclick="btnSearch_Click" OnClientClick="$('body').showLoading()" />&nbsp;
                <asp:Button ID="btnDel" Text="删除" CssClass="btn_del" OnClientClick="return beforeTransfer()" runat="server" 
                    onclick="btnDelete_Click"/>&nbsp;
			</div>
			<div>
			    <asp:ValidationSummary ID="vsummary1" runat="server" ShowMessageBox="true" ShowSummary="false" />
			</div>
		</div>
	</div>
	<div class="dataList" id="divResults" runat="server" visible="false">
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
                                <td><input type="checkbox" title="选择以进行删除" name="chk" value="<%# Eval("WorkflowInstanceId")%>"/></td>
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
    				                <bw:AccessController ID="AccessController25" ResourceValue="M001" runat="server">
                                        <ContentTemplate>
    				                        <a href='' class="ico_edit" title='修改'>修改</a>
    				                    </ContentTemplate>
    				                </bw:AccessController>
    				                </br>
    				                <asp:LinkButton ID="lbtnDelete" CommandArgument='<%# Eval("WorkflowInstanceId") %>' CssClass="ico_del" CommandName="delete" Text="删除" runat="server" OnClientClick="if(confirm('此操作将会删除表单的所有数据，您确实要删除吗？')){$('body').showLoading();}else{return false}"/>
    				            </td>
    				        </tr>				            
				        </ItemTemplate>
				        <AlternatingItemTemplate>
    				        <tr class="trClass" style="text-align:center;">
                                <td><input type="checkbox" title="选择以进行删除" name="chk" value="<%# Eval("WorkflowInstanceId")%>"/></td>
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
    				                <bw:AccessController ID="AccessController25" ResourceValue="M001" runat="server">
                                        <ContentTemplate>
    				                        <a href='' class="ico_edit" title='修改'>修改</a>
    				                    </ContentTemplate>
    				                </bw:AccessController>
    				                </br>
    				                <asp:LinkButton ID="lbtnDelete" CommandArgument='<%# Eval("WorkflowInstanceId") %>' CssClass="ico_del" CommandName="delete" Text="删除" runat="server" OnClientClick="if(confirm('此操作将会删除表单的所有数据，您确实要删除吗？')){$('body').showLoading();}else{return false}"/>
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
	</div>
<script language="javascript" type="text/javascript">
    $(function() {
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

        $("input[name='inputWiid']").each(function() {
            $(this).next("<a>").attr("href", "<%=AppPath %>apps/xqp2/pages/workflows/maintenance/process.aspx?wiid=" + $(this).val());
        });
        $("#chkAll").click(function () {
            if ($(this).attr("checked"))
                $("input[name='chk']").attr("checked", "checked")
            else {
                $("input[name='chk']").removeAttr("checked")
            }
        });
    });
    function beforeTransfer() {
        var sheetId = $("input[name='chk'][checked]");
        //alert(sheetId)
        if (sheetId.length == 0) {
            alert('请选择要删除的工单');
            return false;
        }
        if (confirm("此操作将会删除表单的所有数据，您确实要删除吗？")) {
            $("#<%=btnDel.ClientID %>").hide();
            $('body').showLoading()
            return true;
        }
        return false;
    }
</script>
</asp:Content>
