<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_report_pages_ReportCreate3" Codebehind="ReportCreate3.aspx.cs" %>

<%@ Register Src="../controls/NavigationTools.ascx" TagName="NavigationTools" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">

    <script type="text/javascript" src="../res/js/reportCreate.js"></script>

    <div id="divList" runat="server">
        <div class="titleContent">
            <uc1:NavigationTools ID="NavigationTools1" runat="server" />
        </div>
        <div class="dataList">
            <h5>
                <input type="radio" name="createMode" onclick="window.location.href='ReportCreate.aspx'" />可视化模式

                <input type="radio" name="createMode" onclick="window.location.href='ReportCreate2.aspx'" />SQL模式
                <input type="radio" name="createMode" checked="checked" onclick="window.location.href='ReportCreate3.aspx'" />存储过程模式
            </h5>
            <table class='tblClass' cellspacing='0' border='1' style="border-collapse: collapse;">
                <tr>
                    <td align="right" style="width: 20%">
                        报表名称：

                    </td>
                    <td style="width: 80%">
                        <asp:TextBox ID="txtReportName" runat="server" Width="50%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtReportName"
                            Display="Dynamic" ErrorMessage="报表名称不能为空" ValidationGroup="saveReport">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        备注：

                    </td>
                    <td>
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="50%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        存储过程：

                    </td>
                    <td>
                        <asp:TextBox ID="txtSP" runat="server" Width="50%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSP" runat="server" ControlToValidate="txtSP" Display="Dynamic"
                            ErrorMessage="存储过程不能为空" SetFocusOnError="True" ValidationGroup="saveReport">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        参数：

                    </td>
                    <td style="width: 100%">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    参数名：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtParameter" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvPara" runat="server" ControlToValidate="txtParameter"
                                        Display="Dynamic" ErrorMessage="参数名不能为空" SetFocusOnError="True" ValidationGroup="AddItem">*</asp:RequiredFieldValidator>
                                </td>
                                <td align="right">
                                    数据类型：

                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDataType" runat="server">
                                        <asp:ListItem Value="string">字符型</asp:ListItem>
                                        <asp:ListItem Value="bool">布尔型</asp:ListItem>
                                        <asp:ListItem Value="int">整型</asp:ListItem>
                                        <asp:ListItem Value="datetime">时间型</asp:ListItem>
                                        <asp:ListItem Value="float">浮点型</asp:ListItem>
                                        <asp:ListItem Value="uniqueidentifier">GUID型</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="right">
                                    默认值：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDefaultValue" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDefalut" runat="server" ControlToValidate="txtDefaultValue"
                                        Display="Dynamic" ErrorMessage="默认值不能为空" SetFocusOnError="True" ValidationGroup="AddItem">*</asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Button ID="btnAddItem" runat="server" Text="增加" CssClass="btn_add" OnClick="btnAddItem_Click"
                                        ValidationGroup="AddItem" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:DataGrid ID="dgItem" runat="server" Width="100%" ShowHeader="False" AutoGenerateColumns="False"
                                        OnDeleteCommand="dgItem_DeleteCommand">
                                        <Columns>
                                            <asp:BoundColumn DataField="Parameter"></asp:BoundColumn>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <%# GetTypeName( Eval("DataType").ToString()) %>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="DefaultValue"></asp:BoundColumn>
                                            <asp:ButtonColumn CommandName="Delete" Text="删除"></asp:ButtonColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnPreview" runat="server" Text="预览" CssClass="btnReview" OnClick="btnPreview_Click"
                            ValidationGroup="saveReport" />
                        <asp:Button ID="btnSaveSql" runat="server" Text="保存" CssClass="btnSaveClass" OnClick="btnSaveSql_Click"
                            ValidationGroup="saveReport" />
                        <input type="button" value="返回" class='btnReturnClass' onclick="window.location.href('ReportList.aspx')" />
                        <asp:ValidationSummary ID="vsItem" runat="server" ShowMessageBox="True" ShowSummary="False"
                            ValidationGroup="AddItem" />
                        <asp:ValidationSummary ID="vsSave" runat="server" ShowMessageBox="True" ShowSummary="False"
                            ValidationGroup="saveReport" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
