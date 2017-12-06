<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_WorkflowStat" Codebehind="WorkflowStat.ascx.cs" %>
<table cellpadding="4" cellspacing="1" class="tblGrayClass" id="tblId1" style="text-align: center;">
    <tr>
        <th>步骤名称</th>
        <th>工单数</th>
    </tr>
    <asp:Repeater ID="rptWorkflowStat" runat="server">
        <ItemTemplate>
            <tr style="text-align: center;">
                <td>
                    <a href="../search.aspx?bt=&et=&wname=<%=WorkflowName %>&aname=<%# Eval("StatName") %>&c=&a=&tk=&ck=&i=" target="_parent">
                        <%# Eval("StatName")%></a>
                </td>
                <td><%# Eval("StatInstance")%></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="trClass" style="text-align: center;">
                <td>
                    <a href="../search.aspx?bt=&et=&wname=<%=WorkflowName %>&aname=<%# Eval("StatName") %>&c=&a=&tk=&ck=&i=" target="_parent">
                        <%# Eval("StatName")%></a>
                </td>
                <td><%# Eval("StatInstance")%></td>
            </tr>
        </AlternatingItemTemplate>
    </asp:Repeater>
</table>
