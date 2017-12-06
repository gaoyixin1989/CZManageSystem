<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_IntelligentRemind" Codebehind="IntelligentRemind.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>智能提醒</span></h3>
    </div>
    <div class="dataList">
        <div id="dataDiv1">
            <div class="dataTable" id="dataTable1">
                <asp:GridView ID="gvRemind" Width="100%" CssClass="tblClass" DataKeyNames="ID" runat="server"
                    AutoGenerateColumns="False" OnRowCancelingEdit="gvRemind_RowCancelingEdit" OnRowEditing="gvRemind_RowEditing"
                    OnRowUpdating="gvRemind_RowUpdating" OnRowDataBound="gvRemind_RowDataBound" BorderWidth="0">
                    <AlternatingRowStyle CssClass="trClass" />
                    <Columns>
                        <asp:BoundField DataField="ActivityName" HeaderText="步骤名称" ReadOnly="true">
                            <ItemStyle Width="18%"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="是否紧急单">
                            <ItemStyle Width="11%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblUrgency" runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlUrgency" runat="server">
                                    <asp:ListItem Value="0">否</asp:ListItem>
                                    <asp:ListItem Value="1">是</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="是否重要单">
                            <ItemStyle Width="11%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblImportance" runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlImportance" runat="server">
                                    <asp:ListItem Value="0">否</asp:ListItem>
                                    <asp:ListItem Value="1">是</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="允许滞留时间(小时)">
                            <ItemStyle Width="19%"></ItemStyle>
                            <ItemTemplate>
                                <%# Eval("StayHours").ToString() == "-1" ? "无限制" : Eval("StayHours")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtStayHours" Width="30px" Text='<%# Eval("StayHours")%>' runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="提醒次数">
                            <ItemStyle Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <%# Eval("RemindTimes").ToString() == "-1" ? "无限制" : Eval("RemindTimes")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRemindTimes" Width="30px" Text='<%# Eval("RemindTimes")%>' runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="提醒方式">
                            <ItemStyle Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblRemindTypeName" runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlRemindType" runat="server">
                                    <asp:ListItem Value="0">-未设置-</asp:ListItem>
                                    <asp:ListItem Value="1">电子邮件</asp:ListItem>
                                    <asp:ListItem Value="2">短信</asp:ListItem>
                                    <asp:ListItem Value="3">短信+电子邮件</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField HeaderText="操作" EditText="&lt;span class=&quot;ico_edit&quot;&gt;设置&lt;span&gt;"
                            UpdateText="保存" ShowEditButton="true" ShowDeleteButton="false" CancelText="取消"
                            HeaderStyle-Width="10%" />
                        <asp:TemplateField HeaderStyle-Width="19%" HeaderText=" ">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlinkActivity" runat="server" Text="紧急单设置"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblRemindType" runat="server" Text='<%# Eval("RemindType") %>' />
                                <asp:Label ID="lblActivityName" runat="server" Text='<%# Eval("ActivityName") %>' />
                                <asp:Label ID="lblExtArgs" runat="server" Text='<%# Eval("ExtArgs") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <hr />
                <div style="text-align:right;">
                    <input type="button" onclick="history.go(-1)" class="btnReturnClass" value="返回" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
