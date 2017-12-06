<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_maintenance_Process" Codebehind="Process.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="WorkflowInput" Src="../../../../../contrib/workflow/controls/WorkflowWorkItem.ascx" %>
<%@ Register TagPrefix="bw" TagName="ProcessHistory" Src="../../../../../apps/xqp2/pages/workflows/maintenance/ProcessHistoryEditorLoader.ascx" %>
<%--<%@ Register TagPrefix="bw" TagName="ActivitySelector" Src="../controls/ActivitySelector.ascx" %>
<%@ Register TagPrefix="bw" TagName="ActivityRetrunSelector" Src="../controls/ActivityRetrunSelector.ascx" %>--%>
<%@ Register TagPrefix="bw" TagName="ReviewHistory" Src="../../../../../contrib/workflow/controls/ReviewHistory.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script type="text/javascript" src="../../../../../contrib/workflow/pages/script/dynamic-form.js"></script>
    <script type="text/javascript" src="../../../../../calendar/calendar_all.js"></script>
    <link type="text/css" rel="Stylesheet" href="../../../../../calendar/skins/aqua/theme.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="pageMaskLayout"></div>
    <div id="pageMaskContent">
        <span>正在处理，请稍候...</span>
    </div>
    <asp:ScriptManager ID="scriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="<%=AppPath%>contrib/workflow/pages/WorkflowAjaxService.asmx" />
            <asp:ServiceReference Path="<%=AppPath%>contrib/workflow/pages/GetDataAjaxService.asmx" />
        </Services>
    </asp:ScriptManager>
    <script type="text/javascript" src="../../../../../res/js/jquery-latest.pack.js"></script>
    <script type="text/javascript" src="../../../../../contrib/workflow/pages/script/tooltipAjax.js"></script>
    <script type="text/javascript" src="../../../../../contrib/workflow/pages/script/autofull.js"></script>
    <script type="text/javascript" src="../../../../../contrib/workflow/pages/script/ItemsExtensions.js"></script>
    <div class="titleContent">
        <div style="float:left">
        您当前位置：<a href="<%=AppPath%>apps/xqp2/pages/workflows/maintenance/search.aspx" >工单管理</a> &gt; <span class="cRed">修改工单</span>
        </div>
        <div style="float:right; text-align:right">
        <%--<a href="javascript:void(0)" class="ico_back" onclick="window.close();">关闭</a>--%>
        </div>
    </div>
    
    <div class="dataList">
        <div class="showControl">
            <h4>流程图</h4>
            <button id="btnExpandMap" runat="server" causesvalidation="false" onclick="return onShowWorkflowMap(this,'dataFlowGraph');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataFlowGraph" style="display: none; width:500px">
            <a id="hrefWorkflowMap" href="#" target="_blank"><img id="imgWorkflowMap" alt="流程图" src="" title="查看原图" /></a>
        </div>
        <div class="showControl">
            <h4>摘要内容</h4>
        </div>
        <div class="dataTable" id="dataTable1">
            <bw:WorkflowInput ID="wfInput" runat="server" />
            <input name="UIHR_SSO_uuid" type="hidden" value="<%=_uuid%>" />
            <input name="UIHR_Userid" type="hidden" value="<%=HR_Userid%>" />
            <input name="local_DpId" type="hidden" value="<%=DpId%>" />
        </div>
        <div style="display:none;">
            <asp:Label ID="LblUserName" runat="server"></asp:Label>
            <asp:Label ID="LblPhone" runat="server"></asp:Label>
            <asp:Label ID="LblDepId" runat="server"></asp:Label>
        </div>
        <div style="text-align:right"><%--<input type="button" value="编辑" class="btn_edit" onclick="editBaseInfo()" />
        <input type="button" value="取消" class="btn_del" onclick="window.location.reload()" />--%></div>
        <div class="showControl">
            <h4>详细信息</h4>
            <button id="_Btn_Info_" onclick="return showContent(this,'dataContent2');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataContent2">
            <div id="divDynamicFormContainer" runat="server"></div>
        </div>
        <div style="text-align:right"><input type="button" value="编辑" class="btn_edit" onclick="editForm()" />
        <input type="button" value="取消" class="btn_del" onclick="window.location.reload()" /></div>
        <div class="showControl">
            <h4>附件信息</h4>
            <button id="_Btn_Info_Att" onclick="return showContent(this,'divAttachmentContainer');" style="float:right;" title="收缩">
                <span>折叠</span></button>
                    
        &nbsp;</div>
        <div class="dataTable" id="divAttachmentContainer">
            <iframe id="ifAttachment" src="<%=AppPath%>contrib/workflow/pages/WorkflowAttachment.aspx?wid=<%=WorkflowId %>&&wiid=<%=WorkflowInstanceId %>"
                scrolling="no" frameborder="no" width="100%" onload="this.height=ifAttachment.document.body.scrollHeight+20;" style="height:60px;overflow:hidden;">
            </iframe>
        </div>
        <div id="divRecords" style="display: none">
            <div class="showControl">
                <h4>
                    操作记录</h4>
                <button onclick="showContent(this,'dataContentRecords');" title="收缩">
                    <span>折叠</span></button>
            </div>
            <div class="dataTable" id="dataContentRecords">
                <div>
                    <iframe id="IframeRecords" src="" frameborder="0" width="100%" scrolling="no" onload="this.height=IframeRecords.document.body.scrollHeight;">
                    </iframe>
                </div>
            </div>
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
        <div id="divContent">
            <div class="showControl" style="display:none">
                <h4>
                    处理</h4>
                <button onclick="return showContent(this,'divProcess');" title="收缩">
                    <span>折叠</span></button>
            </div>
            <div class="dataTable" id="divProcess">
                <table class="formTable" style="display:none">
                    <tr>
                        <th id="th_CLYJ" width="102px;" style="vertical-align: baseline; text-align: right;
                            color: #333;">
                            <asp:Literal ID="ltlOpinion" runat="server" Text="处理意见" />：
                        </th>
                        <td style="vertical-align:top;">  
                            &nbsp;</td>
                        <td style="vertical-align:top">
                            <div style="clear: both;">
                                <textarea id="txtReason" cols="70" rows="3" runat="server" style="margin-top: 6px;"></textarea>
                                <input id="CheckBoxEdit" type="checkbox" name="CheckBoxEdit_WYCC" style="display: none;" /><label
                                    id="labelCheckBox" for="CheckBoxEdit" style="display: none;">差错</label>
                            </div>
                        </td>
                    </tr>
                </table>
                <div id="assigmentContainer">
                </div>
                <div class="pageButtonList" style="width: 99%">
                    <div style="float: right; width: 42%; text-align: right;">
                        
                        <asp:Button ID="btnSave" CssClass="btnSaveClass" runat="server" CausesValidation="false"
                                Text="保存" CommandName="save" OnCommand="Command_Execute" />
                        <asp:Button ID="btnExport" runat="server" ToolTip="导出工单内容" Text="导出" CssClass="btnFWClass"
                            OnClick="btnExport_Click" />
                        <%--<input type="button" id="btnReturn" class="btn_close" value="关闭" onclick="window.location='<%=AppPath%>contrib/workflow/pages/default.aspx';" />--%>
                        
                        <%--<input type="button" id="btnMoreDisplay" class="btn_right" style="display: inline;"
                            onclick="onDisplayCommand(this)" title="更多命令..." />--%>
                        <div id="divAdvanceButton" style="display: none;">
                        <%--<div id="divAdvanceButton">--%>
                            
                            <input type="button" class="btn_left" onclick="onHiddenCommand()" title="折叠." />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="BBSInfo" class="showControl" >
            <h4>评论信息（<span id="spanCommentCount">0</span>）</h4>
            <button class="c" onclick="return showContent(this,'divCommentList1');" title="收缩"><span>折叠</span></button>
        </div>
        <div id="divCommentList1">
            <%--<iframe id="comment_ifAttas" src="<%=this.CommentUrl%>" scrolling="no"
                frameborder="no" width="100%"> onload="this.height=comment_ifAttas.document.body.scrollHeight"
            </iframe>--%>
            <iframe id="comment_ifAttas" src="<%=this.CommentUrl%>" scrolling="no"
                frameborder="no" width="100%" onload="this.height=comment_ifAttas.document.body.scrollHeight;">
            </iframe>
        </div>
    </div> 
    <script language="javascript" type="text/javascript">
    // <!CDATA[
