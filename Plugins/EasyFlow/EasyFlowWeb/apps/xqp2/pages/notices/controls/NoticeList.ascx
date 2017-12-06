<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_notices_controls_NoticeList" Codebehind="NoticeList.ascx.cs" %>
<div class="dataList">
    <div class="showControl">
        <h4>管理公告</h4>
    </div>
    <div>
        <a href="notices.aspx">所有公告</a> |
        <a href="notices.aspx?enabled=true">已启用</a> |
        <a href="notices.aspx?enabled=false">未启用</a>
    </div>    
    <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align:center;margin-top:6px">
        <tr>
            <th>标题</th>
            <th style="width:20%">日期</th>
            <th style="width:10%">作者</th>
            <th style="width:10%">状态</th>
            <th style="width:15%">操作</th>
        </tr>
        <asp:Repeater ID="rptNotices" runat="server">
            <ItemTemplate>
                <tr>
                    <td style="text-align:left">
                       <a href="viewNotice.aspx?&noticeId=<%# Eval("NoticeId") %>"><%# Eval("Title") %></a>
                    </td>
                    <td><%# Eval("LastModTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                    <td><%# Eval("CreatorName") %></td>
                    <td><%# FormatEnabled(Eval("Enabled"))%></td>
                    <td>
                        <a class="ico_edit" href="EditNotice.aspx?action=edit&noticeId=<%# Eval("NoticeId") %>">编辑</a>
                        <a class="ico_del" href="EditNotice.aspx?action=delete&noticeId=<%# Eval("NoticeId") %>" onclick="return onDeleteNotice();">删除</a>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="trClass">
                    <td style="text-align:left">
                       <a href="viewNotice.aspx?&noticeId=<%# Eval("NoticeId") %>"><%# Eval("Title") %></a>
                    </td>
                    <td><%# Eval("LastModTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                    <td><%# Eval("CreatorName")%></td>
                    <td><%# FormatEnabled(Eval("Enabled"))%></td>
                    <td>
                        <a class="ico_edit" href="EditNotice.aspx?action=edit&noticeId=<%# Eval("NoticeId") %>">编辑</a>
                        <a class="ico_del" href="EditNotice.aspx?action=delete&noticeId=<%# Eval("NoticeId") %>" onclick="return onDeleteNotice();">删除</a>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
    </table>    
    <div class="toolBlock" style="border-top:solid 1px #C0CEDF; padding-top:5px">
        <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                Font-Size="9pt" ItemsPerPage="20" PagerStyle="NumericPages" BorderWidth="0px"
                OnPageIndexChanged="listPager_PageIndexChanged" />
    </div>
</div>
<script type="text/javascript">
function onDeleteNotice(){
    return confirm("确定要删除该公告吗？");
}
</script>
