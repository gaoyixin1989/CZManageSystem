<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_Extension_WorkflowHelper" Codebehind="WorkflowHelper.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <link href="<%=AppPath%>res/js/jquery.editable-select.css"
        rel="stylesheet" type="text/css" />
    <script src="<%=AppPath%>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.js"
        type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.pack.js"
        type="text/javascript"></script>
 <div class="titleContent">
    <h3>
        <span>流程辅助功能</span>
    </h3>
</div>
 <div class="dataList">
    <div class="showControl">
        <h4>
            流程名称修改</h4>
        <button onclick="return showContent(this,'tb1');" title="收缩">
            <span>折叠</span></button>
    </div>
    <table id="tb1" class="tblGrayClass grayBackTable" style="word-break: break-all;" cellspacing="1" cellpadding="4" border="0">
            <tr>
                <th width="20%" align="right">
                    原流程名称：
                </th>
                <td width="80%">
                    <asp:DropDownList ID="ddlWorkflows" CssClass="editable-select" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    新流程名称：
                </th>
                <td width="80%">
                    <asp:TextBox ID="txtToWorkflowName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnModify" runat="server" OnClientClick="return beforeModify();"
                        OnClick="btnModify_Click" Text="确定修改" CssClass="btnClass2l" />
                </td>
            </tr>
        </table>
 </div>
<script language="javascript" type="text/javascript">
     function beforeModify(){
        var wname = $("#<%=ddlWorkflows.ClientID %>").val();
        var toname = $("#<%=txtToWorkflowName.ClientID %>").val();
        if(wname.length == 0){
            alert('请选择原流程名称');
            return false;
        }
        if(toname.length == 0){
            alert('请输入新流程名称');
            return false;
        }
        
        return confirm("您确定修改吗？");
    }
    $(document).ready(function () {
            $('#<%=ddlWorkflows.ClientID %>').editableSelect({
                onSelect: function (list_item) {
                    this.select.val(this.text.val());
                }
            })
            $(".editable-select-options").css("text-align","left");
        });
</script>
       
</asp:Content>