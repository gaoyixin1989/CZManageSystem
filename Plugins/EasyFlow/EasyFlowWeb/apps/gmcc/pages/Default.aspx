<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_gmcc_Default" Codebehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>流程易（EF）－中国移动广东公司潮州分公司</title>
</head>
<frameset rows="90,*,20" frameborder="no" border="0" framespacing="0">
		<frame src="<%=AppPath%>apps/gmcc/pages/head.aspx" name="topFrame" scrolling="no" noresize="noresize" id="topFrame"/>
		<frameset  cols="200,*" frameborder="no" border="0" framespacing="0">
		    <frame src="<%=AppPath%>apps/gmcc/pages/left.aspx" name="leftFrame" scrolling="no" noresize="noresize" id="leftFrame" />
		    <frame src="<%=AppPath%>contrib/workflow/pages/default.aspx" name="rightFrame" noresize="noresize" id="rightFrame" />
		</frameset>
		<frame src="<%=AppPath%>apps/gmcc/pages/bottom.aspx" scrolling="no" noresize="noresize" />
	</frameset>
	<noframes>
		<body>
		    您的浏览器不支持框架页，请使用支持框架页的浏览器。
		</body>
	</noframes>
</html>
