<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_security_pages_Roles" Title="权限管理"  Codebehind="Roles.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="titleContent">
        <h3><span>权限管理</span></h3>
        <div style="float:right; margin-right:3px;">
            <input id="btnCreateRole" runat="server" type="button" value="新增角色" onclick="window.location.href = 'editrole.aspx';" class="btnPassClass" />
        </div>
    </div>
    
    <div class="btnControl">
		<div class="btnLeft">
			<input type="button" value="批量分配角色" class="btnFW" onclick="window.location.href='deptroles.aspx';" />
			<input type="button" value="用户管理" class="btnNewwin" onclick="window.location.href='users.aspx';" />
			<input type="button" value="资源管理" class="btnPrint" onclick="window.location.href='resources.aspx';" />
		</div>
	</div>	
    <div class="dataList">
        <div class="showControl">
            <h4>角色组管理</h4>
            <button onclick="return showGroupContent(this,'groupTable');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable" id="groupTable" style="display:none">
            <asp:Button ID="btnBindGroups" runat="server" onclick="btnBindGroups_Click" />
            <asp:UpdatePanel ID="updatePanelGroups" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <table cellpadding="0" cellspacing="0" class="tblClass" style="text-align: center;">
                    <tr>
                        <th>名称</th>
                        <th style="width:120px">开始时间</th>
                        <th style="width:120px">截止时间</th>
                        <th style="width:100px">操 作</th>
                    </tr>
                    <asp:Repeater ID="rptGroups" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td style="text-align:left"><%# Eval("RoleName") %></td>
                                <td><%# Eval("BeginTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                <td><%# Eval("EndTime") %></td>
                                <td>
                                    <a class="ico_edit" href="editrole.aspx?roleId=<%# Eval("RoleId") %>">修改</a> 
                                    <a class="ico_del" href="deleteRole.aspx?RoleId=<%# Eval("RoleId") %>" onclick="return confirm('确定要删除该角色吗?');">
                                    删除</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="trClass">
                                <td style="text-align:left"><%# Eval("RoleName") %></td>
                                <td><%# Eval("BeginTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                <td><%# Eval("EndTime") %></td>
                                <td>
                                    <a class="ico_edit" href="editrole.aspx?roleId=<%# Eval("RoleId") %>">修改</a> 
                                    <a class="ico_del" href="DeleteRole.aspx?RoleId=<%# Eval("RoleId") %>" onclick="return confirm('确定要删除该角色吗?');">
                                    删除</a>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBindGroups" EventName="click" />
                </Triggers>
             </asp:UpdatePanel>
        </div>
        <div class="showControl">
            <h4>角色管理</h4>
            <button onclick="return showContent(this,'roleTable');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable" id="roleTable">
            <div style="text-align:right;">
                选择角色组：
                <asp:DropDownList ID="ddlParentRoles" runat="server" AutoPostBack="true" onselectedindexchanged="ddlParentRoles_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <asp:UpdatePanel ID="updatePanelChilds" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <table cellpadding="0" cellspacing="0" class="tblClass" style="text-align: center;">
                    <tr>
                        <th>名称</th>
                        <th>角色ID</th>
                        <th style="width:120px">开始时间</th>
                        <th style="width:120px">截止时间</th>
                        <th style="width:90px">操 作</th>
                        <th style="width:70px">浏览用户</th>
                    </tr>
                    <asp:Repeater ID="repeaterRoles" runat="server" OnItemDataBound="repeaterRoles_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td style="text-align:left"><%# Eval("RoleName") %></td>
                                 <td style="text-align:left"><%# Eval("RoleID") %></td>
                                <td><%# Eval("BeginTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                <td><%# Eval("EndTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                <td>
                                    <a class="ico_edit" href="EditRole.aspx?RoleId=<%# Eval("RoleId") %>&Pid=<%# Eval("parentid") %>">修改</a> 
                                    <a class="ico_del" href="DeleteRole.aspx?RoleId=<%# Eval("RoleId") %>&Pid=<%# Eval("parentid") %>" onclick="return confirm('确定要删除该角色吗?');">删除</a>
                                </td>
                                <td>
                                    <a class="ico_preview" href="Users.aspx?RoleId=<%# Eval("RoleId") %>&Pid=<%# Eval("parentid") %>">浏览用户</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="trClass">
                                <td style="text-align:left"><%# Eval("RoleName") %></td>
                                <td style="text-align:left"><%# Eval("RoleID") %></td>
                                <td><%# Eval("BeginTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                <td><%# Eval("EndTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                <td>
                                    <a class="ico_edit" href="EditRole.aspx?RoleId=<%# Eval("RoleId") %>&Pid=<%# Eval("parentid") %>">修改</a> 
                                    <a class="ico_del" href="DeleteRole.aspx?RoleId=<%# Eval("RoleId") %>&Pid=<%# Eval("parentid") %>" onclick="return confirm('确定要删除该角色吗?');">删除</a>
                                </td>
                                <td>
                                    <a class="ico_preview" href="Users.aspx?RoleId=<%# Eval("RoleId") %>&Pid=<%# Eval("parentid") %>">浏览用户</a>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>  
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlParentRoles" EventName="selectedindexchanged" />
                </Triggers>
            </asp:UpdatePanel>
            
        </div>
    </div>
    <script type="text/javascript">
    function showGroupContent(sender, contentId){
        document.getElementById("<%=this.btnBindGroups.ClientID%>").click();
        return showContent(sender, contentId)
    }
    </script>
</asp:Content>
