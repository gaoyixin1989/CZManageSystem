<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_report_StatInstanceReport" Codebehind="StatInstanceReport.aspx.cs" %>

<%@ Register Src="../../../apps/pms/pages/Ucs_Serch.ascx" TagName="PMS" TagPrefix="PMS" %>
<%@ Register TagName="Explore" TagPrefix="Explort" Src="../../../apps/pms/pages/PMS_Export.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <script language="javascript" type="text/javascript">
        // <!CDATA[
        function onAddClick() {
            location = "Custom_Rule_Manage.aspx";
        }
        // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <script src="../../../res/js/highcharts-3.0.7.js" type="text/javascript"></script>
    <script>
        // tableName = "UCS_MKPLN_USR_VTL_CMPR_M"; 
        var fieldArrary = new Array();
        $(function () {
            $("#btn_choice").show();
            $("#btn_choice").click(function () {
                showBox();
            })
            $("#ctl00_cphBody_exp_Pms_btn_All_Export_To_Exc").hide();
            <%=javascript %>
        })
        function goLink(url) {
            location = url;
        }
    </script>
    <div class="titleContent">
        <h3>
            <span>
                <%=Title %></span></h3>
    </div>
    <Explort:Explore ID="exp_Pms" runat="server" />
    <div class="dataList">
        <PMS:PMS ID="con_Pms" runat="server" />
    </div>
    <div class="dataList" id="divResults">
        <div class="showControl">
            <h4>
                统计结果</h4>
            <button onclick="return showContent(this,'dataDiv1');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div id="dataDiv1">
            <div class="libox" style="">
                <div class="dataTable" id="dataTable1">
                    <table width="95%" cellpadding="4" cellspacing="1" class="tblGrayClass">
                        <%=fromHtml%>
                    </table>
                    <div class="div_tbfoot">
                    </div>
                </div>
            </div>
            <div class="toolBlock" style="border-top: solid 1px #C0CEDF">
                <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                    Font-Size="9pt" ItemsPerPage="200" PagerStyle="NumericPages" BorderWidth="0px"
                    OnPageIndexChanged="listPager_PageIndexChanged" />
            </div>
        </div>
    </div>
    <asp:Literal ID="ltlScript" runat="server"></asp:Literal>
</asp:Content>
