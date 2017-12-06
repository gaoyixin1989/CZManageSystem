String.prototype.trim = function(){
    return this.replace(/^\s+|\s+$/g, "");
};

var SelectionItem = function(n, v){
	this.name = n;
	this.value = v;
}
var __selectionItems__ = new Array();
function pushSelection(n, v){
    __selectionItems__.push(new SelectionItem(n, v));
}
function bindSelectionItems(elName, elValue){
	if (!elValue || elValue.length == 0) {
		return;
	}
	var elValue2 = elValue + ",";
	var arr = document.getElementsByName(elName);		
	if (arr && arr.length > 0){			
		for (var i = 0, icount = arr.length; i < icount; i++){
		    if (arr[i].nodeName == "SELECT") {
		        //console.warn('>>>>>>>>>'+   $("#bwdf_ddl_F1").children().length)
				arr[i].value = elValue;
			} else {
				if (elValue2.indexOf(arr[i].value) != -1){
					arr[i].checked = true;
					if (arr[i].type == "radio"){
						break;
					}
				} 
			}					
		}
	}
}
function adjustTextArea(el){
    var elH = 0;
    if(el.Height)
        elH = el.Height;
    else if(el.height)
        elH = el.height;
	var h = Math.max(elH, el.scrollHeight) + (el.offsetHeight - el.clientHeight);
	if (!isNaN(h)){
		el.style.height = h;
	}
}

