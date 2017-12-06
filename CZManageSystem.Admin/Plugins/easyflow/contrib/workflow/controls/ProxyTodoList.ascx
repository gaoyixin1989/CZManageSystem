<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_ProxyTodoList" Codebehind="ProxyTodoList.ascx.cs" %>
<div class="showControl">
    <h4>授权的未处理任务列表</h4>
    <button onclick="return showContent(this,'divProxyTodo1');" title="收缩"><span>折叠</span></button>
</div>   
<div id="divProxyTodo1">
    <table class="tblGrayClass" style="text-align:center;" cellpadding="4" cellspacing="1">
        <tr>
            <th width="8%">类别</th>
            <th width="43%">标题</th>
            <th width="10%">受理号</th>
            <th width="19%">当前步骤</th>
            <th width="10%">发起人</th>
            <th width="10%">被授权人</th>
        </tr>
        <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                    </td>
                    <td style="text-align:left; font-weight:bold">
                        <asp:Literal ID="ltlActivityIcons" runat="server"></asp:Literal>
                        <a href="<%# BuildProcessUrl(Eval("ActivityInstanceId"))%>">
                            <asp:Label ID="lbTitle" runat="server" Text='<%# Eval("Title")%>'></asp:Label></a>
                    </td>
                    <td>
                        <a href="<%# BuildProcessUrl(Eval("ActivityInstanceId"))%>"><%# Eval("SheetId")%></a>
                    </td>
                    <td>
                        <a href="<%# BuildProcessUrl(Eval("ActivityInstanceId"))%>">
                            <asp:Literal ID="ltlActivityName" runat="server" Text='<%# Eval("ActivityName")%>'></asp:Literal>
                        </a>
                    </td>               
                    <td><span tooltip="<%# Eval("Creator")%>"><%# Eval("CreatorName")%></span></td>
                    <td><span tooltip="<%# Eval("UserName")%>"><%# Eval("ActorName")%></span></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="trClass">
                    <td>
                        <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                    </td>
                    <td style="text-align:left; font-weight:bold">
                        <asp:Literal ID="ltlActivityIcons" runat="server"></asp:Literal>
                        <a href="<%# BuildProcessUrl(Eval("ActivityInstanceId"))%>">
                            <asp:Label ID="lbTitle" runat="server" Text='<%# Eval("Title")%>'></asp:Label></a>
                    </td>
                    <td>
                        <a href="<%# BuildProcessUrl(Eval("ActivityInstanceId"))%>"><%# Eval("SheetId")%></a>
                    </td>
                    <td>
                        <a href="<%# BuildProcessUrl(Eval("ActivityInstanceId"))%>">
                            <asp:Literal ID="ltlActivityName" runat="server" Text='<%# Eval("ActivityName")%>'></asp:Literal>
                        </a>
                    </td>
                    <td><span tooltip="<%# Eval("Creator")%>"><%# Eval("CreatorName")%></span></td>
                    <td><span tooltip="<%# Eval("UserName")%>"><%# Eval("ActorName")%></span></td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
    </table>
    <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
        <bw:VirtualPager ID="listPagerTodoTask" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
            Font-Size="9pt" ItemsPerPage="10" PagerStyle="NumericPages" BorderWidth="0px" OnPageIndexChanged="listTodoPager_PageIndexChanged" />
    </div>
</div> 
