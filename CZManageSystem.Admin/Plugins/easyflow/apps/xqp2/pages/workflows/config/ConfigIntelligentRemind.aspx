<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ConfigIntelligentRemind" Codebehind="ConfigIntelligentRemind.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="titleContent">
        您当前位置：<a href="<%=AppPath%>contrib/workflow/pages/default.aspx">首页</a> &gt; <a href="<%=AppPath%>apps/xqp2/pages/workflows/workflowDeploy.aspx">
            流程设计</a> &gt; <span class="cRed">智能提醒</span>
    </div>
    <div class="dataList">
        <div style="text-align: left">
            <div style="padding-bottom: 10px">
                说明：
                <ul style="list-style-type: decimal; padding-left: 30px">
                    <li>自然日是正常日期，按照每天24小时计算；</li>
                    <li>允许滞留时间(小时)如果设为小于或等于0，则表示不进行时效考核。 </li>
                </ul>
            </div>
            <span style="font-weight: bold">时效计算标准：</span>
            <asp:UpdatePanel ID="updpLook" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <asp:RadioButton ID="radNormalday" GroupName="stat" runat="server" Text="自然日" AutoPostBack="true"
                        OnCheckedChanged="radNormalday_CheckedChanged" ToolTip="按照每天24小时计算" />&nbsp;
                    <asp:RadioButton ID="radUnnormalday" GroupName="stat" runat="server" Text="工作日" AutoPostBack="true"
                        OnCheckedChanged="radUnnormalday_CheckedChanged" ToolTip="按照每天8.5小时计算，排除节假日" Visible="false" />&nbsp;
                     <asp:RadioButton ID="radCusday" GroupName="stat" runat="server" Text="客服工作日" AutoPostBack="true"
                        OnCheckedChanged="radCusday_CheckedChanged" ToolTip="按照每天8.5小时计算，不排除节假日" Visible="false" />&nbsp;
                    <span style="color: Red">
                        <asp:Literal runat="server" ID="ltlInfo"></asp:Literal></span>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="radNormalday" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="radUnnormalday" EventName="CheckedChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="showControl" style="display:none">
            <h4>
                工单时效设置</h4>
            <button onclick="return showContent(this,'dataDiv2');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div id="dataDiv2" style="display:none">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <div>
                        <span style="font-weight: bold">按"紧急程度"设置：</span>
                        <asp:RadioButton ID="radWorkflowA" GroupName="wfu" runat="server" Text="“一般”工单设置"
                            AutoPostBack="true" OnCheckedChanged="radWorkflowA_CheckedChanged" />&nbsp;
                        <asp:RadioButton ID="radWorkflowB" GroupName="wfu" runat="server" Text="“紧急”工单设置"
                            AutoPostBack="true" OnCheckedChanged="radWorkflowB_CheckedChanged" />&nbsp;
                        <asp:RadioButton ID="radWorkflowC" GroupName="wfu" runat="server" Text="“特急”工单设置"
                            AutoPostBack="true" OnCheckedChanged="radWorkflowC_CheckedChanged" />&nbsp;
                    </div>
                    <div class="dataTable" id="dataTable2" style="margin-top: 10px; text-align: center;">
                        <div>
                            <table class="tblClass" cellspacing="0" rules="all" border="0" id="instanceRemind"
                                style="border-width: 0px; width: 100%; border-collapse: collapse;">
                                <tr>
                                    <th scope="col">
                                        流程名称
                                    </th>
                                    <th scope="col">
                                        允许滞留时间(小时)
                                    </th>
                                    <th>
                                        超时提醒间隔(小时)
                                    </th>
                                    <th>
                                        超时提醒次数
                                    </th>
                                    <th>
                                        超时预警时间(小时)
                                    </th>
                                    <th>
                                        预警提醒间隔(小时)
                                    </th>
                                    <th scope="col">
                                        预警提醒次数
                                    </th>
                                    <th scope="col">
                                        提醒方式
                                    </th>
                                    <th scope="col" style="width: 20%;" colspan="2">
                                        操作
                                    </th>
                                </tr>
                                <tr>
                                    <td class="blue text-left" style="width: 15%;">
                                        <asp:Literal ID="ltlFlowName" runat="server"></asp:Literal>
                                        <input type="hidden" id="hidId" runat="server" />
                                    </td>
                                    <td style="width: 10%;">
                                        <input type="text" runat="server" onkeyup="if(isNaN(value))execCommand('undo')" onafterpaste="if(isNaN(value))execCommand('undo')"
                                            id="txtInstanceStayHours" style="width: 30px;" />
                                    </td>
                                    <td style="width: 10%">
                                        <input type="text" runat="server" onkeyup="if(isNaN(value))execCommand('undo')" onafterpaste="if(isNaN(value))execCommand('undo')"
                                            id="txtInstanceTimeoutInterval" style="width: 30px;" />
                                    </td>
                                    <td style="width: 10%">
                                        <input type="text" runat="server" onkeyup="if(isNaN(value))execCommand('undo')" onafterpaste="if(isNaN(value))execCommand('undo')"
                                            id="txtInstanceTimeoutTimes" style="width: 30px;" />
                                    </td>
                                    <td style="width: 10%">
                                        <input type="text" runat="server" onkeyup="if(isNaN(value))execCommand('undo')" onafterpaste="if(isNaN(value))execCommand('undo')"
                                            id="txtInstanceToHours" style="width: 30px;" />
                                    </td>
                                    <td style="width: 10%">
                                        <input type="text" runat="server" onkeyup="if(isNaN(value))execCommand('undo')" onafterpaste="if(isNaN(value))execCommand('undo')"
                                            id="txtInstanceToInterval" style="width: 30px;" />
                                    </td>
                                    <td style="width: 10%">
                                        <input type="text" runat="server" onkeyup="if(isNaN(value))execCommand('undo')" onafterpaste="if(isNaN(value))execCommand('undo')"
                                            id="txtInstanceToTimes" style="width: 30px;" />
                                    </td>
                                    <td style="width: 10%;">
                                        <asp:DropDownList ID="ddlInstanceReminderType" runat="server">
                                            <asp:ListItem Value="0">-未设置-</asp:ListItem>
                                            <asp:ListItem Value="1">电子邮件</asp:ListItem>
                                            <asp:ListItem Value="2">短信</asp:ListItem>
                                            <asp:ListItem Value="3">短信+电子邮件</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lbtnSave" Text="保存" CssClass="ico_add" OnClick="lbtnSave_Click"
                                            OnClientClick="return checkChecked()"></asp:LinkButton>
                                    </td>
                                    <td>
                                        <%--<asp:HyperLink ID="lbtnSet" Text="抄送设置" runat="server"></asp:HyperLink>--%>
                                        <select id="ddlWorkflowCopy" onchange="linktoUrl(document.getElementById('<%=hidId.ClientID %>').value,'<%=WorkflowId %>',this.value)">
                                            <option value="0">-更多设置-</option>
                                            <option value="1">预警抄送</option>
                                            <option value="2">超时抄送</option>
                                        </select>
                                    </td>
                                </tr>
                            </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="radWorkflowA" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="radWorkflowB" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="radWorkflowC" EventName="CheckedChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="searchBar" style="display:none">
            <div class="searchTitle">
                <h4>
                    排除环节</h4>
                <button onclick="return showContent(this,'authorizationInput');" title="收缩">
                    <span>折叠</span></button>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <table id="authorizationInput" class="tblGrayClass grayBackTable" cellpadding="4"
                        cellspacing="1" style="margin-top: 0; text-align: left">
                        <tr>
                            <td>
                                <asp:Literal ID="ltlActivityNames" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="radWorkflowA" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="radWorkflowB" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="radWorkflowC" EventName="CheckedChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    </div>
    <div class="showControl">
        <h4>
            环节时效设置</h4>
        <button onclick="return showContent(this,'dataDiv1');" title="收缩">
            <span>折叠</span></button>
    </div>
    <div id="dataDiv1">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div>
                    <span style="font-weight: bold">按"紧急程度"设置：</span>
                    <asp:RadioButton ID="radActivityA" GroupName="act" runat="server" Text="“一般”工单设置"
                        AutoPostBack="true" OnCheckedChanged="radActivityA_CheckedChanged" />&nbsp;
                    <asp:RadioButton ID="radActivityB" GroupName="act" runat="server" Text="“紧急”工单设置"
                        AutoPostBack="true" OnCheckedChanged="radActivityB_CheckedChanged" />&nbsp;
                    <asp:RadioButton ID="radActivityC" GroupName="act" runat="server" Text="“特急”工单设置"
                        AutoPostBack="true" OnCheckedChanged="radActivityC_CheckedChanged" />&nbsp;
                </div>
                <div class="dataTable" id="dataTable1" style="margin-top: 10px; text-align: center;">
                    <asp:GridView ID="gvRemind" Width="100%" CssClass="tblClass" DataKeyNames="ID" runat="server"
                        AutoGenerateColumns="False" OnRowCancelingEdit="gvRemind_RowCancelingEdit" OnRowEditing="gvRemind_RowEditing"
                        OnRowUpdating="gvRemind_RowUpdating" OnRowDataBound="gvRemind_RowDataBound" BorderWidth="0">
                        <AlternatingRowStyle CssClass="trClass" />
                        <Columns>
                            <asp:BoundField DataField="ActivityName" HeaderText="步骤名称" ReadOnly="true">
                                <ItemStyle Width="15%" CssClass="text-left"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="字段时效考核" Visible="false">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <%--<span>
                                        <%# Eval("ExpectFinishTime") != null ? Eval("ExpectFinishTime").ToString().Split('$')[1] : Eval("ExpectFinishTime")%></span>--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlLimitedTime" runat="server">
                                        <asp:ListItem Value="">- 选择字段 -</asp:ListItem>
                                        <asp:ListItem Value="ExpectFinishTime">期望完成时间</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="允许滞留时间(小时)">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <%# (float.Parse(Eval("StayHours").ToString()) <= 0) ? "无限制" : Eval("StayHours")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtStayHours" Width="30px" onkeyup="if(isNaN(value))execCommand('undo')"
                                        onafterpaste="if(isNaN(value))execCommand('undo')" Text='<%# float.Parse(Eval("StayHours").ToString())<=0 ? -1 :Eval("StayHours") %>'
                                        runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="超时提醒间隔(小时)">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <%# float.Parse(Eval("TimeoutInterval").ToString()) <=0 ? "无限制" : Eval("TimeoutInterval")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTimeoutInterval" Width="30px" onkeyup="if(isNaN(value))execCommand('undo')"
                                        onafterpaste="if(isNaN(value))execCommand('undo')" Text='<%# float.Parse(Eval("TimeoutInterval").ToString()) <= 0 ? -1 :Eval("TimeoutInterval")%>'
                                        runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="超时提醒次数">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <%# int.Parse(Eval("TimeoutTimes").ToString()) <=0 ? "无限制" : Eval("TimeoutTimes")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTimeoutTimes" Width="30px" onkeyup="if(!checkNum(value))execCommand('undo')"
                                        onafterpaste="if(!checkNum(value))execCommand('undo')" Text='<%# int.Parse(Eval("TimeoutTimes").ToString()) <= 0 ? -1 :Eval("TimeoutTimes")%>'
                                        runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="超时预警时间(小时)">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <%# float.Parse(Eval("ToHours").ToString()) <= 0 ? "无限制" : Eval("ToHours")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtToHours" Width="30px" onkeyup="if(isNaN(value))execCommand('undo')"
                                        onafterpaste="if(isNaN(value))execCommand('undo')" Text='<%# float.Parse(Eval("ToHours").ToString()) <= 0 ? -1 :Eval("ToHours") %>'
                                        runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="预警提醒间隔(小时)">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <%# float.Parse(Eval("ToInterval").ToString()) <= 0 ? "无限制" : Eval("ToInterval")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtToInterval" Width="30px" onkeyup="if(isNaN(value))execCommand('undo')"
                                        onafterpaste="if(isNaN(value))execCommand('undo')" Text='<%# float.Parse(Eval("ToInterval").ToString()) <= 0 ? -1 :Eval("ToInterval")%>'
                                        runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="预警提醒次数">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <%# int.Parse(Eval("ToTimes").ToString()) <= 0 ? "无限制" : Eval("ToTimes")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtToTimes" Width="30px" onkeyup="if(!checkNum(value))execCommand('undo')"
                                        onafterpaste="if(!checkNum(value))execCommand('undo')" Text='<%# int.Parse(Eval("ToTimes").ToString()) <= 0 ? -1 :Eval("ToTimes")%>'
                                        runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="提醒方式">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemindTypeName" runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlRemindType" runat="server">
                                        <asp:ListItem Value="0">-未设置-</asp:ListItem>
                                        <asp:ListItem Value="1">电子邮件</asp:ListItem>
                                        <asp:ListItem Value="2">短信</asp:ListItem>
                                        <asp:ListItem Value="3">短信+电子邮件</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField HeaderText="" EditText="&lt;span class=&quot;ico_edit&quot;&gt;设置&lt;span&gt;"
                                UpdateText="保存" ShowEditButton="true" ShowDeleteButton="false" CancelText="取消"
                                HeaderStyle-Width="10%" />
                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText=" ">
                                <ItemTemplate>
                                    <%--<asp:HyperLink ID="hlinkActivity" runat="server" Text="抄送设置"></asp:HyperLink>--%>
                                    <select id="ddlActivityCopy" onchange="linktoUrl('<%# Eval("Id") %>','<%=WorkflowId %>',this.value)">
                                        <option value="0">-更多设置-</option>
                                        <%--<option value="1">预警抄送</option>--%>
                                        <option value="2">超时抄送</option>
                                    </select>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemindType" runat="server" Text='<%# Eval("RemindType") %>' />
                                    <asp:Label ID="lblActivityName" runat="server" Text='<%# Eval("ActivityName") %>' />
                                    <asp:Label ID="lblExtArgs" runat="server" Text='<%# Eval("ExtArgs") %>' />
                                    <asp:Label ID="lblLimitedTime" runat="server" Text=' <%# Eval("ExpectFinishTime")%>' />
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="radActivityA" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="radActivityB" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="radActivityC" EventName="CheckedChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </div>
    <div class="showControl">
        <h4>
            消息格式设置</h4>
        <button onclick="return showContent(this,'divSMSSetting');" title="收缩">
            <span>折叠</span></button>
    </div>
    <div id="divSMSSetting">
        <div style="padding-bottom: 10px">
            说明：
            <ul style="list-style-type: decimal; padding-left: 30px">
                <li>提醒信息(短信及邮件)格式参数有：：&quot;#Title#&quot;(工单标题), &quot;#Creator#&quot;(发单人), &quot;#OperateType#&quot;(工单操作类型:
                    1 表示回退), &quot;#ActivityName#&quot;(流程活动步骤)，&quot;#PrevActors#&quot;(上一步骤处理人),&quot;#Urgency#&quot;(紧急程度)
                    ,&quot;#Aiid#&quot;(步骤ID),&quot;#Time#(时间),&quot;#CurrentActors#(当前处理人)。</li>
               
            </ul>
        </div>
        <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 6px;">
            <tr style="display:none">
                <th style="width: 17%; text-align: right;">
                    工单预警消息通知的内容格式：
                </th>
                <td style="padding: 5px 0 5px 5px">
                    <asp:TextBox ID="txtWorkOrderWarningNotifyformat" TextMode="MultiLine" Width="92%"
                        Height="50px" runat="server" />
                </td>
            </tr>
            <tr style="display:none">
                <th style="width: 17%; text-align: right;">
                    工单超时消息通知的内容格式：
                </th>
                <td style="padding: 5px 0 5px 5px">
                    <asp:TextBox ID="txtWorkOrderTimeoutNotifyformat" TextMode="MultiLine" Width="92%"
                        Height="50px" runat="server" />
                </td>
            </tr>
            <tr>
                <th style="width: 17%; text-align: right;">
                    环节预警消息通知的内容格式：
                </th>
                <td style="padding: 5px 0 5px 5px">
                    <asp:TextBox ID="txtStepWarningNotifyformat" TextMode="MultiLine" Width="92%" Height="50px"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <th style="width: 17%; text-align: right;">
                    环节超时消息通知的内容格式：
                </th>
                <td style="padding: 5px 0 5px 5px">
                    <asp:TextBox ID="txtStepTimeoutNotifyformat" TextMode="MultiLine" Width="92%" Height="50px"
                        runat="server" />
                </td>
            </tr>
        </table>
        <p align="center" style="margin-top: 10px">
            <asp:Button ID="btnSaveSMSConfig" runat="server" CssClass="btn_sav" Text="保存" OnClick="btnSaveSMSConfig_Click" />
        </p>
    </div>
    </div>

    <script language="javascript">
        function checkChecked() {
            if (!document.getElementById("<%=radNormalday.ClientID %>").checked && !document.getElementById("<%=radUnnormalday.ClientID %>").checked && !document.getElementById("<%=radCusday.ClientID %>").checked) {
                alert("请选择时效考核标准！");
                return false;
            }
            if (document.getElementById("<%=ddlInstanceReminderType.ClientID %>").value == 0) {
                if (document.getElementById("<%=txtInstanceStayHours.ClientID %>").value != "" && document.getElementById("<%=txtInstanceStayHours.ClientID %>").value > 0) {
                    alert("请选择时效考核提醒方式！");
                    return false;
                }
            }
            return true;
        }

        function onToggleNotify(chkName, isChecked) {
            var inputArray = document.getElementsByTagName("input");
            for (var i = 0; i < inputArray.length; i++) {
                if (inputArray[i].type == "checkbox" && inputArray[i].name.indexOf(chkName) != -1 && inputArray[i].title != "1") {
                    inputArray[i].checked = isChecked;
                }
            }
        }

        $(function() {
            if ($("#<%=txtWorkOrderWarningNotifyformat.ClientID %>").val() == "")
                $("#<%=txtWorkOrderWarningNotifyformat.ClientID %>").attr("value", "您好！《#title#》工单即将于[#Time#]超时，现处于[#ActivityName#]步骤，处理人是[#CurrentActors#]。请登录平台处理！");
            if ($("#<%=txtWorkOrderTimeoutNotifyformat.ClientID %>").val() == "")
                $("#<%=txtWorkOrderTimeoutNotifyformat.ClientID %>").attr("value", "您好！《#title#》工单已于#Time#超时，现处于[#ActivityName#]步骤，处理人是[#CurrentActors#]。请登录平台处理！");
            if ($("#<%=txtStepWarningNotifyformat.ClientID %>").val() == "")
                $("#<%=txtStepWarningNotifyformat.ClientID %>").attr("value", "您好！《#title#》工单的[#ActivityName#]步骤环节将于[#Time#]超时，处理人是[#CurrentActors#]。请登录平台处理！");
            if ($("#<%=txtStepTimeoutNotifyformat.ClientID %>").val() == "")
                $("#<%=txtStepTimeoutNotifyformat.ClientID %>").attr("value", "您好！《#title#》工单的[#ActivityName#]步骤环节已于[#Time#]超时，处理人是[#CurrentActors#]。请登录平台处理！");
        });

        function linktoUrl(id, wid, t) {
            if (t == "3") { }
            //window.location = 'ActivityRemind.aspx?wid=' + wid + '&w=' + w + '&a=' + a + '&t=' + s + '&c=' + t;
            else
                window.location = 'ConfigIntelligentRemindNotice.aspx?wid=' + wid + '&id=' + id + '&c=' + t;
        }

        function checkNum(obj) {           
            if (isNaN(obj))
                return false;
            //定义正则表达式部分
            var reg = /^\d+$/;
            if (obj.constructor === String) {
                var re = obj.match(reg);
                if (re == null || re == '')
                    return false;
            }
            return true;
        }
    </script>

</asp:Content>
