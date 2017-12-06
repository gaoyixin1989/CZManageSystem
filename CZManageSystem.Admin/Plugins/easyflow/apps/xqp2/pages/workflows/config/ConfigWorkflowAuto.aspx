<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ConfigWorkflowAuto" Title="自动处理设置" Codebehind="ConfigWorkflowAuto.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="自动处理设置" runat="server" /></span></h3>
    </div>	
    <div class="btnControl">
        <div class="btnLeft">
            <input type="button" value="流程设置" class="btnNewwin" onclick="window.location.href='configWorkflow.aspx?wid=<%=this.WorkflowId%>';" />
        </div>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>自动处理设置</h4>
        </div>
        <div>
            <div style="padding-bottom: 10px"> 说明：               
                <ul style="list-style-type: decimal; padding-left: 30px">
                    <li>自动处理：是当步骤处理人有且仅有上一步骤的处理人时，才会由系统进行自动处理。</li>
                </ul>
            </div>
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 6px;">
                <tr>
                    <th style="width: 17%; text-align:right;">是否允许自动处理：</th>
                    <td style="padding: 5px 0 5px 5px">
                        <asp:CheckBox ID="chkboxIsAuto" runat="server" Text="允许自动处理" />
                        <span style="color:#247ecf;">(选中则表示允许系统自动处理以下列表中选中的步骤)</span>
                    </td>
                </tr>
                <tr>
                    <th style="text-align:right;">允许自动处理的流程步骤：</th>
                    <td style="padding: 5px 0 5px 0px">
                        <asp:CheckBoxList ID="chkboxActivities" DataTextField="ActivityName" DataValueField="ActivityName" RepeatColumns="4" runat="server"></asp:CheckBoxList>
                    </td>
                </tr>
            </table>
            <p align="center" style="margin-top: 10px">
                <asp:Button ID="btnSave" runat="server" CssClass="btn_sav" Text="保存" 
                    onclick="btnSave_Click" />
                <input type="button" value="返回" class="btnFWClass" onclick="document.location='../workflowDeploy.aspx';" />
            </p>
        </div>
</asp:Content>
