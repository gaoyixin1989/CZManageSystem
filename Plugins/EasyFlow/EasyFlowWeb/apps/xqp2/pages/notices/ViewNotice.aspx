<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="xqp2_contrib_cms_pages_notice_ViewNotice" Codebehind="ViewNotice.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="NoticeView" Src="controls/NoticeView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>查看公告</span></h3>
    </div>
    <div class="dataList">
        <bw:NoticeView ID="noticeView1" runat="server" />
        <div style="margin-top:6px; text-align:center">
            <input type="button" value="返回" class="btnReturnClass" onclick="history.go(-1);" />
        </div>
    </div>
</asp:Content>
