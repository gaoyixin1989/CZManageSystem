<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_Import" Codebehind="Import.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" runat="server"></asp:Literal>导入表单</span></h3>
    </div>
    <div class="btnControl">
        <div class="btnLeft">
            <input type="button" value="返回" class="btnNewwin" onclick="window.location.href='list.aspx?wid=<%=WorkflowId%>';" />
        </div>
    </div>
    <div class="dataList"  style="padding-top:5px">   
		<div class="showControl">
            <h4>表单基本属性</h4>
        </div>
        <table  cellpadding="4" cellspacing="1" class="tblGrayClass grayBackTable">
            <tr>
                <th style="width:13%; text-align:right;">表单名称：</th>
                <td>
                    <asp:HiddenField ID="hiddenWorkflowID" runat="server" />
                    <asp:TextBox ID="txtName" Width="320" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th style="width:13%; text-align:right;">描述：</th>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" Height="50" Width="90%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th style="width:13%; text-align:right;">是否当前版本：</th>
                <td>
                    <asp:CheckBox ID="chkboxIsCurrent" runat="server" Text="当前版本" />
                </td>
            </tr>
            <tr>
                <th style="width:13%; text-align:right;">上传表单定义：</th>
                <td>
                    <asp:FileUpload ID="fupload" runat="server" />
                    <asp:HyperLink ID="linkTemplate" runat="server" CssClass="ico_download">下载表单导入模板</asp:HyperLink>
                </td>
            </tr>
        </table>
        <div style="text-align:center; padding:10px;">
            <asp:Button ID="buttonOK" runat="server" Text="保存" CssClass="btn_sav" onclick="buttonOK_Click" />
            <input type="button" value="返回" class="btnReturnClass" style="margin-left:6px" onclick="window.location.href='list.aspx?wid=<%=WorkflowId%>';" />
        </div>
    </div>
</asp:Content>