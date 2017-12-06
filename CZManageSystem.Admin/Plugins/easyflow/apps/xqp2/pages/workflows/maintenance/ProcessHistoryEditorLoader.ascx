<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_ProcessHistoryEditorLoader" Codebehind="ProcessHistoryEditorLoader.ascx.cs" %>
<table class="tblGrayClass" style="text-align:center" cellpadding="4" cellspacing="1" id="tblHistoryEditor">
	<tr style="text-align: center;">
		<th style="width:20%;">步骤</th>
		<th style="width:15%;" colspan="2">处理人</th>
		<th style="width:15%;">执行时间</th>
        <th style="width:20px;"><input type="checkbox" id="chkHistoryEditor" onclick="maintenance.CheckOptions(this)" title="选择删除全部的处理意见。"  style="display:none;"/></th>
		<th style="width:35%;">处理意见</th>
	</tr>
<asp:PlaceHolder ID="historyHolder" runat="server">
</asp:PlaceHolder>
<tr style="text-align: right;">
		<th  colspan="7"><input type="button" value="编辑" class="btn_edit" onclick="maintenance.EditOptions()" />
        <asp:Button ID="btnDelOption" runat="server" CssClass="btn_del" Text="删除" 
                OnClientClick="return confirm('确定删除选中的处理记录吗？')" onclick="btnDelOption_Click" />
        <input type="button" value="取消" class="btn_del" onclick="window.location.reload()" /></th>
	</tr>
</table>