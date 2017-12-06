<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="xqp2_contrib_workflow_pages_WorkflowHistoryDeploy" Codebehind="WorkflowHistoryDeploy.aspx.cs" %>

<%@ Register TagPrefix="bw" Assembly="Botwave.Security" Namespace="Botwave.Security.Web.Controls" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security.Extension" Namespace="Botwave.Security.Extension.Web.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        .main_menu_popup a
        {
            text-decoration: none;
        }
        .main_menu_popup a:hover
        {
            text-decoration: none;
            background-color: LightBlue;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="divList" runat="server">
        <div class="titleContent">
            <h3>
                <span>历史流程管理</span></h3>
            <div style="float: right; margin-right: 3px;">
                <%--<input id="btnCreateRole" runat="server" type="button" value="新增流程" onclick="window.location.href = 'designer/designer.aspx?m=workflow';" class="btnPassClass" />--%>
            </div>
        </div>
        <div class="dataList">
            <div class="showControl">
                <h4>
                    流程列表</h4>
                <button onclick="return showContent(this,'dataTable1');" title="收缩">
                    <span>折叠</span></button>
            </div>
            <div class="toolBlock" id="divSearch" style="margin-bottom: 10px; padding-bottom: 5px;
                text-align: right">
                <asp:TextBox ID="txtKeywords" ToolTip="请输入流程类别或流程名称" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" CssClass="btn_query" Text="搜索" OnClick="btnSearch_Click" />
            </div>
            <div class="dataTable" id="dataTable1">
                <table cellpadding="0" cellspacing="0" style="text-align: center;" class="tblClass"
                    id="tblId1">
                    <tr>
                        <th width="7%">
                            类型
                        </th>
                        <th width="23%">
                            流程名称
                        </th>
                        <th width="10%">
                            版本号
                        </th>
                        <th width="15%">
                            创建时间
                        </th>
                        <th width="50%">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater ID="rpFlowList" runat="server" OnItemCommand="rpFlowList_ItemCommand"
                        OnItemDataBound="rpFlowList_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                                </td>
                                <td style="text-align: left;">
                                    <a href="WorkflowIndex.aspx?wid=<%# Eval("WorkflowId") %>">
                                        <asp:Literal ID="ltlName" runat="server" Text='<%# Eval("WorkFlowName") %>'></asp:Literal>
                                    </a>
                                </td>
                                <td>
                                    <%# Eval("Version")%>
                                </td>
                                <td>
                                    <%# Eval("CreatedTime", "{0:yyyy-MM-dd HH:mm:ss}")%>
                                </td>
                                <td>
                                    <asp:LinkButton CommandName="Export" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="linkbtnExport" CssClass="ico_idt" runat="server">导出流程</asp:LinkButton>
                                    <asp:LinkButton CommandName="ExportItem" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="LinkButton1" CssClass="ico_idt" runat="server">导出表单</asp:LinkButton>
                                    <asp:LinkButton CommandName="ExportRules" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="LinkButton3" CssClass="ico_idt" runat="server" ToolTip="从流程中导出规则">导出规则</asp:LinkButton>
                                    <asp:LinkButton CommandName="SetEnabled" CssClass="ico_disable" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="btnSetEnable" runat="server">停用</asp:LinkButton>
                                    <a href="config/ConfigHistoryWorkflow.aspx?wid=<%# Eval("WorkflowId") %>" class="ico_edit">
                                        流程设置</a> <a href="../../../../contrib/dynamicform/pages/itemcreate.aspx?wid=<%# Eval("WorkflowId") %>&EntityType=Form_Workflow"
                                            class="ico_index">表单设计</a> <a href="javascript:void(0);" id="moreFunction_<%# Eval("WorkFlowID") %>"
                                                onclick="showMenu('divMoreFun_<%# Eval("WorkFlowID") %>', this);">更多▼</a>
                                    <div id="divMoreFun_<%# Eval("WorkFlowID") %>" style="display: none">
                                        <div class="main_menu_popup" style='border: 1px solid #9B9B9B; padding: 6px; background: #F9F9F9 url(/web/res/img/menu_popup_bg.gif) repeat-x bottom;'>
                                            <table width="124" style="margin: 0; padding: 0; line-height: 22px; cursor: pointer;
                                                font-size: 12px; border-collapse: collapse;">
                                                <tr>
                                                    <td onclick="window.parent.location='config/ConfigActivityRemark.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        background="<%=AppPath %>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">步骤处理意见设置</span>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td onclick="window.parent.location='config/RemindTime.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        background="<%=AppPath %>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">提醒时段设置</span>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td onclick="window.parent.location='<%=AppPath %>contrib/workflow/pages/workflowtemplate.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        background="<%=AppPath %>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">上传模板</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="trClass">
                                <td>
                                    <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                                </td>
                                <td style="text-align: left;">
                                    <a href="WorkflowIndex.aspx?wid=<%# Eval("WorkflowId") %>">
                                        <asp:Literal ID="ltlName" runat="server" Text='<%# Eval("WorkFlowName") %>'></asp:Literal>
                                    </a>
                                </td>
                                <td>
                                    <%# Eval("Version")%>
                                </td>
                                <td>
                                    <%# Eval("CreatedTime", "{0:yyyy-MM-dd HH:mm:ss}")%>
                                </td>
                                <td>
                                    <asp:LinkButton CommandName="Export" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="linkbtnExport" CssClass="ico_idt" runat="server">导出流程</asp:LinkButton>
                                    <asp:LinkButton CommandName="ExportItem" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="LinkButton1" CssClass="ico_idt" runat="server">导出表单</asp:LinkButton>
                                    <asp:LinkButton CommandName="ExportRules" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="LinkButton3" CssClass="ico_idt" runat="server" ToolTip="从流程中导出规则">导出规则</asp:LinkButton>
                                    <asp:LinkButton CommandName="SetEnabled" CssClass="ico_disable" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="btnSetEnable" runat="server">停用</asp:LinkButton>
                                    <a href="config/ConfigHistoryWorkflow.aspx?wid=<%# Eval("WorkflowId") %>" class="ico_edit">
                                        流程设置</a> <a href="../../../../contrib/dynamicform/pages/itemcreate.aspx?wid=<%# Eval("WorkflowId") %>&EntityType=Form_Workflow"
                                            class="ico_index">表单设计</a> <a href="javascript:void(0);" id="moreFunction_<%# Eval("WorkFlowID") %>"
                                                onclick="showMenu('divMoreFun_<%# Eval("WorkFlowID") %>', this);">更多▼</a>
                                    <div id="divMoreFun_<%# Eval("WorkFlowID") %>" style="display: none">
                                        <div class="main_menu_popup" style='border: 1px solid #9B9B9B; padding: 6px; background: #F9F9F9 url(/web/res/img/menu_popup_bg.gif) repeat-x bottom;'>
                                            <table width="124" style="margin: 0; padding: 0; line-height: 22px; cursor: pointer;
                                                height: 100px; font-size: 12px; border-collapse: collapse;">
                                                <tr>
                                                    <td onclick="window.parent.location='config/ConfigActivityRemark.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        background="<%=AppPath %>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">步骤处理意见设置</span>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td onclick="window.parent.location='config/RemindTime.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        background="<%=AppPath %>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">提醒时段设置</span>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td onclick="window.parent.location='<%=AppPath %>contrib/workflow/pages/workflowtemplate.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        background="<%=AppPath %>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">上传模板</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
                <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
                    <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                        Font-Size="9pt" ItemsPerPage="30" PagerStyle="NumericPages" BorderWidth="0px"
                        OnPageIndexChanged="listPager_PageIndexChanged" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //更多功能导航
        var oPopup = window.createPopup();
        function showMenu(theId, reference) {
            var menu = document.getElementById(theId);
            var t = reference.offsetTop + 23;
            var l = reference.offsetLeft;
            while (reference = reference.offsetParent) {
                t += reference.offsetTop;
                l += reference.offsetLeft;
            }
            oPopup.document.body.innerHTML = menu.innerHTML;
            oPopup.show(l, t, 140, 70, document.body);
        }
    </script>
</asp:Content>
