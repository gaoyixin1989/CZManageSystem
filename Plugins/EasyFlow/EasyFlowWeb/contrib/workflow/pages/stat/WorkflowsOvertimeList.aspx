<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_stat_WorkflowsOvertimeList" Codebehind="WorkflowsOvertimeList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" runat="server"></asp:Literal>超时工单列表</span></h3>
        <div class="rightSite">
            <input type="button" value="返 回" id="btnStart" onclick="javascript:history.go(-1);" class="btn2" />
        </div>
    </div>
    
    <div class="btnControl">
        <div class="btnRight">
            <input type="button" value="返 回" id="btnStart" onclick="javascript:history.go(-1);" class="btnFW" />
        </div>
    </div>
    <div class="dataList" id="divResults">		
		<div id="dataDiv1">
			<div class="dataTable" id="dataTable1">
				<table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align:center;">
		            <tr>
                        <th width="25%">标题</th>
                        <th width="10%">受理号</th>
                        <th width="10%">发起人</th>
                        <th width="13%">发起时间</th>
                        <th width="13%">期望完成时间</th>
			        </tr>
				    <asp:Repeater ID="listResults" runat="server">
				        <ItemTemplate>
    				        <tr style="text-align:center;">
    				            <td style="text-align:left;">
    				                <a href='../WorkflowView.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'><%# Eval("工单标题") %></a>
    				            </td>
    				            <td><%# Eval("工单号")%></td>
    				            <td><%# Eval("发起人")%></td>
    				            <td><%# Eval("发起时间", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
    				            <td><%# Eval("期望完成时间", "{0:yyyy-MM-dd}")%></td>
    				        </tr>				            
				        </ItemTemplate>
				        <AlternatingItemTemplate>
    				        <tr class="trClass" style="text-align:center;">
                                  <td style="text-align:left;">
    				                <a href='../WorkflowView.aspx?wiid=<%# Eval("WorkflowInstanceId") %>'><%# Eval("工单标题") %></a>
    				            </td>
    				            <td><%# Eval("工单号")%></td>
    				            <td><%# Eval("发起人")%></td>
    				            <td><%# Eval("发起时间", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
    				            <td><%# Eval("期望完成时间", "{0:yyyy-MM-dd}")%></td>
    				        </tr>
				        </AlternatingItemTemplate>
				    </asp:Repeater>
				</table>
                <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
                    <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                        Font-Size="9pt" ItemsPerPage="20" PagerStyle="NumericPages" BorderWidth="0px"
                        OnPageIndexChanged="listPager_PageIndexChanged" />
                </div>
		    </div>
		 </div>
	</div>
</asp:Content>

