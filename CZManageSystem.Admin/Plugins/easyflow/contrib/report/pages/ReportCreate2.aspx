<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_report_pages_ReportCreate2" Codebehind="ReportCreate2.aspx.cs" %>

<%@ Register Src="../controls/NavigationTools.ascx" TagName="NavigationTools" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">

    <script type="text/javascript" src="../res/js/reportCreate.js"></script>

    <div id="divList" runat="server">
        <div class="titleContent">
            <uc1:NavigationTools ID="NavigationTools1" runat="server" />
        </div>
        <div class="dataList">
            <h5>
                <input type="radio" name="createMode" onclick="window.location.href='ReportCreate.aspx'" />可视化模式
                <input type="radio" name="createMode" checked="checked" onclick="window.location.href='ReportCreate2.aspx'" />SQL模式
                <input type="radio" name="createMode" onclick="window.location.href='ReportCreate3.aspx'" />存储过程模式
            </h5>
            <table class='tblClass' cellspacing='0' border='1' style="border-collapse: collapse;">
                <tr>
                    <td align="right" style="width: 20%">
                        报表名称：
                    </td>
                    <td style="width: 80%">
                        <asp:TextBox ID="txtReportName" runat="server" Width="50%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtReportName" Display="Dynamic" ErrorMessage="报表名称不能为空" 
                            SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        备注：
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        SQL：
                    </td>
                    <td>
                        <asp:TextBox ID="txtSQL" runat="server" TextMode="MultiLine" Rows="10" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnPreview" runat="server" Text="预览" CssClass="btnReview" OnClick="btnPreview_Click" />
                        <asp:Button ID="btnSaveSql" runat="server" Text="保存" CssClass="btnSaveClass" OnClick="btnSaveSql_Click" />
                        <input type="button" value="返回" class='btnReturnClass' onclick="window.location.href('ReportList.aspx')" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                            ShowMessageBox="True" ShowSummary="False" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("form").submit(function () {
                var b = new Base64();
                var formContent = b.encode($("#<%=txtSQL.ClientID %>").val());
                $("#<%=txtSQL.ClientID %>").val(formContent);
            });
        });
    </script>
</asp:Content>
