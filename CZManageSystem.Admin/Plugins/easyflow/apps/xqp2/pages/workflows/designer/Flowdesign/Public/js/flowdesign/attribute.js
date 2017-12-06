/*确定保存时调用的方式*/
function saveAttribute(data)
{
    try{
        if(data.status!=1)
        {
            alert(data.msg);//失败时不关闭 attributeModal 所以用 alert
        }else
        {
            $("#attributeModal").modal("hide");
            mAlert(data.msg);
            //刷新加载样式，体验不太好 万一未保存设计
            //location.reload();
        }
    }catch(e)
    {
        alert(data.msg);
    }
}

//-----条件设置--strat----------------
    function _id(id) {
        return !id ? null : document.getElementById(id);
    }
    function trim(str) {
        return (str + '').replace(/(\s+)$/g, '').replace(/^\s+/g, '');
    }

    function fnCheckExp(text){
        //检查公式
        if( text.indexOf("(")>=0 ){
            var num1 = text.split("(").length;
            var num2 = text.split(")").length;
            if( num1!=num2 ) {
                return false;
            }
        }
        return true;
    }
    /**
     * 增加左括号表达式，会断行
     */
    function fnAddLeftParenthesis(id){
        var oObj = _id('conList_' + id);
        var current = 0;
        if(oObj.options.length>0){ //检查是否有条件
            for ( var i = 0;i < oObj.options.length;i++ ){
                if( oObj.options[i].selected ) {
                    current = oObj.selectedIndex;
                    break;
                }
            }
            if(current==0){
                current = oObj.options.length-1;
            }
        } else { //有条件才能添加左括号表达式
            alert("请先添加条件，再选择括号");
            return;
        }
        var sText = oObj.options[current].text,sValue = oObj.options[current].value;
        //已经有条件的话
        if( (trim(sValue).substr(-3,3) == 'AND') || (trim(sValue).substr(-2,2) == 'OR') ){
            alert("无法编辑已经存在关系的条件");
            return;
        }
        var sRelation = _id('relation_'+id).value;
        if( sValue.indexOf('(')>=0 ){
            if( !fnCheckExp(sValue) ){
                alert("条件表达式书写错误,请检查括号匹配");
                return;
            } else {
                sValue = sValue + " " + sRelation;
                sText = sText + " " + sRelation;
            }
        } else {
            sValue = sValue + " " + sRelation;
            sText = sText + " " + sRelation;
        }
        oObj.options[current].value = sValue;
        oObj.options[current].text = sText;
       // $('#conList_'+id+' option').eq(current).text(sText)
       $('#conList_'+id).append('<option value="( ">( </option>');

       /* var oMyop = document.createElement('option');
        oMyop.text = "( ";
        var nPos = oObj.options.length;
        oObj.appendChild(oMyop,nPos);*/

    }
    /**
     * 增加右括号表达式
     */
    function fnAddRightParenthesis(id){
        var oObj = _id('conList_' + id);
        var current = 0;
        if( oObj.options.length>0 ){
            for ( var i = 0;i < oObj.options.length;i++ ){
                if( oObj.options[i].selected ) {
                    current = oObj.selectedIndex;
                    break;
                }
            }
            if( current == 0 ){
                current = oObj.options.length-1;
            }
        } else {
            alert("请先添加条件，再选择括号");
            return;
        }
        var sText = oObj.options[current].text,sValue = oObj.options[current].value;
        if( (trim(sValue).substr(-3,3)=='AND') || (trim(sValue).substr(-2,2)=='OR') ){
            alert("无法编辑已经存在关系的条件");
            return;
        }
        if( (trim(sValue).length==1) ){
            alert("请添加条件");
            return;
        }
        if( !fnCheckExp(sValue) ){
            sValue = sValue + ")";
            sText = sText + ")";
        }
        oObj.options[current].value = sValue;
        oObj.options[current].text = sText;

    }
    /*function fnAddConditions(id){
        var sField = $('#field_'+id).val(),sField_text = $('#field_'+id).find('option:selected').text(),sCon = $('#condition_'+id).val(),sValue = $('#item_value_'+id).val();

        var bAdd = true;
        if( sField!=='' && sCon!=='' && sValue!=='' ){
            var oObj = _id('conList_'+id);

            if( oObj.length>0 ){
                var sLength = oObj.options.length;
                var sText = oObj.options[sLength-1].text;
                if(!fnCheckExp(sText)){
                    bAdd = false;
                }
            }
            if( sValue.indexOf("'")>=0 ){
                alert("值中不能含有'号");
                return;
            }
            var sNewText = "'" + sField + "' " + sCon + " '" + sValue + "'";
            var sNewText_text = "'" + sField_text + "' " + sCon + " '" + sValue + "'";
            for( var i=0;i<oObj.options.length;i++ ){
                if( oObj.options[i].value.indexOf(sNewText)>=0 ){
                    alert("条件重复");
                    return;
                }
            }
            
            var sRelation = $('#relation_'+id).val();

            if( bAdd ){
                //var oMyop = document.createElement('option');
                var nPos = oObj.options.length;
                //oMyop.text = sNewText_text;
               // oMyop.value = sNewText;
                //oObj.appendChild(oMyop,nPos);
                $('#conList_'+id).append('<option value="'+sNewText+'">'+sNewText_text+'</option>');
                if( nPos>0 ){
                    oObj.options[nPos-1].text += "  " + sRelation;
                    oObj.options[nPos-1].value += "  " + sRelation;
                }
            } else {

                if( trim(oObj.options[sLength-1].text).length==1 ){
                    oObj.options[sLength-1].text += sNewText_text;
                    oObj.options[sLength-1].value += sNewText;
                } else {
                    oObj.options[sLength-1].text += " " + sRelation + " " + sNewText_text;
                    oObj.options[sLength-1].value += " " + sRelation + " " + sNewText;
                }
            }
        } else {
            alert("请补充完整条件");
            return;
        }
    }*/
    function fnDelCon(id){
        var oObj = _id('conList_'+id);
        var maxOpt = oObj.options.length;
        if(maxOpt<0) maxOpt = 0;

        for (var i = 0;i < oObj.options.length;i++ ){
            if( oObj.options[i].selected ) {
                if((i+1)==maxOpt){
                    if(typeof oObj.options[i-1] !== 'undefined'){
                        oObj.options[i-1].text = oObj.options[i-1].text.replace(/(AND|OR)$/,'');
                        oObj.options[i-1].value = oObj.options[i-1].value.replace(/(AND|OR)$/,'');
                    }
                }
                oObj.removeChild(oObj.options[i]);
                i--;
            }
        }
    }
    function fnClearCon(id){
        $('#conList_' + id).html('');
    }



    //根据基本信息的下一步骤，设置《条件设置》tab的条件列表
    function fnSetCondition(){
       
        var relation = document.getElementById("ddlrelation");
        $("#ddlrelation").change(function () {
            var discription = document.getElementById("txtDescription");
            var rule = document.getElementById("txtCommandRules");
            if ($(this).val() == "") {
                discription.value += "";
                rules.value += "";
            }
            else {
                discription.value += "" + relation.options[relation.selectedIndex].text + " ";
                rule.value += "" + $(this).val() + " ";
            }
        });

        $("#btnClear").click(function () {
            $("#txtDescription").attr("value", "");
            $("#txtCommandRules").attr("value", "");
            $("#ddlrelation").attr("value", "");
        });

        $("#btn_Add").click(function () { 
            var discription = document.getElementById("txtDescription");
            var rule = document.getElementById("txtCommandRules");
            if ($("#drdlNextActivity").val() == "") {
                alert("请选择下行步骤！");
                //return false;
            }
            else if (discription.value == "") {
                alert("规则描述不能为空！");
                //return false;
            }
            else if (rule.value == "") {
                alert("请填写正确的规则！");
                //return false;
            }
            else {
               
                var activityname = $("#txtActivityName").val();
                var nextactivityname = $("#drdlNextActivity").val();
                var title = $("#workflowProfileModal_ltlWorkflowName").val() + "-" + activityname + "→" + nextactivityname;
                var aid = $("#process_id").val();
                var rulesJson = $("#activity" + aid).attr("rules");
                var FieldsAssemble = "";
                 
                $("#drdlFName option").each(function () {
                    var fName = $(this).val();
                   var re = new RegExp(fName, "gi"); //完全匹配所有，忽略大小写
                    if (re.test(rule.value)&&fName!="") {
                        FieldsAssemble += "_" + fName + "_;";
                    }
                });
                var b = new Base64();
                rulesJson = b.decode(rulesJson);
                var rules = $.evalJSON(rulesJson);
                var has = false;
                for (var i = 0; i < rules.length; i++) {
                    if (activityname == rules[i].activityName && nextactivityname == rules[i].nextactivityName) {
                        has = true;
                        rules[i].title = title;
                        rules[i].condition = rule.value;
                        rules[i].description = discription.value;
                        rules[i].FieldsAssemble = FieldsAssemble;
                    }
                }
                if (!has) {
                    rules.push({ activityName: activityname, nextactivityName: nextactivityname, title: title, conditions: rule.value, description: discription.value, FieldsAssemble: FieldsAssemble });
                }
                $("#listResults tbody").empty();
                for (var i = 0; i < rules.length; i++) {
                    $("#listResults tbody").append("<tr style=\"text-align:center;\">"
    				            + "<td style=\"text-align:left;\">" + rules[i].title + "</td>"
    				            + "<td style=\"text-align:left;\">" + rules[i].description + " </td>"
    				            + "<td style=\"text-align:left;\">" + rules[i].conditions + "</td>"
                                + "<td style=\"text-align:left;\">" + rules[i].FieldsAssemble + "</td>"
    				            + "<td>" + rules[i].nextactivityName + "</td>"
    				            + "<td>"
                                    + "<button type=\"button\" name=\"btnEdit\" class=\"btn btn-small\">编辑</button>"
                                    + "<button type=\"button\" name=\"btnDel\" class=\"btn btn-small\">删除</button>"
    				            + "</td>"
    				        + "</tr>");
                }
                rulesJson = $.toJSON(rules);
                rulesJson = b.encode(rulesJson);
                $("#activity" + aid).attr("rules", rulesJson);
            }
        });
        $("[name='btnDel']").click(function () {
            if (confirm("确认删除此规则？")) {
                var $tr = $(this).parent("td").parent();
                $tr.remove();
            }
        })
        $("[name='btnEdit']").click(function () {
            var $tr = $(this).parent("td").parent();
            var $td = $tr.children('td');
            $("#txtDescription").attr("value", $td.eq(1).html());
            $("#txtCommandRules").attr("value", $td.eq(2).html());
            $("#drdlNextActivity").val($td.eq(4).html());
        })
        $("#txtFieldsAssemble").blur(function () {
            var fields = $(this).val();
            if (fields != "") {
                var arr = fields.split(';');
                $("#drdlFName option[type='field']").remove();
                for (var i = 0; i < arr.length; i++) {
                    if(arr[i]!="")
                        $("#drdlFName").append("<option type=\"field\" value=\"" + arr[i] + "\">" + arr[i] + "</option>");
                }
            }
        });
    }

    function fnAddConditions(aid) {
        var fName = document.getElementById("drdlFName");
        var condition = document.getElementById("drdlCondition");
        var txtval = document.getElementById("txtVal");
        var relation = document.getElementById("ddlrelation");
        var discription = document.getElementById("txtDescription");
        var rules = document.getElementById("txtCommandRules");
        if (fName.value == "" || fName.value == null || txtval.value == "" || txtval.value == null) {
            alert("请填写比较条件！");
            txtval.focus();
            return false;
        }
        else {
            var relationShip = relation.options[relation.selectedIndex].value == "" ? "" : relation.options[relation.selectedIndex].text;
            var conditiontext = condition.options[condition.selectedIndex].text;
            var conditionvalue = condition.options[condition.selectedIndex].value;
            discription.value += fName.options[fName.selectedIndex].text + " " + condition.options[condition.selectedIndex].text + " '" + txtval.value + "' ";
            if (conditionvalue == "startwith")
                rules.value += fName.options[fName.selectedIndex].value + " LIKE '" + txtval.value + "%' ";
            else if (conditionvalue == "endwith")
                rules.value += fName.options[fName.selectedIndex].value + " LIKE '%" + txtval.value + "' ";
            else if (conditionvalue == "like")
                rules.value += fName.options[fName.selectedIndex].value + " LIKE '%" + txtval.value + "%' ";
            else if (conditionvalue == "notlike")
                rules.value += fName.options[fName.selectedIndex].value + " NOT LIKE '%" + txtval.value + "%' ";
            else
                rules.value += fName.options[fName.selectedIndex].value + " " + condition.options[condition.selectedIndex].value + " '" + txtval.value + "' ";
        }
        return true;
    }
    
