/*
*Latest update 2015.11.04 by guilin
*/
(function ($) { $.fn.layer = function (opt) { var opt = $.extend({}, $.fn.layer.defaults, opt); $(this).live(opt.trigger, function () { mainlayer(opt, 0, $(this)) }) }; jQuery.layer = function (opt) { var opt = $.extend({}, $.fn.layer.defaults, opt); mainlayer(opt, 1, 0) }; $.fn.layer.defaults = { width: null, height: null, trigger: "click", autoclose: 0, title: "", contents: "", poptype: 0, addclass: "", id: "", position: "fixed", closeparent: true, confirm: null, confirmback: null, cancel: null, cancelback: null, closeback: null, loadback: null, afterback: null, move: true, fadeout: 0, fadein: 0, masklayer: true, aligncenter: true, showclose: true, shadeclose: false }; function mainlayer(opt, t, th) { $("#greybackground").remove(); if (opt.masklayer) { $("body").append("<div id=\"greybackground\"></div>") } if (opt.loadback != null) { opt.loadback() } if (opt.closeparent) { $(".alertlayer").hide() } var _layerclose = "<a href=\"javascript:void(0);\" class=\"close layerclose\">×</a>", _layertitle = "<h3 class=\"layertitle\"><b></b>" + opt.title + "</h3>", _layerbutton = ""; if (opt.confirm != null || opt.cancel != null) { _layerbutton += "<div class=\"layerinput\">"; if (opt.confirm != null) { _layerbutton += "<input class=\"cell-btn-layerbutton layerbutton\" type=\"submit\" name=\"button\" value=\"" + opt.confirm + "\">" } if (opt.cancel != null) { _layerbutton += "<input class=\"cell-btn-layerreset layerreset\" type=\"reset\" name=\"button\" value=\"" + opt.cancel + "\">" } _layerbutton += "</div>" } var _alertlayer = ""; if (opt.poptype == 0) { var contents = opt.contents; if (opt.contents == "" && t == 0) { contents = "." + th.attr("data-action") } _alertlayer = $(contents) } else if (opt.poptype == 1) { var _class = ""; if (opt.addclass != "") { _class = opt.addclass } else { _class = "layer" + encodeURIComponent(opt.contents).replace(/%/g, '').replace(/ /g, '').replace(".", '').toLocaleLowerCase().substring(0, 10) } var html = "<div class=\"" + _class + "\" data-type=\"1\">"; html += "<div class=\"layercontent\"><p class=\"layerloading\">正在加载...</p></div>"; html += "</div>"; $("body").append(html); _alertlayer = $("." + _class); _alertlayer.children(".layercontent").load(opt.contents, function () { if (opt.afterback != null) { opt.afterback(_alertlayer) } }) } else if (opt.poptype == 2) { var _class = ""; if (opt.addclass != "") { _class = opt.addclass } else { _class = "layer" + encodeURIComponent(opt.contents).replace(/%/g, '').replace(/ /g, '').toLocaleLowerCase().substring(0, 10) } var html = "<div class=\"" + _class + "\" data-type=\"1\">"; html += "<div class=\"layertxt\"><span class=\"layericon\"></span>" + opt.contents + "</div>"; html += "</div>"; $("body").append(html); _alertlayer = $("." + _class) } if (!_alertlayer.hasClass("alertlayer")) { _alertlayer.addClass("alertlayer") } _alertlayer.addClass(opt.addclass); if (opt.id != "") { _alertlayer.attr("id", opt.id) } if (_alertlayer.children(".layertitle").length == 0 && opt.title != "") { _alertlayer.prepend(_layertitle) } if (_alertlayer.children(".layerclose").length == 0 && opt.showclose) { _alertlayer.prepend(_layerclose) } if (_alertlayer.children(".layerinput").length == 0) { _alertlayer.append(_layerbutton) } _alertlayer.fadeIn(opt.fadein); var newwidth = opt.width == null ? _alertlayer.outerWidth(true) : opt.width, newheight = opt.height == null ? _alertlayer.outerHeight(true) : opt.height, winwidth = $(window).width(), winheight = $(window).height(), sleft = $(window).scrollLeft(), _stop = $(window).scrollTop(); if (opt.position != "absolute") { _stop = 0 } var left1 = sleft + (winwidth - newwidth) / 2, top = (winheight - newheight) / 2; if (top < 0) { top = 0 } var top1 = _stop + top; if (opt.aligncenter) { _alertlayer.css({ "width": newwidth, "height": newheight, "left": left1, "top": top1, "position": opt.position }) } else { _alertlayer.css({ "width": newwidth, "height": newheight, "position": opt.position }) } if (opt.afterback != null && opt.poptype != 1) { opt.afterback(_alertlayer) } if (opt.shadeclose) { $("#greybackground").on("click", function () { if (opt.autoclose == 0) { layer.close(_alertlayer, opt.fadeout) } }) } var cleartime; if (opt.autoclose != 0) { _alertlayer.find(".layerclosetime").remove(); _alertlayer.append("<div class=\"layerclosetime\"></div>"); autoclosetime(parseInt(opt.autoclose)) } function autoclosetime(time) { if (time > 0) { --time; _alertlayer.find(".layerclosetime").html(time + "秒后将关闭！"); cleartime = setTimeout(function () { autoclosetime(time) }, 1000) } else { if (opt.closeback != null) { opt.closeback(_alertlayer) } else { layer.close(_alertlayer, opt.fadeout) } } } if (opt.confirm != null) { _alertlayer.find(".layerbutton").click(function (e) { if (opt.confirmback != null) { opt.confirmback(_alertlayer) } else { clearTimeout(cleartime); layer.close(_alertlayer, opt.fadeout) } }) } if (opt.cancel != null) { _alertlayer.find(".layerreset").click(function (e) { if (opt.cancelback != null) { opt.cancelback(_alertlayer) } else { layer.close(_alertlayer, opt.fadeout) } }) } _alertlayer.find(".close").click(function (e) { if (opt.closeback != null) { opt.closeback(_alertlayer) } else { clearTimeout(cleartime); layer.close(_alertlayer, opt.fadeout) } }); if (opt.move) { var docheight = $(document).height(); var _move = false; var _x, _y; _alertlayer.children("h3.layertitle").mousedown(function (e) { _move = true; _x = e.pageX - parseInt(_alertlayer.css("left")); _y = e.pageY - parseInt(_alertlayer.css("top")) }); $(document).mousemove(function (e) { if (_move) { var x = e.pageX - _x; var y = e.pageY - _y; if (x <= 0) { x = 0 } if (x > winwidth - newwidth) { x = winwidth - newwidth - 10 } if (y <= 0) { y = 0 } if (y > docheight - newheight) { y = docheight - newheight } _alertlayer.css({ top: y, left: x }) } }).mouseup(function () { _move = false }) } } $.fn.extend({ tooltip: function (opt) { opt = jQuery.extend({ direction: "tleft", maxwidth: 300, fade: 0, y: 0, x: 10, addclass: "" }, opt); var $this = $(this), objheight, winwidth = $(window).width(), maxwidth = opt.maxwidth == 0 ? "auto" : opt.maxwidth, y = opt.y == 0 ? $this.outerHeight() : opt.y, x = opt.x; $this.hover(function () { if ($(this).attr("data-alt") != "") { $("body").append("<div class=\"mousetips " + opt.direction + "\" style=\"max-width:" + maxwidth + "px\"><i></i><em></em>" + $(this).attr("data-alt") + "</div>"); if (opt.addclass != "") { $(".mousetips").addClass(opt.addclass) } objheight = $(".mousetips").outerHeight() } }, function () { $(".mousetips").fadeOut(opt.fade, function () { $(".mousetips").remove() }) }); $this.mousemove(function (e) { var postX = e.pageX, postY = e.pageY; switch (opt.direction) { case "tleft": $(".mousetips").css({ "left": postX - x, "top": postY - y - objheight }).fadeIn(opt.fade); break; case "tright": $(".mousetips").css({ "left": "auto", "right": winwidth - postX - x, "top": postY - y - objheight }).fadeIn(opt.fade); break; case "bleft": $(".mousetips").css({ "left": postX - x, "top": postY + y }).fadeIn(opt.fade); break; case "bright": $(".mousetips").css({ "left": "auto", "right": winwidth - postX - x, "top": postY + y }).fadeIn(opt.fade); break } }) }, inputfocus: function (sc) { sc = jQuery.extend({ vc: "highlight", fc: "on-focus" }, sc); $(this).each(function () { var $this = $(this); if ($this.attr("type") == "password" && typeof ($this.data("value")) != "undefined") { var t = $this.position().top, lf = $this.position().left; $this.after("<span class=\"pswplaceholder\" style=\"z-index:10;display:block;position:absolute;cursor:text;left:" + lf + "px;top:" + t + "px;height:" + $this.outerHeight() + "px;width:" + $this.width() + "px;color:" + $this.css("color") + ";padding-left:" + $this.css("padding-left") + "\">" + $this.data("value") + "</span>"); $("span.pswplaceholder").live("click", function () { $(this).hide().prev("input").focus() }) } }); $(this).live("focus", function () { var $this = $(this); if ($this.attr("type") == "password") { $this.next("span.pswplaceholder").hide() } $this.addClass(sc.vc).addClass(sc.fc); if ($this.val() == $this[0].defaultValue) { $this.val("") } }).live("blur", function () { var $this = $(this); if ($this.attr("type") == "password" && $this.val() == "") { $this.next("span.pswplaceholder").show() } $this.removeClass(sc.fc); if ($.trim($this.val()) == "" || $this.val() == $this[0].defaultValue) { $this.val($this[0].defaultValue); $this.removeClass(sc.vc) } }) }, inputzoom: function (t) { t = jQuery.extend({ f: 1, c: "w" }, t); $(this).before("<div class=\"zoomnum\" style=\"display:none\"></div>"); var $date = $(this), _t = "", keycode = "", $zoomnum = $(".zoomnum"); $zoomnum.html(""); $(this).focus(function () { var $input = $(this), $zoomnum = $input.prev(".zoomnum"), yW = $input.innerWidth(), yH = $input.outerHeight(), yL = $input.position().left, yT = $input.position().top; $zoomnum.css({ "width": yW, "height": yH * 1.2, "line-height": yH * 1.2 + "px", "left": yL, "top": (yT - yH * 1.2) }); var _txt = $input.val(); focustxt(_txt, $zoomnum); $.data($date, 'key', _txt) }).blur(function () { $zoomnum.fadeOut().html("") }).keydown(function (e) { keycode = e.keyCode; if (keycode == 90 && e.ctrlKey) { return false } }).bind('input propertychange', function (e) { var $input = $(this), $zoomnum = $input.prev(".zoomnum"), _oldv = $.data($date, 'key'), _newv = $input.val(), _cur = "", _postion = getCursorPos($(this).get(0)), _c = 0; if ($input.val() != "") { _c = _newv.length - _oldv.length; $zoomnum.fadeIn() } if (_c == 1) { _cur = _newv.substr(_postion - 1, 1), _append = "<span>" + _cur + "</span>"; addtxt($zoomnum, _append, _postion, t.f) } else if (_c == -1) { deltxt($zoomnum, _postion, t.f, $zoomnum.height(), _newv, keycode) } else if (_c > 1) { _cur = _newv.substr(_postion - _c, _c); var p = _postion - _c + 1; for (var i = 0; i < _c; i++) { var _append = "<span>" + _cur.substr(i, 1) + "</span>"; addtxt($zoomnum, _append, p, t.f); p++ } } else if (_c < -1) { var dp = _postion; for (var i = 0; i < Math.abs(_c) ; i++) { deltxt($zoomnum, dp, t.f, $zoomnum.height(), _newv, keycode); dp++ } } else if (_newv != _oldv) { focustxt(_newv, $zoomnum) } $.data($date, 'key', _newv) }); function focustxt(val, div) { if (val != "") { for (var i = 0; i < val.length; i++) { _t += "<span style=\"top:0\">" + val.substr(i, 1) + "</span>" } div.html("").append(_t); format(div, t.f); div.fadeIn(); _t = "" } else { div.html("") } } function format(div, ty) { var s = div.children("span"); s.removeClass(t.c); if (ty == 0) { s.eq(2).addClass(t.c); s.eq(6).addClass(t.c); s.eq(10).addClass(t.c) } else { s.each(function () { var index = s.index(this) + 1; if (index % 4 == 0) { $(this).addClass(t.c) } }) } } function deltxt(div, d, ty, h, txt, keycode) { if (keycode == 8 || keycode == 46) { div.children("span").eq(d).animate({ top: -h }, function () { $(this).remove(); format(div, ty) }) } else { focustxt(txt, div) } } function addtxt(div, _append, _postion, ty) { if (div.children("span").length == 0) { div.append(_append); div.children("span").animate({ top: 0 }) } else { if (_postion == 1) { div.prepend(_append); div.children("span").eq(0).animate({ top: 0 }) } else { var _eq = parseInt(_postion - 2); div.children("span").eq(_eq).after(_append); div.children("span").eq(_eq).next("span").animate({ top: 0 }) } } format(div, ty) } function getCursorPos(obj) { var result = 0; if (obj.selectionStart) { result = obj.selectionStart } else { try { var rng; rng = document.selection.createRange(); rng.moveStart("character", -event.srcElement.value.length); result = rng.text.length } catch (e) { throw new Error(10, "asdasdasd") } } return result } }, selectdrapdown: function (sdd) { sdd = jQuery.extend({ trigger: "click", linum: 0, slidespeed: 0, fadeout: 0, zindexdiv: null, valuediv: ".cell-input", afterback: null, vc: "highlight", fc: "on-focus" }, sdd); var $this = $(this), liheight = "", num = 0; $this.each(function () { var $th = $(this); $th.css({ "width": $th.children(sdd.valuediv).innerWidth() }); liheight = $th.find("li").outerHeight(); num = $th.find("li").length; if (sdd.linum != 0 && sdd.linum < num) { $th.children(".drapdown").children("ul").css("height", liheight * sdd.linum) } $th.children(".drapdown").css("top", $th.children(sdd.valuediv).outerHeight()) }); $this.children(sdd.valuediv).bind(sdd.trigger, function (event) { var $th = $(this).parent(); if ($th.hasClass("open")) { $th.removeClass("open").css("z-index", ""); $th.parents(sdd.zindexdiv).css("z-index", ""); $(this).removeClass(sdd.fc); $th.children(".drapdown").slideUp(sdd.slidespeed) } else { var sd = $(".selectdrapdown"); sd.removeClass("open").css("z-index", "").children(sdd.valuediv).removeClass(sdd.fc); sd.children(".drapdown").fadeOut(sdd.fadeout); $th.addClass("open").css("z-index", 999); $(this).addClass(sdd.fc); if (sdd.zindexdiv != null) { sd.parents(sdd.zindexdiv).css("z-index", ""); $th.parents(sdd.zindexdiv).css("z-index", 999) } $th.children(".drapdown").slideDown(sdd.slidespeed) } event.stopPropagation() }); $(document).click(function (e) { var c = e.target.className, p = 0; if (c != "") { p = $("." + c).parents(".selectdrapdown").length } if (p == 0) { $this.removeClass("open").children(sdd.valuediv).removeClass(sdd.fc); $this.children(".drapdown").fadeOut(sdd.fadeout); $this.css("z-index", "").parents(sdd.zindexdiv).css("z-index", "") } }); $this.find("li").click(function (event) { var $th = $(this); $th.parents(".drapdown").prevAll(sdd.valuediv).html($th.html()).addClass(sdd.vc).removeClass(sdd.fc); $th.addClass("hover").siblings("li").removeClass("hover"); $th.parents(".drapdown").fadeOut(sdd.fadeout); $this.css("z-index", "").parents(sdd.zindexdiv).css("z-index", ""); $this.removeClass("open").children(sdd.valuediv).removeClass(sdd.fc); sdd.afterback != null ? sdd.afterback($th) : ""; event.stopPropagation() }) } }) })(jQuery); var layer = { close: function (a, fo) { var alertlayer = ".alertlayer"; if (a) { alertlayer = a } $(alertlayer).fadeOut(fo, function () { var layernum = 0; $(".alertlayer").each(function () { if (!$(this).is(":hidden")) { layernum++ } }); if (layernum <= 0) { $("#greybackground").remove() } if ($(this).data("type") == "1") { $(this).remove() } }) } }
var iconfont = {
    json: function () {
        var isie = $.browser.msie, v = $.browser.version, dm = document.documentMode;
        if ((isie && dm <= 7) || (isie && v == 7.0 && !dm)) {
            var fontjson = { "iconcircle": "&#xe600;", "iconright": "&#xe601;", "iconwenhao": "&#xe602;", "iconjia": "&#xe603;", "iconjian": "&#xe604;", "icontipsinfo": "&#xe611;", "iconarrowdown": "&#xe605;", "iconarrowleft": "&#xe606;", "iconarrowright": "&#xe607;", "iconcalculator": "&#xe608;", "iconcorrect": "&#xe609;", "iconerror": "&#xe60a;", "iconfeedback": "&#xe60b;", "iconinstructions": "&#xe60c;", "iconmeg": "&#xe60d;", "iconmoney": "&#xe60e;", "iconmoney2": "&#xe60f;", "icontel": "&#xe610;", "iconback": "&#xe612;", "icondata": "&#xe613;" };

            $("[class*='icon-']").each(function () { var cla = $(this).attr("class").split(" "), c = "", jc = ""; if (cla.length > 1) { for (var i = 0; i < cla.length; i++) { if (cla[i].indexOf("icon-") > -1) { c = cla[i]; break } } } else { c = cla[0] } jc = c.replace(/-/g, ''); $("." + c).html(fontjson['' + jc + '']) })
        }
    }
}

