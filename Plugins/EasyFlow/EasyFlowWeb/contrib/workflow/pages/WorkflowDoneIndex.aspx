<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_pages_WorkflowDoneIndex" Codebehind="WorkflowDoneIndex.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>流程已完成任务列表</title>
    <script type="text/javascript" src="../../../res/js/jquery-latest.pack.js"></script>
</head>
<body style="background: none !important;">
    <div class="dataList" style="margin:0px; padding:0px">
        <script type="text/javascript" src="script/tooltipAjax.js"></script>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="scriptManager1" runat="server">
                <Services>
                    <asp:ServiceReference Path="WorkflowAjaxService.asmx" />
                </Services>
            </asp:ScriptManager>
            <table cellpadding="0" cellspacing="0" style="text-align:center;" class="tblClass">
                 <tr>
                    <th width="7%">类别</th>
                    <th width="26%">标题</th>
                    <th width="12%">发起人</th>
                    <th width="13%">当前步骤</th>
                    <th width="13%">当前处理人</th>
                    <th width="13%">处理步骤</th>                    
                    <th width="16%">处理时间</th>
                </tr>
                <asp:Repeater ID="listDoneTask" runat="server" OnItemDataBound="listDoneTask_ItemDataBound">
                   <ItemTemplate>
                        <tr>
                            <td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>
                            <td style="text-align:left;">
                                <a href="WorkflowView.aspx?aiid=<%# Eval("ActivityInstanceId")%>" target="_parent">
                                    <%# Eval("Title")%>
                                </a>
                            </td>
                            <td><%# Eval("CreatorName")%></td>
                            <td><%# Eval("CurrentActivityNames")%></td>
                            <td><asp:Literal ID="ltlCurrentActors" runat="server" Text='<%# Eval("CurrentActors")%>'></asp:Literal></td>
                            <td>
                                <asp:Literal ID="ltlActivityName" runat="server" Text='<%# Eval("ActivityName")%>'></asp:Literal>
                            </td>                            
                            <td style="text-align:left;"><%# Eval("FinishedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="trClass">
                            <td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>
                            <td style="text-align:left;">
                                <a href="WorkflowView.aspx?aiid=<%# Eval("ActivityInstanceId")%>" target="_parent">
                                    <%# Eval("Title")%>
                                </a>
                            </td>
                            <td><%# Eval("CreatorName")%></td>
                            <td><%# Eval("CurrentActivityNames")%></td>
                            <td><asp:Literal ID="ltlCurrentActors" runat="server" Text='<%# Eval("CurrentActors")%>'></asp:Literal></td>
                            <td>
                                <asp:Literal ID="ltlActivityName" runat="server" Text='<%# Eval("ActivityName")%>'></asp:Literal>
                            </td>                            
                            <td style="text-align:left;"><%# Eval("FinishedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
    
            <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
                <bw:VirtualPager ID="listPagerDoneTask" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                    Font-Size="9pt" ItemsPerPage="10" PagerStyle="NumericPages" BorderWidth="0px"
                    OnPageIndexChanged="listPagerDoneTask_PageIndexChanged" />
            </div>
        </form>
    </div>
</body>
</html>
