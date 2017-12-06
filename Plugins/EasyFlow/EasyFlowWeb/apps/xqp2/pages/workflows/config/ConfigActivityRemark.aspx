<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_ConfigActivityRemark" Codebehind="ConfigActivityRemark.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<div class="dataList">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" Text="步骤处理意见设置" runat="server" /></span></h3>
    </div>
    <div class="showControl" id="divActivityReviewHolder" runat="server">
            <h4>流程步骤处理意见设置</h4>
            <button onclick="return showContent(this,'divActivityReview');" title="收缩"><span>折叠</span></button>
        </div>
        <div id="divActivityReview">
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
                <tr>
                    <th style="width:17%;font-weight:bold; text-align:right">流程步骤名称</th>
                    <th style="font-weight:bold">处理意见设置</th>
                </tr>
                <asp:Literal ID="ltlActivity" runat="server"></asp:Literal>
            </table>
        </div>
    </div>
</asp:Content>