////////////////////////////////////////////////////
$(function () {
    $(".cell-input").inputfocus();
    iconfont.json();
    /*右则浮动*/
    $(".advisory a.close_kf").click(function () {
        $(".advisory").animate({ right: -70 }, 300, function () {
            $(".advisory a.close_kf").fadeOut();
            $(".advisory a.show_kf").show().animate({ left: -24 }, 300);
        })
    });
    $(".advisory a.show_kf").hover(function () {
        $(this).animate({ left: 0 }, 100, function () {
            $(this).hide();
            $(".advisory a.close_kf").fadeIn();
            $(".advisory").animate({ right: 0 }, 300)
        })
    });
    //弹出计算器	
    $(".openreceipts").layer({
        poptype: 1,
        title: "收益计算器",
        width: 550,
        height: 520,
        addclass: "opencomputer",
        contents: "计算器.html .layer-computer" //用相对于根目录的绝对路径
    });
    //弹出计算器，contents地址要用相对于根目录的绝对路径，如/计算器.html
    //也可以把 计算器.html 的body内容复制到文档的底部（像公共底部），然后把脚本修改为以下，意见反馈也是一样
    //  $(".openreceipts").layer({
    //    title: "收益计算器",
    //	addclass:"opencomputer",
    //    width: 450,
    //    height: 420,
    //    contents: ".layer-computer"
    //  });
    //弹出计算器计算结果
    $(".computer-button").live("click",
    function () {
        var a = $(".computer-money").val(); //投资金额
        if (!chcknullnum(a)) {
            layeralert("投资金额不能为空且只能是数字", ".computer-money");
            return;
        }
        if (!chckisnum(a)) {
            layeralert("投资金额最多只能是两位数字！", ".computer-money");
            return;
        }
        var b = $(".computer-bf").val(); //预期年化利率
        if (!chcknullnum(b)) {
            layeralert("预期年化利率不能为空且只能是数字！", ".computer-bf");
            return;
        }
        if (!chckisnum(b)) {
            layeralert("预期年化利率最多只能是两位数字！", ".computer-bf");
            return;
        }
        var c = $(".computer-data").val(); //预期年化利率
        if (!chcknullnum(c)) {
            layeralert("投资期限不能为空且只能是数字！", ".computer-data");
            return;
        }
        var rm = a * 10000 * b / 100 / 365 * c;
        //取两位小数
        var rm2 = Math.round(rm * 100) / 100;
        $(".return-money").html(rm2);
        $(".computer-allmoney").html(rm2 + a * 10000);
    });
    /*重置计算器*/
    $(".computer-reset").live("click",
    function () {
        $(".computer-money").val("");
        $(".computer-data").val("");
        $(".computer-bf").val("");
        $(".return-money").html("");
        $(".computer-allmoney").html("");
    });

    //意见反馈
    $(".openfeedback").layer({
        width: 650,
        height: 520,
        poptype: 1,
        title: "意见反馈",
        contents: "意见反馈.html .layer-feedback",
        addclass: "openfb"
    }); //用相对于根目录的绝对路径
    /**其它添加的写在下面**/

    /**其它添加的写在上面**/
});

