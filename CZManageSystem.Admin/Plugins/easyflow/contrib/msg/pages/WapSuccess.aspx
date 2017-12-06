<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Mobile.master" AutoEventWireup="true" Inherits="contrib_msg_WapSuccess" Codebehind="WapSuccess.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<script language="javascript" type="text/javascript">
<!--//
$(function() {
    $("#<%=btnBack.ClientID%>").focus();
    $('.loading-backdrop').hideLoading();
    $('.loading-backdrop').hide();
});
function redirectUrl(returnUrl) {
    $(".loading-backdrop").show();
    $(".loading-backdrop").showLoading();
    if(parent.rightFrame != null)
        parent.rightFrame.location.href = returnUrl;
    else
        window.location.href = returnUrl;
    return true;
}
//-->
</script>
<div class="page-header">
        <!-- Fixed navbar -->
        <div class="navbar navbar-inverse navbar-fixed-top ui-header" role="navigation">
            <h1 class="text-center ui-title" id="header" style="color: rgb(69, 125, 179);">
                系统提示
            </h1>
        </div>
    </div>
    <div class=" container-fluid theme-showcase" role="main" style="padding-top:6.5em">
    <div class="panel panel-success"><div class="panel-heading">提示：</div>
    <div class="panel-body">
    <p><asp:Literal id="ltlMessage" runat="server" /></p>
</div>
  <div class="panel-footer"style="text-align:center;"><input id="btnBack" type="button" class="btn btn-default" value="返回" runat="server" /></div>
</div></div>

</asp:Content>

