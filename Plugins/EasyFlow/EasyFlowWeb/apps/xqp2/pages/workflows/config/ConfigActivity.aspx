<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="xqp2_contrib_workflow_pages_config_ConfigActivity" Codebehind="ConfigActivity.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="任务分派设置" runat="server" /></asp:Literal></span></h3>
    </div>
    <div class="btnControl">
        <div class="btnLeft">
            <input type="button" value="提醒时间段设置" class="btnNewwin" onclick="window.location.href='ConfigRemindTime.aspx?eid=<%=this.ActivityId%>&name=<%=Server.UrlEncode(this.ActivityName) %>';" />
        </div>
    </div>
    <div class="dataList">
        <div id="divSettings">
            <div style="padding-bottom:10px">
                说明：

                <ul style="list-style-type:decimal; padding-left:30px">
                    <li>输入用户时，只输入用户名(字符、数字以及下划线的组合)，各用户名以逗号隔开(&quot;,&quot;)；</li>
                </ul>
            </div>
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
                <tr>
                    <th style="width:17%;">流程步骤名称：</th>
                    <td style="width:82%;padding:5px 0 5px 5px">
                        <asp:Literal ID="ltlActivityName" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <th>步骤命令规则：</th>
                    <td>
                        <asp:TextBox ID="txtCommandRules" TextMode="MultiLine" Width="520px" Height="70px" runat="server"></asp:TextBox>
                        <div style="color:red; padding:5px">
                            - 命令规则必须符合 NVelocity 的格式。

                        </div>
                    </td>
                </tr>
                <tr>
                    <th>打印设置：</th>
                    <td>
                        <asp:RadioButton ID="radOpenPrint" runat="server" GroupName="Print" Text="开启" Checked="true"></asp:RadioButton>
                        <asp:RadioButton ID="radClosePrint" runat="server" GroupName="Print" Text="关闭"></asp:RadioButton>
                        <div style="color:red; padding:5px">
                            - 默认开启打印。

                        </div>
                    </td>
                </tr>
                <tr>
                    <th>打印次数：</th>
                    <td>
                        <asp:TextBox ID="txtPrintAmount" runat="server" onkeyup="if(isNaN(value))execCommand('undo')" onafterpaste="if(isNaN(value))execCommand('undo')" Text="-1"></asp:TextBox>
                        <div style="color:red; padding:5px">
                            - 打印设置选 开启 时此设置才生效，-1表示无限制打印次数。

                        </div>
                    </td>
                </tr>
                <tr>
                    <th>允许编辑处理意见：</th>
                    <td>
                        <asp:CheckBox ID="chkOption" Text="允许编辑" runat="server" />
                        <div style="color:red; padding:5px">
                            - 选中则在处理工单的时候允许编辑处理意见。

                        </div>
                    </td>
                </tr>
                <tr>
                    <th>允许退还再提交：</th>
                    <td>
                        <asp:CheckBox ID="chkReturn" Text="允许" runat="server" />
                        <div style="color:red; padding:5px">
                            - 选中则在此步骤退还时可以直接提交到此步骤。

                        </div>
                    </td>
                </tr>
                <tr>
                    <th>允许手机审批：</th>
                    <td colspan="3">
                        <asp:CheckBox ID="chkIsMobile" runat="server" Text="允许手机审批" />
                        <div style="color:red; padding:5px">
                            - 勾选后允许该步骤在手机版的页面上审批。

                        </div>
                    </td>
                </tr>
                <tr>
                    <th style="width: 17%; text-align:right;">是否允许自动处理：</th>
                    <td style="padding: 5px 0 5px 5px" colspan="3">
                        <asp:CheckBox ID="chkboxIsAuto" runat="server" Text="允许超时自动处理" />
                        <span style="color:#247ecf;">(选中则表示允许工单在该步骤滞留超时系统自动处理)</span>
                    </td>
                </tr>
                <tr>
                    <th>下行分派控制类型：</th>
                    <td style="padding:5px 0 5px 0">
                        <div style="margin-left:10px" id="divControlTypes" runat="server">
                            <span><input type="checkbox" id="chkField" name="chkField" runat="server" /><label for="<%=chkField.ClientID %>">字段控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkUsers" name="chkUsers" runat="server"/><label for="<%=chkUsers.ClientID %>">用户控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkOrg" name="chkOrg" runat="server" /><label for="<%=chkOrg.ClientID%>">组织控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkRes" name="chkRes" runat="server"/><label for="<%=chkRes.ClientID %>">权限控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkRole" name="chkRole" runat="server" /><label for="<%=chkRole.ClientID %>">角色控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkControl" name="chkControl" runat="server" /><label for="<%=chkControl.ClientID %>">历史步骤处理人</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkPssor" name="chkPssor" runat="server" /><label for="<%=chkPssor.ClientID %>">以前处理人</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkPssctl" name="chkPssctl" runat="server" /><label for="<%=chkPssctl.ClientID %>">过程控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkToOwner" name="chkToOwner" runat="server" /><label for="<%=chkToOwner.ClientID %>">自定义控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkStarter" name="chkStarter" runat="server" /><label for="<%=chkStarter.ClientID %>">发起人</label></span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>平转分派控制类型：</th>
                    <td style="padding:5px 0 5px 0">
                        <div style="margin-left:88px" id="divAssignControlTypes" runat="server">
                            <span><input type="checkbox" id="chkUsersAssign" name="chkUsersAssign" runat="server" /><label for="<%=chkUsersAssign.ClientID %>">用户控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkOrgAssign" name="chkOrgAssign" runat="server" /><label for="<%=chkOrgAssign.ClientID %>">组织控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkResAssign" name="chkResAssign" runat="server" /><label for="<%=chkResAssign.ClientID %>">权限控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkRoleAssign" name="chkRoleAssign" runat="server" /><label for="<%=chkRoleAssign.ClientID %>">角色控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkControlAssign" name="chkControlAssign" runat="server" /><label for="<%=chkControlAssign.ClientID %>">历史步骤处理人</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkPssorAssign" name="chkPssorAssign" runat="server" /><label for="<%=chkPssorAssign.ClientID %>">以前处理人</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkPssctlAssign" name="chkPssctlAssign" runat="server" /><label for="<%=chkPssctlAssign.ClientID %>">过程控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkToOwnerAssign" name="chkToOwnerAssign" runat="server" /><label for="<%=chkToOwnerAssign.ClientID %>">自定义控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkStarterAssign" name="chkStarterAssign" runat="server" /><label for="<%=chkStarterAssign.ClientID %>">发起人</label></span>
                        </div>
                    </td>
                </tr>
                <tr id="trUsers" runat="server" style="padding:5px 0 5px 0;display:none">
                    <th>下行用户设置：</th>
                    <td>
                        <asp:TextBox ID="txtUsers" TextMode="MultiLine" Width="520px" Height="35px" runat="server"></asp:TextBox>
                        <a href="javascript:void(0);" onclick="javascrpt:return openUserSelector('<%=txtUsers.ClientID%>');">选择用户</a>
                    </td>
                </tr>
                <tr id="trUsersAssign" runat="server" style="padding:5px 0 5px 0;display:none">
                    <th>平转用户设置：</th>
                    <td>
                        <asp:TextBox ID="txtUsersAssign" TextMode="MultiLine" Width="520px" Height="35px" runat="server"></asp:TextBox>
                        <a href="javascript:void(0);" onclick="javascrpt:return openUserSelector('<%=txtUsersAssign.ClientID%>');">选择用户</a>
                    </td>
                </tr>
                <tr id="trOrg" runat="server" style="display:none">
                    <th>下行组织设置：</th>
                    <td>
                        <asp:Literal ID="ltlOrg" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr id="trOrgAssign" runat="server" style="display:none">
                    <th>平转组织设置：</th>
                    <td>
                        <asp:Literal ID="ltlOrgAssign" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr id="trRole" runat="server" style="display:none">
                    <th>下行角色设置：</th>
                    <td>
                        <asp:Literal ID="ltlRole" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr id="trRoleAssign" runat="server" style="display:none">
                    <th>平转角色设置：</th>
                    <td>
                        <asp:Literal ID="ltlRoleAssign" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr id="orgType" style="display:none" runat="server">
                    <th>下行分派历史步骤处理人：</th>
                    <td style="padding:5px 0 5px 0">
                        <div style="margin-left:10px" id="div1">
                            <asp:DropDownList ID = "drdlActivities" runat="server"></asp:DropDownList>&nbsp;步骤的处理人
                        </div>
                    </td>
                </tr>
                <tr id="orgTypeAssign" style="display:none" runat="server">
                    <th>平转分派历史步骤处理人：</th>
                    <td style="padding:5px 0 5px 0">
                        <div style="margin-left:10px" id="div2">
                            <asp:DropDownList ID = "drdlActivitiesAssign" runat="server"></asp:DropDownList>&nbsp;步骤的处理人
                        </div>
                    </td>
                </tr>
                <tr id="orgPssor" style="display:none" runat="server">
                    <th>下行分派过程控制：</th>
                    <td style="padding:5px 0 5px 0">
                        <div style="margin-left:10px" id="div3">
                            <asp:DropDownList ID = "drdlPssor" runat="server"></asp:DropDownList>&nbsp;步骤的组织控制类型
                        </div>
                    </td>
                </tr>
                <tr id="orgPssorAssign" style="display:none" runat="server">
                    <th>平转分派过程控制：</th>
                    <td style="padding:5px 0 5px 0">
                        <div style="margin-left:10px" id="div4">
                            <asp:DropDownList ID = "drdlPssorAssign" runat="server"></asp:DropDownList>&nbsp;步骤的组织控制类型
                        </div>
                    </td>
                </tr>
                <tr id="ownerType" runat="server" style="display:none">
                    <th>
                        下行分派自定义控制：
                    </th>
                    <td style="padding: 5px 0 5px 0">
                        <div style="margin-left: 10px" id="div6">
                            部门：<asp:DropDownList ID="drdlDepartments" runat="server" OnSelectedIndexChanged="drdlDepartments_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            &nbsp;
                            <asp:UpdatePanel ID="udpGetContent" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    科室：<asp:DropDownList ID="drdlOffice" runat="server" 
                                        onselectedindexchanged="drdlOffice_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="drdlDepartments" EventName="selectedindexchanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            &nbsp;
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:CheckBox ID="radDepartment" runat="server" Text="部门经理" Enabled="false" />&nbsp;
                                    <asp:CheckBox ID="radDepartment2" runat="server" Text="部门副经理" Enabled="false" />&nbsp;
                                    <asp:CheckBox ID="radOffice" runat="server" Text="室经理" Enabled="false" />
                                    <asp:CheckBox ID="radUser" runat="server" Text="员工" ToolTip="部门或科室的所有员工"  Enabled="false"/>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="drdlDepartments" EventName="selectedindexchanged" />
                                    <asp:AsyncPostBackTrigger ControlID="drdlOffice" EventName="selectedindexchanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            &nbsp;
                        </div>
                    </td>
                </tr>
                <tr id="ownerAssignType" runat="server" style="display:none">
                    <th>
                        平转分派自定义控制：
                    </th>
                    <td style="padding: 5px 0 5px 0">
                        <div style="margin-left: 10px" id="div5">
                            部门：<asp:DropDownList ID="drdlDepartmentsAssign" runat="server" OnSelectedIndexChanged="drdlDepartmentsAssign_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            &nbsp;
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    科室：<asp:DropDownList ID="drdlOfficeAssign" runat="server" 
                                        onselectedindexchanged="drdlOfficeAssign_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="drdlDepartmentsAssign" EventName="selectedindexchanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            &nbsp;
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:CheckBox ID="radDepartmentAssign" runat="server" Text="部门经理" Enabled="false" />&nbsp;
                                    <asp:CheckBox ID="radDepartment2Assign" runat="server" Text="部门副经理" Enabled="false" />&nbsp;
                                    <asp:CheckBox ID="radOfficeAssign" runat="server" Text="室经理" Enabled="false" />
                                    <asp:CheckBox ID="radUserAssign" runat="server" Text="员工" ToolTip="部门或科室的所有员工"  Enabled="false"/>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="drdlDepartmentsAssign" EventName="selectedindexchanged" />
                                    <asp:AsyncPostBackTrigger ControlID="drdlOfficeAssign" EventName="selectedindexchanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            &nbsp;
                        </div>
                    </td>
                </tr>
                <tr id="trFields" runat="server" style="padding:5px 0 5px 0;display:none">
                    <th>下行字段设置：</th>
                    <td>
                        <div class="showControl">
                            <h4>字段控制设置</h4>
                            <button onclick="return showContent(this,'divFieldControls1');" title="收缩"><span>折叠</span></button>
                        </div>
                        <div id="divFieldControls1">
                            <div style="margin-bottom:10px">
                                选择字段：

                                <asp:DropDownList ID="ddlFields" runat="server" AutoPostBack="true" onselectedindexchanged="ddlFields_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <asp:UpdatePanel ID="updatePanel1" runat="server">
                                <ContentTemplate>
                                <div id="divExistsFields" style="color:red" runat="server"></div>
                                <asp:Repeater ID="rptFieldControls" runat="server" OnItemDataBound="rptList_ItemDataBound">
                                    <HeaderTemplate>
                                        <table class="tblGrayClass" style="text-align:center; width:100%" cellpadding="4" cellspacing="1">
                                        <tr>
                                            <th style="width:20%;text-align:center">字段对应值</th>
                                            <th style="width:80%;text-align:center">用户设置</th>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align:left; vertical-align:top">
                                                <asp:HiddenField ID="hiddenFieldId" runat="server" Value='<%# Eval("Id") %>' />
                                                <asp:TextBox ID="txtFieldValue" runat="server" Width="100px" Text='<%# Eval("FieldValue") %>'></asp:TextBox>
                                            </td>
                                            <td style="text-align:left">
                                                <asp:TextBox ID="txtTargetUsers" TextMode="MultiLine" Height="35px" Width="420px" runat="server" Text='<%# Eval("TargetUsers") %>'></asp:TextBox>
                                                <a href="javascript:void(0);" id="btnPopupUserSelector" runat="server">选择用户</a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlFields" EventName="selectedindexchanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr style="display:none;">
                    <th>下行默认控制类型：</th>
                    <td>
                        <select id="selDefaultTypes" runat="server" style="width:120px">
                            <option value="">无</option>
                            <option value="field">字段控制</option>
                            <option value="users">用户控制</option>
                            <option value="superior">组织控制</option>
                            <option value="resource">权限控制</option>
                            <option value="resource">以前处理人</option>
                            <option value="starter">发起人</option>
                        </select>
                    </td>                 
                </tr>
                <tr style="display:none;">
                    <th>平转默认控制类型：</th>
                    <td>
                        <select id="selAssignDefaultTypes" runat="server" style="width:120px">
                            <option value="">无</option>
                            <option value="users">用户控制</option>
                            <option value="superior">组织控制</option>
                            <option value="resource">权限控制</option>
                            <option value="resource">以前处理人</option>
                            <option value="starter">发起人</option>
                        </select>
                    </td>
                </tr>
            </table>
            <p align="center" style="margin-top:10px">
                <asp:Button ID="btnSave" runat="server" CssClass="btn_sav" Text="保存" 
                    onclick="btnSave_Click" />
                <input type="button" value="返回" class="btnFWClass" onclick="window.history.go(-1);" />
            </p>
            <div>
                <asp:ValidationSummary ID="vsummary1" runat="server" ShowSummary="false" ShowMessageBox="true" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
    <!--//
        $(function () {
            $("#<%=chkField.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=trFields.ClientID %>").css("display", "");
                }
                else {
                    $("#<%=trFields.ClientID %>").css("display", "none");
                }
            });
            $("#<%=chkUsers.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=chkUsersAssign.ClientID %>").attr("checked", "checked");
                    $("#<%=trUsers.ClientID %>").css("display", "");
                    $("#<%=trUsersAssign.ClientID %>").css("display", "");
                }
                else {
                    $("#<%=this.txtUsers.ClientID %>").val("");
                    $("#<%=trUsers.ClientID %>").css("display", "none");
                }
            });
            $("#<%=chkUsersAssign.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=trUsersAssign.ClientID %>").css("display", "");
                }
                else {
                    $("#<%=this.txtUsersAssign.ClientID %>").val("");
                    $("#<%=trUsersAssign.ClientID %>").css("display", "none");
                }
            });
            $("#<%=chkOrg.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=chkOrgAssign.ClientID %>").attr("checked", "checked");
                    //$("#<%=chkStarterAssign.ClientID %>").attr("checked", "checked");
                    $("#<%=trOrg.ClientID %>").css("display", "");
                    $("#<%=trOrgAssign.ClientID %>").css("display", "");
                }
                else {
                    $("#<%=trOrg.ClientID %>").css("display", "none");
                    $("input[name='chkOrgArgs']").removeAttr("checked");
                }
            });
            $("#<%=chkOrgAssign.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=trOrgAssign.ClientID %>").css("display", "");
                }
                else {
                    $("#<%=trOrgAssign.ClientID %>").css("display", "none");
                    $("input[name='chkOrgArgsAssign']").removeAttr("checked");
                }
            });
            $("#<%=chkRole.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=chkRoleAssign.ClientID %>").attr("checked", "checked");
                    $("#<%=trRole.ClientID %>").css("display", "");
                    $("#<%=trRoleAssign.ClientID %>").css("display", "");
                }
                else {
                    $("#<%=trRole.ClientID %>").css("display", "none");
                    $("input[name='chkRoleArgs']").removeAttr("checked");
                }
            });
            $("#<%=chkRoleAssign.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=chkRoleAssign.ClientID %>").css("display", "");
                    $("#<%=trRoleAssign.ClientID %>").css("display", "");
                }
                else {
                    $("#<%=trRoleAssign.ClientID %>").css("display", "none");
                    $("input[name='chkRoleArgsAssign']").removeAttr("checked");
                }
            });
            $("#<%=chkRes.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=chkResAssign.ClientID %>").attr("checked", "checked");
                }
            });
            $("#<%=chkPssctl.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=chkPssctlAssign.ClientID %>").attr("checked", "checked");
                    $("#<%=orgPssor.ClientID%>").css("display", "");
                    $("#<%=chkOrg.ClientID %>").attr("checked", "checked");
                    $("#<%=orgPssorAssign.ClientID%>").css("display", "");
                    $("#<%=chkOrgAssign.ClientID %>").attr("checked", "checked");
                    //$("#<%=chkStarterAssign.ClientID %>").attr("checked", "checked");
                    $("#<%=trOrg.ClientID %>").css("display", "");
                    $("#<%=trOrgAssign.ClientID %>").css("display", "");
                }
                else {
                    $("#<%=orgPssor.ClientID%>").css("display", "none");
                    $("#<%=drdlPssor.ClientID%>").get(0).selectedIndex = 0;
                }
            });
            $("#<%=chkPssctlAssign.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=orgPssorAssign.ClientID%>").css("display", "");
                    $("#<%=chkOrgAssign.ClientID %>").attr("checked", "checked");
                    $("#<%=trOrgAssign.ClientID %>").css("display", "");
                }
                else {
                    $("#<%=orgPssorAssign.ClientID%>").css("display", "none");
                    $("#<%=drdlPssorAssign.ClientID%>").get(0).selectedIndex = 0;
                }
            });
            $("#<%=chkPssor.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=chkPssorAssign.ClientID %>").attr("checked", "checked");
                }
            });
            $("#<%=chkStarter.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=chkStarterAssign.ClientID %>").attr("checked", "checked");
                }
            });
            $("#<%=chkControl.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=orgType.ClientID%>").css("display", "");
                    $("#<%=orgTypeAssign.ClientID%>").css("display", "");
                    $("#<%=chkControlAssign.ClientID %>").attr("checked","checked");
                }
                else {
                    $("#<%=orgType.ClientID%>").css("display", "none");
                    $("#<%=drdlActivities.ClientID%>").get(0).selectedIndex = 0;
                }
            });
            $("#<%=chkControlAssign.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=orgTypeAssign.ClientID%>").css("display", "");
                }
                else {
                    $("#<%=orgTypeAssign.ClientID%>").css("display", "none");
                    $("#<%=drdlActivitiesAssign.ClientID%>").get(0).selectedIndex = 0;
                }
            });
            $("#<%=chkToOwner.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=ownerType.ClientID%>").css("display", "");
                    $("#<%=ownerAssignType.ClientID%>").css("display", "");
                    $("#<%=chkToOwnerAssign.ClientID %>").attr("checked", true);
                }
                else {
                    $("#<%=ownerType.ClientID%>").css("display", "none");
                }
            });

            $("#<%=chkToOwnerAssign.ClientID %>").click(function () {
                if (this.checked == true) {
                    $("#<%=ownerAssignType.ClientID%>").css("display", "");
                }
                else {
                    $("#<%=ownerAssignType.ClientID%>").css("display", "none");
                }
            });
        });
    function openUserSelector(inputId){
        var h = 450;
	    var w = 700;
	    var iTop = (window.screen.availHeight-30-h)/2;    
	    var iLeft = (window.screen.availWidth-10-w)/2; 
	    window.open('<%=AppPath%>contrib/security/pages/PopupUserPicker.aspx?inputid='+ inputId, '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');	
	    return false;
    }
    //-->
    </script>
</asp:Content>
