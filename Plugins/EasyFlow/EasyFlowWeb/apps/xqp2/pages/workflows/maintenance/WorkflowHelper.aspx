<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_maintenance_WorkflowHelper" EnableEventValidation="false" Codebehind="WorkflowHelper.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <link href="<%=AppPath%>res/js/jquery.editable-select.css"
        rel="stylesheet" type="text/css" />
    <script src="<%=AppPath%>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.js"
        type="text/javascript"></script>
    <script src="<%=AppPath%>res/js/jquery.editable-select.pack.js"
        type="text/javascript"></script>
    <asp:ScriptManager ID="scriptManager1" runat="server" />

    <div class="titleContent">
        <h3>
            <span>步骤名称修改</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>
                流程步骤名称修改</h4>
            <button onclick="return showContent(this,'tb4');" title="收缩">
                <span>折叠</span></button>
        </div>
        <table id="tb4" class="tblGrayClass grayBackTable" style="word-break: break-all;" cellspacing="1" cellpadding="4" border="0">
             <tr>
                <th width="20%" align="right">
                    流程名称：

                </th>
                <td width="80%">
                   <asp:DropDownList ID="ddlWorkflowList" runat="server" AutoPostBack="True" CssClass="editable-select" OnSelectedIndexChanged="ddlWorkflowList_SelectedIndexChanged">
                <asp:ListItem Value="" Text="－ 请选择 －"></asp:ListItem>
            </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    原步骤名称：
                </th>
                <td width="80%">
                    <asp:UpdatePanel ID="updatepanelActivities" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlActivityList" runat="server">
                                <asp:ListItem Value="" Text="－ 请选择 －"></asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlWorkflowList" EventName="selectedindexchanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    新步骤名称：
                </th>
                <td width="80%">
                    <asp:TextBox ID="txtToActivityName" runat="server"></asp:TextBox><span style="color:Red">*</span>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnModify2" runat="server" OnClientClick="return beforeModify2();"
                        OnClick="btnModify2_Click" Text="确定修改" CssClass="btnClass2l" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">       
        function beforeModify2(){
            var wname = document.getElementById("<%=ddlWorkflowList.ClientID %>").value;
            var aname = document.getElementById("<%=ddlActivityList.ClientID %>").selectedIndex;
            var toname = document.getElementById("<%=txtToActivityName.ClientID %>").value;

            if(wname.length == 0){
                alert('请选择流程名称');
                return false;
            }
            if(toname == 0){
                alert('请选择原步骤名称');
                return false;
            }
            if(toname.length == 0){
                alert('请输入新步骤名称');
                return false;
            }
            return confirm("您确定修改吗？");
        }      
        $(document).ready(function () {
            $('#<%=ddlWorkflowList.ClientID %>').editableSelect({
                onSelect: function (list_item) {
                    this.select.val(this.text.val());
                    //if (this.text.attr("id") == "datasource")
                       // getField(document.getElementById("datasource"));

                       setTimeout('__doPostBack(\''+"<%=ddlWorkflowList.ClientID %>".replace("_","$")+'\',\'\')', 0)
                }
            })
            $(".editable-select-options").css("text-align","left");
        });
    </script>

</asp:Content>
