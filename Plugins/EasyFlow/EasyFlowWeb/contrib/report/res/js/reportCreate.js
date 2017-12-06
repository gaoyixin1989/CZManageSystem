    
    // 客户端回调返回数据处理，格式：[命令]※[数据]
    function ReceiveDataFromServer(valueReturnFromServer, context)
    {
        var retArgs = valueReturnFromServer.split('※');
        switch(retArgs[0])
        {
            case "drop":
                $("#ctl00_cphBody_column_list").empty();
                $("#ctl00_cphBody_column_list").append(retArgs[1]);//把返回的数据保存到一个div中
              
                var options = $("#ctl00_cphBody_column_list").find("option");
                for (var i = 0; i<options.length; i++)
                {                  
                    var newRow = AddNewRow();
                    var txt = options.get(i).text;
                    $(newRow).find(".column").html(txt);
                }   
                break;
            case "preview":
                var tableData = retArgs[1].split('|');
                var retData = "";
                retData += "<table width='100%' cellspacing='0' border='1'>";               
                retData += "<tr>";
                var rowData = tableData[0].split(',');
                for(var j = 0;j < rowData.length;j++)
                {
                    retData += "<th>" + rowData[j] + "</th>";
                }
                retData += "</tr>";
               
                for(var i = 1;i < tableData.length;i++)
                {
                     retData += "<tr>";
                     var rowData = tableData[i].split(',');
                     for(var j = 0;j < rowData.length;j++)
                     {
                         retData += "<td>" + rowData[j] + "</td>";
                     }
                     retData += "</tr>";
                }
                retData += "</table>";                
                
                var newwindow = window.open("ReportPreview.html","","");
                newwindow.document.write("<html>");
                newwindow.document.write("<head><title>报表数据预览</title>");
                newwindow.document.write("<style>table{BORDER-COLLAPSE: collapse;border: 1px solid black;}");
                newwindow.document.write("table th,table td{border:1px solid #999999;empty-cells:show;}");
                newwindow.document.write("</style></head>");
                newwindow.document.write("<body><div class='div_pre_grid'>");
                newwindow.document.write(retData);
                newwindow.document.write("</div></body></html>");
                newwindow.document.close();
                break;
            case "senior":
                break;
        }
    }
    
    // 客户端回调出错处理，IsWindowPre对应控件属性IsWindowPre，页面加载后初始化全局变量
    function CallBackError(arg)
    {
        alert(arg);
    }
    function DropSelectedChange(ctrl)
    {
        var tmp;
        for (var i=$(".tbl_reportset tr").length-1; i>=0; i--)
        {
            tmp = $(".tbl_reportset tr").get(i);
            if ($(tmp).hasClass("header_row") || $(tmp).hasClass("clone_row"))
                continue;
            else
                $(tmp).remove();
        }
        var value = $(ctrl).val(); 
        if (value != "")
            CallBackToTheServer("drop※" + value, "");
    }
    function AddNewRow()
    {
        var rowStr = "<tr style='display:none;' class='clone_row'>" + $(".clone_row").html() + "</tr>";
        var cloneRow = $(".clone_row").get(0);
        $(cloneRow).css("display", "");
        $(cloneRow).removeClass();
        $(cloneRow).after(rowStr);
        
        return cloneRow;
    }
    function DelRow(ctrl)
    {
         $(ctrl.parentNode.parentNode).remove();
    }
    function SaveReport()
    {
        if (!CheckReportPage())
            return false;
            
        SaveReportData();
    }    
    function SaveReportData()
    {
        var length = $(".column").length;
        var chkArray = $(".chk").get();
        var columnArray = $(".column").get();
        var firstTitleArray = $(".firstTitle").get();
        var secondTitleArray = $(".secondTitle").get();
        var thirdTitleArray = $(".thirdTitle").get();
        var orderArray = $(".order").get();
        var orderseqArray = $(".orderseq").get();
        var groupArray = $(".group").get();
        var conditionArray = $(".condition").get();
        var isshow,column,firstTitle,secondTitle,thirdTitle,order, orderseq, group, condition,fieldName;
        var sendServerData = '';
      
        for (var i=0; i<length-1; i++)
        {
            isshow =  chkArray[i].checked ? "true" : "false";
            column = $("#ctl00_cphBody_column_list").find("option:contains('" + columnArray[i].innerText + "')").val();
            //$("#ctl00_cphBody_column_list").find("option[value=" + columnArray[i].innerText + "]").text();
            //$("#ctl00_cphBody_column_list").find("option:contains('" + columnArray[i].innerText + "')").val();
                          
            firstTitle = firstTitleArray[i].innerText;
            secondTitle = secondTitleArray[i].innerText;
            thirdTitle = thirdTitleArray[i].innerText;
            order = orderArray[i].innerText;
            orderseq = orderseqArray[i].innerText;
            group = groupArray[i].innerText;
            condition = conditionArray[i].innerText;
            fieldName = columnArray[i].innerText;
            sendServerData += isshow + "#" + column + "#" + firstTitle + "#" + secondTitle + "#" + thirdTitle + "#" + order + "#" + orderseq + "#" + condition + "#" + group + "#" + fieldName + "@";
        }
        if (sendServerData.length > 0)
            sendServerData = sendServerData.substring(0, sendServerData.length - 1);

        document.getElementById("ctl00_cphBody_hidReportItem").value = sendServerData;
    }
    
    function CheckColumnSelected(ctrl)
    {
        var columnObj = $(ctrl.parentNode).find(".column").get(0);
        if ($(columnObj).text() == "" || $(columnObj).text() == "请选择数据源")
            return false;
        else
            return true;
    }
    
    function CheckReportPage()
    {
        if (txtReportName.val() == "")
        {
            alert("请输入报表名称！");
            return false;
        }
        if ($("#div_report").find("input[@value='OK']").length > 0)
        {
            alert("还有筛选条件未确认，请先全部确认！");
            return false;
        }
        if ($("#div_report").find("input[@value='OK']").length > 0)
        {
            alert("还有筛选条件未确认，请先全部确认！");
            return false;
        }
        
        var ret = true;
        $("#div_report").find(".column").each(function(){
            var columnObj = $(this);
            if ($.trim(columnObj.text()) == "" || $.trim(columnObj.text()) == "请选择数据源")
            {
                if (!$(this.parentNode).hasClass("clone_row"))
                    ret = false;
            }
        });
        
        if (!ret)
            alert("请设置报表显示字段！");
        
        return ret;
    }
    
    function CheckReportCondition()
    {
        if ($("#div_report").find("input[@value='OK']").length > 0)
        {
            alert("还有筛选条件未确认，请先全部确认！");
            return false;
        }        
        //判断所有行是否已设置显示字段
        var ret = true;
        $("#div_report").find(".column").each(function(){
            var columnObj = $(this);
            if ($.trim(columnObj.text()) == "" || $.trim(columnObj.text()) == "请选择数据源")
            {
                if (!$(this.parentNode).hasClass("clone_row"))
                    ret = false;
            }
        });
        
        if (!ret)
        {
            alert("请设置报表显示字段！");
            return false;
        }
        return true;
    }    
    function ReportPreview(mode)
    {
        SaveReportData();
        var sqldata = document.getElementById("ctl00_cphBody_hidReportItem").value;
        var fromTable = document.getElementById("ctl00_cphBody_ddlSource").value;
        CallBackToTheServer("preview※" + fromTable + "※" + encodeURIComponent(sqldata), $('.div_pre_grid').html('获取数据中，请等待。。。'));
    }    
    function CheckSql()
    {
        var sql = txtSQL.val();
        var pattern = /(\binsert\b)|(\bupdate\b)|(\bdelete\b)|(\btruncate\b)|(\bcreate\b)/gi;
        if (pattern.test(sql))
        {
            alert("配置内容含有SQL敏感关键词，提交失败");
            return false;
        }
        else
            return true;
    }