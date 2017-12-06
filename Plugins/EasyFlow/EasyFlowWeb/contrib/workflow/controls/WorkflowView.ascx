<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_WorkflowView" Codebehind="WorkflowView.ascx.cs" %>
<table  cellpadding="4" cellspacing="1" class="tblGrayClass grayBackTable">
    <tr>
        <th style="width:13%">标   题：</th>
        <td colspan="3" id="tdTitle" runat="server">
            <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
        </td>
        <th style="width:13%;" id="th1" runat="server">受理号：</th>
        <td style="width:20%;">
            <asp:Literal ID="ltlSheetId" runat="server"></asp:Literal>
        </td>        
    </tr>
    <%=BasicInfoHtml %>
    <tr>
        <th>当前流程：</th>
        <td colspan="5" style="font-weight:bold;color:blue"><asp:Literal ID="ltlWorkflowName" runat="server"></asp:Literal></td>
    </tr>
</table>
