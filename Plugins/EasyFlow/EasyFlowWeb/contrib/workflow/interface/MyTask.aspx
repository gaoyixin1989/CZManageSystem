<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_interface_MyTask" Title="我的工单" Codebehind="MyTask.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <script type="text/javascript" src="../pages/script/workflowextension.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="../pages/ws/WorkflowExtensionService.asmx" />
        </Services>
    </asp:ScriptManager>

    <div class="titleContent">
        <h3><span>我的工单</span></h3>
    </div>  
    
     <div class="btnControl" style="display:none">
        <div class="btnLeft">
            <input type="button" class="btnFW" value="已阅事宜" onclick="window.location='doneReviews.aspx';" />&nbsp;
            <input type="button" class="btnFW" value="已办事宜" onclick="window.location='doneTaskByAppl.aspx';" />&nbsp;
            <input type="button" class="btnFW" value="转交任务记录" onclick="window.location='assignmentTask.aspx?ass=assign';" />&nbsp;
            <input type="button" class="btnFW" value="已办任务" onclick="window.location='doneTask.aspx';" />&nbsp;
        </div>
    </div>  
    <div class="toolBlock" style="border-bottom:solid 1px #C0CEDF; margin-bottom:10px; padding-bottom:5px;display:none;">
        流程：<asp:DropDownList ID="ddlWorkflows" runat="server"></asp:DropDownList>
        从<bw:DateTimePicker ID="txtStartDT" runat="server" Width="70px" InputBoxCssClass="inputbox" IsRequired="false" ExpressionValidatorText="*" ExpressionErrorMessage="处理时间的开始时间日期格式错误." />
        到<bw:DateTimePicker ID="txtEndDT" runat="server" Width="70px" InputBoxCssClass="inputbox" IsRequired="false" ExpressionValidatorText="*" ExpressionErrorMessage="处理时间的截止时间日期格式错误." />
        关键字：<asp:TextBox ID="textKeywords" runat="server" Width="60px" CssClass="inputbox"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn_query" OnClick="btnSearch_Click" />
    </div>
    
    <div>
        <asp:ValidationSummary ID="searcSummary" runat="server" ShowSummary="false" ShowMessageBox="true" />
    </div>
    <div id="dataDiv1">
        <div class="dataTable" id="dataTable1">
            <table cellpadding="0" cellspacing="0" style="text-align:center;" class="tblClass" id="tblId1">
                <tr>
                    <th width="7%">类别</th>
                    <th width="26%">标题</th>
                    <th width="12%">受理号</th>
                    <th width="13%">当前步骤</th>
                    <th width="13%">当前处理人</th>
                    <th width="16%">发起时间</th>
                    <td style="display:none"></td>
                </tr>
                <asp:Repeater ID="rptList" runat="server" onitemdatabound="rptList_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>
                            <td style="text-align:left;">
                                <a href='WorkflowView.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'><%# Eval("Title") %></a>
                            </td>
                            <td><%# Eval("SheetId")%></td>
                            <td><%# Eval("WorkflowInstanceId")%></td>
                            <td><asp:Literal ID="ltlCurrentActors" runat="server" Text='<%# Eval("CurrentActors")%>'></asp:Literal></td>
                            <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                            <td style="display:none"><%# Eval("State")%></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="trClass">
                            <td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>
                            <td style="text-align:left;">
                                <a href='WorkflowView.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'><%# Eval("Title") %></a>
                            </td>
                            <td><%# Eval("SheetId")%></td>
                            <td><%# Eval("WorkflowInstanceId")%></td>
                            <td><asp:Literal ID="ltlCurrentActors" runat="server" Text='<%# Eval("CurrentActors")%>'></asp:Literal></td>
                            <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                            <td style="display:none"><%# Eval("State")%></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
            <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
                共有<strong>
                    <asp:Literal ID="litlTotalRecordCount" runat="server"></asp:Literal>
                </strong>条记录

                <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                    Font-Size="9pt" ItemsPerPage="10" PagerStyle="NumericPages" BorderWidth="0px"
                    OnPageIndexChanged="listPager_PageIndexChanged" />
            </div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        $(function() {
            var aiids = "";
            $('#tblId1 tr').each(function() {
                var wiid = $(this).children('td').eq(3).html();
                var state = $(this).children('td').eq(6).html();
                if (wiid != null) {
                    WorkflowExtension.GetCurrentActNames(wiid, state, $(this).children('td').eq(3));
                    WorkflowExtension.GetCurrentActors(wiid, state, $(this).children('td').eq(4));
                }
            });
        });
</script>
</asp:Content>

