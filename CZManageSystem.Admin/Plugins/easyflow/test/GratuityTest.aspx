<%@ Page Language="C#" AutoEventWireup="true" Inherits="test_GratuityTest" Codebehind="GratuityTest.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>潮州销售精英平台接口测试</title>
</head>
<body style="text-align:left; padding:8px;">
    <form id="form1" runat="server">
    <div>
        <div style="background-color:Yellow; padding: 10px 0 10px 0;">
        Service URL:<asp:TextBox ID="txtURL" runat="server" Width="80%"></asp:TextBox>
        </div>
        <fieldset>
            <legend><h3>发单</h3></legend>
            <div>
                发起人<asp:TextBox ID="txtCreator" runat="server" Width="208px"></asp:TextBox>
                <br />
                处理人<asp:TextBox ID="txtActor" runat="server" Width="208px"></asp:TextBox>
                <br />
                标题<asp:TextBox ID="txtTitle" runat="server" Width="208px"></asp:TextBox>
                <br />
                内容<asp:TextBox ID="txtcontent" TextMode="MultiLine" runat="server" Height="176px" 
                    Width="636px"></asp:TextBox>
                <br />
                <asp:Button ID="btnTest" runat="server" Text="Send" onclick="btnTest_Click" />
            </div>
        </fieldset>
    </div>
    <div style="margin-top:8px">
        <fieldset>
            <legend><h3>按工号查询</h3></legend>
            <div>
                <asp:TextBox ID="txtEmployeeID" runat="server" Width="300px"></asp:TextBox>
                <asp:Button ID="btn_Query1" runat="server" Text="查询" 
                    onclick="btn_Query1_Click" />
                <asp:Panel ID="result1" runat="server"></asp:Panel>
        </div>
    </div>
    
    <div style="margin-top:8px">
        <fieldset>
            <legend><h3>按ApplyID查询</h3></legend>
            <div>
                <asp:TextBox ID="txtApplyID" runat="server" Width="300px"></asp:TextBox>
                <asp:Button ID="btn_Query2" runat="server" Text="查询" 
                    onclick="btn_Query2_Click" />
                <div>
                <asp:Panel ID="result2" runat="server"></asp:Panel>
                </div>
        </div>
    </div>
    </form>
</body>
</html>
