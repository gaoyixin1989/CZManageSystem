<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_stat_WorkflowsOvertimeStat" Codebehind="WorkflowsOvertimeStat.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<div class="titleContent">
        <h3><span>流程超时统计</span></h3>
    </div>
    <table class="tblClass">
        <tr class="table_tr">
            <th width="40%">
                流程名称
            </th>
            <th width="60%">
                超时工单数</th>
        </tr>
        <asp:Repeater ID="rptStat" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <a href="WorkflowsOvertimeList.aspx?w=<%# HttpUtility.UrlEncode(Eval("StatName").ToString()) %>" title="点击查看超时工单列表">
                            <%# Eval("StatName")%>
                        </a>
                    </td>
                    <td>
                        <%# Eval("StatInstance")%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="trClass">
                    <td>
                       <a href="WorkflowsOvertimeList.aspx?w=<%# HttpUtility.UrlEncode(Eval("StatName").ToString()) %>" title="点击查看超时工单列表">
                            <%# Eval("StatName")%>
                        </a>
                    </td>
                    <td>
                        <%# Eval("StatInstance")%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>

