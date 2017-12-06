<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_extension_Default" Codebehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>工作台</title>
</head>
<frameset rows="120,*,30" cols="*" frameborder="0" framespacing="0">
    <frame src="<%=AppPath%>apps/xqp2/pages/extension/head.aspx" frameborder="0" framespacing="0" noresize="noresize" crolling="no" id="topFrame" name="topFrame" />
    <frameset cols="9,190,*,9" frameborder="0" framespacing="0">
        <frame src="<%=AppPath%>main/VBorder.aspx" frameborder="0" framespacing="0" scrolling="no" id="leftBorder" />
        <frame src="<%=AppPath%>apps/xqp2/pages/extension/left.aspx" frameborder="0" framespacing="0" scrolling="no" id="leftFrame" />
        <frame src="<%=this.ContentUrl%>" frameborder="0" framespacing="0" scrolling="auto" id="rightFrame" name="rightFrame" />
        <frame src="<%=AppPath%>main/VBorder.aspx" frameborder="0" framespacing="0" scrolling="no" id="rightBorder" />
    </frameset>
    <frame src="<%=AppPath%>main/bottom.aspx" frameborder="0" framespacing="0" noresize="noresize" scrolling="no" />
</frameset>
<noframes>
    <body>
        当前浏览器不支持框架，请使用支持框架显示的浏览器。

    </body>
</noframes>
</html>