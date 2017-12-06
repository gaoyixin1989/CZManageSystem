<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_controls_ActivityStat" Codebehind="ActivityStat.ascx.cs" %>
<div id="divActivityStat1">
    <div class="toolBlock" style="border-bottom: solid 1px #C0CEDF; margin-bottom: 10px;
        padding-bottom: 5px;">
        流程名称：<asp:DropDownList ID="ddlWorkflowList" runat="server" 
            AutoPostBack="true" DataTextField="WorkflowName" 
            onselectedindexchanged="ddlWorkflowList_SelectedIndexChanged" CssClass="editable-select">
        </asp:DropDownList>
        &nbsp;&nbsp; 日期：从
        <bw:DateTimePicker ID="txtStartDT" runat="server" ValidatorDisplay="Dynamic" Width="80px" IsValidate="False"/>
        到

        <bw:DateTimePicker ID="txtEndDT" runat="server" Width="80px" ValidatorDisplay="Dynamic"  IsValidate="False" />
        <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn_query" OnClick="btnSearch_Click" />
    </div>
    <table cellpadding="4" cellspacing="1" class="tblGrayClass" id="tblId1" style="text-align: center;">
        <tr>
            <th>步骤名称</th>
            <th>工单数</th>
        </tr>
        <asp:Repeater ID="rptStateStat" runat="server">
            <ItemTemplate>
                <tr style="text-align: center;">
                    <td>
                       <a href="WorkflowList.aspx?wname=<%= HttpUtility.UrlEncode(WorkflowName) %>&aname=<%# HttpUtility.UrlEncode(Eval("StatName").ToString()) %>">
                            <%# Eval("StatName")%></a>
                    </td>
                    <td>
                        <%# Eval("StatInstance")%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="trClass" style="text-align: center;">
                    <td>
                       <a href="WorkflowList.aspx?wname=<%= HttpUtility.UrlEncode(WorkflowName) %>&aname=<%# HttpUtility.UrlEncode(Eval("StatName").ToString()) %>">
                            <%# Eval("StatName")%></a>
                    </td>
                    <td>
                        <%# Eval("StatInstance")%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
    </table>
</div>
<script type="text/javascript">
        $(document).ready(function () {
            $('#<%=ddlWorkflowList.ClientID %>').editableSelect({
                onSelect: function (list_item) {
                    this.select.val(this.text.val());
                    //if (this.text.attr("id") == "datasource")
                       // getField(document.getElementById("datasource"));

                       setTimeout('__doPostBack(\''+"<%=ddlWorkflowList.ClientID %>".replace("_","$")+'\',\'\')', 0)
                }
            })
            $(".editable-select-options").css("text-align","left");
            //if ($("#<%=ddlWorkflowList.ClientID %>").editableSelectInstances()[0])
                //$("#<%=ddlWorkflowList.ClientID %>").editableSelectInstances()[0].text[0].value = "<%=Request.QueryString["wname"]%>";
        });
    </script>