<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_pms_pages_PMS_Export" Codebehind="PMS_Export.ascx.cs" %>
<script type="text/javascript">

    var timer;
    function checktable() {
        var tablecount = '<%=TotoalCount%>';
        if (tablecount <= 0) {
            alert("内容为空，不支持导出！");
            return false;
        }
        var b = new Base64();
        $("#tb_serch input[type='text']").each(function () {
            var v = $(this).val();
            if ($(this).attr("datatype") != "date")
                $(this).attr("value", b.encode(v));
        });
        $("#tb_serch input[type='hidden']").each(function () {
            var v = $(this).val();
            $(this).attr("value", b.encode(v));
        });
        return true;
    }
    function checkExecltable() {
        var tablecount = '<%=TotoalCount%>';
        if (tablecount <= 0) {
            alert("内容为空，不支持导出！");
            return false;
        }
        if (tablecount > 65536) {
            alert("数据量太大,请选择txt格式导出！");
            return false;
        }
        var b = new Base64();
        $("#tb_serch input[type='text']").each(function () {
            var v = $(this).val();
            if ($(this).attr("datatype") != "date")
                $(this).attr("value", b.encode(v));
        });
        $("#tb_serch input[type='hidden']").each(function () {
            var v = $(this).val();
            $(this).attr("value", b.encode(v));
        });

        return true;
    }

    var tager;
    var action;
    function checkTextAll() {
        var falg = checktable();
        if (falg) {

            $(document.body).showLoading({ 'addClass': 'loading-indicator-bars' });

            timer = setInterval("sendTimerAjax();return;", 300);
        }
        tager = document.forms[0].target;

        document.forms[0].target = "hide_iframe";

        return falg;
    }
 
    function sendTimerAjax() {
        $.post("OutAllByText.aspx", { loading: '1' }, function (e) {
            if (e == "True") {
                clearInterval(timer);
                $(".libox").hideLoading();
                $("#getDate").val("1");
                document.forms[0].submit();
                document.forms[0].target = tager;
                // document.forms[0].action = "";
                $("#getDate").val("");

            }
        });
    }
   
</script>
<div class="btnControl">
        <div class="btnRight">
            <iframe id="hide_iframe" name="hide_iframe" style="display: none" src="HTMLPage3.htm">
            </iframe>
            <input type="hidden" name="getData" id="getDate" value="" />
            <asp:Button runat="server" ToolTip="导出当前数据Excel" Text="导出当前页数据" CssClass="btnFW"  ID="btn_This_Expor_To_Exc" OnClick="btn_This_Expor_To_Exc_Click"
                 OnClientClick="return checktable();"/>
                <asp:Button runat="server" ToolTip="导出全部数据Excel" Text="导出全部数据" CssClass="btnFW" 
                    ID="btn_All_Export_To_Exc" OnClick="btn_All_Export_To_Exc_Click" OnClientClick="return checkExecltable();"/>
            
        <%--<fieldset style="width: 110px; float: left; margin-left: 5px; height: 70px;">
            <legend style="color: #247ECF; font-weight: bold">导出Txt </legend><span><a runat="server"
                id="btn_This_Expor_To_Txt" onserverclick="btn_This_Expor_To_Txt_Click" onclick="return checktable();">
                <img src="../../../App_Themes/gmcc/icons/export_07.gif" /></a></span> <a runat="server"
                    id="btn_All_Export_To_Txt" onserverclick="btn_All_Export_To_Txt_Click" onclick="return checktable();">
                    <img src="../../../App_Themes/gmcc/icons/export_09.gif" /></a>
        </fieldset>--%>
        <%--<fieldset style="width: 50px; float: left; margin-left: 5px; height: 70px;" id="field_set">
            <legend style="color: #247ECF; font-weight: bold">工具栏 </legend><a style="display: none;"
                id="btn_choice" onserverclick="btn_This_Expor_To_Exc_Click" href="#">
                <img src="../../../App_Themes/gmcc/icons/export_11.gif" /></a></fieldset>--%>
    </div>
</div>
<script>
    
</script>
