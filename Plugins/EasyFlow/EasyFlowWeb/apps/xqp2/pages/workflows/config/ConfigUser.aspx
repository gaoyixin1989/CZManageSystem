<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_workflows_config_ConfigUser" Title="个人设置" Codebehind="ConfigUser.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>个人设置</span></h3>    
    </div>
    <div class="dataList">
        <div class="showControl" style="display:none">
            <h4>常用审批意见</h4>
            <button onclick="return showContent(this,'divCommentOptions');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable" id="divCommentOptions" style="margin-bottom:10px;display:none">
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
            <asp:GridView ID="gridviewRemarks" runat="server" DataKeyNames="Id"  Visible="false"
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
        
        <div class="showControl">
            <h4>通知设置</h4>
            <button onclick="return showContent(this,'divNotifyOptions');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable" id="divNotifyOptions">
            <div>注：如果是紧急重要单，在禁用消息通知的情况下系统仍然会发送。</div>
            <asp:GridView ID="gviewNotify" runat="server" DataKeyNames="WorkflowName" 
                ShowFooter="true" AutoGenerateColumns="false" BorderWidth="0" 
                CssClass="tblClass" HeaderStyle-HorizontalAlign="Center" 
                RowStyle-HorizontalAlign="Center" onrowdatabound="gviewNotify_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <asp:Literal ID="ltlNumber" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:BoundField DataField="WorkflowName" HeaderText="流程名称" ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderText="允许待办邮件">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkemail" runat="server" Checked='<%# Eval("EnableEmail") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <input type="checkbox" onclick="onToggleNotify('chkemail', this.checked);" title="选择禁用全部的邮件通知。" />全选

                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="允许待办短信">
                        <ItemTemplate>
                            <asp:CheckBox ID="chksms" runat="server" Checked='<%# Eval("EnableSms") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <input type="checkbox" onclick="onToggleNotify('chksms', this.checked);" title="选择禁用全部的短信通知。" />全选

                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="允许待阅邮件">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkcsemail" runat="server" Checked='<%# Eval("EnableReviewEmail") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <input type="checkbox" onclick="onToggleNotify('chkcsemail', this.checked);" title="选择禁用全部的邮件通知。" />全选

                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="允许待阅短信">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkcssms" runat="server" Checked='<%# Eval("EnableReviewSms") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <input type="checkbox" onclick="onToggleNotify('chkcssms', this.checked);" title="选择禁用全部的短信通知。" />全选

                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>                
                <AlternatingRowStyle CssClass="trClass" />
                <FooterStyle HorizontalAlign="Center" />
            </asp:GridView>
            <div class="pageClass" style="text-align:center;">
                <asp:Button ID="btnSaveNotify" runat="server" CausesValidation="false" CssClass="btn_sav" Text="保存" onclick="btnSaveNotify_Click" />
            </div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
    // <!CDATA[
    function onToggleNotify(chkName, isChecked){
        var inputArray = document.getElementsByTagName("input");
		for(var i=0; i<inputArray.length; i++) {
			if (inputArray[i].type == "checkbox" && inputArray[i].name.indexOf(chkName) != -1) {
				inputArray[i].checked = isChecked;
			}
		}
    }
    // ]]>
    </script>
</asp:Content>
