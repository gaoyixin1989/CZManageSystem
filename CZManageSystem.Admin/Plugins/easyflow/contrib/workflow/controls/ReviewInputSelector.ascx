<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_ReviewInputSelector" Codebehind="ReviewInputSelector.ascx.cs" %>

<div class="dataTable" id="divReadersContainer">
        <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 6px;">
            <tr>
                <td style="padding-left:120px">
                    <div style="font-size:14px; font-weight:bold; padding-top:3px; padding-bottom:3px">
                        抄送人列表
                    </div>
                    <div>
                        <input type="text" style="width:360px" id="txtDisplyReviewActors" readonly="readonly" />
                        <a href="javascript:void(0);" onclick="return onOpenReviewPicker();" style="font-weight:bold;" title="选择抄送人">选择抄送人</a> -
                        <a href="javascript:void(0);" onclick="return onClearReviews();" style="font-weight:bold;" title="清除已选择的抄送人">清除已选择</a>
                    </div>
                    <asp:HiddenField ID="txtReviewActors" runat="server"></asp:HiddenField>
                </td>
            </tr>        
            <%--<tr>
                <th style="width: 13%;">
                    抄送方式：
                </th>
                <td>
                        <input type="checkbox" id="chkBeRead" checked="checked" name="chkBeRead" runat="server" />待阅
                        <input type="checkbox" id="chkSms" name="chkSms" runat="server" />短信                                    
                        <input type="checkbox" id="chkEmail" name="chkEmail" runat="server" />邮件              
                </td>
            </tr>--%>
        </table>
</div>
<script type="text/javascript">
<!--//
function onOpenReviewPicker(){
    var h = 450;
    var w = 700;
    var iTop = (window.screen.availHeight-30-h)/2;    
    var iLeft = (window.screen.availWidth-10-w)/2; 
    window.open('<%=AppPath%>contrib/security/pages/popupUserPicker2.aspx?func=onCompletePickReviews', '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');	
    return false;
}
function onClearReviews(){
    if(confirm("确定要清除已选择的抄送人？")){
        $("#<%=txtReviewActors.ClientID%>").val("");
        $("#txtDisplyReviewActors").val("");
    }
    return true;
}
function onCompletePickReviews(result){
    var values = $("#<%=txtReviewActors.ClientID%>").val();
    var names = $("#txtDisplyReviewActors").val();
    for(var i=0; i<result.length;i++){
        values += ("," + result[i].key);
        names += ("," + result[i].value);
    }
    if(values.substring(0, 1) == ",")
        values = values.substring(1, values.length);
    if(names.substring(0, 1) == ",")
        names = names.substring(1, names.length);
    $("#<%=txtReviewActors.ClientID%>").val(values);
    $("#txtDisplyReviewActors").val(names);
}
function validateReviewPicker() {
    var values = $("#<%=txtReviewActors.ClientID %>").val();
    if (values == ""){
        alert('请选择抄送人.');
        return false;
    }
    return true;
}
//-->
</script>