<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_security_pages_EditRole" Title="编辑角色" Codebehind="EditRole.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div style="padding:5px;">
        <div class="titleContent">
            <h3><span><%=this.CommandText%>角色</span></h3>
        </div>
        <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:10px;">
            <tr>
                <th width="15%" style="text-align:right">角色名称：</th>
                <td width="75%">
                    <asp:TextBox ID="textRoleName" CssClass="inputbox" Width="200px" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvRoleName" runat="server" ControlToValidate="textRoleName"
                        ErrorMessage="必须输入角色名称。"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr class="trClass">
                <th style="text-align:right">选择角色组：</th>
                <td>
                    <asp:DropDownList ID="ddlParentRoles" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr class="trClass">
                <th style="text-align:right">开始时间：</th>
                <td>
                    <bw:DateTimePicker ID="dtpBeginTime" runat="server" InputBoxCssClass="inputbox" IsRequired="true" SetFocusOnError="true" RequiredErrorMessage="必须输入开始时间." DateType="Datetime" />
                </td>
            </tr>
            <tr>
                <th style="text-align:right">截止时间：</th>
                <td>
                    <bw:DateTimePicker ID="dtpEndTime" runat="server" InputBoxCssClass="inputbox" IsRequired="true" SetFocusOnError="true" RequiredErrorMessage="必须输入截止时间." DateType="Datetime" />
                </td>
            </tr>
            <tr class="trClass">
                <th style="text-align:right">备　注：</th>
                <td>
                    <asp:TextBox ID="textComment" Columns="70" Rows="3" TextMode="MultiLine"
                        runat="server" /></td>
            </tr>
            <tr>
                <th style="text-align:right">已设置的权限列表：</th>
                <td>
                    <asp:Literal ID="ltlResourceValues" runat="server" EnableViewState="false"></asp:Literal>
                </td>
            </tr>
            <tr>
                <th style="text-align:right">权限设置：</th>
                <td>
                    <div class="dataList">
                        <div style="border-bottom:solid 1px #C0CEDF;padding:10px auto 10px 8px">
                            -  <a name="expandAll" href="javascript:void(0);" title="全部展开/收缩">全部展开/收缩</a>
                        </div>
                        <div id="sysDataDiv" class="list">
                            <asp:Literal ID="ltlSystemResources" runat="server" EnableViewState="false"></asp:Literal>
                        </div>
                        <div id="wfDataDiv" class="list">
                            <asp:Literal ID="ltlWorkflowResources" runat="server" EnableViewState="false"></asp:Literal>
                        </div>
                        <div style="border-top:solid 1px #C0CEDF;padding:10px auto 10px 8px">
                            -  <a name="expandAll" href="javascript:void(0);" title="全部展开/收缩">全部展开/收缩</a>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <p align="center" style="margin-top:10px">
            <asp:Button ID="btnEdit" runat="server" CssClass="btn_add" Text="添加" onclick="btnEdit_Click" />
        </p>
    </div>
    </div>
    <script type="text/javascript">
    <!--//
    $(function(){        
        // 加载时，设置角色被选中的样式

        $("input[name='res_item']").each(function(){
            if(this.checked){
                $(this).next("label").addClass('spanFocus'); // 突出显示被选中
                
            }
        });
        var isHidden = false;
        $("#hidAll").click(function(){
               if(isHidden){
                   $("#sysDataDiv").css("display", "");
                   $("#wfDataDiv").css("display", "");
                   isHidden = false;
               }
               else{
                   $("#sysDataDiv").css("display", "none");
                   $("#wfDataDiv").css("display", "none");
                   isHidden = true;
               }
        });
		var isExpandAll = false;
		$("a[name='expandAll']").click(function(){
		     var isExpanded = false;
		     $("tr[name='res_row']").each(function(){
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
        // 选中角色时，设置被选中的样式

		$("input[name='res_item']").click(function(){
            if(this.checked){
                $(this).next("label").addClass('spanFocus'); // 突出显示被选中
            }
            else{
                $(this).next("label").removeClass('spanFocus');
            }
		});
		$("div[name='res_group']").click(function(){
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
