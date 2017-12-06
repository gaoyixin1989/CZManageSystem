<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/plugins/easyflow/masters/Default.master" Inherits="apps_xqp2_pages_System_SystemAccess" UICulture="auto" Codebehind="SystemAccess.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server" />
    <div class="titleContent">
        <h3>
            <span>推送接口设置</span></h3>
    </div>
    
    
    <div>
        <div class="searchBar">
            <div class="searchTitle">
                <h4>
                    推送接口设置</h4>
                <button onclick="return showSearch(this);" title="收缩">
                    <span>折叠</span></button>
            </div>
            <div class="searchInfo" id="searchBody">
                <table class="tblGrayClass grayBackTable" cellpadding="3" cellspacing="1">
                    <tr>
                        <th style="text-align: right; font-weight: normal;">
                                                        流程：
                        </th>
                        <td >
                            <asp:DropDownList ID="drdlWorkflow" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="drdlWorkflow_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <th style="text-align: right; font-weight: normal;" >
                                                        外部系统名：
                        </th>
                        <td>
                       
                            <asp:TextBox ID="txtSystemID" runat="server" Width="160px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 13%; text-align: right; font-weight: normal;">
                             Webservice地址：
                        </th>
                        <td style="width:37%">
                            <asp:TextBox ID="txtwebservice" runat="server" Width="358px"></asp:TextBox>
                        </td>
                        
                        <th style="text-align: right; font-weight: normal;">
                            
                        </th>
                        <td>
                            
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <th style="text-align: right; font-weight: normal;">步骤：</th>
                        <td colspan="3">
                        <%--<asp:UpdatePanel ID="updtGetContent" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>--%>
                            
                            <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                            </asp:CheckBoxList>
                           <%-- </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="drdlWorkflow" EventName="selectedindexchanged" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" colspan="4">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn_sav" Text="保存" 
                                onclick="btnSave_Click" />
                        </td>
                    </tr>
                    
                </table>
            </div>
            </div>
    </div>
    <script type="text/javascript" language="javascript">
    // <!CDATA[
        function check(obj){
            if(event.keyCode == 13 || event.keyCode == 46)
                 return true;
            if(event.keyCode < 48 || event.keyCode >57)
            {
                alert("只能输入0-9之间的数字！");
                return false;
            }
            else
                return true;
            }
         
         function checkSubmit()
         {
            if($("#ctl00_cphBody_drdlworkflowname").val() == "")
            {
                alert("请选择流程！");
                return false;
            }
            else if($("#ctl00_cphBody_txt_tablename").val() == "")
            {
                alert("请输入表名！");
                return false;
            }
            else if($("#ctl00_cphBody_txt_timmer").val() == "")
            {
                alert("请输入发单时间间隔！");
                return false;
            }
            else if($("#ctl00_cphBody_txt_datasource").val() == "")
            {
                alert("请输入数据源！");
                return false;
            }
            else
                return true;
         }
         function onToggleNotify(chkName, isChecked){
            var inputArray = document.getElementsByTagName("input");
		    for(var i=0; i<inputArray.length; i++) {
			    if (inputArray[i].type == "checkbox" && inputArray[i].name.indexOf(chkName) != -1) {
				    inputArray[i].checked = isChecked;
			    }
		    }
        }
        // ]]>
    </script>
</asp:Content>
