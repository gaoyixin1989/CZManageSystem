<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ConfigWorkflowRelation" Codebehind="ConfigWorkflowRelation.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="titleContent">
        您当前位置：<a href="<%=AppPath%>contrib/workflow/pages/default.aspx">首页</a> &gt; <a href="<%=AppPath%>apps/xqp2/pages/workflows/workflowDeploy.aspx">
            流程设计</a> &gt; <span class="cRed">子流程配置</span>
    </div>
    <div class="dataList">
        <div style="text-align: left">
            <div style="padding-bottom: 10px">
                说明：
                <ul style="list-style-type: decimal; padding-left: 30px">
                    <li>只有启用“超时自动处理”以及配置了步骤的智能提醒设置后，自动发起子流程才会起效；</li>
                </ul>
            </div>
        </div>
        
    <div class="showControl">
        <h4>
            子流程信息设置</h4>
        <button onclick="return showContent(this,'dataDiv1');" title="收缩">
            <span>折叠</span></button>
    </div>
    <div id="dataDiv1">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div style="display:none">
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
                        OnRowUpdating="gvRemind_RowUpdating" 
                        OnRowDataBound="gvRemind_RowDataBound" BorderWidth="0" 
                        onrowdeleting="gvRemind_RowDeleting">
                        <AlternatingRowStyle CssClass="trClass" />
                        <Columns>
                            <asp:BoundField DataField="ActivityName" HeaderText="步骤名称" ReadOnly="true">
                                <ItemStyle Width="15%" CssClass="text-left"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="子流程">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRelationWorkflowName" runat="server" Text='<%# Eval("RelationWorkflowName") %>'/>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlRelationWorkflowName" runat="server">
                                        <asp:ListItem Value="">- 选择子流程 -</asp:ListItem>
                                        
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="子流程呼叫配置">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                      <asp:Label ID="lblSettingTypeName" runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlSettingType" runat="server">
                                        <asp:ListItem Value="0">未设置</asp:ListItem>
                                        <asp:ListItem Value="1">单发起</asp:ListItem>
                                        <asp:ListItem Value="2">多发起</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="子流程发起类型">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                      <asp:Label ID="lblTriggerTypeName" runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlTriggerType" runat="server">
                                        <asp:ListItem Value="0">未设置</asp:ListItem>
                                        <%--<asp:ListItem Value="1">手动发起</asp:ListItem>--%>
                                        <asp:ListItem Value="2">超时自动发起</asp:ListItem>
                                       <%-- <asp:ListItem Value="3">手动+超时自动发起</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="父流程流转规则">
                                <ItemStyle Width="10%"></ItemStyle>
                                <ItemTemplate>
                                      <asp:Label ID="lblOperateTypeName" runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlOperateType" runat="server">
                                        <asp:ListItem Value="0">未设置</asp:ListItem>
                                        <asp:ListItem Value="1">等待子流程结束</asp:ListItem>
                                        <asp:ListItem Value="2">根据子流程字段内容</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField HeaderText="" EditText="&lt;span class=&quot;ico_edit&quot;&gt;设置&lt;span&gt;"
                                UpdateText="保存" ShowEditButton="true" ShowDeleteButton="false" CancelText="取消"
                                HeaderStyle-Width="10%" />
                                <asp:TemplateField HeaderStyle-Width="6%" HeaderText=" ">
                                <ItemTemplate>
                                <asp:LinkButton Text="&lt;span class=&quot;ico_del&quot;&gt;停用&lt;span&gt;" ID="lbtnDel" runat="server" CommandName="Delete" />
                                </ItemTemplate>
                                </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText=" ">
                                <ItemTemplate>
                                    <a id="hlinkActivity" href="configworkflowrelationext.aspx?id=<%# Eval("Id") %>&wid=<%# Eval("WorkflowId") %>">更多设置 </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkflowId" runat="server" Text='<%# Eval("WorkflowId") %>' />
                                    <asp:Label ID="lblActivityId" runat="server" Text='<%# Eval("ActivityId") %>' />
                                    <asp:Label ID="lblSettingType" runat="server" Text='<%# Eval("SettingType") %>' />
                                    <asp:Label ID="lblTriggerType" runat="server" Text='<%# Eval("TriggerType") %>' />
                                    <asp:Label ID="lblOperateType" runat="server" Text='<%# Eval("OperateType") %>' />
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>' />
                                    <asp:Label ID="lblRelationWorkflowNameValue" runat="server" Text='<%# Eval("RelationWorkflowName") %>'/>
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

    <script language="javascript">

        function onToggleNotify(chkName, isChecked) {
            var inputArray = document.getElementsByTagName("input");
            for (var i = 0; i < inputArray.length; i++) {
                if (inputArray[i].type == "checkbox" && inputArray[i].name.indexOf(chkName) != -1 && inputArray[i].title != "1") {
                    inputArray[i].checked = isChecked;
                }
            }
        }

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
