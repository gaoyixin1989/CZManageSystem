<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_gmcc_Left" Codebehind="Left.aspx.cs" %>

<%@ Register TagPrefix="bw" TagName="LeftMenu" Src="../controls/LeftMenu.ascx" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security" Namespace="Botwave.Security.Web.Controls" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.XQP" Namespace="Botwave.XQP.Web.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>菜单页</title>

    <script type="text/javascript" src="../../../res/js/jquery-latest.pack.js"></script>

    <script src="../scripts/jquery.autocomplete-min.js" type="text/javascript"></script>

    <script src="../scripts/common.js" type="text/javascript"></script>

    <link href="../scripts/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    
</head>
<body id="master">
<link href="../../../App_Themes/newblue/select.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function changeView(obj) {
            if ($("#div_" + obj).css("display") == "block") {
                $("#div_" + obj).css("display", "none");
                $("#ico_" + obj).attr("src", '<%=AppPath%>app_themes/gmcc/img/ico_have.gif');
            }
            else {
                $("#div_" + obj).css("display", "block");
                $("#ico_" + obj).attr("src", '<%=AppPath%>app_themes/gmcc/img/ico_nohave.gif');
            }
            defaultView = obj;
        };
    </script>

    <div id="submenucontent">
        <div id="find_item">
            <input name="textfield" id="textfield" runat="server" type="text" class="inputbox"
                size="25" title="请输入关键字搜索菜单" style="width: 118px;" />
            <button id="find_clear" title="清除">
            </button>
        </div>
        <ul class="item_list" id="item_select">
        </ul>
        <div class="submenu" style="text-align: left; overflow: auto; overflow-x: hidden;
            height: 100%;">
            <%--<div class="submenu">--%>
            <div class="menuList" id="menuList" style="overflow: auto;">
                <ul>
                    <bw:LeftMenu ID="leftMenu1" runat="server" />
                    <li style=" display:none">
                        <h2 onclick="changeView(this.id)" id="maintenance"><img src="/Web/app_themes/gmcc/img/ico_have.gif" id="ico_maintenance" />系统运维</h2>
                        <div class="navigation" id="div_maintenance" style="display:none">
                            <div class="menuItems">
                                
                                <bw:AccessController ResourceValue="E001" runat="server" ID="AccessController1" >
                                    <ContentTemplate>
                                        <a href="<%=AppPath %>contrib/workflow/pages/Extension/WorkflowHelper.aspx" target="rightFrame" title="流程辅助">流程辅助</a>
                                    </ContentTemplate>
                                </bw:AccessController>
                                 <bw:AccessController ResourceValue="E004" runat="server" ID="AccessController2" >
                                    <ContentTemplate>
                                        <a href="<%=AppPath %>apps/xqp2/pages/workflows/Extension/WorkflowHelper.aspx" target="rightFrame" title="任务改派">任务改派</a>
                                    </ContentTemplate>
                                </bw:AccessController>
                                 <bw:AccessController ResourceValue="M001" runat="server" ID="AccessController3" >
                                    <ContentTemplate>
                                        <a href="<%=AppPath %>apps/xqp2/pages/workflows/maintenance/search.aspx" target="rightFrame" title="工单管理">工单管理</a>
                                    </ContentTemplate>
                                </bw:AccessController>
                                <bw:AccessController ResourceValue="M002" runat="server" ID="AccessController4" >
                                    <ContentTemplate>
                                        <a href="<%=AppPath %>apps/pms/pages/StatInstanceReport.aspx?formid=0A652E41-CA03-4649-942F-D6B198509C64" target="rightFrame" title="流程监控">流程监控</a>
                                    </ContentTemplate>
                                </bw:AccessController>
                                <bw:AccessController ResourceValue="M003" runat="server" ID="AccessController5" >
                                    <ContentTemplate>
                                        <a href="<%=AppPath %>apps/pms/pages/UCS_FromManager.aspx" target="rightFrame" title="报表配置">报表配置</a>
                                    </ContentTemplate>
                                </bw:AccessController>
                                <bw:AccessController ResourceValue="M003" runat="server" ID="AccessController6" >
                                    <ContentTemplate>
                                        <a href="<%=AppPath %>apps/pms/pages/UCS_ImgFromManager.aspx" target="rightFrame" title="图表配置">图表配置</a>
                                    </ContentTemplate>
                                </bw:AccessController>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(function() {


            //DIV导航
            $("#menuList").height($(document.body).height() - $("#sildTitle").height() - 10 - 30);
            $(window).resize(function() {
                $("#menuList").height($(document.body).height() - $("#sildTitle").height() - 10 - 30);
            });

            var id = 0;
            $(".menuList").children("ul").children("li").children("h4").each(function() {
                $(this).css("cursor", "pointer");
                this.id = id;
                id++;
            });
            $(".menuList").children("ul").children("li").children("h4").click(function() {
                var gid = this.id;
                $(".menuList").children("ul").children("li").children("h4").each(function() {
                    if (gid == this.id) {
                        $(this).parent().toggleClass("c");
                    }
                    else {
                        $(this).parent().removeClass("c");
                    }
                });
            });
            //若无菜单内容,则默认隐藏菜单栏
            var childSize = $(".menuList").children("ul").children("li").size();
            if (childSize == 0) {
                window.parent.document.frames.switchFrame.switchSysBar('hide');
            }

            bw.tip.initBWTIP();
        });
    </script>

    <script type="text/javascript">
        //返回数据元素所在索引，不存在返回-1   
        Array.prototype.index = function (el) {   
            var i = 0;   
            for (var i = 0, len = this.length; i < len; i++) {   
                if (el == this[i]) {   
                    return i;   
                }   
            }   
            return -1;     
        };


        var workflows = <%=GetWorkflows()%>;
         $(function() {
         $('#textfield').autocomplete(workflows, {
                 max: 12,    //列表里的条目数
                 minChars: 0,    //自动完成激活之前填入的最小字符
                 width: 145,     //提示的宽度，溢出隐藏
                scrollHeight: 300,   //提示的高度，溢出显示滚动条
                 matchContains: true,    //包含匹配，就是data参数里的数据，是否只要包含文本框里的数据就显示
                 autoFill: false,    //自动填充


                 formatItem: function(row, i, max) {
                 //return row + "";
                 return row.name;
                 },
                 formatMatch: function(row, i, max) {
                 // return row + row;
                 return row.name;
                 },
                 formatResult: function(row) {
                 return row.name;
                 }
             }).result(function(event, row, formatted) {
                 
                 $("#item_select").css("display","block");
                  $("#item_select").html(row.to);


             });
         });

         
    </script>

    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            $("#form1").keypress(function(e) {
                if (e.which == 13) //判断所按是否回车键 
                { return false; }
            });

            //关键字搜索
            $("#find_item .inputbox").keyup(function() {
                var finditem = $(this).val();
                if (finditem != "") {
                    //$("#item_select").attr("scrollTop", 0);
                    $("#item_select").show();
                    // $("#item_select").html("抱歉没有搜索到！"); //kamael by 2013-01-09
                    $("#find_clear").css("visibility", "visible");
                    var str = "";
                } else {
                    $("#find_clear").css("visibility", "hidden");
                    $("#item_select").html("抱歉没有搜索到！");
                    $("#item_select").hide();
                }
            });
            //清空已选item
            $("#selected_clear").click(function() {
                selected_clear();
                return false;
            });
            //清空搜索关键字
            $("#find_clear").click(function() {
                $(this).css("visibility", "hidden");
                $("#find_item .inputbox").attr("value", "");
                $("#item_select").html("抱歉没有搜索到！。");
                $("#item_select").hide();
                return false;
            });

        });


        //IE6添加鼠标移上样式
        function ie6hover() {
            if ($.browser.msie && $.browser.version == 6.0) {
                $("#item_select li span").remove();
                $(".item_name").hover(function() {
                    $(this).addClass("item_name_hover");
                }, function() {
                    $(this).removeClass("item_name_hover");
                });
            }
        }
    </script>

</body>
</html>
