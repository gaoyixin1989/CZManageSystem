<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_FormIndex" CodeBehind="FormIndex.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="divList" runat="server">
        <div class="titleContent">
            <h3><span>表单库管理</span></h3>
            <div style="float: right; margin-right: 3px;">
                <input type="button" value="新增表单" onclick="window.location = 'create.aspx';" class="btnPassClass" />
            </div>
        </div>
        <div class="btnControl">
            <div class="btnRight">
                <input type="button" value="新增表单" onclick="window.location = 'create.aspx';" class="btnFW" />
            </div>
        </div>
        <div id="dataDiv1" style="margin-top: 5px">
            <div class="dataTable" id="dataTable1">
                <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align: center;">
                    <tr>
                        <th width="35%">表单名称
                        </th>
                        <th width="8%">备注
                        </th>
                        <th width="12%">创建人

                        </th>
                        <th width="20%">创建时间
                        </th>
                        <th width="25%">操作
                        </th>
                    </tr>
                    <asp:Repeater ID="rpFormList" runat="server" OnItemCommand="rpFormList_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("Name") %>
                                </td>
                                <td>
                                    <%# Eval("Comment")%>
                                </td>
                                <td>
                                    <%# Eval("Creator")%>
                                </td>
                                <td>
                                    <%# Eval("CreatedTime", "{0:yyyy-MM-dd  hh:mm:ss}")%>
                                </td>
                                <td>
                                    <a href="TemplateCreate.aspx?fdid=<%# Eval("Id") %>" class="ico_index">表单设计</a>
                                    <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("Id") %>' ID="LinkButton4"
                                        CssClass="ico_del" OnClientClick="return (confirm('确定删除该表单吗？'));" runat="server">删除</asp:LinkButton>

                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="trClass">
                                <td>
                                    <%# Eval("Name") %>
                                </td>
                                <td>
                                    <%# Eval("Comment")%>
                                </td>
                                <td>
                                    <%# Eval("Creator")%>
                                </td>
                                <td>
                                    <%# Eval("CreatedTime", "{0:yyyy-MM-dd  hh:mm:ss}")%>
                                </td>
                                <td>
                                    <a href="TemplateCreate.aspx?fdid=<%# Eval("Id") %>" class="ico_index">表单设计</a>
                                    <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("Id") %>' ID="LinkButton4"
                                        CssClass="ico_del" OnClientClick="return (confirm('确定删除该表单吗？'));" runat="server">删除</asp:LinkButton>

                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
