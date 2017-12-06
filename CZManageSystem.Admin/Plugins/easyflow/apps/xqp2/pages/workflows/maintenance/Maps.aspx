<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/plugins/easyflow/masters/Default.master" Inherits="apps_xqp2_pages_workflows_maintenance_Maps" Codebehind="Maps.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="titleContent">
        <h3>
            <span>流程辅助功能</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>
                流程名修改</h4>
            <button onclick="return showContent(this,'tb1');" title="收缩">
                <span>折叠</span></button>
        </div>
        <table id="tb1" class="tblGrayClass grayBackTable" style="word-break: break-all" cellspacing="1" cellpadding="4" border="0">
            <tr align="center">
                <td></td>
                <td rowspan="2"></td>
                <td><a class="ico_add">工单内容修改</a><input type="button" value="工单内容修改" class="btn_add_user_01" /></td>
                <td rowspan="2"></td>
                <td><a class="ico_add">工单内容修改</a><input type="button" value="工单内容修改" class="btn_add_user_01" /></td>
                
                <td></td>
            </tr>
            <tr align="center">
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr align="center">
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </div>
</asp:Content>
