<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_SearchByOrg" Codebehind="SearchByOrg.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <link href="<%=AppPath%>res/js/jquery.editable-select.css"
        rel="stylesheet" type="text/css" />
    <script src="<%=AppPath%>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.js"
        type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.pack.js"
        type="text/javascript"></script>
    <ajaxToolkit:ToolkitScriptManager ID="toolkitScriptManager1" runat="server" />
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlSearchOrg" runat="server"></asp:Literal></span></h3>
    </div>
	<div class="searchBar">
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
		            </td>
		            <td>至</td>
		            <td>
		                <bw:DateTimePicker ID="dtpStart2" runat="server" InputBoxCssClass="inputbox"  IsRequired="False" ExpressionValidatorText="*" ExpressionErrorMessage="工单发起时间的截止时间日期格式错误." />
		            </td>
		        </tr>
		        <tr>
		            <td>流程：</td>
		            <td>
		                <asp:DropDownList ID="ddlWorkflowList" runat="server" CssClass="editable-select" AutoPostBack="True" onselectedindexchanged="ddlWorkflowList_SelectedIndexChanged">
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
                            ServicePath="WorkflowAjaxService.asmx" ServiceMethod="GetCompletionUserNames" 
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
		                <asp:TextBox ID="txtActor" runat="server" CssClass="inputbox"></asp:TextBox>
		                <ajaxToolkit:AutoCompleteExtender ID="autoCompleteActor" runat="server" 
                            BehaviorID="ActorAutoCompleteEx"  TargetControlID="txtActor" 
                            ServicePath="WorkflowAjaxService.asmx" ServiceMethod="GetCompletionUserNames" 
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
		            </td>
		        </tr>
		        <tr>
		            <td>标题关键字：</td>
		            <td>
		                <asp:TextBox ID="txtTitleKeywords" runat="server" CssClass="inputbox"></asp:TextBox>
		                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtenderTitle" runat="server" 
                            BehaviorID="TitleAutoCompleteEx"  TargetControlID="txtTitleKeywords" 
                            ServicePath="WorkflowAjaxService.asmx" ServiceMethod="GetCompletionWorkflowTitles" 
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
                    onclick="btnSearch_Click" />
			</div>
			<div>
			    <asp:ValidationSummary ID="vsummary1" runat="server" ShowMessageBox="true" ShowSummary="false" />
			</div>
		</div>
	</div>
	<div class="dataList" id="divResults" runat="server" visible="false">
		<div class="showControl">
			<h4>查询结果</h4>
			<button onclick="return showContent(this,'dataDiv1');" title="收缩"><span>折叠</span></button>
		</div>
		<div id="dataDiv1">
			<div class="dataTable" id="dataTable1">
				<table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align:center;">
				    <asp:Repeater ID="listResults" runat="server" onitemdatabound="listResults_ItemDataBound" OnItemCommand="listResults_ItemCommand">
				        <HeaderTemplate>
		                    <tr>
                                <th width="6%">类型</th>
                                <th >标题</th>
                                <th width="10%">受理号</th>
                                <th width="10%">发起人</th>
                                <th width="13%">当前步骤</th>
                                <th width="13%">当前处理人</th>
                                <th width="13%"><asp:LinkButton ID="StartedTime" Runat="server" Text="创建日期" CommandName="StartedTime"></asp:LinkButton></th>
                                <th width="13%"><asp:LinkButton ID="FinishedTime" Runat="server" Text="处理时间" CommandName="FinishedTime"/></th>
			                </tr>
			            </HeaderTemplate>
				    
				        <ItemTemplate>
    				        <tr style="text-align:center;">
                                <td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>
    				            <td style="text-align:left;">
    				                <a href='WorkflowView.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'><%# Eval("Title") %></a>
    				            </td>
    				            <td><%# Eval("SheetId")%></td>
    				            <td><%# Eval("CreatorName")%></td>
    				            <td><%# Eval("ActivityName")%></td>
    				            <td><%# FormatActors(Eval("CurrentActors").ToString())%></td>
    				            <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
    				            <td><%# Eval("FinishedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
    				        </tr>				            
				        </ItemTemplate>
				        <AlternatingItemTemplate>
    				        <tr class="trClass" style="text-align:center;">
                                <td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>
    				            <td style="text-align:left;">
    				                <a href='WorkflowView.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'><%# Eval("Title") %></a>
    				            </td>
    				            <td><%# Eval("SheetId")%></td>
    				            <td><%# Eval("CreatorName")%></td>
    				            <td><%# Eval("ActivityName")%></td>
    				            <td><%# FormatActors(Eval("CurrentActors").ToString())%></td>
    				            <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
    				            <td><%# Eval("FinishedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
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
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=ddlWorkflowList.ClientID %>').editableSelect({
                onSelect: function (list_item) {
                    this.select.val(this.text.val());
                    //if (this.text.attr("id") == "datasource")
                       // getField(document.getElementById("datasource"));

                       setTimeout('__doPostBack(\''+"<%=ddlWorkflowList.ClientID %>".replace("_","$")+'\',\'\')', 0)
                }
            })
            $(".editable-select-options").css("text-align","left");
            if ($("#<%=ddlWorkflowList.ClientID %>").editableSelectInstances()[0])
                $("#<%=ddlWorkflowList.ClientID %>").editableSelectInstances()[0].text[0].value = "<%=Request.QueryString["wname"]%>";
        });
    </script>
</asp:Content>
