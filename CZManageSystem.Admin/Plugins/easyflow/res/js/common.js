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
		sObj.style.display="";
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
/*提示信息开始*/
document.write("<span id='hintdiv' style='display:none;position:absolute;z-index:500;'></span>");
function showHint(obj, title, appPath) {
    var hintContent = getHintContent(title, appPath);
    if (null == hintContent) return false;

    var top = getTop(obj); 
    var showtype = "up";
    var topimg = appPath + "res/img/hint_top.gif";
    var bottomimg = appPath + "res/img/hint_array_bottom.gif";
    var hintimg = appPath + "res/img/hint_info.gif";
    if (top < 150) {
        showtype = "down";
        topimg = appPath + "res/img/hint_array_top.gif";
        bottomimg = appPath + "res/img/hint_bottom.gif";
    }
    showHintInfo(obj, 0, 0, '提示', hintContent, 0, showtype, topimg, bottomimg, hintimg);
}
function showHintInfo(obj, objleftoffset, objtopoffset, title, info, objheight, showtype, topimg, bottomimg, hintimg) {
    var p = getPosition(obj);
    if ((showtype == null) || (showtype == "")) {
        showtype == "up";
    }

    var html = " <div style='position:absolute; visibility: visible; width:271px;z-index:501;'> <p style='margin:0; padding:0;'> <img src='" + topimg + "'/> </p> <div style='overflow:hidden; zoom:1; border-left:1px solid #000000; border-right:1px solid #000000; padding:3px 10px;  text-align:left; word-break:break-all;letter-break:break-all;font: 12px/160% Tahoma, Verdana,snas-serif; color:#6B6B6B; background:#FFFFE1 no-repeat;margin-top:-5px;margin-bottom:-5px;'> <img style='float:left;margin:0 3px 0px 3px;' src='" + hintimg + "' /> <span id='hintinfoup'>" + info + "</span> </div> <p style='margin:0; padding:0;'> <img src='" + bottomimg + "'/> </p> </div> <iframe id='hintiframe' style='position:absolute;z-index:100;width:276px;scrolling:none;' frameborder='0'></iframe>";

    document.getElementById('hintdiv').style.display = 'block';

    if (objtopoffset == 0) {
        document.getElementById("hintdiv").innerHTML = html;
        if (showtype == "up") {
            document.getElementById('hintiframe').style.height = objheight + "px";
            document.getElementById('hintdiv').style.top = (p['y'] - document.getElementById('hintinfo' + showtype).offsetHeight - 43) + "px";
        }
        else {
            document.getElementById('hintiframe').style.height = objheight + "px";
            document.getElementById('hintdiv').style.top = p['y'] + obj.offsetHeight + 3 + "px";
        }
    }
    else {
        document.getElementById('hintdiv').style.top = p['y'] + objtopoffset + "px";
    }

    document.getElementById('hintdiv').style.left = p['x'] + objleftoffset + "px";
}
function hideHint() {
    document.getElementById('hintdiv').style.display = 'none';
}
function getHintContent(title, appPath) {
    var url = appPath + "apps/xqp2/pages/help/GetHintContent.ashx?t=" + escape(title);
    var strResult = ($).ajax({ url: url, async: false }).responseText;

    if (strResult && strResult != "null")
        return strResult;
    return null;
}
function getPosition(obj) {
    var r = new Array();
    r['x'] = obj.offsetLeft;
    r['y'] = obj.offsetTop;
    while (obj = obj.offsetParent) {
        r['x'] += obj.offsetLeft;
        r['y'] += obj.offsetTop;
    }
    return r;
}
function getTop(obj) {
    var top = 0;
    do {
        top += obj.offsetTop - obj.scrollTop;
        obj = obj.parentElement;
    }
    while (obj.tagName != "BODY")
    top -= obj.scrollTop; //减去BODY的scrollTop;     
    return top;
}
/*提示信息结束*/