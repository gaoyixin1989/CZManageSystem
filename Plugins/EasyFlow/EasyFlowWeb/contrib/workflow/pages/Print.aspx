<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="contrib_workflow_pages_Print" Codebehind="Print.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="CompletWorkFlowList" Src="../controls/ProcessHistoryLoader.ascx"%>
<%@ Register TagPrefix="bw" TagName="WorkflowView" Src="../controls/WorkflowView.ascx" %>
<%@ Register TagPrefix="bw" TagName="Attachment" Src="../controls/Attachments.ascx" %>
<%@ Register TagPrefix="bw" TagName="CommentList" Src="../controls/CommentList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>打印</title>
    <script type="text/javascript" src="../../../res/js/jquery-latest.pack.js"></script>
    <script type="text/javascript" src="../../../res/js/common.js"></script>
    <script type="text/javascript" src="script/dynamic-form.js"></script>
    <script type="text/javascript" src="script/autofull.js"></script>
    <script type="text/javascript" src="script/ItemsExtensions.js"></script>
    <script type="text/javascript" src="script/jquery.jqprint.js"></script>
    <script type="text/javascript" src="script/jquery.PrintArea.js"></script>
    <style media="print">
        .noprint
        {
            display: none;
        }
    </style>
    <script>
        //是否是IE浏览器
        /*var browser = navigator.appName
        var b_version = navigator.appVersion
        var version = b_version.split(";");
        var trim_Version = version[1].replace(/[ ]/g, "");
        if (browser == "Microsoft Internet Explorer" && (trim_Version == "MSIE10.0" || trim_Version == "MSIE7.0")) {
            //alert("由于IE兼容性问题，请勿使用IE10！");
            //window.close();
        }*/
    </script>
</head>
<body style="background: none !important; width:100%; padding:0px; margin:0px;" runat="server">
 <form id="form1" runat="server" style="padding:0px; margin:0px;" runat="server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="WorkflowAjaxService.asmx" />
            <asp:ServiceReference Path="GetDataAjaxService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div class="noprint" align="center">
        <div id="pageMaskLayout" style="display:none"></div>
        <div id="pageMaskContent" style="display:none">
            <span>正在处理，请稍候...</span>
        </div>
        <div class="showControl">
            <h4>摘要内容</h4>
        </div>
        <asp:Button CssClass="btnPassClass" 
            ID="btnUpdatePrint" Text="打印" runat="server" onclick="btnPrint_Click" style="display:none"/>
        <input type="button" value="打印" class="btnPassClass"  id="btnPrint"/>&nbsp;&nbsp;
        <input type="button" value="关闭" class="btnClass2"  id="btnClose" onclick="window.close();"/>
    </div>
    <div class="dataList" id="printArea" style="width:90%" align="left">
    <div style="text-align:center"><span style="text-align:center; margin:10 10 10 10"><h2><%=WorkflowName %></h2></span>
        <br />
        </div>
        <div class="dataTable">
            <bw:WorkflowView ID="ucWorkItemView" runat="server" IsReadonly="True" />
        </div>
        <div class="showControl">
            <h4>详细信息</h4>
            <button onclick="return showContent(this,'dataContent2');" title="收缩">
                <span>折叠</span>
            </button>
        </div>
        <div class="dataTable" id="dataContent2">
            <div id="divDynamicFormContainer" runat="server">
            </div>
            <div id="divPrintArea" style="display:none">
            </div>
        </div>
        <div id="divAttachments" runat="server">
            <div class="showControl">
                <h4>
                    附件信息</h4>
                <button onclick="return showContent(this,'dataContent4');" title="收缩">
                    <span>折叠</span></button>
            </div>
            <div class="dataTable" id="dataContent4">
                <bw:Attachment ID="ucAttachment" runat="server" EnableUpload="false" />
            </div>
        </div>
        <div class="showControl">
            <h4>
                处理信息</h4>
            <button onclick="return showContent(this,'dataContent3');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataContent3">
            <bw:CompletWorkFlowList ID="ucCompletWorkFlowList" runat="server" />
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        /*$(function() {
            for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {
                bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
            }
            //工单查看时默认表单项为只读
            $("#<%=divDynamicFormContainer.ClientID %> input").each(function() {
                if ($(this).attr("type") == "hidden") return;
                if ($(this).attr("type") == "checkbox" || $(this).attr("type") == "radio") {
                    if ($(this).attr("checked")) {
                        $(this).after($(this).val());
                        $(this).click(function() { return false; });
                    }
                    else $(this).hide();

                    $(this).next("<span>").hide();
                }
                else {
                    $(this).hide();
                    $(this).after($(this).val());
                    if ($(this).next(".ico_pickdate"))
                        $(this).next(".ico_pickdate").hide();
                }
            });
            $("#<%=divDynamicFormContainer.ClientID %> textarea").each(function() {
                $(this).hide();
                $(this).after($(this).val());
            });
            $("#<%=divDynamicFormContainer.ClientID %> select").each(function() {
                $(this).hide();
                $(this).after($(this).val());
            });
        });*/
    </script>
    <script type="text/javascript">
        document.title = "";
        $(document).ready(function() {
            //if ("<%=CanPrint %>" == "false")
            //$("#btnPrint").css("display", "none");
            //else if ("<%=CanPrint %>" == "true") {
            //showMaskLayout();
            //setTimeout(function() { $(".dataList").printArea(); closeMaskLayout(); }, 5000);

            //}

            $("#btnPrint").click(function() {
                if (window.confirm("您确定打印吗？")) {
                    showMaskLayout();
                    $("#<%=divDynamicFormContainer.ClientID %>").attr("class", "noprint");
                    $("#<%=divDynamicFormContainer.ClientID %>").hide();
                    $("#divPrintArea").show();
                    $("#divPrintArea").html($("#<%=divDynamicFormContainer.ClientID %>").html());
                    $("#<%=divDynamicFormContainer.ClientID %>").html("");
                    jQuery.post("PrintHandler.ashx", { aiid: "<%=ActivityInstanceId %>", wiid: "<%=WorkflowInstanceId %>", command: "update" },
                    function(data) {

                    });

                    $(".dataList").printArea();
                    //alert(result);
                    window.close();
                }
                //document.getElementById("<%=btnUpdatePrint.ClientID %>").click();
            });
        });

        // div 覆盖层代码.
        function showMaskLayout() {
            $("#pageMaskLayout").css("display", "block");
            $("#pageMaskContent").css("display", "block");
            var bodyHeight = $("body").height();
            var bodyWidth = $("body").width();
            $("#pageMaskContent").css("top", bodyHeight - 200);
            $("#pageMaskContent").css("left", bodyWidth / 2 - 100);
        }
        function closeMaskLayout() {
            $("#pageMaskLayout").css("display", "none");
            $("#pageMaskContent").css("display", "none");
        }
    </script> 
    </form>
</body>
</html>
