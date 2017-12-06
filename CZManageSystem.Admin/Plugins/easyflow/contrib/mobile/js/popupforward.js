var popupforward = {};

popupforward.GetDialogHtml = function (orghtml) {
    var DialogHtml = "<!--<div class=\"modal fade\" id=\"assignModal\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"myModalLabel\" aria-hidden=\"true\" style=\"top: 60px\">-->"
        + "<div class=\"modal-dialog\"> <div class=\"modal-content\"> <div class=\"modal-header\"> <button type=\"button\" class=\"close\" data-dismiss=\"modal\">"
        + "<span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button> <h4 class=\"modal-title\" id=\"myModalLabel\"> 转交工单</h4></div>"
        + "<div class=\"modal-body\"><div style=\"width: 100%; height: 256px; overflow: auto; border: solid 1px #ccc;padding-top:.6em\">" + orghtml + "</div></div><div class=\"modal-footer\"><button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">取消</button>"
        + "<button type=\"button\" class=\"btn btn-primary\" onclick=\"return popupforward.validateCheck()\">转交</button></div></div></div><!--</div>-->"
    return DialogHtml;
};
popupforward.PopupForward = function (wiid, aiid) {
    $('#assignModal').modal('show')
    $.ajax({
        type: "post",
        dataType: "json",
        url: "../ajax/PopupForwardAjax.aspx",
        data: { wiid: wiid
                    , aiid: aiid
        },
        async: true,
        timeout: 300000,
        success: function (data) {
            //alert(data)
            var json = data;
            var resultHtml = "";
            if (json.result == "error")
                resultHtml = popupforward.GetDialogHtml(json.info);
            else {
                var jsonR = json.info;
                for (var i = 0; i < jsonR.length; i++) {
                    resultHtml += "<div class=\"row\" style=\"margin-left:0px;margin-right: 0px;\"><div class=\"col-xs-12 col-md-12\"><div class=\"form-group\"><label for=\"exampleInputPassword1\">" + jsonR[i].title + "</label><div class=\"radio\">" + jsonR[i].html + "</div></div></div></div>";
                }
                resultHtml = popupforward.GetDialogHtml(resultHtml);
            }
            $("#assignModal").html(resultHtml);
            
        },
        error: function (e) {
            alert("加载转交失败");
        }

    });
};

popupforward.validateCheck = function () {
    var hasSelected = false;
    $("input[name='chkboxUser']").each(function () {
        if (this.checked) {
            hasSelected = true;
            $('#assignModal').modal('hide')
            showMaskLayout();
            $("#ctl00_cphBody_btnFw").trigger("click");
        }
    });
    if (!hasSelected)
        alert("请选择转交人");
    return hasSelected;
}

popupforward.OpenPopupUserPick = function () {
    WorkflowExtension.PopupUserPickerOption.Type = "Forward";
    WorkflowExtension.PopupUserPickerOption.Container = $("#divAssignActorFromCompany");
    $("#companyModel").modal("show");
    if ($("#ctl00_cphBody_treeDepts").children().length==0)
        $("#ctl00_cphBody_btnChoose").trigger("click");
}

popupforward.OpenPopupUserPick = function () {
    WorkflowExtension.PopupUserPickerOption.Type = "Forward";
    WorkflowExtension.PopupUserPickerOption.Container = $("#divAssignActorFromCompany");
    $("#companyModel").modal("show");
    if ($("#ctl00_cphBody_treeDepts").children().length == 0)
        $("#ctl00_cphBody_btnChoose").trigger("click");
}