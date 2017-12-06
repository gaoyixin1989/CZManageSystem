<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_WorkflowView" Codebehind="WorkflowView.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="CompletWorkFlowList" Src="../controls/ProcessHistoryLoader.ascx"%>
<%@ Register TagPrefix="bw" TagName="WorkflowView" Src="../controls/WorkflowView.ascx" %>
<%@ Register TagPrefix="bw" TagName="Attachment" Src="../controls/Attachments.ascx" %>
<%@ Register TagPrefix="bw" TagName="ReviewHistory" Src="../controls/ReviewHistory.ascx" %>
<%@ Register TagPrefix="bw" TagName="WorkflowHistoryRelation" Src="~/plugins/easyflow/apps/czmcc/pages/WorkflowHistoryRelation.ascx" %>
<%@ Register TagPrefix="bw" TagName="WorkflowAttention" Src="~/plugins/easyflow/apps/czmcc/pages/WorkflowAttentionControl.ascx" %>
<%@ Register TagPrefix="bw" TagName="WorkflowRelation" Src="~/plugins/easyflow/apps/czmcc/pages/WorkflowRelation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script type="text/javascript" src="script/dynamic-form.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="WorkflowAjaxService.asmx" />
            <asp:ServiceReference Path="GetDataAjaxService.asmx" />
        </Services>
    </asp:ScriptManager>
    <script type="text/javascript" src="../../../res/js/jquery-latest.pack.js"></script>
    <script type="text/javascript" src="script/tooltipAjax.js"></script>
    <script type="text/javascript" src="script/autofull.js"></script>
    <script type="text/javascript" src="script/ItemsExtensions.js"></script>
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
        <div class="showControl hideDiv">
            <h4>流程图</h4>
            <button onclick="return showContent(this,'dataFlowGraph');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable hideDiv" id="dataFlowGraph" style="display:none;">
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
        <bw:WorkflowRelation ID="WorkflowRelation1" runat="server" />
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
    
    <%if (EnableUpload)
      { %>
      <script src="<%=AppPath %>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="<%=AppPath %>res/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="<%=AppPath %>res/uploadify/jquery.uploadify-3.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_cphBody_ucAttachment_trFile").show();
            $("#ctl00_cphBody_ucAttachment_fileUpload1").uploadify({
                height: 15,
                swf: '<%=AppPath %>res/uploadify/uploadify.swf',
                uploader: '<%=AppPath %>contrib/workflow/uploadify/UploadAttachment.ashx',
                //checkExisting: '/uploader/uploadify-check-existing.php', //检查上传文件是否存，触发的url，返回1/0
                width: 120,
                cancelImg: '<%=AppPath %>res/uploadify/uploadify-cancel.png',
                buttonText: '添加附件',
                //fileTypeExts: '*.7z;*.bmp;*.doc;*.docx;*.fla;*.flv;*.gif;*.gzip;*.jpeg;*.jpg;*.mid;*.mpeg;*.mpg;*.pdf;*.png;*.ppt;*.pptx;*.pxd;*.ram;*.rar;*.rtf;*.swf;*.tgz;*.tif;*.tiff;*.txt;*.vsd;*.xls;*.xlsx;*.xml;*.zip"',
                //fileSizeLimit: '20MB',
                uploadLimit: 5,
                removeCompleted: true,
                removeTimeout: 1,
                auto: true,
                multi: true,
                method: 'post',
                formData: { 'wiid': "<%=this.WorkflowInstanceId %>" },
                onDialogClose: function (swfuploadifyQueue) {
                },
                onDialogOpen: function () {//当选择文件对话框打开时触发
                    //alert('Open!');
                },
                onSelect: function (file) {
                },
                onSelectError: function (file, errorCode, errorMsg) {
                },
                onQueueComplete: function (stats) {
                    window.location = "<%=Request.RawUrl %>";
                },
                onUploadSuccess: function (file, data, response) {//上传完成时触发（每个文件触发一次）
                    if (data.indexOf('错误提示') > -1) {
                        alert(data);
                    }
                    else {
                    }
                },
                onUploadError: function (file, errorCode, errorMsg, errorString) {//当单个文件上传出错时触发
                    alert('文件：' + file.name + ' 上传失败: ' + errorString);
                }
            });
        });
    </script>
    <%} %>
</asp:Content>
