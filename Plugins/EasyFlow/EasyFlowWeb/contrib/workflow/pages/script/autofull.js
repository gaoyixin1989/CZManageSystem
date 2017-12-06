
//**********************获取数据/方法集合***************************************//
var AutoFull = {}
AutoFull.FormItemDefinitionList = [];//表单设置集合
function getDataSet(wid, wiid, dataSet, state, divId) {
    var resultStr = function() {
        // 调用 Ajax Web Service
        if (wid != "") {
            Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.getDataSet(wid, wiid, successHandler, errorHandler, timeoutHandler);
        }
    };
    var successHandler = function (result) {
        if (result != "") {
            var json = "";
            try {
                json = eval("[" + result + "]");
            }
            catch (e) { }
            //alert(result)
            if (json != "") {
                var arrlist = new Array();

                for (var i = 0; i < json.length; i++) {
                    var obj = json[i];
                    arrlist.push({ Key: obj.Key, ID: obj.ID, Type: obj.Type });
                }
                BindDynamicFormItems(state, divId);
                for (var i = 0; i < arrlist.length; i++) {
                    //getData(arrlist[i].ID, arrlist[i].Key, arrlist[i].Type, dataSet, state);
                    //GenerJSFunction(arrlist[i].ID, arrlist[i].Key, arrlist[i].Type, dataSet, state, wiid);
                    //ExistItemsLinkage(arrlist[i].ID, arrlist[i].Key, arrlist[i].Type, state);
                    var id = arrlist[i].ID, key = arrlist[i].Key, type = arrlist[i].Type;
                    getData(id, key, type, dataSet, state);
                    EncryptStrings(id, key, type, dataSet, state, wiid)
                    GenerJSFunction(id, key, type, dataSet, state, wiid);
                    ExistItemsLinkage(id, key, type, state);
                    GenerItemsRules(id, key, type, state)
                }
                AutoFull.FormItemDefinitionList = arrlist;
            }
        }
        else
            BindDynamicFormItems(state, divId);

        //return "";
    };
    // Ajax 超时.
    var timeoutHandler = function(result) {
        //BindDynamicFormItems(state,divId);
        //BindDynamicFormInputItems(state);
        alert("Timeout :" + result);
        //return "";
    };
    // Ajax 错误.
    var errorHandler = function(result) {
        //BindDynamicFormItems(state,divId);
        //BindDynamicFormInputItems(state);
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
        //return "";
    };
    resultStr();
}
//***************************end*****************************************************//

//**************************获取外部数据源集合***************************************//
function getData(id, key, type, dataSet, state) {
    var resultStr = function() {
        // 调用 Ajax Web Service
        switch (type) {//可编辑时才填充数据
            case "Text":
                if (state == 0 || $("#bwdf_txt_" + key).attr("contentEditable") == "true")
                    Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.getOuterDataSet(id, key, type, dataSet, successHandler, errorHandler, timeoutHandler);
                break;
            case "TextArea":
                if (state == 0 || $("#bwdf_txa_" + key).attr("contentEditable") == "true")
                    Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.getOuterDataSet(id, key, type, dataSet, successHandler, errorHandler, timeoutHandler);
                break;
            case "Date":
                if (state == 0 || $("#bwdf_dat_" + key).attr("contentEditable") == "true")
                    Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.getOuterDataSet(id, key, type, dataSet, successHandler, errorHandler, timeoutHandler);
                break;
            case "CheckBoxList":
                //if ($("#bwdf_ddl_" + key).attr("contentEditable") == "true")
                Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.getOuterDataSet(id, key, type, dataSet, successHandler, errorHandler, timeoutHandler);
                break;
            case "DropDownList":
                //if ($("#bwdf_ddl_" + key).attr("contentEditable") == "true")
                Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.getOuterDataSet(id, key, type, dataSet, successHandler, errorHandler, timeoutHandler);
                break;
            case "RadioButtonList":
                //if ($("#bwdf_ddl_" + key).attr("contentEditable") == "true")
                Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.getOuterDataSet(id, key, type, dataSet, successHandler, errorHandler, timeoutHandler);
                break;
            default:
                break;
        }

        //}
    };
    var successHandler = function(result) {
        if (result != "") {
            var json = "";
            try {
                json = eval("[" + result + "]");
            }
            catch (e) { }
            if (json != "") {
                var arrlist = new Array();
                var valList = new Array();
                for (var i = 0; i < json.length; i++) {//遍历json
                    var obj = json[i];
                    arrlist.push({ Type: obj.Type, Value: obj.Value });
                    if (typeof (obj.Value) == 'object') {
                        for (var j = 0; j < obj.Value.length; j++) {
                            valList.push({ Value: obj.Value[j].Value });

                        }
                    }
                }
                for (var i = 0; i < arrlist.length; i++) {

                    var resultStr = arrlist[i].Value;
                    switch (type) {
                        case "Text":
                            AddDataSet(key, type, resultStr, state);
                            break;
                        case "TextArea":
                            AddDataSet(key, type, resultStr, state);
                            break
                        case "Data":
                            AddDataSet(key, type, resultStr, state);
                            break;
                        case "CheckBoxList":
                            AddDataSet(key, type, valList, state);
                            break;
                        case "DropDownList":
                            AddDataSet(key, type, valList, state);
                            break;
                        case "RadioButtonList":
                            AddDataSet(key, type, valList, state);
                            break;
                        default:
                            break;
                    }
                    break;
                }
                //alert(result);
            } 
        }
        //return "";
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
        //return "";
    };
    resultStr();
}
//***************************end*****************************************************//

