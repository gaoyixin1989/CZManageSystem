<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_security_pages_UserRoles" Title="分配角色" Codebehind="UserRoles.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span>分配角色</span></h3>
    </div>
    <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:12px;">
        <tr>
            <th width="20%" align="center">用 户：</th>
            <td width="80%"><%=this.realName%></td>
        </tr>
        <tr class="trClass">
            <th align="center">所属部门：</th>
            <td><%=this.departmentName%></td>
        </tr>
        <tr>
            <th style="text-align:right">已分配的角色列表：</th>
            <td>
                <asp:Literal ID="ltlRoleValues" runat="server" EnableViewState="false"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th align="center">选择角色：</th>
            <td>
                <div style="border-bottom:solid 1px #C0CEDF;padding:10px auto 10px 8px">
                    -  <input id="chkSelectAll1" name="chkSelectAll" type="checkbox" title="全选/全不选" /><label for="chkSelectAll">全选</label>
                    -  <a name="expandAll" href="javascript:void(0);" title="全部展开/收缩">全部展开/收缩</a>
                </div>
                <div class="list2">
                    <asp:Literal ID="ltlRoles" EnableViewState="false" runat="server"></asp:Literal>
                </div> 
                <div style="border-top:solid 1px #C0CEDF;padding:10px auto 10px 8px">
                    - <input id="chkSelectAll2" name="chkSelectAll" type="checkbox" title="全选/全不选" /><label for="chkSelectAll">全选</label>
                    -  <a name="expandAll" href="javascript:void(0);" title="全部展开/收缩">全部展开/收缩</a>
                </div>
            </td>
        </tr>
    </table>
    <p align="center" style="margin-top:6px">
        <asp:Button ID="btnUpdate" CssClass="btn_sav" Text="保存" runat="server" 
            onclick="btnUpdate_Click" />
        <input type="button" class="btn" value="返回" onclick="window.location='users.aspx';" />
    </p>
    <script type="text/javascript">
    <!--//
    $(function(){
        $("input[name='chkSelectAll']").click(function(){
            var isChecked = this.checked;
            // 选中复选框.
			$("input:checkbox").each(function(){
                this.checked = isChecked;
                if(isChecked){
                    $(this).next("label").addClass('spanFocus'); // 突出显示被选中
                }
                else{
                    $(this).next("label").removeClass('spanFocus');
                }
            });  
		});
		var isExpandAll = false;
		$("a[name='expandAll']").click(function(){
		     var isExpanded = false;
		     $("tr[name='roles_row']").each(function(){
		        if(isExpandAll){
		            $(this).css("display", "none");
		            isExpanded = false;
		        }
		        else{
		            $(this).css("display", "");
		            isExpanded = true;
		        }
		     });
		     isExpandAll = isExpanded;
		});
		
        // 加载时，设置角色被选中的样式

        $("input[name='roles_item']").each(function(){
            if(this.checked){
                $(this).next("label").addClass('spanFocus'); // 突出显示被选中
            }
        });
        // 选中角色时，设置被选中的样式

		$("input[name='roles_item']").click(function(){
            if(this.checked){
                $(this).next("label").addClass('spanFocus'); // 突出显示被选中
            }
            else{
                $(this).next("label").removeClass('spanFocus');
            }
		});
		$("div[name='roles_group']").click(function(){
		    var id = this.id;
		    $("tr[parentid='"+id+"']").each(function(){
		        if($(this).css("display") == "none"){
		            $(this).css("display", "");
		        }
		        else{
		            $(this).css("display", "none");
		        }
		    });
		});
    });
    //--> 
    </script>
</asp:Content>
