<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_List" Codebehind="List.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" runat="server"></asp:Literal>表单管理</span></h3>
    </div>
    <div class="btnControl">
        <div class="btnLeft">
            <input type="button" value="导入表单" class="btnNewwin" onclick="window.location.href='import.aspx?wid=<%=WorkflowId%>';" />
            <input type="button" value="返回" class="btnNewwin" onclick="window.location.href='itemcreate.aspx?wid=<%=WorkflowId%>&EntityType=Form_Workflow';" />
        </div>
    </div>
    
    <div class="dataList"  style="padding-top:5px">   
		<div class="showControl">
            <h4>表单版本列表</h4>
        </div>
        <asp:HiddenField ID="hiddenWorkflowId" runat="server" />
        <table class="tblGrayClass" style="text-align: center; width:100%;margin-top:5px;" cellpadding="4" cellspacing="0">
            <tr style="text-align:center;">
	            <th style="width:50px;">版本号</th>
	            <th style="text-align:left">表单名称</th>
	            <th style="width:80px;">是否当前版本</th>
	            <th style="width:120px;">最后更新人</th>
	            <th style="width:120px;">最后更新时间</th>
	            <th style="width:180px;">操作</th>
            </tr>
            <tbody>
                <asp:Repeater ID="rptForms" runat="server" OnItemCommand="rptForms_ItemCommand" onitemdatabound="rptForms_ItemDataBound">
                    <ItemTemplate>
                    <tr>
                        <td><%# Eval("Version") %></td>
                        <td style="text-align:left;"><%# Eval("Name") %></td>
                        <td><%# FormatBoolean(Eval("IsCurrentVersion"))%></td>
                        <td><%# Eval("LastModifier") %></td>
                        <td><%# Eval("LastModTime", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                        <td style="text-align:left; padding-left:3px;">
                            <asp:LinkButton CssClass="ico_idt" ToolTip="导出表单定义为 Excel" ID="linkExport" runat="server" CommandName="Export" CommandArgument='<%# Eval("Id") %>'>导出</asp:LinkButton>
                            <a class="ico_edit" title="重新设计表单" href="ItemCreate.aspx?wid=<%=WorkflowId%>&fdid=<%# Eval("Id") %>">设计</a>
                            <asp:LinkButton CssClass="ico_del" ToolTip="删除" ID="linkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('确定要删除该表单？');">删除</asp:LinkButton>
                            <asp:LinkButton CssClass="ico_enable" ToolTip="设置为当前版本" ID="linkSetCurrent" runat="server"
                                CommandName="SetCurrentVersion" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('确定要设置为当前版本？');">设置</asp:LinkButton>
                        </td>
                    </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                    <tr class="trClass">
                        <td><%# Eval("Version") %></td>
                        <td style="text-align:left;"><%# Eval("Name") %></td>
                        <td><%# FormatBoolean(Eval("IsCurrentVersion"))%></td>
                        <td><%# Eval("LastModifier") %></td>
                        <td><%# Eval("LastModTime", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                        <td style="text-align:left; padding-left:3px;">
                            <asp:LinkButton CssClass="ico_idt" ToolTip="导出表单定义为 Excel" ID="linkExport" runat="server" CommandName="Export" CommandArgument='<%# Eval("Id") %>'>导出</asp:LinkButton>
                            <a class="ico_edit" title="重新设计表单" href="ItemCreate.aspx?wid=<%=WorkflowId%>&fdid=<%# Eval("Id") %>">设计</a>
                            <asp:LinkButton CssClass="ico_del" ToolTip="删除" ID="linkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('确定要删除该表单？');">删除</asp:LinkButton>
                            <asp:LinkButton CssClass="ico_enable" ToolTip="设置为当前版本" ID="linkSetCurrent" runat="server" 
                                CommandName="SetCurrentVersion" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('确定要设置为当前版本？');">设置</asp:LinkButton>
                        </td>
                    </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div style="margin-top:10px; height:1px; border-top:solid 1px #C0CEDF;">
        </div>
    </div>
</asp:Content>
