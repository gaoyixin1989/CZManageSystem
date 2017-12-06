<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Mobile.master" AutoEventWireup="true" Inherits="contrib_mobile_pages_Process" Codebehind="Process.aspx.cs" %>

<%@ Register TagPrefix="bw" TagName="WorkflowInput" Src="../controls/WorkflowWorkItem.ascx" %>
<%@ Register TagPrefix="bw" TagName="ProcessHistory" Src="../../../contrib/workflow/controls/ProcessHistoryLoader.ascx" %>
<%@ Register TagPrefix="bw" TagName="ActivitySelector" Src="../../../contrib/workflow/controls/ActivitySelector.ascx" %>
<%@ Register TagPrefix="bw" TagName="ReviewHistory" Src="../../../contrib/workflow/controls/ReviewHistory.ascx" %>
<%@ Register TagPrefix="bw" TagName="Attachment" Src="../../../contrib/workflow/controls/Attachments.ascx" %>
<%@ Register TagPrefix="bw" TagName="WorkflowRelation" Src="../../../apps/czmcc/pages/WorkflowRelation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="pageMaskLayout">
    </div>
    <div id="pageMaskContent">
        <span>正在处理，请稍候...</span>
    </div>
    <asp:ScriptManager ID="scriptManager1" runat="server">
        <Services>
            <%--<asp:ServiceReference Path="../../workflow/pages/WorkflowAjaxService.asmx" />--%>
             <asp:ServiceReference Path="../../workflow/pages/GetDataAjaxService.asmx" />
        </Services>
    </asp:ScriptManager>
    <script type="text/javascript" src="../../workflow/pages/script/dynamic-form.js"></script>
    <%--<script type="text/javascript" src="../../workflow/pages/script/tooltipAjax.js"></script>--%>
    <script type="text/javascript" src="<%=AppPath %>res/js/common.js"></script>
    <script type="text/javascript" src="../js/popupforward.js"></script>
     <script type="text/javascript" src="<%=AppPath %>contrib/workflow/pages/script/autofull.js"></script>
     <script type="text/javascript" src="<%=AppPath %>contrib/workflow/pages/script/workflowextension.js"></script>
     <script type="text/javascript" src="<%=AppPath %>contrib/workflow/pages/script/ItemsExtensions.js"></script>
    <div class="page-header">
        <!-- Fixed navbar -->
        <div class="navbar navbar-inverse navbar-fixed-top ui-header" role="navigation">
            <h1 class="text-center ui-title" style="color: rgb(69, 125, 179);" id="header">
                工单处理
            </h1>
            <div class="pull-right ui-btn-right">
                <a id="drop3" href="#" class="btn btn-link btn-link-icon-more"  role="button"></a>
                <ul class="dropdown-menu" style="top: 40px;" role="menu" id="ulmenu">
                    <li><a href="#" onclick="$('.loading-backdrop').show();$('.loading-backdrop').showLoading();window.location='default.aspx'">工作台</a></li>
                    <li class="divider"></li>
                    <li><a onclick="$('.loading-backdrop').show();$('.loading-backdrop').showLoading();window.location='<%=Request.QueryString["returnurl"] %>'">返回</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div class=" container-fluid theme-showcase dataList" role="main">
        <div class="showControl">
            <h4>
                流程图</h4>
            <button id="btnExpandMap" runat="server" causesvalidation="false" onclick="return onShowWorkflowMap(this,'dataFlowGraph');"
                title="收缩">
                <span></span>
            </button>
        </div>
        <div class="dataTable" id="dataFlowGraph" style="display: none; overflow: auto">
            <a id="hrefWorkflowMap" href="#" target="_blank">
                <img id="imgWorkflowMap" alt="流程图" src="" title="查看原图" /></a>
        </div>
        <div class="showControl">
            <h4>
                摘要内容</h4>
            <button onclick="return showContent(this,'dataTable1');" title="收缩">
                <span></span>
            </button>
        </div>
        <div class="dataTable form" id="dataTable1">
            <bw:WorkflowInput ID="wfInput" runat="server" />
        </div>
        <div class="showControl">
            <h4>
                详细信息</h4>
            <button onclick="return showContent(this,'<%=divDynamicFormContainer.ClientID %>');"
                title="收缩">
                <span></span>
            </button>
        </div>
        <div id="divDynamicFormContainer" class="dataTable form" runat="server">
        </div>
        <div class="dataTable table-responsive" id="Div1">
            <bw:WorkflowRelation ID="WorkflowRelation1" runat="server" />
        </div>
        <div class="showControl">
            <h4>
                附件信息</h4>
            <button onclick="return showContent(this,'divAttachmentContainer');" title="收缩">
                <span></span>
            </button>
        </div>
        <div class="dataTable  table-responsive" id="divAttachmentContainer">
            <bw:Attachment ID="ucAttachment" runat="server" EnableUpload="true" />
        </div>
        <div class="showControl">
            <h4>
                处理信息</h4>
            <button onclick="return showContent(this,'dataContent3');" title="收缩">
                <span></span>
            </button>
        </div>
        <div class="dataTable table-responsive" id="dataContent3">
            <bw:ProcessHistory ID="historyList" runat="server" />
        </div>
        <bw:ReviewHistory ID="reviewHistoryList" runat="server" />
        <div class="showControl">
            <h4>
                处理</h4>
            <button onclick="return showContent(this,'divProcess');" title="收缩">
                <span></span>
            </button>
        </div>
        <div class="dataTable" style="border: 0" id="divProcess">
            <div class="row">
                <asp:Literal ID="ltlOpinion" runat="server" Text="处理意见" />：
            </div>
            <div>
                <asp:Literal ID="ltlRemarksOption" runat="server"></asp:Literal>
            </div>
            <div style="clear: both;">
                <textarea id="txtReason" class="form-control" cols="70" rows="3" runat="server" style="margin-top: 6px;"></textarea>
            </div>
            <div id="assigmentContainer">
            </div>
            <bw:ActivitySelector ID="selectorAllocatee" runat="server" />
            <p align="center" style="margin-top: 1.6em;">
                <asp:Button ID="btnApprove" CssClass="btn btn-default" ForeColor="Green" runat="server"
                    Text="通过" CommandName="approve" OnCommand="Command_Execute" CausesValidation="false"
                    OnClientClick="return onProcessApprove();" />
                &nbsp;<input type="button" id="btnSetReject" class="btn btn-default" style="color:Red" value="退还" title="展开退还的步骤名称"
                    onclick="return onDisplayReject();" runat="server" />
                &nbsp;<input type="button" id="btnAssign" class="btn btn-default" value="转交" data-toggle="modal"
                    data-target="#assignModal" />
                &nbsp;<asp:Button ID="btnSave" CssClass="btn btn-default" runat="server" CausesValidation="false"
                    Text="保存" CommandName="save" OnCommand="Command_Execute" OnClientClick="return onProcessSave();" />
               &nbsp; <asp:Button ID="btnCancel" CssClass="btn btn-default" runat="server" Visible="false"
                    ForeColor="Red" Text="作废" CausesValidation="false" OnClientClick="return onProcessCancel();"
                    CommandName="cancel" OnCommand="Command_Execute" />
                
                <asp:Button ID="btnFw" runat="server" CssClass="btn btn-default" OnClick="btnFw_Click"
                    Style="display: none" Text="确定转交" />
                <asp:Button ID="btnChoose" runat="server" CssClass="btn btn-default"
                    Style="display: none" Text="打开从全公司选择窗口" OnClick="btnChoose_Click" />
            </p>
            <div id="divReject" align="center" style="display: none;">
                <asp:UpdatePanel ID="pnlReject" runat="server">
                    <ContentTemplate>
                        <font style="color: Red; font-weight: bolder;">请选择要退还的步骤名称：</font>
                        <asp:RadioButtonList ID="rblRejectActivities" runat="server" RepeatDirection="Horizontal"
                            RepeatColumns="6" AutoPostBack="true" style="margin:1.6em">
                            <%--OnSelectedIndexChanged="rblRejectActivities_SelectedIndexChanged"--%>
                        </asp:RadioButtonList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="rblRejectActivities" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Button ID="btnReject" CssClass="btn btn-default" runat="server" CausesValidation="false"
                    ForeColor="Red" Text="退还" OnClientClick="return onProcessReject2();" CommandName="reject"
                    OnCommand="Command_Execute" />
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="assignModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" style="top: 60px; font-size:1.25em">
    </div>
    <div class="modal fade" id="companyModel" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" style="top: 60px; font-size:1.25em">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="H1">
                        从全公司选择</h4>
                </div>
                <div class="modal-body">
                    <div class="row" style="border-bottom: solid 1px #DFDFE8; margin-bottom: 10px;">
                        <div class="col-xs-12 col-md-12">
                            <div class="form-inline" style="text-align: right;">
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control input-sm" style="width: 140px; display: inline;
                                        padding-bottom: 10px" id="txtPeople" placeholder="关键字" runat="server"/>&nbsp;
                                    <asp:Button CssClass="btn btn-default" Text="搜索" runat="server" ID="btnSearch" OnClick="btnSearch_Click"/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-6 col-md-6">
                            <div style="width: 100%; height: 256px; overflow: auto; border: solid 1px #ccc">
                                <asp:UpdatePanel ID="updatePanel1" UpdateMode="Conditional" RenderMode="Block" runat="server">
                                    <ContentTemplate>
                                        <asp:TreeView ID="treeDepts" runat="server" ShowLines="True" onselectednodechanged="treeDepts_SelectedNodeChanged">
                                        </asp:TreeView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnChoose" EventName="click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="col-xs-6 col-md-6">
                            <div class="form-group">
                                <div style="width: 100%; height: 256px; overflow: auto; border: solid 1px #ccc; padding-left: 1.5em">
                                    <asp:UpdatePanel ID="updatePanel2" UpdateMode="Conditional" RenderMode="Block" runat="server">
                                    <ContentTemplate>
                                        <asp:Literal ID="ltlMen" runat="server"></asp:Literal>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="treeDepts" EventName="selectednodechanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnChoose" EventName="click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        取消</button>
                    <button type="button" class="btn btn-primary" data-dismiss="modal" onclick="WorkflowExtension.OnSelectUser();">
                        确定</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="../../workflow/pages/script/workflowextension.js"></script>
     <link href="<%=AppPath %>res/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="<%=AppPath %>res/uploadify/jquery.uploadify-3.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_cphBody_ucAttachment_trFile").show();
            $("#ctl00_cphBody_ucAttachment_fileUpload2").uploadify({
                height: 15,
                swf: '<%=AppPath %>res/uploadify/uploadify.swf',
                uploader: '<%=AppPath %>contrib/workflow/uploadify/UploadAttachment.ashx',
                width: 120,
                cancelImg: '<%=AppPath %>res/uploadify/uploadify-cancel.png',
                buttonText: '添加附件',
                uploadLimit: 5,
                removeCompleted: true,
                removeTimeout: 1,
                auto: true,
                multi: true,
                method: 'post',
                formData: { 'wiid': "<%=this.WorkflowInstanceId %>" },
                onDialogClose: function (swfuploadifyQueue) {//当文件选择对话框关闭时触发
                },
                onDialogOpen: function () {//当选择文件对话框打开时触发
                    //alert('Open!');
                },
                onSelect: function (file) {//当每个文件添加至队列后触发
                },
                onSelectError: function (file, errorCode, errorMsg) {//当文件选定发生错误时触发
                },
                onQueueComplete: function (stats) {//当队列中的所有文件全部完成上传时触发
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
    <script language="javascript" type="text/javascript">
        // <!CDATA[
        $(document).ready(function () {
            WorkflowExtension.GenerProcessRules("<%=WorkflowId %>", "<%=WorkflowInstanceId %>", "<%=ActivityName %>", "<%=CurrentUserName %>", FormDataSet)
            $("#btnAssign").click(function () {

                var textval = $("textarea[name$='txtReason']").get(0).value;
                popupforward.PopupForward("<%=this.WorkflowInstanceId %>", "<%=this.ActivityInstanceId %>");
                return false;
            });
            $("select[name='radOption']").click(function () {
                $("textarea[name$='txtReason']").get(0).value = this.value;
            });
            $("#divCommentList1").css("display", "none");

            $(".tblGrayClass").addClass("table");
            $("#dataReviewHistoryList").addClass("table-responsive");
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

            $("#dataActivitySelector").children("div").css("margin-left", ".8em");
            //$("#divActors").css("margin-left", ".8em");
            $("#relationContent").attr("class", "dataTable table-responsive");
           
            $("#relationContent a").each(function () {
                var href = $(this).attr("href");
                href = href.replace("workflow", "mobile");
                $(this).attr("href",href);
            });
            $("#relationContent input").hide();
            $("#divActors a").removeAttr("onclick");
            $("#divActors a").attr("class", "btn btn-link");
            $("#divActors a").click(function () {
                WorkflowExtension.PopupUserPickerOption.Type = "Activity";
                WorkflowExtension.PopupUserPickerOption.Container = $(this);
                $("#companyModel").modal("show");
                if ($("#<%=treeDepts.ClientID %>").children().length == 0)
                    $("#<%=btnChoose.ClientID %>").trigger("click");
            });
            $("#divActors td").attr("nowrap", "nowrap");
            $("#divActors").attr("class", "table-responsive");
            $("#divActors").css("margin-left", "0");
            $("#divActors").css("padding", "1rem");
            $("#divActors").css("border", "0");
            $("#divActors td").css("padding-bottom", "1rem");
            $("#divActors li").css("padding-top", "1rem");
            if ($("#divReadersContainer")) {
                $("#divReadersContainer div").css("margin-left", "-120px");
                $("#txtDisplyReviewActors").removeAttr("style");
                $("#txtDisplyReviewActors").attr("class", "form-control");
                $("#divReadersContainer a").attr("class", "btn btn-default");
                $("#divReadersContainer a[title='选择抄送人']").removeAttr("onclick");
                $("#divReadersContainer a[title='选择抄送人']").click(function () {
                    WorkflowExtension.PopupUserPickerOption.Type = "Review";
                    WorkflowExtension.PopupUserPickerOption.Container = $("#txtDisplyReviewActors");
                    $("#companyModel").modal("show");
                    if ($("#<%=treeDepts.ClientID %>").children().length == 0)
                        $("#<%=btnChoose.ClientID %>").trigger("click");
                });
            }
            $('.loading-backdrop').hideLoading();
            $('.loading-backdrop').hide();
        });

        function setOption(obj) {
            $("textarea[name$='txtReason']").get(0).value = obj.value;
        }

        function onShowWorkflowMap(obj, hiddenLayout) {
            var url = "<%=this.MapImageUrl%>";
            $("#imgWorkflowMap").attr("src", url + "&width=700");
            $("#hrefWorkflowMap").attr("href", url);
            $("#imgWorkflowMap").ready(function () {
                showContent(obj, hiddenLayout);
            });
            return false;
        }
        function onDisplayCommand(sender) {
            var cmd = document.getElementById("divAdvanceButton");
            cmd.style.display = "inline";
            sender.style.display = "none";
            return false;
        }
        function onHiddenCommand() {
            var cmd = document.getElementById("divAdvanceButton");
            cmd.style.display = "none";
            var dbutton = document.getElementById("btnMoreDisplay");
            dbutton.style.display = "inline";
            var assignment = document.getElementById("iframeAssigment");
            if (assignment) {
                assignment.style.display = "none";
            }
            return false;
        }
        function onProcessApprove() {
            if (onValidateReviewActors() == false) {
                return false;
            }
            var isValid = checkReason();
            isValid = isValid ? Validator.validate() : false;
            if (isValid) {
                var shouldCheckSelection = "<%=selectorAllocatee.Visible %>";
                if (shouldCheckSelection.toLowerCase() == "true") {
                    isValid = checkSelection();
                }
                if (isValid) {
                    showMaskLayout();
                }
            }
            return isValid;
        }
        function onProcessCancel() {
            if (confirm('确定要将当前工单作废吗？')) {
                showMaskLayout();
                return true;
            }
            return false;
        }
        function onProcessSave() {
            if (Validator.validate()) {
                showMaskLayout();
                return true;
            }
            return false;
        }

        function onPreviewWorkflow() {
            var surl = "<%=this.MapImageUrl %>";
            location = surl;
            return true;
        }
        function checkReason() {
            var el = $("textarea[name$='txtReason']").get(0);
            var v = $.trim(el.value);
            if (v.length == 0) {
                alert("请填写处理意见");
                el.focus();
                return false;
            }
            if (v == "不满意，原因：") {
                alert("请填写不满意的原因");
                el.focus();
                return false;
            }
            return true;
        }
        function setActivityCommentCount(count) {
            $("#spanCommentCount").html(count);
        }
        function displayComment(isVisible) {
            var style = isVisible ? "block" : "none";
            $("#divCommentList1").css("display", style);
        }
        function setCommentFrameHeight(iheight) {
            var h = document.getElementById("comment_ifAttas").height;
            h = Math.abs(h) + Math.abs(iheight);
            $("#comment_ifAttas").attr("height", h);
        }

        // div 覆盖层代码.
        function showMaskLayout() {
            $("#pageMaskLayout").css("display", "block");
            $("#pageMaskContent").css("display", "block");
            var bodyHeight = $("body").height();
            var bodyWidth = $("body").width();
            $("#pageMaskContent").css("top", bodyHeight - 200);
            $("#pageMaskContent").css("left", bodyWidth / 2 - 100);
        }
        function closeMaskLayout() {
            $("#pageMaskLayout").css("display", "none");
            $("#pageMaskContent").css("display", "none");
        }

        function onDisplayReject() {
            var isDisplay = $("#divReject")[0].style.display;
            if (isDisplay == "none") {
                $("#divReject")[0].style.display = "";
                $("#<%=btnSetReject.ClientID %>")[0].title = "隐藏退还步骤选择";
            }
            else {
                $("#divReject")[0].style.display = "none";
                $("#<%=btnSetReject.ClientID %>")[0].title = "展开退还步骤选择";
            }
        }

        function onProcessReject2() {
            var isValid = false;
            var rejectActivity = GetRadioButtonListSelectValue("<%=rblRejectActivities.ClientID %>");

            if (rejectActivity == null || rejectActivity == "undefined") {
                alert('请选择要退还的步骤');
                return isValid;
            }
            var msg = '确定要退还当前工单到 ' + rejectActivity + ' 吗？'
            if (confirm(msg)) {
                isValid = checkReason();
            }
            if (isValid) {
                showMaskLayout();
            }
            return isValid;
        }

        function GetRadioButtonListSelectValue(radioListId) {
            var radioListObj = document.getElementById(radioListId).getElementsByTagName("input");
            for (var i = 0; i < radioListObj.length; i++) {
                var objRadioId = radioListId + '_' + i;
                var objRadio = document.getElementById(objRadioId);
                if (objRadio.checked) {
                    return $(objRadio).next("label").text();
                }
            }
        }

        // ]]>
    </script>
    <asp:Literal ID="ltlScripts" runat="server"></asp:Literal>
</asp:Content>
