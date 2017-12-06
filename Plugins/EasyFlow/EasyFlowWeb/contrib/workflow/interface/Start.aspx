<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_interface_Start" Codebehind="Start.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="WorkflowInput" Src="../controls/WorkflowWorkitem.ascx" %>
<%@ Register TagPrefix="bw" TagName="ActivitySelector" Src="../controls/ActivitySelector.ascx" %>
<%@ Register TagPrefix="bw" TagName="ProcessHistory" Src="../controls/ProcessHistory.ascx" %>
<%@ Register TagPrefix="bw" TagName="WorkflowHistoryRelation" Src="../../../apps/czmcc/pages/WorkflowHistoryRelation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script type="text/javascript" src="../pages/script/dynamic-form.js"></script>
    
    <script type="text/javascript" src="../../../calendar/calendar_all.js"></script>
    <link type="text/css" rel="Stylesheet" href="../../../calendar/skins/aqua/theme.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="pageMaskLayout"></div>
    <div id="pageMaskContent">
        <span>正在处理，请稍候...</span>
    </div>
    <asp:ScriptManager ID="scriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="<%=AppPath%>contrib/workflow/pages/WorkflowAjaxService.asmx" />
            <asp:ServiceReference Path="<%=AppPath%>contrib/workflow/pages/GetDataAjaxService.asmx" />
        </Services>
    </asp:ScriptManager>
    <script type="text/javascript" src="../../../res/js/jquery-latest.pack.js"></script>
    <script type="text/javascript" src="../pages/script/tooltipAjax.js"></script>
    <script type="text/javascript" src="../pages/script/autofull.js"></script>
    <script type="text/javascript" src="../pages/script/ItemsExtensions.js"></script>
    
   <div class="titleContent">
        <h3><span>发起工单</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>摘要内容</h4>
        </div>
        <div class="dataTable" id="dataTable1">
            <bw:WorkflowInput ID="myWorkItemInput" runat="server" />     
        </div>
        <div class="showControl">
            <h4>
                详细信息</h4>
            <button onclick="return showContent(this,'dataContent2');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataContent2">
            <div id="divDynamicFormContainer" runat="server"></div>
        </div>
        <bw:WorkflowHistoryRelation ID="relationHistory1" runat="server" Editable="true" />
        <div class="showControl">
            <h4>
                附件信息</h4>
            <button onclick="return showContent(this,'divAttachmentContainer');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable" id="divAttachmentContainer">
            <iframe id="ifAttachment" src="<%=AppPath%>contrib/workflow/pages/WorkflowAttachment.aspx?wid=<%=WorkflowId %>&&wiid=<%=WorkflowAttachmentId %>" scrolling="no"
                frameborder="no" width="100%" onload="this.height=ifAttachment.document.body.scrollHeight">
            </iframe>
        </div>
        <div id="divWorkflowHistory" runat="server" visible="false">
            <div class="showControl">
                <h4>处理信息</h4>
                <button onclick="return showContent(this,'dataHistory');" title="收缩"><span>折叠</span></button>
            </div>
            <div class="dataTable" id="dataHistory">
                <bw:ProcessHistory ID="historyList" runat="server" />
            </div>
        </div>
        <bw:ActivitySelector ID="myActivtySelector" runat="server" />
        
        <div class="pageButtonList">
            <asp:Button ID="btnCreate" runat="server" CssClass="btn_add" OnClientClick="return beforeCreate();" Text="提交" OnClick="btnCreate_Click" />
            <asp:Button ID="btnSave" runat="server" CssClass="btn_sav" Text="保存" OnClientClick="return beforeSave();"  onclick="btnSave_Click" />
            &nbsp;&nbsp;<asp:Button ID="btnReturn" runat="server" CssClass="btnReturnClass" Text="返回" OnClick="btnReturn_Click" />
        </div>
    </div>
    <script language="javascript" type="text/javascript">
//        $(function() {
//        var FormDataSet =getFormDataSet("<%=WorkflowName %>", "<%=WorkflowAttachmentId %>",$("#ctl00_cphBody_myWorkItemInput_txtTitle").val(),"","<%=CurrentUserName %>","","","");
//        getDataSet("<%=WorkflowId %>", FormDataSet);
        //        });
        
    function beforeCreate(){
        if(onValidateReviewActors()==false)
            return false;
        var isValid = Validator.validate();
        if (isValid){
            var shouldCheckSelection = "<%=myActivtySelector.Visible %>";
            if (shouldCheckSelection.toLowerCase() == "true"){
                isValid = checkSelection();
            }
            if(isValid){
                showMaskLayout();
            }
        }        
        return isValid;
    }
            
    function beforeSave(){
//        if(Validator.validate()){ }
        showMaskLayout();
        return true;
    }

    var FormDataSet = getFormDataSet("<%=WorkflowName %>", "<%=WorkflowAttachmentId %>", $("#ctl00_cphBody_myWorkItemInput_txtTitle").val(), "", "<%=CurrentUserName %>", "<%=DpId %>", "", "");
    getDataSet("<%=WorkflowId %>", "<%=WorkflowAttachmentId %>", FormDataSet, 0, "<%=divDynamicFormContainer.ClientID %>");
    ItemIframe.LoadIframe("<%=WorkflowId %>", "<%=WorkflowAttachmentId %>", "<%=ActivityName %>", FormDataSet);
    ItemDataList.LoadDataList("<%=WorkflowId %>", "<%=WorkflowAttachmentId %>", FormDataSet, 0);
    //绑定下拉、单选、多选
    //setTimeout(function() {
        for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {
            //debugger;
            bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
        }
    //}, 1000);
    // div 覆盖层代码.
    function showMaskLayout() {
        $("#pageMaskLayout").css("display", "block");
        $("#pageMaskContent").css("display", "block");
        var bodyHeight = $("body").height();
        var bodyWidth = $("body").width();
        $("#pageMaskContent").css("top", bodyHeight - 200);
        $("#pageMaskContent").css("left", bodyWidth/2 - 100);
    }
    function closeMaskLayout() {
        $("#pageMaskLayout").css("display", "none");
        $("#pageMaskContent").css("display", "none");
    }

    $(document).ready(function{
        WorkflowExtension.GenerRules("<%=WorkflowId %>", "<%=WorkflowAttachmentId %>", "<%=ActivityName %>", "<%=CurrentUserName %>", FormDataSet) 
    });
    </script>
    <asp:Literal ID="ltlScripts" runat="server"></asp:Literal>
</asp:Content>
