<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_Process" Codebehind="Process.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="WorkflowInput" Src="../controls/WorkflowWorkItem.ascx" %>
<%@ Register TagPrefix="bw" TagName="ProcessHistory" Src="../controls/ProcessHistoryLoader.ascx" %>
<%@ Register TagPrefix="bw" TagName="ActivitySelector" Src="../controls/ActivitySelector.ascx" %>
<%@ Register TagPrefix="bw" TagName="ReviewHistory" Src="../controls/ReviewHistory.ascx" %>
<%@ Register TagPrefix="bw" TagName="WorkflowHistoryRelation" Src="../../../apps/czmcc/pages/WorkflowHistoryRelation.ascx" %>
<%@ Register TagPrefix="bw" TagName="WorkflowAttention" Src="../../../apps/czmcc/pages/WorkflowAttentionControl.ascx" %>
<%@ Register TagPrefix="bw" TagName="WorkflowRelation" Src="../../../apps/czmcc/pages/WorkflowRelation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    
    <script type="text/javascript" src="script/dynamic-form.js"></script>
    <script type="text/javascript" src="../../../calendar/calendar_all.js"></script>
    <link type="text/css" rel="Stylesheet" href="../../../calendar/skins/aqua/theme.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="pageMaskLayout"></div>
    <div id="pageMaskContent">
        <span>正在处理，请稍候...</span>
    </div>
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
    <script type="text/javascript" src="script/workflowextension.js"></script>
    <script type="text/javascript" src="<%=AppPath %>res/js/jquery.json.min.js"></script>
    <div class="titleContent">
        <h3><span>工单处理</span></h3>
        <div class="rightSite">
        <bw:WorkflowAttention ID="workflowAttention" runat="server" />
        </div>
    </div>
    <div class="btnControl">
        <div class="btnRight">
        <bw:WorkflowAttention ID="workflowAttention1" runat="server" />
        </div>
    </div>
    <div class="dataList">
        <div class="showControl hideDiv">
            <h4>流程图</h4>
            <button id="btnExpandMap" runat="server" causesvalidation="false" onclick="return onShowWorkflowMap(this,'dataFlowGraph');" title="收缩"><span>折叠</span></button>
            
        </div>
        <div class="dataTable hideDiv" id="dataFlowGraph" style="display: none; width:500px">
            <a id="hrefWorkflowMap" href="#" target="_blank"><img id="imgWorkflowMap" alt="流程图" src="" title="查看原图" /></a>
        </div>
        <div class="showControl">
            <h4>摘要内容</h4>
        </div>
        <div class="dataTable" id="dataTable1">
            <bw:WorkflowInput ID="wfInput" runat="server" />
        </div>
        <div class="showControl">
            <h4>详细信息</h4>
            <button onclick="return showContent(this,'dataContent2');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataContent2">
            <div id="divDynamicFormContainer" runat="server"></div>
        </div>
        <bw:WorkflowRelation ID="WorkflowRelation1" runat="server" />
        <bw:WorkflowHistoryRelation ID="relationHistory1" runat="server" />
        <div class="showControl">
            <h4>附件信息</h4>
            <button onclick="return showContent(this,'divAttachmentContainer');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="divAttachmentContainer">
            <iframe id="ifAttachment" src="<%=AppPath%>contrib/workflow/pages/WorkflowAttachment.aspx?wid=<%=WorkflowId %>&&wiid=<%=WorkflowInstanceId %>"
                scrolling="no" frameborder="no" width="100%" onload="this.height=$(this).contents().find('body').height()+20">
            </iframe>
        </div>
        <div class="showControl">
            <h4>处理信息</h4>
            <button onclick="return showContent(this,'dataContent3');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataContent3">
            <bw:ProcessHistory ID="historyList" runat="server" />
        </div>
        <bw:ReviewHistory ID="reviewHistoryList" runat="server" />
        <div class="showControl">
            <h4>处理</h4>
            <button onclick="return showContent(this,'divProcess');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="divProcess">
            <table class="formTable">
                <tr>
                    <th width="102px;" style="vertical-align: baseline;text-align:right;">
                        <asp:Literal ID="ltlOpinion" runat="server" Text="处理意见" />：

                    </th>
                    <td>
                        <div>
                            <%--<select id="radOption" name="radOption">
                                <option value="">— 请选择 —</option>
                                <asp:Literal ID="ltlRemarksOption" runat="server"></asp:Literal>
                            </select>--%>
                            <asp:Literal ID="ltlRemarksOption" runat="server"></asp:Literal>
                        </div>
                        <div style="clear: both;">
                            <textarea id="txtReason" cols="70" rows="3" runat="server" style="margin-top: 6px;"></textarea>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="assigmentContainer"></div>
            <bw:ActivitySelector ID="selectorAllocatee" runat="server" />

            <div class="pageButtonList">
                <asp:Button ID="btnApprove" CssClass="btnPassClass" ForeColor="Green" runat="server" Text="通过" CommandName="approve"
                    OnCommand="Command_Execute" CausesValidation="false" OnClientClick="return onProcessApprove();" />
                <%--<asp:Button ID="btnSetReject" CssClass="btnReturnClass" runat="server" CausesValidation="false" ForeColor="Red" Text="退还" OnClientClick="return onDisplayReject();"
                    title="展开退还步骤选择" />--%><%--CommandName="reject"  OnCommand="Command_Execute"--%>
                    <input type="button" id="btnSetReject" class="btnReturnClass" value="退还" title="展开退还的步骤名称" onclick="return onDisplayReject();" runat="server" />
                <input type="button" id="btnReturn" class="btn" value="返回" onclick="window.location='<%=AppPath%>contrib/workflow/pages/default.aspx';"/>
                <input type="button" id="btnMoreDisplay" class="btn_right" style="display:inline;" onclick="onDisplayCommand(this)"
                    title="更多命令..." />
                <div id="divAdvanceButton" style="display: none">
                    <input type="button" id="btnAssign" class="btnFWClass" value="转交"/>
                    <asp:Button ID="btnSave" CssClass="btnSaveClass" runat="server" CausesValidation="false" Text="保存" CommandName="save"
                        OnCommand="Command_Execute" OnClientClick="return onProcessSave();" />
                    <asp:Button ID="btnCancel" CssClass="btn_del" runat="server" Visible="false" Text="作废" CausesValidation="false" OnClientClick="return onProcessCancel();"
                        CommandName="cancel" OnCommand="Command_Execute" />
                    <input type="button" class="btn_left" onclick="onHiddenCommand()" title="折叠." />
                </div>
                <asp:Button ID="btnExport" runat="server" ToolTip="导出工单内容" Text="导出" CssClass="btnFWClass" onclick="btnExport_Click" />
                <asp:Button CssClass="btnReview" ID="btnPrint" Text="打印"  OnClientClick="onPrint();" runat="server" />
            </div>
            <div id="divReject" align="center" style="display:none;">                
                <asp:UpdatePanel ID="pnlReject" runat="server">
                    <ContentTemplate>
                        <font style="color:Red;font-weight:bolder;">请选择要退还的步骤名称：</font>
                        <asp:RadioButtonList ID="rblRejectActivities" runat="server" RepeatDirection="Horizontal" RepeatColumns="6"  AutoPostBack="true"><%--OnSelectedIndexChanged="rblRejectActivities_SelectedIndexChanged"--%>
                        </asp:RadioButtonList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="rblRejectActivities" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Button ID="btnReject" CssClass="btnReturnClass" runat="server" CausesValidation="false"
                    ForeColor="Red" Text="退还" OnClientClick="return onProcessReject2();" CommandName="reject"
                    OnCommand="Command_Execute" />
            </div>  
        </div>        
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
    // <!CDATA[
