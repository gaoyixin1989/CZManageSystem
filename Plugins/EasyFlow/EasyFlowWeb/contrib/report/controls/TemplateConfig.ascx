<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="contrib_report_controls_TemplateConfig" Codebehind="TemplateConfig.ascx.cs" %>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>

<script type="text/javascript">
    function AddData2Template(v)
    { 
        var c = "<%=FCKContent.ClientID %>" + "___Frame";
        var f = document.frames[c].document.getElementById('xEditingArea').document.frames[0].document.body;
        addText(f," " + v.options[v.selectedIndex].text + " ");
    }
    function addText(oTextarea,strText)
    { 
        oTextarea.focus(); 
        document.selection.createRange().text += strText; 
        oTextarea.blur(); 
    } 
</script>

<table>
    <tr>
        <td>
            可用字段名：
        </td>
        <td>
            <asp:DropDownList ID="ddlData" runat="server" DataValueField="ColumnName" DataTextField="ColumnName">
            </asp:DropDownList>
        </td>
        <td>
            <asp:Button ID="btnCreate1" runat="server" Text="保存" OnClientClick="return confirm('你确认要保存吗？')"
                OnClick="btnCreate_Click" CssClass="btn_sav" />
            <input type="button" onclick="window.location.href('ReportList.aspx')" value="返回"
                class="btnReturnClass" />
        </td>
    </tr>
</table>
<FCKeditorV2:FCKeditor ID="FCKContent" runat="server" Height="400px" BasePath="~/res/FCKeditor/">
</FCKeditorV2:FCKeditor>
<br />
<asp:Button ID="btnCreate" runat="server" Text="保存" OnClientClick="return confirm('你确认要保存吗？')"
    OnClick="btnCreate_Click" CssClass="btn_sav" />
<input type="button" onclick="window.location.href('ReportList.aspx')" value="返回"
    class="btnReturnClass" />
