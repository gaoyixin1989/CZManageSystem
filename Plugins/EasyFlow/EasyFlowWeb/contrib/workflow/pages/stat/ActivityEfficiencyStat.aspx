<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_stat_ActivityEfficiencyStat" Codebehind="ActivityEfficiencyStat.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">
   <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" runat="server" Text=""></asp:Literal>步骤运行效率分析</span></h3>
        <div class="rightSite">
        <input type="button" class="btn" value="返回" onclick="javascript:history.back();" />
    </div>
    </div>
    <div class="btnControl">
        <div class="btnRight">
        <input type="button" class="btn" value="返回" onclick="javascript:history.back();" class="btnFW"/>
        </div>
    </div>
    <table class="tblClass">
        <tr class="table_tr">
            <th width="40%">
                步骤名称
            </th>
            <th width="60%">
                平均运行时间</th>
        </tr>
        <asp:Repeater ID="rptStat" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("StatName")%>
                    </td>
                    <td>
                        <%# Eval("StatInstance")%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="trClass">
                    <td>
                        <%# Eval("StatName")%>
                    </td>
                    <td>
                        <%# Eval("StatInstance")%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
