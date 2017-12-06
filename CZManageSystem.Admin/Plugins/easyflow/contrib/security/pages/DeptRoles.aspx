<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_security_pages_DeptRoles" Title="批量分配角色" Codebehind="DeptRoles.aspx.cs" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script type="text/C#" runat="server">
        /// <summary>
        /// 显示树的根部门编号.
        /// </summary>
        public static readonly string RootDeptId = "3297681318";
        /// <summary>
        /// 展开显示的部门编号.
        /// </summary>
        public static readonly string ExpandDeptId = "3297681318";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.BindRoles();
                this.BindDepartmentTree();
            }
        }
        
        /// <summary>
        /// 绑定部门树结构.
        /// </summary>
        private void BindDepartmentTree()
        {
            DataView source = Botwave.XQP.Commons.XQPHelper.GetAllDepartments().DefaultView;//departmentService.GetAllDepartments().DefaultView;
            TreeNode node = new TreeNode("广东移动潮州分公司");
            node.Value = RootDeptId;
            node.SelectAction = TreeNodeSelectAction.Expand;
            this.treeDepts.Nodes.Add(node);
            this.RecursiveDepartmets(source, RootDeptId, node.ChildNodes);  // 直接获取"广州移动通信公司"以下部门
        }

        private void RecursiveDepartmets(DataView sourceData, string parentId, TreeNodeCollection treeNodes)
        {
            DataView data = new DataView(sourceData.Table, string.Format("ParentDpId = '{0}'", parentId), "", DataViewRowState.CurrentRows);
            TreeNode node;
            int count = data.Count;
            for (int i = 0; i < count; i++)
            {
                int level = Botwave.Commons.DbUtils.ToInt32(data[i]["DpLevel"]);
                if (level > 6)
                    break;
                node = new TreeNode();
                if (level > 2)
                    node.Expanded = false;

                string dpId = Botwave.Commons.DbUtils.ToString(data[i]["DpId"]).Trim();
                if (dpId == ExpandDeptId)
                {
                    node.Expanded = true;
                }
                node.Text = Botwave.Commons.DbUtils.ToString(data[i]["DpName"]);
                node.Value = dpId;
                //node.NavigateUrl = string.Format("javascript:return onTreeNodeClick('{0}');", dpId);
                treeNodes.Add(node);
                this.RecursiveDepartmets(sourceData, dpId, node.ChildNodes);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
        <Services>
            <asp:ServiceReference InlineScript="true" Path="securityAjaxService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div class="titleContent">
        <h3><span>批量分配角色</span></h3>
    </div>
    <div class="dataList">
        <div class="dataTable" id="dataAssign">        
            <fieldset>
                <legend>
                    选择待分配的用户
                    - <asp:CheckBox ID="chkboxContainsLower" runat="server" Text="列出下级部门用户" />
                </legend>
                <table style="margin-top:6px;width:100%;">
                    <tr valign="top">
                        <td style="width:260px;">
                            <div class="borderLine" style="height:360px; width:260px; overflow:auto;">
                                <asp:TreeView ID="treeDepts" runat="server" ShowLines="True" 
                                    onselectednodechanged="treeDepts_SelectedNodeChanged">
                                </asp:TreeView>
                            </div>
                        </td>
                        <td>
                            <div class="borderLine" style="height:360px; width:100%; overflow:auto; overflow-x:hidden;">
                            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div id="divMessage" runat="server" visible="false" style="color:red;background-color:#FFFFCC; padding:5px;">
                                </div>
                                <table class="tblClass" cellpadding="0" cellspacing="0">
                                    <tr>
						                <th style="width:36px">选择</th>
						                <th style="width:50px">姓名</th>
						                <th>所属部门</th>
					                </tr>					                        
					                <asp:Repeater ID="rptUsers" runat="server">
					                    <ItemTemplate>
					                        <tr>
					                            <td>
					                                <input type="checkbox" id="chkboxUser" name="chkboxUser" value='<%# Eval("UserId") %>' />
					                            </td>
					                            <td><%# Eval("RealName") %></td>
					                            <td><%=this.treeDepts.SelectedNode.Text%></td>
					                        </tr>
					                    </ItemTemplate>
					                    <AlternatingItemTemplate>
					                        <tr class="trClass">
					                            <td>
					                                <input type="checkbox" id="chkboxUser" name="chkboxUser" value='<%# Eval("UserId") %>' />
					                            </td>
					                            <td><%# Eval("RealName") %></td>
					                            <td><%=this.treeDepts.SelectedNode.Text%></td>
					                        </tr>
					                    </AlternatingItemTemplate>
					                </asp:Repeater>
                                </table>                                
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="treeDepts" EventName="selectednodechanged" />
                            </Triggers>
                            </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>                    
                    <tr>
                        <td></td>
                        <td>
                            <input type="checkbox" onclick="onToggleSelect('chkboxUser', this.checked)" />全选 
                            - 选择被分配的角色：<asp:DropDownList ID="ddlRoles" runat="server"></asp:DropDownList>
                            <div>
                               <asp:RadioButtonList ID="rdiolistBatchType" RepeatDirection="Horizontal"  runat="server">
                                    <asp:ListItem Value="0" Selected="True">分配角色</asp:ListItem>
                                    <asp:ListItem Value="1">收回角色</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div>                                
                                <asp:RequiredFieldValidator ID="rfvRoles" runat="server" Display="Dynamic" ControlToValidate="ddlRoles" ErrorMessage="必须选择被分配或者被收回的角色."></asp:RequiredFieldValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="padding-top:6px">
                            - <asp:Button ID="btnBatchAssgin" CssClass="btnClass2m" runat="server" ForeColor="Blue" ToolTip="为当前选中的用户进行角色分配或者收回." Text="按选中分配" 
                                onclick="btnBatchAssgin_Click" /> 
                            - <input type="button" class="btnReturnClass" value="返回" />
                            <div style="margin-top:6px;">
                            - <asp:Button ID="btnBatchDepartment" CssClass="btnClass2m" runat="server" 
                                ToolTip="为当前选中部门的全部用户进行角色分配或者收回." ForeColor="Blue" Text="按部门分配" 
                                onclick="btnBatchDepartment_Click"/>
                            <asp:UpdatePanel ID="dpNameUpdatePanel" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>- <asp:Literal ID="ltlDpFullName" runat="server"><b style="color:red;">未选择部门.</b></asp:Literal></ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="treeDepts" EventName="selectednodechanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div><script language="javascript" type="text/javascript">
    // <!CDATA[
    function onToggleSelect(chkName, isChecked){
        var inputArray = document.getElementsByTagName("input");
		for(var i=0; i<inputArray.length; i++) {
			if (inputArray[i].type == "checkbox" && inputArray[i].name.indexOf(chkName) != -1) {
				inputArray[i].checked = isChecked;
			}
		}
    };
    // ]]>
    </script>
</asp:Content>
