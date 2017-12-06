<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_LeftMenu" Codebehind="LeftMenu.ascx.cs" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security" Namespace="Botwave.Security.Web.Controls" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security.Extension" Namespace="Botwave.Security.Extension.Web.Controls" %>
<bw:LeftMenuView ID="lmv" VerifyResource="true" runat="server" Title="流程管理">
    <bw:MenuListItem ID="MenuListItem1" runat="server" Text="待办事宜" NavigateUrl="../contrib/workflow/pages/default.aspx" ResourceValue="A011"/>
    <bw:MenuListItem ID="MenuListItem2" runat="server" Text="已办事宜" NavigateUrl="../contrib/workflow/pages/doneTask.aspx" ResourceValue="A012" />
    <bw:MenuListItem ID="MenuListItem3" runat="server" Text="草稿箱" NavigateUrl="../contrib/workflow/pages/draft.aspx" ResourceValue="A009" />
    <bw:MenuListItem ID="MenuListItem4" runat="server" Text="流程部署" NavigateUrl="../contrib/workflow/pages/workflowDeploy.aspx" ResourceValue="A007" />
    <bw:MenuListItem ID="MenuListItem5" runat="server" Text="高级查询" NavigateUrl="../contrib/workflow/pages/search.aspx" ResourceValue="A010" />
</bw:LeftMenuView>
