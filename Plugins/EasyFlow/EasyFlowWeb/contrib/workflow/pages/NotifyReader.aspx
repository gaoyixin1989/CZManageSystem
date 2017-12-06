<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_NotifyReader" Title="抄送消息" Codebehind="NotifyReader.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="titleContent">
        <h3>
            <span>抄送</span></h3>
    </div>
    <div class="dataList">
        <div id="dataDiv1">
            <div class="dataTable" id="dataTable1">
                        <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 6px;">
                            <tr>
                                <th style="width: 15%;">
                                    当前工单：
                                </th>
                                <td>
                                    <asp:Label ID="lbWrokflowTitle" runat="server"></asp:Label>
                                </td>
                            </tr>                    
                            <tr id="trBeRead" runat="server" style="display: ">
                                <th style="width: 15%;">
                                    短信/邮件内容：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtBeReadNotifyFormat" TextMode="MultiLine" Width="80%" Height="50px"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr> 
                            <tr>
                                <th style="width: 15%;">
                                    抄送人列表：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtUsersAssign" TextMode="MultiLine" Width="80%" Height="40px"
                                        runat="server"></asp:TextBox>
                                    <a href="javascript:void(0);" onclick="javascrpt:return openUserSelector('<%=txtUsersAssign.ClientID%>');">
                                        选择用户</a>
                                </td>
                            </tr>        
                            <tr>
                                <th style="width: 15%;">
                                    抄送方式：
                                </th>
                                <td>
                                        <input type="checkbox" id="chkBeRead" checked="checked" name="chkBeRead" runat="server" />待阅
                                        <input type="checkbox" id="chkSms" name="chkSms" runat="server" />短信                                    
                                        <input type="checkbox" id="chkEmail" name="chkEmail" runat="server" />邮件              
                                </td>
                            </tr>                           
                        </table>
                        <div style="text-align:center;margin-top:10px;">
                            <asp:Button ID="btnSend" runat="server" CssClass="btn_sav" Text="提交" OnClientClick="return checkUser();"
                                        OnClick="btnSend_Click" />
                                    <input type="button" value="返回待办" class="btnClass2m" onclick="location = '<%=AppPath%>contrib/workflow/pages/default.aspx'" />
                        </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
    <!--//
    function openUserSelector(inputId){
    var h = 450;
    var w = 700;
    var iTop = (window.screen.availHeight-30-h)/2;    
    var iLeft = (window.screen.availWidth-10-w)/2; 
    window.open('<%=AppPath%>contrib/security/pages/PopupUserPicker.aspx?inputid='+ inputId, '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');	
    return false;
    }
    
    function checkUser()
    {
        var user = document.getElementById("<%=txtUsersAssign.ClientID %>");
        
        if (user.value == "")
        {
            alert('请选择抄送人');
            return false;
        }
        return true;
    }

    //-->
    </script>
</asp:Content>