//    var radUrgencyValue = $("#radUrgency").val();
        //    var radImportanceValue = $("#radImportance").val();
        var maintenance = {}
    var s1 = "<% =WorkflowInstanceId %>";
    $(function() {  
       
		$("#divCommentList1").css("display", "none");
		$("#divWorkflowName").html($("#_nowWorkFlowName_").html());//当前流程赋值
		$("#_nowWorkFlow_").css("display", "none");//当前流程tr隐藏
		var nowWorkFlowName = $("#_nowWorkFlowName_").html();
		var activityName = $("#td_activityName").html();
		$("#ProcessHistory th").css("text-align", "center");//处理信息th居中
		
    });
    
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
     
    
    function onPreviewWorkflow(){
        var surl = "<%=this.MapImageUrl %>";
        location = surl;
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
    
    function editForm()
    {
        for (var i = 0, icount = __selectionItems__.length; i < icount; i++){
            bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
        }

        //工单查看时默认表单项为只读
        $('#<%=divDynamicFormContainer.ClientID %> input').each(function () {
            var inputType = $(this).attr('type');
            if (inputType == 'hidden')
                return;
            if (inputType == 'checkbox' || inputType == 'radio') {
                if ($(this).attr('checked')) {
                    //$(this).next('<span>').removeAttr('style');
                    //$(this).next('<span>').hide();
                    $(this).show();
                    //$(this).click(function(){return true;});
                }
                else $(this).show();

                $(this).next('<span>').show();
            }
            else {
                $(this).next('<span>').hide();
                $(this).show();

                if (inputType == 'button') return;

                if ($(this).next('.ico_pickdate'))
                    $(this).next('.ico_pickdate').css("display", "");
                $(this).removeAttr("readonly");
                $(this).removeAttr('style');
            }
        });
        $('#<%=divDynamicFormContainer.ClientID %> textarea').each(function(){
            $(this).removeAttr('readonly');
            $(this).removeAttr('style');
            adjustTextArea(this);
        });
        $('#<%=divDynamicFormContainer.ClientID %> select').each(function(){
            $(this).show();
            $(this).next('<span>').hide();
        });
       
    }
    
    function editBaseInfo()
    {

        //工单查看时默认表单项为只读
	    $('#dataTable1 input').each(function(){
            var inputType = $(this).attr('type');
            $(this).removeAttr('disabled');
        });
        $('#dataTable1 select').each(function(){
            $(this).removeAttr('disabled');
        });
    }

    maintenance.EditOptions = function () {
        $("#tblHistoryEditor textarea").each(function () {
            $(this).show();
            $(this).next("<span>").hide();
        })
        $("#tblHistoryEditor input[type='checkbox']").each(function () {
            $(this).show();
        })
    }

    maintenance.CheckOptions = function (obj) {
        
        if ($(obj).attr("checked")) {
            $("#tblHistoryEditor  input[type='checkbox']").each(function () {
                $(this).attr("checked", "checked");
            })
        }
        else {
            $("#tblHistoryEditor  input[type='checkbox']").each(function () {
                $(this).removeAttr("checked");
            })
        }
    }
    // ]]>
    </script>
    <asp:Literal ID="ltlScripts" runat="server"></asp:Literal>
</asp:Content>
