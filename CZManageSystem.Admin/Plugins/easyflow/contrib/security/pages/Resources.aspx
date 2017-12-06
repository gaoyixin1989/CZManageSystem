<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_security_pages_Resources" Title="权限资源列表" Codebehind="Resources.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="titleContent">
        <h3><span>资源列表</span></h3>
        <div style="float:right; margin-right:3px;">
            <input type="button" value="新增资源" onclick="location='editresource.aspx'" class="btnPassClass" />
        </div>
    </div>    
    <div class="dataList">
        <div class="showControl">
            <h4>权限资源列表</h4>
            <button onclick="return showContent(this,'rlist_childs');" title="收缩"><span>折叠</span></button>
        </div>
        <div id="rlist_childs">
            <div style="text-align:right;">
                选择权限资源组：
                <asp:DropDownList ID="ddlParentResources" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="ddlParentResources_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <asp:UpdatePanel ID="updatePanelchild" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <table cellpadding="0" cellspacing="0" class="tblClass" style="text-align:center;">
                    <tr>
                        <th width="15%">编号</th>
                        <th width="22%">资源别名</th>
                        <th width="45%">资源名</th>
                        <th width="8%">启用</th>
                        <th width="10%"></th>
                    </tr>
                    <asp:Repeater ID="rptResources" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ResourceId") %></td>
                                <td style="text-align:left"><%# Eval("Alias")%></td>
                                <td style="word-break:break-all"><%# Eval("Name")%></td>
                                <td><asp:CheckBox ID="chkboxEnabled" runat="server" Enabled="false" Checked='<%# Eval("Enabled")%>' /></td>
                                <td>
                                    <a href="editResource.aspx?action=edit&resourceId=<%# Eval("ResourceId") %>">修改</a>
                                    <a href="editResource.aspx?action=del&resourceId=<%# Eval("ResourceId") %>" onclick="return confirm('确定要删除资源吗?');">删除</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="trClass">
                                <td><%# Eval("ResourceId") %></td>
                                <td style="text-align:left"><%# Eval("Alias")%></td>
                                <td style="word-break:break-all"><%# Eval("Name")%></td>
                                <td><asp:CheckBox ID="chkboxEnabled" runat="server" Enabled="false" Checked='<%# Eval("Enabled")%>' /></td>
                                <td>
                                    <a href="editResource.aspx?action=edit&resourceId=<%# Eval("ResourceId") %>">修改</a>
                                    <a href="editResource.aspx?action=del&resourceId=<%# Eval("ResourceId") %>" onclick="return confirm('确定要删除资源吗?');">删除</a>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlParentResources" EventName="selectedindexchanged" />
                </Triggers>
            </asp:UpdatePanel>            
        </div>
    </div>
</asp:Content>
