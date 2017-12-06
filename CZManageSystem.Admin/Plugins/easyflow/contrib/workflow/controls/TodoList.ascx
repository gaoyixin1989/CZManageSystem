<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_TodoList" Codebehind="TodoList.ascx.cs" %>
<link href="<%=AppPath%>res/js/jquery.editable-select.css"
        rel="stylesheet" type="text/css" />
    <script src="<%=AppPath%>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.js"
        type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.pack.js"
        type="text/javascript"></script>
<div class="toolBlock" id="divSearch" style="border-bottom:solid 1px #C0CEDF; margin-bottom:10px; padding-bottom:5px;" runat="server">
        流程：<asp:DropDownList ID="ddlWorkflows" runat="server" CssClass="editable-select"></asp:DropDownList>
        关键字：<asp:TextBox ID="txtKeywords" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" CssClass="btn_query" Text="搜索" 
            onclick="btnSearch_Click" />
       <asp:Button ID="btnExport" runat="server" ToolTip="导出工单内容" Text="导出" 
            CssClass="btnFWClass" onclick="btnExport_Click"/>
    </div>
<div class="dataTable" id="<%=this.ID %>_dataTable1">
    <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align:center;">
        <tr>
            <th width="8%">类别</th>
            <th width="33%">标题</th>
            <th width="10%">受理号</th>
            <th width="19%">当前步骤</th>
            <th width="10%">发起人</th>
            <th width="20%">创建时间</th>
        </tr>
        <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound">
            <ItemTemplate>
                <tr id="listRow" runat="server" style="font-weight:bold">
                    <td>
                        <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                    </td>
                    <td style="text-align:left">
                        <asp:Literal ID="ltlActivityIcons" runat="server"></asp:Literal>
                        <a href='<%=AppPath%>contrib/workflow/pages/process.aspx?aiid=<%# Eval("ActivityInstanceId")%>'>
                            <asp:Label ID="lbTitle" runat="server" Text='<%# Eval("Title")%>'></asp:Label></a>
                    </td>
                    <td>
                        <a href='<%=AppPath%>contrib/workflow/pages/process.aspx?aiid=<%# Eval("ActivityInstanceId")%>'><%# Eval("SheetId")%></a>
                    </td>
                    <td>
                        <a href='<%=AppPath%>contrib/workflow/pages/process.aspx?aiid=<%# Eval("ActivityInstanceId")%>'>
                            <asp:Literal ID="ltlActivityName" runat="server" Text='<%# Eval("ActivityName")%>'></asp:Literal>
                        </a>
                    </td>             
                    <td><span tooltip="<%# Eval("Creator")%>"><%# Eval("CreatorName")%></span></td>
                    <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="trClass" id="listRow" runat="server" style="font-weight:bold">
                    <td>
                        <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                    </td>
                    <td style="text-align:left;">
                        <asp:Literal ID="ltlActivityIcons" runat="server"></asp:Literal>
                        <a href='<%=AppPath%>contrib/workflow/pages/process.aspx?aiid=<%# Eval("ActivityInstanceId")%>'>
                            <asp:Label ID="lbTitle" runat="server" Text='<%# Eval("Title")%>'></asp:Label></a>
                    </td>
                    <td>
                        <a href='<%=AppPath%>contrib/workflow/pages/process.aspx?aiid=<%# Eval("ActivityInstanceId")%>'><%# Eval("SheetId")%></a>
                    </td>
                    <td>
                        <a href='<%=AppPath%>contrib/workflow/pages/process.aspx?aiid=<%# Eval("ActivityInstanceId")%>'>
                            <asp:Literal ID="ltlActivityName" runat="server" Text='<%# Eval("ActivityName")%>'></asp:Literal>
                        </a>
                    </td>
                    <td><span tooltip="<%# Eval("Creator")%>"><%# Eval("CreatorName")%></span></td>
                    <td><%# Eval("CreatedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
    </table>
    <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
        <bw:VirtualPager ID="listPagerTodoTask" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
            Font-Size="9pt" ItemsPerPage="10" PagerStyle="NumericPages" BorderWidth="0px" OnPageIndexChanged="listTodoPager_PageIndexChanged" />
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
                $("#<%=ddlWorkflows.ClientID %>").editableSelectInstances()[0].text[0].value = "<%=Request.QueryString["wfname"]%>";
        });
    </script>
</div>
