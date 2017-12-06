<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_security_pages_PopupUserPicker" Title="选择用户" Codebehind="PopupUserPicker.aspx.cs" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
     <style type="text/css">
        .toolPaging 
        {
            border-top:solid 1px #C0CEDF;
            text-align: left !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="clear:both;margin-top:10px;padding-top:5px; border-top:solid 1px #ccc">
        <div style="width:26%; height:390px; float:left; overflow:auto; border-right:solid 1px #ccc">
            <asp:TreeView ID="treeDepts" runat="server" ShowLines="True" onselectednodechanged="treeDepts_SelectedNodeChanged">
            </asp:TreeView>
        </div>
        <div style="width:73%; height:390px; float:right">
            <div style="text-align:right; margin-bottom:10px">
                <asp:TextBox ID="txtKeyword" runat="server" CssClass="inputbox"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn_query" 
                    onclick="btnSearch_Click" />
            </div>
                    <div id="divMessage" runat="server" visible="false" style="color:red;background-color:#FFFFCC; padding:5px;">
                    </div>
                    <div>
                            <table style="width:100%; border:0px;" cellpadding="0" cellspacing="0">
                                <tr valign="top">
                                    <td>
                    <asp:UpdatePanel ID="updatePanel1" UpdateMode="Conditional" RenderMode="Block" runat="server">
                        <ContentTemplate>
                                    <table class="tblClass" cellpadding="0" cellspacing="0">
                                        <tr>
			                                <th style="width:20px"><input type="checkbox" id="selectAllUser" onclick="onSelectAll(this);" title="全选/全不选" /></th>
			                                <th style="width:150px">姓名</th>
			                                <th>用户名</th>
		                                </tr>					                        
		                                <asp:Repeater ID="rptUsers" runat="server">
		                                    <ItemTemplate>
		                                        <tr>
		                                            <td>
		                                                <input type="checkbox" id="chkboxUser" name="chkboxUser" value='<%# Eval("UserName") %>' title='<%# Eval("RealName") %>' userid='<%# Eval("UserId") %>' onclick="onCheckBoxChange(this);" />
		                                            </td>
		                                            <td><%# Eval("RealName") %></td>
		                                            <td><%# Eval("UserName") %></td>
		                                        </tr>
		                                    </ItemTemplate>
		                                    <AlternatingItemTemplate>
		                                        <tr class="trClass">
		                                            <td>
		                                                <input type="checkbox" id="chkboxUser" name="chkboxUser" value='<%# Eval("UserName") %>' title='<%# Eval("RealName") %>' userid='<%# Eval("UserId") %>' onclick="onCheckBoxChange(this);" />
		                                            </td>
		                                            <td><%# Eval("RealName") %></td>
		                                            <td><%# Eval("UserName") %></td>
		                                        </tr>
		                                    </AlternatingItemTemplate>
		                                </asp:Repeater>
                                    </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
                            <asp:AsyncPostBackTrigger ControlID="treeDepts" EventName="selectednodechanged" />
                            <asp:AsyncPostBackTrigger ControlID="listPager" EventName="PageIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                                    
                                    </td>
                                    <td style="width:150px; text-align:right; border:solid 1px #ccc;">
                                        <select id="selectedList" multiple="multiple" style="width:100%; height:230px">
                                        
                                        </select>
                                        <div style="padding:5px;">
                                            <a href="javascript:void(0);" onclick="return onListRemoveSelected();" class="ico_del" title="删除选中用户">删除</a>
                                            <a href="javascript:void(0);" onclick="return onListRemoveAll();" class="ico_index" title="清空全部用户">清空</a>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                    </div>
                    <asp:UpdatePanel ID="updatePanel2" UpdateMode="Conditional" ChildrenAsTriggers="true" RenderMode="Block" runat="server" >
                        <ContentTemplate>
                    <div class="toolBlock" style="border-top:solid 1px #C0CEDF">
                        <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                            Font-Size="9pt" ItemsPerPage="10" PagerStyle="NumericPages" BorderWidth="0px"
                            OnPageIndexChanged="listPager_PageIndexChanged" />
                    </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
                            <asp:AsyncPostBackTrigger ControlID="treeDepts" EventName="selectednodechanged" />
                        </Triggers>
                    </asp:UpdatePanel>
        </div>
        <script type="text/javascript">
          function onSelectAll(cid){
            if(cid.checked){
                $("input[name='chkboxUser']").attr("checked","checked");
                $("input[name='chkboxUser']").each(function(){
                    var value = $(this).val();
                    if($("#selectedList option[value='"+ value+"']").length == 0){
                        var name = $(this).attr("title");
                        var userid = $(this).attr("userid");
                        $("#selectedList").append("<option value='"+value+"' title='"+name+"' userid='"+userid+"'>"+name + "(" + value +")</option>");
                    }
                });
            }
            else{
                $("input[name='chkboxUser']").removeAttr("checked");
                $("input[name='chkboxUser']").each(function(){
                    var key = $(this).val();
                    $("#selectedList option").each(function(){
                        if($(this).val() == key){
                            $(this).remove();
                        }
                    });
                });
            }
          };
          
          function onCheckBoxChange(o){
            var value = $(o).val();
            if(o.checked){
                 if($("#selectedList option[value='"+ value+"']").length == 0){
                    var name = $(o).attr("title");
                    var userid = $(o).attr("userid");
                    $("#selectedList").append("<option value='"+value+"' title='"+name+"' userid='"+userid+"'>"+name + "(" + value +")</option>");
                }
            }else{
                $("#selectedList option").each(function(){
                    if($(this).val() == value){
                        $(this).remove();
                    }
                });
            }
          }
          
          function onListRemoveSelected(){
            $("#selectedList option:selected").each(function(){
                $(this).remove();
            });
            return true;
          }          
          function onListRemoveAll(){
            if(confirm("确定要清空已选择的全部用户？")){
                $("#selectedList").empty();
            }
            return true;
          }
        </script>
    </div>
    
    <div style="clear:both;text-align:center; margin-top:10px; padding-top:5px; border-top:solid 1px #ccc">
        <input type="button" value="确定" onclick="return onReturnValues();" class="btn_submit" />
    </div>
    <script type="text/javascript">
    <!--//
    function onReturnValues(){
        var id = "<%=this.OpenerInputId%>";
        if(id != ""){
            var openerInput = window.opener.document.getElementById("<%=this.OpenerInputId%>");
            var values = "";
            $("#selectedList option").each(function(){
                values += (","+ $(this).val());
            });
            if(openerInput.value ==""){
                values = values.substr(1, values.length - 1);
            }
		    openerInput.value += values;
        }
        window.parent.close();
        return true;
    }
    //-->
    </script>
</asp:Content>
