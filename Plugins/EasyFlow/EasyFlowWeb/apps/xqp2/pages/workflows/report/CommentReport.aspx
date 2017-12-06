<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_report_CommentReport" Title="审批意见报表" Codebehind="CommentReport.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="WorkflowCommentStat" Src="../controls/WorkflowCommentStat.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>审批意见报表</span></h3>
    </div>
    <div class="btnControl">
        <div class="btnLeft">
            <asp:Button ID="btnExport" runat="server" ToolTip="导出Excel" Text="导出Excel" CssClass="btnFW" OnClick="btnExport_Click" />
            <input type="button" class="btnFW" value="返回" onclick="window.location='reportindex.aspx';" />
        </div>
    </div>    
    <div class="dataList">
        <bw:WorkflowCommentStat ID="workflowCommentStat1" runat="server" />
    </div>
</asp:Content>
