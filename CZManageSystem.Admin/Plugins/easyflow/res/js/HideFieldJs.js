function hideField(arrary) {
    var list = arrary.split(',');

    for (var i = 0; i < list.length; i++) {
        $("tr").each(function (a, b) {
            var cell = $(this).find("th")[list[i]];
            if (cell) {
                $(cell).hide();
            }
            else {
                $($(this).find("td")[list[i]]).hide();
            }
        }
         )
    }

}
//机卡捆绑明细跳转
function goDetail(page, obj) {

    var date = $(obj.parentNode.parentNode).find("td").eq(1).text();
    var dis = $(obj.parentNode.parentNode).find("td").eq(2).text();
    var brid = $(obj.parentNode.parentNode).find("td").eq(3).text();
    var cd = $(obj.parentNode.parentNode).find("td").eq(4).text();

    location.href = page + "&date=" + date + "&dis=" + dis + "&brid=" + brid + "&cd=" + cd;

}
function onToggleSelect(chkName, isChecked) {
    var inputArray = document.getElementsByTagName("input");
    for (var i = 0; i < inputArray.length; i++) {
        if (inputArray[i].type == "checkbox" && inputArray[i].name.indexOf(chkName) != -1) {
            inputArray[i].checked = isChecked;
        }
    }
}

var type = "0";

var page_url = ""; var DefaultArea; var DefaultBrnd;

 

