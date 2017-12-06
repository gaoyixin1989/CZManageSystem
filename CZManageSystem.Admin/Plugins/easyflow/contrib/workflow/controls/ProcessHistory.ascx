<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_ProcessHistory" Codebehind="ProcessHistory.ascx.cs" %>
<table class="tblGrayClass" style="text-align:center" cellpadding="4" cellspacing="1">
	<tr style="text-align: center;">
		<th style="width:20%;">步骤</th>
		<th style="width:20%;" colspan="2">处理人</th>
		<th style="width:17%;">执行时间</th>
		<th style="width:43%;">处理意见</th>
	</tr>
	<asp:Literal ID="ltlHistoryLogs" runat="server"></asp:Literal>
</table>
