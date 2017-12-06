/// <reference path="../../Scripts/jquery-1.4.1.min.js" />

var cao_x, cao_y, move = 0; var dataArray = new Array(); var times = 0; var durl = ""; var fobj; var tarId = ""; var flag = ""; var selectTyep = 0; var tableName = ""; dpid = ""; var id = ""; var tableName2 = ""; var isorganization = "";var issimple;
var IsOrganizationalControl = true;
//获取控件对像id
function get_id(obj_sel) { return document.getElementById(obj_sel); }

//实现层的拖移
function caoMove(obj) {
    if (move == 1) {
        var caoX = obj.clientLeft;
        var caoY = obj.clientTop;
        obj.style.pixelLeft = caoX + (event.x - cao_x);
        obj.style.pixelTop = caoY + (event.y - cao_y);
        if (obj.style.pixelTop > 250) {
            obj.style.pixelTop = 230;
        }
        if (obj.style.pixelTop < -45) {
            obj.style.pixelTop = -42;
        }
        if (obj.style.pixelLeft < -275) {
            obj.style.pixelLeft = -270;
        }
        if (obj.style.pixelLeft > 485) {
            obj.style.pixelLeft = 480;
        }
    }
}

//实现层的相对居中定位
function heartBeat() {
    diffY = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
    percent = .1 * (diffY - lastScrollY);
    if (percent > 0) percent = Math.ceil(percent);
    else percent = Math.floor(percent);
    document.all.div_show.style.pixelTop += percent;
    lastScrollY = lastScrollY + percent;
}

//关闭弹出框
function closeDiv() {
    IntervalId = window.clearInterval(IntervalId);
    times = 0;
    total = 200;
    $("#span_total").text("1~" + total);
    $("#a_getMore").show();
    $(".group_add").show();
    get_id('div_show').style.display = 'none';
    //  $("input[name='textfield']").removeAttr("readonly");
    $("#span_edit").show();

}
//addDivFriendly = "<div id=\"show_form\" style=\"float: right;\"><div id=\"div_show\" class=\"div_1\" style=\"left:40%;top:200px; margin:-150px 0 0 -200px;background-color:Blue\"  ><div id=\"pixelTop\" style=\"position:absolute;  solid #000033;border: 2px solid #597D98;\"  > <table  width=\"400px\" bgcolor=\"White\"   onmousedown=\"move=1;cao_x=event.x-parentNode.style.pixelLeft;cao_y=event.y-parentNode.style.pixelTop;setCapture();\" onmouseup=\"move=0;releaseCapture();\" onmousemove=\"caoMove(this.parentNode)\" style=\"cursor:move;\"><tr align=\"center \" bgcolor=\"#597D98\"><td align=\"center\"colspan=\"4\" style=\"color: #FFFFFF ;\"><span id='title'></span></td><td style=\"width:5px\" ><a href=\"JavaScript:void(0);\" onclick=\"closeDiv();\"><img src=\"/upload/shut.png\" alt=\"点击可以关闭\" /></a></td></tr></table><table width='400px' bgcolor='White'><tr bgcolor='White'><td class='td_border' style='width: 20%'>上传图片：</td><td class='td_border' style='width: 80%;' align='left'><span id=\"myspan\"><iframe src=\"\" id=\"ifUpload1\" frameborder=\"no\" scrolling=\"no\" style=\"width:300px; height:28px;\"></iframe></span><br /><div><img id='pmg' style='width: 180px; height: 180px; margin-top: 5px;margin-left: 5px;'></div><div id=\"font\" style='float: left; color: #FF0000;'></div></td></tr><tr bgcolor='White'><td class='td_border' style='width: 20%'>链接名称：</td><td class='td_border' style='width: 80%;' align='left'><input id='mytitle' type='text' style='width:95%' /></td></tr><tr bgcolor='White'><td class='td_border' style='width: 20%'>链接地址：</td><td class='td_border' style='width: 80%;' align='left'><input id='href' type='text' style='width:95%' /></td></tr><tr bgcolor='White'><td class='td_border' style='width: 20%'>排列序号：</td><td class='td_border' style='width: 80%;' align='left'><input id='myorder' type='text' style='width:95%' /></td></tr><tr bgcolor='White'><td colspan='2' style='height:35px' bgcolor='#597D98'><input id='btn_OK' type='button' value='确认提交' class='btn' onclick='return addpic();' /> </td></tr></table></div></div></div>";

