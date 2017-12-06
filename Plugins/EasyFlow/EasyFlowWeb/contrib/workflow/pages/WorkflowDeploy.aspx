<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_WorkflowDeploy" Codebehind="WorkflowDeploy.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="divList" runat="server">
        <div class="titleContent">
            <h3><span>流程设计</span></h3>
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
                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="导入" CssClass="btnFWClass" />&nbsp;
                        <asp:Label ID="lblInfo" runat="server" ForeColor="Red"></asp:Label>                         
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <a class="ico_download" href="download.ashx?path=/contrib/workflow/res/templates/WorkflowTemplate.xml&displayName=<%=HttpUtility.UrlEncode("流程配置模板") %>"  title="下载流程配置模板">下载流程配置模板</a>
                        <a class="ico_download" href="download.ashx?path=/contrib/workflow/res/templates/SimpleWorkflowTemplate.xml&displayName=<%=HttpUtility.UrlEncode("简单流程配置模板") %>"  title="下载简单流程配置模板">下载简单流程配置模板</a>
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
            <div class="dataTable" id="dataTable1">
                <table cellpadding="0" cellspacing="0" style="text-align:center;" class="tblClass" id="tblId1">
                    <tr>
                        <th width="7%">类型</th>
                        <th width="23%">流程名称</th>
                        <th width="70%">操作</th>
                    </tr>
                    <asp:Repeater ID="rpFlowList" runat="server" 
                        OnItemCommand="rpFlowList_ItemCommand" 
                        onitemdatabound="rpFlowList_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                                </td>
                                <td style="text-align:left;">
                                    <a href="WorkflowIndex.aspx?wid=<%# Eval("WorkflowId") %>">
                                        <asp:Literal ID="ltlName" runat="server" Text='<%# Eval("WorkFlowName") %>'></asp:Literal>
                                    </a>
                                </td>
                                <td>
                                    <asp:LinkButton CommandName="Export" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="linkbtnExport" CssClass="ico_idt" runat="server">导出</asp:LinkButton>
                                    <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="btnDelete" CssClass="ico_del" OnClientClick="return (confirm('确定删除该流程吗？'));"
                                        runat="server">删除</asp:LinkButton>
                                    <asp:LinkButton CommandName="SetEnabled" CssClass="ico_disable" CommandArgument='<%# Eval("WorkFlowID") %>' ID="btnSetEnable" runat="server">停用</asp:LinkButton>
                                    <a href="config/configWorkflow.aspx?wid=<%# Eval("WorkflowId") %>" class="ico_edit">流程设置</a>
                                    <a href="../../dynamicform/pages/itemcreate.aspx?wid=<%# Eval("WorkflowId") %>" class="ico_index">表单设计</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="trClass">
                                <td>
                                    <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                                </td>
                                <td style="text-align:left;">
                                    <a href="WorkflowIndex.aspx?wid=<%# Eval("WorkflowId") %>">
                                        <asp:Literal ID="ltlName" runat="server" Text='<%# Eval("WorkFlowName") %>'></asp:Literal>
                                    </a>
                                </td>
                                <td>
                                    <asp:LinkButton CommandName="Export" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="linkbtnExport" CssClass="ico_idt" runat="server">导出</asp:LinkButton>
                                    <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("WorkFlowID") %>'
                                        ID="btnDelete" CssClass="ico_del" OnClientClick="return (confirm('确定删除该流程吗？'));"
                                        runat="server">删除</asp:LinkButton>
                                    <asp:LinkButton CommandName="SetEnabled" CssClass="ico_disable" CommandArgument='<%# Eval("WorkFlowID") %>' ID="btnSetEnable" runat="server">停用</asp:LinkButton>
                                    <a href="config/configWorkflow.aspx?wid=<%# Eval("WorkflowId") %>" class="ico_edit">流程设置</a>
                                    <a href="../../dynamicform/pages/itemcreate.aspx?wid=<%# Eval("WorkflowId") %>&EntityType=Form_Workflow" class="ico_index">表单设计</a>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>                       
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
