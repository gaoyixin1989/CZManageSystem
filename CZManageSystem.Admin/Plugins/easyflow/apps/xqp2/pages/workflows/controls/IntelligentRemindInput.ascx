<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_workflows_controls_IntelligentRemindInput" Codebehind="IntelligentRemindInput.ascx.cs" %>
 <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1">
    <tr>
        <th colspan="2" style="font-weight:bold; text-align:center">
            <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
        </th>
    </tr>
    <tr>
        <th style="width:20%">允许滞留时间(小时)：</th>
        <td>
            <asp:TextBox ID="txtStayHours" Width="50px" Text="-1" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>提醒次数：</th>
        <td>
            <asp:TextBox ID="txtRemindTimes" Width="50px" Text="-1" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>提醒方式：</th>
        <td>
            <asp:DropDownList ID="ddlRemindType" runat="server">
                <asp:ListItem Value="0">-未设置-</asp:ListItem>
                <asp:ListItem Value="1">电子邮件</asp:ListItem>
                <asp:ListItem Value="2">短信</asp:ListItem>
                <asp:ListItem Value="3">短信+电子邮件</asp:ListItem>
            </asp:DropDownList>
       </td>
    </tr>
</table>