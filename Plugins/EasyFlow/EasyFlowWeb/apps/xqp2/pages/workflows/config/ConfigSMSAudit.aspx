<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ConfigSMSAudit" Title="短信审批设置" Codebehind="ConfigSMSAudit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="短信审批设置" runat="server" /></span></h3>
    </div>	
    <div class="btnControl">
        <div class="btnLeft">
            <input type="button" value="流程设置" class="btnNewwin" onclick="window.location.href='configWorkflow.aspx?wid=<%=this.WorkflowId%>';" />
        </div>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>短信审批设置</h4>
            <button onclick="return showContent(this,'divSMSAuditSetting');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div id="divSMSAuditSetting">
            <div style="padding-bottom: 10px"> 说明：               
                <ul style="list-style-type: decimal; padding-left: 30px">
                    <li>短信审批内容格式参数有：&quot;#Title#&quot;(工单标题), &quot;#Creator#&quot;(发单人), &quot;#OperateType#&quot;(工单操作类型: 1
                        表示回退), &quot;#ActivityName#&quot;(流程活动步骤), &quot;#PrevActors#&quot;(上一步骤处理人), &quot;#字段名称#&quot;(抽取字段数据，如: #F1#)。</li>
                </ul>
            </div>
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 6px;">
                <tr>
                    <th style="width: 17%; text-align:right;">是否允许短信审批：</th>
                    <td style="padding: 5px 0 5px 5px">
                        <asp:CheckBox ID="chkboxSMSAudit" runat="server" Text="允许短信审批" />
                        <span style="color:#247ecf;">(选中则表示允许处理人以回复短信的方式完成工单审批)</span>
                    </td>
                </tr>
                <tr>
                    <th style="text-align:right;">允许短信审批的流程步骤：</th>
                    <td style="padding: 5px 0 5px 0px">
                        <asp:CheckBoxList ID="chkboxActivities" DataTextField="ActivityName" DataValueField="ActivityName" RepeatColumns="4" runat="server" ondatabound="chkboxActivities_DataBound"></asp:CheckBoxList>
                    </td>
                </tr>
                <%--<tr>
                    <th style="width: 17%;text-align:right;">短信审批抽取字段：</th>
                    <td style="padding: 5px 0 5px 5px">
                        <asp:DropDownList ID="ddlSMSAuditFields" runat="server" AutoPostBack="False">
                        </asp:DropDownList>
                    </td>
                </tr>--%>
                 <tr>
                    <th style="width: 17%;text-align:right;"> 短信审批内容格式：</th>
                    <td style="padding: 5px 0 5px 5px">
                        <asp:TextBox ID="txtSMSAuditNotifyFormat" TextMode="MultiLine" Width="92%" Height="100px" runat="server" />
                        <span style="color: Red">*</span>
                        <asp:RequiredFieldValidator ID="rfvSMSAuditNotifyFormat" ControlToValidate="txtSMSAuditNotifyFormat"
                            SetFocusOnError="true" runat="server" Display="None" ErrorMessage="必须填写短信审批内容格式."></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <p align="center" style="margin-top: 10px">
                <asp:Button ID="btnSaveSMSAudit" runat="server" CssClass="btn_sav" Text="保存" onclick="btnSaveSMSAudit_Click" />
                <input type="button" value="返回" class="btnFWClass" onclick="document.location='../workflowDeploy.aspx';" />
            </p>
        </div>
    </div>
</asp:Content>
