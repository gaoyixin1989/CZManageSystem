var f_menuShow=false;

/*弹出窗口*/
function openWindow(url,w,h){
    var top,left;
    if(h>600){
        top=0;
        h=screen.height;
    }else{
        top=(screen.height-h)/2;
    }
    left=(screen.width - w)/2;
    window.open(url,"newSelect","width="+w+",height="+h+",top="+top+",left="+left+",hotkeys=1,menubar=0,resizable=1,scrollbars=1");
}
/*弹窗关闭*/
function windowClose(){
	window.close();
}
/*左侧菜单全部展开*/
function menuShowAll(obj){
	obj.blur();
	var menuObj=document.getElementById("menuList").getElementsByTagName("li");
	if(f_menuShow){
		for(var i=0;i<menuObj.length;i++){
			menuObj[i].className="";
			f_menuShow=false;
			obj.innerHTML="全部展开";
		}
	}else{
		for(var i=0;i<menuObj.length;i++){
			menuObj[i].className="c";
			f_menuShow=true;
			obj.innerHTML="全部收缩";
		}
	}
}
/*表格响应鼠标事件*/
function createTblEvent(tblId){
		var tblObj=document.getElementById(tblId);
		var trObj=tblObj.getElementsByTagName("tr");
		for (var i=0;i<trObj.length;i++){
			trObj[i].id=i;
		}

		var tdObj=tblObj.getElementsByTagName("td");
		for (var i=0;i<tdObj.length;i++){
			tdObj[i].onmouseover=function(){
				var cObj=this.parentNode.getElementsByTagName("td");
				for(var j=0;j<cObj.length;j++){
					cObj[j].className="yellow";
				}
			}
			tdObj[i].onclick=function(){
				
			}
			tdObj[i].onmouseout=function(){
				var cObj=this.parentNode.getElementsByTagName("td");
				for(var j=0;j<cObj.length;j++){
					cObj[j].className="";
				}

			}
		}
}
/*搜索折叠*/
function showSearch(obj){
	var sObj=document.getElementById("searchBody");
	if(sObj.style.display!="none"){
		sObj.style.display="none";
		obj.className="c";
		obj.parentNode.style.backgroundPosition="left bottom";
		obj.title="展开";
	}else{
		sObj.style.display="block";
		obj.className="";
		obj.parentNode.style.backgroundPosition="left top";
		obj.title="收缩";
	}
	obj.blur();
	return false;
}
/*普通折叠*/
function showContent(obj1,obj2){
	var sObj=document.getElementById(obj2);
	if(sObj.style.display!="none"){
		sObj.style.display="none";
		obj1.className="c";
		obj1.title="展开";
	}else{
		sObj.style.display="block";
		obj1.className="";
		obj1.title="收缩";
	}
	obj1.blur();
	return false;
}
/*取目标绝对位置*/
function Offset(e)

{
    try
    {
	var t = e.offsetTop;
	var l = e.offsetLeft;
	var w = e.offsetWidth;
	var h = e.offsetHeight-2;

	while(e=e.offsetParent)
	{
		t+=e.offsetTop;
		l+=e.offsetLeft;
	}
	return {
		top : t,
		left : l,
		width : w,
		height : h
	}
	}
	catch(e)
	{}
}
/*更改信息显示内容字体大小*/
function changeFont(obj,value){
	obj.blur();
	var bObj=document.getElementById("msgBody");
	switch(value){
		case 1:
			bObj.className="msgBody3";
		break;
		case 2:
			bObj.className="msgBody";
		break;
		case 3:
			bObj.className="msgBody2";
		break;
	}
	var pObj=obj.parentNode.getElementsByTagName("input");
	for(var i=0;i<pObj.length;i++){
		pObj[i].style.color="#09c";
	}
	obj.style.color="#F60";
}

