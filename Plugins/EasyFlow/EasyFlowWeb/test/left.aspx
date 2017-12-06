<%@ Page Language="C#" AutoEventWireup="true" Inherits="test_left" Codebehind="left.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container2">
        <div class="mainbox">
            <!--主体区域 / MainContent-->
            <div class="mainContent">
                <!--左侧侧边栏 / Sidebar -->
                <div class="sidebar" style="width: 190px; height: 900px">
                    <table cellspacing="0" cellpadding="0" width="135" align="center">
                        <tbody>
                            <tr>
                                <td height="20">
                                    <a href="<%=AppPath%>contrib/workflow/pages/default.aspx" target="mainFrame">流程主页</a>
                                </td>
                            </tr>
                            <tr>
                                <td height="20">
                                    <a href="<%=AppPath%>contrib/workflow/pages/start.aspx?wid=0766648e-08ad-452b-be7e-22e5b45d0947" target="mainFrame">发起工单</a>
                                </td>
                            </tr>
                            <tr>
                                <td height="20">
                                    <a href="<%=AppPath%>contrib/workflow/pages/default.aspx" target="mainFrame">待办列表</a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!-- Sidebar End-->
                <div class="clearDiv">
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
