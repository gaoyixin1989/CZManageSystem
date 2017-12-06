<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_czmcc_pages_WorkflowAttentions" Codebehind="WorkflowAttentions.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
<style type="text/css">
.expired td, .expired a,.expired a:link, .expired a:visited{color:red;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>我的关注</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>关注工单列表</h4>
        </div>
        <div style="border-bottom: solid 1px #C0CEDF;margin-bottom: 10px;padding-bottom: 5px;">
            <input type="checkbox" name="chkDataTypes" id="chkDataTypes0" value="0" <%=IsChecked(0)%> /><label for="chkDataTypes0">待办</label>
            <input type="checkbox" name="chkDataTypes" id="chkDataTypes1" value="1" <%=IsChecked(1)%> /><label for="chkDataTypes1">已办</label>
            &nbsp;&nbsp;&nbsp;
            关键字：<asp:TextBox ID="txtKeywords" runat="server"></asp:TextBox>
            <asp:Button ID="buttonQuery" runat="server" CssClass="btn_query" Text="搜索"  onclick="buttonQuery_Click" />
        </div>
        <div class="dataTable">
        <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align:center;">
            <tr>
                <th width="22px"></th>
                <th width="30px">类别</th>
                <th>标题</th>
                <th width="10%">受理号</th>
                <th width="10%">当前步骤</th>
                <th width="10%">当前处理人</th>
                <th width="10%">发起人</th>
                <th width="13%">创建时间</th>
                <th width="13%">期望完成时间</th>
            </tr>
        <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound">
            <ItemTemplate>
                <tr id="listRow" runat="server">
                    <td><input type="checkbox" name="chkboxRptList" value="<%# Eval("ID") %>" /></td>
                    <td><img alt='<%# Eval("WorkflowAlias")%>' class="groupImage" src='<%=AppPath%>contrib/workflow/res/groups/flow_<%# Eval("WorkflowAlias")%>.gif' /></td>
                    <td style="text-align:left;"><a href='<%=AppPath%>contrib/workflow/pages/workflowview.aspx?wiid=<%# Eval("WorkflowInstanceId")%>'><%# Eval("Title")%></a></td>
                    <td><%# Eval("SheetID")%></td>
                    <td><%# Eval("CurrentActivities")%></td>
                    <td><%# FormatWorkflowActor(Eval("CurrentActors"))%></td>
                    <td><%# Eval("CreatorName")%></td>
                    <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                    <td><%# Eval("ExpectFinishedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="trClass" id="listRow" runat="server">
                    <td><input type="checkbox" name="chkboxRptList" value="<%# Eval("ID") %>" /></td>
                    <td><img alt='<%# Eval("WorkflowAlias")%>' class="groupImage" src='<%=AppPath%>contrib/workflow/res/groups/flow_<%# Eval("WorkflowAlias")%>.gif' /></td>
                    <td style="text-align:left;"><a href='<%=AppPath%>contrib/workflow/pages/workflowview.aspx?wiid=<%# Eval("WorkflowInstanceId")%>'><%# Eval("Title")%></a></td>
                    <td><%# Eval("SheetID")%></td>
                    <td><%# Eval("CurrentActivities")%></td>
                    <td><%# FormatWorkflowActor(Eval("CurrentActors"))%></td>
                    <td><%# Eval("CreatorName")%></td>
                    <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                    <td><%# Eval("ExpectFinishedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
        </table>
        </div>
        <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
            <div style="float:left; width:29%; text-align:left">
            -  <input type="checkbox" id="selectAll" onclick="onToggleSelect('chkboxRptList', this.checked);" title="全选" />全选 
            -  <asp:LinkButton ID="btnRecycle" CssClass="ico_del" runat="server" Text="取消关注" ToolTip="取消选中的关注工单" onclick="btnRecycle_Click" OnClientClick="return confirm('确定要取消选中的关注？');"></asp:LinkButton>
            </div>
            <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                Font-Size="9pt" ItemsPerPage="15" PagerStyle="NumericPages" BorderWidth="0px" OnPageIndexChanged="listPager_PageIndexChanged" />
        </div>
    </div>
    <script type="text/javascript">
        // <!CDATA[
        function onToggleSelect(chkName, isChecked) {
            var inputArray = document.getElementsByTagName("input");
            for (var i = 0; i < inputArray.length; i++) {
                if (inputArray[i].type == "checkbox" && inputArray[i].name.indexOf(chkName) != -1) {
                    inputArray[i].checked = isChecked;
                }
            }
        };
    // ]]>
    </script>
</asp:Content>