if(!window.bw) window.bw = {};
bw.tip = {
    initBWTIP: function(){
	       $("[bwtip]")
		   .mouseover(function(){if (!window.ActiveXObject) return;bw.tip.displayBWTIP(this.bwtip,this.id,this.params)})
		   .mouseout(function(){$("#BWTIP").remove();});		   
    },

    displayBWTIP: function(tipType,id,params){
	    var de = document.documentElement;
	    var w = self.innerWidth || (de&&de.clientWidth) || document.body.clientWidth;
	    var hasArea = w - bw.tip.getAbsoluteLeft(id);
	    var clickElementy = bw.tip.getAbsoluteTop(id) - 3; //set y position
	    var clickElementx = 0;
    	
	    var tipWidth = 650;
	    var title = "&nbsp;";
	    var appPath = "/";
        if(params != undefined){
	        var paramsArray = bw.tip.parseQuery(params);
	        if (paramsArray['width'] != undefined) { tipWidth = paramsArray['width']; };
	        if (paramsArray['title'] != undefined) { title = paramsArray['title']; };
	        if (paramsArray['apppath'] != undefined) { appPath = paramsArray['apppath']; };
	    }

	    if(hasArea>(tipWidth*1)){
		    $("body").append("<div id='BWTIP' style='width:"+tipWidth*1+"px'><div id='BWTIP_arrow_left'></div><div id='BWTIP_close_left'>"+title+"</div><div id='BWTIP_copy'><div class='BWTIP_loader'><div></div></div>");//right side
		    var arrowOffset = bw.tip.getElementWidth(id) + 11;
		    clickElementx = bw.tip.getAbsoluteLeft(id) + arrowOffset;
	    }else if(hasArea <= 200){
		    $("body").append("<div id='BWTIP' style='width:"+tipWidth*1+"px;filter:Alpha(opacity=80);'><div id='BWTIP_close_left'>"+title+"</div><div id='BWTIP_copy'><div class='BWTIP_loader'><div></div></div>");//right side
		    var arrowOffset = bw.tip.getElementWidth(id) + 11;
		    clickElementx = 10;
		    clickElementy = clickElementy + 20;
	    }else{
		    $("body").append("<div id='BWTIP' style='width:"+tipWidth*1+"px;'><div id='BWTIP_arrow_right' style='left:"+((tipWidth*1)+1)+"px'></div><div id='BWTIP_close_right'>"+title+"</div><div id='BWTIP_copy'><div class='BWTIP_loader'><div></div></div>");//left side
		    clickElementx = bw.tip.getAbsoluteLeft(id) - ((tipWidth*1) + 15); 
	    }	

    	if ($(window).height() - clickElementy < 125) {
	        clickElementy = clickElementy - 140;
		    $("#BWTIP_arrow_right , #BWTIP_arrow_left").css("top", "140px");
        }else if ($(window).height() - clickElementy < 150) {
	        clickElementy = clickElementy - 220;
		    $("#BWTIP_arrow_right , #BWTIP_arrow_left").css("top", "220px");
        } else if ($(window).height() - clickElementy < 220) {
		    clickElementy = clickElementy - 50;
		    $("#BWTIP_arrow_right , #BWTIP_arrow_left").css("top", "50px");
        } 
    	
	    $('#BWTIP').css({left: clickElementx+"px", top: clickElementy+"px"});
	    $('#BWTIP').show();
    	
	    bw.tip.getTipContent(id, tipType, appPath);
    },

    getTipContent: function(id, tipType, appPath){
        var url = appPath + "workflow/extension/GetBWTipContent.ashx";
        $.get(url, {id: id, tipType: tipType}, 
            function(result){
                $('#BWTIP_copy').html(result);
            });
    },

    getElementWidth: function(objectId) {
	    x = document.getElementById(objectId);
	    return x.offsetWidth;
    },

    getAbsoluteLeft: function(objectId) {
	    o = document.getElementById(objectId)
	    oLeft = o.offsetLeft            
	    while(o.offsetParent!=null) {  
		    oParent = o.offsetParent   
		    oLeft += oParent.offsetLeft
		    o = oParent
	    }
	    return oLeft
    },

    getAbsoluteTop: function(objectId) {
	    o = document.getElementById(objectId)
	    oTop = o.offsetTop           
	    while(o.offsetParent!=null) { 
		    oParent = o.offsetParent  
		    oTop += oParent.offsetTop 
		    o = oParent
	    }
	    return oTop
    },

   parseQuery: function(query) {
       var Params = new Object ();
       if ( ! query ) return Params; 
       var Pairs = query.split(/[;&]/);
       for ( var i = 0; i < Pairs.length; i++ ) {
          var KeyVal = Pairs[i].split('=');
          if ( ! KeyVal || KeyVal.length != 2 ) continue;
          var key = unescape( KeyVal[0] );
          var val = unescape( KeyVal[1] );
          val = val.replace(/\+/g, ' ');
          Params[key] = val;
       }
       return Params;
    }
};

//获取URL参数值.
var getQueryString = function(fieldName) {  
    var urlString = document.location.search;
    if(urlString != null) {
       var typeQu = fieldName+"=";
       var urlEnd = urlString.indexOf(typeQu);
       if(urlEnd != -1) {
            var paramsUrl = urlString.substring(urlEnd+typeQu.length);
            var isEnd =  paramsUrl.indexOf('&');
            if(isEnd != -1) {
                 return paramsUrl.substring(0, isEnd);
            } else {
                return paramsUrl;
            }
       } else {
            return null;
       }
    } else {
        return null;
    }
}