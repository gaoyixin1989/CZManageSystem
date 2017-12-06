<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_ReviewHistory" Codebehind="ReviewHistory.ascx.cs" %>
<div class="showControl">
    <h4>查阅信息</h4>
    <button onclick="return showContent(this,'dataReviewHistoryList');" title="收缩"><span></span></button>
</div>
<div class="dataTable" id="dataReviewHistoryList">
    <table class="tblGrayClass" style="text-align:center" cellpadding="4" cellspacing="1">
	    <tr style="text-align: center;">
		    <th style="width:20%;">步骤</th>
		    <th style="width:12%;">提交人</th>
		    <th style="width:20%;" colspan="2">阅读人</th>
		    <th style="width:17%;">阅读时间</th>
		    <th style="width:31%;"></th>
	    </tr>
	    <asp:Literal ID="ltlHistoryLogs" runat="server"></asp:Literal>
    </table>
</div>