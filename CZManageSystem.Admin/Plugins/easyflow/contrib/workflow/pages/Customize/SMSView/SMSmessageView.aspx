<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_Customize_SMSView_SMSmessageView" Codebehind="SMSmessageView.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
<style type="text/css">
#smsTableView td:hover{background-color:#F8FBFD;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3>
            <span>短信信息查看</span>
        </h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>查询短信</h4>
            <button onclick="return showContent(this,'queryInput');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable" id="queryInput">
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-bottom:16px;">
                <tr>
                    <th style="width:108px; text-align:right;">起始时间：</th>
                    <td >
                        <bw:DateTimePicker ID="dtpBeginDT" runat="server" IsRequired="false" Width="120px" ExpressionValidatorText="*" ExpressionErrorMessage="起始时间范围最小时间日期格式错误." />
                         &nbsp;
                         到
                         &nbsp;&nbsp;
                         <bw:DateTimePicker ID="dtpEndDT" runat="server" IsRequired="false" Width="120px" ExpressionValidatorText="*" ExpressionErrorMessage="起始时间范围最大时间日期格式错误." />
                    </td>
                </tr>
                <tr>
                    <th style="text-align:right;">接收人：</th>
                    <td>
                        <asp:TextBox ID="txtReceiver" runat="server" MaxLength="20" CssClass="inputbox" ToolTip="按接收人进行筛选"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th style="text-align:right;">工单受理号：</th>
                    <td>
                        <asp:TextBox ID="txtSheetID" runat="server" MaxLength="20" CssClass="inputbox" ToolTip="按工单受理号进行筛选"></asp:TextBox></td>
                </tr>
                <tr>
                  <td colspan="4" align="center">
                        <asp:Button ID="btnQuery" runat="server" CssClass="btn_query" Text="搜索" onclick="btnQuery_Click" />
                  </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="validationSummary1" ShowSummary="false" ShowMessageBox="true" runat="server" />
        </div>
        <div class="showControl">
            <h4>短信列表</h4>
        </div>
        <div class="dataTable" id="smsTable">
            <table class="tblGrayClass" id="smsTableView" 
                cellspacing="1"  cellpadding="0"  style="text-align:center;  margin:0px; padding:0px;">
                <tr id="tr1" style="background-color:#F4F4F4" runat="server">
                    <th align="center" style=" font-weight:bold; width:90px;  " >
                        受理号
                    </th>
                    <th align="center" style=" font-weight:bold;width:90px;  " >
                        发送人
                    </th>
                    <th align="center" style=" font-weight:bold;" >
                        发送内容
                    </th>
                    <th align="center" style=" font-weight:bold;width:70px" >
                        发送状态
                    </th>
                   <th align="center" style=" font-weight:bold;width:70px" >
                        发送时间
                    </th>
                    <th align="center" style=" font-weight:bold;width:90px " >
                        接收人
                    </th>
                    <th align="center" style=" font-weight:bold; width:70px; " >
                        回复内容
                    </th>
                    <th align="center" style=" font-weight:bold;width:70px " >
                        回复状态
                    </th>
                   <th align="center" style=" font-weight:bold;width:70px " >
                        回复时间
                    </th>
                </tr>
                <asp:Repeater ID="rptResult" runat="server" >
                    <ItemTemplate>
                    <tr title='<%# Eval("Content") %>'>
                        <td><%# Eval("SheetID") %></td>
                        <td><%# Eval("Sender")%></td>
                        <td style="text-align:left;"><%# SubString(Eval("Content"))%></td>
                        <td><%# Eval("SendStatus")%></td>
                        <td><%# Eval("CreatedTime")%></td>
                        <td><%# Eval("ReceiverName")%></td>
                        <td><%# Eval("ProcessContent")%></td>
                        <td><%# Eval("ProcessStatus")%></td>
                        <td><%# Eval("ReceivedTime")%></td>
                    </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                    <tr class="trClass" title='<%# Eval("Content") %>'>
                        <td><%# Eval("SheetID") %></td>
                        <td><%# Eval("Sender")%></td>
                        <td style="text-align:left;"><%# SubString(Eval("Content"))%></td>
                        <td><%# Eval("SendStatus")%></td>
                        <td><%# Eval("CreatedTime")%></td>
                        <td><%# Eval("ReceiverName")%></td>
                        <td><%# Eval("ProcessContent")%></td>
                        <td><%# Eval("ProcessStatus")%></td>
                        <td><%# Eval("ReceivedTime")%></td>
                    </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
            <div style="text-align:right;">
                <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                        Font-Size="9pt" ItemsPerPage="15" PagerStyle="NumericPages" BorderWidth="0px"
                        OnPageIndexChanged="listPager_PageIndexChanged" />
            </div>
        </div>
    </div>
</asp:Content>

