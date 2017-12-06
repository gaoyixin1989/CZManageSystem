<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="apps_czmcc_pages_NotebookResourcesList" Codebehind="NotebookResourcesList.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>笔记本资源列表</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="dataTable">
        <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align: center;">
            <tr>
                <th>
                </th>
                <th width="45%">
                    电脑型号
                </th>
                <th width="45%">
                    序列号

                </th>
            </tr>
            <asp:Repeater ID="rptResourcesList" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="radio" name="chkbox_resources" value='<%# Eval("ID")%>' />
                            <asp:HiddenField ID="hidID" runat="server" Value='<%# Eval("ID")%>' />
                        </td>
                        <td>
                            <asp:Label ID="lbModel" runat="server" Text='<%# Eval("ResourcesModel")%>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbNumber" runat="server" Text='<%# Eval("SerialNumber")%>'></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trClass">
                        <td>
                            <input type="radio" name="chkbox_resources" value='<%# Eval("ID")%>' />
                            <asp:HiddenField ID="hidID" runat="server" Value='<%# Eval("ID")%>' />
                        </td>
                        <td>
                            <asp:Label ID="lbModel" runat="server" Text='<%# Eval("ResourcesModel")%>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbNumber" runat="server" Text='<%# Eval("SerialNumber")%>'></asp:Label>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </table>
        <br />
        <div>
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn" Text="确  定" OnClick="btnSubmit_Click" />&nbsp;&nbsp;<input
                type="button" name="btnCancel" class="btn" value=" 取  消 " onclick="window.close();" />
        </div>
    </div>
    </form>
</body>
</html>
