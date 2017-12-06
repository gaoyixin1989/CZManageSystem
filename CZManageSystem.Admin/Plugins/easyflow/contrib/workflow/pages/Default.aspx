<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_Default" Codebehind="Default.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="TodoList" Src="../controls/TodoList.ascx" %>
<%--<%@ Register TagPrefix="bw" TagName="ReviewList" Src="../controls/ReviewList.ascx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">

    <div class="titleContent">
        <h3><span>待办事宜</span></h3>
    </div>
    
    <div class="dataList">
        <bw:TodoList ID="todoList1" runat="server" />
        
        <%--<bw:ReviewList ID="reviewList1" runat="server" />--%>
    </div>
</asp:Content>
