<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Mobile.master" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_Preview" Codebehind="Preview.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<script type="text/javascript" language="javascript" src="scripts/dynamic-form.js"></script>
<div class="page-header">
        <!-- Fixed navbar -->
        <div class="navbar navbar-inverse navbar-fixed-top ui-header" role="navigation">
            <h1 class="text-center ui-title" style="color: White" id="header">
                表单预览
            </h1>
        </div>
    </div>
    <div class=" container-fluid theme-showcase dataList" role="main">
        <%--<div class="showControl">
            <h4>
                基本信息</h4>
            <button onclick="return showContent(this,'dataReviewHistoryList');" title="收缩">
                <span></span></button>
        </div>
        <div class="dataTable">
            <div class="row">
                <div class="col-xs-12 colspan">
                    <div class="th" style="width: 5.25em">
                        标题</div>
                    <div class="td" style="margin-left: 5.25em">
                        演示标题</div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-md-4 col">
                    <div class="th" style="width: 5.25em">
                        受理号</div>
                    <div class="td" style="margin-left: 5.25em">
                        AA141209001</div>
                </div>
                <div class="col-xs-12 col-md-4 col">
                    <div class="th" style="width: 5.25em">
                        发起人</div>
                    <div class="td" style="margin-left: 5.25em">
                        系统管理员</div>
                </div>
                <div class="col-xs-12 col-md-4 col">
                    <div class="th" style="width: 5.25em">
                        联系电话</div>
                    <div class="td" style="margin-left: 5.25em">
                        123456789</div>
                </div>
                <div class="col-xs-12 col-md-4 col">
                    <div class="th" style="width: 5.25em">
                        创建时间</div>
                    <div class="td" style="margin-left: 5.25em">
                        0001-01-01 00:00:00</div>
                </div>
                <div class="col-xs-12 col-md-4 col">
                    <div class="th" style="width: 5.25em">
                        步&nbsp;&nbsp;骤</div>
                    <div class="td" style="margin-left: 5.25em">
                        演示步骤</div>
                </div>
                <div class="col-xs-12 col-md-4 col">
                    <div class="th" style="width: 5.25em">
                        处理人</div>
                    <div class="td" style="margin-left: 5.25em">
                        系统管理员</div>
                </div>
            </div>
            <div class="row" style="border-top: 0;">
                <div class="col-xs-12 colspan" style="border-bottom: solid 1px #DFDFE8;">
                    <div class="th" style="width: 5.25em;">
                        流程名称</div>
                    <div class="td" style="margin-left: 5.25em; font-weight: bold; color: blue;">
                        演示名称</div>
                </div>
            </div>
        </div>--%>
        <div class="showControl">
            <h4>
                表单信息</h4>
            <button  title="收缩">
                <span></span></button>
        </div>
        <div id="divPreview">
            
        </div>
        
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divPreview").html($("#divWapPreview", window.parent.document).html());
            $("#divPreview input:text").each(function () {
                this.className = "form-control";
                if($(this).attr("name").indexOf("_dat_")!=-1)
                    this.className = "form-control form_datetime";
            });
            $("#divPreview textarea").each(function () {
                this.className = "form-control";
            });

            for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {
                bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
            }    
        });
    </script>
</asp:Content>

