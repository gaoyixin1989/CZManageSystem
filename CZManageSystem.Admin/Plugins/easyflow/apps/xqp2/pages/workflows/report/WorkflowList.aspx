<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_report_WorkflowList" Title="工单列表" Codebehind="WorkflowList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>未处理工单列表</span></h3>
    </div>
    <div class="btnControl">
        <div class="btnLeft">
            <input type="button" class="btnFW" value="返回" onclick="window.location='reportindex.aspx';" />
        </div>
    </div>  
    
    <div class="dataList">  
        <div class="showControl">
            <h4>未处理工单列表</h4>
            <button onclick="return showContent(this,'divwflist1');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div id="divwflist1">
            <div class="dataTable" id="dataTable1">
                <table cellpadding="4" cellspacing="1" class="tblGrayClass" id="tblId1" style="text-align: center;">
                    <tr>
                        <th width="7%">类型</th>
                        <th width="10%">标题</th>
                        <th width="35%">标题</th>
                        <th width="20%">当前步骤</th>
                        <th width="10%">发起人</th>
                        <th width="18%">创建时间</th>
                    </tr>
                    <asp:Repeater ID="rptTrackingWorkflows" runat="server" OnItemDataBound="rptTrackingWorkflows_ItemDataBound">
                        <ItemTemplate>
                            <tr style="text-align: center;">
                                <td>
                                    <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <%# Eval("SheetId") %>
                                </td>
                                <td style="text-align: left;">
                                    <asp:HyperLink ID="linkActivity" runat="server" Text='<%# Eval("Title") %>'>
                                    </asp:HyperLink>
                                </td>
                                <td><%# Eval("ActivityName")%></td>
                                <td><%# Eval("CreatorName")%></td>
                                <td><%# Eval("CreatedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="trClass" style="text-align: center;">
                                <td>
                                   <asp:Literal ID="ltlWorkflowAlias" Text='<%# Eval("WorkflowAlias") %>' runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <%# Eval("SheetId") %>
                                </td>
                                <td style="text-align: left;">
                                    <asp:HyperLink ID="linkActivity" runat="server" Text='<%# Eval("Title") %>'>
                                    </asp:HyperLink>
                                </td>
                                <td><%# Eval("ActivityName")%></td>
                                <td><%# Eval("CreatorName")%></td>
                                <td><%# Eval("CreatedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
                <div class="toolBlock" style="border-top: solid 1px #C0CEDF">
                    <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                        Font-Size="9pt" ItemsPerPage="20" PagerStyle="NumericPages" BorderWidth="0px"
                        OnPageIndexChanged="listPager_PageIndexChanged" />
                </div>
            </div>
        </div>
    
    </div>
</asp:Content>
