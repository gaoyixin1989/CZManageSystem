<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Popup.master" AutoEventWireup="true" Inherits="apps_czmcc_pages_WorkflowRelationPopup" Codebehind="WorkflowRelationPopup.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="dataList" style="padding:3px;">
        <div class="showControl">
            <h4>历史工单关联选择列表</h4>
        </div>
        <div style="text-align:right;">
            <asp:TextBox ID="txtKeywords" runat="server"></asp:TextBox>
            <asp:Button ID="buttonOK" runat="server" Text="搜索" CssClass="btn_query" onclick="buttonOK_Click" />
        </div>
        <div style="height:350px; overflow:auto;">
            <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align: center;">
                <tr>
                    <th style="width:30px; text-align:center;">选择</th>
                    <th style="width:80px; text-align:center;">受理号</th>
                    <th>工单标题</th>
                    <th style="width:120px; text-align:center;">发起人</th>
                    <th style="width:120px; text-align:center;">发起时间</th>
                    <th style="width:100px; text-align:center;">属性</th>
                </tr>
                <asp:Repeater ID="rptList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="radio" name="boxWorkflowInstances" value="<%# Eval("WorkflowInstanceId") %>" />
                            </td>
                            <td><%# Eval("SheetId") %></td>
                            <td style="text-align:left;"><%# Eval("Title") %></td>
                            <td><%# Eval("CreatorName") %></td>
                            <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                            <td>
                                <input type="checkbox" name="boxRefAttachments" id="boxRefAttachments_<%#RowIndex%>" disabled="disabled" value="<%# Eval("WorkflowInstanceId") %>" />
                                <label for="boxRefAttachments_<%#RowIndex++%>">引用附件</label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="trClass">
                            <td>
                                <input type="radio" name="boxWorkflowInstances" value="<%# Eval("WorkflowInstanceId") %>" />
                            </td>
                            <td><%# Eval("SheetId") %></td>
                            <td style="text-align:left;"><%# Eval("Title") %></td>
                            <td><%# Eval("CreatorName") %></td>
                            <td><%# Eval("StartedTime", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                            <td>
                                <input type="checkbox" name="boxRefAttachments" id="boxRefAttachments_<%#RowIndex%>" disabled="disabled" value="<%# Eval("WorkflowInstanceId") %>" />
                                <label for="boxRefAttachments_<%#RowIndex++%>">引用附件</label>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div class="toolBlock" style="border-top:solid 1px #C0CEDF; border-bottom:solid 1px #C0CEDF; margin-top:3px;">
            <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                Font-Size="9pt" ItemsPerPage="10" PagerStyle="NumericPages" BorderWidth="0px"
                OnPageIndexChanged="listPager_PageIndexChanged" />
        </div>
        
        <div style="text-align:center; margin-top:10px;">
            <input type="button" class="btn_submit" value="确定" onclick="onOKClick();" />
            <input type="button" class="btn_del" value="取消" onclick="window.close();" />
        </div>
    </div>
    
    <script type="text/javascript">
        function onOKClick() {
            //var rel = '<%=Request["rel"]%>';
            var rel = '<%=Rel%>';
            var target = $("input[name='boxWorkflowInstances']:checked").val();
            if (!target || target == "") {
                alert("请选择关联的历史工单。");
                return false;
            }
            var isChecked = $("input[name='boxRefAttachments'][value='" + target + "']").attr("checked");
            isChecked = (!isChecked || isChecked == false) ? false : true;

            $.post("workflowRelationAjax.aspx", { action: "save", rel: rel, target: target, attachment: isChecked },
                function(result) {
                    if (result && result.indexOf("成功", 0) > 1) {
                        window.returnValue = true;
                        window.close();
                    } else {
                        alert((!result) ? "保存失败，请重试。" : result);
                    }
                });
        };

        $(function() {
            $("input[name='boxWorkflowInstances']").click(function() {
                $("input[name='boxRefAttachments']").each(function() {
                    $(this).attr("disabled", "disabled");
                    $(this).removeAttr("checked");
                });
                $("input[name='boxRefAttachments'][value='" + $(this).val() + "']").removeAttr("disabled");
            });
        });
    </script>
</asp:Content>
