var WorkflowExtension = {}    //my namespace:)
WorkflowExtension.GetCurrentActNames = function(wiid, state, obj) {//
    var resultStr = function() {
        // 调用 Ajax Web Service
        if (wiid != "") {
            Botwave.XQP.Web.WorkflowExtensionService.GetCurrentActNames(wiid, parseInt(state), successHandler, errorHandler, timeoutHandler);
        }
    };
    var successHandler = function(result) {
        obj.html(result);
    };
    // Ajax 超时.
    var timeoutHandler = function(result) {
        alert("Timeout :" + result);
        //return "";
    };
    // Ajax 错误.
    var errorHandler = function(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
}

WorkflowExtension.GetCurrentActors = function(wiid, state, obj) {//
    var resultStr = function() {
        // 调用 Ajax Web Service
        if (wiid != "") {
            Botwave.XQP.Web.WorkflowExtensionService.GetCurrentActors(wiid, parseInt(state), successHandler, errorHandler, timeoutHandler);
        }
    };
    var successHandler = function(result) {
    obj.html(result);
    };
    // Ajax 超时.
    var timeoutHandler = function(result) {
        alert("Timeout :" + result);
        //return "";
    };
    // Ajax 错误.
    var errorHandler = function(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
}

WorkflowExtension.GetPreActors = function(prevSetId, actor, obj) {//
    var resultString = "";
    var resultStr = function() {
        // 调用 Ajax Web Service
        if (prevSetId != "") {
            Botwave.XQP.Web.WorkflowExtensionService.GetPreActors(prevSetId, actor, successHandler, errorHandler, timeoutHandler);
        }

    };
    var successHandler = function(result) {
    resultString = result;
    obj.html(resultString);
    };
    // Ajax 超时.
    var timeoutHandler = function(result) {
        alert("Timeout :" + result);
        //return "";
    };
    // Ajax 错误.
    var errorHandler = function(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
    //alert(resultString);
}

WorkflowExtension.GetTodoCount = function (actor, func) {//
    var resultString = "0";
    var resultStr = function () {
        // 调用 Ajax Web Service
            Botwave.XQP.Web.WorkflowExtensionService.GetTodoCount(actor, successHandler, errorHandler, timeoutHandler);

    };
    var successHandler = function (result) {
        resultString = result;
        func(resultString);
    };
    // Ajax 超时.
    var timeoutHandler = function (result) {
    };
    // Ajax 错误.
    var errorHandler = function (result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
    //alert(resultString);
}

WorkflowExtension.GetToReviewCount = function (actor, func) {//
    var resultString = "0";
    var resultStr = function () {
        // 调用 Ajax Web Service
            Botwave.XQP.Web.WorkflowExtensionService.GetToReviewCount(actor, successHandler, errorHandler, timeoutHandler);

    };
    var successHandler = function (result) {
        resultString = result;
        func(resultString);
    };
    // Ajax 超时.
    var timeoutHandler = function (result) {
    };
    // Ajax 错误.
    var errorHandler = function (result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
    //alert(resultString);
}

WorkflowExtension.GetDraftCount = function (actor, func) {//
    var resultString = "0";
    var resultStr = function () {
        // 调用 Ajax Web Service
        Botwave.XQP.Web.WorkflowExtensionService.GetDraftCount(actor, successHandler, errorHandler, timeoutHandler);

    };
    var successHandler = function (result) {
        resultString = result;
        func(resultString);
    };
    // Ajax 超时.
    var timeoutHandler = function (result) {
    };
    // Ajax 错误.
    var errorHandler = function (result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
    //alert(resultString);
}

WorkflowExtension.PopupUserPickerOption = {};

WorkflowExtension.OnCheckBoxChange = function (obj) {
    if (WorkflowExtension.PopupUserPickerOption.Type) {
        switch (WorkflowExtension.PopupUserPickerOption.Type) {
            case "Forward":
                if ($(obj).attr("checked"))
                    WorkflowExtension.PopupUserPickerOption.Container.append("<label><input type=\"radio\" name=\"chkboxUser\" value=\"" + $(obj).val() + "\" /><span tooltip=\"" + $(obj).val() + "\">" + $(obj).attr("title") + "</span></label>&nbsp;");
                break;
            case "Activity":
                var index
                break;
        }
    }
    WorkflowExtension.PopupUserPickerOption = {};
}

WorkflowExtension.OnSelectUser = function () {
    if (WorkflowExtension.PopupUserPickerOption.Type) {

        switch (WorkflowExtension.PopupUserPickerOption.Type) {
            case "Forward":
                $("#companyModel input[name='chkCompanyUser']").each(function () {
                    if ($(this).attr("checked")) {
                        if ($("input[name='chkboxUser'][value*='" + $(this).val() + "']").length > 0)
                        { }
                        else {
                            WorkflowExtension.PopupUserPickerOption.Container.append("<label style='margin-bottom: 1em;'><input type=\"radio\" name=\"chkboxUser\" value=\"" + $(this).val() + "\" /><span for='chkboxUser" + $(this).val() + "' tooltip=\"" + $(this).val() + "\" style='margin-right: 1.6em;'>" + $(this).attr("title") + "</span></label>&nbsp;");
                        }
                    }
                });
                break;
            case "Activity":
                var idx = WorkflowExtension.PopupUserPickerOption.Container.attr("index");
                var aid = WorkflowExtension.PopupUserPickerOption.Container.attr("aid");
                var htmlText = "";
                var index = $("#actors_" + idx).children("input[name='activityAllocatee']").length;
                var aid = $("#activityOption_" + idx).attr("value");
                $("#companyModel input[name='chkCompanyUser']").each(function () {
                    if ($(this).attr("checked")) {
                        if ($("#actors_" + idx).children("input[name='activityAllocatee'][value*='" + aid + "$" + $(this).val() + "$']").length > 0) { }
                        else {
                            index++;
                            if (index > 0 && index % 8 == 0) { htmlText += "<br />"; }
                            htmlText += "<input type='checkbox' id='activityAllocatee_" + idx + "_" + index + "' name='activityAllocatee' value='" + aid + "$" + $(this).val() + "$' onclick='onPreSelectAllocatee(\"activityOption_" + idx + "\", this, false);' checked='checked' /><span><span tooltip='" + $(this).val() + "'>" + $(this).attr("title") + '</span></span>';
                        }
                    }
                });
                $("#actors_" + idx).prepend(htmlText);
                break;
            case "Review":
                var values = $("#reviewActors_values").val();
                var names = $("#txtDisplyReviewActors").val();
                $("#companyModel input[name='chkCompanyUser']").each(function () {
                    if ($(this).attr("checked") && names.indexOf($(this).attr("title")) == -1) {
                        values += ("," + $(this).val());
                        names += ("," + $(this).attr("title"));
                    }
                });
                if (values.substring(0, 1) == ",")
                    values = values.substring(1, values.length);
                if (names.substring(0, 1) == ",")
                    names = names.substring(1, names.length);
                $("#reviewActors_values").val(values);
                $("#txtDisplyReviewActors").val(names);
                break;
        }

    }
    WorkflowExtension.PopupUserPickerOption = {};
}

 WorkflowExtension.openPopupUserPick = function(obj) {
    var h = 500; var w = 700; var iTop = (window.screen.availHeight - 30 - h) / 2; var iLeft = (window.screen.availWidth - 10 - w) / 2;
    ActorIndex = $(obj).attr("index");
    window.open('../../security/pages/popupUserPicker2.aspx?func=WorkflowExtension.chooseCompanyActors', '', 'height=' + h + ', width=' + w + ', top=' + iTop + ', left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no'); return false;
}


 WorkflowExtension.chooseCompanyActors = function(result) {
    var htmlText = "";
    var index = $("#actors_" + ActorIndex).children("input[name='activityAllocatee']").length;
    var aid = $("#activityOption_" + ActorIndex).attr("value");
    for (var i = 0; i < result.length; i++) {
        if ($("#actors_" + ActorIndex).children("input[name='activityAllocatee'][value*='$" + result[i].key + "$']").length > 0) { continue; };
        index++;
        if (index > 0 && index % 8 == 0) { htmlText += "<br />"; }

        htmlText += "<input type='checkbox' id='activityAllocatee_" + ActorIndex + "_" + index + "' name='activityAllocatee' value='" + aid + "$" + result[i].key + "$' onclick='onPreSelectAllocatee(\"activityOption_" + ActorIndex + "\", this, false);' checked='checked' /><span><span tooltip='" + result[i].key + "'>" + result[i].value + '</span></span>';
    }
    $("#actors_" + ActorIndex).prepend(htmlText);
    ActorIndex = "";
}

/******************************路由规则**************************************/
WorkflowExtension.GenerRules = function (wid,wiid, actname, actor, dataSet) {
    //debugger
    $("input[activity]").each(function () {
        var nextactname = $(this).attr("activity");
        WorkflowExtension.InitRules(wiid, wid, actname, nextactname, actor, dataSet);
    });
}

/**
* 初始化工单处理页面规则，包括子流程控制规则和步骤路由规则，第一次加载页面
*/
WorkflowExtension.GenerProcessRules = function (wid, wiid, actname, actor, dataSet) {
    var resultStr = function () {
        // 调用 Ajax Web Service
        Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.GenerRelationRules(wiid, wid, actname, actor, true, successHandler, errorHandler, timeoutHandler);
    };
    var successHandler = function (result) {
        var json = eval("(" + result + ")");
        if (json.result == "false") {
            $("input[activity]").each(function () {
                $(this).hide();
                $(this).next("span").hide();
                $(this).removeAttr("checked");
                var index = $(this).attr("id").split('_')[1];
                $("#actors_" + index).hide();
                $("#actors_" + index + " input").removeAttr("checked");
            });
        }
        else {
            $("input[activity]").each(function () {
                var nextactname = $(this).attr("activity");
                WorkflowExtension.InitRules(wiid, wid, actname, nextactname, actor, dataSet);
            });
        }
    };
    // Ajax 超时.
    var timeoutHandler = function (result) {
    };
    // Ajax 错误.
    var errorHandler = function (result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
}

/**
* 初始化规则，第一次加载页面
*/
WorkflowExtension.InitRules = function (wiid, wid, actname, nextactname, actor, dataSet) {
    var resultString = "0";
    $("#ctl00_cphBody_btnApprove,#ctl00_cphBody_btnCreate").hide();
    var resultStr = function () {
        // 调用 Ajax Web Service
        Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.GenerRules(wiid, wid, actname, nextactname, actor, dataSet, true, successHandler, errorHandler, timeoutHandler);
    };
    var successHandler = function (result) {
        var json = "";
        try {
            json = eval("(" + result + ")");
        }
        catch (e) { }
        if (json != "") {
            if (json.result == "false") {
                $("input[activity]").each(function () {
                    if ($(this).attr("activity") == nextactname) {
                        $(this).hide();
                        $(this).next("span").hide();
                        $(this).removeAttr("checked");
                        var index = $(this).attr("id").split('_')[1];
                        $("#actors_" + index).hide();
                        $("#actors_" + index + " input").removeAttr("checked");
                    }

                });
            }
            if (json.result != "none") {
                var FieldsAssemble = json.AssemblyInfo.split(';');
                $('#ctl00_cphBody_divDynamicFormContainer input,#ctl00_cphBody_divDynamicFormContainer textarea').each(function () {
                    var inputType = $(this).attr('type');
                    if (inputType == 'checkbox' || inputType == 'radio') {
                        var key = $(this).attr("name") + "_"
                        for (var i = 0; i < FieldsAssemble.length; i++) {
                            if ("bwdf_cbl" + FieldsAssemble[i] == key || "bwdf_rbl" + FieldsAssemble[i] == key) {
                                $(this).click(function () {
                                    var id = "#" + $(this).attr("id").split('_')[2] + "#";
                                    var hasVal = false;
                                    for (var i = 0; i < dataSet.length; i++) {
                                        if (dataSet[i].split('$')[0] == id) {
                                            dataSet[i] = id + "$" + $(this).val();
                                            hasVal = true;
                                            break;
                                        }
                                    }
                                    if (!hasVal)
                                        dataSet.push(id + "$" + $(this).val());
                                    WorkflowExtension.GenerRulesResult(wiid, wid, actname, nextactname, actor, dataSet)
                                });
                                break;
                            }
                        }
                    }
                    else if (inputType != 'button') {
                        var key = $(this).attr("name") + "_"
                        for (var i = 0; i < FieldsAssemble.length; i++) {
                            if ("bwdf_txt" + FieldsAssemble[i] == key || "bwdf_dat" + FieldsAssemble[i] == key || "bwdf_txa" + FieldsAssemble[i] == key) {
                                $(this).blur(function () {
                                    var id = "#" + $(this).attr("id").split('_')[2] + "#";
                                    for (var i = 0; i < dataSet.length; i++) {
                                        if (dataSet[i].split('$')[0] == id) {
                                            dataSet[i] = id + "$" + $(this).val();
                                            break;
                                        }
                                    }
                                    WorkflowExtension.GenerRulesResult(wiid, wid, actname, nextactname, actor, dataSet)
                                });
                                break;
                            }
                        }
                    }
                });
                $('#ctl00_cphBody_divDynamicFormContainer select').each(function () {
                    var key = $(this).attr("name") + "_"
                    for (var i = 0; i < FieldsAssemble.length; i++) {
                        if ("bwdf_ddl" + FieldsAssemble[i] == key) {
                            $(this).change(function () {
                                var id = "#" + $(this).attr("id").split('_')[2] + "#";
                                for (var i = 0; i < dataSet.length; i++) {
                                    if (dataSet[i].split('$')[0] == id) {
                                        dataSet[i] = id + "$" + $(this).val();
                                        break;
                                    }
                                }
                                WorkflowExtension.GenerRulesResult(wiid, wid, actname, nextactname, actor, dataSet);
                            });
                            break;
                        }
                    }
                });
            }
        }
        $("#ctl00_cphBody_btnApprove,#ctl00_cphBody_btnCreate").show();
    }
    // Ajax 超时.
    var timeoutHandler = function (result) {
    };
    // Ajax 错误.
    var errorHandler = function (result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
};
    /**
    * 动态规则，根据字段内容实时变化
    */
WorkflowExtension.GenerRulesResult = function (wiid, wid, actname, nextactname, actor, dataSet) {
    var resultString = "0";
    $("#ctl00_cphBody_btnApprove,#ctl00_cphBody_btnCreate").hide();
    var resultStr = function () {
        // 调用 Ajax Web Service
        Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.GenerRules(wiid, wid, actname, nextactname, actor, dataSet, false, successHandler, errorHandler, timeoutHandler);
    };
    var successHandler = function (result) {
        var json = "";
        try {
            json = eval("(" + result + ")");
        }
        catch (e) { }
        if (json != "") {
            if (json.result == "false") {
                $("input[activity]").each(function () {
                    if ($(this).attr("activity") == nextactname) {
                        $(this).hide();
                        $(this).next("span").hide();
                    }
                    $(this).removeAttr("checked");
                    var index = $(this).attr("id").split('_')[1];
                    $("#actors_" + index).hide();
                    $("#actors_" + index + " input").removeAttr("checked");
                });
            }
            else {
                $("input[activity]").each(function () {
                    if ($(this).attr("activity") == nextactname) {
                        $(this).show();
                        $(this).next("span").show();
                    }
                    $(this).removeAttr("checked");
                });
            }
        }
        $("#ctl00_cphBody_btnApprove,#ctl00_cphBody_btnCreate").show();
    }
    // Ajax 超时.
    var timeoutHandler = function (result) {
    };
    // Ajax 错误.
    var errorHandler = function (result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
}

        /**
        * 根据表单字段内容动态改变字段内容集合
        */
WorkflowExtension.GenerFieldsContext = function (dataSet) {
    $('#ctl00_cphBody_divDynamicFormContainer input').each(function () {
        var inputType = $(this).attr('type');
        if (inputType == 'checkbox' || inputType == 'radio') {
            if ($(this).attr('checked')) {
                //arr.push("#" + $(this).attr("id").split('_')[2] + "#$" + $(this).val());
                var key = "#" + $(this).attr("id").split('_')[2] + "#";
                for (var i = 0; i < dataSet.length; i++) {
                    if (dataSet[i] == key) {
                        dataSet[i] = $(this).val();
                    }
                }
            }
        }
        else {
            if (inputType == 'button') return;
            //arr.push("#" + $(this).attr("id").split('_')[2] + "#$" + $(this).val());
            var key = "#" + $(this).attr("id").split('_')[2] + "#";
            for (var i = 0; i < dataSet.length; i++) {
                if (dataSet[i] == key) {
                    dataSet[i] = $(this).val();
                }
            }
        }
    });
    return dataSet;
}
   

/*****************************路由规则end************************************/



//这个方法做了一些操作、然后调用回调函数
//code:方法名
//args:参数集合 
WorkflowExtension.Eval = function (code, args) {
    if (!!(window.attachEvent && !window.opera)) {
        //ie
        doCallback(window[code], args);
    } else {
        //not ie
        doCallback(window[code], args);
    }
}

function doCallback(fn, args) {
    try {
        fn.apply(this, args);
    }
    catch (e) {
    }
}