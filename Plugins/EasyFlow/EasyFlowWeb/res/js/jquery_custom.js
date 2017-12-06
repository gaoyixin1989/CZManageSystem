var itemarr=null;

//$(document).ready(function() {
//			
////为每个item添加图标、id等
//$("#item_select li").each(function(){  $($(this)[0].childNodes[0]).wrap("<div class='item_name'></div>");  });
//$("#item_select li:has(ul)>.item_name").prepend("<div class='item_close'></div>");
//$("#item_select li:not(:has(ul))>.item_name").prepend("<div class='items'></div>");
//$("#item_select li:has(ul)>.item_name").prepend("<div class='group_add' title='添加该组'></div>");
//$("#item_select .item_name").each(function(i){
//	$(this).attr("id", "item"+i );
//});

////各个item生成数组，以及把当前树的HTML预存到一个变量
//itemarr = jQuery.makeArray($(".item_name:not(:has('.group_add'))").parent());
//var item_select_html =  $("#item_select").html();

////初始化及重新绑定所有事件
//allclickable();

////关键字搜索
//$("#find_item .inputbox").keyup(function(){
//	var finditem = $(this).attr("value").toLowerCase();
//	if ( finditem != "" ) {
//		$("#find_clear").css("visibility","visible");
//		var str = "";
//		for(i in itemarr){
//			var itemtext = $(itemarr[i]).text().toLowerCase();
//			var itemhtm = $(itemarr[i]).html();
//			if(itemtext.indexOf(finditem) != -1 ){
//				str += ("<li>" + itemhtm + "</li>");
//			}
//		}
//		if ( str == "" ) {
//			str = "<div style='font-size:14px;height:30px;line-height:30px;padding-left:10px;color:#999;'>抱歉，谁也没找到</div>";
//		}
//		$("#item_select").html(str);
//		allclickable();
//	} else {
//		$("#find_clear").css("visibility","hidden");
//		$("#item_select").html(item_select_html);
//		allclickable();
//	}
//});
////清空已选item
//$("#selected_clear").click(function() {
//	selected_clear();
//	return false;
//});
////清空搜索关键字
//$("#find_clear").click(function() {
//	$(this).css("visibility","hidden");
//	$("#find_item .inputbox").attr("value","");
//	$("#item_select").html(item_select_html);
//	allclickable();
//	return false;
//});


//});


function init() {
    //为每个item添加图标、id等
    $("#item_select li").each(function () { $($(this)[0].childNodes[0]).wrap("<div class='item_name'></div>"); });
    $("#item_select li:has(ul)>.item_name").prepend("<div class='item_close'></div>");
    $("#item_select li:not(:has(ul))>.item_name").prepend("<div class='items'></div>");
    $("#item_select li:has(ul)>.item_name").prepend("<div class='group_add' title='添加该组'></div>");
    $("#item_select .item_name").each(function (i) {
        $(this).attr("id", "item" + i);
    });
    if (issimple)//单选
        $("#item_select .group_add").hide();
    //各个item生成数组，以及把当前树的HTML预存到一个变量
    itemarr = jQuery.makeArray($(".item_name:not(:has('.group_add'))").parent());
    var item_select_html = $("#item_select").html();

    //初始化及重新绑定所有事件
    allclickable();

  
    //关键字搜索
//    $("#find_item .inputbox").keyup(function () {
//        var finditem = $(this).attr("value").toLowerCase();
//        if (finditem != "") {
//            $("#find_clear").css("visibility", "visible");
//            var str = "";
//            for (i in itemarr) {
//                var itemtext = $(itemarr[i]).text().toLowerCase();
//                var itemhtm = $(itemarr[i]).html();
//                if (itemtext.indexOf(finditem) != -1) {
//                    str += ("<li>" + itemhtm + "</li>");
//                }
//            }
//            if (str == "") {
//                str = "<div style='font-size:14px;height:30px;line-height:30px;padding-left:10px;color:#999;'>抱歉，谁也没找到</div>";
//            }
//            $("#item_select").html(str);
//            allclickable();
//        } else {
//            $("#find_clear").css("visibility", "hidden");
//            $("#item_select").html(item_select_html);
//            allclickable();
//        }
//    });



    //清空已选item
    $("#selected_clear").click(function () {
        selected_clear();
        return false;
    });
    //清空搜索关键字
    $("#find_clear").click(function () {
        $(this).css("visibility", "hidden");
        $("#find_item .inputbox").attr("value", "");
        $("#item_select").html(item_select_html);
        allclickable();
        return false;
    });
}

