<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="ReportCreate" Codebehind="ReportCreate.aspx.cs" %>

<%@ Register Src="../controls/NavigationTools.ascx" TagName="NavigationTools" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <link href="../res/css/Report.css" type="text/css" rel="Stylesheet" rev="Stylesheet" />

    <script type="text/javascript" src="../res/js/jquery-1.2.6.pack.js"></script>

    <script type="text/javascript" src="../res/js/reportCreate.js"></script>

    <script type="text/javascript" src="../res/js/reportCreateSet.js"></script>

    <div id="divList" runat="server">
        <div class="titleContent">
            <uc1:NavigationTools ID="NavigationTools1" runat="server" />
        </div>
        <div class="dataList">
            <h5>
                <input type="radio" name="createMode" onclick="window.location.href='ReportCreate.aspx'"
                    checked="checked" />可视化模式

                <input type="radio" name="createMode" onclick="window.location.href='ReportCreate2.aspx'" />SQL模式
                <input type="radio" name="createMode" onclick="window.location.href='ReportCreate3.aspx'" />存储过程模式
            </h5>
            <table id="tb_normal_mode" class="tblClass" cellspacing="0" border="1" style="border-collapse: collapse;
                collapse">
                <tr>
                    <td align="right">
                        数据来源：

                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSource" runat="server" DataTextField="TableAlias" DataValueField="TableName">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        报表名称：

                    </td>
                    <td>
                        <asp:TextBox ID="txtReportName" runat="server" Columns="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtReportName"
                            Display="Dynamic" SetFocusOnError="True">报表名称不能为空</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        备注：

                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Columns="100" Rows="3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table id="tblConfig" width="100%" class="tbl_reportset" runat="server">
                            <tr class="header_row">
                                <th>
                                    输出
                                </th>
                                <th>
                                    列

                                </th>
                                <th>
                                    一级列名

                                </th>
                                <th>
                                    二级列名
                                </th>
                                <th>
                                    三级列名
                                </th>
                                <th>
                                    排序类型
                                </th>
                                <th>
                                    排序顺序
                                </th>
                                <th>
                                    分组
                                </th>
                                <th>
                                    条件
                                </th>
                                <th>
                                    操作
                                </th>
                            </tr>
                            <tr class="clone_row" id="clone_row" style="display: none">
                                <td>
                                    <input name="checkbox_name" type="checkbox" checked="checked" class="chk" />
                                </td>
                                <td class="column" onfocus="ColumnFocus(this)">
                                </td>
                                <td class="firstTitle" onfocus="AliasFocus(this)">
                                </td>
                                <td class="secondTitle" onfocus="AliasFocus(this)">
                                </td>
                                <td class="thirdTitle" onfocus="AliasFocus(this)">
                                </td>
                                <td class="order" onfocus="OrderFocus(this)">
                                </td>
                                <td class="orderseq" onfocus="OrderSeqFocus(this)">
                                </td>
                                <td class="group" onfocus="GroupFocus(this)">
                                </td>
                                <td class="condition" onfocus="ConditionFocus(this)">
                                </td>
                                <td class="control">
                                    <input type="button" value="+" onclick="AddNewRow()" /><input type="button" value="-"
                                        onclick="DelRow(this)" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:HiddenField ID="hidReportItem" runat="server" />
                        <input type="button" value="预览" runat="server" class="btnReview" onclick="ReportPreview('preview')" />
                        <input type="button" id="btnSave" runat="server" value="保存" class="btn_sav" onclick="SaveReportData();"
                            onserverclick="btnSave_Click" />
                        <input type="button" value="返回" class="btnReturnClass" onclick="window.location.href('ReportList.aspx')" />
                    </td>
                </tr>
            </table>
            <div id="column_list" style="display: none" runat="server">
            </div>
        </div>
    </div>
</asp:Content>
