<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_notices_controls_NoticeView" Codebehind="NoticeView.ascx.cs" %>
<table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1">
    <tr id="trTitle" runat="server" enableviewstate="false">
        <th width="10%">公告标题：</th>
         <td align="left">
            <asp:Literal ID="ltlTitle" EnableViewState="false" runat="server"></asp:Literal>
        </td>
    </tr>
    <asp:Literal ID="ltlEntity" runat="server" EnableViewState="false"></asp:Literal>
    <tr>
        <th> 开始时间：</th>
        <td align="left">
            <asp:Literal ID="ltlStartTime" EnableViewState="false" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr class="trClass">
        <th> 结束时间：</th>
        <td align="left">
            <asp:Literal ID="ltlEndTime" EnableViewState="false" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th>发 表 人：</th>
         <td align="left">
            <asp:Literal ID="ltlCreator" EnableViewState="false" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr class="trClass">
        <th>公告内容：</th>
        <td style="word-break: break-all;" align="left">
            <asp:Literal ID="ltlContent" EnableViewState="false" runat="server"></asp:Literal>
        </td>
    </tr>
</table>