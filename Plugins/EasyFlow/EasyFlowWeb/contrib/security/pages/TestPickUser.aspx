<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_security_pages_TestPickUser" Codebehind="TestPickUser.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">

<script type="text/javascript">

    function openUserSelector(inputId){
        var h = 450;
	    var w = 700;
	    var iTop = (window.screen.availHeight-30-h)/2;    
	    var iLeft = (window.screen.availWidth-10-w)/2; 
	    window.open('popupUserPicker.aspx?inputid='+ inputId, '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');	
	    return false;
    }
    function openUserSelector2(){
        var h = 450;
	    var w = 700;
	    var iTop = (window.screen.availHeight-30-h)/2;    
	    var iLeft = (window.screen.availWidth-10-w)/2; 
	    window.open('popupUserPicker2.aspx?func=onCompleteUserPick', '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');	
	    return false;
    }
    function onCompleteUserPick(result){
        var values = "";
        for(var i=0; i<result.length;i++){
            values += (result[i].key + "<\"" + result[i].value + "\">,");
        }
        if(values.length > 0)
            values = values.substring(0, values.length - 1);
        $("#<%=txtUsers2.ClientID%>").val(values);
    }
</script>
<fieldset style="padding:8px">
    <legend>选择用户示例 - 传入文本框 ID</legend>
    <asp:TextBox ID="txtUsers" TextMode="MultiLine" Height="50px" Width="60%" runat="server">
        
    </asp:TextBox>
    <a href="javascript:;" onclick="javascrpt:return openUserSelector('<%=txtUsers.ClientID%>');">
        选择用户
    </a>
</fieldset>

<fieldset style="padding:8px">
    <legend>选择用户示例 - 传入 JS 函数(Function)名称</legend>
    <asp:TextBox ID="txtUsers2" TextMode="MultiLine" Height="50px" Width="60%" runat="server">
        
    </asp:TextBox>
    <a href="javascript:;" onclick="javascrpt:return openUserSelector2();">
        选择用户
    </a>
</fieldset>
</asp:Content>
