<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ConfigReview" Title="抄送设置" Codebehind="ConfigReview.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="抄送设置" runat="server" /></span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>流程抄送设置</h4>
        </div>
        <div id="divWorkflowReview">
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
                <tr>
                    <th style="width:17%; text-align:right">流程名称：</th>
                    <td style="padding:5px 0 5px 5px">
                        <asp:Literal ID="ltlWorkflowName" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <th style="text-align:right">是否启用抄送功能：</th>
                    <td style="padding:5px 0 5px 5px">
                        <asp:CheckBox ID="chkboxIsRevew" runat="server" Text="启用抄送功能" />
                    </td>
                </tr>
                <tr>
                    <th style="text-align:right">是否使用旧有方式抄送：</th>
                    <td style="padding:5px 0 5px 5px">
                        <asp:CheckBox ID="chkboxIsClassicType" runat="server" Text="使用旧有方式抄送" />
                        <span style="color:#247ECF;">（旧有方式：由处理人自行选择抄送人）</span>
                    </td>
                </tr>
                <tr>
                    <th style="text-align:right">抄送消息醒格式：</th>
                    <td>
                        <asp:TextBox ID="txtReviewMessage" TextMode="MultiLine" Width="85%" Height="50px" runat="server"></asp:TextBox>
                        <div>参数： (1) #title#：工单标题。 (2) #from#: 消息发送人。</div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="showControl" id="divActivityReviewHolder" runat="server">
            <h4>流程步骤抄送人设置</h4>
            <button onclick="return showContent(this,'divActivityReview');" title="收缩"><span>折叠</span></button>
        </div>
        <div id="divActivityReview">
            <div>
                 说明：
                <ul style="color:#247ECF;list-style-type:decimal; padding-left:30px">
                    <li>抄送人员数限制：指限制抄送人员的选择数，值只能为数字。值为 -1 时，表示不限制抄送人数(任意选择)。</li>
                </ul>
            </div>
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
                <tr>
                    <th style="width:17%;font-weight:bold; text-align:right">流程步骤名称</th>
                    <th style="font-weight:bold" colspan="2">抄送人设置</th>
                </tr>
                <asp:Repeater ID="rptActivities" runat="server" OnItemDataBound="rptActivityList_ItemDataBound">
                    <ItemTemplate>
                         <tr>
                         <th style="text-align:right"><%# Eval("ActivityName") %>：</th> 
                         <td>
                          <table class="tblGrayClass grayBackTable" cellpadding="1" cellspacing="1" style="width:100%; margin:0 0 0 0">
                          <tr>
                          <th style="text-align:right">选择抄送人</th>
                            <td style="padding:5px 0 5px 5px">
                                <div style="margin-bottom:5px">
                                    <asp:CheckBox ID="chkboxActivityIsReview" runat="server" Text="启用抄送功能" />
                                    <asp:CheckBox ID="chkActivity" runat="server" Text="启用组织控制" onclick="checktr(this)" />
                                    <asp:CheckBox ID="chkboxReviewValidateType" runat="server" Text="是否全选" />　
                                    抄送人数限制：<asp:TextBox ID="txtReviewActorCount" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                                    <asp:HiddenField ID="hiddenProfileID" runat="server" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField ID="hiddenActivityName" runat="server" Value='<%# Eval("ActivityName") %>' />
                                </div>
                                <div>
                                    <asp:TextBox ID="txtReviewActors" TextMode="MultiLine" Width="85%" Height="32px" runat="server"></asp:TextBox>
                                    <a href="javascript:void(0);" id="linkPickActors" runat="server">选择抄送人</a>
                                </div>
                            </td>
                        </tr>
                        <tr id="trOrgAct" runat="server" style="display:none;">
                            <th style="text-align:right">选择步骤</th>
                                <td>
                                <asp:DropDownList ID="ddlActivity" runat="server"  ToolTip="不选择则默认为当前步骤"></asp:DropDownList><span style="color:Red">不选择则默认为当前步骤设置</span>
                            </td>
                        </tr>
                        <tr id="trOrg" runat="server" style="display:none;">
                            <th style="text-align:right">选择组织设置</th>
                                <td>
                                    <asp:Literal ID="ltlOrg" runat="server"></asp:Literal>
                            </td>
                        </tr>
                          </table>
                         </td>
                            </tr>
                    </ItemTemplate>
                    <%--<AlternatingItemTemplate>
                          <tr>
                         <th style="text-align:right" rowspan="3"><%# Eval("ActivityName") %>：</th> 
                            <th style="text-align:right">选择抄送人</th>
                            <td style="padding:5px 0 5px 5px">
                                <div style="margin-bottom:5px">
                                    <asp:CheckBox ID="chkboxActivityIsReview" runat="server" Text="启用抄送功能" />
                                    <asp:CheckBox ID="chkActivity" runat="server" Text="启用组织控制" onclick="checktr(this)" />
                                    <asp:CheckBox ID="chkboxReviewValidateType" runat="server" Text="是否全选" />　
                                    抄送人数限制：<asp:TextBox ID="txtReviewActorCount" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                                    <asp:HiddenField ID="hiddenProfileID" runat="server" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField ID="hiddenActivityName" runat="server" Value='<%# Eval("ActivityName") %>' />
                                </div>
                                <div>
                                    <asp:TextBox ID="txtReviewActors" TextMode="MultiLine" Width="85%" Height="32px" runat="server"></asp:TextBox>
                                    <a href="javascript:void(0);" id="linkPickActors" runat="server">选择抄送人</a>
                                </div>
                            </td>
                        </tr>
                        <tr id="trOrgAct" runat="server" style="display:none;">
                            <th style="text-align:right">选择步骤</th>
                                <td>
                                <asp:DropDownList ID="ddlActivity" runat="server"  ToolTip="不选择则默认为当前步骤"></asp:DropDownList><span style="color:Red">不选择则默认为当前步骤设置</span>
                            </td>
                        </tr>
                        <tr id="trOrg" runat="server" style="display:none;">
                            <th style="text-align:right">选择组织设置</th>
                                <td>
                                    <asp:Literal ID="ltlOrg" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>--%>
                </asp:Repeater>
            </table>
        </div>
        <div style="text-align:center; margin-top:10px">
            <asp:Button ID="btnSave" CssClass="btn_sav" runat="server" Text="保存" onclick="btnSave_Click" /> 
            <input type="button" class="btnReturnClass" style="margin-left:10px" value="返回" onclick="window.location='<%=AppPath%>apps/xqp2/pages/workflows/WorkflowDeploy.aspx';" />
        </div>
     </div>
     <script type="text/javascript" language="javascript">
     <!--//
      function openUserSelector(inputId){
        var h = 450;
	    var w = 700;
	    var iTop = (window.screen.availHeight-30-h)/2;    
	    var iLeft = (window.screen.availWidth-10-w)/2; 
	    window.open('<%=AppPath%>contrib/security/pages/PopupUserPicker.aspx?inputid='+ inputId, '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');	
	    return false;
	}
	function checktr(obj) {
	    var idtop = obj.id.replace("chkActivity", "trOrg")
	    if (obj.checked)
	        $("#" + idtop).css("display", "");
            else
                $("#" + idtop).css("display", "none");
    }
     $(function(){
//         $("#<%=chkboxIsRevew.ClientID%>").click(function(){
//            var isChecked = this.checked;
//            $("input[name $='$chkboxActivityIsReview']").each(function(){
//                this.checked = isChecked;
//            });
//         });
     });
     //-->
     </script>
</asp:Content>
