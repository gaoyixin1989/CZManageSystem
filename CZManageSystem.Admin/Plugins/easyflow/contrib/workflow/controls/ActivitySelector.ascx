<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_ActivitySelector" Codebehind="ActivitySelector.ascx.cs" %>
<div class="showControl">
    <h4>工单路径选择</h4>
    <button onclick="return showContent(this,'dataActivitySelector');" title="收缩"><span></span></button>
</div>
<div id="dataActivitySelector" class="dataTable">
    <asp:Literal ID="ltlNextActivities" runat="server"></asp:Literal>
</div>
<script language="javascript" type="text/javascript">
// <!CDATA[ (分派)
 var checkLimit = 10;
 var Count = -2;
 var ActorIndex = ""; //活动选择器索引
 function toggleRadioAllocator(chkName, sender, actorsId, disableSelect, isSelectedActor, Selected) {
     if (isSelectedActor=='True')
         Count = Selected;
    onToggleAllocator(chkName, sender, disableSelect);  
    displayBlockActors(actorsId, sender);
    $("input[name='activityOption']").next("span").removeClass('spanFocus');
    $(sender).next("span").addClass('spanFocus'); // 突出显示被选中
}
//function toggleCheckBoxAllocator(chkName, sender, actorsId, disableSelect){ 
function toggleCheckBoxAllocator(chkName, sender, actorsId, disableSelect, isSelectedActor, Selected) { 
    onToggleAllocator(chkName, sender, disableSelect);
    var isChecked = sender.checked;  
    // 设置移除样式
    if(isChecked == false){
        $("#" + actorsId + " input:checkbox").each(function(){
            $(this).next("span").removeClass("spanFocus");
        });
    }
    // 当前显示 Actor 数等于 isCheckLimit 时，设置 checkbox 为选中状态.
    var isCheckLimit = ($("#" + actorsId + " input[name='activityAllocatee']").length <= checkLimit);
    var thisCheckLimit = checkLimit;
    if (isSelectedActor == 'True')//当有步骤限制设置时
    {
        thisCheckLimit = Selected;
    }
    // 当前显示用户数目等于 checkLimit 时，设置 checkbox 为选中状态.
    var selectedCount = 0;
    $("#" + actorsId + " input[name='activityAllocatee']").each(function () {
        this.checked = isChecked;
        if (isChecked) {
            //if(isCheckLimit){
            if (isSelectedActor == 'True' && (selectedCount < thisCheckLimit || thisCheckLimit == -1)) {
                $(this).next("span").addClass('spanFocus'); // 突出显示被选中
            }
            else if (isCheckLimit) {//没有有步骤限制设置时则按默认的选中
                $(this).next("span").addClass('spanFocus');
            }
        }
        else {
            $(this).next("span").removeClass('spanFocus');
        }
        selectedCount++;
    });
    
    // 抄送设置.
    var isAllChecked = (($(sender).attr("reviewAllChecked") == null?  true : $(sender).attr("reviewAllChecked") == "True"));
    isAllChecked = (isChecked == true? isAllChecked : isChecked);
    $("#" + actorsId + " input[name='reviewActors_values']").each(function () {
        this.checked = isAllChecked;
   });
}
function onToggleAllocator(chkName, sender, disableSelect) {
    return false;
    if (disableSelect){
        alert("不能修改默认的选择项");
        sender.checked = true;
    } else {
        var inputArray = document.getElementsByTagName("input");
	    for(var i=0; i<inputArray.length; i++) {
	        if(sender.type == "checkbox"){
		        if (inputArray[i].type == "checkbox" && inputArray[i].id.indexOf(chkName) != -1) {
			        inputArray[i].checked = false;
		        }
	        }
	        else {
	            if (inputArray[i].type == "checkbox" && inputArray[i].name.indexOf(chkName) != -1) {
			        inputArray[i].checked = false;
		        }
	        }
	    }
    }
};

