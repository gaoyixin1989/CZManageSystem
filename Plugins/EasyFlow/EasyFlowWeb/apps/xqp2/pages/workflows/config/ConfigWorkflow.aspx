<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="xqp2_contrib_workflow_pages_config_ConfigWorkflow" Codebehind="ConfigWorkflow.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="流程设置" runat="server" /></span></h3>
    </div>	
    <div class="btnControl">
        <div class="btnLeft">
            <input type="button" value="发单控制设置" class="btnNewwin" onclick="window.location.href='configCreation.aspx?name=<%=HttpUtility.UrlEncode(this.WorkflowName)%>';" />
            <input type="button" value="短信审批设置" class="btnNewwin" onclick="window.location.href='configSMSAudit.aspx?wid=<%=this.WorkflowId%>';" />
            <input type="button" value="自动处理设置" class="btnNewwin" onclick="window.location.href='ConfigWorkflowAuto.aspx?wid=<%=this.WorkflowId%>';" />
            <input type="button" value="提醒时间段设置" class="btnNewwin" onclick="window.location.href='ConfigRemindTime.aspx?eid=<%=this.WorkflowId%>&name=<%=Server.UrlEncode(this.WorkflowName) %>';" />
             <input type="button" value="智能提醒" class="btnNewwin" onclick="window.location.href='ConfigIntelligentRemind.aspx?wid=<%=this.WorkflowId%>';" />
             <input type="button" value="子流程设置" class="btnNewwin" onclick="window.location.href='ConfigWorkflowRelation.aspx?wid=<%=this.WorkflowId%>';" />
        </div>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>流程设置</h4>
            <button onclick="return showContent(this,'divSettings');" title="收缩"><span>折叠</span></button>
        </div>
        <div id="divSettings">
            <div style="padding-bottom:10px">
                说明：
                <ul style="list-style-type:decimal; padding-left:30px">
                    <li>提醒信息(短信及邮件)格式参数有：：&quot;#Title#&quot;(工单标题), &quot;#Creator#&quot;(发单人), &quot;#OperateType#&quot;(工单操作类型: 1
                        表示回退), &quot;#ActivityName#&quot;(流程活动步骤)，&quot;#PrevActors#&quot;(上一步骤处理人), 。</li>
                    <li>发单控制，发单数（每月最大发单数、每周最大发单数）值为 -1 或 0 时表示发单无限制.</li>
                    <li>未完成的最大发单数值为 -1 与 0 时，表示不限制未完成的发单数.</li>
                </ul>
            </div>
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
                <tr>
                    <th style="width:17%;">流程名称：</th>
                    <td colspan="5" style="padding:5px 0 5px 5px">
                        <asp:Literal ID="ltlWorkflowName" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <th>基本字段显示：</th>
                    <td colspan="5" style="padding:5px 0 5px 5px">
                        <asp:CheckBox ID="chkboxExpectFinishTime" runat="server" Text="期望完成时间" />
                        <asp:CheckBox ID="chkboxSecrecy" runat="server" Text="保密设置" />
                        <asp:CheckBox ID="chkboxUrgency" runat="server" Text="紧急程度" />
                        <asp:CheckBox ID="chkboxImportance" runat="server" Text="重要级别" />
                    </td>
                </tr>
                <tr>
                    <th>流程别名：</th>
                    <td style="width:33%">
                        <asp:TextBox ID="txtAlias" Text="" MaxLength="6" Width="50" runat="server"></asp:TextBox>
                    </td>
                    <th style="width:17%;">流程别名图片：</th>
                    <td>
                        <asp:HiddenField  ID="hiddenAliasImage" runat="server" />
                        <span id="aliasImg_holder" style="color:Gray"><%=this.AliasImage %></span>
                        <a onclick="openAliasImagePicker();" href="javascript:void(0);">选择</a>
                    </td>
                </tr>
                <tr>
                    <th>提醒最小任务数：</th>
                    <td style="width:33%">
                        <asp:TextBox ID="txtMinNotifyTaskCount" Text="-1" MaxLength="6" Width="50" runat="server"></asp:TextBox>
                        <span style="color:Red">*</span>
                        <asp:RequiredFieldValidator ID="rfvMinNotifyTaskCount" ControlToValidate="txtMinNotifyTaskCount" SetFocusOnError="true" runat="server" Display="None" ErrorMessage="必须填写提醒最小任务数."></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revMinNotifyTaskCount" ControlToValidate="txtMinNotifyTaskCount" runat="server" SetFocusOnError="true" Display="None" ValidationExpression="[-](\d)*" ErrorMessage="请填写提醒最小任务数的正确数字." />
                    </td>
                    <th style="width:17%;">未完成的最大发单数：</th>
                    <td>
                        <asp:TextBox ID="txtMaxUndone" Text="-1" MaxLength="6" runat="server" Width="50px" />
                    </td>
                </tr>
                <tr>
                <th>导出和打印：</th>
                <td colspan="3">
                <asp:CheckBox ID="chkPrint" runat="server" Text="打印表单" />&nbsp;
                <asp:CheckBox ID="chkExp" runat="server" Text="导出表单" />&nbsp;
                </td>
                </tr>
                <tr>
                    <th>打印次数：</th>
                    <td colspan="3">
                        <asp:TextBox ID="txtPrintCount" runat="server" Text="-1" Width="50"></asp:TextBox>
                        <div style="color:red; padding:5px">
                            - 此设置选 打印表单 时此设置才生效。只针对已经完成的工单，-1表示无限制次数。

                        </div>
                    </td>
                </tr>
                <tr>
                    <th>允许手机审批：</th>
                    <td colspan="3">
                        <asp:CheckBox ID="chkIsMobile" runat="server" Text="允许手机审批" />
                        <div style="color:red; padding:5px">
                            - 勾选后允许该流程在手机版的页面上审批。

                        </div>
                    </td>
                </tr>
                 <tr>
                    <th>处理时间短时间格式：</th>
                    <td colspan="3">
                        <asp:CheckBox ID="chkIsToShortDateString" runat="server" Text="启用短时间格式" />
                        <div style="color:red; padding:5px">
                            - 勾选后处理记录上的时间格式显示为 年-月-日。

                        </div>
                    </td>
                </tr>
                <tr>
                    <th style="width: 17%; text-align:right;">是否允许自动处理：</th>
                    <td style="padding: 5px 0 5px 5px" colspan="3">
                        <asp:CheckBox ID="chkboxIsAuto" runat="server" Text="允许超时自动处理" />
                        <span style="color:#247ecf;">(选中则表示允许工单在滞留超时系统自动处理)</span>
                    </td>
                </tr>
                <tr>
                    <th>所属部门：</th>
                    <td colspan="3">
                        <asp:TextBox ID="txtDepts" runat="server"  TextMode="MultiLine" Width="520px" Height="35px" onfocus="this.blur()"></asp:TextBox>
                        <input type='button' id="btnDepts" style='cursor: pointer; background: url(<%=AppPath%>App_Themes/gmcc/img/btnse01.jpg);border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;' />
                    </td>
                </tr>
                <tr>
                    <th>流程经理人：</th>
                    <td colspan="3">
                        <asp:TextBox ID="txtManager" runat="server"  TextMode="MultiLine" Width="520px" Height="35px" onfocus="this.blur()"></asp:TextBox>
                        <asp:HiddenField ID="hidManager" runat="server"></asp:HiddenField>
                        <input type='button' id="btnManager" style='cursor: pointer; background: url(<%=AppPath%>App_Themes/gmcc/img/btnse01.jpg);border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;' />
                    </td>
                </tr>
            </table>
            
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
                <tr>
                    <th style="width:17%;">工单自动生成的标题：</th>
                    <td colspan="5">
                        <asp:TextBox ID="txtWorkflowInstanceTitle" Width="92%" runat="server"></asp:TextBox>
                        <div style="color:#247ecf">
                            参数说明：(1)$workflow：流程名称；(2)$user：发单人姓名；(3)$dept：发单人所在部门；(4)$mobile:发单人手机号码；(5)$tel:发单人联系电话，如果有手机号码则为手机号码否则为固定电话号码；(6)$date：发单日期，如：20090802；(7)$datetime：发单时间，如：200908021022。
                        </div>
                    </td>
                </tr>
                <tr>
                    <th style="width:17%;">短信提醒格式：</th>
                    <td colspan="5">
                        <asp:TextBox ID="txtSmsNotifyFormat" TextMode="MultiLine" Width="92%" Height="50px"
                            runat="server"></asp:TextBox>
                        <span style="color: Red">*</span>
                        <asp:RequiredFieldValidator ID="rfbSmsNotifyFormat" ControlToValidate="txtSmsNotifyFormat"
                            SetFocusOnError="true" runat="server" Display="None" ErrorMessage="必须填写短信提醒格式."></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>邮件提醒格式：</th>
                    <td colspan="5">
                        <asp:TextBox ID="txtEmailNotifyFormat" TextMode="MultiLine" Width="92%" Height="50px"
                            runat="server"></asp:TextBox>
                        <span style="color: Red">*</span>
                        <asp:RequiredFieldValidator ID="rfvEmailNotifyFormat" ControlToValidate="txtEmailNotifyFormat"
                            SetFocusOnError="true" runat="server" Display="None" ErrorMessage="必须填写邮件提醒格式."></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>统计短信提醒格式：</th>
                    <td colspan="5">
                        <asp:TextBox ID="txtStatSmsNotifyFormat" TextMode="MultiLine" Width="92%" Height="50px"
                            runat="server"></asp:TextBox>
                        <span style="color: Red">*</span>
                        <asp:RequiredFieldValidator ID="rfvStatSmsNotifyFormat" ControlToValidate="txtStatSmsNotifyFormat"
                            SetFocusOnError="true" runat="server" Display="None" ErrorMessage="必须填写统计短信提醒格式."></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>统计邮件提醒格式：</th>
                    <td colspan="7">
                        <asp:TextBox ID="txtStatEmailNotifyFormat" TextMode="MultiLine" Width="92%" Height="50px"
                            runat="server"></asp:TextBox>
                        <span style="color: Red">*</span>
                        <asp:RequiredFieldValidator ID="rfvStatEmailNotifyFormat" ControlToValidate="txtStatEmailNotifyFormat"
                            SetFocusOnError="true" runat="server" Display="None" ErrorMessage="必须填写统计邮件提醒格式."></asp:RequiredFieldValidator>
                    </td>
                </tr>
                 <tr>
                    <th>普通单发起控制：</th>
                    <td style="width:16%;">
                        <asp:DropDownList ID="ddlCreationTypes" runat="server">
                            <asp:ListItem Value="">默认</asp:ListItem>
                            <asp:ListItem Value="dept">部门控制</asp:ListItem>
                            <asp:ListItem Value="room">科室控制</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <th style="width:17%;">每月最大发单数：</th>
                    <td style="width:16%;">
                        <asp:TextBox ID="txtMaxInMonth" Text="-1" MaxLength="6" runat="server" Width="30px" />
                    </td>
                    <th style="width:17%;">每周最大发单数：</th>
                    <td style="width:16%;">
                        <asp:TextBox ID="txtMaxInWeek" Text="-1" MaxLength="6" runat="server" Width="30px" />
                    </td>
                </tr>
                <tr>
                    <th>子站点地址[发起工单]：</th>
                    <td colspan="5">
                        <asp:Label ID="lblStartSite" runat="server" EnableViewState="false" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>
                        子站点地址[流程主页]：

                    </th>
                    <td colspan="5">
                        <asp:Label ID="lblWorkflowSite" runat="server" EnableViewState="false" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>子站点地址[待办列表]：</th>
                    <td colspan="5">
                        <asp:Label ID="lblTodoSite" runat="server" EnableViewState="false" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <p align="center" style="margin-top:10px">
                <asp:Button ID="btnSave" runat="server" CssClass="btn_sav" Text="保存" 
                    onclick="btnSave_Click" />
                <input type="button" value="返回" class="btnFWClass" onclick="document.location='../workflowDeploy.aspx';" />
            </p>
            <div>
                <asp:RegularExpressionValidator ID="rfvMaxUndone" ControlToValidate="txtMaxUndone" runat="server" SetFocusOnError="true" Display="None" ValidationExpression="[-](\d)*" ErrorMessage="请填写未完成的最大发单数的正确数字." />
                <asp:ValidationSummary ID="vsummary1" runat="server" ShowSummary="false" ShowMessageBox="true" />
            </div>
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
                    <%--<th>权限控制</th>--%>
                    <th>自定义控制</th>
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
    <script type="text/javascript">
    function previewAliasImage(path){
        if(path != ""){
            var alias = path.replace(".gif","").replace("flow_", "").toUpperCase();
            onChangeWorkflowAlias(alias);
        }
    }
    function openAliasImagePicker(){
        var inputId = "<%=this.hiddenAliasImage.ClientID%>";
        var inputValue = $("#"+inputId).val();
        var h = 400;
	    var w = 700;
	    var iTop = (window.screen.availHeight-30-h)/2;    
	    var iLeft = (window.screen.availWidth-10-w)/2; 
	    window.open('<%=AppPath%>contrib/workflow/pages/PopupAliasImages.aspx?inputid='+ inputId+'&inputValue='+inputValue, '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');	
	    return false;
    }
    $(function () {
        $("#<%=txtAlias.ClientID%>").keyup(function (event) {
            var val = $("#<%=txtAlias.ClientID%>").val();
            onChangeWorkflowAlias(val);
        });

        $("#btnDepts").click(function () {
            showDiv({ isorganization: 'False', tableName: 'bw_depts', text: 'dpfullname', value: 'dpfullname', fieldWhere: '' }, '<%=AppPath%>apps/pms/pages/GetMarkData.aspx', { hide: '<%=txtDepts.ClientID%>', text: '<%=txtDepts.ClientID%>' });
            $("img[alt='点击可以关闭']").attr("src", "<%=AppPath %>App_Themes/gmcc/img/close_1.jpg")
        });
        $("#btnManager").click(function () {
            var depts = $("#<%=txtDepts.ClientID %>").val()
            var where = ""
            for (var i = 0; i < depts.split(',').length; i++) {
                where += "or dpfullname like '" + depts.split(',')[i] + "%' "
            }
            if (depts.length > 0) {
                where = "dpid in (select dpid from bw_depts where " + where.substring(3, where.length) + ")";
                showDiv({ isorganization: 'False', tableName: 'bw_users', text: 'realname', value: 'userid', fieldWhere: where }, '<%=AppPath%>apps/pms/pages/GetMarkData.aspx', { hide: '<%=hidManager.ClientID%>', text: '<%=txtManager.ClientID%>' });
                $("img[alt='点击可以关闭']").attr("src", "<%=AppPath %>App_Themes/gmcc/img/close_1.jpg")
            }
            else
                alert("请先选择部门");

        });
    });
    function onChangeWorkflowAlias(alias){
        alias = alias.replace(" ","");
        $("#<%=txtAlias.ClientID%>").val(alias.toUpperCase());
        
        var starturl = $("#<%=lblStartSite.ClientID%>").text();
        starturl = starturl.substring(0, starturl.indexOf("ssoproxy/start/",0));        
        $("#<%=lblStartSite.ClientID%>").text(starturl + "ssoproxy/start/" + alias.toLowerCase() + ".ashx");        
        starturl = $("#<%=lblWorkflowSite.ClientID%>").text();
        starturl = starturl.substring(0, starturl.indexOf("ssoproxy/index/",0));        
        $("#<%=lblWorkflowSite.ClientID%>").text(starturl + "ssoproxy/index/" + alias.toLowerCase() + ".ashx");
        
        $("#aliasImg_holder").empty();
        if(alias.length > 0){
            $("#aliasImg_holder").prepend("<img src='<%=AppPath%>contrib/workflow/res/groups/flow_"+ alias +".gif' />");
        }
        return true;
    }
    </script>
</asp:Content>
