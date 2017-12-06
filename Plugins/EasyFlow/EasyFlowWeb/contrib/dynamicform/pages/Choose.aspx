<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_Choose" Codebehind="Choose.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="divList" runat="server">
        <div class="titleContent">
            <h3><span>为[<%=WorkflowName %>]生成表单</span></h3>
        <div style="float:right; margin-right:3px;">
            <input type="button" id="btnReturn" class="btn" value="返回" onclick="window.location='itemcreate.aspx?wid=<%=WorkflowId %>&EntityType=Form_Workflow';"/>
       </div>
        </div>
        
        <div id="dataDiv1" style="margin-top:5px">
            <div class="dataTable" id="dataTable1">
                <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1">
                    <tr class="table_tr" style="text-align: center;">
                        <th width="35%">
                            表单名称
                        </th>
                        <th width="8%">
                            备注
                        </th>
                        <th width="12%">
                            创建人

                        </th>
                        <th width="20%">
                            创建时间
                        </th>
                        <th width="25%">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater ID="rpFormList" runat="server" OnItemCommand="rpFormList_ItemCommand">
                        <ItemTemplate>
                            <tr class="table_tr">
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
                                    <%# Eval("CreatedTime", "{0:yy-MM-dd  hh:mm:ss}")%>
                                </td>
                                <td>                               
                                <asp:LinkButton CommandName="Bind" CommandArgument='<%# Eval("Id") %>' ID="ltlBind"
                                CssClass="ico_edit" OnClientClick="return (confirm('确定将该表单绑定至流程中吗？'));" runat="server">马上生成</asp:LinkButton>
                                    <a href="TemplateCreate.aspx?fdid=<%# Eval("Id") %>" class="ico_index">表单设计</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
