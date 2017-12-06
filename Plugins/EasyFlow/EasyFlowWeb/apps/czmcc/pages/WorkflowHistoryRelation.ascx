<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_czmcc_pages_WorkflowHistoryRelation" Codebehind="WorkflowHistoryRelation.ascx.cs" %>
<div class="showControl hideDiv">
    <h4>历史工单关联</h4>
    <button onclick="return showContent(this,'relationHistoryContent');" title="收缩"><span>折叠</span></button>
</div>
<div id="relationHistoryContent" class="hideDiv">
<% if (Editable){ %>
    <input type="button" class="button2l" style="display: block" id="buttonPopupHistorySheets" value="选择历史工单" onclick="popupHistorySheets();">
    <%} %>
    <asp:HiddenField ID="hiddenRelationID" runat="server" />
    <table class="tblGrayClass" style="text-align: center; width:100%;margin-top:5px;" cellpadding="4" cellspacing="0">
        <tr id="relationHistoryHeader" style="text-align:center;">
	        <th style="width:95px;">受理号</th>
	        <th style="text-align:left">工单标题</th>
	        <th style="width:130px;">当前步骤</th>
	        <th style="width:52px;">引用附件</th>
	        <th style="width:110px;">发起人</th>
	        <th style="width:110px;">发起时间</th>
            <% if (Editable){ %>
            <th style="width:50px;">删除</th>
            <%} %>
        </tr>
        <tbody id="relationHistoryTableBody"></tbody>
    </table>
    <div id="relationHistoryTableLoading" style="margin:10px; color:Red; display:none;">
        正在加载，请稍后...
    </div>
</div>
<script type="text/javascript">
    isIE6 = ($.browser.msie && $.browser.version == "6.0");
    function loadHistoryRelations(refreshAttachment) {
        relID = "<%=this.RelationID%>";
        started = '<%= this.Starting ? "true" : "false"%>';
        editable = '<%= this.Editable ? "true" : "false"%>';
        $("#relationHistoryTableLoading").css("display", "");
        $.post("<%=Botwave.Web.WebUtils.GetAppPath()%>apps/czmcc/pages/workflowRelationAjax.aspx", { action: "relation", rel: relID, started: started, editable: editable },
                function(result) {
                    $("#relationHistoryTableLoading").css("display", "none");
                    $("#relationHistoryTableBody").html(result);
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
        loadHistoryRelations(0);
    });
</script>