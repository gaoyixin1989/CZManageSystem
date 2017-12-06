<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Mobile.master" AutoEventWireup="true" Inherits="contrib_mobile_pages_Search" Codebehind="Search.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Botwave.XQP" Namespace="Botwave.XQP.Web.Controls" TagPrefix="bw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="toolkitScriptManager1" runat="server" />
    <div class="page-header">
        <!-- Fixed navbar -->
        <div class="navbar navbar-inverse navbar-fixed-top ui-header" role="navigation">
            <h1 class="text-center ui-title" style="color: rgb(69, 125, 179);" id="header">
                高级查询
            </h1>
            <div class="pull-right ui-btn-right">
                <a class="btn btn-link btn-link-icon-search" href="#" role="button" data-toggle="modal"
                    data-target="#myModal"></a>
            </div>
        </div>
    </div>
    <div class=" container-fluid theme-showcase dataList" role="main">
        <asp:UpdatePanel ID="updatepanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div class="list-group" style="margin-bottom:0">
                    <asp:Repeater ID="listResults" runat="server" OnItemDataBound="listResults_ItemDataBound">
                        <HeaderTemplate>
                        <span id="resultsCompleted" style="display:none">
                        </span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="#" onclick="$('.loading-backdrop').show();$('.loading-backdrop').showLoading();window.location='<%=AppPath%>contrib/mobile/pages/workflowview.aspx?wiid=<%# Eval("WorkflowInstanceId")%>&returnurl=search.aspx';"
                                class="list-group-item ui-btn-icon-right ui-icon-carat-r">
                                <div style="float: left; margin-top: 2.8rem">
                                    <asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal>
                                </div>
                                <div style="margin-left: 3rem;">
                                    <h4 class="list-group-item-heading">
                                        <%# Eval("Title") %></h4>
                                    <p class="list-group-item-text">
                                        受理号：<%# Eval("SheetId")%></p>
                                    <p class="list-group-item-text">
                                        步&nbsp;&nbsp; 骤：<%# Eval("ActivityName")%></p>
                                    <p class="list-group-item-text">
                                        处理人：
                                        <%# FormatActors(Eval("CurrentActors").ToString())%></p>
                                </div>
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div style="text-align: center">
                    <bw:VirtualMobilePager ID="listPager" runat="server" DisplayCurrentPage="true" style="display:none" 
                       ItemsPerPage="10" PagerStyle="NextPrev" PageButtonCount="5" BorderWidth="0px" OnPageIndexChanged="listPager_PageIndexChanged" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="listPager" EventName="PageIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="listResults" EventName="ItemDataBound" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="navbar navbar-default navbar-fixed-bottom ui-bar-inherit" role="navigation">
        <div class="btn-group btn-group-justified">
            <a class="btn ui-btn ui-icon-home ui-icon-top ui-footer-btn" role="button" href="default.aspx">
                工作台</a> <a class="btn ui-btn ui-icon-search ui-icon-top ui-footer-btn ui-btn-active"
                    role="button">高级查询</a> <a class="btn ui-btn ui-icon-exit ui-icon-top ui-footer-btn" role="button" style="display:none" onclick="window.history.go(-1);">返回</a>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" style="top: 60px">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel">
                        组合条件查询</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-6 col-md-6">
                            <div class="form-group">
                                <label for="exampleInputPassword1">
                                    发起时间</label>
                                <asp:TextBox runat="server" size="16" type="text" ID="dtpStart1" onfocus="this.blur()"
                                    CssClass="form_date form-control" />
                            </div>
                        </div>
                        <div class="col-xs-6 col-md-6">
                            <div class="form-group">
                                <label for="exampleInputPassword1">
                                    至</label>
                                <asp:TextBox runat="server" ID="dtpStart2" CssClass="form_date form-control" onfocus="this.blur()" />
                            </div>
                        </div>
                        <div class="col-xs-6 col-md-6">
                            <div class="form-group">
                                <label for="ddlWorkflowList">
                                    流程名称</label>
                                <asp:DropDownList ID="ddlWorkflowList" class="form-control" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlWorkflowList_SelectedIndexChanged">
                                    <asp:ListItem Value="" Text="－ 全部 －"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-xs-6 col-md-6">
                            <div class="form-group">
                                <label for="ddlActivityList">
                                    步骤</label>
                                <asp:UpdatePanel ID="updatepanelActivities" runat="server" UpdateMode="Conditional"
                                    RenderMode="Inline">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlActivityList" class="form-control" runat="server">
                                            <asp:ListItem Value="" Text="－ 全部 －"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlWorkflowList" EventName="selectedindexchanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="col-xs-6 col-md-6">
                            <div class="form-group">
                                <label for="form-control">
                                    发起人</label>
                                <asp:TextBox ID="txtCreator" runat="server" CssClass="form-control" placeholder="工单发起人"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-6 col-md-6">
                            <div class="form-group">
                                <label for="exampleInputPassword1">
                                    处理人</label>
                                <asp:TextBox ID="txtActor" runat="server" CssClass="form-control" placeholder="处理人"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-6 col-md-6">
                            <div class="form-group">
                                <label for="exampleInputPassword1">
                                    标题关键字</label>
                                <asp:TextBox ID="txtTitleKeywords" runat="server" CssClass="form-control" placeholder="标题关键字"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-6 col-md-6">
                            <div class="form-group">
                                <label for="txtSheetId">
                                    受理号</label>
                                <asp:TextBox CssClass="form-control" runat="server" ID="txtWorkId" placeholder="受理号" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-md-12">
                            <div class="form-group">
                                <label for="txtContentKeywords">
                                    内容关键字</label>
                                <asp:TextBox CssClass="form-control" runat="server" ID="txtContentKeywords" placeholder="内容关键字" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="btnclear" class="btn btn-default">
                        清空</button>
                    <button type="button" class="btn btn-primary" data-dismiss="modal" id="search">
                        搜索</button>
                    <asp:Button ID="btnSearch" Text="搜索" CssClass="btn btn-primary" runat="server" OnClick="btnSearch_Click" style="display:none"/>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#search").click(function () {
                if ($("#<%=dtpStart1.ClientID %>").val() == "") {
                    alert("请选择起始时间");
                }
                else if ($("#<%=dtpStart2.ClientID %>").val() == "") {
                    alert("请选择结束时间");
                }
                else {
                    $('.loading-backdrop').show();
                    $('.loading-backdrop').showLoading();
                    var timer = setInterval(function () {
                        if ($("resultsCompleted")) {
                            $('.loading-backdrop').hideLoading();
                            $('.loading-backdrop').hide();
                            $("#<%=listPager.ClientID %>").show();
                            bindPagerButtons();
                            clearInterval(timer);
                        }
                    }, 1000);
                    document.getElementById("<%=btnSearch.ClientID %>").click();
                }
            });
            $("#btnclear").click(function () {
                $(".modal-body input").attr("value", "");
                $(".modal-body select").attr("value", "");
            });
            $(".ui-footer-btn").click(function () {
                if ($(this).attr("class").indexOf("ui-btn-active") == -1)
                    $(".loading-backdrop").showLoading();
            });
            var nowdate = new Date();
            var olddate = getLastMonthYestdy(nowdate);
            $("#<%=dtpStart1.ClientID %>").attr("value", olddate);
            $("#<%=dtpStart2.ClientID %>").attr("value", GetDateStr(1));
            $('.loading-backdrop').hideLoading();
            $('.loading-backdrop').hide();
        });
        function bindPagerButtons() {
            $("#<%=listPager.ClientID %> li").click(function () {
                if ($(this).attr("class") == "active" || $(this).attr("class") == "disabled")
                    return;
                $('.loading-backdrop').show();
                $('.loading-backdrop').showLoading();
                var timer = setInterval(function () {
                    if ($("resultsCompleted")) {
                        $('.loading-backdrop').hideLoading();
                        $('.loading-backdrop').hide();
                        $("#<%=listPager.ClientID %>").show();
                        bindPagerButtons();
                        clearInterval(timer);
                    }
                }, 500);
            });
        }

        //获取当前日期的前后若干天
        function GetDateStr(AddDayCount) {
            var dd = new Date();
            dd.setDate(dd.getDate() + AddDayCount); //获取AddDayCount天后的日期
            var y = dd.getFullYear();
            var m = dd.getMonth() + 1; //获取当前月份的日期
            var d = dd.getDate();
            return y + "-" + m + "-" + d;
        }

        //获得上个月在昨天这一天的日期   
        function getLastMonthYestdy(date) {
            var daysInMonth = new Array([0], [31], [28], [31], [30], [31], [30], [31], [31], [30], [31], [30], [31]);
            var strYear = date.getFullYear();
            var strDay = date.getDate();
            var strMonth = date.getMonth() + 1;
            if (strYear % 4 == 0 && strYear % 100 != 0) {
                daysInMonth[2] = 29;
            }
            if (strMonth - 1 == 0) {
                strYear -= 1;
                strMonth = 12;
            }
            else {
                strMonth -= 1;
            }
            strDay = daysInMonth[strMonth] >= strDay ? strDay : daysInMonth[strMonth];
            if (strMonth < 10) {
                strMonth = "0" + strMonth;
            }
            if (strDay < 10) {
                strDay = "0" + strDay;
            }
            datastr = strYear + "-" + strMonth + "-" + strDay;
            return datastr;
        }  

    </script>
</asp:Content>
