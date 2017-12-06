<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_config_WorkflowGroup" Codebehind="WorkflowGroup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>流程分组</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>
                分组管理</h4>
            <button onclick="return showContent(this,'dataDiv1');" title="收缩">
                <span>折叠</span></button>
        </div>        
        <div id="dataDiv1">
            <div class="dataTable" id="dataTable1">
                <fieldset>
                    <legend>新增分组</legend>
                    <div style="margin-bottom: 10px;">
                        分组名称：<asp:TextBox ID="txtGroupName" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;显示顺序：<asp:TextBox
                            ID="txtShowOrder" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;<span id="spGroupMsg" style="color:Red"></span>
                        <asp:Button ID="btnAddGroup" runat="server" Text="新增" class="btn_add" OnClick="btnAddGroup_Click" />
                    </div>
                </fieldset><br />
                <asp:GridView ID="gvMenuGroup" Width="100%" CssClass="tblClass" DataKeyNames="GroupID"
                    runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="gvMenuGroup_RowCancelingEdit"
                    OnRowEditing="gvMenuGroup_RowEditing" OnRowUpdating="gvMenuGroup_RowUpdating"
                    OnRowDeleting="gvMenuGroup_RowDeleting">
                    <Columns>                       
                        <asp:BoundField DataField="GroupName" HeaderText="分组名称">
                            <HeaderStyle Width="60%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ShowOrder" HeaderText="显示顺序">
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:CommandField HeaderText="操作" EditText="编辑" UpdateText="更新" ShowEditButton="true"
                            ShowDeleteButton="true" DeleteText="&lt;span id=&quot;del&quot; onclick=&quot;javascript:return confirm('确定删除吗？')&quot;&gt;删除&lt;/span&gt; "
                            CancelText="取消" HeaderStyle-Width="20%" />
                    </Columns>
                    <AlternatingRowStyle CssClass="trClass" />
                </asp:GridView>
            </div>
        </div>
        <div class="showControl">
            <h4>
                流程列表</h4>
            <button onclick="return showContent(this,'dataDiv2');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div id="dataDiv2" style="display:none">
            <div class="dataTable" id="Div2">
                <asp:GridView ID="gvFlowInMenuGroup" Width="100%" CssClass="tblClass" DataKeyNames="WorkflowName"
                    runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" OnRowDataBound="gvFlowInMenuGroup_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="50" DataField="WorkflowAlias" HeaderText="别名">
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="10%" DataField="WorkflowName" HeaderText="流程名称">
                            <HeaderStyle Width="50%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="所属分组" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlGroupName" runat="server" DataTextField="GroupName" DataValueField="GroupID">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="显示顺序" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <asp:TextBox ID="txtShowOrder" Width="20px" Text='<%# Eval("ShowOrder") %>' runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MenuGroupID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass="trClass" BorderWidth="0" />
                </asp:GridView>
            </div>
            <div align="center">
                <asp:Button ID="btnSave" runat="server" Text="保存" class="btnSaveClass" OnClick="btnSave_Click" />
                &nbsp;&nbsp;<input type="button" value="返回" class="btnReturnClass" onclick="history.go(-1);" /></div>
        </div>
    </div>
    <script language="javascript">
    $(function(){
        $("#<%=btnAddGroup.ClientID %>").click(function(){
            if($("#<%=txtGroupName.ClientID %>").get(0).value.length==0){
                $("#spGroupMsg").html(" *请输入分组区名称!");
                return false;
                }
            else
                $("#spGroupMsg").html("");
                
            if($("#<%= txtShowOrder.ClientID %>").get(0).value.length!=0)
            {
                var newPar=/^\d+$/;    
                if(!newPar.test($("#<%= txtShowOrder.ClientID %>").get(0).value)){
                    $("#<%=txtShowOrder.ClientID %>").get(0).value = "";
                    $("#spGroupMsg").html(" *显示顺序必须为数字!");
                    return false;                
                }
            }
        });
    });
    </script>
</asp:Content>
