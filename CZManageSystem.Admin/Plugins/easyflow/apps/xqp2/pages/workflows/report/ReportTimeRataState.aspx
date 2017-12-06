<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_report_ReportTimeRataState" Codebehind="ReportTimeRataState.aspx.cs" %>

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
    您当前位置：<a href="<%=AppPath%>contrib/workflow/pages/default.aspx" >首页</a> &gt; <span class="cRed">智能提醒统计</span>
</div>
<div class="btnControl">
    <div class="btnLeft">
        <input type="button" class="btnFW" value="返回" onclick="window.location='reportindex.aspx';" />
    </div>
</div>
<div class="dataList">
    <div id="divActivityStat1">
        <div class="toolBlock" style="border-bottom: solid 1px #C0CEDF; margin-bottom: 10px;padding-bottom: 5px;">
            流程：<asp:DropDownList ID="ddlWorkflowList" CssClass="editable-select" runat="server"></asp:DropDownList>
            受理号：<asp:TextBox ID="txtSheetId" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp; 步骤起始时间：<bw:DateTimePicker ID="txtStart" runat="server" Width="80px" ValidatorDisplay="Dynamic"  IsValidate="False"/>
        &nbsp; 到&nbsp;&nbsp; <bw:DateTimePicker ID="txtEnd" runat="server" Width="80px" ValidatorDisplay="Dynamic"  IsValidate="False" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" Text=" 统 计 " CssClass="btn_query" onclick="Button1_Click" />
        <asp:Button ID="btnExport" runat="server" Text="导出Excel" CssClass="btn" ToolTip="点击以导出所有结果" onclick="btnExport_Click" />
        </div>
        <div id="dataDiv1" style="width: 100%;">
        <asp:Repeater ID="Repeater1" runat="server">
        <HeaderTemplate>
        <table cellpadding="4" cellspacing="1" class="tblGrayClass" id="tblId1" style="text-align: center;">
        <tr>
            <th style="text-align:center;">流程名称</th>
            <th style="text-align:center;">工单标题</th>
            <th style="text-align:center;">受理号</th>
            <th style="text-align:center;">工单步骤</th>
            <th style="text-align:center;">开始时间</th>
            <th style="text-align:center;">结束时间</th>
            <th style="text-align:center;">处理时长(小时)</th>
            <th style="text-align:center;">步骤限定时长(小时)</th>
            <%--<th style="text-align:center;">是否超时</th>--%>
        </tr>
        </HeaderTemplate>
        <ItemTemplate>
        <tr>
            <td><%# Eval("workflowname")%></td>
            <td><a href='../../../../../contrib/workflow/pages/WorkflowView.aspx?wiid=<%# Eval("workflowinstanceid")%>' target="_blank"><%# Eval("title") %></a></td>
            <td><a href='../../../../../contrib/workflow/pages/WorkflowView.aspx?wiid=<%# Eval("workflowinstanceid")%>' target="_blank"><%# Eval("sheetid")%></a></td>
            <td><%# Eval("activityname")%></td>
            <td><%# Eval("createdtime")%></td>
            <td><%# Eval("finishedtime")%></td>
            <td><%# Eval("hours")%></td>
            <td><%# Eval("Stayhours")%></td>
            <%--<td><%# Eval("istimeout")%></td>--%>
        </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
        <tr>
            <td><%# Eval("workflowname")%></td>
            <td><a href='../../../../../contrib/workflow/pages/WorkflowView.aspx?wiid=<%# Eval("workflowinstanceid")%>' target="_blank"><%# Eval("title") %></a></td>
            <td><a href='../../../../../contrib/workflow/pages/WorkflowView.aspx?wiid=<%# Eval("workflowinstanceid")%>' target="_blank"><%# Eval("sheetid")%></a></td>
            <td><%# Eval("activityname")%></td>
            <td><%# Eval("createdtime")%></td>
            <td><%# Eval("finishedtime")%></td>
            <td><%# Eval("hours")%></td>
            <td><%# Eval("Stayhours")%></td>
            <%--<td><%# Eval("istimeout")%></td>--%>
        </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
        </table>
        </FooterTemplate>
        </asp:Repeater>
        <div class="toolBlock" style="border-top:solid 1px #C0CEDF;">
            <bw:VirtualPager ID="listPagerTodoTask" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                Font-Size="9pt" ItemsPerPage="10" PagerStyle="NumericPages" BorderWidth="0px" OnPageIndexChanged="listTodoPager_PageIndexChanged" />
        </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#<%=ddlWorkflowList.ClientID %>').editableSelect({
            onSelect: function (list_item) {
                this.select.val(this.text.val());
            }
        })
        $(".editable-select-options").css("text-align", "left");
        if ($("#<%=ddlWorkflowList.ClientID %>").editableSelectInstances()[0])
                $("#<%=ddlWorkflowList.ClientID %>").editableSelectInstances()[0].text[0].value = "<%=Request.QueryString["wf"]%>";
    });
</script>
</asp:Content>

