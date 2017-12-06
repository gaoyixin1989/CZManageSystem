<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_report_BusinessReport" Title="业务统计" Codebehind="BusinessReport.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="WorkflowBusinessStat" Src="../controls/WorkflowBusinessStat.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>流程业务统计</span></h3>
    </div>
    <div class="btnControl">
        <div class="btnLeft">
            <asp:Button ID="btnExport" runat="server" ToolTip="导出当前数据Excel" Text="导出当前页数据" CssClass="btnFW" OnClick="btnExport_Click" />
            <asp:Button ID="btnExportAll" runat="server" ToolTip="导出全部数据Excel" Text="导出全部数据" CssClass="btnFW" OnClick="btnExportAll_Click" />
            <input type="button" class="btnFW" value="返回" onclick="window.location='reportindex.aspx';" />
        </div>
    </div>    
    <div class="dataList">
        <bw:WorkflowBusinessStat ID="workflowBusinessStat1" runat="server" />
    </div>
    
    <script language="javascript">
        //弹出对话框选择内容
        function OpenSelectionDialog(wiid,fdid) {
            var arr = new Array();
            var url = "datalistinfodetail.aspx?fdid=" + fdid + "&wiid=" + wiid + "&t=" + Math.random();
            window.showModalDialog(url, arr, "dialogHeight:400px;dialogWidth:600px;status:no;resizable:no;scroll:y;");
            //return true;
            //window.open(url, "x", "height=600px,width=800px,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=n, oresizable=no;");
        }
    </script>
</asp:Content>
