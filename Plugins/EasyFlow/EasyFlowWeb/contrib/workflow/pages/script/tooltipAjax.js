$(function(){
    $("[tooltip]").hover(function(){            
        var current = this;
        var tooltipLayout = $('<div id="tooltip"></div>').insertAfter(this); // 显示提示信息层.
        var onTooltip = function(){
            // 调用 Ajax Web Service
            var name = $(current).attr("tooltip");
            if(name != ""){
                tooltipLayout.prepend("正在加载 ...");
                Botwave.Workflow.Extension.WebServices.WorkflowAjaxService.GetActorTooltipInfo(name , dispalyTooltip, errorHandler, timeoutHandler);
            }
        };
        var dispalyTooltip = function(result){
            //var titleHtml = "姓　名：" + result.RealName;
            var titleHtml = "部　门：" + result.DpFullName;
            //titleHtml += "<br />电子邮箱：" + result.Email;
            titleHtml += "<br />电　话：" + result.Mobile + "," + result.Tel;
            titleHtml += "<br />任务数：" + result.WorkingCount;
            //titleHtml += "<br />固定电话：" + result.Tel;
            // 显示提示.
            tooltipLayout.empty();
            tooltipLayout.prepend(titleHtml);
        };
        // Ajax 超时.
        var timeoutHandler = function (result){
           alert("Timeout :" + result);
        };               
        // Ajax 错误.
        var errorHandler = function (result){
           var msg=result.get_exceptionType() + "\r\n";
           msg += result.get_message() + "\r\n";
           msg += result.get_stackTrace();
           alert(msg);
        };
        
        onTooltip(); // 执行 Ajax 提示    
        $(this).mousemove(function(e){
            e = e || window.event;
            var x = e.pageX - 16;
            var pageY = e.pageY;
            if(x - 2 < 0)
                x = 2;
            if(x + 280 > document.body.clientWidth)
                x = document.body.clientWidth - 300;
            var y = pageY + 18;
            if(pageY > (document.body.clientHeight - 80))
                y = pageY - 80;
            $("#tooltip").css({"left": x, "top": y, "display": "block"}); 
        });
    },
    function(){
        $("#tooltip").remove();                
    })
});
