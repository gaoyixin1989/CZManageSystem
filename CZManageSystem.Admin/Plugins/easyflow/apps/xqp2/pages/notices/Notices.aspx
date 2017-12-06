<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="xqp2_contrib_cms_pages_notice_Notices" Codebehind="Notices.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="NoticeList" Src="controls/NoticeList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>公告管理</span></h3>
        <div style="float:right; margin-right:3px;">
            <input type="button" value="新增公告" onclick="window.location.href = 'editNotice.aspx';" class="btnPassClass" />
        </div>
    </div>    
    <bw:NoticeList ID="noticeList1" runat="server" />    
</asp:Content>
