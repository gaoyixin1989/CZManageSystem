<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="Workflow_Extension_WorkflowHelper" EnableEventValidation="false" Codebehind="WorkflowHelper.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server" />

    <div class="titleContent">
        <h3>
            <span>任务改派功能</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>
                任务改派</h4>
            <button onclick="return showContent(this,'tb1');" title="收缩">
                <span>折叠</span></button>
        </div>
        <table id="tb1" class="tblGrayClass grayBackTable" style="word-break: break-all" cellspacing="1" cellpadding="4" border="0">
            <tr>
                <th width="20%" align="right">
                    工单号：
                </th>
                <td width="80%">
                    <asp:TextBox ID="txtSheetId" runat="server"></asp:TextBox>&nbsp;
                    
                    <asp:Button ID="btnGetSheetInfo" runat="server" ToolTip="获取工单信息及改派步骤列表" Text="获取" 
                        onclick="btnGetSheetInfo_Click" CssClass="btn" />                        
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    工单信息：

                </th>
                <td width="80%">
                    <asp:UpdatePanel ID="updatepanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Label ID="lblSheetInfo" runat="server" Text=""></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGetSheetInfo" EventName="click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    改派步骤：

                </th>
                <td width="80%">
                    <asp:UpdatePanel ID="updatepanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTransferActivityList" runat="server">
                            </asp:DropDownList> &nbsp;(不选, 则默认当前步骤不变)
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGetSheetInfo" EventName="click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    改派对象用户名：
                </th>
                <td width="80%">
                <div id = "ToUser">
                    <asp:HiddenField ID="txtToUser" runat="server"></asp:HiddenField>
                    <asp:TextBox ID="txtToReal" runat="server" ReadOnly="true"></asp:TextBox>
                    <a href="#" id="pickResetUsers" class="ico_pickdate" onclick="javascrpt:return openUserSelector();">
                    <%--<a href="#" id="pickResetUsers" class="ico_pickdate" onclick="javascrpt:return openUserSelector('ToUser');">--%>
                        选择用户</a>
                   </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnTransfer" runat="server" OnClientClick="return beforeTransfer();"
                        OnClick="btnTransfer_Click" Text="任务改派" CssClass="btnClass2l" />
                    &nbsp;
                        <asp:Button ID="btn_CloseFlow" runat="server"
                         Text="结束工单" CssClass="btnClass2l" onclick="btn_CloseFlow_Click" />
                    &nbsp;
                        <asp:Button ID="btn_CannalFlow" runat="server"
                         Text="作废工单" CssClass="btnClass2l" onclick="btn_CannalFlow_Click" />
                         <input type="button" id="btn_Dealing" disabled="disabled" value="正在处理..." style="display:none" class="btnClass2l" />
                </td>
            </tr>
        </table>
        <p>
        </p>
        <p>
        </p>
    </div>
<script type="text/javascript">
    function beforeTransfer() {
        var sheetId = document.getElementById("<%=txtSheetId.ClientID %>").value;
        var toUser = document.getElementById("<%=txtToUser.ClientID %>").value;
        if (sheetId.length == 0) {
            alert('请输入要改派的工单号');
            return false;
        }
        if (toUser.length == 0) {
            alert('请选择改派对象用户名');
            return false;
        }
        if(confirm("确定改派？"))
            return true;
        return false;
    }

    function openUserSelector() {
        var h = 450;
        var w = 700;
        var iTop = (window.screen.availHeight - 30 - h) / 2;
        var iLeft = (window.screen.availWidth - 10 - w) / 2;
        var selector = "<%=AppPath %>contrib/security/pages/PopupUserPicker2.aspx?func=onCompletePickReviews";
        window.open(selector, '', 'height=' + h + ', width=' + w + ', top=' + iTop + ', left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');
        return false;
    }

    function onCompletePickReviews(result) {
        var values = "";
        var names = "";
        for (var i = 0; i < result.length; i++) {
            values += ("," + result[i].key);
            names += ("," + result[i].value);
            break;
        }
        if (values.substring(0, 1) == ",")
            values = values.substring(1, values.length);
        if (names.substring(0, 1) == ",")
            names = names.substring(1, names.length);
        $("#<%=txtToUser.ClientID%>").attr("value", values);
        $("#<%=txtToReal.ClientID%>").attr("value", names);
    }

    $(function() {
        $('#<%=btn_CloseFlow.ClientID %>').click(function() {
            var sheetId = document.getElementById("<%=txtSheetId.ClientID %>").value;
            if (sheetId.length == 0) {
                alert('请输入要改派的工单号');
                return false;
            }
            else {
                if (confirm('您确定要结束工单吗？')) {
                    $('#<%=btn_CannalFlow.ClientID %>').css('display', 'none');
                    $('#<%=btn_CloseFlow.ClientID %>').css('display', 'none');
                    $("#btn_Dealing").show();
                }
                else
                    return false;
            }
        });
        $('#<%=btn_CannalFlow.ClientID %>').click(function () {
            var sheetId = document.getElementById("<%=txtSheetId.ClientID %>").value;
            if (sheetId.length == 0) {
                alert('请输入要改派的工单号');
                return false;
            }
            else {
                if (confirm('您确定要作废工单吗？')) {
                    $('#<%=btn_CannalFlow.ClientID %>').css('display', 'none');
                    $('#<%=btn_CloseFlow.ClientID %>').css('display', 'none');
                    $("#btn_Dealing").show();
                }
                else
                    return false;
            }
        });
    });

</script>
</asp:Content>