//********************************获取js方法******************************//
function GenerJSFunction(id, key, type, dataSet, state, wiid) {
    var resultStr = function() {
        switch (type) {//可编辑时才填充数据
            case "Text":
                if (state == 0 || $("#bwdf_txt_" + key).attr("contentEditable") == "true")
                    Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.GenerJSFunction(id, type, dataSet, successHandler, errorHandler, timeoutHandler);
                //else
                    //Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.EncryptStrings(wiid, id, type, successHandler, errorHandler, timeoutHandler);
                break;
            case "TextArea":
                if (state == 0 || $("#bwdf_txa_" + key).attr("contentEditable") == "true")
                    Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.GenerJSFunction(id, type, dataSet, successHandler, errorHandler, timeoutHandler);
                //else
                    //Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.EncryptStrings(wiid, id, type, successHandler, errorHandler, timeoutHandler);
                break;
            default:
                break;
        }
    };
    var successHandler = function(result) {
        if (result != "") {
            var json = "";
            try {
                json = eval("[" + result + "]");
            }
            catch (e) { }
            if (json != "") {
                var arrlist = new Array();
                for (var i = 0; i < json.length; i++) {//遍历json
                    var obj = json[i];
                    arrlist.push({ Type: obj.Type, EventName: obj.EventName, FunctionName: obj.FunctionName, src: obj.src, value: obj.Value, useto: obj.UseTo });
                }
                for (var i = 0; i < arrlist.length; i++) {

                    var functionName = arrlist[i].FunctionName;
                    switch (type) {
                        case "Text":
                            if (arrlist[i].useto == "EncryptStrings") {
                                //
                                //var val = arrlist[i].value;
                                //$("#bwdf_txt_" + key).attr("value",val);
                                //document.getElementById("bwdf_txt_" + key).value = val;
                            }
                            else {
                                var getjsT = loadJS(key, decodeURIComponent(arrlist[i].src) + "?" + Math.random());
                                if (getjsT) {
                                    functionName = functionName.replace("#" + key + "#", "");
                                    switch (arrlist[i].EventName) {
                                        case "onblur":

                                            $("#bwdf_txt_" + key).blur(function() { X2.Eval(functionName, ["#bwdf_txt_" + key]); });
                                            break;
                                        case "onkeypress":
                                            $("#bwdf_txt_" + key).keypress(function() { X2.Eval(functionName, ["#bwdf_txt_" + key]); });
                                            break;
                                        case "onkeyup":
                                            $("#bwdf_txt_" + key).keyup(function() { X2.Eval(functionName, ["#bwdf_txt_" + key]); });
                                            break;
                                    }
                                }
                            }
                            break;
                        case "TextArea":
                            if (arrlist[i].useto == "EncryptStrings") {
                                //$("#bwdf_txa_" + key).attr("value", arrlist[i].value);
                            }
                            else {
                                var getjsA = loadJS(key, decodeURIComponent(arrlist[i].src) + "?" + Math.random());
                                if (getjsA) {
                                    functionName = functionName.replace("#" + key + "#", "");
                                    switch (arrlist[i].EventName) {
                                        case "onblur":

                                            $("#bwdf_txa_" + key).blur(function() { X2.Eval(functionName, ["#bwdf_txa_" + key]); });
                                            break;
                                        case "onkeypress":
                                            $("#bwdf_txa_" + key).keypress(function() { X2.Eval(functionName, ["#bwdf_txa_" + key]); });
                                            break;
                                        case "onkeyup":
                                            $("#bwdf_txa_" + key).keyup(function() { X2.Eval(functionName, ["#bwdf_txa_" + key]); });
                                            break;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                } 
            }
        }
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
        //return "";
    };
    resultStr();
}

//加密字符
function EncryptStrings(id, key, type, dataSet, state, wiid) {
    var resultStr = function() {
        switch (type) {//不可编辑时才填充数据
            case "Text":
                //if (state == 0 || $("#bwdf_txt_" + key).attr("contentEditable") == "true")
                Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.EncryptStrings(wiid, id, type, successHandler, errorHandler, timeoutHandler);
                break;
            case "TextArea":
                Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.EncryptStrings(wiid, id, type, successHandler, errorHandler, timeoutHandler);
                break;
            default:
                break;
        }
    };
    var successHandler = function(result) {
        if (result != "") {
            var json = "";
            try {
                json = eval("[" + result + "]");
            }
            catch (e) { }
            if (json != "") {
                var arrlist = new Array();
                for (var i = 0; i < json.length; i++) {//遍历json
                    var obj = json[i];
                    arrlist.push({ Type: obj.Type, EventName: obj.EventName, FunctionName: obj.FunctionName, src: obj.src, value: obj.Value, oldvalue: obj.OldValue, useto: obj.UseTo });
                }
                for (var i = 0; i < arrlist.length; i++) {

                    var functionName = arrlist[i].FunctionName;
                    var old = arrlist[i].oldvalue;
                    switch (type) {
                        case "Text":
                            if (arrlist[i].useto == "EncryptStrings") {
                                if (state == 0 || $("#bwdf_txt_" + key).attr("contentEditable") == "true") {
                                    if (old != "")
                                        $("#bwdf_txt_" + key).attr("value", old);
                                }
                                else {
                                    var val = arrlist[i].value;
                                    //$("#bwdf_txt_" + key).attr("value", val);
                                    //$(this).hide("fast");
                                    $("#spContent_bwdf_txt_" + key).html(val);
                                    $("#ctl00_cphBody_btnApprove").click(function() {
                                        $("#bwdf_txt_" + key).hide();
                                        $("#bwdf_txt_" + key).attr("value", old);
                                    });
                                    $("#ctl00_cphBody_btnSave").click(function() {
                                        $("#bwdf_txt_" + key).hide();
                                        $("#bwdf_txt_" + key).attr("value", old);
                                    });
                                    $("#ctl00_cphBody_btnReject").click(function() {
                                        $("#bwdf_txt_" + key).hide();
                                        $("#bwdf_txt_" + key).attr("value", old);
                                    });
                                }
                            }
                            break;
                        case "TextArea":
                            if (arrlist[i].useto == "EncryptStrings") {
                                if (state == 0 || $("#bwdf_txa_" + key).attr("contentEditable") == "true") {
                                    if (old != "")
                                        $("#bwdf_txa_" + key).attr("value", old);
                                }
                                else {
                                    var val = arrlist[i].value;
                                    $("#bwdf_txa_" + key).attr("value", val);
                                    $("#ctl00_cphBody_btnApprove").click(function() {
                                        $("#bwdf_txa_" + key).hide();
                                        $("#bwdf_txa_" + key).attr("value", old);
                                    });
                                    $("#ctl00_cphBody_btnSave").click(function() {
                                        $("#bwdf_txa_" + key).hide();
                                        $("#bwdf_txa_" + key).attr("value", old);
                                    });
                                    $("#ctl00_cphBody_btnReject").click(function() {
                                        $("#bwdf_txt_" + key).hide();
                                        $("#bwdf_txt_" + key).attr("value", old);
                                    });
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            } 
        }
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
        //return "";
    };
    resultStr();
}
//***************************end*****************************************************//

//**************************判断是否存在字段联动*********************//
function ExistItemsLinkage(id, key, type, state) {
    var resultStr = function() {
        switch (type) {//可编辑时才填充数据
            case "DropDownList":
            case "Text":
            case "TextArea":
            case "CheckBoxList":
            case "RadioButtonList":
            case "Data":
                //if (state == 0 || $("#bwdf_ddl_" + key).attr("contentEditable") == "true")
                    Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.ExistItemsLinkage(id, successHandler, errorHandler, timeoutHandler);
                break;
            default:
                break;
        }
    };
    var successHandler = function (result) {
        if (result != "") {
            if (result == "true") {
                //if (state == 0 || $("#bwdf_ddl_" + key).attr("contentEditable") == "true")
                //$("#bwdf_ddl_" + key).change(function () { GenerItemsLinkage(id, key, type, $(this).val(), state); });
                switch (type) {//可编辑时才填充数据
                    case "DropDownList":
                        $("#bwdf_ddl_" + key).change(function () { GenerItemsLinkage(id, key, type, $(this).val(), state); });
                        break;
                    case "Text":
                        $("#bwdf_txt_" + key).blur(function () { GenerItemsLinkage(id, key, type, $(this).val(), state); });
                        break;
                    case "TextArea":
                        $("#bwdf_txa_" + key).blur(function () { GenerItemsLinkage(id, key, type, $(this).val(), state); });
                        break;
                    case "Data":
                        $("#bwdf_dat_" + key).blur(function () { GenerItemsLinkage(id, key, type, $(this).val(), state); });
                        break;
                    case "CheckBoxList":
                        $("input[name='bwdf_cbl_" + key + "']").click(function () { GenerItemsLinkage(id, key, type, $(this).val(), state); });
                        break;
                    case "RadioButtonList":
                        $("input[name='bwdf_rbl_" + key + "']").click(function () { GenerItemsLinkage(id, key, type, $(this).val(), state); });
                        break;
                }
                GenerItemsLinkage(id, key, type, $("#bwdf_ddl_" + key).val(), state);
            }
        }
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
        //return "";
    };
    resultStr();
}
//***************************end*****************************************************//

//**************************加载字段联动*****************************//
function GenerItemsLinkage(id, key, type, fval, state) {
    var resultStr = function() {
        //if (state == 0 || $("#bwdf_txt_" + key).attr("contentEditable") == "true")
        Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.GenerItemsLinkage(id, type, successHandler, errorHandler, timeoutHandler);
    };
    var successHandler = function (result) {
        if (result != "") {
            var json = "";
            try {
                json = eval(result);
            }
            catch (e) { }
            if (json != "") {
                var arrlist = new Array();
                var ChildrenList = new Array();
                for (var i = 0; i < json.length; i++) {//遍历json
                    var ctlId = "";
                    var obj = json[i];
                    if ($("#bwdf_ddl_" + obj.FName).length > 0)
                        ctlId = "#bwdf_ddl_" + obj.FName;
                    else if ($("#bwdf_txt_" + obj.FName).length > 0)
                        ctlId = "#bwdf_txt_" + obj.FName;
                    else if ($("#bwdf_dat_" + obj.FName).length > 0)
                        ctlId = "#bwdf_dat_" + obj.FName;
                    else if ($("#bwdf_txa_" + obj.FName).length > 0)
                        ctlId = "#bwdf_txa_" + obj.FName;
                    else if ($("input[name='bwdf_cbl_" + obj.FName + "']").length > 0)
                        ctlId = "input[name='bwdf_cbl_" + obj.FName + "']";
                    else if ($("input[name='bwdf_rbl_" + obj.FName + "']").length > 0)
                        ctlId = "input[name='bwdf_cbl_" + obj.FName + "']";
                    else if ($("#divHTMLContainer_" + obj.FName).length > 0) {
                        ctlId = "#divHTMLContainer_" + obj.FName;
                    }
                    if (ctlId == "")
                        continue;

                    if (ChildrenList.length == 0)
                        ChildrenList.push(ctlId);
                    else {
                        for (var j = 1; j < ChildrenList.length; j++) {
                            if (ChildrenList[j] != ChildrenList[j - 1])
                                ChildrenList.push(ctlId);
                        }
                    }
                    if (obj.Parent == fval) {
                        arrlist.push({ FName: obj.FName, FValue: obj.FValue, Parent: obj.Parent });
                    }
                }
                for (var j = 0; j < ChildrenList.length; j++) {
                    if ($(ChildrenList[j]).is("select"))
                        $(ChildrenList[j]).empty();
                    else if ($(id).attr("type") == "checkbox" || $(id).attr("type") == "radio") {
                        $(ChildrenList[j]).removeAttr("checked")
                        $(ChildrenList[j]).hide();
                    }
                    else
                        $(ChildrenList[j]).attr("value", "");
                }
                for (var i = 0; i < arrlist.length; i++) {//绑定控件值
                    var ctlType = "";
                    if ($("#bwdf_ddl_" + arrlist[i].FName).length > 0) {
                        $("#bwdf_ddl_" + arrlist[i].FName).append("<option value=\"" + arrlist[i].FValue + "\">" + arrlist[i].FValue + "</option>");
                        ctlType = "DropDownList";
                    }
                    else if ($("#bwdf_txt_" + arrlist[i].FName).length > 0) {
                        ctlType = "Text";
                        $("#bwdf_txt_" + arrlist[i].FName).val(arrlist[i].FValue);
                        //设置焦点，触发其他焦点离开事件
                        //$("#bwdf_txt_" + arrlist[i].FName).focus().select();
                        $("#bwdf_txt_" + arrlist[i].FName).focus();
                    }
                    else if ($("#bwdf_dat_" + arrlist[i].FName).length > 0) {
                        ctlType = "Date";
                        $("#bwdf_dat_" + arrlist[i].FName).val(arrlist[i].FValue);
                        //设置焦点，触发其他焦点离开事件
                        $("#bwdf_dat_" + arrlist[i].FName).focus();
                    }
                    else if ($("#bwdf_txa_" + arrlist[i].FName).length > 0) {
                        ctlType = "TextArea";
                        $("#bwdf_txa_" + arrlist[i].FName).val(arrlist[i].FValue);
                        //设置焦点，触发其他焦点离开事件
                        $("#bwdf_txa_" + arrlist[i].FName).focus();
                    }
                    else if ($("input[name='bwdf_cbl_" + arrlist[i].FName + "']").length > 0) {//复选框
                        ctlType = "CheckBoxList";
                        $("input[name='bwdf_cbl_" + arrlist[i].FName + "']").each(function () {
                            if ($(this).val() == arrlist[i].FValue) {
                                $(this).show();
                                //$(this).attr("checked")
                            }
                        })
                    }
                    else if ($("input[name='bwdf_rbl_" + arrlist[i].FName + "']").length > 0) {//单选框

                        ctlType = "RadioButtonList";
                        $("input[name='bwdf_rbl_" + arrlist[i].FName + "']").each(function () {
                            if ($(this).val() == arrlist[i].FValue) {
                                $(this).show();
                                //$(this).attr("checked")
                            }
                        });
                    }
                    else if ($("#divHTMLContainer_" + arrlist[i].FName).length > 0) {
                        ctlType = "Html";
                    }
                    if (arrlist[i].FValue == "{GetOuterData}") {
                        for (var idx = 0; idx < FormDataSet.length; idx++) {
                            var dkey = FormDataSet[idx].split('$')[0];
                            if (dkey == "#" + key + "#") {
                                FormDataSet[idx] = "#" + key + "#$" + arrlist[i].Parent;
                                break;
                            }
                        }
                        if (ctlType == "Html") {
                            var fobj = {};
                            for (var idx = 0; idx < ItemDataList.FormItemDefinitionList.length; idx++) {
                                if (arrlist[i].FName == ItemDataList.FormItemDefinitionList[idx].Key) {
                                    fobj = ItemDataList.FormItemDefinitionList[idx];
                                }
                            }
                            ItemDataList.GenerDataLists(fobj, FormDataSet, state);
                        }
                        else {
                            var fiid = "";
                            for (var idx = 0; idx < AutoFull.FormItemDefinitionList.length; idx++) {
                                if (arrlist[i].FName == AutoFull.FormItemDefinitionList[idx].Key) {
                                    fiid = AutoFull.FormItemDefinitionList[idx].ID;
                                }
                            }
                            getData(fiid, arrlist[i].FName, ctlType, FormDataSet, state);
                        }
                    }
                }
                for (var j = 0; j < ChildrenList.length; j++) {
                    var fid = $(ChildrenList[j]).attr("id");
                    //绑定 下拉框
                    if ($(fid).is("select") || $(fid).attr("type") == "checkbox" || $(fid).attr("type") == "radio")
                        for (var k = 0, icount = __selectionItems__.length; k < icount; k++) {
                            if (__selectionItems__[k].name == fid) {
                                $("#" + fid).attr("value", __selectionItems__[k].value);
                                break;
                            }
                        }
                }

            }
        }
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
        //return "";
    };
    resultStr();
}
//***************************end*****************************************************//

//**************************加载字段规则*****************************//
function GenerItemsRules(id, key, type, state) {
    var resultStr = function() {
        //if (state == 0 || $("#bwdf_txt_" + key).attr("contentEditable") == "true")
        Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.GenerItemsRules(id, successHandler, errorHandler, timeoutHandler);
    };
    var successHandler = function(result) {
        if (result != "") {
            var json = "";
            try {
                json = eval(result);
            }
            catch (e) { }
            if (json != "") {
                var arrlist = new Array();
                var templist = [];
                for (var i = 0; i < json.length; i++) {//遍历json
                    var obj = json[i];

                    if (obj.ParentName == key) {
                        arrlist.push({ FName: obj.ParentName, FValue: obj.ParentValue, CName: obj.CName, CType: obj.CType, CitemType: obj.CitemType, IdName: obj.IdName, IdType: obj.IdType });
                        switch (type) {
                            case "Text":
                                if ($("#bwdf_txt_" + obj.ParentName).val() == obj.ParentValue) {
                                    GenerItems(obj,state);
                                }
                                break;
                            case "TextArea":
                                if ($("#bwdf_txa_" + obj.ParentName).val() == obj.ParentValue) {
                                    GenerItems(obj, state);
                                }
                                break;
                            case "Date":
                                if ($("#bwdf_dat_" + obj.ParentName).val() == obj.ParentValue) {
                                    GenerItems(obj, state);
                                }
                                break;
                            case "DropDownList":
                                if ($("#bwdf_ddl_" + obj.ParentName).val() == obj.ParentValue) {
                                    GenerItems(obj, state);
                                }

                                break;
                            case "RadioButtonList":
                                if ($("input[name='bwdf_rbl_" + obj.ParentName + "'][@checked]").val() == obj.ParentValue) {
                                    GenerItems(obj, state);
                                }
                                break;
                            case "CheckBoxList":
                                $("input[name='bwdf_cbl_" + obj.ParentName + "']").each(function() {
                                    if ($(this).attr("checked") && $(this).val() == obj.ParentValue)
                                        GenerItems(obj, state);
                                });

                                break;
                        }
                    }
                    templist = arrlist;
                }
                switch (type) {
                    case "DropDownList":
                        if (state == 0 || $("#bwdf_ddl_" + key).attr("contentEditable") == "true") {
                            $("#bwdf_ddl_" + key).change(function() {
                                //alert($("#bwdf_ddl_" + key).val());
                                for (var i = 0; i < templist.length; i++) {
                                    var parentValue = templist[i].FValue;
                                    if ($(this).val() == parentValue && key == templist[i].FName) {
                                        GenerItems(templist[i], state);
                                    }
                                }
                            });
                        }
                        break;
                    case "CheckBoxList":
                        $("input[name='bwdf_cbl_" + key + "']").click(function() {
                            var radId = $(this).attr("id");
                            for (var i = 0; i < templist.length; i++) {
                                var parentValue = templist[i].FValue;
                                if ($(this).attr("checked") && key == templist[i].FName && $(this).val() == parentValue) {
                                    GenerItems(templist[i], state);
                                }
                                else if (!$(this).attr("checked") && key == templist[i].FName && $(this).val() == parentValue) {
                                    var ctype = templist[i].CType;
                                    var idtype = templist[i].IdType;
                                    if (ctype == "show") {
                                        templist[i].CType = "hide";                                       
                                    }
                                    else if (ctype == "hide") {
                                        templist[i].CType = "show";
                                    }
                                    if (idtype == "show") {
                                        templist[i].IdType = "hide";
                                    }
                                    else if (idtype == "hide") {
                                        templist[i].IdType = "show"; 
                                    }
                                    GenerItems(templist[i], state);
                                    templist[i].CType = ctype;
                                    templist[i].IdType = idtype;
                                }
                            }
                        });
                        break;
                    case "RadioButtonList":
                        $("input[name='bwdf_rbl_" + key + "']").click(function() {
                            for (var i = 0; i < templist.length; i++) {
                                var parentValue = templist[i].FValue;
                                if ($(this).val() == parentValue && key == templist[i].FName && $(this).attr("checked")) {
                                    GenerItems(templist[i], state);
                                }
                            }
                        });
                        break;
                }
            }
        }
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
        //return "";
    };
    resultStr();
}

//加载元素 显示/隐藏
function GenerItems(obj, state)
{
    if (obj.CName != "") {
        switch (obj.CType) {
            case "show":
                if (obj.CitemType == "RadioButtonList" || obj.CitemType == "CheckBoxList") {
                    $("input[name=bwdf_rbl_" + obj.CName + "]").each(function() {
                        if ($(this).attr("checked")) {
                            //$(this).show();
                            $(this).next("span").show();
                        }
                    });
                    $("#spContent_bwdf_rbl_" + obj.CName).show();
                    $("#rbl_list_" + obj.CName).show();
                    $("input[name=bwdf_cbl_" + obj.CName + "]").each(function() {
                        if ($(this).attr("checked")) {
                            //$(this).show();
                            $(this).next("span").show();
                        }
                    });
                    $("#spContent_bwdf_cbl_" + obj.CName).show();
                    $("#cbl_list_" + obj.CName).show();
                }
                else {
                    switch (obj.CitemType) {
                        case "Text":
                            if (state == 0 || $("#bwdf_txt_" + obj.CName).attr("contentEditable") == "true")
                                $("#bwdf_txt_" + obj.CName).show();
                            else
                                $("#spContent_bwdf_txt_" + obj.CName).show();
                            break;
                        case "TextArea":
                            //if (state == 0 || $("#bwdf_txa_" + obj.CName).attr("contentEditable") == "true")
                            $("#bwdf_txa_" + obj.CName).show();
                            break;
                        case "Date":
                            if (state == 0 || $("#bwdf_dat_" + obj.CName).attr("contentEditable") == "true")
                                $("#bwdf_dat_" + obj.CName).show();
                            else
                                $("#spContent_bwdf_dat_" + obj.CName).show();
                            break;
                        case "DropDownList":
                            if (state == 0 || $("#bwdf_ddl_" + obj.CName).attr("contentEditable") == "true")
                                $("#bwdf_ddl_" + obj.CName).show();
                            else
                                $("#spContent_bwdf_ddl_" + obj.CName).show();
                            break;
                    }
                }
                break;
            case "hide":
                if (obj.CitemType == "RadioButtonList" || obj.CitemType == "CheckBoxList") {
                    $("input[name=bwdf_rbl_" + obj.CName + "]").each(function() {
                        $(this).hide("fast");
                        $(this).next("span").hide("fast");
                    });
                    $("#spContent_bwdf_rbl_" + obj.CName).hide();
                    $("#rbl_list_" + obj.CName).hide();
                    $("input[name=bwdf_cbl_" + obj.CName + "]").each(function() {
                        $(this).hide("fast");
                        $(this).next("span").hide("fast");
                    });
                    $("#spContent_bwdf_cbl_" + obj.CName).hide();
                    $("#cbl_list_" + obj.CName).hide();
                }
                else {
                    switch (obj.CitemType) {
                        case "Text":
                            if (state == 0 || $("#bwdf_txt_" + obj.CName).attr("contentEditable") == "true")
                                $("#bwdf_txt_" + obj.CName).hide();
                            else
                                $("#spContent_bwdf_txt_" + obj.CName).hide();
                            break;
                        case "TextArea":
                            //if (state == 0 || $("#bwdf_txa_" + obj.CName).attr("contentEditable") == "true")
                            $("#bwdf_txa_" + obj.CName).hide();
                            break;
                        case "Date":
                            if (state == 0 || $("#bwdf_dat_" + obj.CName).attr("contentEditable") == "true")
                                $("#bwdf_dat_" + obj.CName).hide();
                            else
                                $("#spContent_bwdf_dat_" + obj.CName).hide();
                            break;
                        case "DropDownList":
                            if (state == 0 || $("#bwdf_ddl_" + obj.CName).attr("contentEditable") == "true")
                                $("#bwdf_ddl_" + obj.CName).hide();
                            else
                                $("#spContent_bwdf_ddl_" + obj.CName).hide();
                            break;
                    }
                }
                break;
        } 
    }
    if (obj.IdName != "") {
        switch (obj.IdType) {
            case "show":
                $("#" + obj.IdName).show();
                break;
            case "hide":
                $("#" + obj.IdName).hide();
                break;
        }
    }                       
}
//***************************end*****************************************************//

//**************************绑定数据*********************************//
function AddDataSet(key, type, value, state) {
    var msg;
    var required;
    var validateType;
    var n;
    switch (type) {
        case "Text":
            if (state == 0 || $("#bwdf_txt_" + key).attr("contentEditable") == "true")
                $("#bwdf_txt_" + key).attr("value",value);
            break;
        case "TextArea":
            if (state == 0 || $("#bwdf_txa_" + key).attr("contentEditable") == "true")
                $("#bwdf_txa_" + key).attr("value", value);
            break;
        case "Date":
            if (state == 0 || $("#bwdf_dat_" + key).attr("contentEditable") == "true")
                $("#bwdf_dat_" + key).attr("value", value);
            break;
        case "DropDownList":
            if (value.length > 0) {
                document.getElementById("bwdf_ddl_" + key).innerHTML = "";
                document.getElementById("bwdf_ddl_" + key).options.length = 0;
                for (var i = 0; i < value.length; i++) {

                    var varItem = new Option(value[i].Value, value[i].Value);
                    //       objSelect.options[objSelect.options.length] = varItem;
                    document.getElementById("bwdf_ddl_" + key).options.add(varItem);

                }
                //绑定 下拉框
                for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {
                    //debugger;
                    bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
                    //alert($("#bwdf_ddl_" + key).attr("contentEditable"));
                    if (__selectionItems__[i].name == "bwdf_ddl_" + key && $(this).attr('contentEditable') != 'true' && state != 0) {
                        //alert($("#bwdf_ddl_" + key).attr("contentEditable"));
                        $("#" + __selectionItems__[i].name).attr("value", __selectionItems__[i].value);
                        $("#" + __selectionItems__[i].name).hide('fast');
                        //$("#" + __selectionItems__[i].name).after(__selectionItems__[i].value);
                        $("#spContent_bwdf_ddl_" + key).attr("value", __selectionItems__[i].value);
                        
                    }
                } 
            }
            break;
        case "CheckBoxList":
            if (value.length > 0) {
                n = $("input[name='bwdf_cbl_" + key + "']").length - 1;
                //alert($("#bwdf_cbl_" + key + "_" + n).attr("value"));
                msg = $("#bwdf_cbl_" + key + "_" + n).attr("msg");
                validateType = $("#bwdf_cbl_" + key + "_" + n).attr("ValidateType");
                required = $("#bwdf_cbl_" + key + "_" + n).attr("require");
                var chkindex = 0;
                $("#cbl_list_" + key).html("");
                for (var i = 0; i < value.length; i++) {
                    if (chkindex == value.length - 1) {
                        $("#cbl_list_" + key).append("<input type=\"checkbox\" id=\"bwdf_cbl_" + key + "_" + chkindex + "\" name=\"bwdf_cbl_" + key + "\" value=\"" + value[i].Value + "\" /><span>" + value[i].Value + "</span>");
                        if (validateType != "undefined")
                            $("#bwdf_cbl_" + key + "_" + chkindex + "").attr("ValidateType", validateType);
                        if (required != "undefined")
                            $("#bwdf_cbl_" + key + "_" + chkindex + "").attr("require", required);
                        if (msg != "undefined")
                            $("#bwdf_cbl_" + key + "_" + chkindex + "").attr("msg", msg);
                    }
                    else
                        $("#cbl_list_" + key).append("<input type=\"checkbox\" id=\"bwdf_cbl_" + key + "_" + chkindex + "\" name=\"bwdf_cbl_" + key + "\" value=\"" + value[i].Value + "\" /><span>" + value[i].Value + "</span>");
                    chkindex++;
                }
                //绑定 单选、多选
                for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {
                    //debugger;
                    bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
                    //alert($("#bwdf_ddl_" + key).attr("contentEditable"));
                    if (__selectionItems__[i].name == "bwdf_cbl_" + key && $(this).attr('contentEditable') != 'true' && state != 0) {
                        $("input[name='" + __selectionItems__[i].name + "']").each(function() {
                            if ($(this).attr('checked')) {
                                //$(this).after($(this).val());
                                $(this).hide("fast");
                                $(this).next('<span>').hide();
                                $(this).after("<span id='spContent_" + $(this).attr('name') + "'>" + $(this).val() + "</span>");
                                $(this).click(function() { return false; });
                            }
                            else {
                                $(this).hide();
                                $(this).next('<span>').hide();
                            }
                        });
                    }
                }
            }
            break;
        case "RadioButtonList":
            if (value.length > 0) {
                n = $("input[name='bwdf_rbl_" + key + "']").length - 1;
                msg = $("#bwdf_rbl_" + key + "_" + n).attr("msg");
                validateType = $("#bwdf_rbl_" + key + "_" + n).attr("ValidateType");
                required = $("#bwdf_rbl_" + key + "_" + n).attr("require");
                var rblindex = 0;
                $("#rbl_list_" + key).html("");
                for (var i = 0; i < value.length; i++) {
                    if (chkindex == value.length - 1) {
                        $("#rbl_list_" + key).append("<input type=\"radio\" id=\"bwdf_rbl_" + key + "_" + chkindex + "\" name=\"bwdf_rbl_" + key + "\" value=\"" + value[i].Value + "\" /><span>" + value[i].Value + "</span>");
                        if (validateType != "undefined")
                            $("#bwdf_rbl_" + key + "_" + chkindex + "").attr("ValidateType", validateType);
                        if (required != "undefined")
                            $("#bwdf_rbl_" + key + "_" + chkindex + "").attr("require", required);
                        if (msg != "undefined")
                            $("#bwdf_rbl_" + key + "_" + chkindex + "").attr("msg", msg);
                    }
                    else
                        $("#rbl_list_" + key).append("<input type=\"radio\" id=\"bwdf_rbl_" + key + "_" + chkindex + "\" name=\"bwdf_rbl_" + key + "\" value=\"" + value[i].Value + "\" /><span>" + value[i].Value + "</span>");
                    rblindex++;
                }
                //绑定单选
                for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {
                    //debugger;
                    bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
                    //alert($("#bwdf_ddl_" + key).attr("contentEditable"));
                    if (__selectionItems__[i].name == "bwdf_rbl_" + key && $(this).attr('contentEditable') != 'true' && state != 0) {
                        $("input[name='" + __selectionItems__[i].name + "']").each(function() {
                            if ($(this).attr('checked')) {
                                //$(this).after($(this).val());
                                $(this).hide("fast");
                                $(this).next('<span>').hide();
                                $(this).after("<span id='spContent_" + $(this).attr('name') + "'>" + $(this).val() + "</span>");
                                $(this).click(function() { return false; });
                            }
                            else {
                                $(this).hide();
                                $(this).next('<span>').hide();
                            }
                        });
                    }
                }
            }
            break;
        default:
            break;
    }
}
//end***********************************************************************//

//***************************begin*****************************************************//
//参数解析
function getFormDataSet(wfname, wiid, title, sheetid, curuser, dpid, aiid, actname) {
    var arr = new Array();
    arr.push("#WorkflowName#$" + wfname);
    arr.push("#Wiid#$" + wiid);
    arr.push("#Title#$" + title);
    arr.push("#SheetId#$" + sheetid);
    arr.push("#CurrentUser#$" + curuser);
    arr.push("#DpId#$" + dpid);
    arr.push("#Aiid#$" + aiid);
    arr.push("#ActivityName#$" + actname);
    $('#ctl00_cphBody_divDynamicFormContainer input,#ctl00_cphBody_divDynamicFormContainer textarea').each(function () {
        var inputType = $(this).attr('type');
        if (inputType == 'checkbox' || inputType == 'radio') {
            //if ($(this).attr('checked')) {
                //arr.push("#" + $(this).attr("name").split('_')[2] + "#$" + $(this).val());
            //}
            for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {//用数据库值绑定，防止控件还未绑定而无法取值的情况
                if ($(this).attr("name") == __selectionItems__[i].name) {
                    arr.push("#" + $(this).attr("name").split('_')[2] + "#$" + __selectionItems__[i].value);
                }
            }
        }
        else {
            if (inputType == 'button') return;
            //arr.push("#" + $(this).attr("id").split('_')[2] + "#$" + $(this).val());
            //bwwf_txt_Fxx  bwwf_txa_Fxx bwwd_dat_Fxx
            if ($(this).attr("name").length > 9) {
                arr.push("#" + $(this).attr("name").substring(9) + "#$" + $(this).val());
            }
        }
    });
    $('#ctl00_cphBody_divDynamicFormContainer select').each(function () {
        //arr.push("#" + $(this).attr("id").split('_')[2] + "#$" + $(this).val());
        for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {//用数据库值绑定，防止控件还未绑定而无法取值的情况
            if ($(this).attr("name") == __selectionItems__[i].name) {
                arr.push("#" + $(this).attr("name").substring(9) + "#$" + __selectionItems__[i].value);
            }
        }
    });
    return arr;
}

//绑定表单项
function BindDynamicFormItems(state,divId) {
    //绑定下拉、单选、多选

    for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {
        bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
    }

    //工单处理时默认表单项为只读,除了已设置为可编辑的表单项

    $('#' + divId + ' input').each(function() {
        var inputType = $(this).attr('type');
        if ($(this).attr('contentEditable') == 'true' || inputType == 'hidden' || state == 0) {
            if (window.location.href.toLowerCase().indexOf("process.aspx")!=-1)
                return;
            else if (window.location.href.toLowerCase().indexOf("start.aspx") != -1)
                return;
        }
        if (inputType == 'checkbox' || inputType == 'radio') {
            if ($(this).attr('checked')) {
                $(this).hide("fast");
                $(this).next('span').hide();
                $(this).after("<span id='spContent_" + $(this).attr('name') + "'>" + $(this).next("span").html() + "</span>");
                $(this).click(function() { return false; });
            }
            else {
                $(this).hide();
                $(this).next('span').hide();
            }
        }
        else {
            $(this).hide();
            if (inputType == 'button') return;
            $(this).after("<span id='spContent_" + $(this).attr('id') + "'>" + $(this).val() + "</span>");
            //$(this).attr('readonly', 'readonly');
            //$(this).attr('style', 'width:auto;overflow:auto;border:0;background:#FFF; ');
            if ($(this).next('.ico_pickdate'))
                $(this).next('.ico_pickdate').hide();
        }
    });

    $('#' + divId + ' textarea').each(function() {
        if ($(this).attr('contentEditable') == 'true' || state == 0) {
            if (window.location.href.toLowerCase().indexOf("process.aspx") != -1)
                return;
            else if (window.location.href.toLowerCase().indexOf("start.aspx") != -1)
                return;
        }
        $(this).attr('readonly', 'readonly');
        $(this).attr('style', 'width:90%;overflow:auto;border:0;background:#FFF; ');
        // $(this).mouseover(function(){adjustTextArea(this);});
        adjustTextArea(this);
    });

    $('#' + divId + ' select').each(function() {
        if ($(this).attr('contentEditable') == 'true' || state == 0) {
            if (window.location.href.toLowerCase().indexOf("process.aspx") != -1)
                return;
            else if (window.location.href.toLowerCase().indexOf("start.aspx") != -1)
                return;
        }
        $(this).hide('fast');
        //$(this).after($(this).val());
        $(this).after("<span id='spContent_" + $(this).attr('id') + "'>" + $(this).val() + "</span>");
    });
}

//***************************end*****************************************************//

/**
* 同步加载js脚本
* @param id   需要设置的<script>标签的id
* @param url   js文件的相对路径或绝对路径
* @return {Boolean}   返回是否加载成功，true代表成功，false代表失败
*/
function loadJS(id, url) {
    var xmlHttp = null;
    if (window.ActiveXObject)//IE
    {
        try {
            //IE6以及以后版本中可以使用
            xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
            //IE5.5以及以后版本可以使用
            xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
    }
    else if (window.XMLHttpRequest)//Firefox，Opera 8.0+，Safari，Chrome
    {
        xmlHttp = new XMLHttpRequest();
    }
    //采用同步加载
    xmlHttp.open("GET", url, false);
    //发送同步请求，如果浏览器为Chrome或Opera，必须发布后才能运行，不然会报错
    xmlHttp.send(null);
    //4代表数据发送完毕
    if (xmlHttp.readyState == 4) {
        //0为访问的本地，200到300代表访问服务器成功，304代表没做修改访问的是缓存
        if ((xmlHttp.status >= 200 && xmlHttp.status < 300) || xmlHttp.status == 0 || xmlHttp.status == 304) {
            var myHead = document.getElementsByTagName("HEAD").item(0);
            var myScript = document.createElement("script");
            myScript.language = "javascript";
            myScript.type = "text/javascript";
            myScript.id = id;
            try {
                //IE8以及以下不支持这种方式，需要通过text属性来设置
                myScript.appendChild(document.createTextNode(xmlHttp.responseText));
            }
            catch (ex) {
                myScript.text = xmlHttp.responseText;
            }
            myHead.appendChild(myScript);
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
}
//***************************end*****************************************************//

//这个方法做了一些操作、然后调用回调函数
//code:方法名
//args:参数集合 
var X2 = {}    //my namespace:)
X2.Eval = function(code, args) {
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