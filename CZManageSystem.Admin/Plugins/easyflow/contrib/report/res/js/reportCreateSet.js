 
    // [列]获取焦点
    function ColumnFocus(ctrl)
    {        
        if ($('#ctl00_cphBody_ddlSource').find("option:selected").val() == "" || $('#ctl00_cphBody_ddlSource').val() == undefined)
        {
            $(ctrl).empty(); 
            $(ctrl).append("请选择数据源");
        }
        else
        {
            var selectedText = $.trim($(ctrl).text());//保存已选择的值
            $(ctrl).empty(); 
            $(ctrl).append( $("#ctl00_cphBody_column_list").html());//添加下拉框
            // 还原上次下拉框选择值
            $(ctrl).find("select option").each(function(){
                if ($(this).text() == selectedText)
                    $(this).attr("selected", "true");
            });
        }
        $(ctrl).find("select").focus();
        $(ctrl).find("select").blur(function(){
            $(this).replaceWith($(this).find("option:selected").text());//替换下拉框为选择值
        });
    }
    
    // [条件]列获取焦点
    function ConditionFocus(ctrl)
    {
        if (!CheckColumnSelected(ctrl))  //判断是否已选择显示字段
            return;
        if ($(ctrl).find("input[value='OK']").length != 0)  //根据是否有ok按钮判断条件是否已是编辑模式
            return;
        var selectStr = "<select>"
                + "<option value='-1'>请选择类型</option>"
                + "<option value='1'>空处理</option>"
                + "<option value='2'>字符比较</option>"
                + "<option value='3'>数字比较</option>"
                + "<option value='4'>日期比较</option>" 
                + "<option value='5'>内容匹配</option>"
                + "</select>";
            $(ctrl).empty();
            $(ctrl).append(selectStr);
            $(ctrl).find("select").focus();
            $(ctrl).find("select").blur(function(){
                if ($(this).find("option:selected").text() == "请选择类型")
                    $(this).replaceWith("");
            });
            $(ctrl).find("select").change(function(){
                $(this).siblings().remove();
                var okStr = "<input type='button' value='OK' onclick='GetCondition(this)'/>";
                $(this).after(okStr);
                switch($(this).find("option:selected").text())
                {
                    case "请选择类型":
                        $(this).siblings().remove();
                        $(this).blur(function(){
                            $(this).replaceWith("");
                        });
                        break;
                    case "空处理":
                        var nullSelectStr = "<select>"
                        + "<option value='is null'>为空</option>"
                        + "<option value='is not null'>不为空</option>"
                        + "</select>";
                        $(this).after(nullSelectStr);
                        break;
                    case "字符比较":
                    case "日期比较":
                        var controlStr = "<select>"
                        + "<option value='='>等于</option>"
                        + "<option value='<>'>不等于</option>"
                        + "<option value='<'>小于</option>"
                        + "<option value='<='>小于等于</option>"
                        + "<option value='>'>大于</option>"
                        + "<option value='>='>大于等于</option>"
                        + "</select>";
                        var txtStr = "<input type='text' />";
                        $(this).after(txtStr);
                        $(this).after(controlStr);
                        break;
                    case "内容匹配":
                        var controlStr = "<select>"
                        + "<option value='1'>包含</option>"
                        + "<option value='2'>不包含</option>"
                        + "<option value='3'>匹配开头</option>"
                        + "<option value='4'>不匹配开头</option>"
                        + "<option value='5'>匹配结尾</option>"
                        + "<option value='6'>不匹配结尾</option>"
                        + "</select>";
                        var txtStr = "<input type='text' />";
                        $(this).after(txtStr);
                        $(this).after(controlStr);
                        break;
                    case "数字比较":
                        var controlStr = "<select>"
                        + "<option value='='>等于</option>"
                        + "<option value='<>'>不等于</option>"
                        + "<option value='<'>小于</option>"
                        + "<option value='<='>小于等于</option>"
                        + "<option value='>'>大于</option>"
                        + "<option value='>='>大于等于</option>"
                        + "</select>";
                        var txtStr = "<input type='text' />";
                        $(this).after(txtStr);
                        $(this).after(controlStr);
                        break;
                }
            });
    }
    
    // 获取设定的条件
    function GetCondition(ctrl)
    {
        var selectType = $(ctrl.parentNode).find("select").eq(0).find("option:selected").text();
        var value;
        switch(selectType)
        {
            case "空处理":  //空处理
                var tdObj = $(ctrl.parentNode);
                value = tdObj.find("select").get(1).value;
                tdObj.empty();
                tdObj.append(value);
                break;
            case "字符比较":  //字符比较 
            case "日期比较":  //日期比较
                var tdObj = $(ctrl.parentNode);
                value = tdObj.find("select").get(1).value;
                inputStr = tdObj.find("input").val();
                tdObj.empty();
                tdObj.append(value + " '" + inputStr + "'");
                break;
            case "内容匹配":  //内容匹配 
                var likecondiiton = GetLikeCondition(ctrl.parentNode);
                var tdObj = $(ctrl.parentNode);
                tdObj.empty();
                tdObj.append(likecondiiton);
                break;
            case "数字比较":
                var tdObj = $(ctrl.parentNode);
                value = tdObj.find("select").get(1).value;
                inputStr = tdObj.find("input").val();
                tdObj.empty();
                tdObj.append(value + inputStr);
                break;
        }
    }
    
    //获取匹配条件
    function GetLikeCondition(ctrl)
    {
        var result = '';
        var value = $(ctrl).find("select").get(1).value;
        var inputStr = $(ctrl).find("input").val();
        switch(value)
        {
            case "1":  // 包含
                result = "LIKE '%" + inputStr + "%'"
                break;
            case "2":  // 不包含
                result = "NOT LIKE '%" +inputStr + "%'"
                break;
            case "3":  // 匹配开头
                result = "LIKE '" +inputStr + "%'"
                break;
            case "4":  // 不匹配开头
                result = "NOT LIKE '" +inputStr + "%'"
                break;
            case "5":  // 匹配结尾
                result = "LIKE '%" + inputStr + "'"
                break;
            case "6":  // 不匹配结尾
                result = "NOT LIKE '%" + inputStr + "'"
                break;
        }
        return result;
    }
    function GroupFocus(ctrl)
    {
        if (!CheckColumnSelected(ctrl))
            return;
        var txt = $(ctrl).text();
        var selectStr = "<select>"
                + "<option value='-1'>请选择类型</option>"
                + "<option value='GroupBy'>分组</option>"
                + "<option value='Min'>Min</option>"
                + "<option value='Max'>Max</option>"
                + "<option value='Count'>Count</option>" 
                + "<option value='Sum'>Sum</option>"
                + "<option value='Avg'>Avg</option>"
                + "<option value='Where'>Where</option>"
                + "</select>";
            $(ctrl).empty();
            $(ctrl).append(selectStr);
            // 还原上次下拉框选择值
            $(ctrl).find("select option").each(function(){
                if ($(this).text() == txt)
                {
                    $(this).attr("selected", "true");
//                    if (txt == "Where")
//                        $(this.parentNode.parentNode.parentNode).find("input[type='checkbox']").eq(0).removeAttr("disabled");
                }
            });
            $(ctrl).find("select").focus();
            $(ctrl).find("select").blur(function(){
                if ($(this).find("option:selected").text() == "请选择类型")
                    $(this).replaceWith("");
                else if ($(this).find("option:selected").text() == "Where")
                {                    
//                    var chk = $(this.parentNode.parentNode).find("input[type='checkbox']").eq(0);
//                    chk.removeAttr("checked");
//                    chk.attr("disabled", "true");
                    $(this).replaceWith($(this).find("option:selected").text());
                }
                else
                    $(this).replaceWith($(this).find("option:selected").text());
            });
    }
    function AliasFocus(ctrl)
    {
        if (!CheckColumnSelected(ctrl))
            return;
        var txtStr = "<input type='text' value='" + $(ctrl).text() + "' style='width:50px;'/>";
            $(ctrl).empty();
            $(ctrl).append(txtStr);
            $(ctrl).find("input").focus();
            $(ctrl).find("input").blur(function(){
                if ($.trim($(this).val()) == "")
                    $(this).replaceWith("");
                else
                    $(this).replaceWith($(this).val());
            });
    }
    function OrderFocus(ctrl)
    {
        if (!CheckColumnSelected(ctrl))
            return;
        var txt = $(ctrl).text();
        var selectStr = "<select>"
                + "<option value='-1'>未排序</option>"
                + "<option value='Asc'>升序</option>"
                + "<option value='Desc'>降序</option>"
                + "</select>";
        $(ctrl).empty();
        $(ctrl).append(selectStr);
        // 还原上次下拉框选择值
        $(ctrl).find("select option").each(function(){
            if ($(this).text() == txt)
                $(this).attr("selected", "true");
        });
        $(ctrl).find("select").focus();
        $(ctrl).find("select").blur(function(){
            if ($(this).find("option:selected").text() == "未排序")
            {
                var t = $(this.parentNode.nextSibling);  // 顺序行的jquery对象
                var value = t.text();  // 取该行顺序值
                $(this.parentNode.nextSibling).html("");  // 清空“排序顺序”内容
                //把所有大于它顺序值的行全减1
                if (value != "")
                { 
                    var seqs =  $(".tbl_reportset").find(".orderseq").get();
                    var len = seqs.length;
                    var maxNum = 0;
                    for (var i=0; i<len-1; i++)
                    {
                        if (seqs[i].innerText != "" && parseInt(seqs[i].innerText) > parseInt(value))
                            seqs[i].innerText = parseInt(seqs[i].innerText) - 1;
                    }
                }
                
                $(this).replaceWith("");  // 清空“排序类型”内容
            }
            else
            {
                // 自动设置顺序 
                var seqs =  $(".tbl_reportset").find(".orderseq").get();
                if ($(this.parentNode.nextSibling).text() == "")
                {
                    var len = seqs.length;
                    var maxNum = 0;
                    for (var i=0; i<len-1; i++)
                    {
                        if (seqs[i].innerText != "")
                            maxNum++;
                    }
                    $(this.parentNode.nextSibling).html(maxNum+1);
                }
                //设置内容
                $(this).replaceWith($(this).find("option:selected").text());
            }
            
        });
    }
    function OrderSeqFocus(ctrl)
    {
        if (!CheckColumnSelected(ctrl))
            return;
        if ($(ctrl.previousSibling).text() == "")
            return;
        var len = $(".tbl_reportset").find("tr").length - 2;
        var selectStr = "<select>";
        for(var i=0; i<len; i++)
        {
            selectStr += "<option value='" + (i+1) + "'>" + (i+1) + "</option>";
        }
        selectStr += "</select>";
        var txt = $(ctrl).text();
        $(ctrl).empty();
        $(ctrl).append(selectStr);
        
        // 还原上次下拉框选择值
        $(ctrl).find("select option").each(function(){
            if ($(this).text() == txt)
                $(this).attr("selected", "true");
        });
        $(ctrl).find("select").focus();
        $(ctrl).find("select").blur(function(){
            $(this).replaceWith($(this).find("option:selected").text());
        });
    }