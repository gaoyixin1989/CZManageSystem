<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_report_pages_DrawGrapConfig" Codebehind="DrawGrapConfig.aspx.cs" %>

<%@ Register Src="../controls/NavigationTools.ascx" TagName="NavigationTools" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="divList" runat="server">
        <div class="titleContent">
            <uc1:NavigationTools ID="NavigationTools1" runat="server" />
        </div>
        <div class="dataList">
            <table class="tblClass" cellspacing='0' border='1' style='border-collapse: collapse;'>
                <tr>
                    <td align="right">
                        已有图表：

                    </td>
                    <td colspan="3">
                        <asp:DataGrid ID="dgList" runat="server" AutoGenerateColumns="False" class="tblClass"
                            Width="100%" DataKeyField="ID" OnDeleteCommand="dgList_DeleteCommand" OnEditCommand="dgList_EditCommand"
                            OnItemDataBound="dgList_ItemDataBound">
                            <Columns>
                                <asp:TemplateColumn HeaderText="序号">
                                    <ItemTemplate>
                                        <%# Container.ItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="TypeName" HeaderText="图表类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Title" HeaderText="图表名称"></asp:BoundColumn>
                                <asp:EditCommandColumn CancelText="取消" EditText="编辑" UpdateText="更新"></asp:EditCommandColumn>
                                <asp:ButtonColumn CommandName="Delete" Text="删除"></asp:ButtonColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        名称：

                    </td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                    </td>
                    <td align="right">
                        图表类型：

                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblType" RepeatDirection="Horizontal" runat="server" RepeatLayout="Flow">
                            <asp:ListItem Value="0" Text="线图" onclick="TypeChange(0)" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="1" Text="柱图" onclick="TypeChange(0)"></asp:ListItem>
                            <asp:ListItem Value="2" Text="饼图" onclick="TypeChange(2)"></asp:ListItem>
                            <asp:ListItem Value="3" Text="柱线图" onclick="TypeChange(3)"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        画板宽度：

                    </td>
                    <td>
                        <asp:TextBox ID="txtWidth" runat="server"></asp:TextBox>PX
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtWidth"
                            Display="Dynamic" ErrorMessage="请为宽度输入1-1024之间的整数" MaximumValue="1024" MinimumValue="1"
                            SetFocusOnError="True" Type="Integer">请输入1-1024之间的整数</asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtWidth"
                            Display="Dynamic" ErrorMessage="宽度不能为空" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    </td>
                    <td align="right">
                        画板高度：

                    </td>
                    <td>
                        <asp:TextBox ID="txtHeight" runat="server"></asp:TextBox>PX
                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtHeight"
                            Display="Dynamic" ErrorMessage="请为高度输入大于1的整数" MaximumValue="9999999" MinimumValue="1"
                            SetFocusOnError="True" Type="Integer">请为输入大于1的整数</asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtHeight"
                            Display="Dynamic" ErrorMessage="高度不能为空" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="divBar1">
                    <td align="right">
                        X轴的名称：

                    </td>
                    <td>
                        <asp:TextBox ID="txtXTitle" runat="server"></asp:TextBox>
                    </td>
                    <td align="right">
                        Y轴的名称：

                    </td>
                    <td>
                        <asp:TextBox ID="txtYTitle" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr id="divBar2">
                    <td align="right">
                        X轴的数据：

                    </td>
                    <td>
                        <asp:DropDownList ID="ddlXFieldName" runat="server" DataTextField="FieldName" DataValueField="FieldName">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        X轴的数据：

                    </td>
                    <td>
                        <asp:DropDownList ID="ddlXType" runat="server">
                            <asp:ListItem Value="3">文本</asp:ListItem>
                            <asp:ListItem Value="0">数值</asp:ListItem>
                            <asp:ListItem Value="2">日期</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="divBar3">
                    <td align="right">
                        Y轴的数据：

                    </td>
                    <td colspan="3">
                        <asp:CheckBoxList ID="cblYFieldNames" RepeatDirection="Horizontal" runat="server"
                            DataTextField="FieldName" DataValueField="Field" RepeatColumns="6">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr id="divBarline" style="display: none">
                    <td align="right">
                        Y轴的数据：

                    </td>
                    <td colspan="3">
                        <table width="100%" class="tblClass" cellspacing='0' border='1' style='border-collapse: collapse;'>
                            <tr>
                                <td>
                                    柱：
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="cblYBar" RepeatDirection="Horizontal" runat="server" DataTextField="FieldName"
                                        DataValueField="Field" RepeatColumns="6">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    线：
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="cblYLine" RepeatDirection="Horizontal" runat="server" DataTextField="FieldName"
                                        DataValueField="Field" RepeatColumns="6">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trPie" style="display: none">
                    <td align="right">
                        数据字段：

                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDataField" runat="server" DataTextField="FieldName" DataValueField="FieldName">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        说明字段：

                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTagField" runat="server" DataTextField="FieldName" DataValueField="FieldName">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                            ShowSummary="False" />
                        <asp:Button ID="btnSave" runat="server" Text="增加" OnClientClick="if(!confirm('你确认要提交吗？')) return false;"
                            CssClass="btnSaveClass" OnClick="btnSave_Click" />
                        <input type="button" onclick="window.location.href('ReportList.aspx')" value="返回"
                            class="btnReturnClass" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript">   
    var s = "<%= GrapItemID %>";
    if(s && s != "")
        TypeChange(parseInt(s));
    function TypeChange(v)
    {
        document.getElementById("divBar1").style.display = v != 2 ? "inline" : "none";;
        document.getElementById("divBar2").style.display = v != 2 ? "inline" : "none";;
        document.getElementById("divBar3").style.display = v == 0 ? "inline" : "none";;
        document.getElementById("trPie").style.display = v == 2 ? "inline" : "none";
        document.getElementById("divBarline").style.display = v == 3 ? "inline" : "none";
    }   
    </script>

</asp:Content>
