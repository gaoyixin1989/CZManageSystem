<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="xqp2_contrib_cms_pages_notice_EditNotice" Codebehind="EditNotice.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="NoticeEditor" Src="controls/NoticeEditor.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>编辑公告</span></h3>
    </div>
    <div class="dataList" style="padding:0">
        <bw:NoticeEditor ID="noticeEditor1" runat="server" />
    </div>
</asp:Content>
