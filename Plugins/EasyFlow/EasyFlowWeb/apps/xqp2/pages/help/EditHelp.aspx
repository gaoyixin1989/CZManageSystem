<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_help_EditHelp" Codebehind="EditHelp.aspx.cs" %>

<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="titleContent">
        <h3>
            <span><asp:Literal ID="ltlTitle" runat="server" Text="新增" />帮助文档</span></h3>
    </div>
    <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 10px">
        <tr>
            <th width="18%">
                标题：

            </th>
            <td>
                <asp:TextBox ID="txtTitle" runat="server" Width="500px"></asp:TextBox><span style="color: red">*</span><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle" ErrorMessage="标题必填!"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr class="trClass">
            <th>
                内容：

            </th>
            <td>
                <FCKeditorV2:FCKeditor ID="FCKContent" runat="server" Height="400px" BasePath="../../../../res/fckeditor/">
                </FCKeditorV2:FCKeditor>
            </td>
        </tr>
        <tr>
            <th>
                父节点：
            </th>
            <td>
                <asp:DropDownList ID="ddlHelps" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="trClass">
            <th>
                显示顺序：

            </th>
            <td>
                <asp:TextBox ID="txtShowOrder" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnSave" runat="server" CssClass="btn_sav" Text="保存" OnClick="btnSave_Click">
    </asp:Button>&nbsp;&nbsp;&nbsp;
    <input type="button" value="返回" onclick="window.location='default.aspx';" class="btnReturnClass" />
</asp:Content>
