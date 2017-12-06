<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_czmcc_pages_WorkflowRelation" Codebehind="WorkflowRelation.ascx.cs" %>
<div class="showControl">
    <h4>子流程工单关联</h4>
    <button onclick="return showContent(this,'relationContent');" title="收缩"><span>折叠</span></button>
</div>
<div id="relationContent">
<% if (Editable){ %>
    <input type="button" value="发起工单" id="btnStartEF"  runat="server" onclick="onStartWorkflow()"  class="big_btn" />
    <%} %>
    <asp:HiddenField ID="hiddenRelationID" runat="server" />
    <table class="tblGrayClass" style="text-align: center; width:100%;margin-top:5px;" cellpadding="4" cellspacing="0">
        <tr id="relationHeader" style="text-align:center;">
	        <th style="width:95px;">受理号</th>
	        <th style="text-align:left">工单标题</th>
	        <th style="width:130px;">当前步骤</th>
	        <th style="width:110px;">发起人</th>
	        <th style="width:110px;">发起时间</th>
            <th style="width:110px;">当前处理人</th>
            <% if (Editable){ %>
            <%--<th style="width:50px;">取消关联</th>--%>
            <%} %>
        </tr>
        <tbody id="relationTableBody"></tbody>
    </table>
    <div id="relationTableLoading" style="margin:10px; color:Red; display:none;">
        正在加载，请稍后...
    </div>
</div>
<script type="text/javascript">
    isIE6 = ($.browser.msie && $.browser.version == "6.0");
    function loadRelations(refreshAttachment) {
        relID = "<%=this.RelationID%>";
        started = '<%= this.Starting ? "true" : "false"%>';
        editable = '<%= this.Editable ? "true" : "false"%>';
        $("#relationTableLoading").css("display", "");
        $.post("<%=Botwave.Web.WebUtils.GetAppPath()%>apps/czmcc/pages/workflowRelationAjax.aspx", { action: "relationlist", rel: relID, started: started, editable: editable },
                function(result) {
                    $("#relationTableLoading").css("display", "none");
                    $("#relationTableBody").html(result);
                    // 刷新附件.
                    if (refreshAttachment != 0 && document.getElementById("ifAttachment")) {
                        var url = $("#ifAttachment").attr("src");
                        $("#ifAttachment").attr("src", url + "&".concat(Math.random()));
                    }
                });
    };
    function popupHistorySheets() {
        var dialogWidth = isIE6 ? "740px" : "730px";
        var dialogHeight = isIE6 ? "550px" : "500px";

        relationID = "<%=this.RelationID%>";
        var returnValue = window.showModalDialog("<%=Botwave.Web.WebUtils.GetAppPath()%>apps/czmcc/pages/workflowRelationPopup.aspx?rel=" + relationID, "", "dialogWidth=" + dialogWidth + ";dialogheight=" + dialogHeight + ";status=no;scroll=no;help=no");

        if (returnValue && returnValue == true) {
            loadHistoryRelations();
        }
        return returnValue;
    };
    function deleteHistoryRelation(o, id) {
        if (confirm("确定要删除这个历史工单的关联？")) {

            $.post("<%=Botwave.Web.WebUtils.GetAppPath()%>apps/czmcc/pages/workflowRelationAjax.aspx", { action: "delete", id: id },
                function(result) {
                    if (result && result != "") {
                        alert(result);
                    } else {
                        $(o).parent().parent().empty();
                        if (document.getElementById("ifAttachment")) {
                            var url = $("#ifAttachment").attr("src");
                            $("#ifAttachment").attr("src", url + "&".concat(Math.random()));
                        }
                    }
                });
        }
        return false;
    }
    $(function() {
        loadRelations(0);
    });
    function onStartWorkflow() {
        var wiid = $("#Hiddenwiid").val();
        var number = $("#HiddennNumber").val();
        var name = $("#HiddenName").val();
        if (wiid != "") {

            var url = "<%=Botwave.Web.WebUtils.GetAppPath()%>contrib/Workflow/Interface/Start.aspx?wid=<%=this.WorkflowId%>&parentid=<%=this.RelationID%>";
            openWindow(url, 900, 500);
            // window.open("/Workflow/Interface/StartSubflow.aspx?wfalias=AB&parentid=" + wiid, "_blank");
        }
    }

    /*弹出窗口*/
    function openWindow(url, w, h) {
        var top, left;
        if (h > 600) {
            top = 0;
            h = screen.height;
        } else {
            top = (screen.height - h) / 2;
        }
        left = (screen.width - w) / 2;
        window.open(url, "newSelect", "width=" + w + ",height=" + h + ",top=" + top + ",left=" + left + ",hotkeys=1,menubar=0,resizable=1,scrollbars=1");
    }
</script>