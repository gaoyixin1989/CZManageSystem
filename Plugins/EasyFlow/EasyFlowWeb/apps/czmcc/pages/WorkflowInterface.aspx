<%@ Page Title="" Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_czmcc_pages_WorkflowInterface" Codebehind="WorkflowInterface.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>外部流程接入</span></h3>
         <div style="float: right; margin-right: 3px;">
            <input type="button" value="新增接入" onclick="window.location='workflowInterfaceEdit.aspx';" class="btnPassClass" />
        </div>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>外部流程接入列表</h4>
        </div>
         <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align: center;">
            <tr>
                <th style="width:30px">序号</th>
                <th style="width:100px">流程名称</th>
                <th>接入地址</th>
                <th style="width:50px">状态</th>
                <th style="width:120px">最近修改时间</th>
                <th style="width:100px; text-align:center;">操作</th>
            </tr>
            <asp:Repeater ID="rptList" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%# rowIndex++ %></td>
                        <td style="text-align:left;"><%# Eval("Name") %></td>
                        <td style="text-align:left;"><%# Eval("Url") %></td>
                        <td><%# FormatStatus(Eval("Status")) %></td>
                        <td><%# Eval("LastModTime", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                        <td>
                            <a href="WorkflowInterfaceEdit.aspx?id=<%# Eval("Id") %>" class="ico_edit">编辑</a>
                            <a href="WorkflowInterfaceEdit.aspx?action=delete&id=<%# Eval("Id") %>" class="ico_del" onclick="return confirm('确定要删除这个外部接入流程？');">删除</a>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trClass">
                        <td><%# rowIndex++ %></td>
                        <td style="text-align:left;"><%# Eval("Name") %></td>
                        <td style="text-align:left;"><%# Eval("Url") %></td>
                        <td><%# FormatStatus(Eval("Status")) %></td>
                        <td><%# Eval("LastModTime", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                        <td>
                            <a href="WorkflowInterfaceEdit.aspx?id=<%# Eval("Id") %>" class="ico_edit">编辑</a>
                            <a href="WorkflowInterfaceEdit.aspx?action=delete&id=<%# Eval("Id") %>" class="ico_del" onclick="return confirm('确定要删除这个外部接入流程？');">删除</a>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
           </asp:Repeater>
        </table>
    </div>
</asp:Content>