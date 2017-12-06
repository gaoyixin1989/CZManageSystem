<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_controls_WorkflowCreationControl" Codebehind="WorkflowCreationControl.ascx.cs" %>
<table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1">
    <tr>
        <th colspan="2" style="font-weight:bold;text-align:center ">
            <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
        </th>
    </tr>
    <tr>
        <th style="width:20%">每月最大发单数：</th>
        <td>
            <asp:TextBox ID="txtMonthCount" Width="50px" Text="-1" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>每周最大发单数：</th>
        <td>
            <asp:TextBox ID="txtWeekCount" Width="50px" Text="-1" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>发单控制类型：</th>
        <td>
            <asp:DropDownList ID="ddlCreationTypes" runat="server">
                <asp:ListItem Value="">默认</asp:ListItem>
                <asp:ListItem Value="user">用户控制</asp:ListItem>
                <asp:ListItem Value="dept">部门控制</asp:ListItem>
                <asp:ListItem Value="room">科室控制</asp:ListItem>
            </asp:DropDownList>
       </td>
    </tr>
</table>