//-----条件设置--end----------------
    //-----常规设置--begin----------------
    function fnInitBasic(aid) {
        var $activityDiv = $("#activity" + aid);
    }

    function fnSetPower(aid) {
        var aname = $("#txtActivityName").val();
        if (aname == "") {
            aname = $("#activity" + aid).attr("activityname");
            $("#txtActivityName").val(aname);
        }
        $("#chkField").click(function () {
            if (this.checked == true) {
                $("#trFields").css("display", "");
            }
            else {
                $("#trFields").css("display", "none");
            }
        });
        $("#chkUsers").click(function () {
            if (this.checked == true) {
                $("#chkUsersAssign").attr("checked", "checked");
                $("#trUsers").css("display", "");
                $("#trUsersAssign").css("display", "");
            }
            else {
                $("#txtUsers").val("");
                $("#trUsers").css("display", "none");
            }
        });
        $("#chkUsersAssign").click(function () {
            if (this.checked == true) {
                $("#trUsersAssign").css("display", "");
            }
            else {
                $("#txtUsersAssign").val("");
                $("#trUsersAssign").css("display", "none");
            }
        });
        $("#chkOrg").click(function () {
            if (this.checked == true) {
                $("#chkOrgAssign").attr("checked", "checked");
                //$("#chkStarterAssign").attr("checked", "checked");
                $("#trOrg").css("display", "");
                $("#trOrgAssign").css("display", "");
            }
            else {
                $("#trOrg").css("display", "none");
                $("input[name='chkOrgArgs']").removeAttr("checked");
            }
        });
        $("#chkOrgAssign").click(function () {
            if (this.checked == true) {
                $("#trOrgAssign").css("display", "");
            }
            else {
                $("#trOrgAssign").css("display", "none");
                $("input[name='chkOrgArgsAssign']").removeAttr("checked");
            }
        });
        $("#chkRole").click(function () {
            if (this.checked == true) {
                $("#chkRoleAssign").attr("checked", "checked");
                $("#trRole").css("display", "");
                $("#trRoleAssign").css("display", "");
            }
            else {
                $("#trRole").css("display", "none");
                $("input[name='chkRoleArgs']").removeAttr("checked");
            }
        });
        $("#chkRoleAssign").click(function () {
            if (this.checked == true) {
                $("#chkRoleAssign").css("display", "");
                $("#trRoleAssign").css("display", "");
            }
            else {
                $("#trRoleAssign").css("display", "none");
                $("input[name='chkRoleArgsAssign']").removeAttr("checked");
            }
        });
        $("#chkRes").click(function () {
            if (this.checked == true) {
                $("#chkResAssign").attr("checked", "checked");
            }
        });
        $("#chkPssctl").click(function () {
            if (this.checked == true) {
                $("#chkPssctlAssign").attr("checked", "checked");
                $("#orgPssor").css("display", "");
                $("#chkOrg").attr("checked", "checked");
                $("#orgPssorAssign").css("display", "");
                $("#chkOrgAssign").attr("checked", "checked");
                //$("#chkStarterAssign").attr("checked", "checked");
                $("#trOrg").css("display", "");
                $("#trOrgAssign").css("display", "");
            }
            else {
                $("#orgPssor").css("display", "none");
                $("#drdlPssor").get(0).selectedIndex = 0;
            }
        });
        $("#chkPssctlAssign").click(function () {
            if (this.checked == true) {
                $("#orgPssorAssign").css("display", "");
                $("#chkOrgAssign").attr("checked", "checked");
                $("#trOrgAssign").css("display", "");
            }
            else {
                $("#orgPssorAssign").css("display", "none");
                $("#drdlPssorAssign").get(0).selectedIndex = 0;
            }
        });
        $("#chkPssor").click(function () {
            if (this.checked == true) {
                $("#chkPssorAssign").attr("checked", "checked");
            }
        });
        $("#chkStarter").click(function () {
            if (this.checked == true) {
                $("#chkStarterAssign").attr("checked", "checked");
            }
        });
        $("#chkControl").click(function () {
            if (this.checked == true) {
                $("#orgType").css("display", "");
                $("#orgTypeAssign").css("display", "");
                $("#chkControlAssign").attr("checked", "checked");
            }
            else {
                $("#orgType").css("display", "none");
                $("#drdlActivities").get(0).selectedIndex = 0;
            }
        });
        $("#chkControlAssign").click(function () {
            if (this.checked == true) {
                $("#orgTypeAssign").css("display", "");
            }
            else {
                $("#orgTypeAssign").css("display", "none");
                $("#drdlActivitiesAssign").get(0).selectedIndex = 0;
            }
        });
        $("#ddlrejectOption").change(function () {
            if ($(this).val() == "customize")
                $(".customize").show();
            else
                $(".customize").hide();
        });
    }

    function openUserSelector(appPath,inputId) {
        var h = 450;
        var w = 700;
        var iTop = (window.screen.availHeight - 30 - h) / 2;
        var iLeft = (window.screen.availWidth - 10 - w) / 2;
        window.open(appPath+'contrib/security/pages/PopupUserPicker.aspx?inputid=' + inputId, '', 'height=' + h + ', width=' + w + ', top=' + iTop + ', left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');
        return false;
    }
    $(function () {
        //TAB
        $('#attributeTab a').click(function (e) {
            e.preventDefault();
            $(this).tab('show');
            if ($(this).attr("href") == '#attrJudge') {
                //加载下一步数据 处理 决策项目 
            }
            $("#attributeModal .tab-pane").removeClass("active");
            $("#attributeModal .tab-pane").hide();
            var id = $(this).attr("href");
            $(id).addClass("active");
            $(id).show();
            /*var url = $(this).attr("url");
            var aid = $("#process_id").val();
            var flowid = $("#flow_id").val();
            var attributeModal = $("#attributeModal");
            attributeModal.find(".tab-content").html('<img src="../../images/loading.gif"/>');
            $.ajax({
            type: "post",
            dataType: "html",
            url: url,
            data: {wid:flowid,aid:aid},
            async: true,
            timeout: 300000,
            success: function (data) {
            attributeModal.find(".tab-content").html(data);
            },
            error: function () {
            },
            complete: function () {
            }

            });*/
        })

        //步骤类型
        $('input[name="process_type"]').on('click', function () {

            if ($(this).val() == 'is_child') {
                $('#current_flow').hide();
                $('#child_flow').show();
            } else {
                $('#current_flow').show();
                $('#child_flow').hide();
            }
        });
        //返回步骤
        $('input[name="child_after"]').on('click', function () {

            if ($(this).val() == 2) {
                $("#child_back_id").show();
            } else {
                $("#child_back_id").hide();
            }
        });

        //步骤select 2
        $('#process_multiple').multiselect2side({
            selectedPosition: 'left',
            moveOptions: true,
            labelTop: '最顶',
            labelBottom: '最底',
            labelUp: '上移',
            labelDown: '下移',
            labelSort: '排序',
            labelsx: '<i class="icon-ok"></i> 下一步步骤',
            labeldx: '<i class="icon-list"></i> 备选步骤',
            autoSort: false,
            autoSortAvailable: true,
            minSize: 7
        });



        //选人方式
        $("#auto_person_id").on('change', function () {
            var apid = $(this).val();
            if (apid > 0) {
                $('#auto_unlock_id').show();
            } else {
                $('#auto_unlock_id').hide();
            }
            if (apid == 4)//指定用户
            {
                $("#auto_person_4").show();
            } else {
                $("#auto_person_4").hide();
            }
            if (apid == 5)//指定角色
            {
                $("#auto_person_5").show();
            } else {
                $("#auto_person_5").hide();
            }


        });




        /*---------表单字段 start---------*/
        //可写字段
        function write_click(e) {
            var id = $(e).attr('key');
            if (!$(e).attr('disabled')) {
                if ($(e).attr('checked')) {
                    $('#secret_' + id).attr({ 'disabled': true, 'checked': false });
                } else {
                    $('#secret_' + id).removeAttr('disabled').attr('checked', false);
                }
            }
        }
        //保密字段
        function secret_click(e) {
            var id = $(e).attr('key');

            if (!$(e).attr('disabled')) {
                if ($(e).attr('checked')) {
                    $('#write_' + id).attr({ 'disabled': true, 'checked': false });
                } else {
                    $('#write_' + id).removeAttr('disabled').attr('checked', false);
                }
            }
        }
        //checkbox全选及反选操作
        function icheck(ac, op) {
            if (ac == 'write') {
                $("input[name='write_fields[]']").each(function () {
                    if (this.disabled !== true) {
                        this.checked = op;
                    }
                    write_click(this);
                })
            } else if (ac == 'secret') {
                $("input[name='secret_fields[]']").each(function () {
                    if (this.disabled !== true) {
                        this.checked = op;
                    }
                    secret_click(this);
                })
            }
        }

        $('#write').click(function () {
            if ($(this).attr('checked')) {
                icheck('write', true);
                $('#secret').attr({ 'disabled': true, 'checked': false });
                $('#check').attr('checked', false).removeAttr('disabled');
            } else {
                icheck('write', false);
                $('#secret').attr('checked', false).removeAttr('disabled');
                $('#check').attr({ 'disabled': true, 'checked': false });
            }
        })
        $('#secret').click(function () {
            if ($(this).attr('checked')) {
                icheck('secret', true)
                $('#write').attr({ 'disabled': true, 'checked': false });
            } else {
                icheck('secret', false);
                $('#write').attr('checked', false).removeAttr('disabled');
            }
        })

        $("input[name='write_fields[]']").click(function () {
            write_click(this);
            $('#write').removeAttr('disabled');
            if ($('#write').attr('checked') == true) {
                $('#write').attr('checked', false)
            }
        })
        $("input[name='secret_fields[]']").click(function () {
            secret_click(this);
            $('#secret').removeAttr('disabled');
            if ($('#secret').attr('checked') == true) {
                $('#secret').attr('checked', false)
            }
        })
        /*---------表单字段 end---------*/

        /*样式*/
        $('.colors li').click(function () {
            var self = $(this);
            if (!self.hasClass('active')) {
                self.siblings().removeClass('active');
            }
            var color = self.attr('org-data') ? self.attr('org-data') : '';


            var parentDiv = self.parents(".colors");
            var orgBind = parentDiv.attr("org-bind");
            if (orgBind == 'style_icon') {
                /*$("#"+orgBind).css({ color:'#fff',background: color });*/
                $("#" + orgBind).val(color);
                $("#style_icon_preview").attr("class", color + " icon-white");
            } else//颜色
            {
                $("#" + orgBind).css({ color: '#fff', background: color });
                $("#" + orgBind).val(color);
            }
            self.addClass('active');
        });




        //表单提交前检测
        $("#flow_attribute").submit(function () {
            //条件检测
            var cond_data = $("#process_condition").val();
            if (cond_data !== '') {
                var pcarr = cond_data.split(',');
                for (var i = 0; i < pcarr.length; i++) {
                    if (pcarr[i] !== '') {
                        var obj = _id('conList_' + pcarr[i]);
                        if (obj.length > 0) {
                            var constr = '';
                            for (var j = 0; j < obj.options.length; j++) {
                                constr += obj.options[j].value + '@leipi@';
                                if (!fnCheckExp(constr)) {
                                    alert("条件表达式书写错误,请检查括号匹配");
                                    $('#condition').click();
                                    return false;
                                }
                            }
                            _id('process_in_set_' + pcarr[i]).value = constr;
                        } else {
                            _id('process_in_set_' + pcarr[i]).value = '';
                        }
                    }
                }
            }

        });

        /*-------------处理人设置--------------*/
        fnSetPower();
        /*------------处理人设置end--------*/
        //条件设置
        fnSetCondition();

    });