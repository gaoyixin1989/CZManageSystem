<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_RemindTime" Codebehind="RemindTime.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="TimeSpanSelector" Src="../controls/TimeSpanSelector.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>提醒时段设置</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>设置说明</h4>
            <button onclick="return showContent(this,'divComment');" title="收缩"><span>折叠</span></button>
        </div>
        <div id="divComment" style="display:none;">
            提醒时段可设置多个，比如上午8:30至12:00，下午2:00至5:30。如果有提醒时段设置，系统将只在设置的提醒时段内发送此流程的消息通知。紧急、重要单是例外情况，消息通知产生后立即发送，与提醒时段设置无关。

        </div>        
        <div class="showControl">
            <h4>时段设置</h4>
            <button onclick="return showContent(this,'divTimeSetting');" title="收缩"><span>折叠</span></button>
        </div>
        <div id="divTimeSetting">
            <div class="toolBlock" style="border-bottom:solid 1px #f4f4f4;">
                开始时间：<bw:TimeSpanSelector ID="inertSelectorBegin" runat="server" />
                结束时间：<bw:TimeSpanSelector ID="inserSelectorEnd" runat="server" />
                <asp:Button ID="btnInsert" runat="server" CssClass="btn_add" Text="新增" 
                    onclick="btnInsert_Click" />
                <input type="button" onclick="history.go(-1)" class="btnReturnClass" value="返回" />
            </div>
        
            <asp:GridView ID="gvRemindTimespans" DataKeyNames="TimeId" CellPadding="0" CellSpacing="0" 
                BorderWidth="0" RowStyle-HorizontalAlign="Center" 
                HeaderStyle-HorizontalAlign="Center" CssClass="tblClass" 
                AutoGenerateColumns="false" UseAccessibleHeader="true" runat="server" 
                onrowcancelingedit="gvRemindTimespans_RowCancelingEdit" 
                onrowdeleting="gvRemindTimespans_RowDeleting" 
                onrowediting="gvRemindTimespans_RowEditing" 
                onrowupdating="gvRemindTimespans_RowUpdating">
                <Columns>
                    <asp:BoundField DataField="TimeId" ReadOnly="true" Visible="false" HeaderText="编号" HeaderStyle-Width="10%"/>
                    <asp:BoundField DataField="WorkflowName" ReadOnly="true" HeaderText="流程名称" />
                    <asp:TemplateField HeaderText="开始时间">
                        <ItemTemplate><%# Eval("BeginTimeSpan", "{0:HH:mm:ss}")%></ItemTemplate>
                        <EditItemTemplate>
                            <bw:TimeSpanSelector ID="selectorBeign" Hours='<%# Eval("BeginHours") %>' Minutes='<%# Eval("BeginMinutes") %>' runat="server"/>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="结束时间">
                        <ItemTemplate><%# Eval("EndTimeSpan", "{0:HH:mm:ss}")%></ItemTemplate>
                        <EditItemTemplate>
                            <bw:TimeSpanSelector ID="selectorEnd" Hours='<%# Eval("EndHours") %>' Minutes='<%# Eval("EndMinutes") %>' runat="server"/>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField HeaderText="操作" EditText="&lt;span class=&quot;ico_edit&quot;&gt;编辑&lt;span&gt;"
                            UpdateText="保存" ShowEditButton="true" CancelText="取消" ShowDeleteButton="true" DeleteText="&lt;span class=&quot;ico_del&quot; onclick=&quot;return confirm('确定要删除时间段设置?');&quot;&gt;删除&lt;/spa&gt;"
                            HeaderStyle-Width="20%" />
                </Columns>
                <AlternatingRowStyle CssClass="trClass" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
