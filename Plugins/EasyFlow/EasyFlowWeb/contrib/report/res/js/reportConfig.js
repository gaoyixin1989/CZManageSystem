$(document).ready(function(){
    $(".tr_show_table .td_show_table").each(function(){
        $(this).click(function(){
            if ($(this.parentNode.nextSibling).is(":visible"))
                $(this.parentNode.nextSibling).css("display", "none"); //点击第二次隐藏字段行
            else
            {
                $(".tbl_report_config .tr_show_column:visible").css("display", "none"); //隐藏前一显示字段行
                $(this.parentNode.nextSibling).css("display", "block"); // 显示相邻的详细字段行
            }
         });
     });
     $(".tr_show_table input[type='checkbox']").each(function(){
        $(this).click(function(){
            if (this.checked)
            {
                //       checkbox->  td->    tr->     下一行
                var tr = $(this.parentNode.parentNode.nextSibling);
                tr.find("input[type='checkbox']").attr("checked", "checked");  // 全选
                $(".tbl_report_config .tr_show_column:visible").css("display", "none"); //隐藏前一显示字段行
                tr.css("display", "block"); // 显示相邻的详细字段行
            }
            else
            {
                //去掉字段所有勾
               var tr = $(this.parentNode.parentNode.nextSibling);
               tr.find("input[type='checkbox']").removeAttr("checked");  // 取消全选
            }
         });
     });
     $(".tr_show_column input[type='checkbox']").each(function(){
        $(this).click(function(){
            if (this.checked)
            {
                //       checkbox->  td->       tr->     tbody->    table->     td->       tr->     上一行
                var tr = $(this.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.previousSibling);
                tr.find("input[type='checkbox']").attr("checked", "checked");
            }
            else
            {
                //已去掉所有勾，去掉相关表的勾
                var tr = $(this.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.previousSibling);
                if ($(this.parentNode.parentNode.parentNode).find("input:checked").length == 0)
                    tr.find("input[type='checkbox']").removeAttr("checked");
            }
         });
      });
 });
 
 function CheckReportSetting()
 {
    //所有“显示名称”都不能为空
    var flag = true;
    var errorStr = '';
    var column;
    var table;
    $(".tbl_report_config .tr_show_table").find(":text").each(function(){
        if ($(this.parentNode.parentNode).find("input[type='checkbox']").get(0).checked && $.trim($(this).val())=="")
        {   
            table = $(this.parentNode.parentNode).find(".td_show_table").get(0).innerHTML;
            errorStr += "\"" + table + "\"的显示名称为空！";
            flag = false;
        }
    });
    $(".tbl_report_config .tr_show_column").find(":text").each(function(){
        if ($(this.parentNode.parentNode).find("input[type='checkbox']").get(0).checked && $(this).val()=="")
        {
            column = $(this.parentNode.parentNode).find("td").get(1).innerHTML;
            table = $(this.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.previousSibling).find(".td_show_table").get(0).innerHTML;
            errorStr += "\"" + table + "\"表的\"" + column + "\"字段的显示名称为空！";
            flag = false;
        }
    });
    
    if (!flag)
        alert(errorStr);
        
    return flag;
 }