addDivFriendly = '<div id=\"show_form\" style=\"float: right;\"><div id=\"div_show\" class=\"div_1\" style=\"left:40%;top:200px; margin:-150px 0 0 -200px;background-color:Blue\"  ><div id=\"pixelTop\" style=\"position:absolute;  solid #000033;border: 2px solid #597D98;\"  > <div   style=\"height:15px;width:530px ;background-color:#597D98\"   onmousedown=\"move=1;cao_x=event.x-parentNode.style.pixelLeft;cao_y=event.y-parentNode.style.pixelTop;setCapture();\" onmouseup=\"move=0;releaseCapture();\" onmousemove=\"caoMove(this.parentNode)\" style=\"cursor:move;\"><div style=\"float:right;\"><a href=\"JavaScript:void(0);\" onclick=\"closeDiv();\"><img src=\"../../../App_Themes/gmcc/img/close_1.jpg\" alt=\"点击可以关闭\" /></a></div></div><div class="select_box" style=\"background-color:#FFFFFF;"><div class="select_item"><div id="find_item"><input name="textfield" type="text" class="inputbox" size="20" /><a href="javascript:void(0)" style="color:blue" title="搜索" id="friend_a_serch" onclick="serch_item(\'a\')">搜索</a><span id="span_total">1~200</span><a href=\"JavaScript:void(0);\" id="a_getMore" title="增加显示条数" style="text-decoration:none" onclick=\"getMore();\">更多</a></div><ul class="item_list" id="item_select"></ul></div><div class="selected_item"><div class="selected_title"><a href="#" id="selected_clear" class="selected_clear">清空</a><span id="span_edit"><a href=\"JavaScript:void(0);\" title=\"导入\" class="selected_clear" onclick=\"firend_show(1)\">导入</a><a href=\"JavaScript:void(0);\" title=\"编辑\" class="selected_clear" onclick=\"firend_show(2)\">编辑</a></span>已选择</div><ul class="item_list" id="selected_item"></ul></div><div class="clear"></div><div style="text-align:center"><input type="button" value="确定" onclick=\"getItem();\"><input style="margin-left:50px !important" type="button" value="取消" onclick=\"closeDiv();\"></div></div></div>';
var first = 0;
function firend_show(type) {
    var rValue = window.showModalDialog("Friend_ShowDialog.aspx?iii=" + 99999999 * Math.random(), type, "dialogHeight: 300px; dialogWidth: 520px; center: Yes; help: no; resizable: no; status: no;");
    if (rValue) {
        //        var str = rValue.split(',');
        //        for (var i = 0; i < str.length; i++) {
        //            $("<li><div class='item_name' rel=" + str[i] + "><div class='items' ></div><div class='item_del' onclick='$(this).parent().remove();' title='取消选择'></div>" + str[i] + "</div></li>").appendTo($("#selected_item"));
        //        }
        var html = $("#selected_item").html();
        $("#selected_item").html(html + rValue);
    }
}
//实现层弹出框
function showDiv(field, dataUrl, tid) {
    durl = dataUrl;
    fobj = field;
    tarId = tid;
    field.index = times;
    issimple = field.issimple; //是否单选
    if (field.tableName) { tableName = field.tableName; }
    if (field.isorganization) { isorganization = field.isorganization; }
    if (field.isSelectOne) { selectTyep = 1; $(".group_add").hide(); }
    if (field.fieldWhere) {
        if (field.fieldWhere.length > 0) {
            var b = new Base64();
            field.fieldWhere = b.encode(field.fieldWhere);
        }
    }
    var str = "";
    if (typeof (tid) == "string") {
        str = tid;
    }
    else {
        str = tid.hide;
    }

    if (flag != str && get_id('div_show') != null) {
        dataArray = new Array();
        selected_clear();
        clearItem();
        first = 0;
        if (!field.data) {
            loadData(field, dataUrl);
        } else {
            $("#a_getMore").hide();
            insetTodivByData(field, field.data, dataUrl);
            friend_init(field.data);
        }
    }
    if (get_id('div_show') == null) {

        $("body").append(addDivFriendly);
        //        if ($.browser.version == "6.0") {
        //            alert($(".selected_title").css("height"));
        //            $(".selected_title").css("height", "27px");
        //        }
        $("#find_item .inputbox").keyup(serch_item);
        IntervalId = window.setInterval("heartBeat()", 1);
        if (!field.data) {
            loadData(field, dataUrl);
        }
        else {
            $("#a_getMore").hide();
            insetTodivByData(field, field.data, dataUrl);
            friend_init(field.data);
        }
    }
    if (field.ishide) {
        //   $("input[name='textfield']").attr("readonly", "readonly");
        $("#span_edit").hide();
    }

    get_id('div_show').style.display = 'block';
    lastScrollY = 0;
    if (typeof (tid) == "string") {
        flag = tid;
    } else {
        flag = tid.hide;
    }

}
//加载数据
function loadData(obj, dataUrl) {
    sendAjax(obj, dataUrl);
}
function friend_init(data) {
    
    init();
    $("#item_select").find("ul").slideToggle("fast");
    $("#item_select").find(".item_close").toggleClass("item_open");
    if (first == 0) {
        item_select_html = $("#item_select").html()
    }
    if (data.length == 0 && dataArray.length == 0) {
        str = "<div style='font-size:14px;height:30px;line-height:30px;padding-left:10px;color:#999;'>抱歉，谁也没找到</div>";
        $("#item_select").html(str);
    }
    first = 1;
}
function sendAjax(obj, dataUrl) {
    $.post(dataUrl + "?tableName=" + tableName + "&dpid=" + dpid + "&id=" + id + "&IsOrganizationalControl=" + IsOrganizationalControl, obj, function (data) {
        //   data = eval("(" + data + ")");
        dataArray.push(data);
        insetToDiv(obj, data);

        friend_init(data);
    })
}
function insetToDiv(obj, data, istree) {
    clearItem();
    if (!istree) {
        // for (var i = 0; i < data.length; i++)
        {
            insetToDivNotree(obj, data);
        }
    }

}
var total = 200;
function getMore() {
    times++;
    fobj.index = times;
    total = total + 200;
    $("#span_total").text("1~" + total);
    loadData(fobj, durl);

}
var item_select_html
var friend_ul
function insetToDivNotree(obj, data) {

    if (data) {

        if (!$("#friend_ul").get(0)) {
            var li = $("<li>请选择</li>").appendTo($("#item_select"));
            friend_ul = $("<ul id='friend_ul'></ul>").appendTo(li);
        }
        $("#friend_ul").html($("#friend_ul").html() + data);

        //        for (var i = 0; i < data.length; i++) {
        //            var hid;
        //            if (obj.value) {
        //                hid = eval("data[i]." + obj.value);
        //            }
        //            else {
        //                hid = eval("data[i]." + obj.text);
        //            }
        //            var text = eval("data[i]." + obj.text);
        //            $("<li hid='" + hid + "'>" + text + "</li>").appendTo($("#friend_ul"));
        //        }

    }
}
function insetTodivByData(obj, data) {
    if (data) {
        if (!$("#friend_ul").get(0)) {
            var li = $("<li>请选择</li>").appendTo($("#item_select"));
            friend_ul = $("<ul id='friend_ul'></ul>").appendTo(li);
        }
        for (var i = 0; i < data.length; i++) {
            var hid;
            if (obj.value) {
                hid = eval("data[i]." + obj.value);
            }
            else {
                hid = eval("data[i]." + obj.text);
            }
            var text = eval("data[i]." + obj.text);
            $("<li hid='" + hid + "'>" + text + "</li>").appendTo($("#friend_ul"));
        }
    }
}

