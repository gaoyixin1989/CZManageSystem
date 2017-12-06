<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="xqp2_contrib_workflow_pages_WorkflowDeploy" CodeBehind="WorkflowDeploy.aspx.cs" %>

<%@ Register TagPrefix="bw" Assembly="Botwave.Security" Namespace="Botwave.Security.Web.Controls" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security.Extension" Namespace="Botwave.Security.Extension.Web.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        .main_menu_popup a {
            text-decoration: none;
        }

            .main_menu_popup a:hover {
                text-decoration: none;
                background-color: LightBlue;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="divList" runat="server">
        <div class="titleContent">
            <h3><span>流程设计</span></h3>
            <div style="float: right; margin-right: 3px;">
                <%--<input id="btnCreateRole" runat="server" type="button" value="新增流程" onclick="window.location.href = 'designer/designer.aspx?m=workflow';" class="btnPassClass" />--%>
                <input id="btnCreateRole" type="button" value="新增流程" onclick="window.location.href = 'designer/flowdesign.aspx?wid=<%=Guid.Empty.ToString() %>    ';" class="btnPassClass" />
            </div>
        </div>
        <div class="btnControl">
            <div class="btnLeft">
                <bw:AccessController ID="acontroller1" runat="server" ResourceValue="A020">
                    <ContentTemplate>
                        <input type="button" value="消息通知内容设置" class="btnNewwin" onclick="window.location.href='config/smsconfig.aspx?sms=sms';" />
                    </ContentTemplate>
                </bw:AccessController>
            </div>
            <div class="btnRight">
                <input id="btnCreateRole" type="button" value="新增流程" onclick="window.location.href = 'designer/flowdesign.aspx?wid=<%=Guid.Empty.ToString() %>    ';" class="btnFW" />
            </div>
        </div>
        <div class="dataList">
            <div class="showControl">
                <h4>导入流程</h4>
                <button onclick="return showContent(this,'tableExport');" title="收缩"><span>折叠</span></button>
            </div>
            <table id="tableExport" width="100%" cellspacing="4">
                <tr>
                    <th width="14%" align="right">导入流程配置：</th>
                    <td width="86%">
                        <asp:FileUpload ID="fuAttachment" runat="server" CssClass="inputbox" />
                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="导入" CssClass="btnFWClass" OnClientClick="return confirm('确定导入？')" />&nbsp;
                        <asp:Label ID="lblInfo" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <a class="ico_download" href="download.ashx?path=/contrib/workflow/res/templates/WorkflowTemplate.xml&displayName=<%=HttpUtility.UrlEncode("流程配置模板") %>" title="下载流程配置模板">下载流程配置模板</a>
                        <a class="ico_download" href="download.ashx?path=/contrib/workflow/res/templates/SimpleWorkflowTemplate.xml&displayName=<%=HttpUtility.UrlEncode("简单流程配置模板") %>" title="下载简单流程配置模板">下载简单流程配置模板</a>
                    </td>
                </tr>
                <tr>
                    <th width="14%" align="right">导入规则配置：</th>
                    <td width="86%">
                        <asp:FileUpload ID="RulesItem" runat="server" CssClass="inputbox" />
                        <asp:Button ID="btnRulesUpload" runat="server" Text="导入" CssClass="btnFWClass"
                            OnClick="btnRulesUpload_Click" OnClientClick="return confirm('确定导入？')" />&nbsp;                       
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <a class="ico_download" href="download.ashx?path=/contrib/workflow/res/templates/ActivityRulesTemplate.xls&displayName=<%=HttpUtility.UrlEncode("流程规则库配置模板") %>" title="下载流程配置模板">下载规则库配置模板</a>
                    </td>
                </tr>
                <tr>
                    <th width="14%" align="right">导入表单配置：</th>
                    <td width="86%">
                        <asp:FileUpload ID="fuItem" runat="server" CssClass="inputbox" />
                        <asp:Button ID="btnItemUpload" runat="server" Text="导入" CssClass="btnFWClass"
                            OnClick="btnItemUpload_Click" OnClientClick="return confirm('确定导入？')" />&nbsp;
                        <asp:Label ID="lblItem" runat="server" ForeColor="Red"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp 部署方式：
                        <asp:DropDownList ID="DropDownList1" runat="server">
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="0">更新表单</asp:ListItem>
                            <asp:ListItem Value="1">部署新版本</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="dataList">
            <div class="showControl">
                <h4>流程列表</h4>
                <button onclick="return showContent(this,'dataTable1');" title="收缩">
                    <span>折叠</span></button>
            </div>
            <div class="toolBlock" id="divSearch" style="margin-bottom: 10px; padding-bottom: 5px; text-align: right">
                <asp:TextBox ID="txtKeywords" ToolTip="请输入流程类别或流程名称" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" CssClass="btn_query" Text="搜索" OnClick="btnSearch_Click" />
            </div>
            <div class="dataTable" id="dataTable1">
                <table cellpadding="0" cellspacing="0" style="text-align: center;" class="tblClass" id="tblId1">
                    <tr>
                        <th width="7%">类型</th>
                        <th width="23%">流程名称</th>
                        <th width="70%">操作</th>
                    </tr>
                    <asp:Repeater ID="rpFlowList" runat="server"
                        OnItemCommand="rpFlowList_ItemCommand"
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
                                    <asp:LinkButton CommandName="Export" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="linkbtnExport" CssClass="ico_idt" runat="server">导出流程</asp:LinkButton>
                                    <asp:LinkButton CommandName="ExportItem" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="LinkButton1" CssClass="ico_idt" runat="server">导出表单</asp:LinkButton>
                                    <asp:LinkButton CommandName="ExportRules" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="LinkButton3" CssClass="ico_idt" runat="server" ToolTip="从流程中导出规则">导出规则</asp:LinkButton>
                                    <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="btnDelete" CssClass="ico_del" OnClientClick="return (confirm('确定删除该流程吗？'));"
                                        runat="server">删除</asp:LinkButton>
                                    <asp:LinkButton CommandName="SetEnabled" CssClass="ico_disable" CommandArgument='<%# Eval("WorkFlowID") %>' ID="btnSetEnable" runat="server">停用</asp:LinkButton>
                                    <a href="config/ConfigWorkflow.aspx?wid=<%# Eval("WorkflowId") %>" class="ico_edit">流程设置</a>
                                    <%--<a href="designer/designer.aspx?wid=<%# Eval("WorkflowId") %>" class="ico_edit">可视化设计</a>--%>
                                    <a href="designer/flowdesign.aspx?wid=<%# Eval("WorkflowId") %>" class="ico_edit">可视化设计</a>
                                    <a href="../../../../contrib/dynamicform/pages/itemcreate.aspx?wid=<%# Eval("WorkflowId") %>&EntityType=Form_Workflow" class="ico_index">表单设计</a>
                                    <a href="javascript:void(0);" id="moreFunction_<%# Eval("WorkFlowID") %>" onclick="showMenu('divMoreFun_<%# Eval("WorkFlowID") %>', this);">更多▼</a>
                                    <div id="divMoreFun_<%# Eval("WorkFlowID") %>" style="display: none">
                                        <div class="main_menu_popup" style='border: 1px solid #9B9B9B; padding: 6px; background: #F9F9F9 url(/web/res/img/menu_popup_bg.gif) repeat-x bottom;'>
                                            <table width="144px" style="margin: 0; padding: 0; line-height: 22px; cursor: pointer; font-size: 12px; border-collapse: collapse;">
                                                <tr>
                                                    <td onclick="document.location='config/ConfigReview.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                         style="background:url('<%=AppPath %>/res/img/menu_popup_normal.gif') no-repeat"  onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">抄送设置</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td onclick="document.location='config/ConfigMagage.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        style="background:url('<%=AppPath %>/res/img/menu_popup_normal.gif') no-repeat"  onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">处理人设置</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td onclick="document.location='config/ConfigIntelligentRemind.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        style="background:url('<%=AppPath %>/res/img/menu_popup_normal.gif') no-repeat"  onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">智能提醒</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td onclick="document.location='config/ConfigActivityRemark.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        style="background:url('<%=AppPath %>/res/img/menu_popup_normal.gif') no-repeat"  onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
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
                                                    <td onclick="document.location='<%=AppPath %>contrib/workflow/pages/workflowtemplate.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        style="background:url('<%=AppPath %>/res/img/menu_popup_normal.gif') no-repeat"  onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
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
                                    <asp:LinkButton CommandName="Export" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="linkbtnExport" CssClass="ico_idt" runat="server">导出流程</asp:LinkButton>
                                    <asp:LinkButton CommandName="ExportItem" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="LinkButton1" CssClass="ico_idt" runat="server">导出表单</asp:LinkButton>
                                    <asp:LinkButton CommandName="ExportRules" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="LinkButton3" CssClass="ico_idt" runat="server" ToolTip="从流程中导出规则">导出规则</asp:LinkButton>
                                    <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="btnDelete" CssClass="ico_del" OnClientClick="return (confirm('确定删除该流程吗？'));"
                                        runat="server">删除</asp:LinkButton>
                                    <asp:LinkButton CommandName="SetEnabled" CssClass="ico_disable" CommandArgument='<%# Eval("WorkFlowID") %>' ID="btnSetEnable" runat="server">停用</asp:LinkButton>
                                    <a href="config/ConfigWorkflow.aspx?wid=<%# Eval("WorkflowId") %>" class="ico_edit">流程设置</a>
                                    <%--<a href="designer/designer.aspx?wid=<%# Eval("WorkflowId") %>" class="ico_edit">可视化设计</a>--%>
                                    <a href="designer/flowdesign.aspx?wid=<%# Eval("WorkflowId") %>" class="ico_edit">可视化设计</a>
                                    <a href="../../../../contrib/dynamicform/pages/itemcreate.aspx?wid=<%# Eval("WorkflowId") %>&EntityType=Form_Workflow" class="ico_index">表单设计</a>
                                    <a href="javascript:void(0);" id="moreFunction_<%# Eval("WorkFlowID") %>" onclick="showMenu('divMoreFun_<%# Eval("WorkFlowID") %>', this);">更多▼</a>
                                    <div id="divMoreFun_<%# Eval("WorkFlowID") %>" style="display: none">
                                        <div class="main_menu_popup" style='border: 1px solid #9B9B9B; padding: 6px; background: #F9F9F9 url(/web/res/img/menu_popup_bg.gif) repeat-x bottom;'>
                                            <table width="144" style="margin: 0; padding: 0; line-height: 22px; cursor: pointer; height: 100px; font-size: 12px; border-collapse: collapse;">
                                                <tr>
                                                    <td onclick="document.location='config/ConfigReview.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        style="background:url('<%=AppPath %>/res/img/menu_popup_normal.gif') no-repeat"  onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">抄送设置</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td onclick="document.location='config/ConfigMagage.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                         style="background:url('<%=AppPath %>/res/img/menu_popup_normal.gif') no-repeat"  onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">处理人设置</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td onclick="document.location='config/ConfigIntelligentRemind.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                         style="background:url('<%=AppPath %>/res/img/menu_popup_normal.gif') no-repeat"  onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
                                                        onmouseout="background='<%=AppPath %>res/img/menu_popup_normal.gif';">
                                                        <span style="padding-left: 26px;">智能提醒</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td onclick="document.location='config/ConfigActivityRemark.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                         style="background:url('<%=AppPath %>/res/img/menu_popup_normal.gif') no-repeat"  onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
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
                                                    <td onclick="document.location='<%=AppPath %>contrib/workflow/pages/workflowtemplate.aspx?wid=<%# Eval("WorkflowId") %>';"
                                                        style="background:url('<%=AppPath %>/res/img/menu_popup_normal.gif') no-repeat"  onmouseover="background='<%=AppPath %>res/img/menu_popup_hover.gif';"
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
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var menuDiv=document.createElement("div");
        menuDiv.style.width="146px";
        menuDiv.style.height="140px";
        menuDiv.style.position="absolute";
        menuDiv.style.display == "none";
        menuDiv.style.left="0px";
        menuDiv.style.top="0px";
        document.body.appendChild(menuDiv);
        function showMenu(theId, reference) {
            var menu = document.getElementById(theId);
            var t = reference.offsetTop + 23;
            var l = reference.offsetLeft;
            while (reference = reference.offsetParent) {
                t += reference.offsetTop;
                l += reference.offsetLeft;
            }
            menuDiv.style.left=l+"px";
            menuDiv.style.top=t+"px";
            menuDiv.innerHTML=menu.innerHTML;
            menuDiv.style.display = "";  
          
        }
    </script>
</asp:Content>
