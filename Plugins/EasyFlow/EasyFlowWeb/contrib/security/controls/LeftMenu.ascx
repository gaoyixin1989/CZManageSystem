<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_security_controls_LeftMenu" Codebehind="LeftMenu.ascx.cs" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security" Namespace="Botwave.Security.Web.Controls" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security.Extension" Namespace="Botwave.Security.Extension.Web.Controls" %>
<bw:LeftMenuView ID="lmv" VerifyResource="true" runat="server" Title="用户权限">
    <bw:MenuListItem ID="MenuListItem1" runat="server" Text="登录" NavigateUrl="../contrib/security/pages/login.aspx" Target="_parent" />
    <bw:MenuListItem ID="MenuListItem2" runat="server" Text="带验证码的登录" NavigateUrl="../contrib/security/pages/loginindex.aspx" Target="_parent" />
    <bw:MenuListItem ID="MenuListItem3" runat="server" Text="用户列表" NavigateUrl="../contrib/security/pages/users.aspx" ResourceValue="A002" />
    <bw:MenuListItem ID="MenuListItem4" runat="server" Text="角色列表" NavigateUrl="../contrib/security/pages/roles.aspx" ResourceValue="A003" />
    <bw:MenuListItem ID="MenuListItem5" runat="server" Text="权限资源列表" NavigateUrl="../contrib/security/pages/resources.aspx" ResourceValue="A003" />
    <bw:MenuListItem ID="MenuListItem6" runat="server" Text="授权" NavigateUrl="../contrib/security/pages/authorize.aspx" ResourceValue="A005" />
    <bw:MenuListItem ID="MenuListItem7" runat="server" Text="修改密码" NavigateUrl="../contrib/security/pages/changePassword.aspx" ResourceValue="A006" />
    <bw:MenuListItem ID="MenuListItem8" runat="server" Text="用户选择示例" NavigateUrl="../contrib/security/pages/TestPickUser.aspx" />
</bw:LeftMenuView>
