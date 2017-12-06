<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_WorkflowWorkItem" Codebehind="WorkflowWorkItem.ascx.cs" %>
<table  cellpadding="4" cellspacing="1" class="tblGrayClass grayBackTable">
    <tr>
        <th width="13%">标   题：</th>
        <td colspan="3" id="tdTitle" runat="server">
            <asp:TextBox ID="txtTitle" runat="server" CssClass="inputbox" Width="290px" MaxLength="50"></asp:TextBox>
            <span class="require">*</span><asp:RegularExpressionValidator ID="revTitle" runat="server"
                ControlToValidate="txtTitle" ValidationExpression="^[^']*$" ErrorMessage="标题不能包含单引号!"></asp:RegularExpressionValidator>
        </td>
        <th style="width:13%;" id="thSheetId" runat="server">受理号：</th>
        <td id="tdSheetId" runat="server" style="width:20%;">
            &nbsp;
        </td>
    </tr>
    <asp:Literal ID="ltlBasicHtml" runat="server" />
    <tr>
        <th>当前流程：</th>
        <td colspan="5" style="font-weight:bold;color:blue"><asp:Literal ID="ltlWorkflowName" runat="server"></asp:Literal></td>
    </tr>
</table>
<script type="text/javascript">
$(function(){
  $("#<%=txtTitle.ClientID%>").keypress(function(){
      if(event.keyCode == 13)
        return false;
      return true;
  });
});
</script>