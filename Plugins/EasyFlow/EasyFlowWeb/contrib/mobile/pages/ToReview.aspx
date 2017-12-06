<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_mobile_pages_ToReview"
MasterPageFile="~/plugins/easyflow/masters/Mobile.master" Title="待阅事宜" Codebehind="ToReview.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="page-header">
        <!-- Fixed navbar -->
        <div class="navbar navbar-inverse navbar-fixed-top ui-header" role="navigation">
            <h1 class="text-center ui-title" style="color: rgb(69, 125, 179);" id="header">
                待阅事宜
            </h1>
            <div class="pull-right ui-btn-right">
                <a class="btn btn-link btn-link-icon-search" href="#" role="button" data-toggle="modal"
                    data-target="#myModal"></a>
            </div>
        </div>
    </div>
    <div class=" container-fluid theme-showcase" role="main" id="myTaskPage">
        <div id="wrapper">
            <div id="scroller" style="position: relative">
                <div id="pullDown">
                    <span class="pullDownIcon"></span><span class="pullDownLabel">下拉刷新...</span>
                </div>
                <div class="list-group" id="thelist">
                    
                </div>
                <div id="pullUp">
                    <span class="pullUpIcon" style="display: none"></span><span class="pullUpLabel" style="display: none">
                        上拉加载更多...</span>
                </div>
            </div>
        </div>
    </div>
    <div class="navbar navbar-default navbar-fixed-bottom ui-bar-inherit page-footer"
        role="navigation">
        <div class="btn-group btn-group-justified">
            <a class="btn ui-btn ui-icon-home ui-icon-top ui-footer-btn" role="button" href="default.aspx">
                工作台</a> <a class="btn ui-btn ui-icon-search ui-icon-top ui-footer-btn" href="search.aspx"
                    role="button">高级查询</a> <a class="btn ui-btn ui-icon-exit ui-icon-top ui-footer-btn" role="button" style="display:none" onclick="window.history.go(-1);">返回</a>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" style="top: 70px">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel">
                        条件查询</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-6 col-md-6">
                            <div class="form-group">
                                <label for="exampleInputPassword1">
                                    流程名称</label>
                                <asp:DropDownList CssClass="form-control" runat="server" ID="ddlWorkflows">
                                    
                                </asp:DropDownList></div>
                        </div>
                        <div class="col-xs-6 col-md-6">
                            <div class="form-group">
                                <label for="InputKeyword">
                                    关键字</label>
                                <asp:TextBox ID="txtKeywords" CssClass="form-control" runat="server" placeholder="关键字"></asp:TextBox>
                            </div>
                        </div>
                        
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        取消</button>
                    <button type="button" id="btnSearch" data-dismiss="modal" class="btn btn-primary">
                        搜索</button>
                </div>
            </div>
        </div>
    </div>
   <asp:HiddenField ID="hidPageIndex" runat="server" />
    <script type="text/javascript">
        var myScroll,
	pullDownEl, pullDownOffset,
	pullUpEl, pullUpOffset,
	generatedCount = 0;
        var pullToRefresh = true;
        function loaded() {
            pullDownEl = document.getElementById('pullDown');
            pullDownOffset = pullDownEl.offsetHeight;
            pullUpEl = document.getElementById('pullUp');
            pullUpOffset = pullUpEl.offsetHeight;
            myScroll = new iScroll('wrapper', {

                useTransition: true
                , topOffset: pullDownOffset
                , onRefresh: function () {
                    if (pullDownEl.className.match('loading')) {
                        pullDownEl.className = '';
                        pullDownEl.querySelector('.pullDownLabel').innerHTML = '下拉刷新...';
                    }
                    else if (pullUpEl.className.match('loading')) {
                        pullUpEl.className = '';
                        pullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉加载更多...';
                    }
                }, onScrollMove: function () {
                    if (this.y > 0 && !pullDownEl.className.match('flip')) {
                        pullDownEl.className = 'flip';
                        pullDownEl.querySelector('.pullDownLabel').innerHTML = '松手开始更新...';
                        this.minScrollY = 0;
                    } else if (this.y < 0 && pullDownEl.className.match('flip')) {
                        pullDownEl.className = ''; pullDownEl.querySelector('.pullDownLabel').innerHTML = '下拉刷新...';
                        this.minScrollY = -pullDownOffset;
                    } else if (-this.y > (-this.maxScrollY) && !pullUpEl.className.match('flip') && this.maxScrollY < 0 && pullToRefresh) {
                        pullToRefresh = false;
                        pullUpEl.className = 'flip';
                        //pullUpEl.querySelector('.pullUpLabel').innerHTML = '释放立即加载...';
                        pullUpEl.className = 'loading';
                        pullUpEl.querySelector('.pullUpLabel').innerHTML = '加载中...';
                        pullUpAction(); // Execute custom function (ajax call?)
                        //this.maxScrollY = this.maxScrollY;
                    }
                    if (this.maxScrollY < 0) {
                        $(".pullUpIcon").show();
                        $(".pullUpLabel").show();
                    }
                    else {
                        $(".pullUpIcon").hide();
                        $(".pullUpLabel").hide();
                    }
                }, onScrollEnd: function () {
                    if (pullDownEl.className.match('flip') && pullToRefresh) {
                        pullToRefresh = false;
                        pullDownEl.className = 'loading';
                        pullDownEl.querySelector('.pullDownLabel').innerHTML = '加载中...';
                        pullDownAction();
                        // Execute custom function (ajax call?)
                    } else if (pullUpEl.className.match('flip')) {
                        //pullUpEl.className = 'loading';
                        //pullUpEl.querySelector('.pullUpLabel').innerHTML = '加载中...';
                        //pullUpAction();
                        // Execute custom function (ajax call?) 
                    }
                }
            });
            setTimeout(function () { document.getElementById('wrapper').style.left = '0'; }, 1000);
        }
        //初始化绑定iScroll控件 
        document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
        document.addEventListener('DOMContentLoaded', loaded, false);
        function pullDownAction() {
            //setTimeout(function () {
                var el, a, i;
                el = document.getElementById('thelist');
                $.ajax({
                    type: "post",
                    dataType: "html",
                    url: "<%=AppPath %>contrib/mobile/ajax/ToReviewAjax.aspx",
                    data: { wfname: $("#<%=ddlWorkflows.ClientID %>").val(), key: $("#<%=txtKeywords.ClientID %>").val(), pageIndex: "0" },
                    async: true,
                    timeout: 300000,
                    success: function (data) {
                        var d = document.createDocumentFragment();
                        $(el).empty();
                        $(el).html(data);
                        //el.appendChild(d);
                        pullToRefresh = true;
                        $("#<%=hidPageIndex.ClientID %>").val(0);
                        //if (myScroll)
                            //myScroll.refresh();
                        //$("#scroller").hideLoading();
                    },
                    error: function () {
                        pullToRefresh = true;
                        $(".loading-backdrop").hideLoading();
                        $(".loading-backdrop").hide();
                    },
                    complete: function () {
                        if (myScroll)
                            myScroll.refresh();
                        $(".loading-backdrop").hideLoading();
                        $(".loading-backdrop").hide();
                    }
                    
                });
                //if (myScroll)
                    //myScroll.refresh(); 	// Remember to refresh when contents are loaded (ie: on ajax completion)
            //}, 1000);
            
        }

        function pullUpAction() {
            //setTimeout(function () {
                var el, li, i;
                el = document.getElementById('thelist');
                var pageIndex = $("#<%=hidPageIndex.ClientID %>").val(); //
                $.ajax({
                    type: "post",
                    dataType: "html",
                    url: "<%=AppPath %>contrib/mobile/ajax/ToReviewAjax.aspx",
                    data: { wfname: $("#<%=ddlWorkflows.ClientID %>").val(), key: $("#<%=txtKeywords.ClientID %>").val(), pageIndex: (parseInt(pageIndex) + 1) },
                    async: true,
                    timeout: 300000,
                    success: function (data) {
                        var d = document.createDocumentFragment();
                        //$(el).empty();
                        $(el).append(data);
                        //el.appendChild(d);
                        pullToRefresh = true;
                        $("#<%=hidPageIndex.ClientID %>").val((parseInt(pageIndex) + 1));
                        //myScroll.refresh();
                    },
                    error: function () {
                        pullToRefresh = true;
                        //alert("加载待阅列表失败");
                    },
                    complete: function () {
                        if (myScroll)
                            myScroll.refresh();
                        ///$("#scroller").hideLoading();
                    }
                });
                //myScroll.refresh(); 	// Remember to refresh when contents are loaded (ie: on ajax completion)
            //}, 1000);
        }

        $(document).ready(function () {
            pullDownAction();
            $("#btnSearch").click(function () {
                $(".loading-backdrop").show();
                $(".loading-backdrop").showLoading();
                pullDownAction();
            });
            $(".ui-icon-home,ui-icon-search").click(function () {
                $(".loading-backdrop").showLoading();
            });
            $(".ui-icon-exit").show();
            $(".ui-icon-exit").html("批量审批");
            $(".ui-icon-exit").removeAttr("onclick");
            $(".ui-icon-exit").click(function () {
                var sheetId = $("input[name='chk']");
                //alert(sheetId)
                var len = 0;
                sheetId.each(function () {
                    if ($(this).attr("checked"))
                        len++;
                })
                if (len == 0) {
                    alert('请选择要处理的工单');
                }
                else if (confirm("确定处理？")) {
                    $(".loading-backdrop").showLoading();
                    var aiids = "";
                    sheetId.each(function () {
                        aiids += sheetId.val() + ","
                    });
                    $.ajax({
                        type: "post",
                        dataType: "text",
                        url: "<%=AppPath %>contrib/mobile/ajax/CommonAjax.aspx",
                        data: { aiids: aiids, command: "reading" },
                        async: true,
                        success: function (data) {
                            alert(data);
                        },
                        error: function () {
                            pullToRefresh = true;
                            //alert("加载待阅列表失败");
                        },
                        complete: function () {
                            $("#scroller").hideLoading();
                            pullDownAction();
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>

