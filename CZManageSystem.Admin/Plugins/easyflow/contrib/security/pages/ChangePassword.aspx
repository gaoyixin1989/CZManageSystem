<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_security_pages_ChangePassword" Title="修改密码" Codebehind="ChangePassword.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<div class="titleContent">
        <h3><span>修改密码</span></h3>
    </div>
    <div>
        <fieldset style="margin:10px;">
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" align="center" style="line-height: 22px;" onkeypress="javascript:return WebForm_FireDefaultButton(event, '<%=btnChangePassword.ClientID%>')">
                <%--<tr>
                    <th style="text-align:right; width:20%">原密码：</th>
                    <td>
                        <asp:TextBox ID="textOldPassword" TextMode="Password" CssClass="inputbox" Width="150px"
                            runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rgvOldPassword" runat="server" ErrorMessage="- 必须输入原密码。"
                            ControlToValidate="textOldPassword">*</asp:RequiredFieldValidator>
                    </td>
                </tr>--%>
                <tr class="trClass">
                    <th style="text-align:right">新密码：</th>
                    <td>
                        <asp:TextBox ID="textNewPassword" TextMode="Password" CssClass="inputbox" Width="150px"
                            runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rgvNewPassword" runat="server" ErrorMessage="- 必须输入新密码。"
                            ControlToValidate="textNewPassword">*</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <th style="text-align:right">确认新密码：</th>
                    <td>
                        <asp:TextBox ID="textNewPassword2" TextMode="Password" CssClass="inputbox" Width="150px"
                            runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rgvNewPassword2" runat="server" ControlToValidate="textNewPassword2"
                            ErrorMessage="- 必须确认新密码。" Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvNewPassword" runat="server" ControlToCompare="textNewPassword"
                            ControlToValidate="textNewPassword2" Display="Dynamic" ErrorMessage="- 两次输入的新密码不一致。">*</asp:CompareValidator></td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnChangePassword" runat="server" CssClass="btn_submit" Text="提交"
                            OnClick="btnChangePassword_Click" /></td>
                </tr>
            </table>
            <div style="text-indent:20%">
                <asp:ValidationSummary ID="vsummary1" runat="server" />
            </div>
        </fieldset>
    </div>
</asp:Content>