$(function () {
 
    if (DefaultBrnd) {
        var tspan = $("table").find("span");
        for (var i = 0; i < tspan.length; i++) {
            if (DefaultArea && !($(tspan[i]).html() == DefaultArea || $(tspan[i]).html() == DefaultBrnd)) {
                $(tspan[i].parentNode).html($(tspan[i]).text());
            }

        }
    }
    //   if ($(".libox").length > 0) {
    $("#ctl00_cphBody_listPager a").click(function () { $(document.body).showLoading(); })

    //  }

    if ($("#isshow").length > 0) {
        if ($("#isshow").val() == "1") {
            $("#div_hightSertch").show();
        } else {
            $("#div_hightSertch").hide();
        }
        $("#btn_high_Seach").show();
    }
    var h3 = $("h3");
    if (h3.length > 0 && document.URL.indexOf("menuid") > 0 && isShow == "True") {
        $('<a href=\"javascript:void(0)\" style="float:right;display:block;color:White;font-weight:800;margin-top:5px;margin-right:10px" onclick="show_explain()">口径说明</a>').insertAfter(h3);
    }


    $(".libox table tr").each(function (a, b) {

        var thres = $(this).find("td[thres]");
        if (thres.length > 0) {
            if (thres.html() == 1) {
                var text = $(this).find("td").first().text();
                $(this).find("td").first().html("<label style='color:#6E306A;font-size:24px;'>●</label>" + text);
            }
            if (thres.html() == 2) {
                var text = $(this).find("td").first().text();
                $(this).find("td").first().html("<label style='color:red;font-size:24px;'>●</label>" + text);
            }
        }
    }
    )


})
function show_explain() {
    showDialog();
    $("#div_back").show();
}
function getQueryString(name) { var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i"); var r = window.location.search.substr(1).match(reg); if (r != null) return unescape(r[2]); return null; }
function putin() {
    if (typeof ($(document.body).showLoading) == "function") {
        $(document.body).showLoading();
    }
}
function hide() {
    $(document.body).hideLoading();
}
//选择字段box
function showBox(page) {
    var thlist = $("table").find("th");

    if (fieldArrary.length == 0) {
        for (var i = 0; i < thlist.length; i++) {
            fieldArrary.push({ field: $(thlist[i]).attr("field"), text: $(thlist[i]).text(), sort: i });
        }
    }

    var rValue = window.showModalDialog("ChoiceField.aspx?type=" + type + "&formid=" + getQueryString("formid") + "&page=" + page + "&iii=" + 99999999 * Math.random(), fieldArrary, "dialogHeight: 270px; dialogWidth: 520px; center: Yes; help: no; resizable: no; status: no;");

    if (rValue) {
        var tempurl = document.URL;
        if (tempurl.indexOf("?") > 0) {
            if (page == "FindUser.aspx") {
                // location = "?i=" + Math.random();
                // document.forms[0].submit();
            }
            else {
                $("#ctl00_cphBody_con_Pms_btn_Seach").click();
            }
        }
        else {
            $("#ctl00_cphBody_con_Pms_btn_Seach").click();
        }
    }
}
//选择营销方案box
function showbox(flag) {

    var rValue = window.showModalDialog("GetMKPLN.aspx?tableName=" + tableName + "&flag=" + flag + "&i=" + Math.random(), "", "dialogHeight: 500px; dialogWidth: 720px; center: Yes; help: no; resizable: yes; status: no;");
    if (rValue > 0) {
        var tempurl = document.URL;
        if (tempurl.indexOf("?") > 0) {


            location = tempurl + "&i=" + Math.random();

        }
        else {
            location = tempurl + "?i=" + Math.random();
        }
    }
    else
    { }
}

function get_data(obj) {
    // alert($(obj.parentNode.parentNode).find("a").length);
    //  debugger;
    var type = $(obj).attr("by");
    var span = $(obj.parentNode.parentNode).find("span").first();
    if ($(obj.parentNode.parentNode).find("span").length == 1 || (span.attr("defaultarea") != span.attr("aid") && span.attr("by") != type)) {

        type = eval($(obj).attr("by")) + 2;
    }

    var a = encodeURIComponent($(obj).attr("aid"));
    var b = encodeURIComponent($(obj).attr("bid"));
    var m = $(obj.parentNode.parentNode.getElementsByTagName("td")[1]).text();
    //  var tempurl = document.URL.indexOf("?")>0? document.URL:document.URL+"?tt=1";
    var tid = getQueryString("tid");
    var formid = getQueryString("formid");
    var s = page_url + "?No=" + $(obj).attr("No") + "&type=" + type + "&bid=" + b + "&aid=" + a + "&cid=" + $(obj).attr("cid") + "&t=" + Math.random() + "&mid=" + m + "&tid=" + tid + "&formid=" + formid;
    location = s;
}
function get_NextLVL(obj) {
    // alert($(obj).attr("lvl_name"));
    var urlstr = "" + "?formid=" + getQueryString("formid") + "&menuid=" + getQueryString("menuid");

    $(obj.parentNode.parentNode).find("td[colname]").each(function (a, b) {
        urlstr += "&" + $(this).attr("colname") + "=" + encodeURIComponent($(this).attr("colval"));
    })
    if (getQueryString('GROUP_VEST_LVL_1') != null) {


        //    urlstr+='&GROUP_VEST_LVL_1='+decodeURIComponent
    }
    // "".substring()
    urlstr += "&lvl_name=" + $(obj).attr("lvl_name");

    // var lvl = $(obj).attr("lvl_name")
    urlstr += "&" + $(obj).attr("pre_lvl") + "=" + encodeURIComponent($(obj).attr('val'));
    // document.forms[0].action = urlstr;
    // document.forms[0].submit();
    //  alert(urlstr);
    location = urlstr;
}



var high_SeachBox;
//高级查询
function high_Seach() {

    if ($("#div_hightSertch").css("display") == "none") {
        $("#div_hightSertch").show();
        $("#isshow").val("1");
    }
    else {
        $("#div_hightSertch").hide();
        $("#isshow").val("2");
    }

}
function goLink(url) {
    var u = url.split('=');
    var str = "";
    for (var i = 0; i < u.length; i = i + 2) {
        var str2 = encodeURIComponent(u[i + 1].split("&")[0]);
        if (i - 1 > 0) {
            str += u[i - 1].split("&")[1] + "="
        }
        str += u[i] + "=" + str2 + "&"
    }

    str = str.substring(str, str.length - 1)
    if (getQueryString("lvl_name") != null) {
        str += "&lvl_name=" + getQueryString("lvl_name");
    }
    if (getQueryString("lvl_name") == "GROUP_VEST_LVL_4") { str += "&HIDE_VEST_LVL_3_CD=" + getQueryString("HIDE_VEST_LVL_3_CD"); }
    location = str;
}
function initKE(name) {
    KindEditor.ready(function (K) {
        K.create('#' + name, {
            allowFileManager: true,
            resizeMode: 1,
            allowPreviewEmoticons: false,
            allowUpload: false,
            items: [
				'fontname', 'fontsize', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
				'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
				'insertunorderedlist', '|', 'emoticons', 'image', 'link']
        });
    });
}