<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_Template" Codebehind="Template.aspx.cs" %>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script type="text/javascript" language="javascript" src="scripts/dynamic-form.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="divList" runat="server">
        <div class="titleContent">
            <h3><span><asp:Literal ID="ltlTitle" runat="server"></asp:Literal>模板编辑</span></h3>
        </div>
        <div id="dataDiv1">
            <div class="dataTable" id="dataTable1">
                <FCKeditorV2:FCKeditor ID="FCKContent" runat="server" Height="400px" BasePath="../../../res/fckeditor/">
                </FCKeditorV2:FCKeditor>
                <asp:Button ID="btnSaveTemplate" runat="server" Text="保存模板" CssClass="btnPassClass"
                    OnClick="btnSaveTemplate_Click" />&nbsp;&nbsp;<asp:Button ID="btnPreview" runat="server" Text="预览" CssClass="btnPassClass"
                    OnClick="btnPreview_Click" />&nbsp;&nbsp;<asp:Button ID="btnReturn" runat="server"
                        Text="返回继续" CssClass="btnPassClass" OnClick="btnReturn_Click" />
            </div>
        </div>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>表单预览</h4>
            <button onclick="return showContent(this,'divFormPreview');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="divFormPreview">
            <asp:Label ID="lblPreview" runat="server" Text=""></asp:Label>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {
                bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
            }
            /*$("#<%=btnPreview.ClientID %>").click(function () {
                var b = new Base64();
                var formContent = b.encode($("#<%=FCKContent.ClientID %>").val());
                $("#<%=FCKContent.ClientID %>").val(formContent);
            });
            $("#<%=btnSaveTemplate.ClientID %>").click(function () {
                var b = new Base64();
                var formContent = b.encode($("#<%=FCKContent.ClientID %>").val());
                $("#<%=FCKContent.ClientID %>").val(formContent);
            });
            $("#<%=btnReturn.ClientID %>").click(function () {
                var b = new Base64();
                var formContent = b.encode($("#<%=FCKContent.ClientID %>").val());
                $("#<%=FCKContent.ClientID %>").val(formContent);
            });*/
            $("form").submit(function () {
                var b = new Base64();
                var formContent = b.encode($("#<%=FCKContent.ClientID %>").val());
                $("#<%=FCKContent.ClientID %>").val(formContent);
            });
        });
    </script>
</asp:Content>