/*计算器输入错误提示*/
function layeralert(title, inputfocus) {
    $.layer({
        contents: title,
        poptype: 2,
        closeparent: false,
        title: "温馨提示",
        confirm: "确定",
        width: 400,
        confirmback: function (d) {
            layer.close(d);
            $(inputfocus).val("").focus();
        }
    });

}
/*计算器验证输入,检查是不是数字及两数位数*/
function chcknullnum(a) {
    if (!isNaN(a) && a != "") {
        return a;
    } else {
        return false;
    }
}
function chckisnum(a) {
    if (a.indexOf(".") > -1) {
        //如果有小数点
        var b = a.split('.')[1].length;
        if (b > 2) {
            a = false;
        }
    }
    return a;
}

$(function () {
    $(".enterprise-user").hide();
    $(".public-tab a.a-1").click(function () {
        $(".individual-user").show();
        $(".enterprise-user").hide();
        $(this).addClass("current");
        $(".public-tab a.a-2").removeClass("current");
    });

    $(".public-tab a.a-2").click(function () {
        $(".enterprise-user").show();
        $(".individual-user").hide();
        $(this).addClass("current");
        $(".public-tab a.a-1").removeClass("current");
    });
});

/*获取表单键值对Json对象*/
$.fn.formtojsonObj = function () {
    var o = {};
    var a = $(this).serializeArray();
    $.each(a, function (i, item) {
        if (o[item.name] !== undefined) {
            if (!o[item.name].push) {
                o[item.name] = [o[item.name]];
            }
            o[item.name].push(item.value || '');
        } else {
            o[item.name] = item.value || '';
        }
    });
    return o;
}
/*获取表单键值对Json字符串*/
$.fn.formtojsonStr = function () {
    var o = {};
    var a = $(this).serializeArray();
    $.each(a, function (i, item) {
        if (o[item.name] !== undefined) {
            if (!o[item.name].push) {
                o[item.name] = [o[item.name]];
            }
            o[item.name].push(item.value || '');
        } else {
            o[item.name] = item.value || '';
        }
    });
    return JSON.stringify(o);
}