var Validator = {

require : /.+/,

email : /^\w+([-+.]\w+)*@\w+([-.]\\w+)*\.\w+([-.]\w+)*$/,

phone : /^((\(\d{3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}$/,

mobile : /^((\(\d{3}\))|(\d{3}\-))?13\d{9}$/,

url : /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/,

idcard : /^\d{15}(\d{2}[A-Za-z0-9])?$/,

currency : /^\d+(\.\d+)?$/,

number : /^\d+$/,

zip : /^[1-9]\d{5}$/,

qq : /^[1-9]\d{4,8}$/,

integer : /^[-\+]?\d+$/,

double : /^[-\+]?\d+(\.\d+)?$/,

english : /^[A-Za-z]+$/,

chinese : /^[\u0391-\uFFE5]+$/,

unsafe : /^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\"]*)|.{0,5})$|\s/,

issafe : function(str){return !this.unsafe.test(str);},

safestring : "this.issafe(value)",

limit : "this.isLimit(value.length,getAttribute('min'), getAttribute('max'))",

limitb : "this.isLimit(this.lenB(value), getAttribute('min'), getAttribute('max'))",

date : "this.isDate(value, getAttribute('format'))",

repeat : "value == document.getElementsByName(getAttribute('to'))[0].value",

range : "getAttribute('min') < value && value < getAttribute('max')",

compare : "this.isCompare(value,getAttribute('operator'),getAttribute('to'))",

match : "this.isMatch(value,getAttribute('operator'),getAttribute('to'))",

custom : "this.exec(value, getAttribute('to'))",

group : "this.mustChecked(getAttribute('name'), getAttribute('min'), getAttribute('max'))",

ErrorItem : [document.forms[0]],

ErrorMessage : ["以下原因导致提交失败：\t\t\t\t"],

validate : function(theForm, mode){
	var obj = theForm || event.srcElement.form || event.srcElement;
	var count = obj.elements.length;
	this.ErrorMessage.length = 1;
	this.ErrorItem.length = 1;
	this.ErrorItem[0] = obj;

	for(var i=0;i<count;i++) {
		with(obj.elements[i]) {	
			var _validateType = getAttribute("validateType");
						
			if(typeof(_validateType) == "object" || typeof(this[_validateType]) == "undefined") 
				continue;
			
			this.clearState(obj.elements[i]);
			
			if(getAttribute("require") != null && getAttribute("require").toLowerCase() == "false" && value == "") 
				continue;
			
			_validateType = _validateType.toLowerCase();
			value = value.trim();
			switch(_validateType) {			
			case "date" :			
			case "repeat" :			
			case "range" :			
			case "compare" :			
			case "custom" :			
			case "group" : 			
			case "limit" :			
			case "limitb" :			
			case "safestring" :
			case "match" :
				if(!eval(this[_validateType])) {				
					this.addError(i, getAttribute("msg"));				
				}
				break;			
			default :
				if(!this[_validateType].test(value)){				
					this.addError(i, getAttribute("msg"));				
				}				
				break;	
			}	
		}	
	}

	if(this.ErrorMessage.length > 1){	
		mode = mode || 1;		
		var errCount = this.ErrorItem.length;
		
		switch(mode){		
		case 2 :		
			for(var i=1;i<errCount;i++)		
				this.ErrorItem[i].style.color = "red";		
		case 1 :		
			alert(this.ErrorMessage.join("\n"));		
			this.ErrorItem[1].focus();	
			break;		
		case 3 :		
			for(var i=1;i<errCount;i++){		
				try{				
					var span = document.createElement("SPAN");					
					span.id = "__ErrorMessagePanel";					
					span.style.color = "red";					
					this.ErrorItem[i].parentNode.appendChild(span);					
					span.innerHTML = this.ErrorMessage[i].replace(/\d+:/,"*");				
				}				
				catch(e){alert(e.description);}			
			}
			
			this.ErrorItem[1].focus();			
			break;		
		default :		
			alert(this.ErrorMessage.join("\n"));		
			break;		
		}
		
		return false;	
	}
	
	return true;
},

isLimit : function(len,min, max){
	min = min || 0;
	max = max || Number.MAX_VALUE;
	return min <= len && len <= max;
},

lenB : function(str){
	return str.replace(/[^\x00-\xff]/g,"**").length;
},

clearState : function(elem){
	with(elem){	
		if(style.color == "red")	
			style.color = "";	
		var lastNode = parentNode.childNodes[parentNode.childNodes.length-1];	
		if(lastNode.id == "__ErrorMessagePanel")	
			parentNode.removeChild(lastNode);	
	}
},

addError : function(index, str){
	this.ErrorItem[this.ErrorItem.length] = this.ErrorItem[0].elements[index];
	this.ErrorMessage[this.ErrorMessage.length] = this.ErrorMessage.length + ":" + str;
},

exec : function(op, reg){
	return new RegExp(reg,"g").test(op);
},

isCompare : function(op1,operator,op2){
	operator = operator.toLowerCase();
	if (isNaN(op1) || isNaN(op2)) {
		return false;
	}
	
	op1 = parseFloat(op1);
	op2 = parseFloat(op2);
	switch (operator) {
		case "notequal":	//NotEqual
			return (op1 != op2);
		case "greaterthan":	//GreaterThan
			return (op1 > op2);
		case "greaterthanequal":	//GreaterThanEqual
			return (op1 >= op2);
		case "lessthan":		//LessThan
			return (op1 < op2);
		case "lessthanequal":	//LessThanEqual
			return (op1 <= op2);
		default:
			return (op1 == op2); 
	}
},

isMatch : function(op1,operator,op2){
	operator = operator.toLowerCase();
	switch (operator) {	
		case "notcontain":			//不包含 
			return op1.indexOf(op2) == -1;	
		case "beginwith":			//匹配开头
			return op1.indexOf(op2) == 0;	
		case "notbeginwith":		//不匹配开头
			return op1.indexOf(op2) != 0;	
		case "endwith":				//匹配结尾
			return op1.lastIndexOf (op2) == (op1.length - op2.length);	
		case "notendwith":		//不匹配结尾
			return op1.lastIndexOf (op2) != (op1.length - op2.length);	
		default:						//contain 包含 
			return op1.indexOf(op2) != -1;	
	}
},

mustChecked : function(name, min, max){
	var arr = name.split(":");
	var groupName = arr[0];
	var groups = getItems(groupName);
	var hasChecked = 0;
	min = min || 1;
	max = max || groups.length;

	for(var i=groups.length-1;i>=0;i--)
		if(groups[i].checked) hasChecked++;

	return min <= hasChecked && hasChecked <= max;
	
	function getItems(groupName) {
		var items = new Array();
		var arr = document.getElementsByTagName("input");		
		for (var i=0; i<arr.length; i++) {
			if (arr[i].type == "checkbox" || arr[i].type == "radio") {
				if(arr[i].name.indexOf(groupName) != -1) {
					items.push(arr[i]);
				}
			}
		}
		return items;
	}	
},

isDate : function(op, formatString){
	formatString = formatString || "ymd";
	var m, year, month, day;
	
	switch(formatString){	
		case "ymd" :		
			m = op.match(new RegExp("^\\s*((\\d{4})|(\\d{2}))([-./])(\\d{1,2})\\4(\\d{1,2})\\s*$"));		
			if(m == null ) 
				return false;			
			day = m[6];			
			month = m[5];			
			year = (m[2].length == 4) ? m[2] : GetFullYear(parseInt(m[3], 10));			
			break;		
		case "dmy" :		
			m = op.match(new RegExp("^\\s*(\\d{1,2})([-./])(\\d{1,2})\\2((\\d{4})|(\\d{2}))\\s*$"));			
			if(m == null ) 
				return false;			
			day = m[1];			
			month = m[3];			
			year = (m[5].length == 4) ? m[5] : GetFullYear(parseInt(m[6], 10));			
			break;
		case "datetime":
			m = op.match(new RegExp("^\\s*((\\d{4})|(\\d{2}))([-./])(\\d{1,2})\\4(\\d{1,2})(\\s[0-2]?[0-9][:][0-5]?[0-9][:][0-5]?[0-9])?\\s*$"));
			if(m == null ) 
				return false;
			day = m[6];			
			month = m[5];			
			year = (m[2].length == 4) ? m[2] : GetFullYear(parseInt(m[3], 10));
			break;
		default :		
			break;
	}
	month--;
	var date = new Date(year, month, day);	
	return (typeof(date) == "object" && year == date.getFullYear() && month == date.getMonth() && day == date.getDate());
	
	function GetFullYear(y){
		return ((y<30 ? "20" : "19") + 	y)|0;
	}
}
}