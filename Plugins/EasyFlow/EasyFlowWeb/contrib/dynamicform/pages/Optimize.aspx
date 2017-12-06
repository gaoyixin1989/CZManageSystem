<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_Optimize" Title="优化" Codebehind="Optimize.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div>
        <table class="tblGrayClass">
            <tr>
                <th style="width:20%">分表序号:</th>
                <td>
                    <asp:TextBox ID="txtNumber" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTxtNumber" ControlToValidate="txtNumber" runat="server" ErrorMessage="请输入数据分表序号." Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revTxtNumber" runat="server" ControlToValidate="txtNumber" ValidationExpression="(\d)+" ErrorMessage="格式不正确." Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th>迁移表单数据数:</th>
                <td>
                    <asp:TextBox ID="txtTopValue" runat="server" Text="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTxtTopValue" ControlToValidate="txtTopValue" runat="server" ErrorMessage="请输入迁移的数据数，建议小于 1000，多次迁移, 保证数据正确." Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revTxtTopValue" runat="server" ControlToValidate="txtTopValue" ValidationExpression="(\d)+" ErrorMessage="格式不正确." Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th></th>
                <td>
                    <asp:Button ID="btnTablePart" runat="server" Text="执行分表." 
                        onclick="btnTablePart_Click" />
                </td>
            </tr>
        </table>
        
    </div>
    <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
</asp:Content>
