<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="xqp2_contrib_workflow_pages_WorkflowIndex" Codebehind="WorkflowIndex.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="TaskList" Src="../../../../contrib/workflow/controls/WorkflowTaskList.ascx"%>
<%@ Register TagPrefix="bw" TagName="WorkflowStat" Src="../../../../contrib/workflow/controls/WorkflowStat.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" runat="server" /></span></h3>
        <div class="rightSite">
            <input type="button" value="发起工单" id="btnStart" onclick="onStartWorkflow()" class="big_btn" />
        </div>
    </div>    
    <div class="btnControl">
        <div class="btnRight">
            <input type="button" value="发起工单" id="btnStart" onclick="onStartWorkflow()" class="btnFW" />
        </div>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>流程归属</h4>
            <button onclick="return showContent(this,'Div1');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="Div1">
            <asp:Literal ID="ltlM" runat="server"></asp:Literal>
        </div>
        <div class="showControl">
            <h4>流程说明</h4>
            <button onclick="return showContent(this,'dataFlowComment');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataFlowComment">
            <asp:Literal ID="ltlRemark" runat="server"></asp:Literal>
        </div>
        <div class="showControl">
            <h4>流程图</h4>
            <button onclick="return showContent(this,'dataFlowGraph');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataFlowGraph">
            <a id="hrefWorkflowMap" href="<%=this.WorkflowImageUrl%>" target="_blank"><img id="imgWorkflowMap" alt="流程图" src="<%=this.WorkflowImageUrl%>&width=700" title="查看原图" /></a>
        </div>
       <div class="showControl">
            <h4>工单统计</h4>
            <button onclick="return onShowStat(this);" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataWorkflowStat" style="display:none;">
             <iframe id="frameWorkflowStat" style="width:100%; border:0" frameborder="0" onload='this.height=frameWorkflowStat.document.body.scrollHeight'></iframe>
        </div>
        
       <div class="showControl">
            <h4>未处理任务列表</h4>
            <button onclick="return showContent(this,'divTodoList');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="divTodoList">
            <bw:TaskList ID="TaskList1" EnableSearch="false" runat="server" />
        </div>

        <div class="showControl">
            <h4>已处理任务列表</h4>
            <button id="Button1" onclick="return onShowDoneTasks(this);" runat="server" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="dataDoneTask" style="display:none;">
             <iframe id="frameDoneTask" style="width:100%; border:0" frameborder="0" onload='this.height=frameDoneTask.document.body.scrollHeight'></iframe>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
    // <!CDATA[
    function onStartWorkflow(){
        location = "<%=WorkflowDirectory%>/start.aspx?wid=<%=this.WorkflowId%>";
    }
    var hasLoadedDoneTasks = false;
    function onShowDoneTasks(obj){
        if (!hasLoadedDoneTasks){
            $("#frameDoneTask").attr("src","<%=WorkflowDirectory%>/WorkflowDoneIndex.aspx?wname=<%=HttpUtility.UrlEncode(this.WorkflowName)%>");
            hasLoadedDoneTasks = true;
        }
        return showContent(obj,'dataDoneTask');
    }
    
    var hasLoadedStat = false;
    function onShowStat(obj){
        if (!hasLoadedStat){
            $("#frameWorkflowStat").attr("src","<%=WorkflowDirectory%>/stat/WorkflowStat.aspx?wname=<%=HttpUtility.UrlEncode(this.WorkflowName)%>");
            hasLoadedStat = true;
        }
        return showContent(obj,'dataWorkflowStat');
    }
    
    $("#btnStart").mouseover(function(){
        $(this).attr("class","big_btn_hover");
    });
    $("#btnStart").mouseout(function(){
        $(this).attr("class","big_btn");
    });
    // ]]>
    </script>
</asp:Content>
