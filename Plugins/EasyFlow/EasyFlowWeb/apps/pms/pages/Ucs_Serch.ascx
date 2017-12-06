<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_pms_pages_Ucs_Serch" Codebehind="Ucs_Serch.ascx.cs" %>
<script type="text/javascript">
    $(document).ready(function () {
        $("#btnclear").click(function () {
            //            $("#ctl00_cphBody_con_Pms_txt_StartData").val("");
            //            $("#ctl00_cphBody_con_Pms_txt_EndData").val("");
            //            $("#ctl00_cphBody_con_Pms_select_Brnd").val("");
            //            $("#ctl00_cphBody_con_Pms_select_Area").val("");
            //            $("#ctl00_cphBody_con_Pms_select_city").val("");
            //            $("#ctl00_cphBody_con_Pms_select_Brnd2").val("");
            //            $("#ctl00_cphBody_con_Pms_select_Area2").val("");
            //            $("#ctl00_cphBody_con_Pms_select_city2").val("");
            //            $("#ctl00_cphBody_con_Pms_txt_Code").val("");
            //            $("#ctl00_cphBody_con_Pms_txt_Name").val("");
            //            $("#ctl00_cphBody_con_Pms_txt_StartData2").val("");
            //            $("#ctl00_cphBody_con_Pms_txt_EndData2").val("");
            //            $("#ctl00_cphBody_con_Pms_select_Num").val("");

            var inputText = $(".toolBlock input[type=text]");
            var inputHidden = $(".toolBlock input[type=hidden]");
            var inputHight = $(".div_high_serch input[type=text]");
            if (inputText.length > 0) { inputText.val(""); }
            if (inputHidden.length > 0) { inputHidden.val(""); }
            if (inputHight.length > 0) { inputHight.val(""); }
        });
    
        $("#<%=btn_Seach.ClientID %>").click(function () {
            encodeStr();    
        });
    });
    function encodeStr() {
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
    }
    /*function htmlencode(str) {
            str = str.replace(/&/g, '&amp');
            str = str.replace(/</g, '&lt;');
            str = str.replace(/>/g, '&gt;');
            str = str.replace(/(?:t| |v|r)*n/g, '');
            str = str.replace(/ /g, '');
            str = str.replace(/x22/g, '');
            str = str.replace(/x27/g, '');
            return str;
        }*/
</script>
    <div class="toolBlock"  style="text-align: left;border-bottom: solid 1px #C0CEDF; margin-bottom: 10px;padding-bottom: 5px;">
        <span style="margin-right: 5px" runat="server">
            <table width="99%" id="tb_serch">
                <tr>
                    <td colspan="2">
                        <%=sbUp.ToString() %>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top:5px;">
                        <%=sbDown.ToString()%>
                    </td>
                    <td style="text-align: right">
                        <input id="btn_high_Seach" type="button" class="btn_query" value="高级搜索" style="display: none"
                            onclick="high_Seach()" />
                        <asp:CheckBox runat="server" ToolTip="统计到人" Text="统计到人" 
                    ID="chkPeople" Visible="false"/>
                        <asp:Button ID="btn_Seach" CssClass="btn_query" runat="server" Text="搜索" OnClientClick="putin()"
                            OnClick="btn_Seach_Click"></asp:Button>
                        <input id="btnclear" type="button" class="btn_query" value="清空" />
                        <input id="btn_goback" type="button" class="btn_query" value="返回" onclick="history.back();" />
                    </td>
                </tr>
            </table>
        </span>
    </div>
    <div class="div_high_serch" id="div_hightSertch">
        <table>
            <tr>
                <%=sbHightSeach.ToString()%>
            </tr>
        </table>
    </div>
