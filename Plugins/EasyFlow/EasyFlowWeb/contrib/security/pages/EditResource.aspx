<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_security_pages_EditResource" Title="编辑权限资源" Codebehind="EditResource.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" runat="server"></asp:Literal>资源</span></h3>
    </div>    
    <div class="dataList">
        <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1">
            <tr>
                <th>资源编号：</th>
                <td>
                    <asp:TextBox ID="txtResourceId" runat="server" Text="A0" Width="160px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvResourceId" runat="server" ControlToValidate="txtResourceId" ErrorMessage="需要填写资源编号."></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr class="trClass">
                <th>资源别名：</th>
                <td>
                    <asp:TextBox ID="txtAlias" runat="server" Width="160px"></asp:TextBox>
                    <asp:CheckBox ID="chkboxEnabled" Checked="true" runat="server" />启用
                    <asp:RequiredFieldValidator ID="rfvAlias" runat="server" ControlToValidate="txtAlias" ErrorMessage="需要填写资源别名."></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th>父资源：</th>
                <td>
                    <asp:DropDownList ID="ddlParents" runat="server">
                        <asp:ListItem Value="00" Text=""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="trClass">
                <th>类型：</th>
                <td>
                    <asp:DropDownList ID="ddlTypes" runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="function">function</asp:ListItem>
                        <asp:ListItem Value="page">page</asp:ListItem>
                        <asp:ListItem Value="workflow">workflow</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>资源名：</th>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Width="350px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="需要填写资源名."></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
        <div style="text-align:center; margin-top:6px">
            <asp:Button ID="btnEdit" runat="server" Text="添加" CssClass="btn_add" 
                onclick="btnEdit_Click" />
            <input type="button" value="返回" class="btnReturnClass" onclick="history.go(-1);" />
        </div>
    </div>
    <script type="text/javascript">
    function setResourceId(ctrl){
        var prefixId = ctrl.value.toLocaleUpperCase();
        var ctrlValue = document.getElementById("<%=txtResourceId.ClientID%>").value;
        ctrlValue = ctrlValue.toLocaleUpperCase();
        if(ctrlValue.indexOf(prefixId) != 0){
            ctrlValue = prefixId + ctrlValue;
        }
        document.getElementById("<%=txtResourceId.ClientID%>").value = ctrlValue;
    }
    </script>
</asp:Content>