function clearItem() {
    $("#item_select").html("")
}
function getItem() {
    var items = $("#selected_item").find(".item_name");
    if (selectTyep == 1) {
        if (typeof (tarId) == "string") {
            $("#" + tarId).val(items.first().attr("rel")); closeDiv(); return false;
        }
        else {
            $("#" + tarId.hide).val(items.first().attr("rel"));
            $("#" + tarId.text).val(items.first().text()); closeDiv(); return false;
        }
    }
    var str = ""; var str2 = "";
    for (var i = 0; i < items.length; i++) {
        str += "'" + $(items[i]).attr("rel") + "'";
        str2 += $(items[i]).text();
        if (i != items.length - 1) {
            str += ",";
            str2 += ",";
        }
    }
    if (typeof (tarId) == "string") {
        $("#" + tarId).val(str);
    } else {
        $("#" + tarId.hide).val(str);
        $("#" + tarId.text).val(str2);
    }

    if (typeof (onfrined_close) == "function") {

        onfrined_close();

    }

    closeDiv();
}
var oldval = "";
function serch_item(e) {

    if (event.keyCode != 13 && e != "a") { return false; }


    var finditem = $("input[name='textfield']").attr("value").toLowerCase();
    if (fobj.data) { return true; }
    if (finditem != "") {
        $("#find_clear").css("visibility", "visible");
        var str = "";
        fobj.strWhere = finditem;
        dataArray = new Array();
        loadData(fobj, durl);
        allclickable();
    } else {
        $("#find_clear").css("visibility", "visible");
        var str = "";
        fobj.strWhere = finditem;
        dataArray = new Array();
        loadData(fobj, durl);
        allclickable();

        //   allclickable();
    }
}





