<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_report_pages_TemplateConfig" Codebehind="TemplateConfig.aspx.cs" %>

<%@ Register Src="../controls/TemplateConfig.ascx" TagName="TemplateConfig" TagPrefix="uc1" %>
<%@ Register Src="../controls/NavigationTools.ascx" TagName="NavigationTools" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="divList" runat="server">
        <div class="titleContent">
            <uc1:NavigationTools ID="NavigationTools1" runat="server" />
        </div>
        <div class="dataList">
            <uc1:TemplateConfig ID="TemplateConfig1" runat="server" />
        </div>
    </div>
</asp:Content>
