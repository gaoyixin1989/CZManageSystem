<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ActivityRemind" Codebehind="ActivityRemind.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="IntelligentRemindInput" Src="../controls/IntelligentRemindInput.ascx"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>流程步骤提醒设置</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>按”紧急程度“设置流程步骤智能提醒</h4>
            <button onclick="return showContent(this,'divContents');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div id="divContents">
            <div style="padding-left:5px">
                1. 允许滞留时间(小时)为 －1 时，表示滞留时间无限制.<br />
                2. 提醒次数为 －1 时，表示提醒次数无限制.
            </div>
            <div>
                <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1">
                    <tr>
                        <th style="width:20%">流程名称：</th>
                        <td style="font-weight:bold"><%=this.WorkflowName%></td>
                    </tr>
                    <tr>
                        <th style="width:20%">流程步骤名称：</th>
                        <td style="font-weight:bold"><%=this.ActivityName%></td>
                    </tr>
               </table>
                <bw:IntelligentRemindInput ID="iri1" runat="server" ControlTitle="”&lt;span style='color:red'&gt;一般&lt;/span&gt;“ 工单设置" />
                <bw:IntelligentRemindInput ID="iri2" runat="server" ControlTitle="”&lt;span style='color:red'&gt;紧急&lt;/span&gt;“ 工单设置" />
                <bw:IntelligentRemindInput ID="iri3" runat="server" ControlTitle="”&lt;span style='color:red'&gt;最紧急&lt;/span&gt;“ 工单设置" />
            </div>
            <div style="text-align:center; padding:5px">
                <asp:Button id="btnSave" CssClass="btn_sav" Text="保存" runat="server" 
                    onclick="btnSave_Click" />
                <%--<input type="button" class="btnReturnClass" style="margin-left:5px" onclick="window.history.go(-1);" value="返回" />--%>
            </div>
        </div>
    </div>
</asp:Content>
