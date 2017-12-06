<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_czmcc_pages_WorkflowInterfaceEdit" Codebehind="WorkflowInterfaceEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>新增外部流程接入</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>外部流程接入属性</h4>
        </div>
        <table  cellpadding="4" cellspacing="1" class="tblGrayClass grayBackTable">
            <tr>
                <th style="width:13%; text-align:right;">流程名称：</th>
                <td>
                    <asp:HiddenField ID="hiddenItemID" runat="server" />
                    <asp:TextBox ID="txtName" Width="320" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th style="width:13%; text-align:right;">流程发起地址：</th>
                <td>
                    <asp:TextBox ID="txtUrl" Width="320" runat="server" MaxLength="255"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th style="width:13%; text-align:right;">描述：</th>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" Height="50" Width="90%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th style="width:13%; text-align:right;">排序索引：</th>
                <td>
                    <asp:DropDownList ID="ddlSortOrders" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th style="width:13%; text-align:right;">是否启用：</th>
                <td>
                    <asp:CheckBox ID="chkboxStatus" runat="server" Checked="true" Text="启用流程" />
                </td>
            </tr>
        </table>
        <div style="text-align:center; padding:10px;">
            <asp:Button ID="buttonOK" runat="server" Text="保存" CssClass="btn_sav" onclick="buttonOK_Click" />
            <input type="button" value="返回" class="btnReturnClass" style="margin-left:6px" onclick="window.location='workflowinterface.aspx';" />
        </div>
    </div>
</asp:Content>