//    var radUrgencyValue = $("#radUrgency").val();
//    var radImportanceValue = $("#radImportance").val();
        $(document).ready(function() {
            $("#btnAssign").click(function() {

                var textval = $("textarea[name$='txtReason']").get(0).value;
                openWindow('PopupForward.aspx?wiid=<%=this.WorkflowInstanceId %>&aiid=<%=this.ActivityInstanceId %>&txt=' + textval, 620, 390);
                return false;
            });
            $("select[name='radOption']").click(function() {
                $("textarea[name$='txtReason']").get(0).value = this.value;
            });
            $("#divCommentList1").css("display", "none");

            WorkflowExtension.GenerProcessRules("<%=WorkflowId %>", "<%=WorkflowInstanceId %>", "<%=ActivityName %>", "<%=CurrentUserName %>", FormDataSet) 
        });
        
    function setOption(obj) {
        $("textarea[name$='txtReason']").get(0).value = obj.value;
    }
    
    function onShowWorkflowMap(obj, hiddenLayout){
        var url = "<%=this.MapImageUrl%>";
        $("#imgWorkflowMap").attr("src", url + "&width=700");
        $("#hrefWorkflowMap").attr("href", url);
        $("#imgWorkflowMap").ready(function(){
            showContent(obj, hiddenLayout);
        });
        return false;
    }
    function onDisplayCommand(sender){
        var cmd = document.getElementById("divAdvanceButton");
        cmd.style.display = "inline";
        sender.style.display = "none";
        return false;
    }
    function onHiddenCommand(){
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
    
//    function onProcessReject(){
//        if(confirm('确定要退还当前工单吗？')){
//            if(checkReason()){
//                showMaskLayout();
//                return true;
//            }
//        }
//        return false;
//    }    
    function onProcessApprove(){
        if(onValidateReviewActors()==false){
            return false;
        }
        var isValid = checkReason();		
        isValid = isValid ? Validator.validate() : false;
        if (isValid){
            var shouldCheckSelection = "<%=selectorAllocatee.Visible %>";
            if (shouldCheckSelection.toLowerCase() == "true"){
                isValid = checkSelection();
            }
            if(isValid){
                showMaskLayout();
            }
        }        
        return isValid;
    }
    function onProcessCancel(){
        if(confirm('确定要将当前工单作废吗？')){
            showMaskLayout();
            return true;
        }
        return false;
    }
    function onProcessSave(){
        if(Validator.validate()){
            showMaskLayout();
            return true;
        }
        return false;
    }
    
    function onPreviewWorkflow(){
        var surl = "<%=this.MapImageUrl %>";
        location = surl;
        return true;
    }
    function checkReason(){
        var el = $("textarea[name$='txtReason']").get(0);
        var v = $.trim(el.value);
        if (v.length == 0){
			alert("请填写处理意见");
			el.focus();
			return false;
		}
		if(v == "不满意，原因："){
		    alert("请填写不满意的原因");
			el.focus();
			return false;
		}
		return true;
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
    
    function onPrint() {
        window.open("<%=AppPath%>contrib/workflow/pages/print.aspx?wiid=<%=WorkflowInstanceId%>&aiid=<%=ActivityInstanceId%>");
        //window.showModalDialog("<%=AppPath%>contrib/workflow/pages/print.aspx?wiid=<%=WorkflowInstanceId%>&aiid=<%=ActivityInstanceId%>", "", "dialogwidth=600px;dialogheight=500px;status=no;scroll=yes;help=no");
    }
    // ]]>
    </script>
    <asp:Literal ID="ltlScripts" runat="server"></asp:Literal>
</asp:Content>