//IE6添加鼠标移上样式
function ie6hover() {
	if ($.browser.msie && $.browser.version==6.0) {
		$("#item_select li span").remove();
		$(".item_name").hover(function() {
				$(this).addClass("item_name_hover");
			}, function() {
				$(this).removeClass("item_name_hover");
		});
	}
}
//初始化及重新绑定所有事件
function allclickable() {
	$("#item_select li:not(:has(ul))>.item_name").click(function(){itemsadd(this)});
	$("#item_select .group_add").click(groupadd);
	remakeclick();
	$(".item_click").click(del_selected);
	itemtree();
	ie6hover()
}
//左边item树的折叠
function itemtree() {
	$("#item_select li:has(ul)").find(".item_name").click(function() {
	    $(this).next("ul").slideToggle("fast");
        
		$(this).find(".item_close").toggleClass("item_open");
	}); 
}
//取消搜索后，重新根据右边所选item，对初始化的树重新生成打勾符号
function remakeclick() {
	$("#selected_item .item_name").each(function() {
		var nameid = $(this).attr("rel") ;
		var itemclick = $("#item_select li[hid='"+nameid+"']");
		if ( itemclick.find(".item_click").size() == 0 ) {
			itemclick.prepend("<div class='item_click' title='已添加'></div>").click(del_selected);
		}
	});
}
//对已选item执行清空操作的同时，删掉左边对应打勾
function selected_clear() {
	$("#selected_item .item_name").each(function() {
		var nameid = $(this).attr("rel") ;
		$(this).parent().remove();
	//	var itemclick = $("#item_select li[hid=" + nameid + "]");
		$("#item_select li[hid='" + nameid + "']").find(".item_click").remove();
	});
}
//对已选item执行删除操作
function del_selected(e) {
    var nameid = $(this.parentNode).parent().attr("hid") || $(this).parent().attr("rel");
	$("#selected_item").find(".item_name[rel='"+nameid+"']").parent("li").remove();
	$("#item_select li[hid='" + nameid + "']").find(".item_click").remove();
	e.stopPropagation();
}
//添加item操作
function itemsadd() {
    if (selectTyep == 1) { selected_clear(); }
	var oThis = arguments[0];
	var itemname = $(oThis).text();
	var nameid = $(oThis.parentNode).attr("hid");
	var hid = $(oThis.parentNode).attr("hid");
	if(issimple)
        selected_clear();
	if ( $("#selected_item").find(".item_name[rel='"+nameid+"']").size() == 0 ) {
	    $(oThis).prepend("<div class='item_click' title='已添加'></div>");
	    nameid = nameid.replace(/>/g, '&gt;');
	    hid = hid.replace(/>/g, '&gt;');
        //alert(nameid)
		var selected = $("<li><div class='item_name' title='" + itemname.replace(/>/g, '&gt;') + "' hid=" + hid + "  rel='" + nameid.replace(/>/g, '&gt;') + "'><div class='items'></div><div class='item_del' title='取消选择'></div>" + itemname.replace(/>/g, '&gt;') + "</div></li>");
		$("#selected_item").append(selected);	
		selected.find(".item_del").click(del_selected);
		$(oThis).find(".item_click").click(del_selected);
}


}
//整组添加
function groupadd(e) {
	$(this).parent().parent().find(".item_name:not(:has('.group_add'))").each(function() {
	itemsadd(this);
	});
	e.stopPropagation();
}

/*Html编码解码*/
$(function () {
    $.fn.HTMLEncode = function () {
        var str = $(this).val();
        var s = "";
        if (str == null)
            $(this).attr("value", "");
        else if (str.length == 0) $(this).attr("value", "");
        else {
            s = str.replace(/&/g, "&amp;");
            s = s.replace(/</g, "&lt;");
            s = s.replace(/>/g, "&gt;");
            //s = s.replace(/ /g, "&nbsp;");
            s = s.replace(/ /g, " ");
            s = s.replace(/\'/g, "&#39;");
            s = s.replace(/\"/g, "&quot;");
            $(this).attr("value", s);
        }
    }
    $.fn.HTMLDecode = function (str) {
        var str = $(this).val();
        var s = "";
        if (str == null)
            $(this).attr("value", "");
        else if (str.length == 0) $(this).attr("value", "");
        else {
            s = str.replace(/&amp;/g, "&");
            s = s.replace(/&lt;/g, "<");
            s = s.replace(/&gt;/g, ">");
            s = s.replace(/&nbsp;/g, " ");
            s = s.replace(/&#39;/g, "\'");
            s = s.replace(/&quot;/g, "\"");
            $(this).attr("value", s);
        }
    }
});
/*Html编码解码结束*/