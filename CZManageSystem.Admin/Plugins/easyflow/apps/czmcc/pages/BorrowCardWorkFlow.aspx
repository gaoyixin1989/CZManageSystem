﻿<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_czmcc_pages_BorrowCardWorkFlow"
    Title="无标题页" Codebehind="BorrowCardWorkFlow.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="titleContent">
        <h3>
            <span>EDGE卡对应工单</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>EDGE卡基本信息</h4>
        </div>
        <div class="dataTable">
            <table  cellpadding="4" cellspacing="1" class="tblGrayClass grayBackTable">
                <tr>
                    <th style="width:13%">EDGE卡号：</th>
                    <td style="width:37%;">
                        <asp:Literal ID="ltlEDGENumber" runat="server" />
                    </td>
                    <th style="width:13%;">SIM卡号码：</th>
                    <td style="width:37%;">
                        <asp:Literal ID="ltlSIMNumber" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th style="width:13%">当前状态：</th>
                    <td style="width:37%;">
                        <asp:Literal ID="ltlEDGEStatus" runat="server" />
                    </td>
                    <th style="width:13%;"></th>
                    <td style="width:37%;">
                        <asp:Literal ID="Literal2" runat="server" />
                    </td>        
                </tr>
                <asp:Literal ID="ltlResourceInfo" runat="server" />
            </table>
        </div>
        <div id="dataDiv1">
            <div class="dataTable" id="dataTable1">
                <fieldset>
                    <legend>工单查询</legend>
                    <div style="margin-bottom: 10px;">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;工单名称：<asp:TextBox ID="txtWorkFlowName" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnSearch" runat="server" Text="查 询" class="btn_add" OnClick="btnSearch_Click" />
                    </div>
                </fieldset>
                <br />
                <div id="Note">
                    <asp:GridView ID="gvBorrowList" Width="100%" CssClass="tblClass" DataKeyNames="ID"
                        runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="工单名称">
                                <ItemTemplate>
                                    <a href="../../../contrib/workflow/pages/WorkflowView.aspx?aiid=<%# Eval("CorrWorkID") %>">
                                    <%# Eval("Title")%></a>
                                </ItemTemplate>
                                <HeaderStyle Width="25%" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ResourcesModel" HeaderText="EDGE卡号">
                                <HeaderStyle Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SerialNumber" HeaderText="SIM卡号码">
                                <HeaderStyle Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <a href="../../../contrib/workflow/pages/WorkflowView.aspx?aiid=<%# Eval("CorrWorkID") %>">
                                        查看工单</a>
                                </ItemTemplate>
                                <HeaderStyle Width="9%" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="trClass" />
                    </asp:GridView>
                </div>
                <div style="float: right; width: 40%">
                    <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                        Font-Size="9pt" ItemsPerPage="20" PagerStyle="NumericPages" BorderWidth="0px"
                        OnPageIndexChanged="listPager_PageIndexChanged" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
