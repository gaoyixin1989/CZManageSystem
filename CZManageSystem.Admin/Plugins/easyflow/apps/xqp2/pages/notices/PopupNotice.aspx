<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="xqp2_contrib_cms_pages_notice_PopupNotice" Title="公告" Codebehind="PopupNotice.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="NoticeView" Src="controls/NoticeView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">    
    <div class="dataList" style="top:50px;left:50px;right:50px;">
        <bw:NoticeView ID="noticeView1" runat="server" />
    </div>
    <script type="text/javascript" language="javascript"> 
		function autoClose()
		{
			i=i-1;
			document.title="本公告将在"+i+"秒后自动关闭"; 
			if(i>0)
				setTimeout("autoClose();",1000); 
			else 
				self.close();
		} 
		var i=15;
		autoClose(); 
    </script>
</asp:Content>