// (分派)检查是否选中用户所属于的步骤选项
function onPreSelectAllocatee(optionId, sender, disableSelect){
    if (disableSelect){
        alert("不能修改默认的选择项");
        sender.checked = true;
    } else {
        var parentInput = document.getElementById(optionId);
        if(parentInput != null && parentInput.checked == false){
            sender.checked = false;
        }
    }
};
function displayBlockActors(displayId, sender){    
    $(".blockActors").css("display", "none"); // 隐藏全部 Actor 显示区域.
    $("#" + displayId).css("display", ""); 
    // 清除其他步骤的被选中项.
    $("#divActors input[name='activityAllocatee']").each(function(){
        this.checked = false;
        $(this).next("span").removeClass('spanFocus');
    }); 
    $("#divActors input[name='reviewActors_values']").each(function(){
        this.checked = false;
    });

    // 当前显示用户数目等于 checkLimit 时，设置 checkbox 为选中状态.
    var selectedCount = 0;
    $("#" + displayId + " input[name='activityAllocatee']").each(function() {
    if (selectedCount < Count || Count == -1) {
            this.checked = true;
            $(this).next("span").addClass('spanFocus');
        }
        selectedCount++;
    });
    
    if($("#" + displayId + " input[name='activityAllocatee']").length <=  checkLimit){
        $("#" + displayId + " input[name='activityAllocatee']").each(function() {
            if (Count == -2) {
                this.checked = true;
                $(this).next("span").addClass('spanFocus');
            }
        });
    }
    
    // 抄送的显示.
    var isAllChecked =  ($(sender).attr("reviewAllChecked") == null?  true : $(sender).attr("reviewAllChecked").toLowerCase() == "true");
    $("#" + displayId + " input[name='reviewActors_values']").each(function(){
        this.checked = isAllChecked;
   });
};
$(function(){
    $("#divActors input:checkbox").click(function(){
        if(this.checked){
            $(this).next("span").addClass("spanFocus"); // 突出显示被选中
        }
        else{
            $(this).next("span").removeClass("spanFocus");
        }
	});
});
function isInArray(arr, el){
	for (var i = 0, icount = arr.length; i < icount; i++){
		if (arr[i] == el){
			return true;
		}
	}
	return false;
}
function parseExpression(expression, allNodes, selectedNodes){
	var n = 0, m = 0;
	var defNodes = [];
	var re = /^\d+(,\d*)?$/;

	var items = expression.split('+');
	for (var i = 0, icount = items.length; i < icount; i++){
		var el = items[i].replace(/^\s+|\s+$/g, "");
		if (el.length > 0){
			if (re.test(el)){
				var ss = el.split(',');
				n = parseInt(ss[0]);
				if (ss.length >= 2 && ss[1].length > 0){
					m = parseInt(ss[1]);
				} 
			} else {
				if (!isInArray(defNodes, el)){
					defNodes.push(el);
				}
			}
		}
	}
	
	if (defNodes.length > 0){
		var countOfDefNodes = defNodes.length;
		for (var i = 0; i < countOfDefNodes; i++){
			if (!isInArray(selectedNodes, defNodes[i])){
				return false;
			}
		}
		n += countOfDefNodes;
		if (m > 0){
			m += countOfDefNodes;
		}
	}

	var countOfSelectedNodes = selectedNodes.length;
	if (countOfSelectedNodes < n){
		alert("请选择至少" + n + "个步骤");
		return false;
	}
	if (m > 0 && countOfSelectedNodes > m){
		alert("请选择至多" + m + "个步骤");
		return false;
	}
	return true;
}
function checkSelection(){
    var condition = "<%=SplitCondition %>";
    var endActivityId = "<%=EndActivityId %>"; // 最后一步流程步骤

    var allItems = document.getElementsByName("activityOption");
	if (!allItems){
		return true;
	}

	var allCount = allItems.length;
	var selectedNodes = [];
	var allNodes = [];
	for (var i = 0; i < allCount; i++){
		if (allItems[i].checked){
			selectedNodes.push(allItems[i].value);
		}
		allNodes.push(allItems[i].value);
	}
	
	var selectedCount = selectedNodes.length;
	if(endActivityId == ""){
	    if (null != condition && condition.length > 0){
		    if (condition == "all"){
			    if (selectedCount != allCount){
				    alert("由于并行，工单的所有下行路径都需要选择");
				    return false;
			    }
		    } else {
				var isValid = parseExpression(condition, allNodes, selectedNodes);
				if (isValid){
					return true;
				} else {
					return false;
				}				
			}
	    } else {
		    if (selectedCount != 1){
			    alert("请选择工单的下行路径(单选)");
			    return false;
		    }
	    }
	}
	return true;
}

$(function(){
     $("input[name='activityOption']").each(function(){
        if(this.checked){
            var isAllChecked = ($(this).attr("reviewAllChecked") == null?  true : $(this).attr("reviewAllChecked") == "True");
            var type = ($("input[name='reviewActors_values']").length == 0? "": $("input[name='reviewActors_values']").attr("type"));
            if(type != null && type.toLowerCase() == "checkbox"){
                $("input[name='reviewActors_values']").each(function(){
                    $(this).removeAttr("checked");
                    if(isAllChecked){ $(this).attr("checked", "checked"); }
                });
            }
        }
    });
});
function onValidateReviewActors(){
    var isValid = true;
    $("input[name='activityOption']").each(function(){
        if(isValid && this.checked){
            if($(this).attr("isReview") != null && $(this).attr("isReview") == "True"){
                var count = ($(this).attr("reviewActorCount") == null? -1 : $(this).attr("reviewActorCount"));
                isValid = validateReviewActors(count);
            }
        }
    });
    return isValid;
}
function validateReviewActors(max){
    if(max<=-1 || $("input[name='reviewActors_values']").length == 0){ 
        return true; 
    }
    var type =  $("input[name='reviewActors_values']").attr("type");
    var values = "";
    if(type != null && type == "checkbox"){
        $("input[name='reviewActors_values']").each(function(){
            if(this.checked){ values+=(values == "" ? ($(this).val()) : ("," + $(this).val())); }
        });
    }else{
        values = $("input[name='reviewActors_values']").val();
    }
    var count = (values == ""? 0 : values.split(",").length);
    
    if(count > max){
        alert("错误：选择的抄送人过多，抄送人数不能超过 " + max + " 人。请重新选择！");
        return false;
    }
    return true;
}
// ]]>
</script>
