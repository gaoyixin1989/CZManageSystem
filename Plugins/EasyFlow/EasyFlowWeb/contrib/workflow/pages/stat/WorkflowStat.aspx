<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_pages_stat_WorkflowStat" Codebehind="WorkflowStat.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="WorkflowStat" Src="../../controls/WorkflowStat.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>流程统计页</title>
</head>
<body style="background: none !important;">
    <form id="form1" runat="server">
    <div>
        <bw:WorkflowStat ID="workflowStat1" runat="server" />
    </div>
    </form>
</body>
</html>
