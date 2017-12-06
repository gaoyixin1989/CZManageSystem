<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_help_Default" CodeBehind="Default.aspx.cs" %>

<%@ Register TagPrefix="bw" TagName="HelpTree" Src="controls/helptree.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="titleContent">
        <h3>
            <span>帮助文档管理</span></h3>
        <div class="rightSite">
            <input type="button" value="新 增" id="btnStart" onclick="javascript: location.href = 'edithelp.aspx'" class="btn_add" />
        </div>
    </div>
    <div class="btnControl">
        <input type="button" value="新 增" id="btnStart" onclick="javascript: location.href = 'edithelp.aspx'" class="btnFW" />
    </div>
    <div class="dataList">
        <bw:HelpTree ID="helptree" runat="server" />
    </div>
</asp:Content>

