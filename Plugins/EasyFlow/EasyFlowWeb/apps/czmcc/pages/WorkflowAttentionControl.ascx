<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_czmcc_pages_WorkflowAttentionControl" Codebehind="WorkflowAttentionControl.ascx.cs" %>
<input type="button" id="buttonAttention1" class="btnReview" value='<%=IsExists?"-取消关注":"+加入关注"%>' style='color:<%=IsExists?"red":"green"%>'  onclick="attentionClick();" />
<script type="text/javascript">
    var attentionCommand = '<%=IsExists? "attention-del":"attention-add" %>';
    function attentionClick() {
        $.post("<%=AppPath%>apps/czmcc/pages/workflowRelationAjax.aspx", { action: attentionCommand, wiid: "<%=WorkflowInstanceId%>", type: "<%=AttentionType%>", actor: "<%=Actor%>" },
                function(result) {
                    if (result && result.indexOf("成功", 0) >= 0) {
                        alert(result);
                        window.location = "<%=Request.Url%>";
                    } else {
                        alert((!result) ? "关注失败，请重试。" : result);
                    }
                });
    };
</script>