<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="apps_xqp2_pages_workflows_controls_WorkflowBusinessStat" Codebehind="WorkflowBusinessStat.ascx.cs" %>
<link href="<%=AppPath%>res/js/jquery.editable-select.css"
    rel="stylesheet" type="text/css" />
<script src="<%=AppPath%>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="<%=AppPath%>res/js/jquery.editable-select.js"
    type="text/javascript"></script>
<script src="<%=AppPath%>res/js/jquery.editable-select.pack.js"
    type="text/javascript"></script>
<div id="divWorkflowBizStat1">
    <div class="toolBlock" style="border-bottom: solid 1px #C0CEDF; margin-bottom: 10px;
        padding-bottom: 5px;">
        流程名称：<asp:DropDownList ID="ddlWorkflowList" runat="server" AutoPostBack="false" CssClass="editable-select">
        </asp:DropDownList>
        &nbsp;&nbsp; 日期：从<bw:DateTimePicker ID="txtStartDT" runat="server" Width="80px" ValidatorDisplay="Dynamic"
            IsValidate="False" />
        到
        <bw:DateTimePicker ID="txtEndDT" runat="server" Width="80px" ValidatorDisplay="Dynamic"
            IsValidate="False" />
        是否完成：<asp:DropDownList ID="ddlState" runat="server">
            <asp:ListItem Value="" Text="- 请选择 -"></asp:ListItem>
            <asp:ListItem Value="2" Text="已完成"></asp:ListItem>
            <asp:ListItem Value="1" Text="流转中"></asp:ListItem>
            <asp:ListItem Value="99" Text="已取消"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp; <span style="display:none">发起人部门：
        <textarea ID="txtDepts" runat="server" readonly="readonly" style="font-size:8pt; width:180px; height:35px"></textarea>
        <input type='button' id="btnDepts" style='cursor: pointer; background: url(<%=AppPath%>App_Themes/gmcc/img/btnse01.jpg);
            border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;' />&nbsp;&nbsp;</span><asp:Button
                ID="btnSearch" runat="server" Text="搜索" CssClass="btn_query" OnClick="btnSearch_Click" />
    </div>
    <div style="overflow-x: auto; width: 100%">
        <asp:DataGrid ID="dataBusiness" runat="server" CssClass="tblGrayClass" OnItemDataBound="dataBusiness_ItemDataBound">
        </asp:DataGrid>
    </div>
    <div class="toolBlock" style="border-top: solid 1px #C0CEDF">
        <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
            Font-Size="9pt" ItemsPerPage="16" PagerStyle="NumericPages" BorderWidth="0px"
            OnPageIndexChanged="listPager_PageIndexChanged" />
    </div>
</div>
<script type="text/javascript" src="<%=AppPath %>res/js/HideFieldJs.js"></script>
<script type="text/javascript" src="<%=AppPath %>res/js/jquery_custom.js"></script>
<script type="text/javascript" src="<%=AppPath %>res/js/Frienddetail.js"></script>
<script type="text/javascript" src="<%=AppPath %>res/js/Base64.js"></script>
<script type="text/javascript" src="<%=AppPath %>res/js/jquery.showLoading.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('#<%=ddlWorkflowList.ClientID %>').editableSelect({
            onSelect: function (list_item) {
                this.select.val(this.text.val());
            }
        })
        $(".editable-select-options").css("text-align", "left");
        //if ($("#<%=ddlWorkflowList.ClientID %>").editableSelectInstances()[0])
        //$("#<%=ddlWorkflowList.ClientID %>").editableSelectInstances()[0].text[0].value = "<%=Request.QueryString["wname"]%>";
        $("#btnDepts").click(function () {
            showDiv({ isorganization: 'False', tableName: 'bw_depts', text: 'dpfullname', value: 'dpfullname', fieldWhere: '' }, '<%=AppPath%>apps/pms/pages/GetMarkData.aspx', { hide: '<%=txtDepts.ClientID%>', text: '<%=txtDepts.ClientID%>' });
            $("img[alt='点击可以关闭']").attr("src", "<%=AppPath %>App_Themes/gmcc/img/close_1.jpg")
        });
        $("#<%=btnSearch.ClientID %>").click(function () {
            $("body").showLoading();
        })
    });
</script>
