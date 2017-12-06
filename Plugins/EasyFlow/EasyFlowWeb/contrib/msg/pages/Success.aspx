<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_msg_Success" Codebehind="Success.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<script  language="javascript" type="text/javascript">
<!--//
$(function() {   
	$("#<%=btnBack.ClientID%>").focus();
});
function redirectUrl(returnUrl){
    if(parent.rightFrame != null)
        parent.rightFrame.location.href = returnUrl;
    else
        window.location.href = returnUrl;
    return true;
}
//-->
</script>
<div class="infoBox">
	<div class="title">
		<span>系统提示</span>
	</div>
	<div class="Inner blackbox_icon_success blackbox_icon">
		
		<p><asp:Literal id="ltlMessage" runat="server" /></p>
        <div class="BlackBoxAction">
		    <a id="btnBack"  class="btnReturn" value="确定" runat="server" >确定</a>
        </div>
	</div>
    <div class="close">×</div>
</div>
</asp:Content>

