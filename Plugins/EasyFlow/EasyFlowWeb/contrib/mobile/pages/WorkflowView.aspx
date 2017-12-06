<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Mobile.master" AutoEventWireup="true" Inherits="contrib_mobile_pages_WorkflowView" Codebehind="WorkflowView.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="CompletWorkFlowList" Src="../../../contrib/workflow/controls/ProcessHistoryLoader.ascx"%>
<%@ Register TagPrefix="bw" TagName="WorkflowView" Src="../controls/WorkflowView.ascx" %>
<%@ Register TagPrefix="bw" TagName="Attachment" Src="../../../contrib/workflow/controls/Attachments.ascx" %>
<%@ Register TagPrefix="bw" TagName="ReviewHistory" Src="../../../contrib/workflow/controls/ReviewHistory.ascx" %>
<%@ Register TagPrefix="bw" TagName="WorkflowRelation" Src="../../../apps/czmcc/pages/WorkflowRelation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
        <Services>
            <%--<asp:ServiceReference Path="../../workflow/pages/WorkflowAjaxService.asmx" />--%>
            <asp:ServiceReference Path="../../workflow/pages/GetDataAjaxService.asmx" />
        </Services>
    </asp:ScriptManager>
    <script type="text/javascript" src="../../workflow/pages/script/dynamic-form.js"></script>
    <%--<script type="text/javascript" src="../../workflow/pages/script/tooltipAjax.js"></script>--%>
    <script type="text/javascript" src="<%=AppPath %>res/js/common.js"></script>
    <script type="text/javascript" src="<%=AppPath %>contrib/workflow/pages/script/autofull.js"></script>
    <script type="text/javascript" src="<%=AppPath %>contrib/workflow/pages/script/ItemsExtensions.js"></script>
    <div class="page-header">
        <!-- Fixed navbar -->
        <div class="navbar navbar-inverse navbar-fixed-top ui-header" role="navigation">
            <h1 class="text-center ui-title" style="color: rgb(69, 125, 179);" id="header">
                工单查看
            </h1>
            <div class="pull-right ui-btn-right">
                <a id="drop3" href="#" class="dropdown-toggle btn btn-link btn-link-icon-more"></a>
                <ul class="dropdown-menu" style="top: 40px;" role="menu" id="ulmenu">
                    <li><a  href="#" onclick="$('.loading-backdrop').show();$('.loading-backdrop').showLoading();window.location='default.aspx'">工作台</a></li>
                    <li class="divider"></li>
                    <li><a onclick="$('.loading-backdrop').show();$('.loading-backdrop').showLoading();window.location='<%=Request.QueryString["returnurl"] %>'">返回</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div class=" container-fluid theme-showcase dataList" role="main">
        <div class="showControl">
            <h4>流程图</h4>
            <button onclick="return onShowWorkflowMap(this,'dataFlowGraph');" title="收缩"><span></span></button>
        </div>
        <div class="dataTable" id="dataFlowGraph" style="display: none; overflow: auto">
            <a id="hrefWorkflowMap" href="#" target="_blank">
                <img id="imgWorkflowMap" alt="流程图" src="" title="查看原图" /></a>
        </div>
        <div class="showControl">
            <h4>基本信息</h4>
            <button onclick="return showContent(this,'dataTable1');" title="收缩">
                <span></span>
            </button>
        </div>
        <div class="dataTable form" id="dataTable1">
            <bw:WorkflowView ID="ucWorkItemView" runat="server" IsReadonly="True" />
        </div>
        <div class="showControl">
            <h4>详细信息</h4>
            <button onclick="return showContent(this,'<%=divDynamicFormContainer.ClientID %>');" title="收缩">
                <span></span>
            </button>
        </div>
            <div id="divDynamicFormContainer" class="dataTable form" runat="server"></div>
         <bw:WorkflowRelation ID="WorkflowRelation1" runat="server" />
        <div id="divAttachments" runat="server" style="clear:both;width:100%">
            <div class="showControl">
                <h4>附件信息</h4>
                <button onclick="return showContent(this,'dataContent4');" title="收缩">
                    <span></span></button>
            </div>
            <div class="dataTable  table-responsive" id="dataContent4">
                <bw:Attachment ID="ucAttachment" runat="server" EnableUpload="false" />
            </div>
        </div>
        <div class="showControl" style="clear:both">
            <h4>处理信息</h4>
            <button onclick="return showContent(this,'dataContent3');" title="收缩">
                <span></span></button>
        </div>
        <div class="dataTable table-responsive" id="dataContent3">        
                <bw:CompletWorkFlowList ID="ucCompletWorkFlowList" runat="server" />
        </div>
        <bw:ReviewHistory ID="reviewHistoryList" runat="server" />
        <p align="center" style="margin-top:6px;">
            <asp:Button ID="btnReview" runat="server" CssClass="btn btn-info" Text="已阅" CommandName="review" OnClientClick="$('.loading-backdrop').show();$('.loading-backdrop').showLoading();"  onclick="btnReview_Click" Visible="false" />
            <asp:Button ID="btnRetract" runat="server" CssClass="btn btn-default"
                CommandName="retract" Text="撤回" OnClientClick="return confirm('确定要撤回该处理任务吗?');" onclick="btnRetract_Click" />
            &nbsp;
            <input type="button" class="btn btn-default" value="返回" onclick="window.history.go(-1);" />
        </p>
    </div>
    <script language="javascript" type="text/javascript">
    <!--        //
        $(document).ready(function () {
            $(".tblGrayClass").addClass("table");
            $("#drop3").click(function () {
                $("#ulmenu").show();
            });
            $(".dataList").click(function () {
                $("#ulmenu").hide();
            });
            $("#btnShow").addClass("btn");
            $(".showControl").children("h4").click(function () {
                $(this).next("button").trigger("click");
            });
            $("#relationContent,#dataReviewHistoryList").attr("class", "dataTable table-responsive");
            $('.loading-backdrop').hideLoading();
            $('.loading-backdrop').hide();
        });
        function onShowWorkflowMap(obj, hiddenLayout) {
            var url = "<%=this.ImageUrl%>";
            $("#imgWorkflowMap").attr("src", url + "&width=700");
            $("#hrefWorkflowMap").attr("href", url);
            $("#imgWorkflowMap").ready(function () {
                showContent(obj, hiddenLayout);
            });
            return false;
        }
    function setActivityCommentCount(count){
        $("#spanCommentCount").html(count);
    }
    function displayComment(isVisible){
        var style = isVisible ? "block" : "none";
        $("#divCommentList1").css("display", style);   
    }    
    function setCommentFrameHeight(iheight){
        var h = document.getElementById("comment_ifAttas").height;
        h = Math.abs(h) + Math.abs(iheight);
        $("#comment_ifAttas").attr("height", h);
    }
    //-->
    </script>
</asp:Content>
