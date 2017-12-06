<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_DoneTask" Codebehind="DoneTask.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <link href="<%=AppPath%>res/js/jquery.editable-select.css"
        rel="stylesheet" type="text/css" />
    <script src="<%=AppPath%>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.js"
        type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.pack.js"
        type="text/javascript"></script>
    <div class="titleContent">
        <h3><span>已办任务</span></h3>
    </div>
    <div class="btnControl" style="display:none;">
        <div class="btnLeft">
            <input type="button" class="btnFW" value="已阅事宜" onclick="window.location='doneReviews.aspx';" />&nbsp;
            <input type="button" class="btnFW" value="已办事宜" onclick="window.location='doneTaskByAppl.aspx';" />&nbsp;
            <input type="button" class="btnFW" value="我的工单" onclick="window.location='myTask.aspx';" />&nbsp;
            <input type="button" class="btnFW" value="转交任务记录" onclick="window.location='assignmentTask.aspx?ass=assign';" />&nbsp;
        </div>
    </div>
    <div class="toolBlock" style="border-bottom:solid 1px #C0CEDF; margin-bottom:10px; padding-bottom:5px;">
        流程：<asp:DropDownList ID="ddlWorkflows" runat="server" CssClass="editable-select"></asp:DropDownList>&nbsp;
        从<bw:DateTimePicker ID="txtStartDT" runat="server" Width="70px" InputBoxCssClass="inputbox" IsRequired="false" ExpressionValidatorText="*" ExpressionErrorMessage="处理时间的开始时间日期格式错误." />
        到<bw:DateTimePicker ID="txtEndDT" runat="server" Width="70px" InputBoxCssClass="inputbox" IsRequired="false" ExpressionValidatorText="*" ExpressionErrorMessage="处理时间的截止时间日期格式错误." />
        关键字：<asp:TextBox ID="textKeywords" runat="server" Width="60px" CssClass="inputbox"></asp:TextBox>&nbsp;
        发起人：<asp:TextBox ID="txtCreater" runat="server" Width="60px" CssClass="inputbox"></asp:TextBox>&nbsp;
        受理号：<asp:TextBox ID="txtSheetID" runat="server" Width="60px" CssClass="inputbox"></asp:TextBox>&nbsp;
        <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn_query" OnClick="btnSearch_Click" />
    </div>
    <div>
        <asp:ValidationSummary ID="searcSummary" runat="server" ShowSummary="false" ShowMessageBox="true" />
    </div>
    <div id="dataDiv1">
        <div class="dataTable" id="dataTable1">
            <table cellpadding="0" cellspacing="0" style="text-align:center;" class="tblClass" id="tblId1">
                <asp:Repeater ID="rptList" runat="server" onitemdatabound="rptList_ItemDataBound" OnItemCommand="rptList_ItemCommand"><%-- OnItemCommand="rptList_ItemCommand" OnItemDataBound="rptList_ItemDataBound"--%>
                    <HeaderTemplate>
                        <tr>
                            <th width="7%">类别</th>
                            <th >标题</th>
                            <th width="8%">受理号</th>
                            <th width="9%">发起人</th>
                            <th width="10%">当前步骤</th>
                            <th width="10%">当前处理人</th>
                            <th width="10%">处理步骤</th> 
                            <th width="13%"><asp:LinkButton ID="CreatedTime" Runat="server" Text="创建日期" CommandName="CreatedTime"></asp:LinkButton></th>
                            <th width="13%"><asp:LinkButton ID="FinishedTime" Runat="server" Text="处理时间" CommandName="FinishedTime"/></th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>
                            <td style="text-align:left;">
                                <a href='<%=AppPath%>contrib/workflow/pages/workflowView.aspx?aiid=<%# Eval("ActivityInstanceId")%>'>
                                    <%# Eval("Title")%>
                                </a>
                            </td>
                            <td><%# Eval("SheetId")%></td>
                            <td><%# Eval("CreatorName")%></td>
                            <td><%# Eval("CurrentActivityNames")%></td>
                            <td><asp:Literal ID="ltlCurrentActors" runat="server" Text='<%# Eval("CurrentActors")%>'></asp:Literal></td>
                            <td>
                                <asp:Literal ID="ltlActivityName" runat="server" Text='<%# Eval("ActivityName")%>'></asp:Literal>
                            </td>                            
                            <td style="text-align:left;"><%# Eval("CreatedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                            <td style="text-align:left;"><%# Eval("FinishedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="trClass">
                            <td><asp:Literal ID="ltlWorkflowAlias" runat="server" Text='<%# Eval("WorkflowAlias") %>'></asp:Literal></td>
                            <td style="text-align:left;">
                                <a href='<%=AppPath%>contrib/workflow/pages/workflowView.aspx?aiid=<%# Eval("ActivityInstanceId")%>'>
                                    <%# Eval("Title")%>
                                </a>
                            </td>
                            <td><%# Eval("SheetId")%></td>
                            <td><%# Eval("CreatorName")%></td>
                            <td><%# Eval("CurrentActivityNames")%></td>
                            <td><asp:Literal ID="ltlCurrentActors" runat="server" Text='<%# Eval("CurrentActors")%>'></asp:Literal></td>
                            <td>
                                <asp:Literal ID="ltlActivityName" runat="server" Text='<%# Eval("ActivityName")%>'></asp:Literal>
                            </td>                            
                            <td style="text-align:left;"><%# Eval("CreatedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                            <td style="text-align:left;"><%# Eval("FinishedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
            <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
                <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                    Font-Size="9pt" ItemsPerPage="10" PagerStyle="NumericPages" BorderWidth="0px"
                    OnPageIndexChanged="listPager_PageIndexChanged" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=ddlWorkflows.ClientID %>').editableSelect({
                onSelect: function (list_item) {
                    this.select.val(this.text.val());
                }
            })
            $(".editable-select-options").css("text-align","left");
            if ($("#<%=ddlWorkflows.ClientID %>").editableSelectInstances()[0])
                $("#<%=ddlWorkflows.ClientID %>").editableSelectInstances()[0].text[0].value = "<%=Request.QueryString["wf"]%>";
        });
    </script>
</asp:Content>
