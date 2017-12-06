<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_help_controls_helptree" Codebehind="helptree.ascx.cs" %>
<asp:TreeView ID="treehelp" runat="server" ShowLines="True">
</asp:TreeView>
<script language="javascript">
onTreeNodeClick = function(id){
    var toUrl = "viewhelp.aspx?id=" + id;
    location.href = toUrl;
}
</script>