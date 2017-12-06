<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_report_TableView" Codebehind="TableView.ascx.cs" %>

<script type="text/javascript">
    function __doPostBack(eventTarget,eventArgument)
    {
        var theform = document.aspnetForm;
        theform.__EVENTTARGET.value = eventTarget;
        theform.__EVENTARGUMENT.value = eventArgument;
        theform.submit();
    } 
    function printPage() 
    {
        var body = document.body.innerHTML;
        var printArea = document.getElementById("printArea").innerHTML;
        document.body.innerHTML = printArea;
        window.print();
        document.body.innerHTML = body;
    } 
</script>

<asp:Label ID="tbName" runat="server" Font-Bold="true" Font-Size="Large" BackColor="White" />
<asp:Table ID="tbList" runat="server" Width="100%" class="tblClass" CellSpacing="0"
    rules="all" border="1" Style="width: 100%; border-collapse: collapse;">
</asp:Table>
<table width="100%">
    <tr>
        <td align="left">
            <input id="btnPrint" onclick="printPage();" type="button" value="打印" class="btn_query" />
            <asp:Button ID="btnDownLoad" runat="server" Text="导出" Visible="false" CssClass="btn_sav"
                OnClick="btnDownLoad_Click" />
            <input type="button" onclick="window.location.href('ReportList.aspx')" value="返回"
                class="btnReturnClass" />
        </td>
        <td align="right">
            <bw:VirtualPager ID="listPager" runat="server" OnPageIndexChanged="listPager_PageIndexChanged" />
        </td>
    </tr>
</table>
