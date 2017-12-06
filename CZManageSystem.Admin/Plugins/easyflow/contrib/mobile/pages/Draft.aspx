<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_mobile_pages_Draft"
    MasterPageFile="~/plugins/easyflow/masters/Mobile.master" Title="草稿箱" Codebehind="Draft.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="page-header">
        <!-- Fixed navbar -->
        <div class="navbar navbar-inverse navbar-fixed-top ui-header" role="navigation">
            <h1 class="text-center ui-title" style="color: rgb(69, 125, 179);" id="header">
                草稿箱
            </h1>
        </div>
    </div>
    <div class=" container-fluid theme-showcase dataList" role="main" id="myTaskPage">
        <div>
            <div id="scroller" style="position: relative">
                <div class="list-group" id="thelist">
                    <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound"
                        OnItemCommand="rptList_ItemCommand">
                        <ItemTemplate>
                            <div class="list-group-item">
                    <div style="float: left; margin-top: .825em">
                        <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                    </div>
                    <div style="margin-left: 2.5em; margin-right:3.25em">
                    <a class="hover-one" onclick="$('.loading-backdrop').show();$('.loading-backdrop').showLoading();window.location='<%=AppPath %>contrib/mobile/pages/start.aspx?wiid=<%# Eval("WorkflowInstanceId") %>&returnurl=<%=this.Request.RawUrl %>'" style="cursor:pointer">
                        <h4 class="list-group-item-heading">
                            <%# Eval("Title")%></h4>
                        <p class="list-group-item-text">
                            受理号 <%# Eval("SheetId") %></p>
                     </a>
                    </div>
                    <div style="position: absolute; right:.5em; top:1.025em">
                        <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("WorkflowInstanceId") %>'
                                ID="btnDelete" CssClass="btn btn-danger" OnClientClick="if(confirm('确定删除该草稿吗？')){$('.loading-backdrop').show();$('.loading-backdrop').showLoading();}"
                                runat="server">删除</asp:LinkButton>
                    </div>
                </div>
                        </ItemTemplate>
                    </asp:Repeater>
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
    <script type="text/javascript">
        $(document).ready(function () {
            $(".ui-footer-btn").click(function () {
                $(".loading-backdrop").showLoading();
            });
            $(".loading-backdrop").hideLoading();
            $(".loading-backdrop").hide();
        });
    </script>
</asp:Content>
