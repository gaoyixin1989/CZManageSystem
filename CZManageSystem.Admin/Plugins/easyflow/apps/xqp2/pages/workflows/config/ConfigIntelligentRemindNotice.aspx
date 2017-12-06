<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ConfigIntelligentRemindNotice" Codebehind="ConfigIntelligentRemindNotice.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="消息抄送设置" runat="server" /></span></h3>
    </div>
    <div class="dataList">
        <div id="divSettings">
            <div style="padding-bottom:10px">
                说明：

                <ul style="list-style-type:decimal; padding-left:30px">
                    <li>输入用户时，只输入用户名(字符、数字以及下划线的组合)，各用户名以逗号隔开(&quot;,&quot;)；</li>
                    <li>消息默认发给当前步骤的处理人，此页面可设置在发送给当前步骤处理人的同时再将消息发送给其他人或当前步骤处理人的领导或同事。</li>
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
                    <th>消息抄送人控制：</th>
                    <td style="padding:5px 0 5px 0">
                        <div style="margin-left:10px" id="divControlTypes" runat="server">
                            <span style="padding-left:5px"><input type="checkbox" id="chkUsers" name="chkUsers" runat="server"/><label for="<%=chkUsers.ClientID %>">用户控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkOrg" name="chkOrg" runat="server" /><label for="<%=chkOrg.ClientID%>">组织控制</label></span>
                            <span style="padding-left:5px; display:none"><input type="checkbox" id="chkStarter" name="chkStarter" runat="server" /><label for="<%=chkStarter.ClientID %>">发起人</label></span>
                        </div>
                    </td>
                </tr>
                <tr id="trUsers" runat="server" style="padding:5px 0 5px 0;display:none">
                    <th>用户设置：</th>
                    <td>
                        <asp:TextBox ID="txtUsers" TextMode="MultiLine" Width="520px" Height="70px" runat="server"></asp:TextBox>
                        <a href="javascript:void(0);" onclick="javascrpt:return openUserSelector2('<%=txtUsers.ClientID%>');">
                        选择用户</a>
                    </td>
                </tr>
                <tr id="trOrg" runat="server" style="display:none">
                    <th>组织设置：</th>
                    <td>
                        <asp:Literal ID="ltlOrg" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
            <p align="center" style="margin-top:10px">
                <asp:Button ID="btnSave" runat="server" CssClass="btn_sav" Text="保存" 
                    onclick="btnSave_Click" />
                <input type="button" value="返回" class="btnFWClass" onclick="window.location='ConfigIntelligentRemind.aspx?wid=<%=WorkflowId %>';" />
            </p>
            <div>
                <asp:ValidationSummary ID="vsummary1" runat="server" ShowSummary="false" ShowMessageBox="true" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
    <!--//
        $(function() {
            $("#<%=chkUsers.ClientID %>").click(function() {
                if (this.checked == true) {
                    $("#<%=trUsers.ClientID %>").css("display", "");
                }
                else {
                    $("#<%=this.txtUsers.ClientID %>").val("");
                    $("#<%=trUsers.ClientID %>").css("display", "none");
                }
            });
            $("#<%=chkOrg.ClientID %>").click(function() {
                if (this.checked == true) {
                    $("#<%=trOrg.ClientID %>").css("display", "");
                }
                else {
                    $("#<%=trOrg.ClientID %>").css("display", "none");
                    $("input[name='chkOrgArgs']").removeAttr("checked");
                }
            });
        });
    function openUserSelector(inputId){
//        var h = 500;
//	    var w = 800;
//	    var iTop = (window.screen.availHeight-30-h)/2;    
//	    var iLeft = (window.screen.availWidth-10-w)/2; 
//	    window.open('<%=AppPath%>contrib/security/pages/PopupUserPicker.aspx?inputid='+ inputId, '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');
        //	    return false;
        var h = 500;
        var w = 800;
        var user = "";
        if (state == 1) {
            user = $("#ctl00_cphBody_txtUsers").val(); //下行用户
        }
        else if (state == 2) {
            user = $("#ctl00_cphBody_txtUsersAssign").val(); //平转用户
        }
        else {
            //字段控制
            user = $("#" + inputId).val();
        }

        var iTop = (window.screen.availHeight - 30 - h) / 2;
        var iLeft = (window.screen.availWidth - 10 - w) / 2;
        window.open('<%=AppPath%>contrib/security/pages/PopupUserPicker.aspx?username=' + user + '&&inputid=' + inputId, '', 'height=' + h + ', width=' + w + ', top=' + iTop + ', left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');
        return false;
	}

	function openUserSelector2(inputId) {
	    var h = 500;
	    var w = 800;
	    var user = "";
//	    if (state == 1) {
//	        user = $("#ctl00_cphBody_txtUsers").val(); //下行用户
//	    }
//	    else if (state == 2) {
//	        user = $("#ctl00_cphBody_txtUsersAssign").val(); //平转用户
//	    }
//	    else
	     user = $("#" + inputId).val(); 
	    var iTop = (window.screen.availHeight - 30 - h) / 2;
	    var iLeft = (window.screen.availWidth - 10 - w) / 2;
	    window.open('<%=AppPath%>contrib/security/pages/PopupUserPicker.aspx?username='+user+'&&inputid=' + inputId,'', 'height=' + h + ', width=' + w + ', top=' + iTop + ', left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');
	    return false;
	}
    //-->
    </script>
</asp:Content>

