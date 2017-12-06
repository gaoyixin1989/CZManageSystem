<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="xqp2_contrib_workflow_pages_config_ConfigHistoryWorkflow" Codebehind="ConfigHistoryWorkflow.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="历史流程设置" runat="server" ></asp:Literal></span></h3>
    </div>	
    <div class="dataList">
        <div class="showControl">
            <h4>历史流程设置</h4>
            <button onclick="return showContent(this,'divSettings');" title="收缩"><span>折叠</span></button>
        </div>
        <div id="divSettings">
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
                <tr>
                    <th style="width:17%;">流程名称：</th>
                    <td colspan="5" style="padding:5px 0 5px 5px">
                        <asp:Literal ID="ltlWorkflowName" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <th>流程备注：</th>
                    <td colspan="3">
                        <asp:TextBox ID="txtRemark" runat="server"  TextMode="MultiLine" Width="520px" Height="35px"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <p align="center" style="margin-top:10px">
                <asp:Button ID="btnSave" runat="server" CssClass="btn_sav" Text="保存" 
                    onclick="btnSave_Click" />
                <input type="button" value="返回" class="btnFWClass" onclick="document.location='../workflowHistoryDeploy.aspx';" />
            </p>
        </div>
        
        <div class="showControl">
            <h4>流程任务分派设置</h4>
            <button onclick="return showContent(this,'divAllocators');" title="收缩"><span>折叠</span></button>
        </div>
        <div id="divAllocators">
            <table cellpadding="0" cellspacing="0" class="tblClass" style="text-align:center;">
                <tr>
                    <th>序号</th>
                    <th>流程步骤名称</th>
                    <th>字段控制</th>
                    <th>用户控制</th>
                    <th>组织控制</th>
                    <th>权限控制</th>
                    <th>角色控制</th>
                    <th>历史步骤处理人</th>
                    <th>以前处理人</th>
                    <th>过程控制</th>
                    <th>发起人</th>
                    <th>设置</th>
                </tr>
                <asp:Repeater ID="rptActivities" runat="server" onitemdatabound="rptActivities_ItemDataBound" EnableViewState="false">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("SortOrder") %></td>
                            <td><%# Eval("ActivityName") %></td>
                            <td><asp:Literal ID="ltlItemField" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemUsers" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemSuperior" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemResource" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemRole" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemControl" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemProcessor" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemProcessctl" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemStarter" runat="server" /></td>
                            <td>
                                <a href="configActivity.aspx?wfid=<%# Eval("WorkflowId") %>&aid=<%# Eval("ActivityId") %>" class="ico_edit">设置</a>
                                 <a href="configActivityRules.aspx?wfid=<%# Eval("WorkflowId") %>&aid=<%# Eval("ActivityId") %>" class="ico_edit">规则设置</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="trClass">
                            <td><%# Eval("SortOrder") %></td>
                            <td><%# Eval("ActivityName") %></td>
                            <td><asp:Literal ID="ltlItemField" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemUsers" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemSuperior" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemResource" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemRole" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemControl" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemProcessor" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemProcessctl" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemStarter" runat="server" /></td>
                            <td>
                                <a href="configActivity.aspx?wfid=<%# Eval("WorkflowId") %>&aid=<%# Eval("ActivityId") %>" class="ico_edit">设置</a>
                                 <a href="configActivityRules.aspx?wfid=<%# Eval("WorkflowId") %>&aid=<%# Eval("ActivityId") %>" class="ico_edit">规则设置</a>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
</asp:Content>
