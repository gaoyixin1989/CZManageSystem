<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="ReportConfig" Codebehind="ReportConfig.aspx.cs" %>

<%@ Register Src="../controls/NavigationTools.ascx" TagName="NavigationTools" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <link href="../res/css/Report.css" type="text/css" rel="Stylesheet" rev="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <script type="text/javascript" src="../res/js/jquery-1.2.6.pack.js"></script>

    <script type="text/javascript" src="../res/js/reportConfig.js"></script>

    <div id="divList" runat="server">
        <div class="titleContent">
            <uc1:NavigationTools ID="NavigationTools1" runat="server" />
        </div>
        <div class="dataList">           
            <input type="button" id="btnSave" runat="server" class="btnSaveClass" value="保存"
                onclick="if(!CheckReportSetting()) return false;" onserverclick="btnSave_Click" />
            <input type="button" class="btnReturnClass" value="返回" onclick="javascript:window.location.href('ReportList.aspx');" /><br />
            <asp:Repeater ID="rptConfig" runat="server" OnItemDataBound="rptConfig_ItemDataBound">
                <HeaderTemplate>
                    <table cellpadding="3" cellspacing="0" width="100%" class="tbl_report_config">
                        <tr class="tblClass">
                            <th>
                            </th>
                            <th>
                                序号
                            </th>
                            <th>
                                表名
                            </th>
                            <th>
                                显示名称
                            </th>
                            <th>
                                类型
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="tr_show_table">
                        <td>
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                        </td>
                        <td>
                            <%# Container.ItemIndex + 1 %>
                        </td>
                        <td class="td_show_table">
                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("OBJECT_NAME")%>'> </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("OBJECT_NAME")%>' />
                        </td>
                        <td>
                            <asp:Label ID="lblType" runat="server" Text=' <%# Eval("TYPE")%>'></asp:Label>
                        </td>
                    </tr>
                    <tr class="tr_show_column">
                        <td colspan="5">
                            <asp:Repeater ID="rptTableData" runat="server" OnItemDataBound="rptTableData_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="tblClass" width="100%">
                                        <tr>
                                            <th>
                                                选择
                                            </th>
                                            <th>
                                                字段名

                                            </th>
                                            <th>
                                                显示名称
                                            </th>
                                            <th>
                                                表名
                                            </th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox2" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblColumnName" runat="server" Text='<%# Eval("NAME") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Eval("NAME") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTableName" runat="server" Text='<%# Eval("TABLE_NAME") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <br />
            <input type="button" id="btnSave2" runat="server" class="btnSaveClass" value="保存"
                onclick="if(!CheckReportSetting()) return false;" onserverclick="btnSave_Click" />
            <input type="button" class="btnReturnClass" value="返回" onclick="javascript:window.location.href('ReportList.aspx');" />
        </div>
    </div>
</asp:Content>
