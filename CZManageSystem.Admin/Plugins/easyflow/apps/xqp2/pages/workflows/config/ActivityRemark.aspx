<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ActivityRemark" Codebehind="ActivityRemark.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>处理意见设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="dataList">
    <div class="dataTable" id="divCommentOptions" style="margin-bottom:10px;">
            <fieldset style="margin-bottom:5px;">
                <legend>新增审批意见</legend>
                <table align="center">
                    <tr>
                        <td>
                            显示名称
                            <asp:TextBox ID="txtRemarkText" runat="server" Width="100px" CssClass="inputbox"></asp:TextBox>
                            内容
                            <asp:TextBox ID="txtRemarkValue" runat="server" Width="300px" CssClass="inputbox"></asp:TextBox>
                            <asp:Button ID="btnInsertRemark" runat="server" CssClass="btn_add" Text="添加" 
                                onclick="btnInsertRemark_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvTxtRemarkName" ControlToValidate="txtRemarkText" runat="server" Text="*" Display="Dynamic">- 必须填写意见显示名称.<br /></asp:RequiredFieldValidator>                        </td>
                    </tr>
                </table>
            </fieldset>        
            <asp:GridView ID="gridviewRemarks" runat="server" DataKeyNames="Id" 
                AutoGenerateColumns="False" BorderWidth="0px" 
                CssClass="tblClass"                RowStyle-HorizontalAlign="Center" 
                HeaderStyle-HorizontalAlign="Center" 
                onrowdatabound="gridviewRemarks_RowDataBound" 
                onrowediting="gridviewRemarks_RowEditing"  
                onrowcancelingedit="gridviewRemarks_RowCancelingEdit" 
                onrowupdating="gridviewRemarks_RowUpdating" 
                onrowdeleting="gridviewRemarks_RowDeleting">
                <RowStyle HorizontalAlign="Center"></RowStyle>
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <asp:Literal ID="ltlNumber" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="意见名称">
                        <ItemTemplate>
                            <%# Eval("RemarkText")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemarkText" runat="server" CssClass="inputbox" Width="100px" Text='<%# Eval("RemarkText") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="意见内容">
                        <ItemTemplate>
                            <%# Eval("RemarkValue")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemarkValue" runat="server" CssClass="inputbox" Width="300px" Text='<%# Eval("RemarkValue") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle Width="60%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:CommandField HeaderText="操作" EditText="修改" UpdateText="更新" CancelText="取消" CausesValidation="false" ShowEditButton="true" ShowCancelButton="true" />
                    <asp:TemplateField HeaderText="删除">
                        <ItemTemplate>
                            <asp:LinkButton ID="linkbtnDelete" runat="server" CommandName="Delete" Text="删除" CausesValidation="false"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <AlternatingRowStyle CssClass="trClass" />
            </asp:GridView>
            <asp:Literal ID="ltlRemarkMsg" runat="server"></asp:Literal>
        </div>
    </div>
    </form>
</body>
</html>
