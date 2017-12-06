<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_config_ConfigWorkflow" Codebehind="ConfigWorkflow.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="流程设置" runat="server" /></asp:Literal></span></h3>
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
                    <th>权限控制</th>
                    <th>以前处理人</th>
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
                            <td><asp:Literal ID="ltlItemProcessor" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemStarter" runat="server" /></td>
                            <td>
                                <a href="configActivity.aspx?wfid=<%# Eval("WorkflowId") %>&aid=<%# Eval("ActivityId") %>" class="ico_edit">设置</a>
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
                            <td><asp:Literal ID="ltlItemProcessor" runat="server" /></td>
                            <td><asp:Literal ID="ltlItemStarter" runat="server" /></td>
                            <td>
                                <a href="configActivity.aspx?wfid=<%# Eval("WorkflowId") %>&aid=<%# Eval("ActivityId") %>" class="ico_edit">设置</a>
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
        var h = 250;
        var w = 530;
        var iTop = (window.screen.availHeight-30-h)/2;    
        var iLeft = (window.screen.availWidth-10-w)/2; 
        window.open('../PopupAliasImages.aspx?inputid='+ inputId+'&inputValue='+inputValue, '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');	
        return false;
    }
    $(function(){
        $("#<%=txtAlias.ClientID%>").keyup(function(event){
            var val = $("#<%=txtAlias.ClientID%>").val();
            onChangeWorkflowAlias(val);
        });  
    });
    function onChangeWorkflowAlias(alias){
        alias = alias.replace(" ","");
        $("#<%=txtAlias.ClientID%>").val(alias);
        $("#aliasImg_holder").empty();
        if(alias.length > 0){
            $("#aliasImg_holder").prepend("<img src='<%=AppPath%>contrib/workflow/res/groups/flow_"+ alias +".gif' />");
        }
        return true;
    }
    </script>
</asp:Content>
