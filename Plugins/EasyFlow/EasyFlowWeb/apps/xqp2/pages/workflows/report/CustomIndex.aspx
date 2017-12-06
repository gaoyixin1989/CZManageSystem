<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_report_CustomIndex" Title="自定义报表" Codebehind="CustomIndex.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="CustomReportList" Src="../controls/CustomReportList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>自定义报表</span></h3>
    </div>
    <div class="btnControl">
        <div class="btnLeft">
            <input type="button" class="btnSave" value="新增报表" onclick="window.location='../../../../../contrib/report/pages/reportCreate.aspx';" />&nbsp;
            <input type="button" class="btnSave" value="管理配置" onclick="window.location='../../../../../contrib/report/pages/reportConfig.aspx';" />&nbsp;
            <input type="button" class="btnFW" value="返回" onclick="window.location='reportindex.aspx';" />
        </div>
    </div>
    <div class="dataList">
        <bw:CustomReportList ID="reportList1" runat="server" />
    </div>
</asp:Content>
