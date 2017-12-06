<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_notices_controls_NoticeEditor" Codebehind="NoticeEditor.ascx.cs" %>
<div class="btnControl" style="margin-bottom:6px">
    <div class="btnLeft">
        <asp:Button ID="btnSave2" CssClass="btnSave" runat="server" Text="保存" onclick="btnSave_Click" CausesValidation="false" OnClientClick="return onValidateSave();" />
        <asp:Button ID="btnPublish2" CssClass="btnFW" runat="server" Text="发布" onclick="btnPublish_Click" />
    </div>
</div>	
<table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="padding:3px">
    <tr>
        <th>公告标题：</th>
        <td>
            <asp:TextBox ID="txtTitle" runat="server" Width="280px"></asp:TextBox>
            <asp:RequiredFieldValidator Id="rfvTxtTitle" runat="server" ControlToValidate="txtTitle" ErrorMessage="请输入标题." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
            <span style="color:red">*</span>
        </td>
    </tr>
     <tr class="trClass">
        <th>所属外部实体：</th>
        <td>
            <asp:DropDownList ID="ddlEntities" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>开始时间：</th>
        <td>
            <bw:DateTimePicker ID="dtpBegin" runat="server" IsRequired="true" RequiredErrorMessage="必须输入日期" DateType="OnlyDate" SetFocusOnError="true" Width="120px" />
            <span style="color:red">*</span>
        </td>
    </tr>
     <tr class="trClass">
        <th>结束时间：</th>
        <td>
            <bw:DateTimePicker ID="dtpEnd" runat="server" IsRequired="true" RequiredErrorMessage="必须输入日期" DateType="OnlyDate" SetFocusOnError="true" Width="120px" />
            <span style="color:red">*</span>
        </td>
    </tr>
    <tr>
        <th>公告内容：</th>
        <td>
            <asp:TextBox ID="txtContent" Columns="70" Height="200px" TextMode="MultiLine" runat="server"></asp:TextBox>
        </td>
    </tr>
</table>
 <div class="pageButtonList" style="margin-top:10px">
    <asp:Button ID="btnSave" CssClass="btn_sav" runat="server" Text="保存" onclick="btnSave_Click" CausesValidation="false" OnClientClick="return onValidateSave();" />
    <asp:Button ID="btnPublish" CssClass="btn_add" runat="server" Text="发布" onclick="btnPublish_Click" />
 </div>
 <script type="text/javascript">
 <!--//
 function onValidateSave(){
    if($("#<%=txtTitle.ClientID%>").val() == ""){
        alert("请输入公告标题.");
        $("#<%=txtTitle.ClientID%>").focus();
        return false;
    }
    return true;
 }
 //-->
 </script>
