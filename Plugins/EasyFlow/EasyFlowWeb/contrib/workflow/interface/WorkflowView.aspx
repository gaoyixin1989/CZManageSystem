<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_interface_WorkflowView" Codebehind="WorkflowView.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="CompletWorkFlowList" Src="../controls/ProcessHistoryLoader.ascx"%>
<%@ Register TagPrefix="bw" TagName="WorkflowView" Src="../controls/WorkflowView.ascx" %>
<%@ Register TagPrefix="bw" TagName="Attachment" Src="../controls/Attachments.ascx" %>
<%@ Register TagPrefix="bw" TagName="ReviewHistory" Src="../controls/ReviewHistory.ascx" %>
<%@ Register TagPrefix="bw" TagName="WorkflowHistoryRelation" Src="~/plugins/easyflow/apps/czmcc/pages/WorkflowHistoryRelation.ascx" %>
<%@ Register TagPrefix="bw" TagName="WorkflowAttention" Src="~/plugins/easyflow/apps/czmcc/pages/WorkflowAttentionControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script type="text/javascript" src="../pages/script/dynamic-form.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="../pages/WorkflowAjaxService.asmx" />
            <asp:ServiceReference Path="../pages/GetDataAjaxService.asmx" />
        </Services>
    </asp:ScriptManager>
    <script type="text/javascript" src="../../../res/js/jquery-latest.pack.js"></script>
    <script type="text/javascript" src="../pages/script/tooltipAjax.js"></script>
    <script type="text/javascript" src="../pages/script/autofull.js"></script>
    <script type="text/javascript" src="../pages/script/ItemsExtensions.js"></script>
    <style>
        .titleContent{display:block; border-bottom-width:0px;}
        .titleContent h3{display:none;}
    </style>
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" runat="server" />工单查看</span></h3>
        <div class="rightSite">
        <bw:WorkflowAttention ID="workflowAttention" runat="server" />
        </div>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>流程图</h4>
            <button onclick="return showContent(this,'dataFlowGraph');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataFlowGraph" style="display:none;">
            <a href="<%=this.ImageUrl%>" title="查看原图." target="_blank"><asp:Image ID="workflowImage" runat="server" /></a>
        </div>
        <div class="showControl">
            <h4>基本信息</h4>
        </div>
        <div class="dataTable">
            <bw:WorkflowView ID="ucWorkItemView" runat="server" IsReadonly="True" />
        </div>
        <div class="showControl">
            <h4>详细信息</h4>
            <button onclick="return showContent(this,'dataContent2');" title="收缩">
                <span>折叠</span>
            </button>
        </div>
        <div class="dataTable" id="dataContent2">
            <div id="divDynamicFormContainer" runat="server"></div>
        </div>
        <bw:WorkflowHistoryRelation ID="relationHistory1" runat="server" />
        <div id="divAttachments" runat="server" style="clear:both;width:100%">
            <div class="showControl">
                <h4>附件信息</h4>
                <button onclick="return showContent(this,'dataContent4');" title="收缩">
                    <span>折叠</span></button>
            </div>
            <div class="dataTable" id="dataContent4">
                <bw:Attachment ID="ucAttachment" runat="server" EnableUpload="true" />
            </div>
        </div>
        <div class="showControl" style="clear:both">
            <h4>处理信息</h4>
            <button onclick="return showContent(this,'dataContent3');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataContent3">        
                <bw:CompletWorkFlowList ID="ucCompletWorkFlowList" runat="server" />
        </div>
        <bw:ReviewHistory ID="reviewHistoryList" runat="server" />
        <p align="center" style="margin-top:6px;">
            <asp:Button ID="btnReview" runat="server" CssClass="btnPassClass" Text="已阅" CommandName="review"  onclick="btnReview_Click" Visible="false" />
            <asp:Button ID="btnRetract" runat="server" CssClass="btnFWClass"
                CommandName="retract" Text="撤回" OnClientClick="return confirm('确定要撤回该处理任务吗?');" onclick="btnRetract_Click" />
            &nbsp;&nbsp;
            <%--<asp:Button ID="btnReturn" runat="server" CssClass="btnReturnClass" Text="返回" OnClick="btnReturn_Click" />--%>
            <input type="button" class="btnReturnClass" value="返回" onclick="window.history.go(-1);" />
            <asp:Button ID="btnExport" runat="server" ToolTip="导出工单内容" Text="导出" CssClass="btnFWClass" onclick="btnExport_Click"  />
            <asp:Button CssClass="btnReview" ID="btnPrint" Text="打印"  OnClientClick="onPrint();" runat="server" />
        </p>
        <div class="showControl">
            <h4>评论信息（<span id="spanCommentCount">0</span>）</h4>
            <button class="c" onclick="return showContent(this,'divCommentList1');" title="收缩"><span>折叠</span></button>
        </div>
        <div id="divCommentList1">
            <iframe id="comment_ifAttas" src="<%=this.CommentUrl%>" scrolling="no" frameborder="no" width="100%">
            </iframe>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
    <!--//
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
    function onPrint() {
        window.open("<%=AppPath%>contrib/workflow/pages/print.aspx?wiid=<%=WorkflowInstanceId%>&aiid=<%=ActivityInstanceId%>");
        //window.showModalDialog("<%=AppPath%>contrib/workflow/pages/print.aspx?wiid=<%=WorkflowInstanceId%>&aiid=<%=ActivityInstanceId%>", "", "dialogwidth=600px;dialogheight=500px;status=no;scroll=yes;help=no");
    }
    //-->
    </script>
</asp:Content>
