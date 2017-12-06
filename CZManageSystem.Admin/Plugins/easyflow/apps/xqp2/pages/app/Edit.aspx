<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_app_Edit" Codebehind="Edit.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" runat="server">新增</asp:Literal>应用系统</span></h3>
    </div>
    <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 6px;">
        <tr>
            <th style="width: 20%;">应用系统名称：</th>
            <td>
                <asp:TextBox ID="txtAppName" CssClass="inputbox" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAppName" ControlToValidate="txtAppName" SetFocusOnError="true"
                    runat="server" Text="*" ErrorMessage="必须填写应用系统名称."></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                        ID="rev1" ControlToValidate="txtAppName" Display="Dynamic" ValidationExpression="^[a-zA-z0-9]+$"
                        runat="server" ErrorMessage="应用系统名称为应用系统的英文别名."></asp:RegularExpressionValidator>
                <span style="color:red;"><asp:Literal ID="ltlNameMessage" runat="server"></asp:Literal></span>
            </td>
        </tr>
        <tr id="Tr1" runat="server">
            <th>接入密码：</th>
            <td>
                <asp:TextBox ID="txtPassword" CssClass="inputbox" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" ControlToValidate="txtPassword" SetFocusOnError="true"
                    runat="server" Text="*" ErrorMessage="必须填写接入密码."></asp:RequiredFieldValidator>
            </td>
        </tr>
         <tr>
            <th>是否激活：</th>
            <td>
                <asp:CheckBox ID="chkEnable" runat="server" Checked="true" />激活

            </td>
        </tr>
        <tr>
            <th>备注：</th>
            <td>
                <asp:TextBox ID="txtRemark" CssClass="inputbox" TextMode="MultiLine" Columns="70"
                    Rows="5" runat="server"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <th>
                接入方式：

            </th>
            <td>
                <asp:RadioButtonList ID="rblAccessType" RepeatDirection="Horizontal" AutoPostBack="true"
                    runat="server" OnSelectedIndexChanged="rblAccessType_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="0">Web</asp:ListItem>
                    <asp:ListItem Value="1">WebService</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th>接入地址：</th>
            <td>
                <asp:TextBox ID="txtAccessUrl" CssClass="inputbox" Columns="70" TextMode="SingleLine" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr id="trFlows">
            <th>流程选择：</th>
            <td>
                <asp:CheckBoxList ID="cblWorkflows" RepeatDirection="Horizontal" RepeatColumns="3" runat="server">
                </asp:CheckBoxList>
            </td>
        </tr>
    </table>
    <p align="center">
        <asp:Button ID="btnEdit" runat="server" CssClass="btn_add" Text="保 存" OnClick="btnEdit_Click" />&nbsp;&nbsp;<input
            type="button" onclick="location='List.aspx'" value="返 回" class="btnReturnClass" />
    </p>    
     <script language="javascript" type="text/javascript">
         $(function() {
             $("#<%=txtAccessUrl.ClientID %>").attr("contentEditable", "false");

             var accessTypeName = "<%=rblAccessType.ClientID %>".replace("_", "$").replace("_", "$");
             var isService = $('input[@name$=' + accessTypeName + ']').get(1).checked;
             if (isService) {
                 $("#trFlows")[0].style.display = "none";
                 $("#<%=txtAccessUrl.ClientID %>").attr("contentEditable", "true");
             }
             else {
                 var accessUrl = $("#<%=txtAccessUrl.ClientID %>").val();
                 $("#<%=txtAppName.ClientID %>").keyup(function() {
                     var appName = $("#<%=txtAppName.ClientID %>").val();
                     $("#<%=txtAccessUrl.ClientID %>").val(accessUrl + appName);
                 });
             }
         });
    </script>
</asp:Content>
