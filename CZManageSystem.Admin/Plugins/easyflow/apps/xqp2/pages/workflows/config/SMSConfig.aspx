<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_SMSConfig" Title="短信消息设置" Codebehind="SMSConfig.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="短信内容设置" runat="server" /></span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>消息通知内容设置</h4>
            <button onclick="return showContent(this,'divSMSSetting');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div id="divSMSSetting">
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 6px;">
                 <tr>
                    <th style="width: 17%;text-align:right;"> 退回工单消息通知的内容格式：</th>
                    <td style="padding: 5px 0 5px 5px">
                        <asp:TextBox ID="txtRejectMessage" TextMode="MultiLine" Width="92%" Height="50px" runat="server" />
                        <div style="color:#247ecf">参数： (1) #title#：工单标题.</div>
                    </td>
                </tr>
                 <tr>
                    <th style="width: 17%;text-align:right;"> 转交工单消息通知的内容格式：</th>
                    <td style="padding: 5px 0 5px 5px">
                        <asp:TextBox ID="txtAssignmentMessage" TextMode="MultiLine" Width="92%" Height="50px" runat="server" />
                        <div style="color:#247ecf">参数： (1) #title#：工单标题。 (2) #from#: 转交人。</div>
                    </td>
                </tr>
                 <tr>
                    <th style="width: 17%;text-align:right;"> 短信审批成功内容格式：</th>
                    <td style="padding: 5px 0 5px 5px">
                        <asp:TextBox ID="txtFeedbackSuccessMessage" TextMode="MultiLine" Width="92%" Height="50px" runat="server" />
                        <div style="color:#247ecf">参数： (1) #NextActivities#：下一步骤以及下一步骤操作人，无数据时为空。</div>
                    </td>
                </tr>
                 <tr>
                    <th style="width: 17%;text-align:right;"> 短信审批失败内容格式：</th>
                    <td style="padding: 5px 0 5px 5px">
                        <asp:TextBox ID="txtFeedbackErrorMessge" TextMode="MultiLine" Width="92%" Height="50px" runat="server" />
                    </td>
                </tr>
                 <tr>
                    <th style="width: 17%;text-align:right;"> 短信审批回复有误的内容格式：</th>
                    <td style="padding: 5px 0 5px 5px">
                        <asp:TextBox ID="txtReceiveInvalidMessage" TextMode="MultiLine" Width="92%" Height="50px" runat="server" />
                    </td>
                </tr>
                 <tr>
                    <th style="width: 17%;text-align:right;"> 短信审批回复有误的最后一次提醒内容格式：</th>
                    <td style="padding: 5px 0 5px 5px">
                        <asp:TextBox ID="txtLastReceiveInvalidMessage" TextMode="MultiLine" Width="92%" Height="50px" runat="server" />
                    </td>
                </tr>
                 <tr>
                    <th style="width: 17%;text-align:right;"> 酬金申告流程结束答复会员步骤的消息内容格式：</th>
                    <td style="padding: 5px 0 5px 5px">
                        <asp:TextBox ID="txtGratuityFeedbackMessage" TextMode="MultiLine" Width="92%" Height="50px" runat="server" />
                        <div style="color:#247ecf">参数： (1) #title#：工单标题。 (2) #creator#: 发单人。</div>
                    </td>
                </tr>
            </table>
            <p align="center" style="margin-top: 10px">
                <asp:Button ID="btnSaveSMSConfig" runat="server" CssClass="btn_sav" Text="保存"  onclick="btnSaveSMSConfig_Click" />
                <input type="button" value="返回" class="btnFWClass" onclick="document.location='../workflowDeploy.aspx';" />
            </p>
        </div>
    </div>
</asp:Content>
