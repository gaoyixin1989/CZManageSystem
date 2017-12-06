<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_help_ViewHelp" Codebehind="ViewHelp.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="titleContent">
        <h3>
            <span>查看帮助文档</span></h3>
        <div class="rightSite">
            <input type="button" value="编 辑" id="btnStart" onclick="javascript:location.href='edithelp.aspx?id=<%=HelpId %>'"
                class="btn_edit" />&nbsp;&nbsp;<asp:Button ID="btnDelete" runat="server" Text="删 除"
                    CssClass="btn_del" OnClick="btnDelete_Click" OnClientClick="javascript:return confirm('确定要删除吗?');" />
            &nbsp;&nbsp;<input type="button" value="返 回" onclick="window.location='default.aspx';"
                class="btnReturnClass" />
        </div>
    </div>
    <div class="btnControl">
        <div class="btnRight">
            <input type="button" value="编 辑" id="btnStart" onclick="javascript:location.href='edithelp.aspx?id=<%=HelpId %>'"
                class="btnFW" />&nbsp;&nbsp;<asp:Button ID="Button1" runat="server" Text="删 除"
                    CssClass="btnFW" OnClick="btnDelete_Click" OnClientClick="javascript:return confirm('确定要删除吗?');" />
            &nbsp;&nbsp;<input type="button" value="返 回" onclick="window.location='default.aspx';"
                class="btnFW" />
        </div>
    </div>
    <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 10px">
        <tr>
            <th width="18%">
                标题：    </th>
            <td>
                <asp:Label ID="lblTitle" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                内容：

            </th>
            <td>
                <asp:Literal ID="ltlContent" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>    
</asp:Content>
