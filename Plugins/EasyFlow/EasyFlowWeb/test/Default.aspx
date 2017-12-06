<%@ Page Language="C#" AutoEventWireup="true" Inherits="test_Default" Codebehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body style="background: none">
    <form id="form1" runat="server">
    <div >
        <div >
        <input onblur
            <table cellspacing="0" cellpadding="0" width="135" align="center">
                <tbody>
                    <tr>
                        <td height="20">
                            <a href="<%=AppPath%>ssoprox/index/aj.ashx" target="mainFrame">流程主页</a>
                        </td>
                    </tr>
                    <tr>
                        <td height="20">
                            <a href="<%=AppPath%>ssoprox/start/aa.ashx"
                                target="mainFrame">发起工单</a>
                        </td>
                    </tr>
                    <tr>
                        <td height="20">
                            <a href="<%=AppPath%>ssoprox/todo/aj.ashx" target="mainFrame">查看工单</a>
                        </td>
                    </tr>
                </tbody>
        </div>
    </div>
    </form>
</body>
</html>
