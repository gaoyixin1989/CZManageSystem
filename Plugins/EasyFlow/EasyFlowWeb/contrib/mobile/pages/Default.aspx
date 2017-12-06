<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_mobile_pages_Default"
MasterPageFile="~/plugins/easyflow/masters/Mobile.master"  Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="page-header">
        <!-- Fixed navbar -->
        <div class="navbar navbar-inverse navbar-fixed-top ui-header" role="navigation">
            <h1 class="text-center ui-title" style="color: rgb(69, 125, 179);">
                工作台
            </h1>
        </div>
    </div>
    <div class="container theme-showcase" role="main">
     <h4><small id="smlogin"></small></h4>
        <div class="row">
            <div class="col-xs-4 col-md-4">
                <div class="thumbnail" style="padding:0" onmouseover="this.className='thumbnail hover-one'" onmouseout="this.className='thumbnail'">
                    <a href="Todo.aspx" class="hover-one">
                        <div class="caption" style="text-align: center;padding-bottom: 0;">
                            <img id="todoimg" src="<%=AppPath %>app_themes/mobile/img-icon/to_do.png"
                                alt="..." style="height: 4.5rem;width: 4.5rem;">
                            <h4 style="font-size: 9pt; margin-top:.35rem">
                                待办事宜</h4>
                            <%--<h4>
                                <small id="todo"></small></h4>--%>
                        </div>
                    </a>
                </div>
            </div>
            <div class="col-xs-4 col-md-4">
                <div class="thumbnail" style="padding:0" onmouseover="this.className='thumbnail hover-one'" onmouseout="this.className='thumbnail'">
                    <a href="ToReview.aspx" class="hover-one">
                        <div class="caption" style="text-align: center;padding-bottom: 0;">
                            <img id="imgreview" src="<%=AppPath %>app_themes/mobile/img-icon/review.png" alt="..." style="height: 4.5rem;width: 4.5rem;">
                            <h4 style="font-size: 9pt; margin-top:.35rem">
                                待阅事宜</h4>
                            <%--<h4>
                                <small id="review"></small></h4>--%>
                        </div>
                    </a>
                </div>
            </div>
            <div class="col-xs-4 col-md-4">
                <div class="thumbnail" style="padding:0" onmouseover="this.className='thumbnail hover-one'" onmouseout="this.className='thumbnail'">
                    <a href="doneTaskByAppl.aspx" class="hover-one">
                        <div class="caption" style="text-align: center;padding-bottom: 0;">
                            <img src="<%=AppPath %>app_themes/mobile/img-icon/done.png" alt="..." style="height: 4.5rem;width: 4.5rem;">
                            <h4 style="font-size: 9pt; margin-top:.35rem">
                                已办事宜</h4>
                            <%--<h4>
                                <small>10条记录</small></h4>--%>
                        </div>
                    </a>
                </div>
            </div>
            <div class="col-xs-4 col-md-4">
                <div class="thumbnail" style="padding:0" onmouseover="this.className='thumbnail hover-one'" onmouseout="this.className='thumbnail'">
                    <a href="myTask.aspx" class="hover-one">
                        <div class="caption" style="text-align: center;padding-bottom: 0;">
                            <img src="<%=AppPath %>app_themes/mobile/img-icon/mywork.png" alt="..." style="height: 4.5rem;width: 4.5rem;">
                            <h4 style="font-size: 9pt; margin-top:.35rem">
                                我的工单</h4>
                           <%-- <h4>
                                <small>10条记录</small></h4>--%>
                        </div>
                    </a>
                </div>
            </div>
            
            <div class="col-xs-4 col-md-4">
                <div class="thumbnail" style="padding:0" onmouseover="this.className='thumbnail hover-one'" onmouseout="this.className='thumbnail'">
                    <a href="doneReviews.aspx" class="hover-one">
                        <div class="caption" style="text-align: center;padding-bottom: 0;">
                            <img src="<%=AppPath %>app_themes/mobile/img-icon/red.png" alt="..." style="height: 4.5rem;width: 4.5rem;">
                            <h4 style="font-size: 9pt; margin-top:.35rem">
                                已阅事宜</h4>
                            <%--<h4>
                                <small>10条记录</small></h4>--%>
                        </div>
                    </a>
                </div>
            </div>
            <div class="col-xs-4 col-md-4">
                <div class="thumbnail" style="padding:0" onmouseover="this.className='thumbnail hover-one'" onmouseout="this.className='thumbnail'">
                    <a href="donetask.aspx" class="hover-one">
                        <div class="caption" style="text-align: center;padding-bottom: 0;">
                            <img src="<%=AppPath %>app_themes/mobile/img-icon/mytask.png" alt="..." style="height: 4.5rem;width: 4.5rem;">
                            <h4 style="font-size: 9pt; margin-top:.35rem">
                                已办任务</h4>
                            <%--<h4>
                                <small>10条记录</small></h4>--%>
                        </div>
                    </a>
                </div>
            </div>
            <div class="col-xs-4 col-md-4">
                <div class="thumbnail" style="padding:0" onmouseover="this.className='thumbnail hover-one'" onmouseout="this.className='thumbnail'">
                    <a href="assignmentTask.aspx" class="hover-one">
                        <div class="caption" style="text-align: center;padding-bottom: 0;">
                            <img src="<%=AppPath %>app_themes/mobile/img-icon/forward.png" alt="..." style="height: 4.5rem;width: 4.5rem;">
                            <h4 style="font-size: 9pt; margin-top:.35rem">
                                转交任务</h4>
                            <%--<h4>
                                <small>10条记录</small></h4>--%>
                        </div>
                    </a>
                </div>
            </div>
            <div class="col-xs-4 col-md-4">
                <div class="thumbnail" style="padding:0" onmouseover="this.className='thumbnail hover-one'" onmouseout="this.className='thumbnail'">
                    <a href="draft.aspx" class="hover-one">
                        <div class="caption" style="text-align: center;padding-bottom: 0;">
                            <img  id="imgdraft" src="<%=AppPath %>app_themes/mobile/img-icon/draft.png" alt="..." style="height: 4.5rem;width: 4.5rem;">
                            <h4 style="font-size: 9pt; margin-top:.35rem">
                                草稿箱</h4>
                            <%--<h4>
                                <small>10条记录</small></h4>--%>
                        </div>
                    </a>
                </div>
            </div>
        </div>
    </div>
    <div class="navbar navbar-default navbar-fixed-bottom ui-bar-inherit" role="navigation">
        <div class="btn-group btn-group-justified">
            <a class="btn ui-btn ui-icon-home ui-icon-top ui-footer-btn ui-btn-active" role="button">工作台</a>
            <a class="btn ui-btn ui-icon-search ui-icon-top ui-footer-btn" href="search.aspx" role="button">高级查询</a>
            <%--<a class="btn ui-btn ui-icon-exit ui-icon-top ui-footer-btn" role="button" href="<%=AppPath%>contrib/security/pages/logout.aspx?type=mobile">退出</a>--%>
            <a class="btn ui-btn ui-icon-exit ui-icon-top ui-footer-btn" role="button" id="btnBack" style="display:none" onclick="window.history.go(-1);">返回</a>
        </div>
    </div>
    <script type="text/javascript" src="../../workflow/pages/script/workflowextension.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ("<%=Botwave.Security.LoginHelper.UserName %>" != "anonymous"
            && ($.cookie("<%=Botwave.Security.LoginHelper.UserCookieKey %>") != "undefined"
            && $.cookie("<%=Botwave.Security.LoginHelper.UserCookieKey %>") != null)) {//缓存页面的cookie操作
                $("a").click(function () {
                    if ($(this).attr("class").indexOf("ui-btn-active") == -1)
                        $(".loading-backdrop").showLoading();
                    //$(".loading-backdrop").showLoading();
                });

                $.post("<%=AppPath %>contrib/mobile/ajax/CommonAjax.aspx", { command: "todo" }, function (data) {
                    //$("#todo").html("有" + data + "条任务");
                    if (parseInt(data) > 0) {
                        //$("#todoimg").attr("src", "<%=AppPath %>app_themes/mobile/img-icon/new_to_do.png")
                        $("#todoimg").next("h4").append("<small class=\"badge\" style=\"background: rgb(254, 33, 41);display: inline;position: absolute;top: 1.25rem;margin-left: -1.5rem;\">" + data + "</small>");
                    }
                });

                $.post("<%=AppPath %>contrib/mobile/ajax/CommonAjax.aspx", { command: "toreview" }, function (data) {
                    //$("#review").html("有"+data+"条任务");
                    if (parseInt(data) > 0) {
                        //$("#imgreview").attr("src", "<%=AppPath %>app_themes/mobile/img-icon/review_new.png")
                        $("#imgreview").next("h4").append("<small class=\"badge\" style=\"background: rgb(254, 33, 41);display: inline;position: absolute;top: 1.25rem;margin-left: -1rem;\">" + data + "</small>");
                    }
                });

                $.post("<%=AppPath %>contrib/mobile/ajax/CommonAjax.aspx", { command: "draft" }, function (data) {
                    //$("#review").html("有"+data+"条任务");
                    if (parseInt(data) > 0) {
                        //$("#imgdraft").attr("src", "<%=AppPath %>app_themes/mobile/img-icon/draft_new.png")
                        $("#imgdraft").next("h4").append("<small class=\"badge\" style=\"background: rgb(254, 33, 41);display: inline;position: absolute;top: 1.25rem;margin-left: -1rem;\">" + data + "</small>");
                    }
                });

                $.post("<%=AppPath %>contrib/mobile/ajax/CommonAjax.aspx", { command: "loginname" }, function (data) {
                    //$("#review").html("有"+data+"条任务");
                    $("#smlogin").html(data + "：您好！");
                });

                $(".loading-backdrop").hideLoading();
                $(".loading-backdrop").hide();
            }
        });

    </script>
</asp:Content>

