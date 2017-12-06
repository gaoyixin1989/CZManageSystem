<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_config_AdvancedConfig" Codebehind="AdvancedConfig.aspx.cs" %>

<%@ Register Src="UC/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal ID="litFName" runat="server"></asp:Literal></title>
</head>
<body style="background:url(<%=AppPath%>App_Themes/new/images/expand_left_bg.gif) repeat-y left;">
<style>
body { font-family:Arial, simsun; font-size:12px; margin:0; padding:0; color:#333; overflow-y:scroll; *overflow-y:inherit; }
input,textarea,select,button { color:#000; margin:0; vertical-align:middle; }
form , ul , ol , li { margin:0; padding:0; list-style:none; }
img { border:0; }
h1,h2,h3,h4,h5 { margin:0; padding:0; font-size:12px; }
p { margin:0 0 8px; }
a:visited , a:link { text-decoration:underline; color:#333; }
a:hover { color:#F50 !important; text-decoration:underline; }
.clear { clear:both; height:1px; font-size:0; overflow:hidden;  }

.expand{width:790px;}
.expand .left_e{float:left;width:200px;}
.expand .left_e ul{}
.expand .left_e a{display:block;height:39px;}
.expand .left_e a:link,.expand .left_e a:visited{color:#336699!important; text-decoration:none!important;}
.expand .left_e a:hover{color:#ff6600!important;}
.expand .left_e li{background:url(<%=AppPath%>App_Themes/new/images/expand_left_li.gif) no-repeat right -40px;height:39px;line-height:39px;padding-left:30px;}
.expand .left_e .c{background:url(<%=AppPath%>App_Themes/new/images/expand_left_li.gif) no-repeat top right!important;}
.expand .right_e{float:left;width:550px;margin-left:20px;}
.expand .right_e dl{margin:0;padding-bottom:20px;}
.expand .right_e dt{margin:0;font-weight:bold;color:#247ecf;padding-bottom:10px;_padding-bottom:6px;}
.expand .right_e dd{margin:0;padding-left:16px; line-height:18px;}
</style>

    <form id="form1" runat="server">
    <div class="expand">
<div class="left_e">
<uc1:Menu ID="Menu1" runat="server" />
</div>
<div class="right_e">
<iframe id="IframeEF" src="UC/GetOuterData.aspx?fdid=<%=FormDefinitionId %>&wfid=<%=WorkflowId %>&EntityType=<%=EntityType%>&fid=<%=FormItemDefinitionId %>" frameborder="0" width="100%" scrolling-y="auto" onload="this.height=$(this).contents().find('body').height()"></iframe>
    </div>
    <div class="clear"></div>
</div>
    
    </form>
</body>
</html>
