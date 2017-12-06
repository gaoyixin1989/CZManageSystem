<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_exceptionLogger_ErrorView" Title="异常日志"  Codebehind="ErrorView.aspx.cs" %>

<%@ Register Src="../../../contrib/report/controls/NavigationTools.ascx" TagName="NavigationTools"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <link href="../res/css/Report.css" type="text/css" rel="Stylesheet" rev="Stylesheet" />
    <div id="divList" runat="server">
        <div class="titleContent">
            <uc1:NavigationTools ID="NavigationTools1" runat="server" />
        </div>
        <div class="dataList">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        检索方式：<asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            <asp:ListItem Value="0">按错误编号</asp:ListItem>
                            <asp:ListItem Value="1">按报错时间</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        关键字：
                    </td>
                    <td id="tdNo" runat="server">
                        <asp:TextBox ID="txtExceptionID" runat="server" />
                    </td>
                    <td id="tdDT" runat="server" visible="false">
                        <bw:DateTimePicker ID="dtpBegin" runat="server" IsRequired="true" RequiredErrorMessage="必须输入日期"
                            DateType="Datetime" />
                        至

                        <bw:DateTimePicker ID="dtpEnd" runat="server" IsRequired="true" RequiredErrorMessage="必须输入日期"
                            DateType="Datetime" />
                    </td>
                    <td>
                        &nbsp;
                        <asp:Button ID="btnQuery" runat="server" Text="检索" OnClick="btnQuery_Click" CssClass="btn_query" />
                    </td>
                    <td>
                        &nbsp; &nbsp;
                        <asp:Button ID="btnExport2File" runat="server" Text="导出" OnClick="btnExport2File_Click"
                            CssClass="btn_sav" />
                    </td>
                </tr>
            </table>
            <br />
            &nbsp;
            <asp:DataGrid ID="dgLogs" runat="server" Width="100%" CssClass="tblClass" AutoGenerateColumns="false"
                OnItemDataBound="dgLogs_ItemDataBound">
                <Columns>
                    <asp:BoundColumn DataField="ID" HeaderText="错误号"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Message" HeaderText="提示信息"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Describe" HeaderText="描述"></asp:BoundColumn>
                    <asp:BoundColumn DataField="ClientIP" HeaderText="客户端IP"></asp:BoundColumn>
                    <asp:BoundColumn DataField="ServerIP" HeaderText="服务器IP"></asp:BoundColumn>
                    <asp:BoundColumn DataField="PageURL" HeaderText="报错页面"></asp:BoundColumn>
                    <asp:BoundColumn DataField="ExceptionTime" HeaderText="报错时间"></asp:BoundColumn>
                    <asp:BoundColumn DataField="ExceptionContent" HeaderText="异常内容"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="堆栈信息">
                        <ItemTemplate>
                            <asp:Label ID="lblStackTrace" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
            <div class="toolBlock" style="border-top: solid 1px #C0CEDF">
                共有<strong><asp:Literal ID="ltlTotalRecordCount" runat="server"></asp:Literal></strong>
                条记录

                <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                    Font-Size="9pt" ItemsPerPage="15" PagerStyle="NumericPages" BorderWidth="0px"
                    OnPageIndexChanged="listPager_PageIndexChanged" />
            </div>
        </div>
    </div>
</asp:Content>
