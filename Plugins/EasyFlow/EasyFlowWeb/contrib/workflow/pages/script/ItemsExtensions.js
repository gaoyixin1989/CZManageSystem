
var ItemIframe = {}    //my namespace:)
ItemIframe.LoadIframe = function(wid, wiid, actname, dataSet) {//加载IFrame集合
    var resultStr = function() {
        // 调用 Ajax Web Service
        if (wid != "") {
            Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.GetIFramesSetting(wid, wiid, successHandler, errorHandler, timeoutHandler);
        }
    };
    var successHandler = function(result) {
        if (result != "") {
            var json = "";
            try {
                json = eval("[" + result + "]");
            }
            catch (e) { }
            //alert(result)
            if (json != "") {
                var arrlist = new Array();
                var actlist = new Array();

                if (arrlist.length == 0) {
                    for (var i = 0; i < json.length; i++) {
                        var obj = json[i];
                        if (obj.SettingType == "0") {
                            arrlist.push({ Key: obj.Key, ID: obj.ID, Type: obj.Type, SettingType: obj.SettingType, ActivityName: obj.ActivityName, Height: obj.Height, Width: obj.Width });
                        }
                    }
                }
                for (var i = 0; i < json.length; i++) {
                    var obj = json[i];
                    for (var j = 0; j < arrlist.length; j++) {
                        if (obj.ActivityName == actname && obj.Key == arrlist[j].Key) {//如果步骤有定义IFrame，则只嵌入步骤表单
                            var keylist = new Array();
                            keylist.push({ Key: obj.Key, ID: obj.ID, Type: obj.Type, SettingType: obj.SettingType, ActivityName: obj.ActivityName, Height: obj.Height, Width: obj.Width});
                            arrlist[j] = keylist[0];
                            //break;
                        }
                        else if (obj.SettingType != "0") {
                            actlist.push({ Key: obj.Key, ID: obj.ID, Type: obj.Type, SettingType: obj.SettingType, ActivityName: obj.ActivityName, Height: obj.Height, Width: obj.Width });
                        }
                    }
                }
                if (arrlist.length > 0) {
                    for (var i = 0; i < arrlist.length; i++) {
                        ItemIframe.GenerIFramesSetting(arrlist[i], dataSet);
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
    };
    resultStr();
}

ItemIframe.GenerIFramesSetting = function(obj, dataSet) {//加载IFrame
    var resultStr = function() {
        // 调用 Ajax Web Service
        if (obj.ID != "") {
            Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.GenerIFramesSetting(obj.ID, obj.ActivityName, obj.SettingType, dataSet, successHandler, errorHandler, timeoutHandler);
        }
    };
    var successHandler = function(result) {
        if (result != "") {
            var src = result;
            try {
                if (obj.Type == "Html") {
                    var height = obj.Height > "0" ? "'" + obj.Height + "px'" : "Iframe_" + obj.Key + ".document.body.scrollHeight";
                    var width = obj.Width == "0" ? "100%" : obj.Width + "px";
                    
                    $("#divHTMLContainer_" + obj.Key).append("<iframe id=\"Iframe_" + obj.Key + "\" src=\"" + src + "\" frameborder=\"0\" width=\"" + width + "\" scrolling=\"auto\" onload=\"this.height=" + height + "\"></iframe>");
                }
            }
            catch (e) { }
        }
    };
    // Ajax 超时.
    var timeoutHandler = function(result) {
        //BindDynamicFormItems(state);
        alert("Timeout :" + result);
        //return "";
    };
    // Ajax 错误.
    var errorHandler = function(result) {
        //BindDynamicFormItems(state);
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
}

var ItemDataList = {}    //my namespace:)
ItemDataList.FormItemDefinitionList = []; //表单设置集合
ItemDataList.LoadDataList = function (wid, wiid, dataSet, state) {//加载DataList集合
    var resultStr = function () {
        // 调用 Ajax Web Service
        if (wid != "") {
            Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.GetDataListSettings(wid, wiid, successHandler, errorHandler, timeoutHandler);
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
                    arrlist.push(obj);

                    ItemDataList.GenerDataLists(obj, dataSet, state);
                }
                ItemDataList.FormItemDefinitionList = arrlist;
            }
        }
    };
    // Ajax 超时.
    var timeoutHandler = function (result) {
        alert("Timeout :" + result);
        //return "";
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

ItemDataList.GenerDataLists = function (obj, dataSet, state) {//加载DataList
    var resultStr = function () {
        // 调用 Ajax Web Service
        if (obj.ID != "") {
            Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.GenerDataLists(obj.Wiid, obj.ID, obj.Key, obj.SettingType, obj.Columns, obj.Rows, dataSet, successHandler, errorHandler, timeoutHandler);
        }
    };
    var successHandler = function (result) {
        if (result != "") {
            var talbeStr = result;
            try {
                if (obj.Type == "Html") {
                    if(talbeStr!="")
                        $("#divHTMLContainer_" + obj.Key).html(talbeStr);
                    if ($("#bwdf_htm_" + obj.Key).attr('contentEditable') == 'true' || state == 0) { }
                    else {
                        ItemDataList.BindDynamicDataListItems("divHTMLContainer_" + obj.Key, state);
                        $("#divHTMLContainer_" + obj.Key + " a").hide();
                    }
                }
            }
            catch (e) { }
        }
    };
    // Ajax 超时.
    var timeoutHandler = function (result) {
        //BindDynamicFormItems(state);
        alert("Timeout :" + result);
        //return "";
    };
    // Ajax 错误.
    var errorHandler = function (result) {
        //BindDynamicFormItems(state);
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
}

/*
*  DataList添加行
*/
ItemDataList.AddRow = function (tbId, fdid, fname) {
    var resultStr = function () {
        var rows = $("#" + tbId + " tbody").children("tr").length;
        // 调用 Ajax Web Service
        Botwave.DynamicForm.Extension.WebServices.GetDataAjaxService.AddRow(fdid, fname, rows, successHandler, errorHandler, timeoutHandler);
    };
    var successHandler = function (result) {
        if (result != "") {
            var talbeStr = result;
            try {
                $("#" + tbId + " tbody").append(talbeStr);
                //绑定行数，用于动态行后台数据保存绑定
                var rowCnt = $("#" + tbId + " tbody").children("tr").length;
                $("#hid_DataList_rowcnt" + fname).val(rowCnt);
            }
            catch (e) { }
        }
    };
    // Ajax 超时.
    var timeoutHandler = function (result) {
        //BindDynamicFormItems(state);
        alert("Timeout :" + result);
        //return "";
    };
    // Ajax 错误.
    var errorHandler = function (result) {
        //BindDynamicFormItems(state);
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert("error" + msg);
    };
    resultStr();
}

/*
*  DataList删除行
*/
ItemDataList.DelRow = function (obj, tbId, fname) {
    obj.parentNode.parentNode.parentNode.removeChild(obj.parentNode.parentNode);
    //绑定行数，用于动态行后台数据保存绑定
    var rowCnt = $("#" + tbId + " tbody").children("tr").length;
    $("#hid_DataList_rowcnt" + fname).val(rowCnt);
    var tr = $("#" + tbId + " tbody").children("tr");
    for (var i = 0; i < rowCnt; i++) {//重新排列行号
        $(tr[i]).children("td").children("input").each(function () {
            var id = $(this).attr("id");
            var name = $(this).attr("name");
            var r = name.split('_')[4];
            $(this).attr("id", id.replace(r, "r" + i)).attr("name", name.replace(r, "r" + i))
        });
        $(tr[i]).children("td").children("select").each(function () {
            var id = $(this).attr("id");
            var name = $(this).attr("name");
            var r = name.split('_')[4];
            $(this).attr("id", id.replace(r, "r" + i)).attr("name", name.replace(r, "r" + i))
        });
        $(tr[i]).children("td").children("textarea").each(function () {
            var id = $(this).attr("id");
            var name = $(this).attr("name");
            var r = name.split('_')[4];
            $(this).attr("id", id.replace(r, "r" + i)).attr("name", name.replace(r, "r" + i))
        });
    }
}

//绑定DataList表单项
ItemDataList.BindDynamicDataListItems = function (divId, state) {
    //绑定下拉、单选、多选

    /*for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {
    bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
    }*/

    //工单处理时默认表单项为只读,除了已设置为可编辑的表单项

    $('#' + divId + ' input').each(function () {
        var inputType = $(this).attr('type');
        if ($(this).attr('contentEditable') == 'true' || inputType == 'hidden' || state == 0)
            return;
        if (inputType == 'checkbox' || inputType == 'radio') {
            if ($(this).attr('checked')) {
                $(this).after($(this).val());
                $(this).click(function () { return false; });
            }
            else $(this).hide();

            $(this).next('span').hide();
        }
        else {
            $(this).hide();
            if (inputType == 'button') return;
            //$(this).after($(this).val());
            //$(this).attr('readonly', 'readonly');
            //$(this).attr('style', 'width:auto;overflow:auto;border:0;background:#FFF; ');
            if ($(this).next('.ico_pickdate'))
                $(this).next('.ico_pickdate').hide();
            $(this).after("<span id='spContent_" + $(this).attr('id') + "'>" + $(this).val() + "</span>");
            
        }
    });

    $('#' + divId + ' textarea').each(function () {
        if ($(this).attr('contentEditable') == 'true' || state == 0)
            return;
        //$(this).attr('readonly', 'readonly');
        //$(this).attr('style', 'width:90%;overflow:auto;border:0;background:#FFF; ');
        // $(this).mouseover(function(){adjustTextArea(this);});
        //adjustTextArea(this);
        $(this).hide();
        $(this).after("<span id='spContent_" + $(this).attr('id') + "'>" + $(this).val() + "</span>");
    });

    $('#' + divId + ' select').each(function () {
        if ($(this).attr('contentEditable') == 'true' || state == 0)
            return;
        $(this).hide('fast');
        $(this).after($(this).val());
    });
}