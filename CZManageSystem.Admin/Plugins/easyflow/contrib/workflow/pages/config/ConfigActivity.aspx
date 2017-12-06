<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_config_ConfigActivity" Codebehind="ConfigActivity.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="任务分派设置" runat="server" /></asp:Literal></span></h3>
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
                    <th>下行分派控制类型：</th>
                    <td style="padding:5px 0 5px 0">
                        <div style="margin-left:10px" id="divControlTypes" runat="server">
                            <span><input type="checkbox" id="chkField" name="chkField" runat="server" /><label for="<%=chkField.ClientID %>">字段控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkUsers" name="chkUsers" runat="server"/><label for="<%=chkUsers.ClientID %>">用户控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkOrg" name="chkOrg" runat="server" /><label for="<%=chkOrg.ClientID%>">组织控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkRes" name="chkRes" runat="server"/><label for="<%=chkRes.ClientID %>">权限控制</label></span>
                            <span style="padding-left:5px"><input type="checkbox" id="chkPssor" name="chkPssor" runat="server" /><label for="<%=chkPssor.ClientID %>">以前处理人</label></span>
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
                            <span style="padding-left:5px"><input type="checkbox" id="chkPssorAssign" name="chkPssorAssign" runat="server" /><label for="<%=chkPssorAssign.ClientID %>">以前处理人</label></span>
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
                <tr>
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
                <tr>
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
    $(function(){
        $("#<%=chkField.ClientID %>").click(function(){
            if(this.checked == true){
                $("#<%=trFields.ClientID %>").css("display", "");
            }
            else{
                $("#<%=trFields.ClientID %>").css("display", "none");
            }
        });
        $("#<%=chkUsers.ClientID %>").click(function(){
            if(this.checked == true){
                $("#<%=chkUsersAssign.ClientID %>").attr("checked", "checked");
                $("#<%=trUsers.ClientID %>").css("display", "");
                $("#<%=trUsersAssign.ClientID %>").css("display", "");
            }
            else{
                $("#<%=this.txtUsers.ClientID %>").val("");
                $("#<%=trUsers.ClientID %>").css("display", "none");
            }
        });
        $("#<%=chkUsersAssign.ClientID %>").click(function(){
            if(this.checked == true){
                $("#<%=trUsersAssign.ClientID %>").css("display", "");
            }
            else{
                $("#<%=this.txtUsersAssign.ClientID %>").val("");
                $("#<%=trUsersAssign.ClientID %>").css("display", "none");
            }
        });
        $("#<%=chkOrg.ClientID %>").click(function(){
            if(this.checked == true){
                $("#<%=chkOrgAssign.ClientID %>").attr("checked", "checked");
                $("#<%=chkStarterAssign.ClientID %>").attr("checked", "checked");
                $("#<%=trOrg.ClientID %>").css("display", "");
                $("#<%=trOrgAssign.ClientID %>").css("display", "");
            }
            else{
                $("#<%=trOrg.ClientID %>").css("display", "none");
                $("input[name='chkOrgArgs']").removeAttr("checked");
            }
        });
        $("#<%=chkOrgAssign.ClientID %>").click(function(){
            if(this.checked == true){
                $("#<%=trOrgAssign.ClientID %>").css("display", "");
            }
            else{
                $("#<%=trOrgAssign.ClientID %>").css("display", "none");
                $("input[name='chkOrgArgsAssign']").removeAttr("checked");
            }
        });
        $("#<%=chkRes.ClientID %>").click(function(){
            if(this.checked == true){
                $("#<%=chkResAssign.ClientID %>").attr("checked", "checked");
            }
        });
        $("#<%=chkPssor.ClientID %>").click(function(){
            if(this.checked == true){
                $("#<%=chkPssorAssign.ClientID %>").attr("checked", "checked");
            }
        });
        $("#<%=chkStarter.ClientID %>").click(function(){
            if(this.checked == true){
                $("#<%=chkStarterAssign.ClientID %>").attr("checked", "checked");
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
