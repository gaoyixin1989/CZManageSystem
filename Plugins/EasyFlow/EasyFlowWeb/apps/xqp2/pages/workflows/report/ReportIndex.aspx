<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_report_ReportIndex" Title="报表统计" Codebehind="ReportIndex.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="ActivityStat" Src="../controls/ActivityStat.ascx" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.XQP" Namespace="Botwave.XQP.Web.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <link href="<%=AppPath%>res/js/jquery.editable-select.css"
        rel="stylesheet" type="text/css" />
    <script src="<%=AppPath%>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.js"
        type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.pack.js"
        type="text/javascript"></script>
    <div class="titleContent">
        <h3><span>报表统计</span></h3>
    </div>
    <div class="btnControl">
        <div class="btnLeft">
            <input type="button" class="btnFW" value="工单流转明细" onclick="window.location='detailReport.aspx';" />&nbsp;
            <input type="button" class="btnFW" value="流程业务报表" onclick="window.location='businessReport.aspx';" />&nbsp;
            <input type="button" class="btnFW" value="审批意见报表" onclick="window.location='commentReport.aspx';" />&nbsp;
            <bw:AccessController ID="AccessController1" runat="server" ResourceValue="E004">
                <ContentTemplate>
                   <input type="button" class="btnFW" value="智能提醒统计" onclick="window.location='ReportTimeRataState.aspx';" /> &nbsp;
                </ContentTemplate>
            </bw:AccessController>
            <bw:AccessController ID="acc1" runat="server" ResourceValue="A007">
                <ContentTemplate>
                   <input type="button" class="btnFW" value="自定义报表" onclick="window.location='<%=AppPath%>contrib/report/pages/reportList.aspx';" /> 
                </ContentTemplate>
            </bw:AccessController>
        </div>
    </div>
    <div class="dataList">
        <bw:ActivityStat ID="activityStat1" runat="server" />
    </div>
</asp:Content>
