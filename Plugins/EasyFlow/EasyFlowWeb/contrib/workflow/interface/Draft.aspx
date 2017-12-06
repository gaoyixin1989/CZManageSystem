<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_interface_Draft" Codebehind="Draft.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<div class="module_div">
	<div class="titleContent">
        <h3><span>草稿箱</span></h3>
    </div>
	<div class="dataList">
	    <table id="dtWfSheets" class="tblClass" cellpadding="0" cellspacing="0" style="text-align:center;">
            <tr>
              <th width="8%">类别</th>
              <th width="32%">标题</th>
              <th width="20%">受理号</th>
              <th width="20%">创建时间</th>
              <th width="20%">操作</th>
            </tr>
	        <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound" OnItemCommand="rptList_ItemCommand">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                        </td>
                        <td style="text-align:left;"><a href='start.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'><%# Eval("Title")%></a></td>
                        <td><%# Eval("SheetId") %></td>
                        <td><%# Eval("StartedTime", "{0:yyyy-MM-dd}")%></td>
                        <td>
                            <a class="ico_index" href='start.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'>发起</a>
                            <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("WorkflowInstanceId") %>'
                                ID="btnDelete" CssClass="ico_del" OnClientClick="return (confirm('确定删除该草稿吗？'));"
                                runat="server">删除</asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trClass">
                        <td>
                            <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                        </td>                  
                        <td style="text-align:left;"><a href='../pages/start.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'><%# Eval("Title")%></a></td>
                        <td><%# Eval("SheetId") %></td>
                        <td><%# Eval("StartedTime", "{0:yyyy-MM-dd}")%></td>
                        <td>
                            <a class="ico_index" href='start.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'>发起</a>
                            <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("WorkflowInstanceId") %>'
                                ID="btnDelete" CssClass="ico_del" OnClientClick="return (confirm('确定删除该草稿吗？'));"
                                runat="server">删除</asp:LinkButton>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </table>        
	</div>
</div>
</asp:Content>
