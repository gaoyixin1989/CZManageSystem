<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_Create" Codebehind="Create.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="divList" runat="server">
        <div class="titleContent">
            <h3><span>新增表单</span></h3>
        </div>
        <table id="Table1" style="border-collapse: collapse" bordercolor="#cccccc" cellspacing="0"
            cellpadding="0" width="98%" align="center" border="1">
            <tr>
                <td height="24">&nbsp;表单名称：</td>
                <td height="24">
                    <asp:TextBox ID="txtName" runat="server" Width="250px" CssClass="inputbox"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" ErrorMessage="请输入表单名称"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td height="24">&nbsp;备注：</td>
                <td height="24">
                    <asp:TextBox ID="txtComment" Columns="50" Rows="10" TextMode="MultiLine" runat="server"
                        CssClass="textarea"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSaveFormDefinition" runat="server" CssClass="btn_sav" Text="保 存" OnClick="btnSaveFormDefinition_Click" />
                     &nbsp; &nbsp; &nbsp; &nbsp;<input type="button" class="btn_ext" onclick="location='formIndex.aspx?wid=<%=EntityId %>'" value="返回" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
