<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_theme_pages_Default" Title="Untitled Page" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>界面主题</span></h3>
    </div>    
    <div class="pageClass" />
    <asp:HiddenField ID="hiddenTheme" runat="server" />
    <div style="margin-top:20px; text-align:center">
        <asp:DataList ID="listThemes" runat="server" RepeatLayout="Table" CellPadding="10" CellSpacing="5">
            <ItemTemplate>
                <img src="<%# Eval("Preview") %>" alt="<%# Eval("Title") %>" style="width:200px;" />
                <div>
                    <input type="radio" id="theme_<%# Eval("Index") %>" name="radioThemes" value="<%# Eval("Name") %>" />
                    <label for="theme_<%# Eval("Index") %>"><%# Eval("Title") %></label>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
    <div style="text-align:center;" class="pageClass">
        <asp:Button ID="btnSetTheme" runat="server" CssClass="btn_sav" Text="保存" onclick="btnSetTheme_Click" />
    </div>
    <script type="text/javascript">
    $(function(){
        var themeName = "<%=this.ThemeName%>";
        $("input[name='radioThemes']").each(function(){
            if(this.value == themeName){
                this.checked = true;
            }
        });
        $("input[name='radioThemes']").click(function(){
            if(this.checked){
                $("#<%=hiddenTheme.ClientID%>").val(this.value);
            }
        });
    });
    </script>
</asp:Content>

