<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_ReviewList" Codebehind="ReviewList.ascx.cs" %>
<div class="showControl">
    <h4>待阅事宜</h4>
    <button onclick="return showContent(this,'dataReviewList');" title="收缩"><span>折叠</span></button>
</div>
<div class="toolBlock" id="divSearch" style="border-bottom:solid 1px #C0CEDF; margin-bottom:10px; padding-bottom:5px;" runat="server">
    <asp:Button ID="btn_Review" runat="server"
                         Text="批量阅办" CssClass="btnClass2l" OnClick="btnReview_Click" OnClientClick="return  beforeTransfer()" />
                         <input type="button" id="btn_Dealing" disabled="disabled" value="正在处理..." style="display:none" class="btnClass2l" />
</div>
<div class="dataTable todoClass" id="dataReviewList">
    <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align:center;">
        <tr>
            <th><input type="checkbox" title="全选" id="chkAll"/></th>
            <th width="8%">类别</th>
            <th width="33%">标题</th>
            <th width="10%">受理号</th>
            <th width="19%">当前步骤</th>
            <th width="10%">发起人</th>
            <th width="20%">创建时间</th>
        </tr>
        <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td><input type="checkbox" title="全选" name="chk" value="<%# Eval("ActivityInstanceId")%>"/></td>
                    <td>
                        <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                    </td>
                    <td style="text-align:left; font-weight:bold">
                        <asp:Literal ID="ltlActivityIcons" runat="server"></asp:Literal>
                        <a href='<%=AppPath%>contrib/workflow/pages/workflowview.aspx?type=review&aiid=<%# Eval("ActivityInstanceId")%>'>
                            <asp:Label ID="lbTitle" runat="server" Text='<%# Eval("Title")%>'></asp:Label></a>
                    </td>
                    <td>
                        <a href='<%=AppPath%>contrib/workflow/pages/workflowview.aspx?type=review&aiid=<%# Eval("ActivityInstanceId")%>'><%# Eval("SheetId")%></a>
                    </td>
                    <td>
                        <a href='<%=AppPath%>contrib/workflow/pages/workflowview.aspx?type=review&aiid=<%# Eval("ActivityInstanceId")%>'>
                            <asp:Literal ID="ltlActivityName" runat="server" Text='<%# Eval("ActivityName")%>'></asp:Literal>
                        </a>
                    </td>            
                    <td><span tooltip="<%# Eval("Creator")%>"><%# Eval("CreatorName")%></span></td>
                    <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="trClass">
                    <td><input type="checkbox" title="全选" name="chk" value="<%# Eval("ActivityInstanceId")%>"/></td>
                    <td>
                        <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                    </td>
                    <td style="text-align:left; font-weight:bold">
                        <asp:Literal ID="ltlActivityIcons" runat="server"></asp:Literal>
                        <a href='<%=AppPath%>contrib/workflow/pages/workflowview.aspx?type=review&aiid=<%# Eval("ActivityInstanceId")%>'>
                            <asp:Label ID="lbTitle" runat="server" Text='<%# Eval("Title")%>'></asp:Label></a>
                    </td>
                    <td>
                        <a href='<%=AppPath%>contrib/workflow/pages/workflowview.aspx?type=review&aiid=<%# Eval("ActivityInstanceId")%>'><%# Eval("SheetId")%></a>
                    </td>
                    <td>
                        <a href='<%=AppPath%>contrib/workflow/pages/workflowview.aspx?type=review&aiid=<%# Eval("ActivityInstanceId")%>'>
                            <asp:Literal ID="ltlActivityName" runat="server" Text='<%# Eval("ActivityName")%>'></asp:Literal>
                        </a>
                    </td>
                    <td><span tooltip="<%# Eval("Creator")%>"><%# Eval("CreatorName")%></span></td>
                    <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
    </table>
    <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
        <bw:VirtualPager ID="listPagerToReview" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
            Font-Size="9pt" ItemsPerPage="10" PagerStyle="NumericPages" BorderWidth="0px" OnPageIndexChanged="listPagerToReview_PageIndexChanged" />
    </div>
</div>
<script type="text/javascript">
    function beforeTransfer() {
        var sheetId = $("input[name='chk'][checked]");
        //alert(sheetId)
        if (sheetId.length == 0) {
            alert('请选择要处理的工单');
            return false;
        }
        if (confirm("确定处理？")) {
            $("#<%=btn_Review.ClientID %>").hide();
            $("#btn_Dealing").show();
            return true;
        }
        return false;
    }
    $(document).ready(function () {
        $("#chkAll").click(function () {
            if ($(this).attr("checked"))
                $("input[name='chk']").attr("checked", "checked")
            else {
                $("input[name='chk']").removeAttr("checked")
            }
        });
    });
   </script>
