<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Mobile.master" AutoEventWireup="true" Inherits="contrib_mobile_pages_Notify" Title="发送消息通知" Codebehind="Notify.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>工单处理信息提示</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>工单已成功处理并提交到以下步骤及处理人</h4>
        </div>
        <table class="tblGrayClass" style="text-align:center" cellpadding="4" cellspacing="1">	        
	        <asp:Repeater ID="rptList" runat="server">
	            <HeaderTemplate>
	                <tr style="text-align: center;">
		                <th style="width:30%;">步骤</th>
		                <th style="width:70%;">处理人</th>
	                </tr>
	            </HeaderTemplate>
	            <ItemTemplate>
	                <tr>
		                <td><%# Eval("Key") %></td>
		                <td><%# Eval("Value") %></td>
	                </tr>
	            </ItemTemplate>
	            <AlternatingItemTemplate>
	                <tr class="trClass">
		                <td><%# Eval("Key") %></td>
		                <td><%# Eval("Value") %></td>
	                </tr>
	            </AlternatingItemTemplate>
	        </asp:Repeater>
        </table>
        <br />
        <div id="divReviewActors" runat="server">
            <div class="showControl">
                <h4>工单已成功处理并抄送给以下待阅人</h4>
            </div>
            <table class="tblGrayClass" style="text-align:center" cellpadding="4" cellspacing="1">	        
	            <asp:Repeater ID="rptReviewActors" runat="server">
	                <HeaderTemplate>
	                    <tr style="text-align: center;">
		                    <th style="width:30%;">步骤</th>
		                    <th style="width:70%;">待阅人</th>
	                    </tr>
	                </HeaderTemplate>
	                <ItemTemplate>
	                    <tr>
		                    <td><%# Eval("Key") %></td>
		                    <td><%# Eval("Value") %></td>
	                    </tr>
	                </ItemTemplate>
	                <AlternatingItemTemplate>
	                    <tr class="trClass">
		                    <td><%# Eval("Key") %></td>
		                    <td><%# Eval("Value") %></td>
	                    </tr>
	                </AlternatingItemTemplate>
	            </asp:Repeater>
            </table>
        </div>
        <br />
        <div style="text-align:center;">
            <input id="btnBackTodo" type="button" class="btnClass2m" value="返回待办" onclick="location = 'default.aspx';" />&nbsp;&nbsp;&nbsp;
            <input id="btnBackAppl" type="button" class="btnClass2m" value="返回工单" onclick="location = '<%=ReturnUrl%>';" />&nbsp;&nbsp;&nbsp;
             <%--<input id="btnPush" type="button" class="btnClass2m" value="抄送他人" onclick="location = '<%=AppPath%>contrib/workflow/pages/NotifyReader.aspx?aiid=<%=aiid %>';" />--%>
        </div>
    </div>
</asp:Content>
