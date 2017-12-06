<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" ValidateRequest="false" AutoEventWireup="true" Inherits="contrib_security_pages_Authorize" Title="委托授权" Codebehind="Authorize.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="bw" TagName="AuthorizeHistory" Src="../controls/AuthorizeHistory.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="toolkitScriptManager1" runat="server" />
    <div class="titleContent">
        <h3><span>委托授权</span></h3>
    </div>    
    <div>        
        <div class="dataList">
            <div class="showControl">
                <h4>新增授权</h4>
                <button onclick="return showContent(this,'authorizationInput');" title="收缩"><span>折叠</span></button>
            </div>            
            <table id="authorizationInput" class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1">
                <tr>
                    <th width="15%" align="center">被授权用户</th>
                    <td>
                        <asp:TextBox ID="textName" CssClass="inputbox" Width="90%" runat="server"  autocomplete="off"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvRoleName" runat="server" ControlToValidate="textName" Display="Dynamic"
                                ErrorMessage="必须输入被授权人。"></asp:RequiredFieldValidator>
                        </div>
                        <ajaxToolkit:AutoCompleteExtender ID="autoComplete1" runat="server" 
                            BehaviorID="AutoCompleteEx"  TargetControlID="textName" 
                            ServicePath="SecurityAjaxService.asmx" ServiceMethod="GetUserCompletionList" 
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
                                            var behavior = $find('AutoCompleteEx');
                                            if (!behavior._height) {
                                                var target = behavior.get_completionList();
                                                behavior._height = target.offsetHeight - 2;
                                                target.style.height = '0px';
                                            }" />
                                        <Parallel Duration=".4">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <Parallel Duration=".4">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                            </Animations>
                        </ajaxToolkit:AutoCompleteExtender>
                        
                    </td>
                </tr>
                <tr class="trClass">
                    <th align="center">开始时间</th>
                    <td>
                        <bw:DateTimePicker ID="dtpBeginTime" runat="server" InputBoxCssClass="inputbox" IsRequired="true" RequiredErrorMessage="必须输入开始时间." ExpressionErrorMessage="开始时间日期格式错误." DateType="Datetime" />
                    </td>
                </tr>
                <tr>
                    <th align="center">截止时间</th>
                    <td>
                        <bw:DateTimePicker ID="dtpEndTime" runat="server" InputBoxCssClass="inputbox" IsRequired="true" RequiredErrorMessage="必须输入截止时间." ExpressionErrorMessage="截止时间日期格式错误." DateType="Datetime" />
                    </td>
                </tr>
                <tr class="trClass">
                    <th align="center">授权类型</th>
                    <td>
                        <asp:RadioButton ID="radionAuthrizationFull" runat="server" GroupName="authrizationType" /> 完全授权
                        <asp:RadioButton ID="radionAuthrizationHalf" runat="server" Checked="true" GroupName="authrizationType" /> 基本授权（只允许处理待办任务）

                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-left:100px">
                        <asp:Button ID="btnInsert" runat="server" CssClass="btn_sav" Text="保存"  onclick="btnInsert_Click" />
                        （注：只能授权给一个用户）
                    </td>
                </tr>
            </table>
        </div>
        
        <div class="dataList">
            <div class="showControl">
                <h4>授权记录</h4>
                <button onclick="return showContent(this,'authorizationList');" title="收缩"><span>折叠</span></button>
            </div>    
            <div id="authorizationList">      
                <table class="tblGrayClass" style="text-align:center;" cellpadding="4" cellspacing="1">
                    <tr>
                      <th width="10%">被授权用户</th>
                      <th width="33%">所属部门</th>
                      <th width="18%">起始时间</th>
                      <th width="18%">结束时间</th>
                      <th width="8%">是否有效</th>
                      <th width="8%">完全授权</th>
                      <th width="5%">操作</th>
                    </tr>
                    <asp:Repeater ID="authorizationRepeater" runat="server" onitemcommand="authorizationRepeater_ItemCommand" onitemdatabound="authorizationRepeater_ItemDataBound">
                        <ItemTemplate>
                            <tr class="trClass">
                                <td><%# Eval("ToRealName") %></td>
                                <td style="text-align:left;"><%# Eval("ToDpFullName") %></td>
                                <td><%# Eval("BeginTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                <td><%# Eval("EndTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                <td><asp:Literal ID="ltlEnabled" runat="server" /></td>
                                <td><asp:Literal ID="ltlIsFullAuthorized" runat="server" /></td>
                                <td>
                                    <asp:LinkButton ID="cmd1" CausesValidation="false" CommandName="SetEnabled" CommandArgument='<%# Eval("Id") %>' runat="server">禁用</asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>               
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
</asp:Content>
