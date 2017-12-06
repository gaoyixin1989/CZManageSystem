<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_controls_WorkflowDetailStat" Codebehind="WorkflowDetailStat.ascx.cs" %>
 <link href="<%=AppPath%>res/js/jquery.editable-select.css"
        rel="stylesheet" type="text/css" />
    <script src="<%=AppPath%>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.js"
        type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.pack.js"
        type="text/javascript"></script>
<div id="divDetailStat1">
     <div class="toolBlock" style="border-bottom: solid 1px #C0CEDF; margin-bottom: 10px; padding-bottom: 5px;">
        流程名称：
        <asp:DropDownList ID="ddlWorkflowList" runat="server" AutoPostBack="false">
        </asp:DropDownList>
        &nbsp; 日期：从<bw:DateTimePicker ID="txtStartDT" runat="server" Width="80px" ValidatorDisplay="Dynamic"  IsValidate="False" />到  
        <bw:DateTimePicker ID="txtEndDT" runat="server" Width="80px" ValidatorDisplay="Dynamic"  IsValidate="False" />
        &nbsp;统计方式：

        <asp:DropDownList ID="ddlStatType" runat="server" AutoPostBack="false">
            <asp:ListItem Value="1">按用户</asp:ListItem>
            <asp:ListItem Value="2">按部门</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn_query" OnClick="btnSearch_Click" /> 
    </div>
  
    <div id="dataDiv1" style="overflow-y: auto; overflow-x: auto; width: 100%;height:300px;min-height:300px;">
        <table class="dataTable"   id="dataTable1" runat="server" style="width:100%" border="0" cellpadding="0" cellspacing="0">
            <tr><td>
            <table cellpadding="4" cellspacing="1" class="tblGrayClass" style="word-break:keep-all;text-align:center;">
                <tr>
                    <th><%=ddlStatType.SelectedItem.Text.Substring(1) %>名称</th>
                    <th>月份</th>
                    <asp:Repeater ID="rptActivityList" runat="server">
                        <ItemTemplate>
                            <th><%# Eval("ActivityName") %></th>
                        </ItemTemplate>
                    </asp:Repeater>
                    <th>合计</th>
                </tr>
                <asp:Repeater ID="rptReportDetail" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Actor")%></td>
                            <td><%# Eval("StatTime")%></td>
                            <asp:Repeater runat="server" ID="rptReportDetails" DataSource='<%# ((System.Data.DataRowView)Container.DataItem).Row.GetChildRows("relation") %>'>
                                <ItemTemplate>
                                    <td><%# Eval("[\"ActivityTrackingCount\"]")%></td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td><%# Eval("TotalCount")%></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="trClass">
                            <td><%# Eval("Actor")%></td>
                            <td><%# Eval("StatTime")%></td>
                            <asp:Repeater runat="server" ID="rptReportDetails" DataSource='<%# ((System.Data.DataRowView)Container.DataItem).Row.GetChildRows("relation") %>'>
                                <ItemTemplate>
                                    <td><%# Eval("[\"ActivityTrackingCount\"]")%></td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td><%# Eval("TotalCount")%></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
             </td></tr>
        </table>
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
        //if ($("#<%=ddlWorkflowList.ClientID %>").editableSelectInstances()[0])
        //$("#<%=ddlWorkflowList.ClientID %>").editableSelectInstances()[0].text[0].value = "<%=Request.QueryString["wname"]%>";
    });
    </script>
