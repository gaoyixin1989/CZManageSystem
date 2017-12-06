<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_security_pages_EditUser" Title="编辑用户"  Codebehind="EditUser.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="toolkitScriptManager1" runat="server" />
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" runat="server" Text="新增"></asp:Literal>用户</span></h3>
    </div>    
    <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
        <tr>
            <th style="width:20%;">用户名</th>
            <td>
                <asp:TextBox ID="txtUserName" CssClass="inputbox" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUserName" ControlToValidate="txtUserName" SetFocusOnError="true" runat="server" Text="*" ErrorMessage="必须填写用户名."></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="trPassword" runat="server">
            <th>密码</th>
            <td>
                <asp:TextBox ID="txtPassword" CssClass="inputbox" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="trClass">
            <th>真实姓名</th>
            <td>
                <asp:TextBox ID="txtRealName" CssClass="inputbox" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvRealName" ControlToValidate="txtRealName" SetFocusOnError="true" runat="server" Text="*" ErrorMessage="必须填写真实姓名."></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>工号</th>
            <td>
                <asp:TextBox ID="txtEmployeeId" CssClass="inputbox" Width="280px" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="trClass">
            <th>所属部门</th>
            <td>
                <asp:TextBox ID="txtDepartment" CssClass="inputbox" Width="360px" runat="server"></asp:TextBox>
                <ajaxToolkit:AutoCompleteExtender ID="autoComplete1" runat="server" 
                    BehaviorID="AutoCompleteEx"  TargetControlID="txtDepartment" 
                    ServicePath="SecurityAjaxService.asmx" ServiceMethod="GetCompletionDepartments" 
                    MinimumPrefixLength="2" CompletionInterval="1000" 
                    EnableCaching="true" CompletionSetCount="20" DelimiterCharacters=";, :"
                    CompletionListCssClass="autocomplete_completionListElement" 
                    CompletionListItemCssClass="autocomplete_listItem" 
                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                    <Animations>
                        <OnShow>
                            <Sequence>
                                <OpacityAction Opacity="0" />
                                <HideAction Visible="true" />
                                <ScriptAction Script="
                                    // Cache the size and setup the initial size
                                    var behavior = $find('AutoCompleteEx');
                                    if (!behavior._height) {
                                        var target = behavior.get_completionList();
                                        behavior._height = target.offsetHeight - 2;
                                        target.style.height = '0px';
                                    }" />
                                <Parallel Duration=".4">
                                    <FadeIn />
                                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx')._height" />
                                </Parallel>
                            </Sequence>
                        </OnShow>
                        <OnHide>
                            <Parallel Duration=".4">
                                <FadeOut />
                                <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx')._height" EndValue="0" />
                            </Parallel>
                        </OnHide>
                    </Animations>
                </ajaxToolkit:AutoCompleteExtender>
                        
            </td>
        </tr>
        <tr>
            <th>固定电话</th>
            <td>
                <asp:TextBox ID="txtTel" CssClass="inputbox" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revTel" runat="server" 
                    ControlToValidate="txtTel" SetFocusOnError="True" Text="*" ErrorMessage="请填写正确的固定电话." 
                    ValidationExpression="^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$"></asp:RegularExpressionValidator>
            </td>
        <tr class="trClass">
        </tr>
            <th>手机</th>
            <td>
                <asp:TextBox ID="txtMobile" CssClass="inputbox" runat="server"></asp:TextBox>
                <%--<asp:RegularExpressionValidator ID="revMobile" runat="server" 
                    ControlToValidate="txtMobile" SetFocusOnError="True" Text="*" ErrorMessage="请填写正确的手机号码." 
                    ValidationExpression="^((\(\d{3}\))|(\d{3}\-))?((13\d{9})|(15\d{9}))$"></asp:RegularExpressionValidator>--%>
                <asp:RegularExpressionValidator ID="revMobile" runat="server" 
                    ControlToValidate="txtMobile" SetFocusOnError="True" Text="*" ErrorMessage="请填写正确的手机号码." 
                    ValidationExpression="^[1]+[1,2,3,4,5,6,7,8,9,0]+\d{9}"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>电子邮箱</th>
            <td>
                <asp:TextBox ID="txtEmail" CssClass="inputbox" Width="280px" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="txtEmail" SetFocusOnError="True" Text="*" ErrorMessage="请填写正确的电子邮箱."
                    ValidationExpression="^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$"></asp:RegularExpressionValidator>
            </td>
        </tr>
    </table>
    <p align="center">
        <asp:Button ID="btnEdit" runat="server" CssClass="btn_add" Text="新增" 
            onclick="btnEdit_Click"/>&nbsp;&nbsp;<input type="button" value="返 回" id="btnBack" onclick="window.location='users.aspx';" class="btnReturnClass" />
    </p>
    <div>
        <asp:ValidationSummary ID="vsummary1" runat="server" ShowSummary="false" ShowMessageBox="true" />
    </div>
    
<script language="javascript" type="text/javascript">
// <!CDATA[
 var emailDomain = "@cz.gd.chinamobile.com";
function autoSetEmail(txt){
    var account = txt.value;
    var emailText = document.getElementById("<%=txtEmail.ClientID%>").value;
    var email = "";
    var startIndex = emailText.indexOf("@", 0);
    if(startIndex > -1){
        emailDomain = emailText.substring(startIndex, emailText.length);
    }
    if(account != "" && emailDomain  != ""){
        email = account + emailDomain;
        document.getElementById("<%=txtEmail.ClientID%>").value = email;
    }
}
// ]]>
</script>
</asp:Content>
