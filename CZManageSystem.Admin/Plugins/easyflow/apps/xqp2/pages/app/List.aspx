<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_app_List" Codebehind="List.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3>
            <span>应用系统接入</span></h3>
        <div style="float: right; margin-right: 3px;">
            <input type="button" value="新增系统" onclick="window.location='edit.aspx';" class="btnPassClass" />
        </div>
    </div>   
    <div class="btnControl">
        <div class="btnRight">
            <input type="button" value="新增系统" onclick="window.location='edit.aspx';" class="btnFW" />
         </div>
    </div>
    <div id="dataDiv1" style="margin-top:5px;">
        <div class="dataTable" id="dataTable1">
            <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1">
                <tr style="text-align: center;">
                    <th style="width: 60px;">系统名称</th>
                    <th>
                        接入方式
                    </th>
                    <th>接入地址</th>
                    <th>创建人</th>
                    <th style="width: 80px;">创建时间</th>
                    <th style="width: 60px;">激活状态</th>
                    <th style="width: 100px;">操作</th>
                </tr>
                <asp:Repeater ID="rpAppsList" runat="server" OnItemCommand="rpAppsList_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <%# Eval("AppName") %>
                            </td>
                              <td align="center">
                                <%# (bool)Eval("AccessType")?"WebService":"Web" %>
                            </td>
                            <td align="center">
                                <%# AppDoaminReplace(Eval("AccessUrl").ToString())%>
                            </td>
                            <td align="center">
                                <%# Eval("Creator") %>
                            </td>
                            <td align="center">
                                <%# Eval("CreatedTime", "{0:yyyy-MM-dd}")%>
                            </td>                          
                            <td align="center">
                                <%# Eval("Enabled").ToString().ToLower() == "true" ? "已激活" : "未激活"%>
                            </td>
                            <td style="text-align: center;">
                                <a href="Edit.aspx?AppId=<%# Eval("AppId") %>" class="ico_edit">修改</a>
                                <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("AppId") %>' ID="btnDelete"
                                    CssClass="ico_del" OnClientClick="return (confirm('确定删除当前这条记录吗？'));" runat="server">删除</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="trClass">
                            <td align="center">
                                <%# Eval("AppName") %>
                            </td>
                              <td align="center">
                                <%# (bool)Eval("AccessType")?"WebService":"Web" %>
                            </td>
                             <td align="center">
                                <%# AppDoaminReplace(Eval("AccessUrl").ToString())%>
                            </td>
                            <td align="center">
                                <%# Eval("Creator") %>
                            </td>
                            <td align="center">
                                <%# Eval("CreatedTime", "{0:yyyy-MM-dd}")%>
                            </td>
                            <td align="center">
                                 <%# Eval("Enabled").ToString().ToLower() == "true" ? "已激活" : "未激活"%>
                            </td>
                            <td style="text-align: center;">
                                <a href="Edit.aspx?AppId=<%# Eval("AppId") %>" class="ico_edit">修改</a>
                                <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("AppId") %>' ID="btnDelete"
                                    CssClass="ico_del" OnClientClick="return (confirm('确定删除当前这条记录吗？'));" runat="server">删除</asp:LinkButton>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
</asp:Content>
