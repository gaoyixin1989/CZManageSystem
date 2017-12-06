<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="ReportList" Codebehind="ReportList.aspx.cs" %>

<%@ Register Src="../controls/NavigationTools.ascx" TagName="NavigationTools" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <script type="text/javascript">
        function GotoEditPage(page,id)
        {
            var p = page == "1" ? "" : page;
            window.location.href = "ReportCreate" + p + ".aspx?id=" + id;
        }
    </script>
    <div id="divList" runat="server">
        <div class="titleContent">
            <uc1:NavigationTools ID="NavigationTools1" runat="server" />
        </div>
        <div class="dataList">
            <input type="button" onclick="window.location.href='ReportCreate.aspx'" value="新增报表"
                class="btnClass2m" />
            <input type="button" onclick="window.location.href='ReportConfig.aspx'" value="管理配置"
                class="btnClass2m" />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                CssClass="tblClass" DataKeyNames="Id" OnRowDeleting="GridView1_RowDeleting" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="报表名称" SortExpression="Name">
                        <ItemTemplate>
                            <a href='ReportView.aspx?id=<%# Eval("Id") %>'>
                                <%# Eval("Name")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Remark" HeaderText="备注" SortExpression="Remark" />
                    <asp:BoundField DataField="Extends" HeaderText="Extends" SortExpression="Extends"
                        Visible="false" />
                    <asp:BoundField DataField="Creator" HeaderText="创建人" SortExpression="Creator" Visible="false" />
                    <asp:BoundField DataField="CreatedTime" HeaderText="创建时间" SortExpression="CreatedTime"
                        DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href='TemplateConfig.aspx?id=<%# Eval("Id") %>'>编辑模板</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href='DrawGrapConfig.aspx?id=<%# Eval("Id") %>'>编辑图表</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href="javascript:GotoEditPage('<%# Eval("SourceType") %>','<%# Eval("Id") %>')">编辑列表</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowDeleteButton="True" DeleteText="删除" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
