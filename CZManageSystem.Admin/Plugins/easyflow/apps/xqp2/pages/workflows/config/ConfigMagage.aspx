<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ConfigMagage" Codebehind="ConfigMagage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="处理人设置" runat="server" /></span></h3>
    </div>
    <div class="dataList">
        <div class="showControl" id="divActivityReviewHolder" runat="server">
            <h4>流程步骤处理人设置</h4>
            <button onclick="return showContent(this,'divActivityReview');" title="收缩"><span>折叠</span></button>
        </div>
        <div id="divActivityReview">
            <div>
                <ul style="color:#247ECF;list-style-type:decimal; padding-left:30px">
                    <li>说明：处理人员数限制：指限制处理人员的选择数，值只能为数字。值为 -1 时，表示不限制处理人数(任意选择)。</li>
                </ul>
            </div>
                        <div >
                <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
                    <tr>
                        <th style="width:17%; text-align:right">流程名称：</th>
                        <td style="padding:5px 0 5px 5px">
                            <asp:Literal ID="ltlWorkflowName" runat="server" ></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
                <tr>
                    <th style="width:27%;font-weight:bold; text-align:right; padding-right:15px;">流程步骤名称</th>
                    <th style="font-weight:bold;text-align:left;padding-left:150px;">处理人设置</th>
                </tr>
                <asp:Repeater ID="rptActivities" runat="server" OnItemDataBound="rptActivityList_ItemDataBound">
                    <ItemTemplate>
                         <tr>
                            <th style="text-align:right;padding-right:15px;"><%# Eval("ActivityName") %>：</th>
                            <td style="padding:5px 0 5px 5px">
                                <div style="margin-bottom:5px;padding-right:30px;">
                                    <asp:CheckBox ID="chkboxReviewValidateType" runat="server" Text="是否全选" />　
                                    处理人人数限制：<asp:TextBox ID="txtReviewActorCount" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                                    <asp:HiddenField ID="hiddenProfileID" runat="server" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField ID="hiddenActivityName" runat="server" Value='<%# Eval("ActivityName") %>' />
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                         <tr class="trClass">
                            <th style="text-align:right;padding-right:15px;"><%# Eval("ActivityName") %>：</th>
                            <td style="padding:5px 0 5px 5px">
                                <div style="margin-bottom:5px;padding-right:30px;">
                                    <asp:CheckBox ID="chkboxReviewValidateType" runat="server" Text="是否全选" />　
                                    处理人人数限制：<asp:TextBox ID="txtReviewActorCount" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                                    <asp:HiddenField ID="hiddenProfileID" runat="server" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField ID="hiddenActivityName" runat="server" Value='<%# Eval("ActivityName") %>' />
                                </div>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div style="text-align:center; margin-top:10px">
            <asp:Button ID="btnSave" CssClass="btn_sav" runat="server" Text="保存" onclick="btnSave_Click" /> 
            <input type="button" class="btnReturnClass" style="margin-left:10px" value="返回" onclick="window.location='<%=AppPath%>apps/xqp2/pages/workflows/WorkflowDeploy.aspx';" />
        </div>
     </div>
</asp:Content>

