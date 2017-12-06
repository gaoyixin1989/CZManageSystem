<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_report_pages_bwtable" Codebehind="bwtable.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>列表</title>
    <link href="../res/css/Report.css" type="text/css" rel="Stylesheet" rev="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="tblClass">
        <input type="button" id="btnExpor" onclick="__doPostBack('EXPORTEXCEL','')"
            value="导出" class="btn_sav" style="display: none" />
        <asp:Label ID="lblName" runat="server" Font-Bold="true" Font-Size="Large" BackColor="White" />
        <asp:Table ID="tbList" runat="server" Width="100%" CellSpacing="0" border="1" CssClass="tbl_report_config"
            rules="all" Style="width: 100%; border-collapse: collapse;">
        </asp:Table>
        <table width="100%">
            <tr>
                <td align="right">
                    <bw:VirtualPager ID="listPager" runat="server" OnPageIndexChanged="listPager_PageIndexChanged" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
