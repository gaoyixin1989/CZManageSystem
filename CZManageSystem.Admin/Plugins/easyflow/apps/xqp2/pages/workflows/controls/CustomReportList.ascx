<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_controls_CustomReportList" Codebehind="CustomReportList.ascx.cs" %>
<div class="dataTable" id="dataTable1">
    <table cellpadding="0" cellspacing="0" style="text-align:center;" class="tblClass" id="tblId1">
        <tr>
            <th width="20%">报表名称</th>
            <th width="30%">备注</th>
            <th width="15%">创建时间</th>
            <th width="35%">操作</th>
        </tr>
        <asp:Repeater ID="rptReports" runat="server">
            <ItemTemplate>
                <tr>
                    <td style="text-align:left">
                        <a href="../../../../../contrib/report/pages/ReportTableView.aspx?id=<%# Eval("Id") %>"><%# Eval("Name") %></a>
                    </td>
                    <td style="text-align:left">
                    </td>
                    <td> <%# Eval("CreatedTime", "{0:yyyy-MM-dd}")%></td>
                    <td>
                        <a class="ico_edit" href="../../../../../contrib/report/pages/ReportEdit.aspx?id=<%# Eval("Id") %>">编辑报表</a>
                        <a class="ico_edit" href="../../../../../contrib/report/pages/TemplateConfig.aspx?id=<%# Eval("Id") %>">编辑模板</a>
                        <a class="ico_edit" href="../../../../../contrib/report/pages/DrawGrapConfig.aspx?id=<%# Eval("Id") %>">编辑图表</a>
                        <asp:LinkButton ID="lnkbtnDelete" runat="server" CssClass="ico_del" CommandName="delete" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('确定要删除改报表吗？');">删除</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="trClass">
                    <td style="text-align:left">
                        <a href="../../../../../contrib/report/pages/ReportTableView.aspx?id=<%# Eval("Id") %>"><%# Eval("Name") %></a>
                    </td>
                    <td style="text-align:left">
                    </td>
                    <td> <%# Eval("CreatedTime", "{0:yyyy-MM-dd}")%></td>
                    <td>
                        <a class="ico_edit" href="../../../../../contrib/report/pages/ReportEdit.aspx?id=<%# Eval("Id") %>">编辑报表</a>
                        <a class="ico_edit" href="../../../../../contrib/report/pages/TemplateConfig.aspx?id=<%# Eval("Id") %>">编辑模板</a>
                        <a class="ico_edit" href="../../../../../contrib/report/pages/DrawGrapConfig.aspx?id=<%# Eval("Id") %>">编辑图表</a>
                        <asp:LinkButton ID="lnkbtnDelete" runat="server" CssClass="ico_del" CommandName="delete" CommandArgument='<%# Eval("Id") %>'>删除</asp:LinkButton>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>                    
    </table>
</div>