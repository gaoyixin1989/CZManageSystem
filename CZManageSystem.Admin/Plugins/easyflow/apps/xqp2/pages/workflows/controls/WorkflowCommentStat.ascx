<%@ Control Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_controls_WorkflowCommentStat" Codebehind="WorkflowCommentStat.ascx.cs" %>
 <link href="<%=AppPath%>res/js/jquery.editable-select.css"
        rel="stylesheet" type="text/css" />
    <script src="<%=AppPath%>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.js"
        type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.pack.js"
        type="text/javascript"></script>
<div id="divCommentStat1">
    <div class="toolBlock" style="border-bottom: solid 1px #C0CEDF; margin-bottom: 10px;padding-bottom: 5px;">
        流程名称：
        <asp:DropDownList ID="ddlWorkflowList" runat="server" AutoPostBack="false" CssClass="editable-select">
        </asp:DropDownList>
        &nbsp; 日期：从<bw:DateTimePicker ID="txtStartDT" runat="server" Width="80px" ValidatorDisplay="Dynamic"  IsValidate="False" />
        到  <bw:DateTimePicker ID="txtEndDT" runat="server" Width="80px" ValidatorDisplay="Dynamic"  IsValidate="False" />
        <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn_query" OnClick="btnSearch_Click" />
    </div>
    <div id="dataDiv1" style="overflow-y: auto; overflow-x: auto; width: 100%; height:300px; min-height: 300px;">
        <table class="dataTable" id="dataTable1" runat="server" style="width:100%" border="0" cellpadding="0" cellspacing="0">
            <tr><td>
            <table cellpadding="4" cellspacing="1" class="tblGrayClass" id="tblId1" style="word-break: keep-all; text-align: center;">
                <tr>
                    <th width="40%" rowspan="2">标题</th>
                    <th width="15%" rowspan="2">受理号</th>
                    <th width="60%" colspan="4">审批信息</th>
                </tr>
                <tr style="text-align: center;">
                    <th style="width: 18%;">审批步骤</th>
                    <th style="width: 6%;">审批人</th>
                    <th style="width: 18%;">审批意见</th>
                    <th style="width: 18%;">审批时间</th>
                </tr>
                <asp:Repeater ID="rptOption" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="left">
                                <a href="../../../../../contrib/workflow/pages/workflowView.aspx?aiid=<%# Eval("ActivityInstanceId")%>"><%# Eval("Title")%></a>
                            </td>
                            <td align="left">
                                <a href="../../../../../contrib/workflow/pages/workflowView.aspx?aiid=<%# Eval("ActivityInstanceId")%>"><%# Eval("SheetId")%></a>
                            </td>
                            <td colspan="4">
                                <table width="100%" cellpadding="1" cellspacing="1" style="text-align:center">                                  
                                    <asp:Repeater runat="server" ID="rptDetails" DataSource='<%# ((System.Data.DataRowView)Container.DataItem).Row.GetChildRows("option") %>'>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="width: 30%;"><%# Eval("[\"ActivityName\"]")%></td>
                                               
                                                <td style="width: 10%;"> <%# Botwave.XQP.Commons.XQPHelper.FormatActors(Eval("[\"ActorName\"]").ToString())%></td>
                                                <td style="width: 30%;"><%# Eval("[\"Reason\"]")%></td>
                                                <td style="width: 30%;"><%# Eval("[\"FinishedTime\"]")%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="trClass">
                                                 <td style="width: 30%;"><%# Eval("[\"ActivityName\"]")%></td>
                                                <td style="width: 10%;"> <%# Botwave.XQP.Commons.XQPHelper.FormatActors(Eval("[\"ActorName\"]").ToString())%></td>
                                                <td style="width: 30%;"><%# Eval("[\"Reason\"]")%></td>
                                                <td style="width: 30%;"><%# Eval("[\"FinishedTime\"]")%></td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td align="left">
                                <a href="../../../../../contrib/workflow/pages/workflowView.aspx?aiid=<%# Eval("ActivityInstanceId")%>"><%# Eval("Title")%></a>
                            </td>
                            <td align="left">
                                <a href="../../../../../contrib/workflow/pages/workflowView.aspx?aiid=<%# Eval("ActivityInstanceId")%>"><%# Eval("SheetId")%></a>
                            </td>
                            <td colspan="4">
                                <table width="100%" cellpadding="1" cellspacing="1" style="text-align:center">                                       
                                    <asp:Repeater runat="server" ID="rptDetails" DataSource='<%# ((System.Data.DataRowView)Container.DataItem).Row.GetChildRows("option") %>'>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="width: 30%;"><%# Eval("[\"ActivityName\"]")%></td>
                                                <td style="width: 10%;"> <%# Botwave.XQP.Commons.XQPHelper.FormatActors(Eval("[\"ActorName\"]").ToString())%></td>
                                                <td style="width: 30%;"><%# Eval("[\"Reason\"]")%></td>
                                                <td style="width: 30%;"><%# Eval("[\"FinishedTime\"]")%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="trClass">
                                                 <td style="width: 30%;"><%# Eval("[\"ActivityName\"]")%></td>
                                                <td style="width: 10%;"> <%# Botwave.XQP.Commons.XQPHelper.FormatActors(Eval("[\"ActorName\"]").ToString())%></td>
                                                <td style="width: 30%;"> <%# Eval("[\"Reason\"]")%></td>
                                                <td style="width: 30%;"><%# Eval("[\"FinishedTime\"]")%></td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </td>
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