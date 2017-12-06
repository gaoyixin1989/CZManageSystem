<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_stat_EfficiencyStat" Codebehind="EfficiencyStat.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">
      <div class="titleContent">
        <h3><span>运行效率分析</span></h3>
    </div>
    <table class="tblClass">
        <tr class="table_tr">
            <th width="40%">
                流程名称
            </th>
            <th width="60%">
                平均运行时间</th>
        </tr>
        <asp:Repeater ID="rptStat" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <a href="ActivityEfficiencyStat.aspx?w=<%# HttpUtility.UrlEncode(Eval("StatName").ToString()) %>" title="点击查看步骤效率">
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
                       <a href="ActivityEfficiencyStat.aspx?w=<%# HttpUtility.UrlEncode(Eval("StatName").ToString()) %>" title="点击查看步骤效率">
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
