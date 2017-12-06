<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="true" Inherits="apps_czmcc_pages_SQLAnalyzer" Codebehind="SQLAnalyzer.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TOOL-SQL Analyzer</title>
</head>
<body style="background:none !important; text-align:left; padding:10px;">
    <form id="form1" runat="server">
    <div>
        <fieldset style="padding:5px;">
            <legend>SQL Command</legend>
            <div>SQL 命令语句：</div>
            <asp:TextBox ID="txtCommand" runat="server" Width="90%" Height="120" TextMode="MultiLine"></asp:TextBox>
            
            <div style="margin:5px;">
                命令类型：
                <asp:RadioButtonList ID="radioTypes" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Value="0" Selected="True">查询</asp:ListItem>
                    <asp:ListItem Value="1">更新</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            
            <div style="margin:5px;">
                <asp:Button ID="buttonExecute" runat="server" Text="Execute" CssClass="btn2" onclick="buttonExecute_Click" />
            </div>
            <div style="margin:5px;">
                <asp:Label ID="labelError" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
    <div>
        <fieldset style="padding:5px; margin-top:10px;">
            <legend>结果</legend>
            <asp:Label ID="labelResult" runat="server"></asp:Label>
            <div id="holder" style="width:100%; overflow:auto; margin-top:10px;">
                <asp:PlaceHolder ID="holderResult" runat="server">
                
                </asp:PlaceHolder>
            </div>
        </fieldset>
    </div>
    </form>
</body>
</html>
