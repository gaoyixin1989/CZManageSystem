<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Mobile.master" AutoEventWireup="true" Inherits="contrib_mobile_pages_Start" Codebehind="Start.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="WorkflowInput" Src="../controls/WorkflowWorkitem.ascx" %>
<%@ Register TagPrefix="bw" TagName="ActivitySelector" Src="../../../contrib/workflow/controls/ActivitySelector.ascx" %>
<%@ Register TagPrefix="bw" TagName="Attachment" Src="../../../contrib/workflow/controls/Attachments.ascx" %>
<%@ Register TagPrefix="bw" TagName="ProcessHistory" Src="../../../contrib/workflow/controls/ProcessHistory.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <script type="text/javascript">
        $("body").showLoading()
    </script>
    <div id="pageMaskLayout"></div>
    <div id="pageMaskContent">
        <span>正在处理，请稍候...</span>
    </div>
    <asp:ScriptManager ID="scriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="../../workflow/pages/WorkflowAjaxService.asmx" />
        </Services>
    </asp:ScriptManager>
    <script type="text/javascript" src="../../workflow/pages/script/dynamic-form.js"></script>
    <%--<script type="text/javascript" src="../../workflow/pages/script/tooltipAjax.js"></script>--%>
    <script type="text/javascript" src="<%=AppPath %>res/js/common.js"></script>
    
   <div class="page-header">
        <!-- Fixed navbar -->
        <div class="navbar navbar-inverse navbar-fixed-top ui-header" role="navigation">
            <h1 class="text-center ui-title" style="color: rgb(69, 125, 179);" id="header">
                发起工单
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
            <h4>摘要内容</h4>
            <button onclick="return showContent(this,'dataTable1');" title="收缩">
                <span></span></button>
        </div>
        <div class="dataTable" id="dataTable1">
            <bw:WorkflowInput ID="myWorkItemInput" runat="server" />     
        </div>
        <div class="showControl">
            <h4>
                详细信息</h4>
            <button onclick="return showContent(this,'<%=divDynamicFormContainer.ClientID %>');" title="收缩"><span></span></button>
        </div>
        <div id="divDynamicFormContainer" class="dataTable form" runat="server"></div>
        <div class="showControl">
            <h4>
                附件信息</h4>
            <button onclick="return showContent(this,'divAttachmentContainer');" title="收缩"><span></span></button>
        </div>
        <div class="dataTable  table-responsive" id="divAttachmentContainer">
            <bw:Attachment ID="ucAttachment" runat="server" EnableUpload="true" />
        </div>
        <div id="divWorkflowHistory" runat="server" visible="false">
            <div class="showControl">
                <h4>处理信息</h4>
                <button onclick="return showContent(this,'dataHistory');" title="收缩"><span></span></button>
            </div>
            <div class="dataTable table-responsive" id="dataHistory">
                <bw:ProcessHistory ID="historyList" runat="server" />
            </div>
        </div>
        <bw:ActivitySelector ID="myActivtySelector" runat="server" />
        
        <p align="center" style="margin-top: 2rem;">
            <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-default" OnClientClick="return beforeCreate();" Text="提交" style="color:Green" OnClick="btnCreate_Click" />
            &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="btn btn-default" Text="保存" OnClientClick="return beforeSave();"  onclick="btnSave_Click" />
            &nbsp;&nbsp;<asp:Button ID="btnReturn" runat="server" CssClass="btn btn-default" Text="返回" OnClick="btnReturn_Click" />
            <asp:Button ID="btnChoose" runat="server" CssClass="btn btn-default"
                    Style="display: none" Text="打开从全公司选择窗口" OnClick="btnChoose_Click" />
        </p>
    </div>
    <!-- Modal -->
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
    <script language="javascript" type="text/javascript">
//        $(function() {
//        var FormDataSet =getFormDataSet("<%=WorkflowName %>", "<%=WorkflowAttachmentId %>",$("#ctl00_cphBody_myWorkItemInput_txtTitle").val(),"","<%=CurrentUserName %>","","","");
//        getDataSet("<%=WorkflowId %>", FormDataSet);
        //        });
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
            $("#dataActivitySelector").children("div").css("margin-left", ".8em");
            
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
    function beforeCreate(){
        if(onValidateReviewActors()==false)
            return false;
        var isValid = Validator.validate();
        if (isValid){
            var shouldCheckSelection = "<%=myActivtySelector.Visible %>";
            if (shouldCheckSelection.toLowerCase() == "true"){
                isValid = checkSelection();
            }
            if(isValid){
                showMaskLayout();
            }
        }        
        return isValid;
    }
            
    function beforeSave(){
//        if(Validator.validate()){ }
        showMaskLayout();
        return true;
    }

    //var FormDataSet = getFormDataSet("<%=WorkflowName %>", "<%=WorkflowAttachmentId %>", $("#ctl00_cphBody_myWorkItemInput_txtTitle").val(), "", "<%=CurrentUserName %>", "<%=DpId %>", "", "");
    //getDataSet("<%=WorkflowId %>", "<%=WorkflowAttachmentId %>", FormDataSet, 0, "<%=divDynamicFormContainer.ClientID %>");
    //ItemIframe.LoadIframe("<%=WorkflowId %>", "<%=WorkflowAttachmentId %>", "<%=ActivityName %>", FormDataSet);
    //ItemDataList.LoadDataList("<%=WorkflowId %>", "<%=WorkflowAttachmentId %>", FormDataSet, 0);
    //绑定下拉、单选、多选
    //setTimeout(function() {
        for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {
            //debugger;
            bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
        }
    //}, 1000);
    // div 覆盖层代码.
    function showMaskLayout() {
        $("#pageMaskLayout").css("display", "block");
        $("#pageMaskContent").css("display", "block");
        var bodyHeight = $("body").height();
        var bodyWidth = $("body").width();
        $("#pageMaskContent").css("top", bodyHeight - 200);
        $("#pageMaskContent").css("left", bodyWidth/2 - 100);
    }
    function closeMaskLayout() {
        $("#pageMaskLayout").css("display", "none");
        $("#pageMaskContent").css("display", "none");
    }
    </script>
    <asp:Literal ID="ltlScripts" runat="server"></asp:Literal>
</asp:Content>
